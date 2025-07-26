using System;
using System.Data;
using System.Data.SqlTypes;
using System.Text.Json;
using Microsoft.Data.SqlClient;

namespace SpendingTrackerDAL
{
    public class clsBudgetDTO
    {
        public int BudgetID { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public SqlDateTime DateTimeFrom { get; set; }
        public SqlDateTime DateTimeTo { get; set; }
        public bool BudgetStatus { get; set; }

        public clsBudgetDTO(int budgetID, string name, double amount, SqlDateTime dateTimeFrom, SqlDateTime dateTimeTo, bool budgetStatus)
        {
            BudgetID = budgetID;
            Name = name;
            Amount = amount;
            DateTimeFrom = dateTimeFrom;
            DateTimeTo = dateTimeTo;
            BudgetStatus = budgetStatus;
        }
    }
    public class clsBudgetData
    {

        public static List<clsBudgetDTO> GetAllBudgets()
        {
            var BudgetsList = new List<clsBudgetDTO>();

            using (SqlConnection conn = new SqlConnection(clsDataAccessSettings._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_GetAllBudgets", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int budgetID = reader.IsDBNull(reader.GetOrdinal("BudgetID")) ? 0 : reader.GetInt32(reader.GetOrdinal("BudgetID"));
                            string name = reader.IsDBNull(reader.GetOrdinal("Name")) ? string.Empty : reader.GetString(reader.GetOrdinal("Name"));
                            double amount = reader.IsDBNull(reader.GetOrdinal("Amount")) ? 0.0 : (double)reader.GetDecimal(reader.GetOrdinal("Amount"));
                            SqlDateTime dateTimeFrom = reader.IsDBNull(reader.GetOrdinal("DateTimeFrom")) ? SqlDateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("DateTimeFrom"));
                            SqlDateTime dateTimeTo = reader.IsDBNull(reader.GetOrdinal("DateTimeTo")) ? SqlDateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("DateTimeTo"));
                            bool budgetStatus = reader.IsDBNull(reader.GetOrdinal("BudgetStatus")) ? false : reader.GetBoolean(reader.GetOrdinal("BudgetStatus"));

                            BudgetsList.Add(new clsBudgetDTO(budgetID, name, amount, dateTimeFrom, dateTimeTo, budgetStatus));
                        }
                    }
                }

                return BudgetsList;
            }
        }
        public static List<clsBudgetDTO> GetValidBudgets()
        {
            var ValidBudgetsList = new List<clsBudgetDTO>();

            using (SqlConnection conn = new SqlConnection(clsDataAccessSettings._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_GetValidBudgets", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int budgetID = reader.IsDBNull(reader.GetOrdinal("BudgetID")) ? 0 : reader.GetInt32(reader.GetOrdinal("BudgetID"));
                            string name = reader.IsDBNull(reader.GetOrdinal("Name")) ? string.Empty : reader.GetString(reader.GetOrdinal("Name"));
                            double amount = reader.IsDBNull(reader.GetOrdinal("Amount")) ? 0.0 : (double)reader.GetDecimal(reader.GetOrdinal("Amount"));
                            SqlDateTime dateTimeFrom = reader.IsDBNull(reader.GetOrdinal("DateTimeFrom")) ? SqlDateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("DateTimeFrom"));
                            SqlDateTime dateTimeTo = reader.IsDBNull(reader.GetOrdinal("DateTimeTo")) ? SqlDateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("DateTimeTo"));
                            bool budgetStatus = reader.IsDBNull(reader.GetOrdinal("BudgetStatus")) ? false : reader.GetBoolean(reader.GetOrdinal("BudgetStatus"));

                            ValidBudgetsList.Add(new clsBudgetDTO(budgetID, name, amount, dateTimeFrom, dateTimeTo, budgetStatus));
                        }
                    }
                }

                return ValidBudgetsList;
            }
        }
        public static clsBudgetDTO GetBudgetByID(int budgetID)
        {
            using (var connection = new SqlConnection(clsDataAccessSettings._connectionString))
            using (var command = new SqlCommand("SP_GetBudgetByID", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@BudgetID", budgetID);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int id = reader.IsDBNull(reader.GetOrdinal("BudgetID")) ? 0 : reader.GetInt32(reader.GetOrdinal("BudgetID"));
                        string name = reader.IsDBNull(reader.GetOrdinal("Name")) ? string.Empty : reader.GetString(reader.GetOrdinal("Name"));
                        double amount = reader.IsDBNull(reader.GetOrdinal("Amount")) ? 0.0 : (double)reader.GetDecimal(reader.GetOrdinal("Amount"));
                        SqlDateTime dateTimeFrom = reader.IsDBNull(reader.GetOrdinal("DateTimeFrom")) ? SqlDateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("DateTimeFrom"));
                        SqlDateTime dateTimeTo = reader.IsDBNull(reader.GetOrdinal("DateTimeTo")) ? SqlDateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("DateTimeTo"));
                        bool budgetStatus = reader.IsDBNull(reader.GetOrdinal("BudgetStatus")) ? false : reader.GetBoolean(reader.GetOrdinal("BudgetStatus"));

                        return new clsBudgetDTO(id, name, amount, dateTimeFrom, dateTimeTo, budgetStatus);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
        public static int AddBudget(clsBudgetDTO BudgetDTO)
        {
            using (var connection = new SqlConnection(clsDataAccessSettings._connectionString))
            using (var command = new SqlCommand("SP_AddBudget", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Name", BudgetDTO.Name);
                command.Parameters.AddWithValue("@Amount", BudgetDTO.Amount);
                command.Parameters.AddWithValue("@DateTimeFrom", BudgetDTO.DateTimeFrom);
                command.Parameters.AddWithValue("@DateTimeTo", BudgetDTO.DateTimeTo);
                command.Parameters.AddWithValue("@BudgetStatus", BudgetDTO.BudgetStatus);
                var outputIdParm = new SqlParameter("@NewBudgetID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                command.Parameters.Add(outputIdParm);

                connection.Open();
                command.ExecuteNonQuery();

                return (int)outputIdParm.Value;
            }
        }

        public static bool UpdateBudget(clsBudgetDTO BudgetDTO)
        {
            using (var connection = new SqlConnection(clsDataAccessSettings._connectionString))
            using (var command = new SqlCommand("SP_UpdateBudget", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@BudgetID", BudgetDTO.BudgetID);

                // Check for NULL values before setting parameters
                command.Parameters.Add("@Name", SqlDbType.VarChar).Value = (object)BudgetDTO.Name ?? DBNull.Value;
                command.Parameters.Add("@Amount", SqlDbType.Decimal).Value = (object)BudgetDTO.Amount ?? DBNull.Value;

                // Convert DateTime to ISO 8601 formatted string
                string dateTimeFromString = BudgetDTO.DateTimeFrom.ToString();
                string dateTimeToString = BudgetDTO.DateTimeTo.ToString();

                // Convert ISO 8601 formatted strings to SqlDateTime
                SqlDateTime sqlDateTimeFrom = SqlDateTime.Parse(dateTimeFromString);
                SqlDateTime sqlDateTimeTo = SqlDateTime.Parse(dateTimeToString);

                command.Parameters.Add("@DateTimeFrom", SqlDbType.DateTime).Value = sqlDateTimeFrom;
                command.Parameters.Add("@DateTimeTo", SqlDbType.DateTime).Value = sqlDateTimeTo;

                command.Parameters.Add("@BudgetStatus", SqlDbType.Bit).Value = (object)BudgetDTO.BudgetStatus ?? DBNull.Value;

                connection.Open();
                command.ExecuteNonQuery();

                return true;
            }
        }
        public static bool DeleteBudget(int budgetID)
        {
            using (var connection = new SqlConnection(clsDataAccessSettings._connectionString))
            using (var command = new SqlCommand("SP_DeleteBudget", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@BudgetID", budgetID);

                connection.Open();

                int rowsAffected = (int)command.ExecuteScalar();
                return (rowsAffected == 1);
            }
        }

    }
}
