using System;
using System.Collections.Generic;
using System.Text;

namespace MySql.Data.MySqlClient
{
    public class MySqlServer
    {
        string _versionNumber;
        string _edition;
        decimal _majorVersionNumber = 0;
        string _characterSetServer = "";
        string _characterSetSystem = "";
        string _characterSetConnection = "";
        string _characterSetDatabase = "";
        string _currentUser = "";
        string _currentUserClientHost = "";
        string _currentClientHost = "";

        public string Version { get { return string.Format("{0} {1}", _versionNumber, _edition); } }
        public string VersionNumber { get { return _versionNumber; } }
        public decimal MajorVersionNumber { get { return _majorVersionNumber; } }
        public string Edition { get { return _edition; } }
        public string CharacterSetServer { get { return _characterSetServer; } }
        public string CharacterSetSystem { get { return _characterSetSystem; } }
        public string CharacterSetConnection { get { return _characterSetConnection; } }
        public string CharacterSetDatabase { get { return _characterSetDatabase; } }
        public string CurrentUser { get { return _currentUser; } }
        public string CurrentUserClientHost { get { return _currentUserClientHost; } }
        public string CurrentClientHost { get { return _currentClientHost; } }

        public List<string> DocumentHeaders { get; set; }
        public List<string> DocumentFooters { get; set; }

        public MySqlServer()
        { }

        public void GetServerInfo(MySqlCommand cmd)
        {
            _edition = QueryExpress.ExecuteScalarStr(cmd, "SHOW variables LIKE 'version_comment';", 1);
            _versionNumber = QueryExpress.ExecuteScalarStr(cmd, "SHOW variables LIKE 'version';", 1);
            _characterSetServer = QueryExpress.ExecuteScalarStr(cmd, "SHOW variables LIKE 'character_set_server';", 1);
            _characterSetSystem = QueryExpress.ExecuteScalarStr(cmd, "SHOW variables LIKE 'character_set_system';", 1);
            _characterSetConnection = QueryExpress.ExecuteScalarStr(cmd, "SHOW variables LIKE 'character_set_connection';", 1);
            _characterSetDatabase = QueryExpress.ExecuteScalarStr(cmd, "SHOW variables LIKE 'character_set_database';", 1);

            _currentUserClientHost = QueryExpress.ExecuteScalarStr(cmd, "SELECT current_user;");

            string[] ca = _currentUserClientHost.Split('@');

            _currentUser = ca[0];
            _currentClientHost = ca[1];

            GetMajorVersionNumber();

            InitializeHeaderFooter(cmd);
        }

        void GetMajorVersionNumber()
        {
            string[] vsa = _versionNumber.Split('.');
            string v = "";
            if (vsa.Length > 1)
                v = vsa[0] + "." + vsa[1];
            else
                v = vsa[0];
            decimal.TryParse(v, out _majorVersionNumber);
        }

        void InitializeHeaderFooter(MySqlCommand cmd)
        {
            DocumentHeaders = new List<string>();
            DocumentFooters = new List<string>();

            DocumentHeaders.Add("/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;");
            DocumentHeaders.Add("/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;");
            DocumentHeaders.Add("/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;");
            DocumentHeaders.Add(string.Format("/*!40101 SET NAMES {0} */;", _characterSetDatabase));
            //DocumentHeaders.Add("/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;");
            //DocumentHeaders.Add("/*!40103 SET TIME_ZONE='+00:00' */;");
            DocumentHeaders.Add("/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;");
            DocumentHeaders.Add("/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;");
            DocumentHeaders.Add("/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;");
            DocumentHeaders.Add("/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;");

            //DocumentFooters.Add("/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;");
            DocumentFooters.Add("/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;");
            DocumentFooters.Add("/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;");
            DocumentFooters.Add("/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;");
            DocumentFooters.Add("/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;");
            DocumentFooters.Add("/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;");
            DocumentFooters.Add("/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;");
            DocumentFooters.Add("/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;");
        }
    }
}

