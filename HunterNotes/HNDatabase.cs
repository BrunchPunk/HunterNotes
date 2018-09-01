using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using Microsoft.VisualBasic.FileIO;

namespace HunterNotes
{
    public static class HNDatabase
    {
        static SQLiteConnection HNDatabaseConn;

        public static void Init()
        {
            Console.WriteLine("Initializing Database");

            //TODO - THIS IS A HACK TO FORCE DB CREATION EVERY TIME DELETE THIS WHEN IT'S WORKING
            if (File.Exists("HNDatabase.sqlite"))
            {
                File.Delete("HNDatabase.sqlite");
            }

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
                try
                {
                    //LoadSkillsTable();
                    LoadMaterialsTable();
                    //LoadArmorTable();
                    //LoadForgeTable();
                    //LoadDecorationsTable();
                }
                catch(FileNotFoundException ex)
                {
                    Console.WriteLine("FileNotFoundException encountered while loading data: " + ex.Message);
                }
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
            if(File.Exists("data/formatted/skills.csv"))
            {
                Console.WriteLine("Loading data into the Skills table");
            }
            else
            {
                throw new FileNotFoundException("The skills.csv file is missing.");
            }
        }

        private static void LoadMaterialsTable()
        {
            if (File.Exists("data/formatted/materials.csv"))
            {
                Console.WriteLine("Loading data into the Materials table");

                //String defining the insert SQL for the materials table
                string insertMaterialsSQL = "INSERT INTO Materials VALUES ";

                //Prepare the parser to parse the materials data file
                TextFieldParser materialsParser = new TextFieldParser("data/formatted/materials.csv");
                materialsParser.TextFieldType = FieldType.Delimited;
                materialsParser.SetDelimiters(",");

                //Read each line of the file and add it to insertMaterialsSQL as a value e.g. "(potion, heals or whatever), "
                while (!materialsParser.EndOfData)
                {
                    //Begin a new value
                    insertMaterialsSQL = insertMaterialsSQL + "(";

                    //Add each field to the value
                    string[] fields = materialsParser.ReadFields();
                    foreach (string field in fields)
                    {
                        //Since both fields in Materials should be strings, wrap them in quotes for SQL
                        insertMaterialsSQL = insertMaterialsSQL + "\"" + field + "\"" + ", ";
                    }

                    //Remove trailing ", " from last value field and close this value
                    insertMaterialsSQL = insertMaterialsSQL.Substring(0, insertMaterialsSQL.Length - 2); 
                    insertMaterialsSQL = insertMaterialsSQL + "), ";
                }
                //Remove trailing ", " from last value and add SQL terminating ";"
                insertMaterialsSQL = insertMaterialsSQL.Substring(0, insertMaterialsSQL.Length - 2);
                insertMaterialsSQL = insertMaterialsSQL + ";";

                //Create the SQL command and execute it
                SQLiteCommand insertMaterialsCommand = new SQLiteCommand(insertMaterialsSQL, HNDatabase.HNDatabaseConn);
                insertMaterialsCommand.ExecuteNonQuery();
            }
            else
            {
                throw new FileNotFoundException("The materials.csv file is missing.");
            }
        }

        private static void LoadArmorTable()
        {
            //TODO Load data from files into Armor table
            if (File.Exists("data/formatted/armor.csv"))
            {
                Console.WriteLine("Loading data into the Armor table");
            }
            else
            {
                throw new FileNotFoundException("The armor.csv file is missing.");
            }
        }

        private static void LoadForgeTable()
        {
            //TODO Load data from files into Forge table
            if (File.Exists("data/formatted/forge.csv"))
            {
                Console.WriteLine("Loading data into the Forge table");
            }
            else
            {
                throw new FileNotFoundException("The forge.csv file is missing.");
            }
        }

        private static void LoadDecorationsTable()
        {
            //TODO Load data from files into Decorations table
            if (File.Exists("data/formatted/decorations.csv"))
            {
                Console.WriteLine("Loading data into the Decorations table");
            }
            else
            {
                throw new FileNotFoundException("The decorations.csv file is missing.");
            }
        }

        #endregion
    }
}
