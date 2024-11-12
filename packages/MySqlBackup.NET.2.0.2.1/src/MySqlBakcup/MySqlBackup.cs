using System;
using System.Timers;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Globalization;
using System.Security.Cryptography;

namespace MySql.Data.MySqlClient
{
    public class MySqlBackup : IDisposable
    {
        enum ProcessType
        {
            Export,
            Import
        }

        public enum ProcessEndType
        {
            UnknownStatus,
            Complete,
            Cancelled,
            Error
        }

        public const string Version = "2.0.2";

        MySqlDatabase _database = new MySqlDatabase();
        MySqlServer _server = new MySqlServer();

        Encoding utf8WithoutBOM;
        TextWriter textWriter;
        TextReader textReader;
        DateTime timeStart;
        DateTime timeEnd;
        ProcessType currentProcess;

        string sha512HashedPassword = "";
        ProcessEndType processCompletionType;
        bool stopProcess = false;
        Exception _lastError = null;

        string _currentTableName = "";
        long _totalRowsInCurrentTable = 0;
        long _totalRowsInAllTables = 0;
        long _currentRowIndexInCurrentTable = 0;
        long _currentRowIndexInAllTable = 0;
        int _totalTables = 0;
        int _currentTableIndex = 0;
        Timer timerReport = null;

        long _currentBytes = 0L;
        long _totalBytes = 0L;
        StringBuilder _sbImport = null;
        MySqlScript _mySqlScript = null;
        string _delimiter = "";
        bool _nameIsSet = false;
        bool _isNewDatabase = false;

        enum NextImportAction
        {
            Ignore,
            SetNames,
            CreateNewDatabase,
            AppendLine,
            ChangeDelimiter,
            AppendLineAndExecute
        }

        public Exception LastError { get { return _lastError; } }

        /// <summary>
        /// Gets the information about the connected database.
        /// </summary>
        public MySqlDatabase Database { get { return _database; } }
        /// <summary>
        /// Gets the information about the connected MySQL server.
        /// </summary>
        public MySqlServer Server { get { return _server; } }
        /// <summary>
        /// Gets or Sets the instance of MySqlCommand.
        /// </summary>
        public MySqlCommand Command { get; set; }

        public ExportInformations ExportInfo = new ExportInformations();
        public ImportInformations ImportInfo = new ImportInformations();

        public delegate void exportProgressChange(object sender, ExportProgressArgs e);
        public event exportProgressChange ExportProgressChanged;

        public delegate void exportComplete(object sender, ExportCompleteArgs e);
        public event exportComplete ExportCompleted;

        public delegate void importProgressChange(object sender, ImportProgressArgs e);
        public event importProgressChange ImportProgressChanged;

        public delegate void importComplete(object sender, ImportCompleteArgs e);
        public event importComplete ImportCompleted;

        public delegate void getTotalRowsProgressChange(object sender, GetTotalRowsArgs e);
        public event getTotalRowsProgressChange GetTotalRowsProgressChanged;

        public MySqlBackup()
        {
            InitializeComponents();
        }

        public MySqlBackup(MySqlCommand cmd)
        {
            Command = cmd;
            InitializeComponents();
        }

        void InitializeComponents()
        {
            _database.GetTotalRowsProgressChanged += _database_GetTotalRowsProgressChanged;
            
            timerReport = new Timer();
            timerReport.Elapsed += timerReport_Elapsed;

            utf8WithoutBOM = new UTF8Encoding(false);
        }

        void _database_GetTotalRowsProgressChanged(object sender, GetTotalRowsArgs e)
        {
            if (GetTotalRowsProgressChanged != null)
            {
                GetTotalRowsProgressChanged(this, e);
            }
        }

        #region Export

        /// <summary>
        /// Export the SQL dump and save as SQL dump file format. Don't run this method in Asynchronuous Mode (MultiThreading or BackgroundWorker), use ExportToTextWriterStream() instead. This is because if the user ABORT THE TASK while it is running, there is 5% chances that the file stream is not closed and resulting the file remains open.
        /// </summary>
        /// <param name="filePath"></param>
        public void ExportToFile(string filePath)
        {
            using (textWriter = new StreamWriter(filePath, false, utf8WithoutBOM))
            {
                ExportStart();
                textWriter.Close();
            }
        }

        /// <summary>
        /// Export the SQL dump into Memory Stream of TextWriter.
        /// </summary>
        /// <param name="textWriter"></param>
        public void ExportToTextWriter(TextWriter tw)
        {
            textWriter = tw;

            ExportStart();
        }

        void ExportStart()
        {
            try
            {
                int stage = 1;
                while (stage < 12)
                {
                    if (stopProcess) break;

                    switch (stage)
                    {
                        case 1: Export_InitializeVariables(); break;
                        case 2: Export_BasicInfo(); break;
                        case 3: Export_CreateDatabase(); break;
                        case 4: Export_DocumentHeader(); break;
                        case 5: Export_TableRows(); break;
                        case 6: Export_Functions(); break;
                        case 7: Export_Procedures(); break;
                        case 8: Export_Events(); break;
                        case 9: Export_Views(); break;
                        case 10: Export_Triggers(); break;
                        case 11: Export_DocumentFooter(); break;
                        default: break;
                    }

                    textWriter.Flush();

                    stage = stage + 1;
                }

                if (stopProcess) processCompletionType = ProcessEndType.Cancelled;
                else processCompletionType = ProcessEndType.Complete;
            }
            catch (Exception ex)
            {
                _lastError = ex;
                StopAllProcess();
                throw;
            }

            ReportEndProcess();
        }

        void Export_InitializeVariables()
        {
            timeStart = DateTime.Now;

            processCompletionType = ProcessEndType.UnknownStatus;
            currentProcess = ProcessType.Export;
            _lastError = null;
            timerReport.Interval = ExportInfo.IntervalForProgressReport;
            GetSHA512HashFromPassword(ExportInfo.EncryptionPassword);

            _database.GetDatabaseInfo(Command, ExportInfo.GetTotalRowsBeforeExport);
            _server.GetServerInfo(Command);
            _currentTableName = "";
            _totalRowsInCurrentTable = 0;
            _totalRowsInAllTables = _database.TotalRows;
            _currentRowIndexInCurrentTable = 0;
            _currentRowIndexInAllTable = 0;
            _totalTables = 0;
            _currentTableIndex = 0;

            try
            {
                Command.CommandText = "SET GLOBAL max_allowed_packet=1024*1024*1024;"; // 1GB. Maximum length of single query allow by MySQL
                Command.ExecuteNonQuery();
            }
            catch
            {
                // Purposely do nothing.
                // MySqlException will raise if user do not has the privilege to modify max_allowed_packet for this connection.
            }
        }

        void Export_BasicInfo()
        {
            Export_WriteComment(string.Format("MySqlBackup.NET {0}", MySqlBackup.Version));

            if (ExportInfo.RecordDumpTime)
                Export_WriteComment(string.Format("Dump Time: {0}", timeStart.ToString("yyyy-MM-dd HH:mm:ss")));
            else
                Export_WriteComment("");

            Export_WriteComment("--------------------------------------");
            Export_WriteComment(string.Format("Server version {0}", _server.Version));
            textWriter.WriteLine();
        }

        void Export_CreateDatabase()
        {
            if (!ExportInfo.AddCreateDatabase)
                return;

            Export_WriteComment("");
            Export_WriteComment(string.Format("Create schema {0}", _database.Name));
            Export_WriteComment("");
            textWriter.WriteLine();
            Export_WriteLine(_database.CreateDatabaseSQL);
            Export_WriteLine(string.Format("Use `{0}`;", _database.Name));
            textWriter.WriteLine();
            textWriter.WriteLine();
        }

        void Export_DocumentHeader()
        {
            textWriter.WriteLine();

            foreach (string s in _server.DocumentHeaders)
            {
                Export_WriteLine(s);
            }

            textWriter.WriteLine();
            textWriter.WriteLine();
        }

        void Export_TableRows()
        {
            Dictionary<string, string> dicTables = Export_GetTablesToBeExported();

            _totalTables = dicTables.Count;

            if (ExportInfo.ExportTableStructure || ExportInfo.ExportRows)
            {
                if (ExportProgressChanged != null)
                    timerReport.Start();

                foreach (KeyValuePair<string, string> kvTable in dicTables)
                {
                    if (stopProcess)
                        return;

                    string tableName = kvTable.Key;
                    string selectSQL = kvTable.Value;

                    _currentTableName = tableName;
                    _currentTableIndex = _currentTableIndex + 1;
                    _totalRowsInCurrentTable = _database.Tables[tableName].TotalRows;

                    if (ExportInfo.ExportTableStructure)
                        Export_TableStructure(tableName);

                    if (ExportInfo.ExportRows)
                        Export_Rows(tableName, selectSQL);
                }
            }
            
        }

        void Export_TableStructure(string tableName)
        {
            if (stopProcess)
                return;

            Export_WriteComment("");
            Export_WriteComment(string.Format("Definition of {0}", tableName));
            Export_WriteComment("");

            textWriter.WriteLine();

            Export_WriteLine(string.Format("DROP TABLE IF EXISTS `{0}`;", tableName));

            if (ExportInfo.ResetAutoIncrement)
                Export_WriteLine(_database.Tables[tableName].CreateTableSqlWithoutAutoIncrement);
            else
                Export_WriteLine(_database.Tables[tableName].CreateTableSql);

            textWriter.WriteLine();

            textWriter.Flush();
        }

        Dictionary<string, string> Export_GetTablesToBeExported()
        {
            var dic = new Dictionary<string, string>();

            if (ExportInfo.TablesToBeExportedDic == null || ExportInfo.TablesToBeExportedDic.Count == 0)
            {
                foreach (MySqlTable table in _database.Tables)
                {
                    dic[table.Name] = string.Format("SELECT * FROM `{0}`;", table.Name);
                }
            }
            else
            {
                foreach (KeyValuePair<string, string> kv in ExportInfo.TablesToBeExportedDic)
                {
                    dic[kv.Key] = kv.Value;
                }
            }

            return dic;
        }

        void Export_Rows(string tableName, string selectSQL)
        {
            Export_WriteComment("");
            Export_WriteComment(string.Format("Dumping data for table {0}", tableName));
            Export_WriteComment("");
            textWriter.WriteLine();
            Export_WriteLine(string.Format("/*!40000 ALTER TABLE `{0}` DISABLE KEYS */;", tableName));

            Export_RowsData(tableName, selectSQL);

            Export_WriteLine(string.Format("/*!40000 ALTER TABLE `{0}` ENABLE KEYS */;", tableName));
            textWriter.WriteLine();
        }

        void Export_RowsData(string tableName, string selectSQL)
        {
            MySqlTable table = _database.Tables[tableName];

            _currentRowIndexInCurrentTable = 0L;

            Command.CommandText = selectSQL;
            MySqlDataReader rdr = Command.ExecuteReader();

            string insertStatementHeader = null;

            var sb = new StringBuilder((int)ExportInfo.MaxSqlLength);

            while (rdr.Read())
            {
                if (stopProcess)
                    return;

                if (insertStatementHeader == null)
                {
                    insertStatementHeader = Export_GetInsertStatementHeader(tableName, rdr);
                }

                _currentRowIndexInAllTable = _currentRowIndexInAllTable + 1;
                _currentRowIndexInCurrentTable = _currentRowIndexInCurrentTable + 1;

                string sqlDataRow = Export_GetValueString(rdr, table);

                if (sb.Length == 0)
                {
                    sb.AppendLine(insertStatementHeader);
                    sb.Append(sqlDataRow);
                }
                else if ((long)sb.Length + (long)sqlDataRow.Length < ExportInfo.MaxSqlLength)
                {
                    sb.AppendLine(",");
                    sb.Append(sqlDataRow);
                }
                else
                {
                    sb.AppendFormat(";");

                    Export_WriteLine(sb.ToString());
                    textWriter.Flush();

                    sb = new StringBuilder((int)ExportInfo.MaxSqlLength);
                    sb.AppendLine(insertStatementHeader);
                    sb.Append(sqlDataRow);
                }
            }

            rdr.Close();

            if (sb.Length > 0)
            {
                sb.Append(";");
            }

            Export_WriteLine(sb.ToString());
            textWriter.Flush();

            sb = null;
        }

        private string Export_GetInsertStatementHeader(string tableName, MySqlDataReader rdr)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO `");
            sb.Append(tableName);
            sb.Append("`(");

            for (int i = 0; i < rdr.FieldCount; i++)
            {
                if (i > 0)
                    sb.Append(",");
                sb.Append("`");
                sb.Append(rdr.GetName(i));
                sb.Append("`");
            }

            sb.Append(") VALUES");
            return sb.ToString();
        }

        private string Export_GetValueString(MySqlDataReader rdr, MySqlTable table)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < rdr.FieldCount; i++)
            {
                if (sb.Length == 0)
                    sb.AppendFormat("(");
                else
                    sb.AppendFormat(",");

                string columnName = rdr.GetName(i);
                object ob = rdr[i];
                string mySqlDataType = table.Columns[columnName].MySqlDataType;

                sb.Append(QueryExpress.ConvertToSqlFormat(ob, true, true, mySqlDataType));
            }

            sb.AppendFormat(")");
            return sb.ToString();
        }

        void Export_Procedures()
        {
            if (!ExportInfo.ExportProcedures || _database.Procedures.Count == 0)
                return;

            Export_WriteComment("");
            Export_WriteComment("Dumping procedures");
            Export_WriteComment("");
            textWriter.WriteLine();

            foreach (MySqlProcedure procedure in _database.Procedures)
            {
                if (stopProcess)
                    return;

                Export_WriteLine(string.Format("DROP PROCEDURE IF EXISTS `{0}`;", procedure.Name));
                Export_WriteLine("DELIMITER " + ExportInfo.ScriptsDelimiter);

                if (ExportInfo.ExportRoutinesWithoutDefiner)
                    Export_WriteLine(procedure.CreateProcedureSQLWithoutDefiner + " " + ExportInfo.ScriptsDelimiter);
                else
                    Export_WriteLine(procedure.CreateProcedureSQL + " " + ExportInfo.ScriptsDelimiter);

                Export_WriteLine("DELIMITER ;");
                textWriter.WriteLine();
            }
            textWriter.Flush();
        }

        void Export_Functions()
        {
            if (!ExportInfo.ExportFunctions || _database.Functions.Count == 0)
                return;

            Export_WriteComment("");
            Export_WriteComment("Dumping functions");
            Export_WriteComment("");
            textWriter.WriteLine();

            foreach (MySqlFunction function in _database.Functions)
            {
                if (stopProcess)
                    return;

                Export_WriteLine(string.Format("DROP FUNCTION IF EXISTS `{0}`;", function.Name));
                Export_WriteLine("DELIMITER " + ExportInfo.ScriptsDelimiter);

                if (ExportInfo.ExportRoutinesWithoutDefiner)
                    Export_WriteLine(function.CreateFunctionSQLWithoutDefiner + " " + ExportInfo.ScriptsDelimiter);
                else
                    Export_WriteLine(function.CreateFunctionSQL + " " + ExportInfo.ScriptsDelimiter);

                Export_WriteLine("DELIMITER ;");
                textWriter.WriteLine();
            }

            textWriter.Flush();
        }

        void Export_Views()
        {
            if (!ExportInfo.ExportViews || _database.Views.Count == 0)
                return;

            Export_WriteComment("");
            Export_WriteComment("Dumping views");
            Export_WriteComment("");
            textWriter.WriteLine();

            foreach (MySqlView view in _database.Views)
            {
                if (stopProcess)
                    return;

                Export_WriteLine(string.Format("DROP TABLE IF EXISTS `{0}`;", view.Name));
                Export_WriteLine(string.Format("DROP VIEW IF EXISTS `{0}`;", view.Name));

                if (ExportInfo.ExportRoutinesWithoutDefiner)
                    Export_WriteLine(view.CreateViewSQLWithoutDefiner);
                else
                    Export_WriteLine(view.CreateViewSQL);

                textWriter.WriteLine();
            }

            textWriter.WriteLine();
            textWriter.Flush();
        }

        void Export_Events()
        {
            if (!ExportInfo.ExportEvents || _database.Events.Count == 0)
                return;

            Export_WriteComment("");
            Export_WriteComment("Dumping events");
            Export_WriteComment("");
            textWriter.WriteLine();

            foreach (MySqlEvent e in _database.Events)
            {
                if (stopProcess)
                    return;

                Export_WriteLine(string.Format("DROP EVENT IF EXISTS `{0}`;", e.Name));
                Export_WriteLine("DELIMITER " + ExportInfo.ScriptsDelimiter);

                if (ExportInfo.ExportRoutinesWithoutDefiner)
                    Export_WriteLine(e.CreateEventSqlWithoutDefiner + " " + ExportInfo.ScriptsDelimiter);
                else
                    Export_WriteLine(e.CreateEventSql + " " + ExportInfo.ScriptsDelimiter);

                Export_WriteLine("DELIMITER ;");
                textWriter.WriteLine();
            }

            textWriter.Flush();
        }

        void Export_Triggers()
        {
            if (!ExportInfo.ExportTriggers || _database.Triggers.Count == 0)
                return;

            Export_WriteComment("");
            Export_WriteComment("Dumping triggers");
            Export_WriteComment("");
            textWriter.WriteLine();

            foreach (MySqlTrigger trigger in _database.Triggers)
            {
                if (stopProcess)
                    return;

                Export_WriteLine(string.Format("DROP TRIGGER /*!50030 IF EXISTS */ `{0}`;", trigger.Name));
                Export_WriteLine("DELIMITER " + ExportInfo.ScriptsDelimiter);

                if (ExportInfo.ExportRoutinesWithoutDefiner)
                    Export_WriteLine(trigger.CreateTriggerSQLWithoutDefiner + " " + ExportInfo.ScriptsDelimiter);
                else
                    Export_WriteLine(trigger.CreateTriggerSQL + " " + ExportInfo.ScriptsDelimiter);

                Export_WriteLine("DELIMITER ;");
                textWriter.WriteLine();
            }

            textWriter.Flush();
        }

        void Export_DocumentFooter()
        {
            textWriter.WriteLine();

            foreach (string s in _server.DocumentFooters)
            {
                Export_WriteLine(s);
            }

            timeEnd = DateTime.Now;

            if (ExportInfo.RecordDumpTime)
            {
                TimeSpan ts = timeEnd - timeStart;

                textWriter.WriteLine();
                textWriter.WriteLine();
                Export_WriteComment(string.Format("Dump completed on {0}", timeEnd.ToString("yyyy-MM-dd HH:mm:ss")));
                Export_WriteComment(string.Format("Total time: {0}:{1}:{2}:{3}:{4} (d:h:m:s:ms)", ts.Days, ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds));
            }

            textWriter.Flush();
        }

        void Export_WriteComment(string text)
        {
            Export_WriteLine(string.Format("-- {0}", text));
        }

        void Export_WriteLine(string text)
        {
            if (ExportInfo.EnableEncryption)
            {
                textWriter.WriteLine(Encrypt(text));
            }
            else
            {
                textWriter.WriteLine(text);
            }
        }

        #endregion

        #region Import

        public void ImportFromFile(string filePath)
        {
            System.IO.FileInfo fi = new FileInfo(filePath);

            using (TextReader tr = new StreamReader(filePath))
            {
                ImportFromTextReaderStream(tr, fi);
            }
        }

        public void ImportFromTextReader(TextReader tr)
        {
            ImportFromTextReaderStream(tr, null);
        }

        void ImportFromTextReaderStream(TextReader tr, FileInfo fileInfo)
        {
            Import_InitializeVariables();

            if (fileInfo != null)
                _totalBytes = fileInfo.Length;
            else
                _totalBytes = 0L;

            textReader = tr;

            string line = "";

            while (line != null)
            {
                if (stopProcess)
                {
                    processCompletionType = ProcessEndType.Cancelled;
                    break;
                }

                try
                {
                    line = Import_GetLine();

                    Import_ProcessLine(line);
                }
                catch (Exception ex)
                {
                    
                    _lastError = ex;
                    if (ImportInfo.IgnoreSqlError)
                    {
                        if (!string.IsNullOrEmpty(ImportInfo.ErrorLogFile))
                        {
                            File.AppendAllText(ImportInfo.ErrorLogFile, Environment.NewLine + Environment.NewLine + ex.ToString());
                        }
                    }
                    else
                    {
                        StopAllProcess();
                        throw;
                    }
                }
            }

            ReportEndProcess();
        }

        string Import_GetLine()
        {
            string line = textReader.ReadLine();

            if (line == null)
                return null;

            if (ImportProgressChanged != null)
            {
                _currentBytes = _currentBytes + (long)line.Length;
            }

            if (Import_IsEmptyLine(line))
            {
                return string.Empty;
            }

            line = line.Trim();

            if (!ImportInfo.EnableEncryption)
                return line;

            line = Decrypt(line);

            return line.Trim();
        }

        void Import_InitializeVariables()
        {
            GetSHA512HashFromPassword(ImportInfo.EncryptionPassword);
            _lastError = null;
            timeStart = DateTime.Now;
            _currentBytes = 0L;
            _sbImport = new StringBuilder();
            _mySqlScript = new MySqlScript(Command.Connection);
            currentProcess = ProcessType.Import;
            processCompletionType = ProcessEndType.Complete;
            _delimiter = ";";

            if (ImportProgressChanged != null)
                timerReport.Start();

            if (ImportInfo.TargetDatabase.Length > 0)
            {
                Import_CreateNewDatabase();
            }

            // Modifying max_allowed_packet for this connection.
            try
            {
                Command.CommandText = "SET GLOBAL max_allowed_packet=1024*1024*1024;"; // 1GB. Maximum length of single query allow by MySQL
                Command.ExecuteNonQuery();
            }
            catch
            {
                // Purposely do nothing.
                // MySqlException will raise if user do not has the privilege to modify max_allowed_packet for this connection.
            }
        }

        void Import_ProcessLine(string line)
        {
            NextImportAction nextAction = Import_AnalyseNextAction(line);

            switch (nextAction)
            {
                case NextImportAction.Ignore:
                    break;
                case NextImportAction.SetNames:
                    Import_SetNames();
                    break;
                case NextImportAction.AppendLine:
                    Import_AppendLine(line);
                    break;
                case NextImportAction.ChangeDelimiter:
                    Import_ChangeDelimiter(line);
                    break;
                case NextImportAction.AppendLineAndExecute:
                    Import_AppendLineAndExecute(line);
                    break;
                default:
                    break;
            }
        }

        NextImportAction Import_AnalyseNextAction(string line)
        {
            if (Import_IsEmptyLine(line))
                return NextImportAction.Ignore;

            if (_isNewDatabase)
            {
                if (line.StartsWith("CREATE DATABASE ", StringComparison.OrdinalIgnoreCase))
                    return NextImportAction.Ignore;
                if (line.StartsWith("USE ", StringComparison.OrdinalIgnoreCase))
                    return NextImportAction.Ignore;
            }

            if (_nameIsSet)
            {
                if (line.StartsWith("/*!40101 SET NAMES ", StringComparison.OrdinalIgnoreCase) || line.StartsWith("SET NAMES ", StringComparison.OrdinalIgnoreCase))
                {
                    return NextImportAction.Ignore;
                }
                if (line.StartsWith("/*!40101 SET CHARACTER_SET_CLIENT", StringComparison.OrdinalIgnoreCase) || line.StartsWith("SET CHARACTER_SET_CLIENT", StringComparison.OrdinalIgnoreCase))
                {
                    return NextImportAction.Ignore;
                }
            }
            else
            {
                if (line.StartsWith("/*!40101 SET NAMES ", StringComparison.OrdinalIgnoreCase) || line.StartsWith("SET NAMES ", StringComparison.OrdinalIgnoreCase))
                {
                    return NextImportAction.SetNames;
                }
            }

            if (line.EndsWith(_delimiter))
                return NextImportAction.AppendLineAndExecute;

            if (line.StartsWith("DELIMITER ", StringComparison.OrdinalIgnoreCase))
                return NextImportAction.ChangeDelimiter;

            return NextImportAction.AppendLine;
        }

        void Import_CreateNewDatabase()
        {
            if (ImportInfo.DatabaseDefaultCharSet.Length == 0)
                Command.CommandText = string.Format("CREATE DATABASE IF NOT EXISTS `{0}`;", ImportInfo.TargetDatabase);
            else
                Command.CommandText = string.Format("CREATE DATABASE IF NOT EXISTS `{0}` /*!40100 DEFAULT CHARACTER SET {1} */;", ImportInfo.TargetDatabase, ImportInfo.DatabaseDefaultCharSet);

            Command.ExecuteNonQuery();

            Command.CommandText = string.Format("USE `{0}`;", ImportInfo.TargetDatabase);
            Command.ExecuteNonQuery();

            Import_SetNames();

            _isNewDatabase = true;
            _nameIsSet = true;
        }

        void Import_SetNames()
        {
            string setname = QueryExpress.ExecuteScalarStr(Command, "SHOW VARIABLES LIKE 'character_set_database';", 1);
            Command.CommandText = string.Format("/*!40101 SET NAMES {0} */;", setname);
            Command.ExecuteNonQuery();
            _nameIsSet = true;
        }

        void Import_AppendLine(string line)
        {
            _sbImport.AppendLine(line);
        }

        void Import_ChangeDelimiter(string line)
        {
            string nextDelimiter = line.Substring(9);
            _delimiter = nextDelimiter.Replace(" ", string.Empty);
        }

        void Import_AppendLineAndExecute(string line)
        {
            _sbImport.AppendLine(line);
            if (!line.EndsWith(_delimiter))
                return;

            _mySqlScript.Query = _sbImport.ToString();
            _mySqlScript.Delimiter = _delimiter;
            _mySqlScript.Execute();
            _sbImport = new StringBuilder();

        }

        bool Import_IsEmptyLine(string line)
        {
            if (line == null)
                return true;
            if (line == string.Empty)
                return true;
            if (line.Trim().Length == 0)
                return true;
            if (line.StartsWith("--"))
                return true;
            if (line == Environment.NewLine)
                return true;
            if (line == "\r")
                return true;
            if (line == "\n")
                return true;

            return false;
        }

        #endregion

        #region Encryption

        void GetSHA512HashFromPassword(string password)
        {
            sha512HashedPassword = CryptoExpress.Sha512Hash(password);
        }

        string Encrypt(string text)
        {
            return CryptoExpress.AES_Encrypt(text, sha512HashedPassword);
        }

        string Decrypt(string text)
        {
            return CryptoExpress.AES_Decrypt(text, sha512HashedPassword);
        }

        public void EncryptDumpFile(string sourceFile, string outputFile, string password)
        {
            using (TextReader trSource = new StreamReader(sourceFile))
            {
                using (TextWriter twOutput = new StreamWriter(outputFile, false, utf8WithoutBOM))
                {
                    EncryptDumpFile(trSource, twOutput, password);
                    twOutput.Close();
                }
                trSource.Close();
            }
        }

        public void EncryptDumpFile(TextReader trSource, TextWriter twOutput, string password)
        {
            GetSHA512HashFromPassword(password);

            string line = "";

            while (line != null)
            {
                line = trSource.ReadLine();

                if (line == null)
                    break;

                line = Encrypt(line);

                twOutput.WriteLine(line);
                twOutput.Flush();
            }
        }

        public void DecryptDumpFile(string sourceFile, string outputFile, string password)
        {
            using (TextReader trSource = new StreamReader(sourceFile))
            {
                using (TextWriter twOutput = new StreamWriter(outputFile, false, utf8WithoutBOM))
                {
                    DecryptDumpFile(trSource, twOutput, password);
                    twOutput.Close();
                }
                trSource.Close();
            }
        }

        public void DecryptDumpFile(TextReader trSource, TextWriter twOutput, string password)
        {
            GetSHA512HashFromPassword(password);

            string line = "";

            while (line != null)
            {
                line = trSource.ReadLine();

                if (line == null)
                    break;

                if (line.Trim().Length == 0)
                {
                    twOutput.WriteLine();
                }

                line = Decrypt(line);

                twOutput.WriteLine(line);
                twOutput.Flush();
            }
        }

        #endregion

        void ReportEndProcess()
        {
            timeEnd = DateTime.Now;

            StopAllProcess();

            if (currentProcess == ProcessType.Export)
            {
                ReportProgress();
                if (ExportCompleted != null)
                {
                    ExportCompleteArgs arg = new ExportCompleteArgs(timeStart, timeEnd, processCompletionType, _lastError);
                    ExportCompleted(this, arg);
                }
            }
            else if (currentProcess == ProcessType.Import)
            {
                _currentBytes = _totalBytes;

                ReportProgress();
                if (ImportCompleted != null)
                {
                    ImportCompleteArgs arg = new ImportCompleteArgs();
                    ImportCompleted(this, arg);
                }
            }
        }

        void timerReport_Elapsed(object sender, ElapsedEventArgs e)
        {
            ReportProgress();
        }

        void ReportProgress()
        {
            if (currentProcess == ProcessType.Export)
            {
                if (ExportProgressChanged != null)
                {
                    ExportProgressArgs arg = new ExportProgressArgs(_currentTableName, _totalRowsInCurrentTable, _totalRowsInAllTables, _currentRowIndexInCurrentTable, _currentRowIndexInAllTable, _totalTables, _currentTableIndex);
                    ExportProgressChanged(this, arg);
                }
            }
            else if (currentProcess == ProcessType.Import)
            {
                if (ImportProgressChanged != null)
                {
                    ImportProgressArgs arg = new ImportProgressArgs(_currentBytes, _totalBytes);
                    ImportProgressChanged(this, arg);
                }
            }
        }

        public void StopAllProcess()
        {
            stopProcess = true;
            timerReport.Stop();
        }

        public void Dispose()
        {
            try
            {
                _database.Dispose();
            }
            catch { }

            try
            {
                _server = null;
            }
            catch { }

            try
            {
                _mySqlScript = null;
            }
            catch { }
        }
    }
}