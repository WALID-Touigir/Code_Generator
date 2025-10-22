using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code_Generator
{
    public class clsUtils
    {
        public static void GenerateFile(string path, string fileName, string content)
        {
            string fullPath = Path.Combine(path, fileName);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            Task.Run(() => File.WriteAllText(fullPath, content));
        }



        public static bool IsValidDatabaseName(string dbName)
        {
            // Check If DB Name has a save Name
            return System.Text.RegularExpressions.Regex.IsMatch(dbName, @"^[a-zA-Z0-9_]+$");
        }
        public static bool IsValidFolderName(string dbName)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(dbName, @"^[a-zA-Z0-9_]+$");
        }

        public static bool IsValidPath(string path)
        {
            // Check if the path exists & not NUll
            return !string.IsNullOrEmpty(path) && Directory.Exists(path);
        }
    }
}
