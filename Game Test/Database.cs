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
            this.Connect = new SQLiteConnection("Data Source=DatabaseSQLite.db;Version=3;");
        }

        public void ExecuteQuery(string txtQuery)
        {
            Connect.Open();
            Command = new SQLiteCommand(txtQuery, Connect);
            Command.ExecuteNonQuery();
            Connect.Close();
        }

        private string ReturnRead(string txtQuery, string ValueName)
        {
            Connect.Open();
            Command = new SQLiteCommand(txtQuery, Connect);
            Reader = Command.ExecuteReader();
            if (Reader.Read())
            {
                string x = Reader[ValueName].ToString();
                Connect.Close();
                return x;
            }
            else
            {
                Connect.Close();
                return null;
            }

        }

        public string ReturnEnemyHP(string EnemyName)
        {
            return ReturnRead("SELECT EnemyHP FROM Enemies WHERE EnemyName LIKE " + EnemyName, "EnemyHP");
        }
    }
}
