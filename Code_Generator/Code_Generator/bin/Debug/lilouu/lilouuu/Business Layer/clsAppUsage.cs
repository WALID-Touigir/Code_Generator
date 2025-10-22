using System;
using System.Data;
using lilouuu_DataAccessLayer;

namespace lilouuu_BusinessLayer
{
    public class clsAppUsage
    {
        //===============================
        // ENUMS & PRIVATE FIELDS
        //===============================

        private enum enMode { AddNew = 0, Update = 1 }
        private enMode Mode;

        //===============================
        // PUBLIC PROPERTIES (Generated from Columns)
        //===============================

   		public int Id { get; set; }
		public string AppName { get; set; }
		public string WindowTitle { get; set; }
		public DateTime? StartTime { get; set; }
		public DateTime? EndTime { get; set; }
		public int? DurationSeconds { get; set; }
		public bool? IsIdle { get; set; }


        //===============================
        // CONSTRUCTORS
        //===============================

        // Empty constructor
        public clsAppUsage()
        {
            this.Id = -1;
			this.AppName = "";
			this.WindowTitle = "";
			this.StartTime = DateTime.MinValue;
			this.EndTime = DateTime.MinValue;
			this.DurationSeconds = -1;
			this.IsIdle = false;

            Mode = enMode.AddNew;
        }

        // Full constructor
        public clsAppUsage(int id, string appname, string windowtitle, DateTime starttime, DateTime endtime, int durationseconds, bool isidle)
        {
           this.Id = id;
			this.AppName = appname;
			this.WindowTitle = windowtitle;
			this.StartTime = starttime;
			this.EndTime = endtime;
			this.DurationSeconds = durationseconds;
			this.IsIdle = isidle;

            Mode = enMode.Update;
        }

        //===============================
        // FIND METHOD (STATIC)
        //===============================

       public static clsAppUsage Find(int Id)
        {
            // Declare local variables to pass to the DAL via 'out'. 
            // The 'out' keyword guarantees the DAL method initializes them.
            int id;
			string appname;
			string windowtitle;
			DateTime starttime;
			DateTime endtime;
			int durationseconds;
			bool isidle; // This placeholder now includes the 'out' keyword

            // Call DAL: Data is retrieved and assigned to the local variables (passed by OUT)
            if (clsAppUsageData.GetAppUsageInfoByID(Id, 
                out id, out appname, out windowtitle, out starttime, out endtime, out durationseconds, out isidle))
            {
                // Constructor Call: Local variables are now passed by VALUE to the constructor
                return new clsAppUsage(id, appname, windowtitle, starttime, endtime, durationseconds, isidle);
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
                    if (AddNewAppUsage())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    break;

                case enMode.Update:
                    if (UpdateAppUsage())
                        return true;
                    break;
            }

            return false;
        }

        // ===============================
        // PRIVATE ADD / UPDATE METHODS
        // ===============================

        private bool AddNewAppUsage()
        {
            // Must map instance properties to DAL's parameters
            this.Id = clsAppUsageData.InsertAppUsage(
            this.AppName,
				this.WindowTitle,
				this.StartTime,
				this.EndTime,
				this.DurationSeconds,
				this.IsIdle 
            );
            return (this.Id != -1);
        }

        private bool UpdateAppUsage()
        {
            // Must map instance properties to DAL's parameters
            return clsAppUsageData.UpdateAppUsage(
            this.Id,
				this.AppName,
				this.WindowTitle,
				this.StartTime,
				this.EndTime,
				this.DurationSeconds,
				this.IsIdle 
            );
        }


        // ===============================
        // DELETE METHOD
        // ===============================

        public bool Delete()
        {
            // Use the instance property value
            return clsAppUsageData.DeleteAppUsage(this.Id);
        }

        //===============================
        // GetAll METHOD
        //===============================

        public static DataTable GetAllAppUsage()
        {
            return clsAppUsageData.GetAllAppUsage();
        }
        
        //===============================
        // OPTIONAL BIT CHECK METHODS
        //===============================

        

        public static bool  IsAppUsageExist(int Id)
        {
            return clsAppUsageData.IsAppUsageExist(Id);
        }


        
        public enum AppUsageColumn
         {
            Id,
AppName,
WindowTitle,
StartTime,
EndTime,
DurationSeconds,
IsIdle,
         }


        public enum SearchMode
        {
            Anywhere,
            StartsWith,
            EndsWith,
            ExactMatch
        }
    

        public static DataTable SearchData(AppUsageColumn ChosenColumn, string SearchValue, SearchMode Mode = SearchMode.Anywhere)
        {
            if (string.IsNullOrWhiteSpace(SearchValue) || !SqlHelper.IsSafeInput(SearchValue))
                return new DataTable();


            return clsAppUsageData.SearchData(ChosenColumn.ToString(), SearchValue, Mode.ToString());
        }        
        





    }
}
