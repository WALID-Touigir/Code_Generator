using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace _DataAccessLayer
{
    public class clsData
    {
        
        // ===============================
        // FIND METHOD BY PRIMARY KEY (using OUT parameters)
        // ===============================
        
        // This is the correct signature using the new OUT placeholder
        public static bool GetLicenseClassesInfoByID(int LicenseClassID,out int licenseclassid,
			out string classname,
			out string classdescription,
			out byte minimumallowedage,
			out byte defaultvaliditylength,
			out decimal classfees)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            SqlCommand command = new SqlCommand("SELECT * FROM AppUsage WHERE LicenseClassID = @Id", connection); 
            command.Parameters.AddWithValue("@Id", LicenseClassID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    licenseclassid = (int)reader["LicenseClassID"];
					classname = (reader["ClassName"] != DBNull.Value) ? (string)reader["ClassName"] : "";
					classdescription = (reader["ClassDescription"] != DBNull.Value) ? (string)reader["ClassDescription"] : "";
					minimumallowedage = (byte)reader["MinimumAllowedAge"];
					defaultvaliditylength = (byte)reader["DefaultValidityLength"];
					classfees = (decimal)reader["ClassFees"]; // Correct placeholder for assignment inside reader.Read()
                    
                    isFound = true;
                }
                else
                {
                    licenseclassid = -1;
				classname = "";
				classdescription = "";
				minimumallowedage = -1;
				defaultvaliditylength = -1;
				classfees = 0; // New placeholder: initialize out params if not found
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                // Log exception here
                isFound = false;
                // Ensure variables are initialized if an exception occurs
                 licenseclassid = -1;
				classname = "";
				classdescription = "";
				minimumallowedage = -1;
				defaultvaliditylength = -1;
				classfees = 0; // New placeholder: initialize out params on exception
            }
            finally
            {
                connection.Close();
            }
            return isFound;
        }
        
        // ===============================
        // INSERT METHOD (Returns New ID)
        // ===============================

        public static int InsertLicenseClasses(string ClassName,
			string ClassDescription,
			byte MinimumAllowedAge,
			byte DefaultValidityLength,
			decimal ClassFees)
        {
            int LicenseClassID = -1; // Default to -1 

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            // SQL to insert and retrieve the new Primary Key (SCOPE_IDENTITY())
            string query = @"INSERT INTO LicenseClasses (ClassName, ClassDescription, MinimumAllowedAge, DefaultValidityLength, ClassFees)
                             VALUES (@ClassName, @ClassDescription, @MinimumAllowedAge, @DefaultValidityLength, @ClassFees);
                             SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            if (ClassName != null && ClassName != "")
            command.Parameters.AddWithValue("@ClassName", ClassName);
        else
            command.Parameters.AddWithValue("@ClassName", System.DBNull.Value);

        if (ClassDescription != null && ClassDescription != "")
            command.Parameters.AddWithValue("@ClassDescription", ClassDescription);
        else
            command.Parameters.AddWithValue("@ClassDescription", System.DBNull.Value);
			command.Parameters.AddWithValue("@MinimumAllowedAge", MinimumAllowedAge);
			command.Parameters.AddWithValue("@DefaultValidityLength", DefaultValidityLength);
			command.Parameters.AddWithValue("@ClassFees", ClassFees);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                int insertedID = -1;
                if (result != null && int.TryParse(result.ToString(), out insertedID))
                {
                    LicenseClassID = insertedID;
                }
            }
            catch (Exception ex)
            {
                // Log exception here
            }
            finally
            {
                connection.Close();
            }

            return LicenseClassID;
        }

        // ===============================
        // UPDATE METHOD 
        // ===============================

        public static bool UpdateLicenseClasses(int LicenseClassID,			string ClassName,
			string ClassDescription,
			byte MinimumAllowedAge,
			byte DefaultValidityLength,
			decimal ClassFees)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"UPDATE LicenseClasses 
                             SET ClassName=@ClassName,
								 ClassDescription=@ClassDescription,
								 MinimumAllowedAge=@MinimumAllowedAge,
								 DefaultValidityLength=@DefaultValidityLength,
								 ClassFees=@ClassFees
                             WHERE LicenseClassID = @LicenseClassID";

            SqlCommand command = new SqlCommand(query, connection);


            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

        if (ClassName != null && ClassName != "")
            command.Parameters.AddWithValue("@ClassName", ClassName);
        else
            command.Parameters.AddWithValue("@ClassName", System.DBNull.Value);

        if (ClassDescription != null && ClassDescription != "")
            command.Parameters.AddWithValue("@ClassDescription", ClassDescription);
        else
            command.Parameters.AddWithValue("@ClassDescription", System.DBNull.Value);
			command.Parameters.AddWithValue("@MinimumAllowedAge", MinimumAllowedAge);
			command.Parameters.AddWithValue("@DefaultValidityLength", DefaultValidityLength);
			command.Parameters.AddWithValue("@ClassFees", ClassFees);

            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                // Log exception here
                return false;
            }
            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);
        }
        
        // ===============================
        // DELETE METHOD
        // ===============================

        public static bool DeleteLicenseClasses(int LicenseClassID)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"DELETE LicenseClasses WHERE LicenseClassID = @LicenseClassID";
            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                // Log exception here
            }
            finally
            {
                connection.Close();
            }
            return (rowsAffected > 0);
        }

        // ===============================
        // GET ALL METHOD
        // ===============================

        public static DataTable GetAllLicenseClasses()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            // Customize this query for joins/captions if needed
            string query = "SELECT * FROM LicenseClasses"; 

            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                    dt.Load(reader);
                reader.Close();
            }
            catch (Exception ex)
            {
                // Log exception here
            }
            finally
            {
                connection.Close();
            }
            return dt;
        }

        // ===============================
        // EXIST CHECK METHOD
        // ===============================

        public static bool IsLicenseClassesExist(int LicenseClassID)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "SELECT Found=1 FROM LicenseClasses WHERE LicenseClassID = @LicenseClassID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                isFound = reader.HasRows;
                reader.Close();
            }
            catch (Exception ex)
            {
                // Log exception here
                isFound = false;
            }
            finally
            {
                connection.Close();
            }
            return isFound;
        }


        public static DataTable SearchData(string ColumnName, string SearchValue, string Mode = "Anywhere")
        {
            DataTable LicenseClassesList = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = $@"
                        SELECT *
                        FROM LicenseClasses
                        WHERE
                            (@Mode = 'Anywhere'   AND {ColumnName} LIKE '%' + @SearchValue + '%')
                         OR (@Mode = 'StartsWith' AND {ColumnName} LIKE @SearchValue + '%')
                         OR (@Mode = 'EndsWith'   AND {ColumnName} LIKE '%' + @SearchValue)
                         OR (@Mode = 'ExactMatch' AND {ColumnName} = @SearchValue);";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.CommandType = CommandType.Text;

                        command.Parameters.AddWithValue("@SearchValue", SearchValue);
                        command.Parameters.AddWithValue("@Mode", Mode);

                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                LicenseClassesList.Load(reader);
                        }
                    }
                }
            }
            catch
            {
                // TODO: log error later
            }

            return LicenseClassesList;
        }





        // 
    }
}