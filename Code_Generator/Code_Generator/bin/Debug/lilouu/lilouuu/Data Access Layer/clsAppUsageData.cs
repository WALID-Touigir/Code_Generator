using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace lilouuu_DataAccessLayer
{
    public class clslilouuuData
    {
        
        // ===============================
        // FIND METHOD BY PRIMARY KEY (using OUT parameters)
        // ===============================
        
        // This is the correct signature using the new OUT placeholder
        public static bool GetAppUsageInfoByID(int Id,out int id,
			out string appname,
			out string windowtitle,
			out DateTime starttime,
			out DateTime endtime,
			out int durationseconds,
			out bool isidle)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            SqlCommand command = new SqlCommand("SELECT * FROM AppUsage WHERE Id = @Id", connection); 
            command.Parameters.AddWithValue("@Id", Id);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    id = (int)reader["Id"];
					appname = (reader["AppName"] != DBNull.Value) ? (string)reader["AppName"] : "";
					windowtitle = (reader["WindowTitle"] != DBNull.Value) ? (string)reader["WindowTitle"] : "";
					starttime = (reader["StartTime"] != DBNull.Value) ? (DateTime)reader["StartTime"] : DateTime.MinValue;
					endtime = (reader["EndTime"] != DBNull.Value) ? (DateTime)reader["EndTime"] : DateTime.MinValue;
					durationseconds = (reader["DurationSeconds"] != DBNull.Value) ? (int)reader["DurationSeconds"] : -1;
					isidle = (reader["IsIdle"] != DBNull.Value) ? (bool)reader["IsIdle"] : false; // Correct placeholder for assignment inside reader.Read()
                    
                    isFound = true;
                }
                else
                {
                    id = -1;
				appname = "";
				windowtitle = "";
				starttime = DateTime.MinValue;
				endtime = DateTime.MinValue;
				durationseconds = -1;
				isidle = false; // New placeholder: initialize out params if not found
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                // Log exception here
                isFound = false;
                // Ensure variables are initialized if an exception occurs
                 id = -1;
				appname = "";
				windowtitle = "";
				starttime = DateTime.MinValue;
				endtime = DateTime.MinValue;
				durationseconds = -1;
				isidle = false; // New placeholder: initialize out params on exception
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

        public static int InsertAppUsage(string AppName,
			string WindowTitle,
			DateTime? StartTime,
			DateTime? EndTime,
			int? DurationSeconds,
			bool? IsIdle)
        {
            int Id = -1; // Default to -1 

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            // SQL to insert and retrieve the new Primary Key (SCOPE_IDENTITY())
            string query = @"INSERT INTO AppUsage (AppName, WindowTitle, StartTime, EndTime, DurationSeconds, IsIdle)
                             VALUES (@AppName, @WindowTitle, @StartTime, @EndTime, @DurationSeconds, @IsIdle);
                             SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            if (AppName != null && AppName != "")
            command.Parameters.AddWithValue("@AppName", AppName);
        else
            command.Parameters.AddWithValue("@AppName", System.DBNull.Value);

        if (WindowTitle != null && WindowTitle != "")
            command.Parameters.AddWithValue("@WindowTitle", WindowTitle);
        else
            command.Parameters.AddWithValue("@WindowTitle", System.DBNull.Value);

        if (StartTime.HasValue)
            command.Parameters.AddWithValue("@StartTime", StartTime.Value);
        else
            command.Parameters.AddWithValue("@StartTime", System.DBNull.Value);

        if (EndTime.HasValue)
            command.Parameters.AddWithValue("@EndTime", EndTime.Value);
        else
            command.Parameters.AddWithValue("@EndTime", System.DBNull.Value);

        if (DurationSeconds.HasValue)
            command.Parameters.AddWithValue("@DurationSeconds", DurationSeconds.Value);
        else
            command.Parameters.AddWithValue("@DurationSeconds", System.DBNull.Value);

        if (IsIdle.HasValue)
            command.Parameters.AddWithValue("@IsIdle", IsIdle.Value);
        else
            command.Parameters.AddWithValue("@IsIdle", System.DBNull.Value);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                int insertedID = -1;
                if (result != null && int.TryParse(result.ToString(), out insertedID))
                {
                    Id = insertedID;
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

            return Id;
        }

        // ===============================
        // UPDATE METHOD 
        // ===============================

        public static bool UpdateAppUsage(int Id,			string AppName,
			string WindowTitle,
			DateTime? StartTime,
			DateTime? EndTime,
			int? DurationSeconds,
			bool? IsIdle)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"UPDATE AppUsage 
                             SET AppName=@AppName,
								 WindowTitle=@WindowTitle,
								 StartTime=@StartTime,
								 EndTime=@EndTime,
								 DurationSeconds=@DurationSeconds,
								 IsIdle=@IsIdle
                             WHERE Id = @Id";

            SqlCommand command = new SqlCommand(query, connection);


            command.Parameters.AddWithValue("@Id", Id);

        if (AppName != null && AppName != "")
            command.Parameters.AddWithValue("@AppName", AppName);
        else
            command.Parameters.AddWithValue("@AppName", System.DBNull.Value);

        if (WindowTitle != null && WindowTitle != "")
            command.Parameters.AddWithValue("@WindowTitle", WindowTitle);
        else
            command.Parameters.AddWithValue("@WindowTitle", System.DBNull.Value);

        if (StartTime.HasValue)
            command.Parameters.AddWithValue("@StartTime", StartTime.Value);
        else
            command.Parameters.AddWithValue("@StartTime", System.DBNull.Value);

        if (EndTime.HasValue)
            command.Parameters.AddWithValue("@EndTime", EndTime.Value);
        else
            command.Parameters.AddWithValue("@EndTime", System.DBNull.Value);

        if (DurationSeconds.HasValue)
            command.Parameters.AddWithValue("@DurationSeconds", DurationSeconds.Value);
        else
            command.Parameters.AddWithValue("@DurationSeconds", System.DBNull.Value);

        if (IsIdle.HasValue)
            command.Parameters.AddWithValue("@IsIdle", IsIdle.Value);
        else
            command.Parameters.AddWithValue("@IsIdle", System.DBNull.Value);

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

        public static bool DeleteAppUsage(int Id)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"DELETE AppUsage WHERE Id = @Id";
            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Id", Id);

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

        public static DataTable GetAllAppUsage()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            // Customize this query for joins/captions if needed
            string query = "SELECT * FROM AppUsage"; 

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

        public static bool IsAppUsageExist(int Id)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "SELECT Found=1 FROM AppUsage WHERE Id = @Id";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", Id);

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
            DataTable AppUsageList = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = $@"
                        SELECT *
                        FROM AppUsage
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
                                AppUsageList.Load(reader);
                        }
                    }
                }
            }
            catch
            {
                // TODO: log error later
            }

            return AppUsageList;
        }





        // 
    }
}