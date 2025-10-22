using System;
using System.Data;
using _DataAccessLayer;

namespace _BusinessLayer
{
    public class clsApplications
    {
        //===============================
        // ENUMS & PRIVATE FIELDS
        //===============================

        private enum enMode { AddNew = 0, Update = 1 }
        private enMode Mode;

        //===============================
        // PUBLIC PROPERTIES (Generated from Columns)
        //===============================

   		public int ApplicationID { get; set; }
		public int ApplicantPersonID { get; set; }
		private Lazy<clsPeople>  _ApplicantPersonIDInfo;
		public clsPeople ApplicantPersonIDInfo => _ApplicantPersonIDInfo.Value;
		public DateTime ApplicationDate { get; set; }
		public int ApplicationTypeID { get; set; }
		private Lazy<clsApplicationTypes>  _ApplicationTypeIDInfo;
		public clsApplicationTypes ApplicationTypeIDInfo => _ApplicationTypeIDInfo.Value;
		public byte ApplicationStatus { get; set; }
		public DateTime LastStatusDate { get; set; }
		public decimal PaidFees { get; set; }
		public int CreatedByUserID { get; set; }
		private Lazy<clsUsers>  _CreatedByUserIDInfo;
		public clsUsers CreatedByUserIDInfo => _CreatedByUserIDInfo.Value;


        //===============================
        // CONSTRUCTORS
        //===============================

        // Empty constructor
        public clsApplications()
        {
            this.ApplicationID = -1;
			this.ApplicantPersonID = -1;
			this.ApplicationDate = DateTime.MinValue;
			this.ApplicationTypeID = -1;
			this.ApplicationStatus = -1;
			this.LastStatusDate = DateTime.MinValue;
			this.PaidFees = 0;
			this.CreatedByUserID = -1;

            Mode = enMode.AddNew;
        }

        // Full constructor
        public clsApplications(int applicationid, int applicantpersonid, DateTime applicationdate, int applicationtypeid, byte applicationstatus, DateTime laststatusdate, decimal paidfees, int createdbyuserid)
        {
           this.ApplicationID = applicationid;
			this.ApplicantPersonID = applicantpersonid;
			_ApplicantPersonIDInfo = new Lazy<clsPeople>(() => clsPeople.Find(this.ApplicantPersonID));

			this.ApplicationDate = applicationdate;
			this.ApplicationTypeID = applicationtypeid;
			_ApplicationTypeIDInfo = new Lazy<clsApplicationTypes>(() => clsApplicationTypes.Find(this.ApplicationTypeID));

			this.ApplicationStatus = applicationstatus;
			this.LastStatusDate = laststatusdate;
			this.PaidFees = paidfees;
			this.CreatedByUserID = createdbyuserid;
			_CreatedByUserIDInfo = new Lazy<clsUsers>(() => clsUsers.Find(this.CreatedByUserID));

            Mode = enMode.Update;
        }

        //===============================
        // FIND METHOD (STATIC)
        //===============================

       public static clsApplications Find(int ApplicationID)
        {
            // Declare local variables to pass to the DAL via 'out'. 
            // The 'out' keyword guarantees the DAL method initializes them.
            int applicationid;
			int applicantpersonid;
			DateTime applicationdate;
			int applicationtypeid;
			byte applicationstatus;
			DateTime laststatusdate;
			decimal paidfees;
			int createdbyuserid; // This placeholder now includes the 'out' keyword

            // Call DAL: Data is retrieved and assigned to the local variables (passed by OUT)
            if (clsApplicationsData.GetApplicationsInfoByID(ApplicationID, 
                out applicationid, out applicantpersonid, out applicationdate, out applicationtypeid, out applicationstatus, out laststatusdate, out paidfees, out createdbyuserid))
            {
                // Constructor Call: Local variables are now passed by VALUE to the constructor
                return new clsApplications(applicationid, applicantpersonid, applicationdate, applicationtypeid, applicationstatus, laststatusdate, paidfees, createdbyuserid);
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
                    if (AddNewApplications())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    break;

                case enMode.Update:
                    if (UpdateApplications())
                        return true;
                    break;
            }

            return false;
        }

        // ===============================
        // PRIVATE ADD / UPDATE METHODS
        // ===============================

        private bool AddNewApplications()
        {
            // Must map instance properties to DAL's parameters
            this.ApplicationID = clsApplicationsData.InsertApplications(
            this.ApplicantPersonID,
				this.ApplicationDate,
				this.ApplicationTypeID,
				this.ApplicationStatus,
				this.LastStatusDate,
				this.PaidFees,
				this.CreatedByUserID 
            );
            return (this.ApplicationID != -1);
        }

        private bool UpdateApplications()
        {
            // Must map instance properties to DAL's parameters
            return clsApplicationsData.UpdateApplications(
            this.ApplicationID,
				this.ApplicantPersonID,
				this.ApplicationDate,
				this.ApplicationTypeID,
				this.ApplicationStatus,
				this.LastStatusDate,
				this.PaidFees,
				this.CreatedByUserID 
            );
        }


        // ===============================
        // DELETE METHOD
        // ===============================

        public bool Delete()
        {
            // Use the instance property value
            return clsApplicationsData.DeleteApplications(this.ApplicationID);
        }

        //===============================
        // GetAll METHOD
        //===============================

        public static DataTable GetAllApplications()
        {
            return clsApplicationsData.GetAllApplications();
        }
        
        //===============================
        // OPTIONAL BIT CHECK METHODS
        //===============================

        

        public static bool  IsApplicationsExist(int ApplicationID)
        {
            return clsApplicationsData.IsApplicationsExist(ApplicationID);
        }


        
        public enum ApplicationsColumn
         {
            ApplicationID,
ApplicantPersonID,
ApplicationDate,
ApplicationTypeID,
ApplicationStatus,
LastStatusDate,
PaidFees,
CreatedByUserID,
         }


        public enum SearchMode
        {
            Anywhere,
            StartsWith,
            EndsWith,
            ExactMatch
        }
    

        public static DataTable SearchData(ApplicationsColumn ChosenColumn, string SearchValue, SearchMode Mode = SearchMode.Anywhere)
        {
            if (string.IsNullOrWhiteSpace(SearchValue) || !SqlHelper.IsSafeInput(SearchValue))
                return new DataTable();


            return clsApplicationsData.SearchData(ChosenColumn.ToString(), SearchValue, Mode.ToString());
        }        
        





    }
}
