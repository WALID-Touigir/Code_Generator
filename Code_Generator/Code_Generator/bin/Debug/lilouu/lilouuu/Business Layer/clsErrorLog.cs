using System;
using System.Data;
using lilouuu_DataAccessLayer;

namespace lilouuu_BusinessLayer
{
    public class clsErrorLog
    {
        //===============================
        // ENUMS & PRIVATE FIELDS
        //===============================

        private enum enMode { AddNew = 0, Update = 1 }
        private enMode Mode;

        //===============================
        // PUBLIC PROPERTIES (Generated from Columns)
        //===============================

   		public int ErrorID { get; set; }
		public string ErrorMessage { get; set; }
		public string StackTrace { get; set; }
		public DateTime? Timestamp { get; set; }
		public string Severity { get; set; }
		public string AdditionalInfo { get; set; }


        //===============================
        // CONSTRUCTORS
        //===============================

        // Empty constructor
        public clsErrorLog()
        {
            this.ErrorID = -1;
			this.ErrorMessage = "";
			this.StackTrace = "";
			this.Timestamp = DateTime.MinValue;
			this.Severity = "";
			this.AdditionalInfo = "";

            Mode = enMode.AddNew;
        }

        // Full constructor
        public clsErrorLog(int errorid, string errormessage, string stacktrace, DateTime timestamp, string severity, string additionalinfo)
        {
           this.ErrorID = errorid;
			this.ErrorMessage = errormessage;
			this.StackTrace = stacktrace;
			this.Timestamp = timestamp;
			this.Severity = severity;
			this.AdditionalInfo = additionalinfo;

            Mode = enMode.Update;
        }

        //===============================
        // FIND METHOD (STATIC)
        //===============================

       public static clsErrorLog Find(int ErrorID)
        {
            // Declare local variables to pass to the DAL via 'out'. 
            // The 'out' keyword guarantees the DAL method initializes them.
            int errorid;
			string errormessage;
			string stacktrace;
			DateTime timestamp;
			string severity;
			string additionalinfo; // This placeholder now includes the 'out' keyword

            // Call DAL: Data is retrieved and assigned to the local variables (passed by OUT)
            if (clsErrorLogData.GetErrorLogInfoByID(ErrorID, 
                out errorid, out errormessage, out stacktrace, out timestamp, out severity, out additionalinfo))
            {
                // Constructor Call: Local variables are now passed by VALUE to the constructor
                return new clsErrorLog(errorid, errormessage, stacktrace, timestamp, severity, additionalinfo);
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
                    if (AddNewErrorLog())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    break;

                case enMode.Update:
                    if (UpdateErrorLog())
                        return true;
                    break;
            }

            return false;
        }

        // ===============================
        // PRIVATE ADD / UPDATE METHODS
        // ===============================

        private bool AddNewErrorLog()
        {
            // Must map instance properties to DAL's parameters
            this.ErrorID = clsErrorLogData.InsertErrorLog(
            this.ErrorMessage,
				this.StackTrace,
				this.Timestamp,
				this.Severity,
				this.AdditionalInfo 
            );
            return (this.ErrorID != -1);
        }

        private bool UpdateErrorLog()
        {
            // Must map instance properties to DAL's parameters
            return clsErrorLogData.UpdateErrorLog(
            this.ErrorID,
				this.ErrorMessage,
				this.StackTrace,
				this.Timestamp,
				this.Severity,
				this.AdditionalInfo 
            );
        }


        // ===============================
        // DELETE METHOD
        // ===============================

        public bool Delete()
        {
            // Use the instance property value
            return clsErrorLogData.DeleteErrorLog(this.ErrorID);
        }

        //===============================
        // GetAll METHOD
        //===============================

        public static DataTable GetAllErrorLog()
        {
            return clsErrorLogData.GetAllErrorLog();
        }
        
        //===============================
        // OPTIONAL BIT CHECK METHODS
        //===============================

        

        public static bool  IsErrorLogExist(int ErrorID)
        {
            return clsErrorLogData.IsErrorLogExist(ErrorID);
        }


        
        public enum ErrorLogColumn
         {
            ErrorID,
ErrorMessage,
StackTrace,
Timestamp,
Severity,
AdditionalInfo,
         }


        public enum SearchMode
        {
            Anywhere,
            StartsWith,
            EndsWith,
            ExactMatch
        }
    

        public static DataTable SearchData(ErrorLogColumn ChosenColumn, string SearchValue, SearchMode Mode = SearchMode.Anywhere)
        {
            if (string.IsNullOrWhiteSpace(SearchValue) || !SqlHelper.IsSafeInput(SearchValue))
                return new DataTable();


            return clsErrorLogData.SearchData(ChosenColumn.ToString(), SearchValue, Mode.ToString());
        }        
        





    }
}
