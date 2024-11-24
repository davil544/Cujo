using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;
using System;
using CujoPasswordManager.DataModels;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;
using System.IO;

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

        // TODO:  Implement encrypt / decrypt functions to store passwords and vault data securely

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

                        if (reader["ItemName"] != DBNull.Value)
                        {
                            vault[i].ItemName = reader["ItemName"].ToString();
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

                        /*if (reader["Password"] != DBNull.Value)
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
                        }*/
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

        public static Vault[] GetVault(int UserID, string SearchQuery)
        {
            Vault[] vault = new Vault[1];

            query = "Select * from Vault WHERE UserID = @userID AND (ItemName like @query OR Username like @query OR URL like @query OR Category like @query);";
            conn = new SqlConnection(connectionString);
            cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@userID", UserID);
            cmd.Parameters.AddWithValue("@query", "%" + SearchQuery + "%");  // %s are needed to search for partial matches

            try
            {
                conn.Open();
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    int i = 0;
                    vault = new Vault[GetPasswordCount(UserID)]; //Maybe overload this to get search query count instead so I don't need that other if not null statement
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

                        if (reader["ItemName"] != DBNull.Value)
                        {
                            vault[i].ItemName = reader["ItemName"].ToString();
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

                        /*if (reader["Password"] != DBNull.Value)
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
                        }*/
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

        public static Vault GetVault(int UserID, int EntryID)
        {
            Vault entry = new Vault
            {
                ID = EntryID
            };

            query = "SELECT * FROM Vault WHERE UserID = @userID AND ID = @ID;";
            conn = new SqlConnection(connectionString);
            cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@userID", UserID);
            cmd.Parameters.AddWithValue("@ID", EntryID);

            try
            {
                conn.Open();
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (reader["ID"] != DBNull.Value && (int)reader["ID"] != 0)
                        {
                            entry.ID = (int)reader["ID"];
                        }

                        if (reader["UserID"] != DBNull.Value)
                        {
                            entry.UserID = (int)reader["UserID"];
                        }

                        if (reader["ItemName"] != DBNull.Value)
                        {
                            entry.ItemName = reader["ItemName"].ToString();
                        }

                        if (reader["URL"] != DBNull.Value)
                        {
                            entry.URL = reader["URL"].ToString();
                        }

                        if (reader["Username"] != DBNull.Value)
                        {
                            entry.Username = reader["Username"].ToString();
                        }

                        if (reader["Password"] != DBNull.Value)
                        {
                            entry.Password = reader["Password"].ToString();
                        }

                        if (reader["Category"] != DBNull.Value)
                        {
                            entry.Category = reader["Category"].ToString();
                        }

                        if (reader["Notes"] != DBNull.Value)
                        {
                            entry.Notes = reader["Notes"].ToString();
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                entry = new Vault { URL = ErrorHandler.SQL(ex) };
            }
            finally
            {
                conn.Close();
            }
            return entry;
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

        public static string AddVaultEntry(Vault entry, int userID)
        {
            if (entry.Username.Equals("") || entry.Password.Equals("") || userID.Equals(null))
            {
                return ErrorHandler.empty;
            }

            query = "INSERT INTO Vault (ItemName, UserID, Username, Password";
            if (entry.URL != null) { query += ", URL"; }
            if (entry.Category != null) { query += ", Category"; }
            if (entry.Notes != null) { query += ", Notes"; }

            query += ") VALUES (@Iname, @UserID, @Uname, @PW";
            if (entry.URL != null) { query += ", @URL"; }
            if (entry.Category != null) { query += ", @Cat"; }
            if (entry.Notes != null) { query += ", @Notes"; }
            query += ");";

            conn = new SqlConnection(connectionString);
            cmd = new SqlCommand(query, conn);
            string status = ErrorHandler.failed;
            int rows;
            cmd.Parameters.AddWithValue("@Iname", entry.ItemName);
            cmd.Parameters.AddWithValue("@UserID", userID);
            cmd.Parameters.AddWithValue("@Uname", entry.Username);
            cmd.Parameters.AddWithValue("@PW", entry.Password);
            if (entry.URL != null) { cmd.Parameters.AddWithValue("@URL", entry.URL); }
            if (entry.Category != null) { cmd.Parameters.AddWithValue("@Cat", entry.Category); }
            if (entry.Notes != null) { cmd.Parameters.AddWithValue("@Notes", entry.Notes); }

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

        public static string UpdateVaultEntry(Vault entry, int userID)
        {
            if (entry.Username.Equals("") || entry.Password.Equals("") || userID.Equals(0))
            {
                return ErrorHandler.empty;
            }
            else if (entry.ID == 0)
            {
                return "Password ID not being passed to the DB, aborting!";
            }

            //Add check if every single field is blank, error out if so

            /*query = "UPDATE Vault " +
                 "SET Username = @Uname, Password = @PW " +
                 "WHERE ID = @ID;"; */

            query = "UPDATE Vault " +
                "SET ItemName = @Iname, Username = @Uname, Password = @PW";  //Tweak This
            if (entry.URL != null) { query += ", URL = @URL"; }
            if (entry.Category != null) { query += ", Category = @Cat"; }
            if (entry.Notes != null) { query += ", Notes = @Notes"; }

            query += " WHERE ID = @ID;";

            conn = new SqlConnection(connectionString);
            cmd = new SqlCommand(query, conn);
            string status = ErrorHandler.failed;
            int rows;
            cmd.Parameters.AddWithValue("@Iname", entry.ItemName);
            cmd.Parameters.AddWithValue("@UserID", userID);
            cmd.Parameters.AddWithValue("@ID", entry.ID);
            cmd.Parameters.AddWithValue("@Uname", entry.Username);
            cmd.Parameters.AddWithValue("@PW", entry.Password);
            if (entry.URL != null) { cmd.Parameters.AddWithValue("@URL", entry.URL); }
            if (entry.Category != null) { cmd.Parameters.AddWithValue("@Cat", entry.Category); }
            if (entry.Notes != null) { cmd.Parameters.AddWithValue("@Notes", entry.Notes); }

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
                //throw new Exception(ex.Message);
                status = ErrorHandler.SQL(ex);
            }
            finally
            {
                conn.Close();
            }

            return status;
        }

        public static void initDB() {
            query = "CREATE TABLE 'Users' ( 'Username' NVARCHAR(MAX) NOT NULL ,'Password' NVARCHAR(MAX) NOT NULL ,  \r\n`Age` INT NOT NULL ," +
                "`Phone_No` VARCHAR(10) NOT NULL ,`Address` VARCHAR(100) NOT NULL ,\r\n PRIMARY KEY ('Username'));";
            // TODO:  Add 2nd query to create vault table as well, get rid of extra fields
            // To be continued once more progress has been made
        }

        // TODO:  Write function to generate key with random salts to improve security for storing passwords
        // TODO:  Switch to a more secure Cipher Mode, ECB is vulnerable to rainbow table attacks!
        public static string Encrypt(string plainText, string key)
        {
            using (Aes aes = Aes.Create())
            {
                // Convert strings to byte arrays here to perform encryption
                byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

                // Hash key here to increase security
                var hash = new SHA256CryptoServiceProvider();
                byte[] hashedKey = hash.ComputeHash(Encoding.UTF8.GetBytes(key));

                aes.Key = hashedKey;
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.PKCS7;

                // Obsolete code, may delete in a future update
                /*using (ICryptoTransform encryptor = aes.CreateEncryptor())
                {
                    encryptedBytes = encryptor.TransformFinalBlock(plainTextByteArray, 0, plainTextByteArray.Length);
                }*/

                // Encryption is performed here
                byte[] encryptedBytes = null;
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(plainTextBytes, 0, plainTextBytes.Length);
                    }

                    encryptedBytes = ms.ToArray();
                }

                // Encode to string here to be stored properly
                return Convert.ToBase64String(encryptedBytes);
            }

        }

        public static string Decrypt(string cipherText, string key)
        {
            using (Aes aes = Aes.Create())
            {
                // Convert strings to byte arrays here to perform decryption
                byte[] cipherBytes = Convert.FromBase64String(cipherText);

                // Hash key here to increase security
                var hash = new SHA256CryptoServiceProvider();
                byte[] hashedKey = hash.ComputeHash(Encoding.UTF8.GetBytes(key));

                aes.Key = hashedKey;
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.PKCS7;

                // Obsolete code, may delete in a future update
                /*using (ICryptoTransform decryptor = aes.CreateDecryptor())
                {
                    decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                }*/

                // Decryption is performed here
                byte[] decryptedBytes = null;
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                    }

                    decryptedBytes = ms.ToArray();
                }

                // Decodes to string here to be displayed properly
                return Encoding.UTF8.GetString(decryptedBytes);
            }
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
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
            Console.WriteLine($"Hashed: {hashed}");
        }
    }
}