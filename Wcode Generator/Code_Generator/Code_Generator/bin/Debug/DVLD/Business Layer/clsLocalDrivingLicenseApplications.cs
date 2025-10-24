using System;
using System.Data;
using DVLD_DataAccessLayer;

namespace DVLD_BusinessLayer
{
    public class clsLocalDrivingLicenseApplications
    {
        //===============================
        // ENUMS & PRIVATE FIELDS
        //===============================

        private enum enMode { AddNew = 0, Update = 1 }
        private enMode Mode;

        //===============================
        // PUBLIC PROPERTIES (Generated from Columns)
        //===============================

   		public int LocalDrivingLicenseApplicationID { get; set; }
		public int ApplicationID { get; set; }
		private Lazy<clsApplications>  _ApplicationIDInfo;
		public clsApplications ApplicationIDInfo => _ApplicationIDInfo.Value;
		public int LicenseClassID { get; set; }
		private Lazy<clsLicenseClasses>  _LicenseClassIDInfo;
		public clsLicenseClasses LicenseClassIDInfo => _LicenseClassIDInfo.Value;


        //===============================
        // CONSTRUCTORS
        //===============================

        // Empty constructor
        public clsLocalDrivingLicenseApplications()
        {
            this.LocalDrivingLicenseApplicationID = -1;
			this.ApplicationID = -1;
			this.LicenseClassID = -1;

            Mode = enMode.AddNew;
        }

        // Full constructor
        public clsLocalDrivingLicenseApplications(int localdrivinglicenseapplicationid, int applicationid, int licenseclassid)
        {
           this.LocalDrivingLicenseApplicationID = localdrivinglicenseapplicationid;
			this.ApplicationID = applicationid;
			_ApplicationIDInfo = new Lazy<clsApplications>(() => clsApplications.Find(this.ApplicationID));

			this.LicenseClassID = licenseclassid;
			_LicenseClassIDInfo = new Lazy<clsLicenseClasses>(() => clsLicenseClasses.Find(this.LicenseClassID));

            Mode = enMode.Update;
        }

        //===============================
        // FIND METHOD (STATIC)
        //===============================

       public static clsLocalDrivingLicenseApplications Find(int LocalDrivingLicenseApplicationID)
        {
            // Declare local variables to pass to the DAL via 'out'. 
            // The 'out' keyword guarantees the DAL method initializes them.
            int localdrivinglicenseapplicationid;
			int applicationid;
			int licenseclassid; // This placeholder now includes the 'out' keyword

            // Call DAL: Data is retrieved and assigned to the local variables (passed by OUT)
            if (clsLocalDrivingLicenseApplicationsData.GetLocalDrivingLicenseApplicationsInfoByID(LocalDrivingLicenseApplicationID, 
                out localdrivinglicenseapplicationid, out applicationid, out licenseclassid))
            {
                // Constructor Call: Local variables are now passed by VALUE to the constructor
                return new clsLocalDrivingLicenseApplications(localdrivinglicenseapplicationid, applicationid, licenseclassid);
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
                    if (AddNewLocalDrivingLicenseApplications())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    break;

                case enMode.Update:
                    if (UpdateLocalDrivingLicenseApplications())
                        return true;
                    break;
            }

            return false;
        }

        // ===============================
        // PRIVATE ADD / UPDATE METHODS
        // ===============================

        private bool AddNewLocalDrivingLicenseApplications()
        {
            // Must map instance properties to DAL's parameters
            this.LocalDrivingLicenseApplicationID = clsLocalDrivingLicenseApplicationsData.InsertLocalDrivingLicenseApplications(
            this.ApplicationID,
				this.LicenseClassID 
            );
            return (this.LocalDrivingLicenseApplicationID != -1);
        }

        private bool UpdateLocalDrivingLicenseApplications()
        {
            // Must map instance properties to DAL's parameters
            return clsLocalDrivingLicenseApplicationsData.UpdateLocalDrivingLicenseApplications(
            this.LocalDrivingLicenseApplicationID,
				this.ApplicationID,
				this.LicenseClassID 
            );
        }


        // ===============================
        // DELETE METHOD
        // ===============================

        public bool Delete()
        {
            // Use the instance property value
            return clsLocalDrivingLicenseApplicationsData.DeleteLocalDrivingLicenseApplications(this.LocalDrivingLicenseApplicationID);
        }

        //===============================
        // GetAll METHOD
        //===============================

        public static DataTable GetAllLocalDrivingLicenseApplications()
        {
            return clsLocalDrivingLicenseApplicationsData.GetAllLocalDrivingLicenseApplications();
        }
        
        //===============================
        // OPTIONAL BIT CHECK METHODS
        //===============================

        

        public static bool  IsLocalDrivingLicenseApplicationsExist(int LocalDrivingLicenseApplicationID)
        {
            return clsLocalDrivingLicenseApplicationsData.IsLocalDrivingLicenseApplicationsExist(LocalDrivingLicenseApplicationID);
        }


        
        public enum LocalDrivingLicenseApplicationsColumn
         {
            LocalDrivingLicenseApplicationID,
ApplicationID,
LicenseClassID,
         }


        public enum SearchMode
        {
            Anywhere,
            StartsWith,
            EndsWith,
            ExactMatch
        }
    

        public static DataTable SearchData(LocalDrivingLicenseApplicationsColumn ChosenColumn, string SearchValue, SearchMode Mode = SearchMode.Anywhere)
        {
            if (string.IsNullOrWhiteSpace(SearchValue) || !SqlHelper.IsSafeInput(SearchValue))
                return new DataTable();


            return clsLocalDrivingLicenseApplicationsData.SearchData(ChosenColumn.ToString(), SearchValue, Mode.ToString());
        }        
        





    }
}
