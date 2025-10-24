using System;
using System.Data;
using DVLD_DataAccessLayer;

namespace DVLD_BusinessLayer
{
    public class clsLicenses
    {
        //===============================
        // ENUMS & PRIVATE FIELDS
        //===============================

        private enum enMode { AddNew = 0, Update = 1 }
        private enMode Mode;

        //===============================
        // PUBLIC PROPERTIES (Generated from Columns)
        //===============================

   		public int LicenseID { get; set; }
		public int ApplicationID { get; set; }
		private Lazy<clsApplications>  _ApplicationIDInfo;
		public clsApplications ApplicationIDInfo => _ApplicationIDInfo.Value;
		public int DriverID { get; set; }
		private Lazy<clsDrivers>  _DriverIDInfo;
		public clsDrivers DriverIDInfo => _DriverIDInfo.Value;
		public int LicenseClass { get; set; }
		private Lazy<clsLicenseClasses>  _LicenseClassInfo;
		public clsLicenseClasses LicenseClassInfo => _LicenseClassInfo.Value;
		public DateTime IssueDate { get; set; }
		public DateTime ExpirationDate { get; set; }
		public string Notes { get; set; }
		public decimal PaidFees { get; set; }
		public bool IsActive { get; set; }
		public byte IssueReason { get; set; }
		public int CreatedByUserID { get; set; }
		private Lazy<clsUsers>  _CreatedByUserIDInfo;
		public clsUsers CreatedByUserIDInfo => _CreatedByUserIDInfo.Value;


        //===============================
        // CONSTRUCTORS
        //===============================

        // Empty constructor
        public clsLicenses()
        {
            this.LicenseID = -1;
			this.ApplicationID = -1;
			this.DriverID = -1;
			this.LicenseClass = -1;
			this.IssueDate = DateTime.MinValue;
			this.ExpirationDate = DateTime.MinValue;
			this.Notes = "";
			this.PaidFees = 0;
			this.IsActive = false;
			this.IssueReason = -1;
			this.CreatedByUserID = -1;

            Mode = enMode.AddNew;
        }

        // Full constructor
        public clsLicenses(int licenseid, int applicationid, int driverid, int licenseclass, DateTime issuedate, DateTime expirationdate, string notes, decimal paidfees, bool isactive, byte issuereason, int createdbyuserid)
        {
           this.LicenseID = licenseid;
			this.ApplicationID = applicationid;
			_ApplicationIDInfo = new Lazy<clsApplications>(() => clsApplications.Find(this.ApplicationID));

			this.DriverID = driverid;
			_DriverIDInfo = new Lazy<clsDrivers>(() => clsDrivers.Find(this.DriverID));

			this.LicenseClass = licenseclass;
			_LicenseClassInfo = new Lazy<clsLicenseClasses>(() => clsLicenseClasses.Find(this.LicenseClass));

			this.IssueDate = issuedate;
			this.ExpirationDate = expirationdate;
			this.Notes = notes;
			this.PaidFees = paidfees;
			this.IsActive = isactive;
			this.IssueReason = issuereason;
			this.CreatedByUserID = createdbyuserid;
			_CreatedByUserIDInfo = new Lazy<clsUsers>(() => clsUsers.Find(this.CreatedByUserID));

            Mode = enMode.Update;
        }

        //===============================
        // FIND METHOD (STATIC)
        //===============================

       public static clsLicenses Find(int LicenseID)
        {
            // Declare local variables to pass to the DAL via 'out'. 
            // The 'out' keyword guarantees the DAL method initializes them.
            int licenseid;
			int applicationid;
			int driverid;
			int licenseclass;
			DateTime issuedate;
			DateTime expirationdate;
			string notes;
			decimal paidfees;
			bool isactive;
			byte issuereason;
			int createdbyuserid; // This placeholder now includes the 'out' keyword

            // Call DAL: Data is retrieved and assigned to the local variables (passed by OUT)
            if (clsLicensesData.GetLicensesInfoByID(LicenseID, 
                out licenseid, out applicationid, out driverid, out licenseclass, out issuedate, out expirationdate, out notes, out paidfees, out isactive, out issuereason, out createdbyuserid))
            {
                // Constructor Call: Local variables are now passed by VALUE to the constructor
                return new clsLicenses(licenseid, applicationid, driverid, licenseclass, issuedate, expirationdate, notes, paidfees, isactive, issuereason, createdbyuserid);
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
                    if (AddNewLicenses())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    break;

                case enMode.Update:
                    if (UpdateLicenses())
                        return true;
                    break;
            }

            return false;
        }

        // ===============================
        // PRIVATE ADD / UPDATE METHODS
        // ===============================

        private bool AddNewLicenses()
        {
            // Must map instance properties to DAL's parameters
            this.LicenseID = clsLicensesData.InsertLicenses(
            this.ApplicationID,
				this.DriverID,
				this.LicenseClass,
				this.IssueDate,
				this.ExpirationDate,
				this.Notes,
				this.PaidFees,
				this.IsActive,
				this.IssueReason,
				this.CreatedByUserID 
            );
            return (this.LicenseID != -1);
        }

        private bool UpdateLicenses()
        {
            // Must map instance properties to DAL's parameters
            return clsLicensesData.UpdateLicenses(
            this.LicenseID,
				this.ApplicationID,
				this.DriverID,
				this.LicenseClass,
				this.IssueDate,
				this.ExpirationDate,
				this.Notes,
				this.PaidFees,
				this.IsActive,
				this.IssueReason,
				this.CreatedByUserID 
            );
        }


        // ===============================
        // DELETE METHOD
        // ===============================

        public bool Delete()
        {
            // Use the instance property value
            return clsLicensesData.DeleteLicenses(this.LicenseID);
        }

        //===============================
        // GetAll METHOD
        //===============================

        public static DataTable GetAllLicenses()
        {
            return clsLicensesData.GetAllLicenses();
        }
        
        //===============================
        // OPTIONAL BIT CHECK METHODS
        //===============================

        

        public static bool  IsLicensesExist(int LicenseID)
        {
            return clsLicensesData.IsLicensesExist(LicenseID);
        }


        
        public enum LicensesColumn
         {
            LicenseID,
ApplicationID,
DriverID,
LicenseClass,
IssueDate,
ExpirationDate,
Notes,
PaidFees,
IsActive,
IssueReason,
CreatedByUserID,
         }


        public enum SearchMode
        {
            Anywhere,
            StartsWith,
            EndsWith,
            ExactMatch
        }
    

        public static DataTable SearchData(LicensesColumn ChosenColumn, string SearchValue, SearchMode Mode = SearchMode.Anywhere)
        {
            if (string.IsNullOrWhiteSpace(SearchValue) || !SqlHelper.IsSafeInput(SearchValue))
                return new DataTable();


            return clsLicensesData.SearchData(ChosenColumn.ToString(), SearchValue, Mode.ToString());
        }        
        





    }
}
