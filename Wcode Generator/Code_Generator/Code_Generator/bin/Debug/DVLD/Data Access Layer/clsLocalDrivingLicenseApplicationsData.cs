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
        public static bool GetLocalDrivingLicenseApplicationsInfoByID(int LocalDrivingLicenseApplicationID,out int localdrivinglicenseapplicationid,
			out int applicationid,
			out int licenseclassid)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            SqlCommand command = new SqlCommand("SELECT * FROM AppUsage WHERE LocalDrivingLicenseApplicationID = @Id", connection); 
            command.Parameters.AddWithValue("@Id", LocalDrivingLicenseApplicationID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    localdrivinglicenseapplicationid = (int)reader["LocalDrivingLicenseApplicationID"];
					applicationid = (int)reader["ApplicationID"];
					licenseclassid = (int)reader["LicenseClassID"]; // Correct placeholder for assignment inside reader.Read()
                    
                    isFound = true;
                }
                else
                {
                    localdrivinglicenseapplicationid = -1;
				applicationid = -1;
				licenseclassid = -1; // New placeholder: initialize out params if not found
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                // Log exception here
                isFound = false;
                // Ensure variables are initialized if an exception occurs
                 localdrivinglicenseapplicationid = -1;
				applicationid = -1;
				licenseclassid = -1; // New placeholder: initialize out params on exception
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

        public static int InsertLocalDrivingLicenseApplications(int ApplicationID,
			int LicenseClassID)
        {
            int LocalDrivingLicenseApplicationID = -1; // Default to -1 

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            // SQL to insert and retrieve the new Primary Key (SCOPE_IDENTITY())
            string query = @"INSERT INTO LocalDrivingLicenseApplications (ApplicationID, LicenseClassID)
                             VALUES (@ApplicationID, @LicenseClassID);
                             SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
			command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                int insertedID = -1;
                if (result != null && int.TryParse(result.ToString(), out insertedID))
                {
                    LocalDrivingLicenseApplicationID = insertedID;
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

            return LocalDrivingLicenseApplicationID;
        }

        // ===============================
        // UPDATE METHOD 
        // ===============================

        public static bool UpdateLocalDrivingLicenseApplications(int LocalDrivingLicenseApplicationID,			int ApplicationID,
			int LicenseClassID)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"UPDATE LocalDrivingLicenseApplications 
                             SET ApplicationID=@ApplicationID,
								 LicenseClassID=@LicenseClassID
                             WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

            SqlCommand command = new SqlCommand(query, connection);


            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
			command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
			command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

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

        public static bool DeleteLocalDrivingLicenseApplications(int LocalDrivingLicenseApplicationID)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"DELETE LocalDrivingLicenseApplications WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";
            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

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

        public static DataTable GetAllLocalDrivingLicenseApplications()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            // Customize this query for joins/captions if needed
            string query = "SELECT * FROM LocalDrivingLicenseApplications"; 

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

        public static bool IsLocalDrivingLicenseApplicationsExist(int LocalDrivingLicenseApplicationID)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "SELECT Found=1 FROM LocalDrivingLicenseApplications WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

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
            DataTable LocalDrivingLicenseApplicationsList = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = $@"
                        SELECT *
                        FROM LocalDrivingLicenseApplications
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
                                LocalDrivingLicenseApplicationsList.Load(reader);
                        }
                    }
                }
            }
            catch
            {
                // TODO: log error later
            }

            return LocalDrivingLicenseApplicationsList;
        }





        // 
    }
}