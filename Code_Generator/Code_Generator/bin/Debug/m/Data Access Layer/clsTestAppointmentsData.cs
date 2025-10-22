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
        public static bool GetTestAppointmentsInfoByID(int TestAppointmentID,out int testappointmentid,
			out int testtypeid,
			out int localdrivinglicenseapplicationid,
			out DateTime appointmentdate,
			out decimal paidfees,
			out int createdbyuserid,
			out bool islocked,
			out int retaketestapplicationid)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            SqlCommand command = new SqlCommand("SELECT * FROM AppUsage WHERE TestAppointmentID = @Id", connection); 
            command.Parameters.AddWithValue("@Id", TestAppointmentID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    testappointmentid = (int)reader["TestAppointmentID"];
					testtypeid = (int)reader["TestTypeID"];
					localdrivinglicenseapplicationid = (int)reader["LocalDrivingLicenseApplicationID"];
					appointmentdate = (DateTime)reader["AppointmentDate"];
					paidfees = (decimal)reader["PaidFees"];
					createdbyuserid = (int)reader["CreatedByUserID"];
					islocked = (bool)reader["IsLocked"];
					retaketestapplicationid = (reader["RetakeTestApplicationID"] != DBNull.Value) ? (int)reader["RetakeTestApplicationID"] : -1; // Correct placeholder for assignment inside reader.Read()
                    
                    isFound = true;
                }
                else
                {
                    testappointmentid = -1;
				testtypeid = -1;
				localdrivinglicenseapplicationid = -1;
				appointmentdate = DateTime.MinValue;
				paidfees = 0;
				createdbyuserid = -1;
				islocked = false;
				retaketestapplicationid = -1; // New placeholder: initialize out params if not found
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                // Log exception here
                isFound = false;
                // Ensure variables are initialized if an exception occurs
                 testappointmentid = -1;
				testtypeid = -1;
				localdrivinglicenseapplicationid = -1;
				appointmentdate = DateTime.MinValue;
				paidfees = 0;
				createdbyuserid = -1;
				islocked = false;
				retaketestapplicationid = -1; // New placeholder: initialize out params on exception
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

        public static int InsertTestAppointments(int TestTypeID,
			int LocalDrivingLicenseApplicationID,
			DateTime AppointmentDate,
			decimal PaidFees,
			int CreatedByUserID,
			bool IsLocked,
			int? RetakeTestApplicationID)
        {
            int TestAppointmentID = -1; // Default to -1 

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            // SQL to insert and retrieve the new Primary Key (SCOPE_IDENTITY())
            string query = @"INSERT INTO TestAppointments (TestTypeID, LocalDrivingLicenseApplicationID, AppointmentDate, PaidFees, CreatedByUserID, IsLocked, RetakeTestApplicationID)
                             VALUES (@TestTypeID, @LocalDrivingLicenseApplicationID, @AppointmentDate, @PaidFees, @CreatedByUserID, @IsLocked, @RetakeTestApplicationID);
                             SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
			command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
			command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
			command.Parameters.AddWithValue("@PaidFees", PaidFees);
			command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
			command.Parameters.AddWithValue("@IsLocked", IsLocked);

        if (RetakeTestApplicationID.HasValue)
            command.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeTestApplicationID.Value);
        else
            command.Parameters.AddWithValue("@RetakeTestApplicationID", System.DBNull.Value);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                int insertedID = -1;
                if (result != null && int.TryParse(result.ToString(), out insertedID))
                {
                    TestAppointmentID = insertedID;
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

            return TestAppointmentID;
        }

        // ===============================
        // UPDATE METHOD 
        // ===============================

        public static bool UpdateTestAppointments(int TestAppointmentID,			int TestTypeID,
			int LocalDrivingLicenseApplicationID,
			DateTime AppointmentDate,
			decimal PaidFees,
			int CreatedByUserID,
			bool IsLocked,
			int? RetakeTestApplicationID)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"UPDATE TestAppointments 
                             SET TestTypeID=@TestTypeID,
								 LocalDrivingLicenseApplicationID=@LocalDrivingLicenseApplicationID,
								 AppointmentDate=@AppointmentDate,
								 PaidFees=@PaidFees,
								 CreatedByUserID=@CreatedByUserID,
								 IsLocked=@IsLocked,
								 RetakeTestApplicationID=@RetakeTestApplicationID
                             WHERE TestAppointmentID = @TestAppointmentID";

            SqlCommand command = new SqlCommand(query, connection);


            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
			command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
			command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
			command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
			command.Parameters.AddWithValue("@PaidFees", PaidFees);
			command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
			command.Parameters.AddWithValue("@IsLocked", IsLocked);

        if (RetakeTestApplicationID.HasValue)
            command.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeTestApplicationID.Value);
        else
            command.Parameters.AddWithValue("@RetakeTestApplicationID", System.DBNull.Value);

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

        public static bool DeleteTestAppointments(int TestAppointmentID)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"DELETE TestAppointments WHERE TestAppointmentID = @TestAppointmentID";
            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

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

        public static DataTable GetAllTestAppointments()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            // Customize this query for joins/captions if needed
            string query = "SELECT * FROM TestAppointments"; 

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

        public static bool IsTestAppointmentsExist(int TestAppointmentID)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "SELECT Found=1 FROM TestAppointments WHERE TestAppointmentID = @TestAppointmentID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

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
            DataTable TestAppointmentsList = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = $@"
                        SELECT *
                        FROM TestAppointments
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
                                TestAppointmentsList.Load(reader);
                        }
                    }
                }
            }
            catch
            {
                // TODO: log error later
            }

            return TestAppointmentsList;
        }





        // 
    }
}