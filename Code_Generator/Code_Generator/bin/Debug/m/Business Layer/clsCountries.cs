using System;
using System.Data;
using _DataAccessLayer;

namespace _BusinessLayer
{
    public class clsCountries
    {
        //===============================
        // ENUMS & PRIVATE FIELDS
        //===============================

        private enum enMode { AddNew = 0, Update = 1 }
        private enMode Mode;

        //===============================
        // PUBLIC PROPERTIES (Generated from Columns)
        //===============================

   		public int CountryID { get; set; }
		public string CountryName { get; set; }


        //===============================
        // CONSTRUCTORS
        //===============================

        // Empty constructor
        public clsCountries()
        {
            this.CountryID = -1;
			this.CountryName = "";

            Mode = enMode.AddNew;
        }

        // Full constructor
        public clsCountries(int countryid, string countryname)
        {
           this.CountryID = countryid;
			this.CountryName = countryname;

            Mode = enMode.Update;
        }

        //===============================
        // FIND METHOD (STATIC)
        //===============================

       public static clsCountries Find(int CountryID)
        {
            // Declare local variables to pass to the DAL via 'out'. 
            // The 'out' keyword guarantees the DAL method initializes them.
            int countryid;
			string countryname; // This placeholder now includes the 'out' keyword

            // Call DAL: Data is retrieved and assigned to the local variables (passed by OUT)
            if (clsCountriesData.GetCountriesInfoByID(CountryID, 
                out countryid, out countryname))
            {
                // Constructor Call: Local variables are now passed by VALUE to the constructor
                return new clsCountries(countryid, countryname);
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
                    if (AddNewCountries())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    break;

                case enMode.Update:
                    if (UpdateCountries())
                        return true;
                    break;
            }

            return false;
        }

        // ===============================
        // PRIVATE ADD / UPDATE METHODS
        // ===============================

        private bool AddNewCountries()
        {
            // Must map instance properties to DAL's parameters
            this.CountryID = clsCountriesData.InsertCountries(
            this.CountryName 
            );
            return (this.CountryID != -1);
        }

        private bool UpdateCountries()
        {
            // Must map instance properties to DAL's parameters
            return clsCountriesData.UpdateCountries(
            this.CountryID,
				this.CountryName 
            );
        }


        // ===============================
        // DELETE METHOD
        // ===============================

        public bool Delete()
        {
            // Use the instance property value
            return clsCountriesData.DeleteCountries(this.CountryID);
        }

        //===============================
        // GetAll METHOD
        //===============================

        public static DataTable GetAllCountries()
        {
            return clsCountriesData.GetAllCountries();
        }
        
        //===============================
        // OPTIONAL BIT CHECK METHODS
        //===============================

        

        public static bool  IsCountriesExist(int CountryID)
        {
            return clsCountriesData.IsCountriesExist(CountryID);
        }


        
        public enum CountriesColumn
         {
            CountryID,
CountryName,
         }


        public enum SearchMode
        {
            Anywhere,
            StartsWith,
            EndsWith,
            ExactMatch
        }
    

        public static DataTable SearchData(CountriesColumn ChosenColumn, string SearchValue, SearchMode Mode = SearchMode.Anywhere)
        {
            if (string.IsNullOrWhiteSpace(SearchValue) || !SqlHelper.IsSafeInput(SearchValue))
                return new DataTable();


            return clsCountriesData.SearchData(ChosenColumn.ToString(), SearchValue, Mode.ToString());
        }        
        





    }
}
