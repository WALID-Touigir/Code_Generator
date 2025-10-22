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
        public static bool GetApplicationsInfoByID(int ApplicationID,out int applicationid,
			out int applicantpersonid,
			out DateTime applicationdate,
			out int applicationtypeid,
			out byte applicationstatus,
			out DateTime laststatusdate,
			out decimal paidfees,
			out int createdbyuserid)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            SqlCommand command = new SqlCommand("SELECT * FROM AppUsage WHERE ApplicationID = @Id", connection); 
            command.Parameters.AddWithValue("@Id", ApplicationID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    applicationid = (int)reader["ApplicationID"];
					applicantpersonid = (int)reader["ApplicantPersonID"];
					applicationdate = (DateTime)reader["ApplicationDate"];
					applicationtypeid = (int)reader["ApplicationTypeID"];
					applicationstatus = (byte)reader["ApplicationStatus"];
					laststatusdate = (DateTime)reader["LastStatusDate"];
					paidfees = (decimal)reader["PaidFees"];
					createdbyuserid = (int)reader["CreatedByUserID"]; // Correct placeholder for assignment inside reader.Read()
                    
                    isFound = true;
                }
                else
                {
                    applicationid = -1;
				applicantpersonid = -1;
				applicationdate = DateTime.MinValue;
				applicationtypeid = -1;
				applicationstatus = -1;
				laststatusdate = DateTime.MinValue;
				paidfees = 0;
				createdbyuserid = -1; // New placeholder: initialize out params if not found
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                // Log exception here
                isFound = false;
                // Ensure variables are initialized if an exception occurs
                 applicationid = -1;
				applicantpersonid = -1;
				applicationdate = DateTime.MinValue;
				applicationtypeid = -1;
				applicationstatus = -1;
				laststatusdate = DateTime.MinValue;
				paidfees = 0;
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

        public static int InsertApplications(int ApplicantPersonID,
			DateTime ApplicationDate,
			int ApplicationTypeID,
			byte ApplicationStatus,
			DateTime LastStatusDate,
			decimal PaidFees,
			int CreatedByUserID)
        {
            int ApplicationID = -1; // Default to -1 

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            // SQL to insert and retrieve the new Primary Key (SCOPE_IDENTITY())
            string query = @"INSERT INTO Applications (ApplicantPersonID, ApplicationDate, ApplicationTypeID, ApplicationStatus, LastStatusDate, PaidFees, CreatedByUserID)
                             VALUES (@ApplicantPersonID, @ApplicationDate, @ApplicationTypeID, @ApplicationStatus, @LastStatusDate, @PaidFees, @CreatedByUserID);
                             SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
			command.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
			command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
			command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
			command.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
			command.Parameters.AddWithValue("@PaidFees", PaidFees);
			command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                int insertedID = -1;
                if (result != null && int.TryParse(result.ToString(), out insertedID))
                {
                    ApplicationID = insertedID;
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

            return ApplicationID;
        }

        // ===============================
        // UPDATE METHOD 
        // ===============================

        public static bool UpdateApplications(int ApplicationID,			int ApplicantPersonID,
			DateTime ApplicationDate,
			int ApplicationTypeID,
			byte ApplicationStatus,
			DateTime LastStatusDate,
			decimal PaidFees,
			int CreatedByUserID)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"UPDATE Applications 
                             SET ApplicantPersonID=@ApplicantPersonID,
								 ApplicationDate=@ApplicationDate,
								 ApplicationTypeID=@ApplicationTypeID,
								 ApplicationStatus=@ApplicationStatus,
								 LastStatusDate=@LastStatusDate,
								 PaidFees=@PaidFees,
								 CreatedByUserID=@CreatedByUserID
                             WHERE ApplicationID = @ApplicationID";

            SqlCommand command = new SqlCommand(query, connection);


            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
			command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
			command.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
			command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
			command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
			command.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
			command.Parameters.AddWithValue("@PaidFees", PaidFees);
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

        public static bool DeleteApplications(int ApplicationID)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"DELETE Applications WHERE ApplicationID = @ApplicationID";
            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

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

        public static DataTable GetAllApplications()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            // Customize this query for joins/captions if needed
            string query = "SELECT * FROM Applications"; 

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

        public static bool IsApplicationsExist(int ApplicationID)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "SELECT Found=1 FROM Applications WHERE ApplicationID = @ApplicationID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

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
            DataTable ApplicationsList = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = $@"
                        SELECT *
                        FROM Applications
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
                                ApplicationsList.Load(reader);
                        }
                    }
                }
            }
            catch
            {
                // TODO: log error later
            }

            return ApplicationsList;
        }





        // 
    }
}