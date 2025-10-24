using System;
using System.Data;
using DVLD_DataAccessLayer;

namespace DVLD_BusinessLayer
{
    public class clsTestTypes
    {
        //===============================
        // ENUMS & PRIVATE FIELDS
        //===============================

        private enum enMode { AddNew = 0, Update = 1 }
        private enMode Mode;

        //===============================
        // PUBLIC PROPERTIES (Generated from Columns)
        //===============================

   		public int TestTypeID { get; set; }
		public string TestTypeTitle { get; set; }
		public string TestTypeDescription { get; set; }
		public decimal TestTypeFees { get; set; }


        //===============================
        // CONSTRUCTORS
        //===============================

        // Empty constructor
        public clsTestTypes()
        {
            this.TestTypeID = -1;
			this.TestTypeTitle = "";
			this.TestTypeDescription = "";
			this.TestTypeFees = 0;

            Mode = enMode.AddNew;
        }

        // Full constructor
        public clsTestTypes(int testtypeid, string testtypetitle, string testtypedescription, decimal testtypefees)
        {
           this.TestTypeID = testtypeid;
			this.TestTypeTitle = testtypetitle;
			this.TestTypeDescription = testtypedescription;
			this.TestTypeFees = testtypefees;

            Mode = enMode.Update;
        }

        //===============================
        // FIND METHOD (STATIC)
        //===============================

       public static clsTestTypes Find(int TestTypeID)
        {
            // Declare local variables to pass to the DAL via 'out'. 
            // The 'out' keyword guarantees the DAL method initializes them.
            int testtypeid;
			string testtypetitle;
			string testtypedescription;
			decimal testtypefees; // This placeholder now includes the 'out' keyword

            // Call DAL: Data is retrieved and assigned to the local variables (passed by OUT)
            if (clsTestTypesData.GetTestTypesInfoByID(TestTypeID, 
                out testtypeid, out testtypetitle, out testtypedescription, out testtypefees))
            {
                // Constructor Call: Local variables are now passed by VALUE to the constructor
                return new clsTestTypes(testtypeid, testtypetitle, testtypedescription, testtypefees);
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
                    if (AddNewTestTypes())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    break;

                case enMode.Update:
                    if (UpdateTestTypes())
                        return true;
                    break;
            }

            return false;
        }

        // ===============================
        // PRIVATE ADD / UPDATE METHODS
        // ===============================

        private bool AddNewTestTypes()
        {
            // Must map instance properties to DAL's parameters
            this.TestTypeID = clsTestTypesData.InsertTestTypes(
            this.TestTypeTitle,
				this.TestTypeDescription,
				this.TestTypeFees 
            );
            return (this.TestTypeID != -1);
        }

        private bool UpdateTestTypes()
        {
            // Must map instance properties to DAL's parameters
            return clsTestTypesData.UpdateTestTypes(
            this.TestTypeID,
				this.TestTypeTitle,
				this.TestTypeDescription,
				this.TestTypeFees 
            );
        }


        // ===============================
        // DELETE METHOD
        // ===============================

        public bool Delete()
        {
            // Use the instance property value
            return clsTestTypesData.DeleteTestTypes(this.TestTypeID);
        }

        //===============================
        // GetAll METHOD
        //===============================

        public static DataTable GetAllTestTypes()
        {
            return clsTestTypesData.GetAllTestTypes();
        }
        
        //===============================
        // OPTIONAL BIT CHECK METHODS
        //===============================

        

        public static bool  IsTestTypesExist(int TestTypeID)
        {
            return clsTestTypesData.IsTestTypesExist(TestTypeID);
        }


        
        public enum TestTypesColumn
         {
            TestTypeID,
TestTypeTitle,
TestTypeDescription,
TestTypeFees,
         }


        public enum SearchMode
        {
            Anywhere,
            StartsWith,
            EndsWith,
            ExactMatch
        }
    

        public static DataTable SearchData(TestTypesColumn ChosenColumn, string SearchValue, SearchMode Mode = SearchMode.Anywhere)
        {
            if (string.IsNullOrWhiteSpace(SearchValue) || !SqlHelper.IsSafeInput(SearchValue))
                return new DataTable();


            return clsTestTypesData.SearchData(ChosenColumn.ToString(), SearchValue, Mode.ToString());
        }        
        





    }
}
