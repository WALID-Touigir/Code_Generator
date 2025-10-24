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
        public static bool GetTestsInfoByID(int TestID,out int testid,
			out int testappointmentid,
			out bool testresult,
			out string notes,
			out int createdbyuserid)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            SqlCommand command = new SqlCommand("SELECT * FROM AppUsage WHERE TestID = @Id", connection); 
            command.Parameters.AddWithValue("@Id", TestID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    testid = (int)reader["TestID"];
					testappointmentid = (int)reader["TestAppointmentID"];
					testresult = (bool)reader["TestResult"];
					notes = (reader["Notes"] != DBNull.Value) ? (string)reader["Notes"] : "";
					createdbyuserid = (int)reader["CreatedByUserID"]; // Correct placeholder for assignment inside reader.Read()
                    
                    isFound = true;
                }
                else
                {
                    testid = -1;
				testappointmentid = -1;
				testresult = false;
				notes = "";
				createdbyuserid = -1; // New placeholder: initialize out params if not found
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                // Log exception here
                isFound = false;
                // Ensure variables are initialized if an exception occurs
                 testid = -1;
				testappointmentid = -1;
				testresult = false;
				notes = "";
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

        public static int InsertTests(int TestAppointmentID,
			bool TestResult,
			string Notes,
			int CreatedByUserID)
        {
            int TestID = -1; // Default to -1 

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            // SQL to insert and retrieve the new Primary Key (SCOPE_IDENTITY())
            string query = @"INSERT INTO Tests (TestAppointmentID, TestResult, Notes, CreatedByUserID)
                             VALUES (@TestAppointmentID, @TestResult, @Notes, @CreatedByUserID);
                             SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
			command.Parameters.AddWithValue("@TestResult", TestResult);

        if (Notes != null && Notes != "")
            command.Parameters.AddWithValue("@Notes", Notes);
        else
            command.Parameters.AddWithValue("@Notes", System.DBNull.Value);
			command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                int insertedID = -1;
                if (result != null && int.TryParse(result.ToString(), out insertedID))
                {
                    TestID = insertedID;
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

            return TestID;
        }

        // ===============================
        // UPDATE METHOD 
        // ===============================

        public static bool UpdateTests(int TestID,			int TestAppointmentID,
			bool TestResult,
			string Notes,
			int CreatedByUserID)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"UPDATE Tests 
                             SET TestAppointmentID=@TestAppointmentID,
								 TestResult=@TestResult,
								 Notes=@Notes,
								 CreatedByUserID=@CreatedByUserID
                             WHERE TestID = @TestID";

            SqlCommand command = new SqlCommand(query, connection);


            command.Parameters.AddWithValue("@TestID", TestID);
			command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
			command.Parameters.AddWithValue("@TestResult", TestResult);

        if (Notes != null && Notes != "")
            command.Parameters.AddWithValue("@Notes", Notes);
        else
            command.Parameters.AddWithValue("@Notes", System.DBNull.Value);
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

        public static bool DeleteTests(int TestID)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"DELETE Tests WHERE TestID = @TestID";
            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestID", TestID);

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

        public static DataTable GetAllTests()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            // Customize this query for joins/captions if needed
            string query = "SELECT * FROM Tests"; 

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

        public static bool IsTestsExist(int TestID)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "SELECT Found=1 FROM Tests WHERE TestID = @TestID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestID", TestID);

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
            DataTable TestsList = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = $@"
                        SELECT *
                        FROM Tests
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
                                TestsList.Load(reader);
                        }
                    }
                }
            }
            catch
            {
                // TODO: log error later
            }

            return TestsList;
        }





        // 
    }
}