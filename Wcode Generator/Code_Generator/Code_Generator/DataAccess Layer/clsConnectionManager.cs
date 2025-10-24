using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Code_Generator
{
    public class clsConnectionManager
    {

        public static class clsDBconnetionstring
        {


            public static string GetMasterConnectionString()
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = ".";
                builder.InitialCatalog = "master";
                builder.UserID = clsGlobal.GlobalUserName;
                builder.Password = clsGlobal.GlobalPassword;
                return builder.ToString();
            }


            public static string GetAppConnectionString(string dbName)
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = ".";
                builder.InitialCatalog = dbName;
                builder.UserID = clsGlobal.GlobalUserName;
                builder.Password = clsGlobal.GlobalPassword;
                return builder.ToString();
            }
        }





        private static DataTable ExecuteQuery(string connectionString, string query)
        {
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            dt.Load(reader);
                        }
                    }
                }
                catch
                {
                    //logger error
                    return new DataTable(); 
                }
            }

            return dt;
        }




        public static DataTable GetDBNames()
        {
            string connString = clsDBconnetionstring.GetMasterConnectionString();

            string query = "SELECT name FROM sys.databases WHERE name NOT IN ('master','model','msdb','tempdb') ORDER BY name;";

            return ExecuteQuery(connString, query);
        }



        public static bool RestorMdfDB(string path, string DBname)
        {

            string query = @"USE master;
                            CREATE DATABASE [@DBname]
                            ON (FILENAME = @path)
                            FOR ATTACH;";

            bool isRestored = false;


            using (SqlConnection connection = new SqlConnection(clsDBconnetionstring.GetMasterConnectionString()))
            {


                try
                {
                    connection.Open();
                    
                    using (var restorDbCmd = new SqlCommand(query, connection))
                    {
                        restorDbCmd.Parameters.AddWithValue("@path", path);
                        restorDbCmd.Parameters.AddWithValue("@DBname", DBname);

                        restorDbCmd.ExecuteNonQuery();
                        isRestored = true;

                    }


                }
                catch
                {
                    isRestored = false;
                }
                finally
                { connection.Close(); }



                return isRestored;
            }
        }


        public static bool RestorBakDB(string path, string DBname)
        {
            bool isRestored= false;


            using (SqlConnection connection = new SqlConnection(clsDBconnetionstring.GetMasterConnectionString()))
            {


                try
                {
                    connection.Open();
                    using (var restorDbCmd = new SqlCommand($@"USE master;
                                                            RESTORE DATABASE [{DBname}]
                                                            FROM DISK = @path
                                                            WITH REPLACE, RECOVERY;", connection))
                    {
                        restorDbCmd.Parameters.AddWithValue("@path", path);

                        restorDbCmd.ExecuteNonQuery();
                        isRestored=  true;
                        
                    }


                }
                catch
                {
                    isRestored =  false;
                }
                finally
                { connection.Close(); }



                return isRestored;
            }
        }
        

        public static DataTable GetDBTableNames()
        {

            string connString = clsDBconnetionstring.GetAppConnectionString(clsGlobal.DataBaseName);

            string query = "SELECT t.name AS TableName FROM sys.tables t WHERE t.is_ms_shipped = 0 AND t.name NOT IN ('sysdiagrams') ORDER BY TableName;";

            return ExecuteQuery(connString, query);
        }



        public static DataTable GetTableSchema(string tableName)
        {
            string query = $@"SELECT
                            C.COLUMN_NAME,
                            C.DATA_TYPE,
                            C.IS_NULLABLE,
                            CASE 
                                WHEN PK.COLUMN_NAME IS NOT NULL THEN 1 
                                ELSE 0          
                            END AS IsPrimaryKey,
                            FK.ReferencedTable,
                            FK.ParentColumn AS ForeignKeyColumn,
                            FK.ParentDataType AS ForeignKeyDataType -- Added the ParentDataType here
                        FROM 
                            INFORMATION_SCHEMA.COLUMNS AS C
                        -- 1. Determine Primary Key Status
                        LEFT JOIN (
                            SELECT 
                                KU.COLUMN_NAME
                            FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TC
                            JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS KU
                                ON TC.CONSTRAINT_NAME = KU.CONSTRAINT_NAME
                                AND TC.TABLE_SCHEMA = KU.TABLE_SCHEMA
                                AND TC.TABLE_NAME = KU.TABLE_NAME
                            WHERE TC.CONSTRAINT_TYPE = 'PRIMARY KEY' 
                                AND TC.TABLE_NAME = '{tableName}'
                        ) AS PK
                            ON C.COLUMN_NAME = PK.COLUMN_NAME
                        -- 2. Determine Foreign Key References
                        LEFT JOIN (
                            SELECT
                                cp.name AS ParentColumn,
                                t_cp.name AS ParentDataType, -- Included the data type here
                                tr.name AS ReferencedTable
                            FROM sys.foreign_keys AS fk
                            INNER JOIN sys.foreign_key_columns AS fkc
                                ON fk.object_id = fkc.constraint_object_id
                            INNER JOIN sys.tables AS tp
                                ON fkc.parent_object_id = tp.object_id
                            INNER JOIN sys.columns AS cp
                                ON fkc.parent_object_id = cp.object_id 
                                AND fkc.parent_column_id = cp.column_id
                            INNER JOIN sys.types AS t_cp -- Joined sys.types to get the data type
                                ON cp.user_type_id = t_cp.user_type_id
                            INNER JOIN sys.tables AS tr
                                ON fkc.referenced_object_id = tr.object_id
                            WHERE tp.name = '{tableName}'
                        ) AS FK
                            ON C.COLUMN_NAME = FK.ParentColumn

                        WHERE 
                            C.TABLE_NAME = '{tableName}'
    

                        ORDER BY 
                            C.ORDINAL_POSITION;";  //Honestly, I didn't write this query myself👍
            return ExecuteQuery(clsDBconnetionstring.GetAppConnectionString(clsGlobal.DataBaseName), query); ;
        }

     

    }

}