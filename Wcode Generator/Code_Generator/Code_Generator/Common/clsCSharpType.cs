using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code_Generator.CsharpMap
{
    public class clsCSharpType
    {
        public static string MapSqlTypeToCSharpType(string sqlDataType)
        {

            string type = sqlDataType.ToLower();

            switch (type)
            {
                case "int":
                    return "int";
                case "bigint":
                    return "long";
                case "smallint":
                    return "short";
                case "tinyint":
                    return "byte";
                case "bit":
                    return "bool";
                case "decimal":
                case "numeric":
                case "money":
                case "smallmoney":
                    return "decimal";
                case "float":
                    return "double";
                case "real":
                    return "float";
                case "char":
                case "varchar":
                case "text":
                case "nchar":
                case "nvarchar":
                case "ntext":
                    return "string";
                case "date":
                case "datetime":
                case "datetime2":
                case "smalldatetime":
                    return "DateTime";
                case "time":
                    return "TimeSpan";
                case "timestamp":
                    return "byte[]"; // In SQL, 'timestamp' is a synonym for 'rowversion'
                case "binary":
                case "varbinary":
                    return "byte[]";
                case "uniqueidentifier":
                    return "Guid";
                case "xml":
                    return "XDocument"; // XML type can be mapped to XDocument or string (for raw XML)
                default:
                    return "object"; // Default fallback for any unknown types
            }
        }
    }
}
