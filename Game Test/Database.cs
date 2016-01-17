using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;


namespace Game_Test
{
    public class Database
    {
        private static Database instance;

        private SQLiteConnection Connect;
        private SQLiteCommand Command;
        private SQLiteDataReader Reader;

        public static Database Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Database();
                }
                return instance;
            }
        }

        public Database()
        {
            SetConnection();
        }


        private void SetConnection()
        {
            this.Connect =
                new SQLiteConnection("Data Source=DatabaseSQLite.db;Version=3;");
        }

        public void ExecuteQuery(string txtQuery)
        {
            //SetConnection();
            Connect.Open();
            Command = new SQLiteCommand(txtQuery, Connect);
            Command.ExecuteNonQuery();
            Connect.Close();
        }

        public string ReturnRead(string ValueName)
        {
            Reader = Command.ExecuteReader();
            if (Reader.Read())
            {
                return Reader[ValueName].ToString();
            }
            else
            {
                return null;
            }

        }

        public int ReadQuery(string query, string value)
        {
            int returnvalue = 0;
            Connect.Open();
            Command = new SQLiteCommand(query, Connect);
            Reader = Command.ExecuteReader();
            while(Reader.Read())
            {
                Connect.Close();
                returnvalue = (int)Reader[value];
            }
            Connect.Close();
            return returnvalue;
        }
    }
}
