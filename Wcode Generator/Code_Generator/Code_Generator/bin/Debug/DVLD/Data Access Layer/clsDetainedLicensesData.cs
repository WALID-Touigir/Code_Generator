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
        public static bool GetDetainedLicensesInfoByID(int DetainID,out int detainid,
			out int licenseid,
			out DateTime detaindate,
			out decimal finefees,
			out int createdbyuserid,
			out bool isreleased,
			out DateTime releasedate,
			out int releasedbyuserid,
			out int releaseapplicationid)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            SqlCommand command = new SqlCommand("SELECT * FROM AppUsage WHERE DetainID = @Id", connection); 
            command.Parameters.AddWithValue("@Id", DetainID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    detainid = (int)reader["DetainID"];
					licenseid = (int)reader["LicenseID"];
					detaindate = (DateTime)reader["DetainDate"];
					finefees = (decimal)reader["FineFees"];
					createdbyuserid = (int)reader["CreatedByUserID"];
					isreleased = (bool)reader["IsReleased"];
					releasedate = (reader["ReleaseDate"] != DBNull.Value) ? (DateTime)reader["ReleaseDate"] : DateTime.MinValue;
					releasedbyuserid = (reader["ReleasedByUserID"] != DBNull.Value) ? (int)reader["ReleasedByUserID"] : -1;
					releaseapplicationid = (reader["ReleaseApplicationID"] != DBNull.Value) ? (int)reader["ReleaseApplicationID"] : -1; // Correct placeholder for assignment inside reader.Read()
                    
                    isFound = true;
                }
                else
                {
                    detainid = -1;
				licenseid = -1;
				detaindate = DateTime.MinValue;
				finefees = 0;
				createdbyuserid = -1;
				isreleased = false;
				releasedate = DateTime.MinValue;
				releasedbyuserid = -1;
				releaseapplicationid = -1; // New placeholder: initialize out params if not found
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                // Log exception here
                isFound = false;
                // Ensure variables are initialized if an exception occurs
                 detainid = -1;
				licenseid = -1;
				detaindate = DateTime.MinValue;
				finefees = 0;
				createdbyuserid = -1;
				isreleased = false;
				releasedate = DateTime.MinValue;
				releasedbyuserid = -1;
				releaseapplicationid = -1; // New placeholder: initialize out params on exception
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

        public static int InsertDetainedLicenses(int LicenseID,
			DateTime DetainDate,
			decimal FineFees,
			int CreatedByUserID,
			bool IsReleased,
			DateTime? ReleaseDate,
			int? ReleasedByUserID,
			int? ReleaseApplicationID)
        {
            int DetainID = -1; // Default to -1 

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            // SQL to insert and retrieve the new Primary Key (SCOPE_IDENTITY())
            string query = @"INSERT INTO DetainedLicenses (LicenseID, DetainDate, FineFees, CreatedByUserID, IsReleased, ReleaseDate, ReleasedByUserID, ReleaseApplicationID)
                             VALUES (@LicenseID, @DetainDate, @FineFees, @CreatedByUserID, @IsReleased, @ReleaseDate, @ReleasedByUserID, @ReleaseApplicationID);
                             SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseID", LicenseID);
			command.Parameters.AddWithValue("@DetainDate", DetainDate);
			command.Parameters.AddWithValue("@FineFees", FineFees);
			command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
			command.Parameters.AddWithValue("@IsReleased", IsReleased);

        if (ReleaseDate.HasValue)
            command.Parameters.AddWithValue("@ReleaseDate", ReleaseDate.Value);
        else
            command.Parameters.AddWithValue("@ReleaseDate", System.DBNull.Value);

        if (ReleasedByUserID.HasValue)
            command.Parameters.AddWithValue("@ReleasedByUserID", ReleasedByUserID.Value);
        else
            command.Parameters.AddWithValue("@ReleasedByUserID", System.DBNull.Value);

        if (ReleaseApplicationID.HasValue)
            command.Parameters.AddWithValue("@ReleaseApplicationID", ReleaseApplicationID.Value);
        else
            command.Parameters.AddWithValue("@ReleaseApplicationID", System.DBNull.Value);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                int insertedID = -1;
                if (result != null && int.TryParse(result.ToString(), out insertedID))
                {
                    DetainID = insertedID;
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

            return DetainID;
        }

        // ===============================
        // UPDATE METHOD 
        // ===============================

        public static bool UpdateDetainedLicenses(int DetainID,			int LicenseID,
			DateTime DetainDate,
			decimal FineFees,
			int CreatedByUserID,
			bool IsReleased,
			DateTime? ReleaseDate,
			int? ReleasedByUserID,
			int? ReleaseApplicationID)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"UPDATE DetainedLicenses 
                             SET LicenseID=@LicenseID,
								 DetainDate=@DetainDate,
								 FineFees=@FineFees,
								 CreatedByUserID=@CreatedByUserID,
								 IsReleased=@IsReleased,
								 ReleaseDate=@ReleaseDate,
								 ReleasedByUserID=@ReleasedByUserID,
								 ReleaseApplicationID=@ReleaseApplicationID
                             WHERE DetainID = @DetainID";

            SqlCommand command = new SqlCommand(query, connection);


            command.Parameters.AddWithValue("@DetainID", DetainID);
			command.Parameters.AddWithValue("@LicenseID", LicenseID);
			command.Parameters.AddWithValue("@DetainDate", DetainDate);
			command.Parameters.AddWithValue("@FineFees", FineFees);
			command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
			command.Parameters.AddWithValue("@IsReleased", IsReleased);

        if (ReleaseDate.HasValue)
            command.Parameters.AddWithValue("@ReleaseDate", ReleaseDate.Value);
        else
            command.Parameters.AddWithValue("@ReleaseDate", System.DBNull.Value);

        if (ReleasedByUserID.HasValue)
            command.Parameters.AddWithValue("@ReleasedByUserID", ReleasedByUserID.Value);
        else
            command.Parameters.AddWithValue("@ReleasedByUserID", System.DBNull.Value);

        if (ReleaseApplicationID.HasValue)
            command.Parameters.AddWithValue("@ReleaseApplicationID", ReleaseApplicationID.Value);
        else
            command.Parameters.AddWithValue("@ReleaseApplicationID", System.DBNull.Value);

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

        public static bool DeleteDetainedLicenses(int DetainID)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"DELETE DetainedLicenses WHERE DetainID = @DetainID";
            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@DetainID", DetainID);

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

        public static DataTable GetAllDetainedLicenses()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            // Customize this query for joins/captions if needed
            string query = "SELECT * FROM DetainedLicenses"; 

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

        public static bool IsDetainedLicensesExist(int DetainID)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "SELECT Found=1 FROM DetainedLicenses WHERE DetainID = @DetainID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DetainID", DetainID);

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
            DataTable DetainedLicensesList = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = $@"
                        SELECT *
                        FROM DetainedLicenses
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
                                DetainedLicensesList.Load(reader);
                        }
                    }
                }
            }
            catch
            {
                // TODO: log error later
            }

            return DetainedLicensesList;
        }





        // 
    }
}