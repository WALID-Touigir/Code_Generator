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
        public static bool GetLicensesInfoByID(int LicenseID,out int licenseid,
			out int applicationid,
			out int driverid,
			out int licenseclass,
			out DateTime issuedate,
			out DateTime expirationdate,
			out string notes,
			out decimal paidfees,
			out bool isactive,
			out byte issuereason,
			out int createdbyuserid)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            SqlCommand command = new SqlCommand("SELECT * FROM AppUsage WHERE LicenseID = @Id", connection); 
            command.Parameters.AddWithValue("@Id", LicenseID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    licenseid = (int)reader["LicenseID"];
					applicationid = (int)reader["ApplicationID"];
					driverid = (int)reader["DriverID"];
					licenseclass = (int)reader["LicenseClass"];
					issuedate = (DateTime)reader["IssueDate"];
					expirationdate = (DateTime)reader["ExpirationDate"];
					notes = (reader["Notes"] != DBNull.Value) ? (string)reader["Notes"] : "";
					paidfees = (decimal)reader["PaidFees"];
					isactive = (bool)reader["IsActive"];
					issuereason = (byte)reader["IssueReason"];
					createdbyuserid = (int)reader["CreatedByUserID"]; // Correct placeholder for assignment inside reader.Read()
                    
                    isFound = true;
                }
                else
                {
                    licenseid = -1;
				applicationid = -1;
				driverid = -1;
				licenseclass = -1;
				issuedate = DateTime.MinValue;
				expirationdate = DateTime.MinValue;
				notes = "";
				paidfees = 0;
				isactive = false;
				issuereason = -1;
				createdbyuserid = -1; // New placeholder: initialize out params if not found
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                // Log exception here
                isFound = false;
                // Ensure variables are initialized if an exception occurs
                 licenseid = -1;
				applicationid = -1;
				driverid = -1;
				licenseclass = -1;
				issuedate = DateTime.MinValue;
				expirationdate = DateTime.MinValue;
				notes = "";
				paidfees = 0;
				isactive = false;
				issuereason = -1;
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

        public static int InsertLicenses(int ApplicationID,
			int DriverID,
			int LicenseClass,
			DateTime IssueDate,
			DateTime ExpirationDate,
			string Notes,
			decimal PaidFees,
			bool IsActive,
			byte IssueReason,
			int CreatedByUserID)
        {
            int LicenseID = -1; // Default to -1 

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            // SQL to insert and retrieve the new Primary Key (SCOPE_IDENTITY())
            string query = @"INSERT INTO Licenses (ApplicationID, DriverID, LicenseClass, IssueDate, ExpirationDate, Notes, PaidFees, IsActive, IssueReason, CreatedByUserID)
                             VALUES (@ApplicationID, @DriverID, @LicenseClass, @IssueDate, @ExpirationDate, @Notes, @PaidFees, @IsActive, @IssueReason, @CreatedByUserID);
                             SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
			command.Parameters.AddWithValue("@DriverID", DriverID);
			command.Parameters.AddWithValue("@LicenseClass", LicenseClass);
			command.Parameters.AddWithValue("@IssueDate", IssueDate);
			command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);

        if (Notes != null && Notes != "")
            command.Parameters.AddWithValue("@Notes", Notes);
        else
            command.Parameters.AddWithValue("@Notes", System.DBNull.Value);
			command.Parameters.AddWithValue("@PaidFees", PaidFees);
			command.Parameters.AddWithValue("@IsActive", IsActive);
			command.Parameters.AddWithValue("@IssueReason", IssueReason);
			command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                int insertedID = -1;
                if (result != null && int.TryParse(result.ToString(), out insertedID))
                {
                    LicenseID = insertedID;
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

            return LicenseID;
        }

        // ===============================
        // UPDATE METHOD 
        // ===============================

        public static bool UpdateLicenses(int LicenseID,			int ApplicationID,
			int DriverID,
			int LicenseClass,
			DateTime IssueDate,
			DateTime ExpirationDate,
			string Notes,
			decimal PaidFees,
			bool IsActive,
			byte IssueReason,
			int CreatedByUserID)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"UPDATE Licenses 
                             SET ApplicationID=@ApplicationID,
								 DriverID=@DriverID,
								 LicenseClass=@LicenseClass,
								 IssueDate=@IssueDate,
								 ExpirationDate=@ExpirationDate,
								 Notes=@Notes,
								 PaidFees=@PaidFees,
								 IsActive=@IsActive,
								 IssueReason=@IssueReason,
								 CreatedByUserID=@CreatedByUserID
                             WHERE LicenseID = @LicenseID";

            SqlCommand command = new SqlCommand(query, connection);


            command.Parameters.AddWithValue("@LicenseID", LicenseID);
			command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
			command.Parameters.AddWithValue("@DriverID", DriverID);
			command.Parameters.AddWithValue("@LicenseClass", LicenseClass);
			command.Parameters.AddWithValue("@IssueDate", IssueDate);
			command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);

        if (Notes != null && Notes != "")
            command.Parameters.AddWithValue("@Notes", Notes);
        else
            command.Parameters.AddWithValue("@Notes", System.DBNull.Value);
			command.Parameters.AddWithValue("@PaidFees", PaidFees);
			command.Parameters.AddWithValue("@IsActive", IsActive);
			command.Parameters.AddWithValue("@IssueReason", IssueReason);
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

        public static bool DeleteLicenses(int LicenseID)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"DELETE Licenses WHERE LicenseID = @LicenseID";
            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseID", LicenseID);

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

        public static DataTable GetAllLicenses()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            // Customize this query for joins/captions if needed
            string query = "SELECT * FROM Licenses"; 

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

        public static bool IsLicensesExist(int LicenseID)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "SELECT Found=1 FROM Licenses WHERE LicenseID = @LicenseID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LicenseID", LicenseID);

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
            DataTable LicensesList = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = $@"
                        SELECT *
                        FROM Licenses
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
                                LicensesList.Load(reader);
                        }
                    }
                }
            }
            catch
            {
                // TODO: log error later
            }

            return LicensesList;
        }





        // 
    }
}