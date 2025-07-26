using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace SpendingTrackerDAL
{
    public class clsUserDTO
    {
        public clsUserDTO(int UserID, string FirstName, string LastName, string Email, string Password)
        {
            this.UserID = UserID;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Email = Email;
            this.Password = Password;
        }

        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class clsLoginDTO
    {
        public clsLoginDTO(string Email, string Password)
        {
            this.Email = Email;
            this.Password = Password;
        }

        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class clsUserData
    {
        public static int AddNewUser(clsUserDTO UserDTO)
        {
            using (var connection = new SqlConnection(clsDataAccessSettings._connectionString))
            using (var command = new SqlCommand("SP_AddNewUser", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@FirstName", UserDTO.FirstName);
                command.Parameters.AddWithValue("@LastName", UserDTO.LastName);
                command.Parameters.AddWithValue("@Email", UserDTO.Email);
                command.Parameters.AddWithValue("@Password", UserDTO.Password);

                var outputIdParm = new SqlParameter("@NewUserID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                command.Parameters.Add(outputIdParm);

                connection.Open();
                command.ExecuteNonQuery();

                return (int)outputIdParm.Value;
            }
        }

        public static bool UpdateUser(clsUserDTO UserDTO)
        {
            using (var connection = new SqlConnection(clsDataAccessSettings._connectionString))
            using (var command = new SqlCommand("SP_UpdateUser", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserID", UserDTO.UserID);
                command.Parameters.AddWithValue("@FirstName", UserDTO.FirstName);
                command.Parameters.AddWithValue("@LastName", UserDTO.LastName);
                command.Parameters.AddWithValue("@Email", UserDTO.Email);
                command.Parameters.AddWithValue("@Password", UserDTO.Password);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public static bool DeleteUser(int userID)
        {
            using (var connection = new SqlConnection(clsDataAccessSettings._connectionString))
            using (var command = new SqlCommand("SP_DeleteUser", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserID", userID);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public static clsUserDTO GetUserByID(int userID)
        {
            using (var connection = new SqlConnection(clsDataAccessSettings._connectionString))
            using (var command = new SqlCommand("SP_GetUserByID", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserID", userID);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new clsUserDTO(
                            reader.GetInt32(reader.GetOrdinal("UserID")),
                            reader.GetString(reader.GetOrdinal("FirstName")),
                            reader.GetString(reader.GetOrdinal("LastName")),
                            reader.GetString(reader.GetOrdinal("Email")),
                            null
                        );
                    }
                }
            }
            return null;
        }

        public static clsUserDTO AuthenticateUser(string email, string password)
        {
            using (var connection = new SqlConnection(clsDataAccessSettings._connectionString))
            using (var command = new SqlCommand("SP_AuthenticateUser", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Password", password);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new clsUserDTO(
                            reader.GetInt32(reader.GetOrdinal("UserID")),
                            reader.GetString(reader.GetOrdinal("FirstName")),
                            reader.GetString(reader.GetOrdinal("LastName")),
                            reader.GetString(reader.GetOrdinal("Email")),
                            null
                        );
                    }
                }
            }
            return null;
        }
    }
}