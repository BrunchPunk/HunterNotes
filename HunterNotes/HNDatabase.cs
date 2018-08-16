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

        public static void Init()
        {
            Console.WriteLine("Initializing Database");

            if (!File.Exists("HNDatabase.sqlite"))
            {
                // Create the database and file and open connection
                CreateHNDatabase();
                HNDatabaseConn = new SQLiteConnection("Data Source=HNDatabase.sqlite;version=3");
                HNDatabaseConn.Open();

                //Create the tables in the database (Note, order is important for foreign key constraints)
                CreateSkillsTable();
                CreateMaterialsTable();
                CreateArmorTable();
                CreateForgeTable();
                CreateDecorationsTable();

                //TODO Read static data into the database

            }
            else
            {
                HNDatabaseConn = new SQLiteConnection("Data Source=HNDatabase.sqlite;version=3");
                HNDatabaseConn.Open();
            }

            
        }

        public static void Close()
        {
            Console.WriteLine("Closing Database");

            HNDatabaseConn.Close();
        }

        private static void CreateHNDatabase()
        {
            Console.WriteLine("Creating Database");
            SQLiteConnection.CreateFile("HNDatabase.sqlite");
        }

        #region Create Table Functions
        private static void CreateSkillsTable()
        {
            Console.WriteLine("Creating Skills Table");

            string createTableSQL = 
                "CREATE TABLE Skills (" +
                "name VARCHAR(50), " +
                "description VARCHAR(100), " +
                "PRIMARY KEY (name) )";
            SQLiteCommand createTableCommand = new SQLiteCommand(createTableSQL, HNDatabase.HNDatabaseConn);
            createTableCommand.ExecuteNonQuery();
        }

        private static void CreateMaterialsTable()
        {
            Console.WriteLine("Creating Materials Table");

            string createTableSQL = 
                "CREATE TABLE Materials (" +
                "name VARCHAR(50), " +
                "description VARCHAR(100), " +
                "PRIMARY KEY (name) )";
            SQLiteCommand createTableCommand = new SQLiteCommand(createTableSQL, HNDatabase.HNDatabaseConn);
            createTableCommand.ExecuteNonQuery();
        }

        private static void CreateArmorTable()
        {
            Console.WriteLine("Creating Armor Table");

            string createTableSQL = 
                "CREATE TABLE Armor (" +
                "name VARCHAR(50), " +
                "equipSlot VARCHAR(20), " +
                "skill1Name VARCHAR(50), " +
                "skill1Points INT, " +
                "skill2Name VARCHAR(50), " +
                "skill2Points INT, " +
                "skill3Name VARCHAR(50), " +
                "skill3Points INT, " +
                "skill4Name VARCHAR(50), " +
                "skill4Points INT, " +
                "decorationSlot1 INT, " +
                "decorationSlot2 INT, " +
                "decorationSlot3 INT, " +
                "PRIMARY KEY (name), " +
                "FOREIGN KEY (skill1Name) REFERENCES Skills (name), " +
                "FOREIGN KEY (skill2Name) REFERENCES Skills (name), " +
                "FOREIGN KEY (skill3Name) REFERENCES Skills (name), " +
                "FOREIGN KEY (skill3Name) REFERENCES Skills (name) )";
            SQLiteCommand createTableCommand = new SQLiteCommand(createTableSQL, HNDatabase.HNDatabaseConn);
            createTableCommand.ExecuteNonQuery();
        }

        private static void CreateForgeTable()
        {
            Console.WriteLine("Creating Forge Table");

            string createTableSQL =
                "CREATE TABLE Forge (" +
                "name VARCHAR(50), " +
                "ingredient1 VARCHAR(50), " +
                "ingredient1Count INT, " +
                "ingredient2 VARCHAR(50), " +
                "ingredient2Count INT, " +
                "ingredient3 VARCHAR(50), " +
                "ingredient3Count INT, " +
                "ingredient4 VARCHAR(50), " +
                "ingredient4Count INT, " +
                "PRIMARY KEY (name), " +
                "FOREIGN KEY (ingredient1) REFERENCES Materials (name), " +
                "FOREIGN KEY (ingredient2) REFERENCES Materials (name), " +
                "FOREIGN KEY (ingredient3) REFERENCES Materials (name), " +
                "FOREIGN KEY (ingredient4) REFERENCES Materials (name) )";
            SQLiteCommand createTableCommand = new SQLiteCommand(createTableSQL, HNDatabase.HNDatabaseConn);
            createTableCommand.ExecuteNonQuery();
        }

        private static void CreateDecorationsTable()
        {
            Console.WriteLine("Creating Decorations Table");

            string createTableSQL = 
                "CREATE TABLE Decorations (" +
                "name VARCHAR(50), " +
                "skill VARCHAR(50), " +
                "size INT, " +
                "points INT, " +
                "owned INT, " +
                "PRIMARY KEY (name), " +
                "FOREIGN KEY (skill) REFERENCES Skills (name) )";
            SQLiteCommand createTableCommand = new SQLiteCommand(createTableSQL, HNDatabase.HNDatabaseConn);
            createTableCommand.ExecuteNonQuery();
        }
        #endregion

        #region Load Table Functions
        private static void LoadSkillsTable()
        {
            //TODO Load data from files into Skills table
            if(File.Exists("skillsFileTBD.txt"))
            {

            }
            else
            {
                //TODO is this the best exception handling? 
                throw new FileNotFoundException("The skillsFileTBD.txt file is missing.");
            }
        }

        private static void LoadMaterialsTable()
        {
            //TODO Load data from files into Materials table
        }

        private static void LoadSArmorTable()
        {
            //TODO Load data from files into Armor table
        }

        private static void LoadForgeTable()
        {
            //TODO Load data from files into Forge table
        }

        private static void LoadDecorationsTable()
        {
            //TODO Load data from files into Decorations table
        }

        #endregion
    }
}
