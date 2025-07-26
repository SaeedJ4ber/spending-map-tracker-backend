using System.Data;
using System.Data.SqlTypes;
using SpendingTrackerDAL;

namespace SpendingTrackerBLL
{
    public class clsExpense
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int ExpenseID { get; set; }
        public string Description { get; set; }
        public int Amount { get; set; }
        public string Location { get; set; }  // String link using map or coordinates.
        public string Merchant { get; set; }
        public string Category { get; set; }
        public string DateTime { get; set; }
        


        public clsExpenseDTO eDTO
        {
            get { return (new clsExpenseDTO(this.ExpenseID, this.Amount, this.Location, this.Merchant, this.Category, this.DateTime)); }
        }

        public clsExpense(clsExpenseDTO eDTO, enMode cMode = enMode.AddNew)
        {
            this.ExpenseID = eDTO.ExpenseID;
            this.Amount = eDTO.Amount;
            this.Location = eDTO.Location;
            this.Merchant = eDTO.Merchant;
            this.Category = eDTO.Category;
            this.DateTime = eDTO.DateTime;
           

            Mode = cMode;
        }

        public static List<clsExpenseDTO> GetAllExpenses()
        {
            return clsExpenseData.GetAllExpenses();
        }

        private bool _AddNewExpense()
        {
            this.ExpenseID = clsExpenseData.AddNewExpense(eDTO);


            return (this.ExpenseID != 1);
        }
        private bool _UpdateExpense()
        {
            return clsExpenseData.UpdateExpense(eDTO);
        }
        public static bool DeleteExpense(int ID)
        {
            return clsExpenseData.DeleteExpense(ID);
        }
        public static clsExpense Find(int ID)
        {
            clsExpenseDTO eDTO = clsExpenseData.GetExpenseByID(ID);

            if (eDTO != null)
            {
                return new clsExpense(eDTO, enMode.Update);
            }
            else
                return null;
        }
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewExpense())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateExpense();

            }
            return false;
        }



    }
}
