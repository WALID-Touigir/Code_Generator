using System;
using System.Data;
using DVLD_DataAccessLayer;

namespace DVLD_BusinessLayer
{
    public class clsUsers
    {
        //===============================
        // ENUMS & PRIVATE FIELDS
        //===============================

        private enum enMode { AddNew = 0, Update = 1 }
        private enMode Mode;

        //===============================
        // PUBLIC PROPERTIES (Generated from Columns)
        //===============================

   		public int UserID { get; set; }
		public int PersonID { get; set; }
		private Lazy<clsPeople>  _PersonIDInfo;
		public clsPeople PersonIDInfo => _PersonIDInfo.Value;
		public string UserName { get; set; }
		public string Password { get; set; }
		public bool IsActive { get; set; }


        //===============================
        // CONSTRUCTORS
        //===============================

        // Empty constructor
        public clsUsers()
        {
            this.UserID = -1;
			this.PersonID = -1;
			this.UserName = "";
			this.Password = "";
			this.IsActive = false;

            Mode = enMode.AddNew;
        }

        // Full constructor
        public clsUsers(int userid, int personid, string username, string password, bool isactive)
        {
           this.UserID = userid;
			this.PersonID = personid;
			_PersonIDInfo = new Lazy<clsPeople>(() => clsPeople.Find(this.PersonID));

			this.UserName = username;
			this.Password = password;
			this.IsActive = isactive;

            Mode = enMode.Update;
        }

        //===============================
        // FIND METHOD (STATIC)
        //===============================

       public static clsUsers Find(int UserID)
        {
            // Declare local variables to pass to the DAL via 'out'. 
            // The 'out' keyword guarantees the DAL method initializes them.
            int userid;
			int personid;
			string username;
			string password;
			bool isactive; // This placeholder now includes the 'out' keyword

            // Call DAL: Data is retrieved and assigned to the local variables (passed by OUT)
            if (clsUsersData.GetUsersInfoByID(UserID, 
                out userid, out personid, out username, out password, out isactive))
            {
                // Constructor Call: Local variables are now passed by VALUE to the constructor
                return new clsUsers(userid, personid, username, password, isactive);
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
                    if (AddNewUsers())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    break;

                case enMode.Update:
                    if (UpdateUsers())
                        return true;
                    break;
            }

            return false;
        }

        // ===============================
        // PRIVATE ADD / UPDATE METHODS
        // ===============================

        private bool AddNewUsers()
        {
            // Must map instance properties to DAL's parameters
            this.UserID = clsUsersData.InsertUsers(
            this.PersonID,
				this.UserName,
				this.Password,
				this.IsActive 
            );
            return (this.UserID != -1);
        }

        private bool UpdateUsers()
        {
            // Must map instance properties to DAL's parameters
            return clsUsersData.UpdateUsers(
            this.UserID,
				this.PersonID,
				this.UserName,
				this.Password,
				this.IsActive 
            );
        }


        // ===============================
        // DELETE METHOD
        // ===============================

        public bool Delete()
        {
            // Use the instance property value
            return clsUsersData.DeleteUsers(this.UserID);
        }

        //===============================
        // GetAll METHOD
        //===============================

        public static DataTable GetAllUsers()
        {
            return clsUsersData.GetAllUsers();
        }
        
        //===============================
        // OPTIONAL BIT CHECK METHODS
        //===============================

        

        public static bool  IsUsersExist(int UserID)
        {
            return clsUsersData.IsUsersExist(UserID);
        }


        
        public enum UsersColumn
         {
            UserID,
PersonID,
UserName,
Password,
IsActive,
         }


        public enum SearchMode
        {
            Anywhere,
            StartsWith,
            EndsWith,
            ExactMatch
        }
    

        public static DataTable SearchData(UsersColumn ChosenColumn, string SearchValue, SearchMode Mode = SearchMode.Anywhere)
        {
            if (string.IsNullOrWhiteSpace(SearchValue) || !SqlHelper.IsSafeInput(SearchValue))
                return new DataTable();


            return clsUsersData.SearchData(ChosenColumn.ToString(), SearchValue, Mode.ToString());
        }        
        





    }
}
