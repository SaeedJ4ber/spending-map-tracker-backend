using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpendingTrackerDAL;

namespace SpendingTrackerBLL
{
    public class clsCategory
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int ID { get; set; }
        public string CategoryName { get; set; }
       
        public CategoryDTO cDTO
        {
            get { return (new CategoryDTO(this.ID, this.CategoryName)); }
        }

        public clsCategory(CategoryDTO cDTO, enMode cMode = enMode.AddNew)  //cMode = Current Mode
        {
            this.ID = cDTO.Id;
            this.CategoryName = cDTO.CategoryName;
            

            Mode = cMode;
        }

        private bool _AddNewCategory()
        {
            this.ID = clsCategoryData.AddCategory(cDTO);
            return (this.ID != 1);
        }

        private bool _UpdateCategory()
        {
            return clsCategoryData.UpdateCategory(cDTO);
        }

        public static bool DeleteCategory(int ID)
        {
            return clsCategoryData.DeleteCategory(ID);
        }


        public static List<CategoryDTO> GetAllCategories()
        {
            return clsCategoryData.GetAllCategories();
        }

 
        public static clsCategory Find(int ID)
        {
            CategoryDTO cDTO = clsCategoryData.GetCategoryById(ID);

            if (cDTO != null)
            {
                return new clsCategory(cDTO, enMode.Update);
            }
            else
                return null;
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewCategory())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateCategory();

            }
            return false;
        }


    }

}
