using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace WindowsFormsApp2
{
    class KoneksiDatabase
    {
        public static MySqlConnection con = new MySqlConnection("server = localhost; userid = root; password = gerlinkju@r@ ; database = glendoscope");
        public static MySqlCommand cmd;
        public static MySqlDataAdapter da;
        public static MySqlDataReader mdr;
    }
}
