using System;
using System.Data;
using Microsoft.Data.SqlClient;
namespace SpendingTrackerDAL
{

    public class clsMerchantDTO
    {

        public clsMerchantDTO(int MerchantID, string Name, string Location)
        {
            this.MerchantID = MerchantID;
            this.Name = Name;
            this.Location = Location;

        }

        public int MerchantID { get; set; }

        public string Name { get; set; }
        public string Location { get; set; }  // String link using map or coordinates.

    }
    public class clsMerchantData
    {



        public static List<clsMerchantDTO> GetAllMerchants()
        {
            var MerchantsList = new List<clsMerchantDTO>();
            using (SqlConnection conn = new SqlConnection(clsDataAccessSettings._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_GetAllMerchants", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            MerchantsList.Add(new clsMerchantDTO
                                (
                                reader.GetInt32(reader.GetOrdinal("MerchantID")),
                                reader.GetString(reader.GetOrdinal("Name")),
                                reader.GetString(reader.GetOrdinal("Location"))
                                ));
                        }
                    }
                }
                return MerchantsList;
            }
        }
        public static int AddNewMerchant(clsMerchantDTO MerchantDTO)
        {
            using (var connection = new SqlConnection(clsDataAccessSettings._connectionString))
            using (var command = new SqlCommand("SP_AddNewMerchant", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Name", MerchantDTO.Name);
                command.Parameters.AddWithValue("@Location", MerchantDTO.Location);
                var outputIdParm = new SqlParameter("@NewMerchantID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                command.Parameters.Add(outputIdParm);

                connection.Open();
                command.ExecuteNonQuery();


                return (int)outputIdParm.Value;
            }
        }
        public static bool UpdateMerchant(clsMerchantDTO MerchantDTO)
        {
            using (var connection = new SqlConnection(clsDataAccessSettings._connectionString))
            using (var command = new SqlCommand("SP_UpdateMerchant", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@MerchantID", MerchantDTO.MerchantID);
                command.Parameters.AddWithValue("@Description", MerchantDTO.Name);
                command.Parameters.AddWithValue("@Location", MerchantDTO.Location);


                connection.Open();
                command.ExecuteNonQuery();

                return true;
            }
        }
        public static bool DeleteMerchant(int MerchantID)
        {

            using (var connection = new SqlConnection(clsDataAccessSettings._connectionString))
            using (var command = new SqlCommand("SP_DeleteMerchant", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@MerchantID", MerchantID);

                connection.Open();

                int rowsAffected = (int)command.ExecuteScalar();
                return (rowsAffected == 1);
            }
        }

        public static clsMerchantDTO GetMerchantByID(int MerchantID)
        {
            using (var connection = new SqlConnection(clsDataAccessSettings._connectionString))
            using (var command = new SqlCommand("SP_GetMerchantByID", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("MerchantID", MerchantID);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new clsMerchantDTO
                        (
                            reader.GetInt32(reader.GetOrdinal("MerchantID")),
                            reader.GetString(reader.GetOrdinal("Name")),
                            reader.GetString(reader.GetOrdinal("Location"))
                        );
                    }
                    else
                        return null;

                }

            }


        }




    }


}
