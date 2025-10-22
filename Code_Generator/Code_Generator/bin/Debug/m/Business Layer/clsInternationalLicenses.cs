using System;
using System.Data;
using _DataAccessLayer;

namespace _BusinessLayer
{
    public class clsInternationalLicenses
    {
        //===============================
        // ENUMS & PRIVATE FIELDS
        //===============================

        private enum enMode { AddNew = 0, Update = 1 }
        private enMode Mode;

        //===============================
        // PUBLIC PROPERTIES (Generated from Columns)
        //===============================

   		public int InternationalLicenseID { get; set; }
		public int ApplicationID { get; set; }
		private Lazy<clsApplications>  _ApplicationIDInfo;
		public clsApplications ApplicationIDInfo => _ApplicationIDInfo.Value;
		public int DriverID { get; set; }
		private Lazy<clsDrivers>  _DriverIDInfo;
		public clsDrivers DriverIDInfo => _DriverIDInfo.Value;
		public int IssuedUsingLocalLicenseID { get; set; }
		private Lazy<clsLicenses>  _IssuedUsingLocalLicenseIDInfo;
		public clsLicenses IssuedUsingLocalLicenseIDInfo => _IssuedUsingLocalLicenseIDInfo.Value;
		public DateTime IssueDate { get; set; }
		public DateTime ExpirationDate { get; set; }
		public bool IsActive { get; set; }
		public int CreatedByUserID { get; set; }
		private Lazy<clsUsers>  _CreatedByUserIDInfo;
		public clsUsers CreatedByUserIDInfo => _CreatedByUserIDInfo.Value;


        //===============================
        // CONSTRUCTORS
        //===============================

        // Empty constructor
        public clsInternationalLicenses()
        {
            this.InternationalLicenseID = -1;
			this.ApplicationID = -1;
			this.DriverID = -1;
			this.IssuedUsingLocalLicenseID = -1;
			this.IssueDate = DateTime.MinValue;
			this.ExpirationDate = DateTime.MinValue;
			this.IsActive = false;
			this.CreatedByUserID = -1;

            Mode = enMode.AddNew;
        }

        // Full constructor
        public clsInternationalLicenses(int internationallicenseid, int applicationid, int driverid, int issuedusinglocallicenseid, DateTime issuedate, DateTime expirationdate, bool isactive, int createdbyuserid)
        {
           this.InternationalLicenseID = internationallicenseid;
			this.ApplicationID = applicationid;
			_ApplicationIDInfo = new Lazy<clsApplications>(() => clsApplications.Find(this.ApplicationID));

			this.DriverID = driverid;
			_DriverIDInfo = new Lazy<clsDrivers>(() => clsDrivers.Find(this.DriverID));

			this.IssuedUsingLocalLicenseID = issuedusinglocallicenseid;
			_IssuedUsingLocalLicenseIDInfo = new Lazy<clsLicenses>(() => clsLicenses.Find(this.IssuedUsingLocalLicenseID));

			this.IssueDate = issuedate;
			this.ExpirationDate = expirationdate;
			this.IsActive = isactive;
			this.CreatedByUserID = createdbyuserid;
			_CreatedByUserIDInfo = new Lazy<clsUsers>(() => clsUsers.Find(this.CreatedByUserID));

            Mode = enMode.Update;
        }

        //===============================
        // FIND METHOD (STATIC)
        //===============================

       public static clsInternationalLicenses Find(int InternationalLicenseID)
        {
            // Declare local variables to pass to the DAL via 'out'. 
            // The 'out' keyword guarantees the DAL method initializes them.
            int internationallicenseid;
			int applicationid;
			int driverid;
			int issuedusinglocallicenseid;
			DateTime issuedate;
			DateTime expirationdate;
			bool isactive;
			int createdbyuserid; // This placeholder now includes the 'out' keyword

            // Call DAL: Data is retrieved and assigned to the local variables (passed by OUT)
            if (clsInternationalLicensesData.GetInternationalLicensesInfoByID(InternationalLicenseID, 
                out internationallicenseid, out applicationid, out driverid, out issuedusinglocallicenseid, out issuedate, out expirationdate, out isactive, out createdbyuserid))
            {
                // Constructor Call: Local variables are now passed by VALUE to the constructor
                return new clsInternationalLicenses(internationallicenseid, applicationid, driverid, issuedusinglocallicenseid, issuedate, expirationdate, isactive, createdbyuserid);
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
                    if (AddNewInternationalLicenses())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    break;

                case enMode.Update:
                    if (UpdateInternationalLicenses())
                        return true;
                    break;
            }

            return false;
        }

        // ===============================
        // PRIVATE ADD / UPDATE METHODS
        // ===============================

        private bool AddNewInternationalLicenses()
        {
            // Must map instance properties to DAL's parameters
            this.InternationalLicenseID = clsInternationalLicensesData.InsertInternationalLicenses(
            this.ApplicationID,
				this.DriverID,
				this.IssuedUsingLocalLicenseID,
				this.IssueDate,
				this.ExpirationDate,
				this.IsActive,
				this.CreatedByUserID 
            );
            return (this.InternationalLicenseID != -1);
        }

        private bool UpdateInternationalLicenses()
        {
            // Must map instance properties to DAL's parameters
            return clsInternationalLicensesData.UpdateInternationalLicenses(
            this.InternationalLicenseID,
				this.ApplicationID,
				this.DriverID,
				this.IssuedUsingLocalLicenseID,
				this.IssueDate,
				this.ExpirationDate,
				this.IsActive,
				this.CreatedByUserID 
            );
        }


        // ===============================
        // DELETE METHOD
        // ===============================

        public bool Delete()
        {
            // Use the instance property value
            return clsInternationalLicensesData.DeleteInternationalLicenses(this.InternationalLicenseID);
        }

        //===============================
        // GetAll METHOD
        //===============================

        public static DataTable GetAllInternationalLicenses()
        {
            return clsInternationalLicensesData.GetAllInternationalLicenses();
        }
        
        //===============================
        // OPTIONAL BIT CHECK METHODS
        //===============================

        

        public static bool  IsInternationalLicensesExist(int InternationalLicenseID)
        {
            return clsInternationalLicensesData.IsInternationalLicensesExist(InternationalLicenseID);
        }


        
        public enum InternationalLicensesColumn
         {
            InternationalLicenseID,
ApplicationID,
DriverID,
IssuedUsingLocalLicenseID,
IssueDate,
ExpirationDate,
IsActive,
CreatedByUserID,
         }


        public enum SearchMode
        {
            Anywhere,
            StartsWith,
            EndsWith,
            ExactMatch
        }
    

        public static DataTable SearchData(InternationalLicensesColumn ChosenColumn, string SearchValue, SearchMode Mode = SearchMode.Anywhere)
        {
            if (string.IsNullOrWhiteSpace(SearchValue) || !SqlHelper.IsSafeInput(SearchValue))
                return new DataTable();


            return clsInternationalLicensesData.SearchData(ChosenColumn.ToString(), SearchValue, Mode.ToString());
        }        
        





    }
}
