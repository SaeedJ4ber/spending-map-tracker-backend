using System.Data;
using System.Data.SqlTypes;
using SpendingTrackerDAL;

namespace SpendingTrackerBLL
{
    public class clsBudget
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int BudgetID { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public SqlDateTime DateTimeFrom { get; set; }
        public SqlDateTime DateTimeTo { get; set; }
        public bool BudgetStatus { get; set; }
        public clsBudgetDTO bDTO
        {
            get { return (new clsBudgetDTO(this.BudgetID, this.Name, this.Amount, this.DateTimeFrom, this.DateTimeTo, this.BudgetStatus)); }
        }

        public clsBudget(clsBudgetDTO bDTO, enMode cMode = enMode.AddNew)
        {
            this.BudgetID = bDTO.BudgetID;
            this.Name = bDTO.Name;
            this.Amount = bDTO.Amount;
            this.DateTimeFrom = bDTO.DateTimeFrom;
            this.DateTimeTo = bDTO.DateTimeTo;
            this.BudgetStatus = bDTO.BudgetStatus;

            Mode = cMode;
        }

        private bool _AddNewBudget()
        {
            this.BudgetID = clsBudgetData.AddBudget(bDTO);
            return (this.BudgetID != 1);
        }

        private bool _UpdateBudget()
        {
            return clsBudgetData.UpdateBudget(bDTO);
        }

        public static bool DeleteBudget(int ID)
        {
            return clsBudgetData.DeleteBudget(ID);
        }


        public static List<clsBudgetDTO> GetAllBudgets()
        {
            return clsBudgetData.GetAllBudgets();
        }

        public static List<clsBudgetDTO> GetValidBudgets()
        {
            return clsBudgetData.GetValidBudgets();
        }


        public static clsBudget Find(int ID)
        {
            clsBudgetDTO bDTO = clsBudgetData.GetBudgetByID(ID);

            if (bDTO != null)
            {
                return new clsBudget(bDTO, enMode.Update);
            }
            else
                return null;
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewBudget())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateBudget();

            }
            return false;
        }


    }
}
