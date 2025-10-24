using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace DVLD_DataAccessLayer
{
    public class clsDVLDData
    {
        
        // ===============================
        // FIND METHOD BY PRIMARY KEY (using OUT parameters)
        // ===============================
        
        // This is the correct signature using the new OUT placeholder
        public static bool GetUsersInfoByID(int UserID,out int userid,
			out int personid,
			out string username,
			out string password,
			out bool isactive)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            SqlCommand command = new SqlCommand("SELECT * FROM AppUsage WHERE UserID = @Id", connection); 
            command.Parameters.AddWithValue("@Id", UserID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    userid = (int)reader["UserID"];
					personid = (int)reader["PersonID"];
					username = (reader["UserName"] != DBNull.Value) ? (string)reader["UserName"] : "";
					password = (reader["Password"] != DBNull.Value) ? (string)reader["Password"] : "";
					isactive = (bool)reader["IsActive"]; // Correct placeholder for assignment inside reader.Read()
                    
                    isFound = true;
                }
                else
                {
                    userid = -1;
				personid = -1;
				username = "";
				password = "";
				isactive = false; // New placeholder: initialize out params if not found
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                // Log exception here
                isFound = false;
                // Ensure variables are initialized if an exception occurs
                 userid = -1;
				personid = -1;
				username = "";
				password = "";
				isactive = false; // New placeholder: initialize out params on exception
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

        public static int InsertUsers(int PersonID,
			string UserName,
			string Password,
			bool IsActive)
        {
            int UserID = -1; // Default to -1 

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            // SQL to insert and retrieve the new Primary Key (SCOPE_IDENTITY())
            string query = @"INSERT INTO Users (PersonID, UserName, Password, IsActive)
                             VALUES (@PersonID, @UserName, @Password, @IsActive);
                             SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

        if (UserName != null && UserName != "")
            command.Parameters.AddWithValue("@UserName", UserName);
        else
            command.Parameters.AddWithValue("@UserName", System.DBNull.Value);

        if (Password != null && Password != "")
            command.Parameters.AddWithValue("@Password", Password);
        else
            command.Parameters.AddWithValue("@Password", System.DBNull.Value);
			command.Parameters.AddWithValue("@IsActive", IsActive);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                int insertedID = -1;
                if (result != null && int.TryParse(result.ToString(), out insertedID))
                {
                    UserID = insertedID;
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

            return UserID;
        }

        // ===============================
        // UPDATE METHOD 
        // ===============================

        public static bool UpdateUsers(int UserID,			int PersonID,
			string UserName,
			string Password,
			bool IsActive)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"UPDATE Users 
                             SET PersonID=@PersonID,
								 UserName=@UserName,
								 Password=@Password,
								 IsActive=@IsActive
                             WHERE UserID = @UserID";

            SqlCommand command = new SqlCommand(query, connection);


            command.Parameters.AddWithValue("@UserID", UserID);
			command.Parameters.AddWithValue("@PersonID", PersonID);

        if (UserName != null && UserName != "")
            command.Parameters.AddWithValue("@UserName", UserName);
        else
            command.Parameters.AddWithValue("@UserName", System.DBNull.Value);

        if (Password != null && Password != "")
            command.Parameters.AddWithValue("@Password", Password);
        else
            command.Parameters.AddWithValue("@Password", System.DBNull.Value);
			command.Parameters.AddWithValue("@IsActive", IsActive);

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

        public static bool DeleteUsers(int UserID)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"DELETE Users WHERE UserID = @UserID";
            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserID", UserID);

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

        public static DataTable GetAllUsers()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            // Customize this query for joins/captions if needed
            string query = "SELECT * FROM Users"; 

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

        public static bool IsUsersExist(int UserID)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "SELECT Found=1 FROM Users WHERE UserID = @UserID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserID", UserID);

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
            DataTable UsersList = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = $@"
                        SELECT *
                        FROM Users
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
                                UsersList.Load(reader);
                        }
                    }
                }
            }
            catch
            {
                // TODO: log error later
            }

            return UsersList;
        }





        // 
    }
}