using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace HunterNotes
{
    public static class HNDatabase
    {
        static SQLiteConnection HNDatabaseConn;

        public static void init()
        {
            Console.WriteLine("Initializing Database");

            if (!File.Exists("HNDatabase.sqlite"))
            {
                SQLiteConnection.CreateFile("HNDatabase.sqlite");
            }

            HNDatabaseConn = new SQLiteConnection("Data Source=HNDatabase.sqlite;version=3");
            HNDatabaseConn.Open();
        }

        public static void close()
        {
            Console.WriteLine("Closing Database");

            HNDatabaseConn.Close();
        }
    }
}
