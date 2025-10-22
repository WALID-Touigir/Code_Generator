using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using static Code_Generator.clsDatabaseSchema;

namespace Code_Generator
{
    public static class clsGlobal
    {
        static string RegestrykeyPath = @"HKEY_CURRENT_USER\SOFTWARE\Code_Generator";


        public static string DataBaseName = "";
        public static string ProjectName = "";
        public static string PathFilesToGenerate = "";
    


        public static bool AddingDTOclass ;


        public static string GlobalUserName { get; private set; }
        public static string GlobalPassword { get; private set; }
        public static void SetCredentials(string userName, string password)
        {
            GlobalUserName = userName;
            GlobalPassword = password;
        }


        public static bool RememberUsernameAndPassword(string Username, string Password)
        {

            try
            {

                Registry.SetValue(RegestrykeyPath, "Username", Username, RegistryValueKind.String);
                Registry.SetValue(RegestrykeyPath, "Password", Password, RegistryValueKind.String);

                return true;
            }
            catch (Exception ex)
            {
                // Loge error later
            }

            return false;
        }

        public static bool GetStoredCredential(ref string Username, ref string Password)
        {
            try
            {
                Username = Registry.GetValue(RegestrykeyPath, "Username", null) as string;
                Password = Registry.GetValue(RegestrykeyPath, "Password", null) as string;

                return true;
            }
            catch (Exception ex)
            {

            }
            return false;
        }

        








    }
}
