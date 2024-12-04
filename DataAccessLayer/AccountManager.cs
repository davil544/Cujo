using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;
using System;
using CujoPasswordManager.DataModels;
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

        public static Account Login(Account account)
        {
            if (account.username.Equals("") || account.password.Equals(""))
            {
                account.status = ErrorHandler.empty;
                return account;
            }

            account.password = CustomFunctions.HashToSHA512(account.password);

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

                            //This will run to retrieve the user's account data
                            account.ID = int.Parse(reader["UserID"].ToString());
                            //account.password = reader["Password"].ToString();
                            account.name = reader["FullName"].ToString();
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

            query = "INSERT INTO Users (Username, Password";
            if (account.name != "") { query += ", FullName"; }
            query += ") VALUES (@Uname, @PW";
            if (account.name != "") { query += ", @Name"; }
            query += ");";
            conn = new SqlConnection(connectionString);
            cmd = new SqlCommand(query, conn);
            string status = ErrorHandler.failed;
            int rows;
            cmd.Parameters.AddWithValue("@Uname", account.username);
            cmd.Parameters.AddWithValue("@PW", CustomFunctions.HashToSHA512(account.password));
            if (account.name != "") { cmd.Parameters.AddWithValue("@Name", account.name); }

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

            cmd.Parameters.Add("@Uname", SqlDbType.NVarChar, 50).Value = account.username;
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

        public static Vault[] GetVault(int UserID, string encryptionKey)
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

                        if (reader["ItemName"] != DBNull.Value)
                        {
                            vault[i].ItemName = Decrypt(reader["ItemName"].ToString(), encryptionKey);
                        }

                        if (reader["URL"] != DBNull.Value)
                        {
                            vault[i].URL = Decrypt(reader["URL"].ToString(), encryptionKey);
                        }

                        if (reader["Username"] != DBNull.Value)
                        {
                            vault[i].Username = Decrypt(reader["Username"].ToString(), encryptionKey);
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

        public static Vault[] GetVault(int UserID, string SearchQuery, string encryptionKey)
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

                        if (reader["ItemName"] != DBNull.Value)
                        {
                            vault[i].ItemName = Decrypt(reader["ItemName"].ToString(), encryptionKey);
                        }

                        if (reader["URL"] != DBNull.Value)
                        {
                            vault[i].URL = Decrypt(reader["URL"].ToString(), encryptionKey);
                        }

                        if (reader["Username"] != DBNull.Value)
                        {
                            vault[i].Username = Decrypt(reader["Username"].ToString(), encryptionKey);
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

        public static Vault GetVault(int UserID, int EntryID, string encryptionKey)
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
                            entry.ItemName = Decrypt(reader["ItemName"].ToString(), encryptionKey);
                        }

                        if (reader["URL"] != DBNull.Value)
                        {
                            entry.URL = Decrypt(reader["URL"].ToString(), encryptionKey);
                        }

                        if (reader["Username"] != DBNull.Value)
                        {
                            entry.Username = Decrypt(reader["Username"].ToString(), encryptionKey);
                        }

                        if (reader["Password"] != DBNull.Value)
                        {
                            entry.Password = Decrypt(reader["Password"].ToString(), encryptionKey);
                        }

                        if (reader["Category"] != DBNull.Value)
                        {
                            entry.Category = Decrypt(reader["Category"].ToString(), encryptionKey);
                        }

                        if (reader["Notes"] != DBNull.Value)
                        {
                            entry.Notes = Decrypt(reader["Notes"].ToString(), encryptionKey);
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

        public static string AddVaultEntry(Vault entry, int userID, string encryptionKey)
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
            cmd.Parameters.AddWithValue("@Iname", Encrypt(entry.ItemName, encryptionKey));
            cmd.Parameters.AddWithValue("@UserID", userID);
            cmd.Parameters.AddWithValue("@Uname", Encrypt(entry.Username, encryptionKey));
            cmd.Parameters.AddWithValue("@PW", Encrypt(entry.Password, encryptionKey));
            if (entry.URL != null) { cmd.Parameters.AddWithValue("@URL", Encrypt(entry.URL, encryptionKey)); }
            if (entry.Category != null) { cmd.Parameters.AddWithValue("@Cat", Encrypt(entry.Category, encryptionKey)); }
            if (entry.Notes != null) { cmd.Parameters.AddWithValue("@Notes", Encrypt(entry.Notes, encryptionKey)); }

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

        public static string UpdateVaultEntry(Vault entry, int userID, string encryptionKey)
        {
            if (entry.Username.Equals("") || userID <= (0))
            {
                return ErrorHandler.empty;
            }
            else if (entry.ID == 0)
            {
                return "Password ID not being passed to the DB, aborting!";
            }

            query = "UPDATE Vault " +
                "SET ItemName = @Iname, Username = @Uname"; 
            if (!entry.Password.Equals("")) { query += ", Password = @PW"; }
            if (entry.URL != null) { query += ", URL = @URL"; }
            if (entry.Category != null) { query += ", Category = @Cat"; }
            if (entry.Notes != null) { query += ", Notes = @Notes"; }
            query += " WHERE ID = @ID;";

            conn = new SqlConnection(connectionString);
            cmd = new SqlCommand(query, conn);
            string status = ErrorHandler.failed;
            int rows;
            cmd.Parameters.AddWithValue("@Iname", Encrypt(entry.ItemName, encryptionKey));
            cmd.Parameters.AddWithValue("@UserID", userID);
            cmd.Parameters.AddWithValue("@ID", entry.ID);
            cmd.Parameters.AddWithValue("@Uname", Encrypt(entry.Username, encryptionKey));
            if (!entry.Password.Equals("")) { cmd.Parameters.AddWithValue("@PW", Encrypt(entry.Password, encryptionKey)); }
            if (entry.URL != null) { cmd.Parameters.AddWithValue("@URL", Encrypt(entry.URL, encryptionKey)); }
            if (entry.Category != null) { cmd.Parameters.AddWithValue("@Cat", Encrypt(entry.Category, encryptionKey)); }
            if (entry.Notes != null) { cmd.Parameters.AddWithValue("@Notes", Encrypt(entry.Notes, encryptionKey)); }

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

        public static void initDB() {
            query = "CREATE TABLE 'Users' ( 'Username' NVARCHAR(MAX) NOT NULL ,'Password' NVARCHAR(MAX) NOT NULL ,  \r\n`Age` INT NOT NULL ," +
                "`Phone_No` VARCHAR(10) NOT NULL ,`Address` VARCHAR(100) NOT NULL ,\r\n PRIMARY KEY ('Username'));";
            // TODO:  Add 2nd query to create vault table as well, get rid of extra fields
            // To be continued once more progress has been made
        }

        public static string Encrypt(string plaintext, string password)
        {
            using (Aes encryptor = Aes.Create())
            {
                // Convert the plaintext string to a byte array
                byte[] plaintextBytes = Encoding.UTF8.GetBytes(plaintext);

                // generate a 128-bit salt using a secure PRNG
                byte[] salt = new byte[128 / 8];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(salt);
                }
                string saltBase64 = Convert.ToBase64String(salt),

                // Hashes the password for increased security
                hashedPass = CustomFunctions.HashToSHA512(password);

                // Derive a new password using the PBKDF2 algorithm and a random salt
                Rfc2898DeriveBytes passwordBytes = new Rfc2898DeriveBytes(hashedPass, salt);

                // Use the password to encrypt the plaintext
                encryptor.Key = passwordBytes.GetBytes(32);
                encryptor.IV = passwordBytes.GetBytes(16);
                encryptor.Mode = CipherMode.CBC;
                encryptor.Padding = PaddingMode.PKCS7;

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(plaintextBytes, 0, plaintextBytes.Length);
                    }
                    return saltBase64 + Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public static string Decrypt(string ciphertext, string password)
        {
            using (Aes encryptor = Aes.Create())
            {
                // Convert the encrypted string to a byte array
                byte[] encryptedBytes = Convert.FromBase64String(ciphertext.Remove(0, 24));

                // Hashes the password for increased security
                string hashedPass = CustomFunctions.HashToSHA512(password),

                // pull salt from the beginning of the string here
                saltBase64 = ciphertext.Remove(24);
                byte[] salt = Convert.FromBase64String(saltBase64);

                // Derive the password using the PBKDF2 algorithm
                Rfc2898DeriveBytes passwordBytes = new Rfc2898DeriveBytes(hashedPass, salt);

                // Use the password to decrypt the encrypted string
                encryptor.Key = passwordBytes.GetBytes(32);
                encryptor.IV = passwordBytes.GetBytes(16);
                encryptor.Mode = CipherMode.CBC;
                encryptor.Padding = PaddingMode.PKCS7;

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(encryptedBytes, 0, encryptedBytes.Length);
                    }
                    return Encoding.UTF8.GetString(ms.ToArray());
                }
            }
        }
    }
}