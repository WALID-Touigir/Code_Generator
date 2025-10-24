using System;
using System.Data;
using DVLD_DataAccessLayer;

namespace DVLD_BusinessLayer
{
    public class clsTestAppointments
    {
        //===============================
        // ENUMS & PRIVATE FIELDS
        //===============================

        private enum enMode { AddNew = 0, Update = 1 }
        private enMode Mode;

        //===============================
        // PUBLIC PROPERTIES (Generated from Columns)
        //===============================

   		public int TestAppointmentID { get; set; }
		public int TestTypeID { get; set; }
		private Lazy<clsTestTypes>  _TestTypeIDInfo;
		public clsTestTypes TestTypeIDInfo => _TestTypeIDInfo.Value;
		public int LocalDrivingLicenseApplicationID { get; set; }
		private Lazy<clsLocalDrivingLicenseApplications>  _LocalDrivingLicenseApplicationIDInfo;
		public clsLocalDrivingLicenseApplications LocalDrivingLicenseApplicationIDInfo => _LocalDrivingLicenseApplicationIDInfo.Value;
		public DateTime AppointmentDate { get; set; }
		public decimal PaidFees { get; set; }
		public int CreatedByUserID { get; set; }
		private Lazy<clsUsers>  _CreatedByUserIDInfo;
		public clsUsers CreatedByUserIDInfo => _CreatedByUserIDInfo.Value;
		public bool IsLocked { get; set; }
		public int? RetakeTestApplicationID { get; set; }
		private Lazy<clsApplications>  _RetakeTestApplicationIDInfo;
		public clsApplications RetakeTestApplicationIDInfo => _RetakeTestApplicationIDInfo.Value;


        //===============================
        // CONSTRUCTORS
        //===============================

        // Empty constructor
        public clsTestAppointments()
        {
            this.TestAppointmentID = -1;
			this.TestTypeID = -1;
			this.LocalDrivingLicenseApplicationID = -1;
			this.AppointmentDate = DateTime.MinValue;
			this.PaidFees = 0;
			this.CreatedByUserID = -1;
			this.IsLocked = false;
			this.RetakeTestApplicationID = -1;

            Mode = enMode.AddNew;
        }

        // Full constructor
        public clsTestAppointments(int testappointmentid, int testtypeid, int localdrivinglicenseapplicationid, DateTime appointmentdate, decimal paidfees, int createdbyuserid, bool islocked, int retaketestapplicationid)
        {
           this.TestAppointmentID = testappointmentid;
			this.TestTypeID = testtypeid;
			_TestTypeIDInfo = new Lazy<clsTestTypes>(() => clsTestTypes.Find(this.TestTypeID));

			this.LocalDrivingLicenseApplicationID = localdrivinglicenseapplicationid;
			_LocalDrivingLicenseApplicationIDInfo = new Lazy<clsLocalDrivingLicenseApplications>(() => clsLocalDrivingLicenseApplications.Find(this.LocalDrivingLicenseApplicationID));

			this.AppointmentDate = appointmentdate;
			this.PaidFees = paidfees;
			this.CreatedByUserID = createdbyuserid;
			_CreatedByUserIDInfo = new Lazy<clsUsers>(() => clsUsers.Find(this.CreatedByUserID));

			this.IsLocked = islocked;
			this.RetakeTestApplicationID = retaketestapplicationid;
			_RetakeTestApplicationIDInfo = new Lazy<clsApplications>(() => clsApplications.Find(this.RetakeTestApplicationID));

            Mode = enMode.Update;
        }

        //===============================
        // FIND METHOD (STATIC)
        //===============================

       public static clsTestAppointments Find(int TestAppointmentID)
        {
            // Declare local variables to pass to the DAL via 'out'. 
            // The 'out' keyword guarantees the DAL method initializes them.
            int testappointmentid;
			int testtypeid;
			int localdrivinglicenseapplicationid;
			DateTime appointmentdate;
			decimal paidfees;
			int createdbyuserid;
			bool islocked;
			int retaketestapplicationid; // This placeholder now includes the 'out' keyword

            // Call DAL: Data is retrieved and assigned to the local variables (passed by OUT)
            if (clsTestAppointmentsData.GetTestAppointmentsInfoByID(TestAppointmentID, 
                out testappointmentid, out testtypeid, out localdrivinglicenseapplicationid, out appointmentdate, out paidfees, out createdbyuserid, out islocked, out retaketestapplicationid))
            {
                // Constructor Call: Local variables are now passed by VALUE to the constructor
                return new clsTestAppointments(testappointmentid, testtypeid, localdrivinglicenseapplicationid, appointmentdate, paidfees, createdbyuserid, islocked, retaketestapplicationid);
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
                    if (AddNewTestAppointments())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    break;

                case enMode.Update:
                    if (UpdateTestAppointments())
                        return true;
                    break;
            }

            return false;
        }

        // ===============================
        // PRIVATE ADD / UPDATE METHODS
        // ===============================

        private bool AddNewTestAppointments()
        {
            // Must map instance properties to DAL's parameters
            this.TestAppointmentID = clsTestAppointmentsData.InsertTestAppointments(
            this.TestTypeID,
				this.LocalDrivingLicenseApplicationID,
				this.AppointmentDate,
				this.PaidFees,
				this.CreatedByUserID,
				this.IsLocked,
				this.RetakeTestApplicationID 
            );
            return (this.TestAppointmentID != -1);
        }

        private bool UpdateTestAppointments()
        {
            // Must map instance properties to DAL's parameters
            return clsTestAppointmentsData.UpdateTestAppointments(
            this.TestAppointmentID,
				this.TestTypeID,
				this.LocalDrivingLicenseApplicationID,
				this.AppointmentDate,
				this.PaidFees,
				this.CreatedByUserID,
				this.IsLocked,
				this.RetakeTestApplicationID 
            );
        }


        // ===============================
        // DELETE METHOD
        // ===============================

        public bool Delete()
        {
            // Use the instance property value
            return clsTestAppointmentsData.DeleteTestAppointments(this.TestAppointmentID);
        }

        //===============================
        // GetAll METHOD
        //===============================

        public static DataTable GetAllTestAppointments()
        {
            return clsTestAppointmentsData.GetAllTestAppointments();
        }
        
        //===============================
        // OPTIONAL BIT CHECK METHODS
        //===============================

        

        public static bool  IsTestAppointmentsExist(int TestAppointmentID)
        {
            return clsTestAppointmentsData.IsTestAppointmentsExist(TestAppointmentID);
        }


        
        public enum TestAppointmentsColumn
         {
            TestAppointmentID,
TestTypeID,
LocalDrivingLicenseApplicationID,
AppointmentDate,
PaidFees,
CreatedByUserID,
IsLocked,
RetakeTestApplicationID,
         }


        public enum SearchMode
        {
            Anywhere,
            StartsWith,
            EndsWith,
            ExactMatch
        }
    

        public static DataTable SearchData(TestAppointmentsColumn ChosenColumn, string SearchValue, SearchMode Mode = SearchMode.Anywhere)
        {
            if (string.IsNullOrWhiteSpace(SearchValue) || !SqlHelper.IsSafeInput(SearchValue))
                return new DataTable();


            return clsTestAppointmentsData.SearchData(ChosenColumn.ToString(), SearchValue, Mode.ToString());
        }        
        





    }
}
