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
                    LoadSkillsTable();
                    LoadMaterialsTable();
                    LoadDecorationsTable();
                    //LoadArmorTable();
                    //LoadForgeTable();
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
                "maxPoints INT, " + 
                "description VARCHAR(600), " +
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
            if(File.Exists("data/formatted/skills.csv"))
            {
                Console.WriteLine("Loading data into the Skills table");

                //String defining the insert SQL for the Skills table
                string insertSkillsSQL = "INSERT INTO Skills VALUES ";

                //Prepare the parser to parse the Skills data file
                TextFieldParser skillsParser = new TextFieldParser("data/formatted/skills.csv");
                skillsParser.TextFieldType = FieldType.Delimited;
                skillsParser.SetDelimiters(",");

                //Read each line of the file and add it to insertSkillsSQL as a value e.g. "("Airborne",1,"Lv1: Jumping attack power +10% | "), "
                while (!skillsParser.EndOfData)
                {
                    //Begin a new value
                    insertSkillsSQL = insertSkillsSQL + "(";

                    //Add each of the three fields to the value (Since first and third fields in skills should be strings, wrap them in quotes for SQL)
                    string[] fields = skillsParser.ReadFields();
                    insertSkillsSQL = insertSkillsSQL + "\"" + fields[0] + "\"" + ", ";
                    insertSkillsSQL = insertSkillsSQL + fields[1] + ", ";
                    insertSkillsSQL = insertSkillsSQL + "\"" + fields[2].Replace(" | ", "\n").Replace(" |", "\n") + "\"";

                    //End the new value
                    insertSkillsSQL = insertSkillsSQL + "), ";

                }
                //Remove trailing ", " from last value and add SQL terminating ";"
                insertSkillsSQL = insertSkillsSQL.Substring(0, insertSkillsSQL.Length - 2);
                insertSkillsSQL = insertSkillsSQL + ";";

                //Create the SQL command and execute it
                SQLiteCommand insertSkillsCommand = new SQLiteCommand(insertSkillsSQL, HNDatabase.HNDatabaseConn);
                insertSkillsCommand.ExecuteNonQuery();
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
            if (File.Exists("data/formatted/armor.csv"))
            {
                Console.WriteLine("Loading data into the Armor table");
                //String defining the insert SQL for the Decorations table
                string insertArmorSQL = "INSERT INTO Armor VALUES ";

                //Prepare the parser to parse the Decorations data file
                TextFieldParser armorParser = new TextFieldParser("data/formatted/armor.csv");
                armorParser.TextFieldType = FieldType.Delimited;
                armorParser.SetDelimiters(",");

                //Read each line of the file and add it to insertArmorSQL as a value e.g. "("Poison Charm I","Charm","Poison Resistance",1,"",,"",,"",,"","",""), "
                while (!armorParser.EndOfData)
                {
                    //Begin a new value
                    insertArmorSQL = insertArmorSQL + "(";

                    //Add each of the 13 fields to the value (wrap strings in quotes where necessary)
                    string[] fields = armorParser.ReadFields();
                    insertArmorSQL = insertArmorSQL + "\"" + fields[0] + "\"" + ", ";
                    insertArmorSQL = insertArmorSQL + "\"" + fields[1] + "\"" + ", ";
                    insertArmorSQL = insertArmorSQL + "\"" + fields[2] + "\"" + ", ";
                    insertArmorSQL = insertArmorSQL + fields[3] + ", ";
                    insertArmorSQL = insertArmorSQL + "\"" + fields[4] + "\"" + ", ";
                    insertArmorSQL = insertArmorSQL + fields[5] + ", ";
                    insertArmorSQL = insertArmorSQL + "\"" + fields[6] + "\"" + ", ";
                    insertArmorSQL = insertArmorSQL + fields[7] + ", ";
                    insertArmorSQL = insertArmorSQL + "\"" + fields[8] + "\"" + ", ";
                    insertArmorSQL = insertArmorSQL + fields[9] + ", ";
                    insertArmorSQL = insertArmorSQL + fields[10] + ", ";
                    insertArmorSQL = insertArmorSQL + fields[11] + ", ";
                    insertArmorSQL = insertArmorSQL + fields[12];

                    //End the new value
                    insertArmorSQL = insertArmorSQL + "), ";

                }
                //Remove trailing ", " from last value and add SQL terminating ";"
                insertArmorSQL = insertArmorSQL.Substring(0, insertArmorSQL.Length - 2);
                insertArmorSQL = insertArmorSQL + ";";

                //Create the SQL command and execute it
                SQLiteCommand insertArmorCommand = new SQLiteCommand(insertArmorSQL, HNDatabase.HNDatabaseConn);
                insertArmorCommand.ExecuteNonQuery();
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

                //String defining the insert SQL for the Decorations table
                string insertForgeSQL = "INSERT INTO Forge VALUES ";

                //Prepare the parser to parse the Decorations data file
                TextFieldParser forgeParser = new TextFieldParser("data/formatted/forge.csv");
                forgeParser.TextFieldType = FieldType.Delimited;
                forgeParser.SetDelimiters(",");

                //Read each line of the file and add it to insertArmorSQL as a value e.g. "("Poison Charm I","Charm","Poison Resistance",1,"",,"",,"",,"","",""), "
                while (!forgeParser.EndOfData)
                {
                    //Begin a new value
                    insertForgeSQL = insertForgeSQL + "(";

                    //Add each of the 13 fields to the value (wrap strings in quotes where necessary)
                    string[] fields = forgeParser.ReadFields();
                    insertForgeSQL = insertForgeSQL + "\"" + fields[0] + "\"" + ", ";
                    insertForgeSQL = insertForgeSQL + "\"" + fields[1] + "\"" + ", ";
                    insertForgeSQL = insertForgeSQL + fields[2] + ", ";
                    insertForgeSQL = insertForgeSQL + "\"" + fields[3] + "\"" + ", ";
                    insertForgeSQL = insertForgeSQL + fields[4] + ", ";
                    insertForgeSQL = insertForgeSQL + "\"" + fields[5] + "\"" + ", ";
                    insertForgeSQL = insertForgeSQL + fields[6] + ", ";
                    insertForgeSQL = insertForgeSQL + "\"" + fields[7] + "\"" + ", ";
                    insertForgeSQL = insertForgeSQL + fields[8] + ", ";

                    //End the new value
                    insertForgeSQL = insertForgeSQL + "), ";

                }
                //Remove trailing ", " from last value and add SQL terminating ";"
                insertForgeSQL = insertForgeSQL.Substring(0, insertForgeSQL.Length - 2);
                insertForgeSQL = insertForgeSQL + ";";

                //Create the SQL command and execute it
                SQLiteCommand insertForgeCommand = new SQLiteCommand(insertForgeSQL, HNDatabase.HNDatabaseConn);
                insertForgeCommand.ExecuteNonQuery();
            }
            else
            {
                throw new FileNotFoundException("The forge.csv file is missing.");
            }
        }

        private static void LoadDecorationsTable()
        {
            if (File.Exists("data/formatted/decorations.csv"))
            {
                Console.WriteLine("Loading data into the Decorations table");

                //String defining the insert SQL for the Decorations table
                string insertDecorationsSQL = "INSERT INTO Decorations VALUES ";

                //Prepare the parser to parse the Decorations data file
                TextFieldParser decorationsParser = new TextFieldParser("data/formatted/decorations.csv");
                decorationsParser.TextFieldType = FieldType.Delimited;
                decorationsParser.SetDelimiters(",");

                //Read each line of the file and add it to insertDecorationsSQL as a value e.g. "("Antidote Jewel","Poison Resistance",1,0), "
                while (!decorationsParser.EndOfData)
                {
                    //Begin a new value
                    insertDecorationsSQL = insertDecorationsSQL + "(";

                    //Add each of the 4 fields to the value (Since first and second fields in decorations should be strings, wrap them in quotes for SQL)
                    string[] fields = decorationsParser.ReadFields();
                    insertDecorationsSQL = insertDecorationsSQL + "\"" + fields[0] + "\"" + ", ";
                    insertDecorationsSQL = insertDecorationsSQL + "\"" + fields[1] + "\"" + ", ";
                    insertDecorationsSQL = insertDecorationsSQL + fields[2] + ", ";
                    insertDecorationsSQL = insertDecorationsSQL + fields[3];

                    //End the new value
                    insertDecorationsSQL = insertDecorationsSQL + "), ";

                }
                //Remove trailing ", " from last value and add SQL terminating ";"
                insertDecorationsSQL = insertDecorationsSQL.Substring(0, insertDecorationsSQL.Length - 2);
                insertDecorationsSQL = insertDecorationsSQL + ";";

                //Create the SQL command and execute it
                SQLiteCommand insertDecorationsCommand = new SQLiteCommand(insertDecorationsSQL, HNDatabase.HNDatabaseConn);
                insertDecorationsCommand.ExecuteNonQuery();
            }
            else
            {
                throw new FileNotFoundException("The decorations.csv file is missing.");
            }
        }

        #endregion

        #region Database Accessors
        #region Materials Methods
        public static List<Material> GetAllMaterials()
        {
            string selectQuery;
            SQLiteCommand selectCommand;
            SQLiteDataReader selectResults;
            List<Material> results = new List<Material>();

            selectQuery = "SELECT * FROM Materials;";
            selectCommand = new SQLiteCommand(selectQuery, HNDatabaseConn);
            selectResults = selectCommand.ExecuteReader();

            while (selectResults.Read())
            {
                results.Add(new Material((string) selectResults[0], (string) selectResults[1]));
            }

            return results;

        }

        public static string GetMaterialDescription(string materialName)
        {
            string selectQuery;
            SQLiteCommand selectCommand;
            SQLiteDataReader selectResults;
            string result = "";

            selectQuery = "SELECT description FROM Materials WHERE name=\"" + materialName + "\" LIMIT 1;";
            selectCommand = new SQLiteCommand(selectQuery, HNDatabaseConn);
            selectResults = selectCommand.ExecuteReader();

            while(selectResults.Read())
            {
                result = (string) selectResults[0];
                break;
            }

            return result;
        }

        #endregion

        #region Skills Method
        public static List<Skill> GetAllSkills()
        {
            string selectQuery;
            SQLiteCommand selectCommand;
            SQLiteDataReader selectResults;
            List<Skill> results = new List<Skill>();

            selectQuery = "SELECT * FROM Skills;";
            selectCommand = new SQLiteCommand(selectQuery, HNDatabaseConn);
            selectResults = selectCommand.ExecuteReader();

            while (selectResults.Read())
            {
                results.Add(new Skill((string)selectResults[0], (int)selectResults[1], (string)selectResults[2]));
            }

            return results;

        }

        public static string GetSkillDescription(string skillName)
        {
            string selectQuery;
            SQLiteCommand selectCommand;
            SQLiteDataReader selectResults;
            string result = "";

            selectQuery = "SELECT description FROM Skills WHERE name=\"" + skillName + "\" LIMIT 1;";
            selectCommand = new SQLiteCommand(selectQuery, HNDatabaseConn);
            selectResults = selectCommand.ExecuteReader();

            while (selectResults.Read())
            {
                result = (string)selectResults[0];
                break;
            }

            return result;
        }

        #endregion

        #region Decorations Methods
        public static List<Decoration> GetAllDecorations()
        {
            string selectQuery;
            SQLiteCommand selectCommand;
            SQLiteDataReader selectResults;
            List<Decoration> results = new List<Decoration>();

            selectQuery = "SELECT * FROM Decorations;";
            selectCommand = new SQLiteCommand(selectQuery, HNDatabaseConn);
            selectResults = selectCommand.ExecuteReader();

            while (selectResults.Read())
            {
                results.Add(new Decoration((string)selectResults[0], (string)selectResults[1], (int)selectResults[2], (int)selectResults[3]));
            }

            return results;
        }

        public static int GetOwned(string decoName)
        {
            string selectQuery;
            SQLiteCommand selectCommand;
            SQLiteDataReader selectResults;

            selectQuery = "SELECT owned FROM Decorations WHERE name = \"" + decoName + "\";";
            selectCommand = new SQLiteCommand(selectQuery, HNDatabaseConn);
            selectResults = selectCommand.ExecuteReader();

            selectResults.Read();
            return (int)selectResults[0];
        }

        #endregion

        #endregion

        #region Database Mutators
        #region Decorations Methods
        public static void UpdateOwned(string decoName, int newOwned)
        {
            string updateQuery = "UPDATE Decorations SET  owned = " + newOwned + " WHERE name = \"" + decoName + "\";";
            SQLiteCommand updateQueryCommand = new SQLiteCommand(updateQuery, HNDatabase.HNDatabaseConn);
            updateQueryCommand.ExecuteNonQuery();
        }

        #endregion

        #endregion
    }
}
