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
        public static bool GetErrorLogInfoByID(int ErrorID,out int errorid,
			out string errormessage,
			out string stacktrace,
			out DateTime timestamp,
			out string severity,
			out string additionalinfo)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            SqlCommand command = new SqlCommand("SELECT * FROM AppUsage WHERE ErrorID = @Id", connection); 
            command.Parameters.AddWithValue("@Id", ErrorID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    errorid = (int)reader["ErrorID"];
					errormessage = (reader["ErrorMessage"] != DBNull.Value) ? (string)reader["ErrorMessage"] : "";
					stacktrace = (reader["StackTrace"] != DBNull.Value) ? (string)reader["StackTrace"] : "";
					timestamp = (reader["Timestamp"] != DBNull.Value) ? (DateTime)reader["Timestamp"] : DateTime.MinValue;
					severity = (reader["Severity"] != DBNull.Value) ? (string)reader["Severity"] : "";
					additionalinfo = (reader["AdditionalInfo"] != DBNull.Value) ? (string)reader["AdditionalInfo"] : ""; // Correct placeholder for assignment inside reader.Read()
                    
                    isFound = true;
                }
                else
                {
                    errorid = -1;
				errormessage = "";
				stacktrace = "";
				timestamp = DateTime.MinValue;
				severity = "";
				additionalinfo = ""; // New placeholder: initialize out params if not found
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                // Log exception here
                isFound = false;
                // Ensure variables are initialized if an exception occurs
                 errorid = -1;
				errormessage = "";
				stacktrace = "";
				timestamp = DateTime.MinValue;
				severity = "";
				additionalinfo = ""; // New placeholder: initialize out params on exception
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

        public static int InsertErrorLog(string ErrorMessage,
			string StackTrace,
			DateTime? Timestamp,
			string Severity,
			string AdditionalInfo)
        {
            int ErrorID = -1; // Default to -1 

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            // SQL to insert and retrieve the new Primary Key (SCOPE_IDENTITY())
            string query = @"INSERT INTO ErrorLog (ErrorMessage, StackTrace, Timestamp, Severity, AdditionalInfo)
                             VALUES (@ErrorMessage, @StackTrace, @Timestamp, @Severity, @AdditionalInfo);
                             SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            if (ErrorMessage != null && ErrorMessage != "")
            command.Parameters.AddWithValue("@ErrorMessage", ErrorMessage);
        else
            command.Parameters.AddWithValue("@ErrorMessage", System.DBNull.Value);

        if (StackTrace != null && StackTrace != "")
            command.Parameters.AddWithValue("@StackTrace", StackTrace);
        else
            command.Parameters.AddWithValue("@StackTrace", System.DBNull.Value);

        if (Timestamp.HasValue)
            command.Parameters.AddWithValue("@Timestamp", Timestamp.Value);
        else
            command.Parameters.AddWithValue("@Timestamp", System.DBNull.Value);

        if (Severity != null && Severity != "")
            command.Parameters.AddWithValue("@Severity", Severity);
        else
            command.Parameters.AddWithValue("@Severity", System.DBNull.Value);

        if (AdditionalInfo != null && AdditionalInfo != "")
            command.Parameters.AddWithValue("@AdditionalInfo", AdditionalInfo);
        else
            command.Parameters.AddWithValue("@AdditionalInfo", System.DBNull.Value);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                int insertedID = -1;
                if (result != null && int.TryParse(result.ToString(), out insertedID))
                {
                    ErrorID = insertedID;
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

            return ErrorID;
        }

        // ===============================
        // UPDATE METHOD 
        // ===============================

        public static bool UpdateErrorLog(int ErrorID,			string ErrorMessage,
			string StackTrace,
			DateTime? Timestamp,
			string Severity,
			string AdditionalInfo)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"UPDATE ErrorLog 
                             SET ErrorMessage=@ErrorMessage,
								 StackTrace=@StackTrace,
								 Timestamp=@Timestamp,
								 Severity=@Severity,
								 AdditionalInfo=@AdditionalInfo
                             WHERE ErrorID = @ErrorID";

            SqlCommand command = new SqlCommand(query, connection);


            command.Parameters.AddWithValue("@ErrorID", ErrorID);

        if (ErrorMessage != null && ErrorMessage != "")
            command.Parameters.AddWithValue("@ErrorMessage", ErrorMessage);
        else
            command.Parameters.AddWithValue("@ErrorMessage", System.DBNull.Value);

        if (StackTrace != null && StackTrace != "")
            command.Parameters.AddWithValue("@StackTrace", StackTrace);
        else
            command.Parameters.AddWithValue("@StackTrace", System.DBNull.Value);

        if (Timestamp.HasValue)
            command.Parameters.AddWithValue("@Timestamp", Timestamp.Value);
        else
            command.Parameters.AddWithValue("@Timestamp", System.DBNull.Value);

        if (Severity != null && Severity != "")
            command.Parameters.AddWithValue("@Severity", Severity);
        else
            command.Parameters.AddWithValue("@Severity", System.DBNull.Value);

        if (AdditionalInfo != null && AdditionalInfo != "")
            command.Parameters.AddWithValue("@AdditionalInfo", AdditionalInfo);
        else
            command.Parameters.AddWithValue("@AdditionalInfo", System.DBNull.Value);

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

        public static bool DeleteErrorLog(int ErrorID)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"DELETE ErrorLog WHERE ErrorID = @ErrorID";
            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ErrorID", ErrorID);

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

        public static DataTable GetAllErrorLog()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            // Customize this query for joins/captions if needed
            string query = "SELECT * FROM ErrorLog"; 

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

        public static bool IsErrorLogExist(int ErrorID)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "SELECT Found=1 FROM ErrorLog WHERE ErrorID = @ErrorID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ErrorID", ErrorID);

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
            DataTable ErrorLogList = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = $@"
                        SELECT *
                        FROM ErrorLog
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
                                ErrorLogList.Load(reader);
                        }
                    }
                }
            }
            catch
            {
                // TODO: log error later
            }

            return ErrorLogList;
        }





        // 
    }
}