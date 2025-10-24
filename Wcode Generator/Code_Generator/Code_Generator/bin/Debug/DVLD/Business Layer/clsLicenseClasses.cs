using System;
using System.Data;
using DVLD_DataAccessLayer;

namespace DVLD_BusinessLayer
{
    public class clsLicenseClasses
    {
        //===============================
        // ENUMS & PRIVATE FIELDS
        //===============================

        private enum enMode { AddNew = 0, Update = 1 }
        private enMode Mode;

        //===============================
        // PUBLIC PROPERTIES (Generated from Columns)
        //===============================

   		public int LicenseClassID { get; set; }
		public string ClassName { get; set; }
		public string ClassDescription { get; set; }
		public byte MinimumAllowedAge { get; set; }
		public byte DefaultValidityLength { get; set; }
		public decimal ClassFees { get; set; }


        //===============================
        // CONSTRUCTORS
        //===============================

        // Empty constructor
        public clsLicenseClasses()
        {
            this.LicenseClassID = -1;
			this.ClassName = "";
			this.ClassDescription = "";
			this.MinimumAllowedAge = -1;
			this.DefaultValidityLength = -1;
			this.ClassFees = 0;

            Mode = enMode.AddNew;
        }

        // Full constructor
        public clsLicenseClasses(int licenseclassid, string classname, string classdescription, byte minimumallowedage, byte defaultvaliditylength, decimal classfees)
        {
           this.LicenseClassID = licenseclassid;
			this.ClassName = classname;
			this.ClassDescription = classdescription;
			this.MinimumAllowedAge = minimumallowedage;
			this.DefaultValidityLength = defaultvaliditylength;
			this.ClassFees = classfees;

            Mode = enMode.Update;
        }

        //===============================
        // FIND METHOD (STATIC)
        //===============================

       public static clsLicenseClasses Find(int LicenseClassID)
        {
            // Declare local variables to pass to the DAL via 'out'. 
            // The 'out' keyword guarantees the DAL method initializes them.
            int licenseclassid;
			string classname;
			string classdescription;
			byte minimumallowedage;
			byte defaultvaliditylength;
			decimal classfees; // This placeholder now includes the 'out' keyword

            // Call DAL: Data is retrieved and assigned to the local variables (passed by OUT)
            if (clsLicenseClassesData.GetLicenseClassesInfoByID(LicenseClassID, 
                out licenseclassid, out classname, out classdescription, out minimumallowedage, out defaultvaliditylength, out classfees))
            {
                // Constructor Call: Local variables are now passed by VALUE to the constructor
                return new clsLicenseClasses(licenseclassid, classname, classdescription, minimumallowedage, defaultvaliditylength, classfees);
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
                    if (AddNewLicenseClasses())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    break;

                case enMode.Update:
                    if (UpdateLicenseClasses())
                        return true;
                    break;
            }

            return false;
        }

        // ===============================
        // PRIVATE ADD / UPDATE METHODS
        // ===============================

        private bool AddNewLicenseClasses()
        {
            // Must map instance properties to DAL's parameters
            this.LicenseClassID = clsLicenseClassesData.InsertLicenseClasses(
            this.ClassName,
				this.ClassDescription,
				this.MinimumAllowedAge,
				this.DefaultValidityLength,
				this.ClassFees 
            );
            return (this.LicenseClassID != -1);
        }

        private bool UpdateLicenseClasses()
        {
            // Must map instance properties to DAL's parameters
            return clsLicenseClassesData.UpdateLicenseClasses(
            this.LicenseClassID,
				this.ClassName,
				this.ClassDescription,
				this.MinimumAllowedAge,
				this.DefaultValidityLength,
				this.ClassFees 
            );
        }


        // ===============================
        // DELETE METHOD
        // ===============================

        public bool Delete()
        {
            // Use the instance property value
            return clsLicenseClassesData.DeleteLicenseClasses(this.LicenseClassID);
        }

        //===============================
        // GetAll METHOD
        //===============================

        public static DataTable GetAllLicenseClasses()
        {
            return clsLicenseClassesData.GetAllLicenseClasses();
        }
        
        //===============================
        // OPTIONAL BIT CHECK METHODS
        //===============================

        

        public static bool  IsLicenseClassesExist(int LicenseClassID)
        {
            return clsLicenseClassesData.IsLicenseClassesExist(LicenseClassID);
        }


        
        public enum LicenseClassesColumn
         {
            LicenseClassID,
ClassName,
ClassDescription,
MinimumAllowedAge,
DefaultValidityLength,
ClassFees,
         }


        public enum SearchMode
        {
            Anywhere,
            StartsWith,
            EndsWith,
            ExactMatch
        }
    

        public static DataTable SearchData(LicenseClassesColumn ChosenColumn, string SearchValue, SearchMode Mode = SearchMode.Anywhere)
        {
            if (string.IsNullOrWhiteSpace(SearchValue) || !SqlHelper.IsSafeInput(SearchValue))
                return new DataTable();


            return clsLicenseClassesData.SearchData(ChosenColumn.ToString(), SearchValue, Mode.ToString());
        }        
        





    }
}
