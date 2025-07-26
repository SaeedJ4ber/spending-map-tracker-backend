using System;
using System.Data;
using System.Data.SqlTypes;
using Microsoft.Data.SqlClient;
namespace SpendingTrackerDAL
{

    public class clsExpenseDTO
    {

        public clsExpenseDTO(int ExpenseID, int Amount, string Location, string Merchant, string Category, string DateTime) 
        {
            this.ExpenseID = ExpenseID;
            this.Amount = Amount;
            this.Location = Location;
            this.Merchant = Merchant;
            this.Category = Category;
            this.DateTime = DateTime;
            
        }

        public int ExpenseID { get; set; }
        
        public int Amount { get; set; }
        public string Location { get; set; }  // String link using map or coordinates.

        public string Merchant { get; set; }
        public string Category { get; set; }
        public string DateTime { get; set; }

        
        
    }
    public class clsExpenseData
    {



        public static List<clsExpenseDTO> GetAllExpenses()
        {
            var ExpensesList = new List<clsExpenseDTO>();
            using (SqlConnection conn = new SqlConnection(clsDataAccessSettings._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_GetAllExpenses", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ExpensesList.Add(new clsExpenseDTO
                                (
                                reader.GetInt32(reader.GetOrdinal("ExpenseID")),
                                reader.GetInt32(reader.GetOrdinal("Amount")),
                                reader.GetString(reader.GetOrdinal("Location")),
                                reader.GetString(reader.GetOrdinal("Merchant")),
                                reader.GetString(reader.GetOrdinal("Category")),
                                reader.GetString(reader.GetOrdinal("DateTime"))
                                ));
                        }
                    }
                }
                return ExpensesList;
            }
        }
        public static int AddNewExpense(clsExpenseDTO expenseDTO)
        {
            using (var connection = new SqlConnection(clsDataAccessSettings._connectionString))
            using (var command = new SqlCommand("SP_AddNewExpense1", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Amount", expenseDTO.Amount);
                command.Parameters.AddWithValue("@Location", expenseDTO.Location);
                command.Parameters.AddWithValue("@Merchant", expenseDTO.Merchant);
                command.Parameters.AddWithValue("@Category", expenseDTO.Category);
                command.Parameters.AddWithValue("@DateTime", expenseDTO.DateTime);
                var outputIdParm = new SqlParameter("@NewExpenseID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                command.Parameters.Add(outputIdParm);

                connection.Open();
                command.ExecuteNonQuery();


                return (int)outputIdParm.Value;
            }
        }
        public static bool UpdateExpense(clsExpenseDTO expenseDTO)
        {
            using (var connection = new SqlConnection(clsDataAccessSettings._connectionString))
            using (var command = new SqlCommand("SP_UpdateExpense", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ExpenseID", expenseDTO.ExpenseID);
                command.Parameters.AddWithValue("@Amount", expenseDTO.Amount);
                command.Parameters.AddWithValue("@Location", expenseDTO.Location);
                command.Parameters.AddWithValue("@Location", expenseDTO.Merchant);
                command.Parameters.AddWithValue("@Location", expenseDTO.Category);
                command.Parameters.AddWithValue("@DateTime", expenseDTO.DateTime);


                connection.Open();
                command.ExecuteNonQuery();

                return true;
            }
        }
        public static bool DeleteExpense(int ExpenseID)
        {

            using (var connection = new SqlConnection(clsDataAccessSettings._connectionString))
            using (var command = new SqlCommand("SP_DeleteExpense", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ExpenseID", ExpenseID);

                connection.Open();

                int rowsAffected = (int)command.ExecuteScalar();
                return (rowsAffected == 1);
            }
        }
        
        public static clsExpenseDTO GetExpenseByID(int ExpenseID)
        {
            using (var connection = new SqlConnection(clsDataAccessSettings._connectionString))
            using (var command = new SqlCommand("SP_GetExpenseByID", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("ExpenseID", ExpenseID);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new clsExpenseDTO
                        (
                            reader.GetInt32(reader.GetOrdinal("ExpenseID")),
                            reader.GetInt32(reader.GetOrdinal("Amount")),
                            reader.GetString(reader.GetOrdinal("Location")),
                            reader.GetString(reader.GetOrdinal("Merchant")),
                            reader.GetString(reader.GetOrdinal("Category")),
                            reader.GetString(reader.GetOrdinal("DateTime"))
                        );
                    }
                    else
                        return null;

                }

            }


        }




    }


}
