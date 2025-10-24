using System;
using System.Data;
using DVLD_DataAccessLayer;

namespace DVLD_BusinessLayer
{
    public class clsDetainedLicenses
    {
        //===============================
        // ENUMS & PRIVATE FIELDS
        //===============================

        private enum enMode { AddNew = 0, Update = 1 }
        private enMode Mode;

        //===============================
        // PUBLIC PROPERTIES (Generated from Columns)
        //===============================

   		public int DetainID { get; set; }
		public int LicenseID { get; set; }
		private Lazy<clsLicenses>  _LicenseIDInfo;
		public clsLicenses LicenseIDInfo => _LicenseIDInfo.Value;
		public DateTime DetainDate { get; set; }
		public decimal FineFees { get; set; }
		public int CreatedByUserID { get; set; }
		private Lazy<clsUsers>  _CreatedByUserIDInfo;
		public clsUsers CreatedByUserIDInfo => _CreatedByUserIDInfo.Value;
		public bool IsReleased { get; set; }
		public DateTime? ReleaseDate { get; set; }
		public int? ReleasedByUserID { get; set; }
		private Lazy<clsUsers>  _ReleasedByUserIDInfo;
		public clsUsers ReleasedByUserIDInfo => _ReleasedByUserIDInfo.Value;
		public int? ReleaseApplicationID { get; set; }
		private Lazy<clsApplications>  _ReleaseApplicationIDInfo;
		public clsApplications ReleaseApplicationIDInfo => _ReleaseApplicationIDInfo.Value;


        //===============================
        // CONSTRUCTORS
        //===============================

        // Empty constructor
        public clsDetainedLicenses()
        {
            this.DetainID = -1;
			this.LicenseID = -1;
			this.DetainDate = DateTime.MinValue;
			this.FineFees = 0;
			this.CreatedByUserID = -1;
			this.IsReleased = false;
			this.ReleaseDate = DateTime.MinValue;
			this.ReleasedByUserID = -1;
			this.ReleaseApplicationID = -1;

            Mode = enMode.AddNew;
        }

        // Full constructor
        public clsDetainedLicenses(int detainid, int licenseid, DateTime detaindate, decimal finefees, int createdbyuserid, bool isreleased, DateTime releasedate, int releasedbyuserid, int releaseapplicationid)
        {
           this.DetainID = detainid;
			this.LicenseID = licenseid;
			_LicenseIDInfo = new Lazy<clsLicenses>(() => clsLicenses.Find(this.LicenseID));

			this.DetainDate = detaindate;
			this.FineFees = finefees;
			this.CreatedByUserID = createdbyuserid;
			_CreatedByUserIDInfo = new Lazy<clsUsers>(() => clsUsers.Find(this.CreatedByUserID));

			this.IsReleased = isreleased;
			this.ReleaseDate = releasedate;
			this.ReleasedByUserID = releasedbyuserid;
			_ReleasedByUserIDInfo = new Lazy<clsUsers>(() => clsUsers.Find(this.ReleasedByUserID));

			this.ReleaseApplicationID = releaseapplicationid;
			_ReleaseApplicationIDInfo = new Lazy<clsApplications>(() => clsApplications.Find(this.ReleaseApplicationID));

            Mode = enMode.Update;
        }

        //===============================
        // FIND METHOD (STATIC)
        //===============================

       public static clsDetainedLicenses Find(int DetainID)
        {
            // Declare local variables to pass to the DAL via 'out'. 
            // The 'out' keyword guarantees the DAL method initializes them.
            int detainid;
			int licenseid;
			DateTime detaindate;
			decimal finefees;
			int createdbyuserid;
			bool isreleased;
			DateTime releasedate;
			int releasedbyuserid;
			int releaseapplicationid; // This placeholder now includes the 'out' keyword

            // Call DAL: Data is retrieved and assigned to the local variables (passed by OUT)
            if (clsDetainedLicensesData.GetDetainedLicensesInfoByID(DetainID, 
                out detainid, out licenseid, out detaindate, out finefees, out createdbyuserid, out isreleased, out releasedate, out releasedbyuserid, out releaseapplicationid))
            {
                // Constructor Call: Local variables are now passed by VALUE to the constructor
                return new clsDetainedLicenses(detainid, licenseid, detaindate, finefees, createdbyuserid, isreleased, releasedate, releasedbyuserid, releaseapplicationid);
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
                    if (AddNewDetainedLicenses())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    break;

                case enMode.Update:
                    if (UpdateDetainedLicenses())
                        return true;
                    break;
            }

            return false;
        }

        // ===============================
        // PRIVATE ADD / UPDATE METHODS
        // ===============================

        private bool AddNewDetainedLicenses()
        {
            // Must map instance properties to DAL's parameters
            this.DetainID = clsDetainedLicensesData.InsertDetainedLicenses(
            this.LicenseID,
				this.DetainDate,
				this.FineFees,
				this.CreatedByUserID,
				this.IsReleased,
				this.ReleaseDate,
				this.ReleasedByUserID,
				this.ReleaseApplicationID 
            );
            return (this.DetainID != -1);
        }

        private bool UpdateDetainedLicenses()
        {
            // Must map instance properties to DAL's parameters
            return clsDetainedLicensesData.UpdateDetainedLicenses(
            this.DetainID,
				this.LicenseID,
				this.DetainDate,
				this.FineFees,
				this.CreatedByUserID,
				this.IsReleased,
				this.ReleaseDate,
				this.ReleasedByUserID,
				this.ReleaseApplicationID 
            );
        }


        // ===============================
        // DELETE METHOD
        // ===============================

        public bool Delete()
        {
            // Use the instance property value
            return clsDetainedLicensesData.DeleteDetainedLicenses(this.DetainID);
        }

        //===============================
        // GetAll METHOD
        //===============================

        public static DataTable GetAllDetainedLicenses()
        {
            return clsDetainedLicensesData.GetAllDetainedLicenses();
        }
        
        //===============================
        // OPTIONAL BIT CHECK METHODS
        //===============================

        

        public static bool  IsDetainedLicensesExist(int DetainID)
        {
            return clsDetainedLicensesData.IsDetainedLicensesExist(DetainID);
        }


        
        public enum DetainedLicensesColumn
         {
            DetainID,
LicenseID,
DetainDate,
FineFees,
CreatedByUserID,
IsReleased,
ReleaseDate,
ReleasedByUserID,
ReleaseApplicationID,
         }


        public enum SearchMode
        {
            Anywhere,
            StartsWith,
            EndsWith,
            ExactMatch
        }
    

        public static DataTable SearchData(DetainedLicensesColumn ChosenColumn, string SearchValue, SearchMode Mode = SearchMode.Anywhere)
        {
            if (string.IsNullOrWhiteSpace(SearchValue) || !SqlHelper.IsSafeInput(SearchValue))
                return new DataTable();


            return clsDetainedLicensesData.SearchData(ChosenColumn.ToString(), SearchValue, Mode.ToString());
        }        
        





    }
}
