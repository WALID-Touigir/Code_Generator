using System;
using System.Data;
using _DataAccessLayer;

namespace _BusinessLayer
{
    public class clsDrivers
    {
        //===============================
        // ENUMS & PRIVATE FIELDS
        //===============================

        private enum enMode { AddNew = 0, Update = 1 }
        private enMode Mode;

        //===============================
        // PUBLIC PROPERTIES (Generated from Columns)
        //===============================

   		public int DriverID { get; set; }
		public int PersonID { get; set; }
		private Lazy<clsPeople>  _PersonIDInfo;
		public clsPeople PersonIDInfo => _PersonIDInfo.Value;
		public int CreatedByUserID { get; set; }
		private Lazy<clsUsers>  _CreatedByUserIDInfo;
		public clsUsers CreatedByUserIDInfo => _CreatedByUserIDInfo.Value;
		public DateTime CreatedDate { get; set; }


        //===============================
        // CONSTRUCTORS
        //===============================

        // Empty constructor
        public clsDrivers()
        {
            this.DriverID = -1;
			this.PersonID = -1;
			this.CreatedByUserID = -1;
			this.CreatedDate = DateTime.MinValue;

            Mode = enMode.AddNew;
        }

        // Full constructor
        public clsDrivers(int driverid, int personid, int createdbyuserid, DateTime createddate)
        {
           this.DriverID = driverid;
			this.PersonID = personid;
			_PersonIDInfo = new Lazy<clsPeople>(() => clsPeople.Find(this.PersonID));

			this.CreatedByUserID = createdbyuserid;
			_CreatedByUserIDInfo = new Lazy<clsUsers>(() => clsUsers.Find(this.CreatedByUserID));

			this.CreatedDate = createddate;

            Mode = enMode.Update;
        }

        //===============================
        // FIND METHOD (STATIC)
        //===============================

       public static clsDrivers Find(int DriverID)
        {
            // Declare local variables to pass to the DAL via 'out'. 
            // The 'out' keyword guarantees the DAL method initializes them.
            int driverid;
			int personid;
			int createdbyuserid;
			DateTime createddate; // This placeholder now includes the 'out' keyword

            // Call DAL: Data is retrieved and assigned to the local variables (passed by OUT)
            if (clsDriversData.GetDriversInfoByID(DriverID, 
                out driverid, out personid, out createdbyuserid, out createddate))
            {
                // Constructor Call: Local variables are now passed by VALUE to the constructor
                return new clsDrivers(driverid, personid, createdbyuserid, createddate);
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
                    if (AddNewDrivers())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    break;

                case enMode.Update:
                    if (UpdateDrivers())
                        return true;
                    break;
            }

            return false;
        }

        // ===============================
        // PRIVATE ADD / UPDATE METHODS
        // ===============================

        private bool AddNewDrivers()
        {
            // Must map instance properties to DAL's parameters
            this.DriverID = clsDriversData.InsertDrivers(
            this.PersonID,
				this.CreatedByUserID,
				this.CreatedDate 
            );
            return (this.DriverID != -1);
        }

        private bool UpdateDrivers()
        {
            // Must map instance properties to DAL's parameters
            return clsDriversData.UpdateDrivers(
            this.DriverID,
				this.PersonID,
				this.CreatedByUserID,
				this.CreatedDate 
            );
        }


        // ===============================
        // DELETE METHOD
        // ===============================

        public bool Delete()
        {
            // Use the instance property value
            return clsDriversData.DeleteDrivers(this.DriverID);
        }

        //===============================
        // GetAll METHOD
        //===============================

        public static DataTable GetAllDrivers()
        {
            return clsDriversData.GetAllDrivers();
        }
        
        //===============================
        // OPTIONAL BIT CHECK METHODS
        //===============================

        

        public static bool  IsDriversExist(int DriverID)
        {
            return clsDriversData.IsDriversExist(DriverID);
        }


        
        public enum DriversColumn
         {
            DriverID,
PersonID,
CreatedByUserID,
CreatedDate,
         }


        public enum SearchMode
        {
            Anywhere,
            StartsWith,
            EndsWith,
            ExactMatch
        }
    

        public static DataTable SearchData(DriversColumn ChosenColumn, string SearchValue, SearchMode Mode = SearchMode.Anywhere)
        {
            if (string.IsNullOrWhiteSpace(SearchValue) || !SqlHelper.IsSafeInput(SearchValue))
                return new DataTable();


            return clsDriversData.SearchData(ChosenColumn.ToString(), SearchValue, Mode.ToString());
        }        
        





    }
}
