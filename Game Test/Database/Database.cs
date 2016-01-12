using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;

namespace Game_Test
{
    class Database
    {
        private SQLiteConnection Connect;
        private SQLiteCommand Command;
        private SQLiteDataReader Reader;

        public Database()
        {
            SetConnection();
        }


        private void SetConnection()
        {
            this.Connect =
                new SQLiteConnection("Data Source=MyDatabase.sqlite;Version=3;");
        }

        private void ExecuteQuery(string txtQuery)
        {
            SetConnection();
            Connect.Open();
            Command = new SQLiteCommand(txtQuery, Connect);
            Command.ExecuteNonQuery();
            Connect.Close();
        }
        
        private string ReturnRead(string ValueName)
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
    }
}
