using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpendingTrackerDAL
{
    public class CategoryDTO
    {
        public CategoryDTO(int id, string categoryName)
        {
            this.Id = id;
            this.CategoryName = categoryName;
            
        }


        public int Id { get; set; }
        public string CategoryName { get; set; }

    }
    public class clsCategoryData
    {

        public static List<CategoryDTO> GetAllCategories()
        {
            var CategoriesList = new List<CategoryDTO>();
            using (SqlConnection conn = new SqlConnection(clsDataAccessSettings._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_GetAllCategories", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CategoriesList.Add(new CategoryDTO
                                (
                                reader.GetInt32(reader.GetOrdinal("CategoryID")),
                                reader.GetString(reader.GetOrdinal("CategoryName"))
                                ));


                        }
                    }
                }
                return CategoriesList;

            }
        }

        public static CategoryDTO GetCategoryById(int CategoryId)
        {
            using (var connection = new SqlConnection(clsDataAccessSettings._connectionString))
            using (var command = new SqlCommand("SP_GetCategoryById", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("CategoryId", CategoryId);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new CategoryDTO
                        (
                            reader.GetInt32(reader.GetOrdinal("CategoryID")),
                            reader.GetString(reader.GetOrdinal("CategoryName"))
                        );
                    }
                    else
                        return null;

                }

            }


        }

        public static int AddCategory(CategoryDTO CategoryDTO)
        {
            using (var connection = new SqlConnection(clsDataAccessSettings._connectionString))
            using (var command = new SqlCommand("SP_AddCategory", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@CategoryName", CategoryDTO.CategoryName);
                
                var outputIdParm = new SqlParameter("@NewCategoryId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                command.Parameters.Add(outputIdParm);

                connection.Open();
                command.ExecuteNonQuery();

                return (int)outputIdParm.Value;
            }
        }

        public static bool UpdateCategory(CategoryDTO CategoryDTO)
        {
            using (var connection = new SqlConnection(clsDataAccessSettings._connectionString))
            using (var command = new SqlCommand("SP_UpdateCategory", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@CategoryID", CategoryDTO.Id);
                command.Parameters.AddWithValue("@CategoryName", CategoryDTO.CategoryName);

                connection.Open();
                command.ExecuteNonQuery();

                return true;
            }
        }

        public static bool DeleteCategory(int CategoryId)
        {
            using (var connection = new SqlConnection(clsDataAccessSettings._connectionString))
            using (var command = new SqlCommand("SP_DeleteCategory", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CategoryId", CategoryId);

                connection.Open();

                int rowsAffected = (int)command.ExecuteScalar();
                return (rowsAffected == 1);
            }
        }

    }
}
