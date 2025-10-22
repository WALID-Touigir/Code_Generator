using System;
using System.Data;
using _DataAccessLayer;

namespace _BusinessLayer
{
    public class clsPeople
    {
        //===============================
        // ENUMS & PRIVATE FIELDS
        //===============================

        private enum enMode { AddNew = 0, Update = 1 }
        private enMode Mode;

        //===============================
        // PUBLIC PROPERTIES (Generated from Columns)
        //===============================

   		public int PersonID { get; set; }
		public string NationalNo { get; set; }
		public string FirstName { get; set; }
		public string SecondName { get; set; }
		public string ThirdName { get; set; }
		public string LastName { get; set; }
		public DateTime DateOfBirth { get; set; }
		public byte Gendor { get; set; }
		public string Address { get; set; }
		public string Phone { get; set; }
		public string Email { get; set; }
		public int NationalityCountryID { get; set; }
		private Lazy<clsCountries>  _NationalityCountryIDInfo;
		public clsCountries NationalityCountryIDInfo => _NationalityCountryIDInfo.Value;
		public string ImagePath { get; set; }


        //===============================
        // CONSTRUCTORS
        //===============================

        // Empty constructor
        public clsPeople()
        {
            this.PersonID = -1;
			this.NationalNo = "";
			this.FirstName = "";
			this.SecondName = "";
			this.ThirdName = "";
			this.LastName = "";
			this.DateOfBirth = DateTime.MinValue;
			this.Gendor = -1;
			this.Address = "";
			this.Phone = "";
			this.Email = "";
			this.NationalityCountryID = -1;
			this.ImagePath = "";

            Mode = enMode.AddNew;
        }

        // Full constructor
        public clsPeople(int personid, string nationalno, string firstname, string secondname, string thirdname, string lastname, DateTime dateofbirth, byte gendor, string address, string phone, string email, int nationalitycountryid, string imagepath)
        {
           this.PersonID = personid;
			this.NationalNo = nationalno;
			this.FirstName = firstname;
			this.SecondName = secondname;
			this.ThirdName = thirdname;
			this.LastName = lastname;
			this.DateOfBirth = dateofbirth;
			this.Gendor = gendor;
			this.Address = address;
			this.Phone = phone;
			this.Email = email;
			this.NationalityCountryID = nationalitycountryid;
			_NationalityCountryIDInfo = new Lazy<clsCountries>(() => clsCountries.Find(this.NationalityCountryID));

			this.ImagePath = imagepath;

            Mode = enMode.Update;
        }

        //===============================
        // FIND METHOD (STATIC)
        //===============================

       public static clsPeople Find(int PersonID)
        {
            // Declare local variables to pass to the DAL via 'out'. 
            // The 'out' keyword guarantees the DAL method initializes them.
            int personid;
			string nationalno;
			string firstname;
			string secondname;
			string thirdname;
			string lastname;
			DateTime dateofbirth;
			byte gendor;
			string address;
			string phone;
			string email;
			int nationalitycountryid;
			string imagepath; // This placeholder now includes the 'out' keyword

            // Call DAL: Data is retrieved and assigned to the local variables (passed by OUT)
            if (clsPeopleData.GetPeopleInfoByID(PersonID, 
                out personid, out nationalno, out firstname, out secondname, out thirdname, out lastname, out dateofbirth, out gendor, out address, out phone, out email, out nationalitycountryid, out imagepath))
            {
                // Constructor Call: Local variables are now passed by VALUE to the constructor
                return new clsPeople(personid, nationalno, firstname, secondname, thirdname, lastname, dateofbirth, gendor, address, phone, email, nationalitycountryid, imagepath);
            }
            else
            {
                return null;
            }
        }

        //===============================
        // SAVE METHOD (INSTANCE)
        //===============================

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (AddNewPeople())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    break;

                case enMode.Update:
                    if (UpdatePeople())
                        return true;
                    break;
            }

            return false;
        }

        // ===============================
        // PRIVATE ADD / UPDATE METHODS
        // ===============================

        private bool AddNewPeople()
        {
            // Must map instance properties to DAL's parameters
            this.PersonID = clsPeopleData.InsertPeople(
            this.NationalNo,
				this.FirstName,
				this.SecondName,
				this.ThirdName,
				this.LastName,
				this.DateOfBirth,
				this.Gendor,
				this.Address,
				this.Phone,
				this.Email,
				this.NationalityCountryID,
				this.ImagePath 
            );
            return (this.PersonID != -1);
        }

        private bool UpdatePeople()
        {
            // Must map instance properties to DAL's parameters
            return clsPeopleData.UpdatePeople(
            this.PersonID,
				this.NationalNo,
				this.FirstName,
				this.SecondName,
				this.ThirdName,
				this.LastName,
				this.DateOfBirth,
				this.Gendor,
				this.Address,
				this.Phone,
				this.Email,
				this.NationalityCountryID,
				this.ImagePath 
            );
        }


        // ===============================
        // DELETE METHOD
        // ===============================

        public bool Delete()
        {
            // Use the instance property value
            return clsPeopleData.DeletePeople(this.PersonID);
        }

        //===============================
        // GetAll METHOD
        //===============================

        public static DataTable GetAllPeople()
        {
            return clsPeopleData.GetAllPeople();
        }
        
        //===============================
        // OPTIONAL BIT CHECK METHODS
        //===============================

        

        public static bool  IsPeopleExist(int PersonID)
        {
            return clsPeopleData.IsPeopleExist(PersonID);
        }


        
        public enum PeopleColumn
         {
            PersonID,
NationalNo,
FirstName,
SecondName,
ThirdName,
LastName,
DateOfBirth,
Gendor,
Address,
Phone,
Email,
NationalityCountryID,
ImagePath,
         }


        public enum SearchMode
        {
            Anywhere,
            StartsWith,
            EndsWith,
            ExactMatch
        }
    

        public static DataTable SearchData(PeopleColumn ChosenColumn, string SearchValue, SearchMode Mode = SearchMode.Anywhere)
        {
            if (string.IsNullOrWhiteSpace(SearchValue) || !SqlHelper.IsSafeInput(SearchValue))
                return new DataTable();


            return clsPeopleData.SearchData(ChosenColumn.ToString(), SearchValue, Mode.ToString());
        }        
        





    }
}
