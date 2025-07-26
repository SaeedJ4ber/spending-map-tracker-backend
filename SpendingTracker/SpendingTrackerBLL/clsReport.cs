using System.Data;
using System.Data.SqlTypes;
using SpendingTrackerDAL;

namespace SpendingTrackerBLL
{
    public class clsReport
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int ReportID { get; set; }
        public string Place { get; set; }
        public SqlDecimal Amount { get; set; }
      



        public clsReportDTO rDTO
        {
            get { return (new clsReportDTO(this.ReportID, this.Place, this.Amount)); }
        }

        public clsReport(clsReportDTO eDTO, enMode cMode = enMode.AddNew)
        {
            this.ReportID = rDTO.ReportID;
            this.Place = rDTO.Place;
            this.Amount = rDTO.Amount;
            


            Mode = cMode;
        }

        public static List<clsReportDTO> GetAllReports()
        {
            return clsReportData.GetAllReports();
        }

        private bool _AddNewReport()
        {
            this.ReportID = clsReportData.AddNewReport(rDTO);


            return (this.ReportID != 1);
        }
        private bool _UpdateReport()
        {
            return clsReportData.UpdateReport(rDTO);
        }
        public static bool DeleteReport(int ID)
        {
            return clsReportData.DeleteReport(ID);
        }
        public static clsReport Find(int ID)
        {
            clsReportDTO rDTO = clsReportData.GetReportByID(ID);

            if (rDTO != null)
            {
                return new clsReport(rDTO, enMode.Update);
            }
            else
                return null;
        }
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewReport())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateReport();

            }
            return false;
        }



    }
}
