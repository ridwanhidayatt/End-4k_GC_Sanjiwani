using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace MySql.Data.MySqlClient
{
    public class MySqlColumn
    {
        public enum DataWrapper
        {
            None,
            Sql
        }

        string _name = "";
        Type _dataType = typeof(string);
        string _mySqlDataType = "";
        string _collation = "";
        bool _allowNull = true;
        string _key = "";
        string _defaultValue = "";
        string _extra = "";
        string _privileges = "";
        string _comment = "";

        public string Name { get { return _name; } }
        public Type DataType { get { return _dataType; } }
        public string MySqlDataType { get { return _mySqlDataType; } }
        public string Collation { get { return _collation; } }
        public bool AllowNull { get { return _allowNull; } }
        public string Key { get { return _key; } }
        public string DefaultValue { get { return _defaultValue; } }
        public string Extra { get { return _extra; } }
        public string Privileges { get { return _privileges; } }
        public string Comment { get { return _comment; } }

        public MySqlColumn(string name, Type type, string mySqlDataType,
            string collation, bool allowNull, string key, string defaultValue,
            string extra, string privileges, string comment)
        {
            _name = name;
            _dataType = type;
            _mySqlDataType = mySqlDataType.ToLower();
            _collation = collation;
            _allowNull = allowNull;
            _key = key;
            _defaultValue = defaultValue;
            _extra = extra;
            _privileges = privileges;
            _comment = comment;
        }

    }
}