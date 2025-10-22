using System;
using System.Data;
using _DataAccessLayer;

namespace _BusinessLayer
{
    public class clsTests
    {
        //===============================
        // ENUMS & PRIVATE FIELDS
        //===============================

        private enum enMode { AddNew = 0, Update = 1 }
        private enMode Mode;

        //===============================
        // PUBLIC PROPERTIES (Generated from Columns)
        //===============================

   		public int TestID { get; set; }
		public int TestAppointmentID { get; set; }
		private Lazy<clsTestAppointments>  _TestAppointmentIDInfo;
		public clsTestAppointments TestAppointmentIDInfo => _TestAppointmentIDInfo.Value;
		public bool TestResult { get; set; }
		public string Notes { get; set; }
		public int CreatedByUserID { get; set; }
		private Lazy<clsUsers>  _CreatedByUserIDInfo;
		public clsUsers CreatedByUserIDInfo => _CreatedByUserIDInfo.Value;


        //===============================
        // CONSTRUCTORS
        //===============================

        // Empty constructor
        public clsTests()
        {
            this.TestID = -1;
			this.TestAppointmentID = -1;
			this.TestResult = false;
			this.Notes = "";
			this.CreatedByUserID = -1;

            Mode = enMode.AddNew;
        }

        // Full constructor
        public clsTests(int testid, int testappointmentid, bool testresult, string notes, int createdbyuserid)
        {
           this.TestID = testid;
			this.TestAppointmentID = testappointmentid;
			_TestAppointmentIDInfo = new Lazy<clsTestAppointments>(() => clsTestAppointments.Find(this.TestAppointmentID));

			this.TestResult = testresult;
			this.Notes = notes;
			this.CreatedByUserID = createdbyuserid;
			_CreatedByUserIDInfo = new Lazy<clsUsers>(() => clsUsers.Find(this.CreatedByUserID));

            Mode = enMode.Update;
        }

        //===============================
        // FIND METHOD (STATIC)
        //===============================

       public static clsTests Find(int TestID)
        {
            // Declare local variables to pass to the DAL via 'out'. 
            // The 'out' keyword guarantees the DAL method initializes them.
            int testid;
			int testappointmentid;
			bool testresult;
			string notes;
			int createdbyuserid; // This placeholder now includes the 'out' keyword

            // Call DAL: Data is retrieved and assigned to the local variables (passed by OUT)
            if (clsTestsData.GetTestsInfoByID(TestID, 
                out testid, out testappointmentid, out testresult, out notes, out createdbyuserid))
            {
                // Constructor Call: Local variables are now passed by VALUE to the constructor
                return new clsTests(testid, testappointmentid, testresult, notes, createdbyuserid);
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
                    if (AddNewTests())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    break;

                case enMode.Update:
                    if (UpdateTests())
                        return true;
                    break;
            }

            return false;
        }

        // ===============================
        // PRIVATE ADD / UPDATE METHODS
        // ===============================

        private bool AddNewTests()
        {
            // Must map instance properties to DAL's parameters
            this.TestID = clsTestsData.InsertTests(
            this.TestAppointmentID,
				this.TestResult,
				this.Notes,
				this.CreatedByUserID 
            );
            return (this.TestID != -1);
        }

        private bool UpdateTests()
        {
            // Must map instance properties to DAL's parameters
            return clsTestsData.UpdateTests(
            this.TestID,
				this.TestAppointmentID,
				this.TestResult,
				this.Notes,
				this.CreatedByUserID 
            );
        }


        // ===============================
        // DELETE METHOD
        // ===============================

        public bool Delete()
        {
            // Use the instance property value
            return clsTestsData.DeleteTests(this.TestID);
        }

        //===============================
        // GetAll METHOD
        //===============================

        public static DataTable GetAllTests()
        {
            return clsTestsData.GetAllTests();
        }
        
        //===============================
        // OPTIONAL BIT CHECK METHODS
        //===============================

        

        public static bool  IsTestsExist(int TestID)
        {
            return clsTestsData.IsTestsExist(TestID);
        }


        
        public enum TestsColumn
         {
            TestID,
TestAppointmentID,
TestResult,
Notes,
CreatedByUserID,
         }


        public enum SearchMode
        {
            Anywhere,
            StartsWith,
            EndsWith,
            ExactMatch
        }
    

        public static DataTable SearchData(TestsColumn ChosenColumn, string SearchValue, SearchMode Mode = SearchMode.Anywhere)
        {
            if (string.IsNullOrWhiteSpace(SearchValue) || !SqlHelper.IsSafeInput(SearchValue))
                return new DataTable();


            return clsTestsData.SearchData(ChosenColumn.ToString(), SearchValue, Mode.ToString());
        }        
        





    }
}
