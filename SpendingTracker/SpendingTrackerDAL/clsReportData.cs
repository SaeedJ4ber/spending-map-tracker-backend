using System;
using System.Data;
using System.Data.SqlTypes;
using Microsoft.Data.SqlClient;
namespace SpendingTrackerDAL
{

    public class clsReportDTO
    {

        public clsReportDTO(int ReportID, string Place, SqlDecimal Amount)
        {
            this.ReportID = ReportID;
            this.Place = Place;
            this.Amount = Amount;
            

        }

        public int ReportID { get; set; }
        public string Place { get; set; }
        public SqlDecimal Amount { get; set; }
       



    }
    public class clsReportData
    {



        public static List<clsReportDTO> GetAllReports()
        {
            var ReportsList = new List<clsReportDTO>();
            using (SqlConnection conn = new SqlConnection(clsDataAccessSettings._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_GetAllReports", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ReportsList.Add(new clsReportDTO
                                (
                                reader.GetInt32(reader.GetOrdinal("ReportID")),
                                reader.GetString(reader.GetOrdinal("Place")),
                                reader.GetSqlDecimal(reader.GetOrdinal("Amount"))
                                ));
                        }
                    }
                }
                return ReportsList;
            }
        }
        public static int AddNewReport(clsReportDTO ReportDTO)
        {
            using (var connection = new SqlConnection(clsDataAccessSettings._connectionString))
            using (var command = new SqlCommand("SP_AddNewReport", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Place", ReportDTO.Place);
                command.Parameters.AddWithValue("@Amount", ReportDTO.Amount);
                
                var outputIdParm = new SqlParameter("@NewReportID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                command.Parameters.Add(outputIdParm);

                connection.Open();
                command.ExecuteNonQuery();


                return (int)outputIdParm.Value;
            }
        }
        public static bool UpdateReport(clsReportDTO ReportDTO)
        {
            using (var connection = new SqlConnection(clsDataAccessSettings._connectionString))
            using (var command = new SqlCommand("SP_UpdateReport", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ReportID", ReportDTO.ReportID);
                command.Parameters.AddWithValue("@Place", ReportDTO.Place);
                command.Parameters.AddWithValue("@Amount", ReportDTO.Amount);
             
                connection.Open();
                command.ExecuteNonQuery();

                return true;
            }
        }
        public static bool DeleteReport(int ReportID)
        {

            using (var connection = new SqlConnection(clsDataAccessSettings._connectionString))
            using (var command = new SqlCommand("SP_DeleteReport", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ReportID", ReportID);

                connection.Open();

                int rowsAffected = (int)command.ExecuteScalar();
                return (rowsAffected == 1);
            }
        }

        public static clsReportDTO GetReportByID(int ReportID)
        {
            using (var connection = new SqlConnection(clsDataAccessSettings._connectionString))
            using (var command = new SqlCommand("SP_GetReportByID", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("ReportID", ReportID);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new clsReportDTO
                        (
                            reader.GetInt32(reader.GetOrdinal("ReportID")),
                            reader.GetString(reader.GetOrdinal("Place")),
                            reader.GetSqlDecimal(reader.GetOrdinal("Amount"))
                            
                        );
                    }
                    else
                        return null;

                }

            }


        }




    }


}
