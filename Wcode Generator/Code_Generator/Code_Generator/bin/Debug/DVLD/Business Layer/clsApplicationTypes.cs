using System;
using System.Data;
using DVLD_DataAccessLayer;

namespace DVLD_BusinessLayer
{
    public class clsApplicationTypes
    {
        //===============================
        // ENUMS & PRIVATE FIELDS
        //===============================

        private enum enMode { AddNew = 0, Update = 1 }
        private enMode Mode;

        //===============================
        // PUBLIC PROPERTIES (Generated from Columns)
        //===============================

   		public int ApplicationTypeID { get; set; }
		public string ApplicationTypeTitle { get; set; }
		public decimal ApplicationFees { get; set; }


        //===============================
        // CONSTRUCTORS
        //===============================

        // Empty constructor
        public clsApplicationTypes()
        {
            this.ApplicationTypeID = -1;
			this.ApplicationTypeTitle = "";
			this.ApplicationFees = 0;

            Mode = enMode.AddNew;
        }

        // Full constructor
        public clsApplicationTypes(int applicationtypeid, string applicationtypetitle, decimal applicationfees)
        {
           this.ApplicationTypeID = applicationtypeid;
			this.ApplicationTypeTitle = applicationtypetitle;
			this.ApplicationFees = applicationfees;

            Mode = enMode.Update;
        }

        //===============================
        // FIND METHOD (STATIC)
        //===============================

       public static clsApplicationTypes Find(int ApplicationTypeID)
        {
            // Declare local variables to pass to the DAL via 'out'. 
            // The 'out' keyword guarantees the DAL method initializes them.
            int applicationtypeid;
			string applicationtypetitle;
			decimal applicationfees; // This placeholder now includes the 'out' keyword

            // Call DAL: Data is retrieved and assigned to the local variables (passed by OUT)
            if (clsApplicationTypesData.GetApplicationTypesInfoByID(ApplicationTypeID, 
                out applicationtypeid, out applicationtypetitle, out applicationfees))
            {
                // Constructor Call: Local variables are now passed by VALUE to the constructor
                return new clsApplicationTypes(applicationtypeid, applicationtypetitle, applicationfees);
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
                    if (AddNewApplicationTypes())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    break;

                case enMode.Update:
                    if (UpdateApplicationTypes())
                        return true;
                    break;
            }

            return false;
        }

        // ===============================
        // PRIVATE ADD / UPDATE METHODS
        // ===============================

        private bool AddNewApplicationTypes()
        {
            // Must map instance properties to DAL's parameters
            this.ApplicationTypeID = clsApplicationTypesData.InsertApplicationTypes(
            this.ApplicationTypeTitle,
				this.ApplicationFees 
            );
            return (this.ApplicationTypeID != -1);
        }

        private bool UpdateApplicationTypes()
        {
            // Must map instance properties to DAL's parameters
            return clsApplicationTypesData.UpdateApplicationTypes(
            this.ApplicationTypeID,
				this.ApplicationTypeTitle,
				this.ApplicationFees 
            );
        }


        // ===============================
        // DELETE METHOD
        // ===============================

        public bool Delete()
        {
            // Use the instance property value
            return clsApplicationTypesData.DeleteApplicationTypes(this.ApplicationTypeID);
        }

        //===============================
        // GetAll METHOD
        //===============================

        public static DataTable GetAllApplicationTypes()
        {
            return clsApplicationTypesData.GetAllApplicationTypes();
        }
        
        //===============================
        // OPTIONAL BIT CHECK METHODS
        //===============================

        

        public static bool  IsApplicationTypesExist(int ApplicationTypeID)
        {
            return clsApplicationTypesData.IsApplicationTypesExist(ApplicationTypeID);
        }


        
        public enum ApplicationTypesColumn
         {
            ApplicationTypeID,
ApplicationTypeTitle,
ApplicationFees,
         }


        public enum SearchMode
        {
            Anywhere,
            StartsWith,
            EndsWith,
            ExactMatch
        }
    

        public static DataTable SearchData(ApplicationTypesColumn ChosenColumn, string SearchValue, SearchMode Mode = SearchMode.Anywhere)
        {
            if (string.IsNullOrWhiteSpace(SearchValue) || !SqlHelper.IsSafeInput(SearchValue))
                return new DataTable();


            return clsApplicationTypesData.SearchData(ChosenColumn.ToString(), SearchValue, Mode.ToString());
        }        
        





    }
}
