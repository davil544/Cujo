using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;
using System;
using CujoPasswordManager.DataModels;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace CujoPasswordManager.DataAccessLayer
{
    public class AccountManager
    {
        private static SqlConnection conn; private static SqlCommand cmd;
        private static string connectionString, query;
        private static SqlDataReader reader;

        static AccountManager()
        {
            // Instantiate creds here and scrub them for SQL command
            connectionString = ConfigurationManager.ConnectionStrings["SiteData"].ToString();
        }

        // TODO:  Add encrypt / decrypt functions to store passwords and vault data securely

        public static Account Login(Account account)
        {
            if (account.username.Equals("") || account.password.Equals(""))
            {
                account.status = ErrorHandler.empty;
                return account;
            }

            string status = ErrorHandler.wrongPass;
            query = "SELECT UserID, Username, Password, FullName " +
                "FROM Users where Username = @Uname AND Password = @PW;";
            conn = new SqlConnection(connectionString);
            cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@Uname", account.username);
            cmd.Parameters.AddWithValue("@PW", account.password);

            try
            {
                conn.Open();
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (reader["Username"].ToString() == account.username && reader["password"].ToString() == account.password)
                        {
                            //This runs when a valid match is found in the database
                            status = "valid";

                            //This will run to retrieve the user's relevant data, change to vault data here
                            //account.FullName = reader["Name"].ToString();
                            account.ID = int.Parse(reader["UserID"].ToString());
                            account.name = reader["FullName"].ToString();
                            /*account.Password = reader["Password"].ToString();
                            account.URL = reader["URL"].ToString();
                            account.Notes = reader["Notes"].ToString();*/
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                status = ErrorHandler.SQL(ex);
            }
            finally
            {
                conn.Close();
            }

            account.status = status;
            return account;
        }

        public static string Register(Account account)
        {
            // Checks if fields contain data, prevents blank usernames or passwords
            if (account.username.Equals("") || account.password.Equals(""))
            {
                return ErrorHandler.empty;
            }

            query = "INSERT INTO Users (Username, Password) VALUES (@Uname, @PW);";
            conn = new SqlConnection(connectionString);
            cmd = new SqlCommand(query, conn);
            string status = ErrorHandler.failed;
            int rows;
            cmd.Parameters.AddWithValue("@Uname", account.username);
            cmd.Parameters.AddWithValue("@PW", account.password);

            try
            {
                conn.Open();
                rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    status = "success";
                }
            }
            catch (SqlException ex)
            {
                status = ErrorHandler.SQL(ex);
            }
            finally
            {
                conn.Close();
            }

            return status;
        }

        public static string UpdateAccount(Account account)
        {
            // Checks if fields contain data, prevents blank usernames or full names
            if (account.username.Equals("") || account.password.Equals(""))
            {
                return ErrorHandler.empty;
            }

            query = "UPDATE Users " +
                 "SET Name = @Name, Password = @PW " +
                 "WHERE Username = @Uname;";

            conn = new SqlConnection(connectionString);
            cmd = new SqlCommand(query, conn);
            account.status = ErrorHandler.failed;
            int rows;

            cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 50).Value = account.name;
            cmd.Parameters.Add("@PW", SqlDbType.NVarChar, 50).Value = account.password;



            try
            {
                conn.Open();
                rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    account.status = "success";
                }
            }
            catch (SqlException ex)
            {
                account.status = ErrorHandler.SQL(ex);
            }
            finally
            {
                conn.Close();
            }

            return account.status;
        }

        public static Vault[] GetVault(int UserID)
        {
            Vault[] vault = new Vault[1];

            query = "SELECT * FROM Vault WHERE UserID = @userID;";
            conn = new SqlConnection(connectionString);
            cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@userID", UserID);

            try
            {
                conn.Open();
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    int i = 0;
                    vault = new Vault[GetPasswordCount(UserID)];
                    while (reader.Read())
                    {
                        vault[i] = new Vault();
                        if (reader["ID"] != DBNull.Value)
                        {
                            vault[i].ID = (int)reader["ID"];
                        }
                        else
                        {
                            //This will cause a SQL exception, will be handled below
                        }

                        if (reader["ID"] != DBNull.Value)
                        {
                            vault[i].UserID = (int)reader["UserID"];
                        }

                        if (reader["URL"] != DBNull.Value)
                        {
                            vault[i].URL = reader["URL"].ToString();
                        }
                        else
                        {
                            //This code probably won't run ever, DB
                            //doesn't allow null data in this field
                            vault[i].URL = "No URL Entered";
                        }

                        if (reader["Username"] != DBNull.Value)
                        {
                            vault[i].Username = reader["Username"].ToString();
                        }
                        else
                        {
                            vault[i].Username = "No user entered";
                        }

                        if (reader["Password"] != DBNull.Value)
                        {
                            vault[i].Password = reader["Password"].ToString();
                        }
                        else
                        {
                            vault[i].Password = "password";
                        }

                        if (reader["Category"] != DBNull.Value)
                        {
                            vault[i].Category = reader["Category"].ToString();
                        }
                        else
                        {
                            vault[i].Category = String.Empty;
                        }

                        if (reader["Notes"] != DBNull.Value)
                        {
                            vault[i].Notes = reader["Notes"].ToString();
                        }
                        else
                        {
                            vault[i].Notes = string.Empty;
                        }
                        i++;
                    }
                }
            }
            catch (SqlException ex)
            {
                vault[0] = new Vault { URL = ErrorHandler.SQL(ex) };
            }
            finally
            {
                conn.Close();
            }
            return vault;
        }

        public static int GetPasswordCount(int UserID)
        {
            query = "SELECT COUNT(*) FROM Vault WHERE UserID = @userID;";
            conn = new SqlConnection(connectionString);
            cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@userID", UserID);
            int count;

            try
            {
                conn.Open();
                count = (int)cmd.ExecuteScalar();
            }
            catch (SqlException)
            {
                count = 0;
            }
            finally
            {
                conn.Close();
            }
            return count;
        }

        public static void initDB() {
            query = "CREATE TABLE 'Users' ( 'Username' NVARCHAR(MAX) NOT NULL ,'Password' NVARCHAR(MAX) NOT NULL ,  \r\n`Age` INT NOT NULL ," +
                "`Phone_No` VARCHAR(10) NOT NULL ,`Address` VARCHAR(100) NOT NULL ,\r\n PRIMARY KEY ('Username'));";
            // TODO:  Add 2nd query to create vault table as well, get rid of extra fields
            // To be continued once more progress has been made
        }
        
        // Will eventually use this for password hashing
        public void Password_Hash()
        {
            // Hashing function for password, will likely move this out of the main soon
            Console.Write("Enter a password: ");
            string password = Console.ReadLine();

            // generate a 128-bit salt using a secure PRNG
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            Console.WriteLine($"Salt: {Convert.ToBase64String(salt)}");

            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
            Console.WriteLine($"Hashed: {hashed}");
        }
    }
}