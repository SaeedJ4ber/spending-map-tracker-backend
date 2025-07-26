using System.Data;
using SpendingTrackerDAL;

namespace SpendingTrackerBLL
{
    public class clsMerchant
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int MerchantID { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }  // String link using map or coordinates.



        public clsMerchantDTO mDTO
        {
            get { return (new clsMerchantDTO(this.MerchantID, this.Name, this.Location)); }
        }

        public clsMerchant(clsMerchantDTO mDTO, enMode cMode = enMode.AddNew)
        {
            this.MerchantID = mDTO.MerchantID;
            this.Name = mDTO.Name;
            this.Location = mDTO.Location;


            Mode = cMode;
        }

        public static List<clsMerchantDTO> GetAllMerchants()
        {
            return clsMerchantData.GetAllMerchants();
        }

        private bool _AddNewMerchant()
        {
            this.MerchantID = clsMerchantData.AddNewMerchant(mDTO);


            return (this.MerchantID != 1);
        }
        private bool _UpdateMerchant()
        {
            return clsMerchantData.UpdateMerchant(mDTO);
        }
        public static bool DeleteMerchant(int ID)
        {
            return clsMerchantData.DeleteMerchant(ID);
        }
        public static clsMerchant Find(int ID)
        {
            clsMerchantDTO mDTO = clsMerchantData.GetMerchantByID(ID);

            if (mDTO != null)
            {
                return new clsMerchant(mDTO, enMode.Update);
            }
            else
                return null;
        }
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewMerchant())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateMerchant();

            }
            return false;
        }



    }
}
