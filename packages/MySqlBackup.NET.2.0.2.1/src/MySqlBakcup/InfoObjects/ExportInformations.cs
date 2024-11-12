using System;
using System.Collections.Generic;
using System.Text;

namespace MySql.Data.MySqlClient
{
    /// <summary>
    /// Informations and Settings of MySQL Database Export Process
    /// </summary>
    public class ExportInformations
    {
        int _interval = 50;
        string _delimiter = "|";

        Dictionary<string, string> _customTable = new Dictionary<string, string>();

        /// <summary>
        /// Gets or Sets the list of tables that will be exported. If none, all tables will be exported.
        /// </summary>
        public List<string> TablesToBeExportedList
        {
            get
            {
                List<string> lst = new List<string>();
                foreach (KeyValuePair<string, string> kv in _customTable)
                {
                    lst.Add(kv.Key);
                }
                return lst;
            }
            set
            {
                _customTable.Clear();
                foreach (string s in value)
                {
                    _customTable[s] = string.Format("SELECT * FROM `{0}`;", s);
                }
            }
        }

        /// <summary>
        /// Gets or Sets the tables that will be exported with custom SELECT defined. If none or empty, all tables and rows will be exported. Key = Table's Name. Value = Custom SELECT Statement. Example 1: SELECT * FROM `product` WHERE `category` = 1; Example 2: SELECT `name`,`description` FROM `product`;
        /// </summary>
        public Dictionary<string, string> TablesToBeExportedDic
        {
            get
            {
                return _customTable;
            }
            set
            {
                _customTable = value;
            }
        }

        /// <summary>
        /// Gets or Sets a value indicates whether the Dump Time should recorded in dump file.
        /// </summary>
        public bool RecordDumpTime = true;

        /// <summary>
        /// Gets or Sets a value indicates whether the Exported Dump File should be encrypted.
        /// </summary>
        public bool EnableEncryption = false;

        /// <summary>
        /// Sets the password used to encrypt the exported dump file.
        /// </summary>
        public string EncryptionPassword = "";

        /// <summary>
        /// Gets or Sets a value indicates whether the SQL statement of "CREATE DATABASE" should added into dump file.
        /// </summary>
        public bool AddCreateDatabase = false;

        /// <summary>
        /// Gets or Sets a value indicates whether the Table Structure (CREATE TABLE) should be exported.
        /// </summary>
        public bool ExportTableStructure = true;

        /// <summary>
        /// Gets or Sets a value indicates whether the value of auto-increment of each table should be reset to 1.
        /// </summary>
        public bool ResetAutoIncrement = false;

        /// <summary>
        /// Gets or Sets a value indicates whether the Rows should be exported.
        /// </summary>
        public bool ExportRows = true;

        /// <summary>
        /// Gets or Sets the maximum length for combining multiple INSERTs into single sql. Default value is 5MB.
        /// </summary>
        public int MaxSqlLength = 5 * 1024 * 1024;

        /// <summary>
        /// Gets or Sets a value indicates whether the Stored Procedures should be exported.
        /// </summary>
        public bool ExportProcedures = true;

        /// <summary>
        /// Gets or Sets a value indicates whether the Stored Functions should be exported.
        /// </summary>
        public bool ExportFunctions = true;

        /// <summary>
        /// Gets or Sets a value indicates whether the Stored Triggers should be exported.
        /// </summary>
        public bool ExportTriggers = true;

        /// <summary>
        /// Gets or Sets a value indicates whether the Stored Views should be exported.
        /// </summary>
        public bool ExportViews = true;

        /// <summary>
        /// Gets or Sets a value indicates whether the Stored Events should be exported.
        /// </summary>
        public bool ExportEvents = true;

        /// <summary>
        /// Gets or Sets a value indicates the interval of time (in miliseconds) to raise the event of ExportProgressChanged.
        /// </summary>
        public int IntervalForProgressReport { get { if (_interval == 0) return 100; return _interval; } set { _interval = value; } }

        /// <summary>
        /// Gets or Sets a value indicates whether the totals of rows should be counted before export process commence.
        /// </summary>
        public bool GetTotalRowsBeforeExport = false;

        /// <summary>
        /// Gets or Sets the delimiter used for exporting Procedures, Functions, Events and Triggers. Default delimiter is "|".
        /// </summary>
        public string ScriptsDelimiter { get { if (string.IsNullOrEmpty(_delimiter)) return "|"; return _delimiter; } set { _delimiter = value; } }

        /// <summary>
        /// Gets or Sets a value indicates whether the exported Scripts (Procedure, Functions, Events, Triggers, Events) should exclude DEFINER.
        /// </summary>
        public bool ExportRoutinesWithoutDefiner = true;

        public ExportInformations()
        {

        }
    }
}
