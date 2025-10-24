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
        public static bool GetPeopleInfoByID(int PersonID,out int personid,
			out string nationalno,
			out string firstname,
			out string secondname,
			out string thirdname,
			out string lastname,
			out DateTime dateofbirth,
			out byte gendor,
			out string address,
			out string phone,
			out string email,
			out int nationalitycountryid,
			out string imagepath)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            SqlCommand command = new SqlCommand("SELECT * FROM AppUsage WHERE PersonID = @Id", connection); 
            command.Parameters.AddWithValue("@Id", PersonID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    personid = (int)reader["PersonID"];
					nationalno = (reader["NationalNo"] != DBNull.Value) ? (string)reader["NationalNo"] : "";
					firstname = (reader["FirstName"] != DBNull.Value) ? (string)reader["FirstName"] : "";
					secondname = (reader["SecondName"] != DBNull.Value) ? (string)reader["SecondName"] : "";
					thirdname = (reader["ThirdName"] != DBNull.Value) ? (string)reader["ThirdName"] : "";
					lastname = (reader["LastName"] != DBNull.Value) ? (string)reader["LastName"] : "";
					dateofbirth = (DateTime)reader["DateOfBirth"];
					gendor = (byte)reader["Gendor"];
					address = (reader["Address"] != DBNull.Value) ? (string)reader["Address"] : "";
					phone = (reader["Phone"] != DBNull.Value) ? (string)reader["Phone"] : "";
					email = (reader["Email"] != DBNull.Value) ? (string)reader["Email"] : "";
					nationalitycountryid = (int)reader["NationalityCountryID"];
					imagepath = (reader["ImagePath"] != DBNull.Value) ? (string)reader["ImagePath"] : ""; // Correct placeholder for assignment inside reader.Read()
                    
                    isFound = true;
                }
                else
                {
                    personid = -1;
				nationalno = "";
				firstname = "";
				secondname = "";
				thirdname = "";
				lastname = "";
				dateofbirth = DateTime.MinValue;
				gendor = -1;
				address = "";
				phone = "";
				email = "";
				nationalitycountryid = -1;
				imagepath = ""; // New placeholder: initialize out params if not found
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                // Log exception here
                isFound = false;
                // Ensure variables are initialized if an exception occurs
                 personid = -1;
				nationalno = "";
				firstname = "";
				secondname = "";
				thirdname = "";
				lastname = "";
				dateofbirth = DateTime.MinValue;
				gendor = -1;
				address = "";
				phone = "";
				email = "";
				nationalitycountryid = -1;
				imagepath = ""; // New placeholder: initialize out params on exception
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

        public static int InsertPeople(string NationalNo,
			string FirstName,
			string SecondName,
			string ThirdName,
			string LastName,
			DateTime DateOfBirth,
			byte Gendor,
			string Address,
			string Phone,
			string Email,
			int NationalityCountryID,
			string ImagePath)
        {
            int PersonID = -1; // Default to -1 

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            // SQL to insert and retrieve the new Primary Key (SCOPE_IDENTITY())
            string query = @"INSERT INTO People (NationalNo, FirstName, SecondName, ThirdName, LastName, DateOfBirth, Gendor, Address, Phone, Email, NationalityCountryID, ImagePath)
                             VALUES (@NationalNo, @FirstName, @SecondName, @ThirdName, @LastName, @DateOfBirth, @Gendor, @Address, @Phone, @Email, @NationalityCountryID, @ImagePath);
                             SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            if (NationalNo != null && NationalNo != "")
            command.Parameters.AddWithValue("@NationalNo", NationalNo);
        else
            command.Parameters.AddWithValue("@NationalNo", System.DBNull.Value);

        if (FirstName != null && FirstName != "")
            command.Parameters.AddWithValue("@FirstName", FirstName);
        else
            command.Parameters.AddWithValue("@FirstName", System.DBNull.Value);

        if (SecondName != null && SecondName != "")
            command.Parameters.AddWithValue("@SecondName", SecondName);
        else
            command.Parameters.AddWithValue("@SecondName", System.DBNull.Value);

        if (ThirdName != null && ThirdName != "")
            command.Parameters.AddWithValue("@ThirdName", ThirdName);
        else
            command.Parameters.AddWithValue("@ThirdName", System.DBNull.Value);

        if (LastName != null && LastName != "")
            command.Parameters.AddWithValue("@LastName", LastName);
        else
            command.Parameters.AddWithValue("@LastName", System.DBNull.Value);
			command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
			command.Parameters.AddWithValue("@Gendor", Gendor);

        if (Address != null && Address != "")
            command.Parameters.AddWithValue("@Address", Address);
        else
            command.Parameters.AddWithValue("@Address", System.DBNull.Value);

        if (Phone != null && Phone != "")
            command.Parameters.AddWithValue("@Phone", Phone);
        else
            command.Parameters.AddWithValue("@Phone", System.DBNull.Value);

        if (Email != null && Email != "")
            command.Parameters.AddWithValue("@Email", Email);
        else
            command.Parameters.AddWithValue("@Email", System.DBNull.Value);
			command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);

        if (ImagePath != null && ImagePath != "")
            command.Parameters.AddWithValue("@ImagePath", ImagePath);
        else
            command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                int insertedID = -1;
                if (result != null && int.TryParse(result.ToString(), out insertedID))
                {
                    PersonID = insertedID;
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

            return PersonID;
        }

        // ===============================
        // UPDATE METHOD 
        // ===============================

        public static bool UpdatePeople(int PersonID,			string NationalNo,
			string FirstName,
			string SecondName,
			string ThirdName,
			string LastName,
			DateTime DateOfBirth,
			byte Gendor,
			string Address,
			string Phone,
			string Email,
			int NationalityCountryID,
			string ImagePath)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"UPDATE People 
                             SET NationalNo=@NationalNo,
								 FirstName=@FirstName,
								 SecondName=@SecondName,
								 ThirdName=@ThirdName,
								 LastName=@LastName,
								 DateOfBirth=@DateOfBirth,
								 Gendor=@Gendor,
								 Address=@Address,
								 Phone=@Phone,
								 Email=@Email,
								 NationalityCountryID=@NationalityCountryID,
								 ImagePath=@ImagePath
                             WHERE PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);


            command.Parameters.AddWithValue("@PersonID", PersonID);

        if (NationalNo != null && NationalNo != "")
            command.Parameters.AddWithValue("@NationalNo", NationalNo);
        else
            command.Parameters.AddWithValue("@NationalNo", System.DBNull.Value);

        if (FirstName != null && FirstName != "")
            command.Parameters.AddWithValue("@FirstName", FirstName);
        else
            command.Parameters.AddWithValue("@FirstName", System.DBNull.Value);

        if (SecondName != null && SecondName != "")
            command.Parameters.AddWithValue("@SecondName", SecondName);
        else
            command.Parameters.AddWithValue("@SecondName", System.DBNull.Value);

        if (ThirdName != null && ThirdName != "")
            command.Parameters.AddWithValue("@ThirdName", ThirdName);
        else
            command.Parameters.AddWithValue("@ThirdName", System.DBNull.Value);

        if (LastName != null && LastName != "")
            command.Parameters.AddWithValue("@LastName", LastName);
        else
            command.Parameters.AddWithValue("@LastName", System.DBNull.Value);
			command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
			command.Parameters.AddWithValue("@Gendor", Gendor);

        if (Address != null && Address != "")
            command.Parameters.AddWithValue("@Address", Address);
        else
            command.Parameters.AddWithValue("@Address", System.DBNull.Value);

        if (Phone != null && Phone != "")
            command.Parameters.AddWithValue("@Phone", Phone);
        else
            command.Parameters.AddWithValue("@Phone", System.DBNull.Value);

        if (Email != null && Email != "")
            command.Parameters.AddWithValue("@Email", Email);
        else
            command.Parameters.AddWithValue("@Email", System.DBNull.Value);
			command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);

        if (ImagePath != null && ImagePath != "")
            command.Parameters.AddWithValue("@ImagePath", ImagePath);
        else
            command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);

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

        public static bool DeletePeople(int PersonID)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"DELETE People WHERE PersonID = @PersonID";
            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

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

        public static DataTable GetAllPeople()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            // Customize this query for joins/captions if needed
            string query = "SELECT * FROM People"; 

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

        public static bool IsPeopleExist(int PersonID)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "SELECT Found=1 FROM People WHERE PersonID = @PersonID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);

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
            DataTable PeopleList = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = $@"
                        SELECT *
                        FROM People
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
                                PeopleList.Load(reader);
                        }
                    }
                }
            }
            catch
            {
                // TODO: log error later
            }

            return PeopleList;
        }





        // 
    }
}