using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Code_Generator.CsharpMap;
using Code_Generator.TemplateProcessor;
using static Code_Generator.clsDatabaseSchema;
using static Code_Generator.clsGlobal;

namespace Code_Generator
{
    public class clsBusinessManager
    {
        public static ConcurrentBag<string> checkedTables = new ConcurrentBag<string>();

        public delegate void ProgressBarEventHandler(int value);
        static public event ProgressBarEventHandler ProgressBar;

        public static DataTable AllDBTables = new DataTable();

        public static DataTable GetAllDBTables()
        {
            return clsConnectionManager.GetDBTableNames();
        }

        public static bool RestoreDB(string path, out string DBname)
        {
            string dbType = Path.GetExtension(path);
            DBname = Path.GetFileNameWithoutExtension(path);
            

            if (dbType == ".bak")
            {
                return clsConnectionManager.RestorBakDB(path, DBname);
            }
            else
               return clsConnectionManager.RestorMdfDB(path, DBname);


        }


        public static bool GenerateAllLayers()
        { return clsTemplateProcessor.GenerateAllLayers(); }


        public static bool GetAllSelectedTables()
        {

            if (checkedTables == null)
                return false;

             int completedTables = 0;
            int totalTables = checkedTables.Count;

            try
            {

                Parallel.ForEach(checkedTables, tableName =>
                { 

                    TableSchema tableSchema = GetTableSchema(tableName);
                    clsTemplateProcessor.TablesSchema.AddOrUpdate(tableName, tableSchema, (key, existingValue) => tableSchema);



                    int currentCompletedCount = Interlocked.Increment(ref completedTables);
                    int percentage = (int)(((float)currentCompletedCount / totalTables) * 100);
                    ProgressBar?.Invoke(percentage);
   

                });


            }
            catch { return false; }


            return true;
        }


        public static TableSchema GetTableSchema(string TableName)
        {

            TableSchema tableSchema = new TableSchema();
            DataTable dtTabelsSchema = clsConnectionManager.GetTableSchema(TableName);

            tableSchema.TableName = TableName;

            tableSchema.Columns = GetColumnSchema(dtTabelsSchema);

            
            tableSchema.ReferencedTableFK = GetFKschemaForSelectedTables(dtTabelsSchema);


            tableSchema.PrimaryKey = GetPrimaryKeyRowSchema(dtTabelsSchema);


            if(tableSchema.PrimaryKey == null) 
                tableSchema.PrimaryKey = tableSchema.Columns.Values.FirstOrDefault(c => c.IsPrimaryKey);

            return tableSchema;
        }

        private static Dictionary<string, ColumnSchema> GetColumnSchema(DataTable dtColumns)
        {

            if (dtColumns != null && dtColumns.Rows.Count > 0)
            {
                Dictionary<string, ColumnSchema> tableSchema = new Dictionary<string, ColumnSchema>();

                for (int i = 0; i < dtColumns.Rows.Count; i++)
                {

                    ColumnSchema columnSchemas = new ColumnSchema();
                    DataRow dt = dtColumns.Rows[i];

                    columnSchemas.Name = dt["COLUMN_NAME"].ToString();
                    columnSchemas.DataType = dt["DATA_TYPE"].ToString();

                    columnSchemas.CSharpType = clsCSharpType.MapSqlTypeToCSharpType(dt["DATA_TYPE"].ToString());

                    if (dt["IS_NULLABLE"] != null && dt["IS_NULLABLE"] != DBNull.Value)
                    {
                        string isNullableStr = dt["IS_NULLABLE"].ToString().ToUpper();
                        columnSchemas.IsNullable = (isNullableStr == "YES" || isNullableStr == "TRUE" || isNullableStr == "1");
                    }
                    else
                    {
                        columnSchemas.IsNullable = false;
                    }

                    if (columnSchemas.IsNullable && columnSchemas.CSharpType != "string" && !columnSchemas.CSharpType.EndsWith("[]"))
                        columnSchemas.CSharpType += "?";// for nullable 



                    if (dt["IsPrimaryKey"] != null && dt["IsPrimaryKey"] != DBNull.Value)
                    {
                        string isPrimaryKey = dt["IsPrimaryKey"].ToString().ToUpper();
                        columnSchemas.IsPrimaryKey = (isPrimaryKey == "YES" || isPrimaryKey == "TRUE" || isPrimaryKey == "1");
                    }
                    else
                    {
                        columnSchemas.IsPrimaryKey = false;
                    }




                    tableSchema[columnSchemas.Name] = columnSchemas;
                }





                return tableSchema;
            }
            else
              return  new Dictionary<string, ColumnSchema>();

        }


        private static Dictionary<string, ReferencedColumnSchema> GetFKschemaForSelectedTables(DataTable dt)
        {

            if (dt.Rows.Count == 0)
                return new Dictionary<string, ReferencedColumnSchema>();

            Dictionary<string, ReferencedColumnSchema> ReferencedFK = new Dictionary<string, ReferencedColumnSchema>();


            foreach(DataRow row in dt.Rows)
            {
                if (row["ForeignKeyColumn"] != null && checkedTables.Contains(row["ReferencedTable"].ToString())) // Even if it has a Reference to a table, but if its not within the selected ones it wont be created
                {
                    ReferencedColumnSchema Fks = new ReferencedColumnSchema();
                    
                    Fks.Name = row["ForeignKeyColumn"].ToString();
                    Fks.CSharpType = clsCSharpType.MapSqlTypeToCSharpType(row["ForeignKeyDataType"].ToString());
                    Fks.ReferencedTable = row["ReferencedTable"].ToString();



                    ReferencedFK.Add(Fks.Name, Fks);

                }
            };


            return ReferencedFK;
        }
        private static ColumnSchema GetPrimaryKeyRowSchema(DataTable primaryKeyTable)
        {

            if (primaryKeyTable != null && primaryKeyTable.Rows.Count > 0)
            {

                DataRow dt = primaryKeyTable.Rows[0];

                    ColumnSchema rowSchema = new ColumnSchema();

                rowSchema.Name = dt["COLUMN_NAME"].ToString();
                rowSchema.DataType = dt["DATA_TYPE"].ToString();

                rowSchema.CSharpType = clsCSharpType.MapSqlTypeToCSharpType(dt["DATA_TYPE"].ToString());
                

                if (dt["IS_NULLABLE"] != null && dt["IS_NULLABLE"] != DBNull.Value)
                {
                    string isNullableStr = dt["IS_NULLABLE"].ToString().ToUpper();
                    rowSchema.IsNullable = (isNullableStr == "YES" || isNullableStr == "TRUE" || isNullableStr == "1");
                }
                else
                {
                    rowSchema.IsNullable = false;
                }
                
                rowSchema.IsPrimaryKey = true;


                return rowSchema;
            }
            else
                return new ColumnSchema();
            
        }






    }
}
