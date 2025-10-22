using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code_Generator
{
    public class clsDatabaseSchema
    {
        public class TableSchema
        {
            public string TableName;
            public Dictionary<string, ColumnSchema> Columns;
            public Dictionary<string, ReferencedColumnSchema> ReferencedTableFK;
            public ColumnSchema PrimaryKey;
        }

        public class ColumnSchema
        {
            public string Name;
            public string DataType;
            public string CSharpType;
            public bool IsNullable;
            public bool IsPrimaryKey;
        }


        public class ReferencedColumnSchema
        {
            public string Name;
            public string CSharpType;
            public string ReferencedTable;
        }


    }
}
