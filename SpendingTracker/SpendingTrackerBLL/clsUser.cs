using System.Data;
using SpendingTrackerDAL;

namespace SpendingTrackerBLL
{
    public class clsUser
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public clsUserDTO uDTO
        {
            get { return new clsUserDTO(this.UserID, this.FirstName, this.LastName, this.Email, this.Password); }
        }

        public clsUser(clsUserDTO uDTO, enMode cMode = enMode.AddNew)
        {
            this.UserID = uDTO.UserID;
            this.FirstName = uDTO.FirstName;
            this.LastName = uDTO.LastName;
            this.Email = uDTO.Email;
            this.Password = uDTO.Password;
            this.Mode = cMode;
        }

        private bool _AddNewUser()
        {
            this.UserID = clsUserData.AddNewUser(uDTO);
            return (this.UserID != 0);
        }

        private bool _UpdateUser()
        {
            return clsUserData.UpdateUser(uDTO);
        }

        public static bool DeleteUser(int ID)
        {
            return clsUserData.DeleteUser(ID);
        }

        public static clsUser Find(int ID)
        {
            clsUserDTO eDTO = clsUserData.GetUserByID(ID);
            return eDTO != null ? new clsUser(eDTO, enMode.Update) : null;
        }

        public static clsUser Find(string Email, string Password)
        {
            clsUserDTO eDTO = clsUserData.AuthenticateUser(Email, Password);
            return eDTO != null ? new clsUser(eDTO, enMode.Update) : null;
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewUser())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    return false;

                case enMode.Update:
                    return _UpdateUser();
            }
            return false;
        }
    }
}