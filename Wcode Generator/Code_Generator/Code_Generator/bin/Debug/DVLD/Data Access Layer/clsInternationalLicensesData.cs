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
        public static bool GetInternationalLicensesInfoByID(int InternationalLicenseID,out int internationallicenseid,
			out int applicationid,
			out int driverid,
			out int issuedusinglocallicenseid,
			out DateTime issuedate,
			out DateTime expirationdate,
			out bool isactive,
			out int createdbyuserid)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            SqlCommand command = new SqlCommand("SELECT * FROM AppUsage WHERE InternationalLicenseID = @Id", connection); 
            command.Parameters.AddWithValue("@Id", InternationalLicenseID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    internationallicenseid = (int)reader["InternationalLicenseID"];
					applicationid = (int)reader["ApplicationID"];
					driverid = (int)reader["DriverID"];
					issuedusinglocallicenseid = (int)reader["IssuedUsingLocalLicenseID"];
					issuedate = (DateTime)reader["IssueDate"];
					expirationdate = (DateTime)reader["ExpirationDate"];
					isactive = (bool)reader["IsActive"];
					createdbyuserid = (int)reader["CreatedByUserID"]; // Correct placeholder for assignment inside reader.Read()
                    
                    isFound = true;
                }
                else
                {
                    internationallicenseid = -1;
				applicationid = -1;
				driverid = -1;
				issuedusinglocallicenseid = -1;
				issuedate = DateTime.MinValue;
				expirationdate = DateTime.MinValue;
				isactive = false;
				createdbyuserid = -1; // New placeholder: initialize out params if not found
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                // Log exception here
                isFound = false;
                // Ensure variables are initialized if an exception occurs
                 internationallicenseid = -1;
				applicationid = -1;
				driverid = -1;
				issuedusinglocallicenseid = -1;
				issuedate = DateTime.MinValue;
				expirationdate = DateTime.MinValue;
				isactive = false;
				createdbyuserid = -1; // New placeholder: initialize out params on exception
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

        public static int InsertInternationalLicenses(int ApplicationID,
			int DriverID,
			int IssuedUsingLocalLicenseID,
			DateTime IssueDate,
			DateTime ExpirationDate,
			bool IsActive,
			int CreatedByUserID)
        {
            int InternationalLicenseID = -1; // Default to -1 

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            // SQL to insert and retrieve the new Primary Key (SCOPE_IDENTITY())
            string query = @"INSERT INTO InternationalLicenses (ApplicationID, DriverID, IssuedUsingLocalLicenseID, IssueDate, ExpirationDate, IsActive, CreatedByUserID)
                             VALUES (@ApplicationID, @DriverID, @IssuedUsingLocalLicenseID, @IssueDate, @ExpirationDate, @IsActive, @CreatedByUserID);
                             SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
			command.Parameters.AddWithValue("@DriverID", DriverID);
			command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", IssuedUsingLocalLicenseID);
			command.Parameters.AddWithValue("@IssueDate", IssueDate);
			command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
			command.Parameters.AddWithValue("@IsActive", IsActive);
			command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                int insertedID = -1;
                if (result != null && int.TryParse(result.ToString(), out insertedID))
                {
                    InternationalLicenseID = insertedID;
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

            return InternationalLicenseID;
        }

        // ===============================
        // UPDATE METHOD 
        // ===============================

        public static bool UpdateInternationalLicenses(int InternationalLicenseID,			int ApplicationID,
			int DriverID,
			int IssuedUsingLocalLicenseID,
			DateTime IssueDate,
			DateTime ExpirationDate,
			bool IsActive,
			int CreatedByUserID)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"UPDATE InternationalLicenses 
                             SET ApplicationID=@ApplicationID,
								 DriverID=@DriverID,
								 IssuedUsingLocalLicenseID=@IssuedUsingLocalLicenseID,
								 IssueDate=@IssueDate,
								 ExpirationDate=@ExpirationDate,
								 IsActive=@IsActive,
								 CreatedByUserID=@CreatedByUserID
                             WHERE InternationalLicenseID = @InternationalLicenseID";

            SqlCommand command = new SqlCommand(query, connection);


            command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);
			command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
			command.Parameters.AddWithValue("@DriverID", DriverID);
			command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", IssuedUsingLocalLicenseID);
			command.Parameters.AddWithValue("@IssueDate", IssueDate);
			command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
			command.Parameters.AddWithValue("@IsActive", IsActive);
			command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

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

        public static bool DeleteInternationalLicenses(int InternationalLicenseID)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"DELETE InternationalLicenses WHERE InternationalLicenseID = @InternationalLicenseID";
            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);

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

        public static DataTable GetAllInternationalLicenses()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            // Customize this query for joins/captions if needed
            string query = "SELECT * FROM InternationalLicenses"; 

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

        public static bool IsInternationalLicensesExist(int InternationalLicenseID)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "SELECT Found=1 FROM InternationalLicenses WHERE InternationalLicenseID = @InternationalLicenseID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);

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
            DataTable InternationalLicensesList = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = $@"
                        SELECT *
                        FROM InternationalLicenses
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
                                InternationalLicensesList.Load(reader);
                        }
                    }
                }
            }
            catch
            {
                // TODO: log error later
            }

            return InternationalLicensesList;
        }





        // 
    }
}