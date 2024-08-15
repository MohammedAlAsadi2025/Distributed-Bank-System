using System.Data.SQLite;
using Web_API_Data_Server.Models.Account;
using Web_API_Data_Server.Models.UserProfile;
using Web_API_Data_Server.Models.Transaction;


namespace Web_API_Data_Server.Data
{
    public class DBManager
    {
        private static string connectionString = "Data Source=mydatabase.db;Version=3;";

        public static bool CreateTable()
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        // Create AccountTable if it doesn't exist
                        command.CommandText = @"
            CREATE TABLE IF NOT EXISTS AccountTable (
                AccountNo INTEGER,
                FirstName TEXT,
                LastName TEXT,
                PIN INTEGER,
                Balance REAL
            )";
                        command.ExecuteNonQuery();

                        // Create UserProfileTable if it doesn't exist
                        command.CommandText = @"
            CREATE TABLE IF NOT EXISTS UserProfileTable (
                UserName TEXT UNIQUE,
                Email TEXT UNIQUE,
                Password TEXT,                
                FirstName TEXT,
                LastName TEXT,
                Phone TEXT,
                AccountNo INTEGER
            )";

                        command.ExecuteNonQuery();

                        // Create TransactionTable if it doesn't exist
                        command.CommandText = @"
            CREATE TABLE IF NOT EXISTS TransactionTable (
                AccountNo INTEGER,
                Amount REAL,
                TransactionType TEXT,
                TransactionDate DATETIME
            )";
                        command.ExecuteNonQuery();

                        connection.Close();
                    }
                }
                Console.WriteLine("Tables created or verified successfully.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return false; // Create tables failed
        }


        public static bool Deposit(uint accountNo, decimal amount)
        {
            // Check if account exists and retrieve the current balance
            Account account = GetByAccountNo(accountNo);
            if (account == null) return false;

            // Check if deposit amount is positive
            if (amount <= 0)
            {
                Console.WriteLine("Invalid deposit amount. Amount should be positive.");
                return false;
            }

            // Update balance
            account.Balance += amount;

            // Check if update was successful
            if (!Update(account))
            {
                Console.WriteLine("Error updating account balance.");
                return false;
            }

            // Log transaction
            Transaction transaction = new Transaction
            {
                AccountNo = accountNo,
                Amount = amount,
                TransactionType = "Deposit",
                TransactionDate = DateTime.Now
            };

            if (!InsertTransaction(transaction))
            {
                Console.WriteLine("Error logging transaction.");
                // Ideally, you should rollback the balance update here
                return false;
            }

            return true;
        }


        public static bool Withdraw(uint accountNo, decimal amount)
        {
            // Check if account exists and retrieve the current balance
            Account account = GetByAccountNo(accountNo);
            if (account == null) return false;

            // Check if the account has enough balance for the withdrawal
            if (account.Balance < amount)
            {
                Console.WriteLine("Insufficient funds.");
                return false;
            }

            // Update balance
            account.Balance -= amount;

            // Check if update was successful
            if (!Update(account))
            {
                Console.WriteLine("Error updating account balance.");
                return false;
            }

            // Log transaction
            Transaction transaction = new Transaction
            {
                AccountNo = accountNo,
                Amount = amount,
                TransactionType = "Withdraw",
                TransactionDate = DateTime.Now
            };

            if (!InsertTransaction(transaction))
            {
                Console.WriteLine("Error logging transaction.");
                // Ideally, you should rollback the balance update here
                return false;
            }

            return true;
        }

        public static List<Transaction> GetAllTransactions()
        {
            List<Transaction> transactionList = new List<Transaction>();
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT * FROM TransactionTable";
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Transaction transaction = new Transaction
                                {
                                    AccountNo = Convert.ToUInt32(reader["AccountNo"]),
                                    Amount = Convert.ToDecimal(reader["Amount"]),
                                    TransactionType = reader["TransactionType"].ToString(),
                                    TransactionDate = Convert.ToDateTime(reader["TransactionDate"])
                                };
                                transactionList.Add(transaction);
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return transactionList;
        }

        public static List<Transaction> GetTransactionsByAccountNo(uint accountNo)
        {
            List<Transaction> transactionList = new List<Transaction>();
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT * FROM TransactionTable WHERE AccountNo = @AccountNo";
                        command.Parameters.AddWithValue("@AccountNo", accountNo);
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Transaction transaction = new Transaction
                                {
                                    AccountNo = Convert.ToUInt32(reader["AccountNo"]),
                                    Amount = Convert.ToDecimal(reader["Amount"]),
                                    TransactionType = reader["TransactionType"].ToString(),
                                    TransactionDate = Convert.ToDateTime(reader["TransactionDate"])
                                };
                                transactionList.Add(transaction);
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return transactionList;
        }


        public static bool InsertTransaction(Transaction transaction)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"
                INSERT INTO TransactionTable (AccountNo, Amount, TransactionType, TransactionDate)
                VALUES (@AccountNo, @Amount, @TransactionType, @TransactionDate)";

                        command.Parameters.AddWithValue("@AccountNo", transaction.AccountNo);
                        command.Parameters.AddWithValue("@Amount", transaction.Amount);
                        command.Parameters.AddWithValue("@TransactionType", transaction.TransactionType);
                        command.Parameters.AddWithValue("@TransactionDate", transaction.TransactionDate);

                        int rowsInserted = command.ExecuteNonQuery();
                        connection.Close();

                        if (rowsInserted > 0)
                        {
                            return true;  // Insertion was successful
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return false;  // Insertion failed
        }

        public static bool TransferMoney(uint senderAccountNo, uint receiverAccountNo, decimal amount, string description)
        {
            // Check if both accounts exist
            Account senderAccount = GetByAccountNo(senderAccountNo);
            Account receiverAccount = GetByAccountNo(receiverAccountNo);
            if (senderAccount == null || receiverAccount == null) return false;

            // Check if the sender has enough balance for the transfer
            if (senderAccount.Balance < amount)
            {
                Console.WriteLine("Insufficient funds.");
                return false;
            }

            // Debit the sender's account
            senderAccount.Balance -= amount;
            if (!Update(senderAccount))
            {
                Console.WriteLine("Error updating sender account balance.");
                return false;
            }

            // Log the debit transaction
            Transaction senderTransaction = new Transaction
            {
                AccountNo = senderAccountNo,
                Amount = amount,
                TransactionType = $"Debit: {description}",
                TransactionDate = DateTime.Now
            };
            if (!InsertTransaction(senderTransaction))
            {
                Console.WriteLine("Error logging sender transaction.");
                return false;
            }

            // Credit the receiver's account
            receiverAccount.Balance += amount;
            if (!Update(receiverAccount))
            {
                Console.WriteLine("Error updating receiver account balance.");
                return false;
            }

            // Log the credit transaction
            Transaction receiverTransaction = new Transaction
            {
                AccountNo = receiverAccountNo,
                Amount = amount,
                TransactionType = $"Credit: {description}",
                TransactionDate = DateTime.Now
            };
            if (!InsertTransaction(receiverTransaction))
            {
                Console.WriteLine("Error logging receiver transaction.");
                return false;
            }

            return true;
        }






        public static bool Insert(Account account)
        {
            try
            {
                // Create a new SQLite connection
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Create a new SQLite command to execute SQL
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        // SQL command to insert data into the "AccountTable" table
                        command.CommandText = @"
                        INSERT INTO AccountTable (AccountNo, FirstName, LastName, PIN, Balance)
                        VALUES (@AccountNo, @FirstName, @LastName, @PIN, @Balance)";

                        // Define parameters for the query
                        command.Parameters.AddWithValue("@AccountNo", account.AccountNo);
                        command.Parameters.AddWithValue("@FirstName", account.FirstName);
                        command.Parameters.AddWithValue("@LastName", account.LastName);
                        command.Parameters.AddWithValue("@PIN", account.PIN);
                        command.Parameters.AddWithValue("@Balance", account.Balance);

                        // Execute the SQL command to insert data
                        int rowsInserted = command.ExecuteNonQuery();

                        // Check if any rows were inserted
                        connection.Close();
                        if (rowsInserted > 0)
                        {
                            return true; // Insertion was successful
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return false; // Insertion failed
        }

        public static bool InsertUserProfile(UserProfile user)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"
                INSERT INTO UserProfileTable (UserName, Email, Password, FirstName, LastName, Phone, AccountNo)
                VALUES (@UserName, @Email, @Password, @FirstName, @LastName, @Phone, @AccountNo)";

                        command.Parameters.AddWithValue("@UserName", user.UserName);
                        command.Parameters.AddWithValue("@Email", user.Email);
                        command.Parameters.AddWithValue("@Password", user.Password);
                        command.Parameters.AddWithValue("@FirstName", user.FirstName);
                        command.Parameters.AddWithValue("@LastName", user.LastName);
                        command.Parameters.AddWithValue("@Phone", user.Phone);
                        command.Parameters.AddWithValue("@AccountNo", user.AccountNo);

                        int rowsInserted = command.ExecuteNonQuery();
                        connection.Close();
                        if (rowsInserted > 0)
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return false;
        }


        public static bool Delete(uint accountNo)
        {
            try
            {
                // Create a new SQLite connection
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Create a new SQLite command to execute SQL
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        // Build the SQL command to delete data by ID
                        command.CommandText = $"DELETE FROM AccountTable WHERE AccountNo = @AccountNo";
                        command.Parameters.AddWithValue("@AccountNo", accountNo);

                        // Execute the SQL command to delete data
                        int rowsDeleted = command.ExecuteNonQuery();

                        // Check if any rows were deleted
                        connection.Close();
                        if (rowsDeleted > 0)
                        {
                            return true; // Deletion was successful
                        }
                    }
                    connection.Close();
                }

                return false; // No rows were deleted
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false; // Deletion failed
            }
        }

        public static bool DeleteUserProfile(string userName)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "DELETE FROM UserProfileTable WHERE UserName = @UserName";
                        command.Parameters.AddWithValue("@UserName", userName);

                        int rowsDeleted = command.ExecuteNonQuery();
                        connection.Close();
                        if (rowsDeleted > 0)
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return false;
        }


        public static bool Update(Account account)
        {
            try
            {
                // Create a new SQLite connection
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Create a new SQLite command to execute SQL
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        // Build the SQL command to update data by ID
                        command.CommandText = $"UPDATE AccountTable SET FirstName = @FirstName, LastName = @LastName, PIN = @PIN, Balance = @Balance WHERE AccountNo = @AccountNo";
                        command.Parameters.AddWithValue("@AccountNo", account.AccountNo);
                        command.Parameters.AddWithValue("@FirstName", account.FirstName);
                        command.Parameters.AddWithValue("@LastName", account.LastName);
                        command.Parameters.AddWithValue("@PIN", account.PIN);
                        command.Parameters.AddWithValue("@Balance", account.Balance);

                        // Execute the SQL command to update data
                        int rowsUpdated = command.ExecuteNonQuery();
                        connection.Close();
                        // Check if any rows were updated
                        if (rowsUpdated > 0)
                        {
                            return true; // Update was successful
                        }
                    }
                    connection.Close();
                }

                return false; // No rows were updated
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false; // Update failed
            }
        }

        public static bool UpdateUserProfile(UserProfile user)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"
                UPDATE UserProfileTable 
                SET Email = @Email, Password = @Password, FirstName = @FirstName, LastName = @LastName, Phone = @Phone 
                WHERE UserName = @UserName";

                        command.Parameters.AddWithValue("@UserName", user.UserName);
                        command.Parameters.AddWithValue("@Email", user.Email);
                        command.Parameters.AddWithValue("@Password", user.Password);
                        command.Parameters.AddWithValue("@FirstName", user.FirstName);
                        command.Parameters.AddWithValue("@LastName", user.LastName);
                        command.Parameters.AddWithValue("@Phone", user.Phone);

                        int rowsUpdated = command.ExecuteNonQuery();
                        connection.Close();
                        if (rowsUpdated > 0)
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return false;
        }


        public static List<Account> GetAll()
        {
            List<Account> accountList = new List<Account>();
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT * FROM AccountTable";
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Account account = new Account
                                {
                                    AccountNo = Convert.ToUInt32(reader["AccountNo"]),
                                    FirstName = reader["FirstName"].ToString(),
                                    LastName = reader["LastName"].ToString(),
                                    PIN = Convert.ToUInt32(reader["PIN"]),
                                    Balance = Convert.ToInt32(reader["Balance"])
                                };
                                accountList.Add(account);
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return accountList;
        }

        public static List<UserProfile> GetAllUserProfiles()
        {
            List<UserProfile> userList = new List<UserProfile>();
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT * FROM UserProfileTable";
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                UserProfile user = new UserProfile
                                {
                                    UserName = reader["UserName"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    Password = reader["Password"].ToString(),
                                    FirstName = reader["FirstName"].ToString(),
                                    LastName = reader["LastName"].ToString(),
                                    Phone = reader["Phone"].ToString()
                                };
                                userList.Add(user);
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return userList;
        }


        public static Account GetByAccountNo(uint accountNo)
        {
            Account account = null;
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT * FROM AccountTable WHERE AccountNo = @AccountNo";
                        command.Parameters.AddWithValue("@AccountNo", accountNo);
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                account = new Account
                                {
                                    AccountNo = Convert.ToUInt32(reader["AccountNo"]),
                                    FirstName = reader["FirstName"].ToString(),
                                    LastName = reader["LastName"].ToString(),
                                    PIN = Convert.ToUInt32(reader["PIN"]),
                                    Balance = Convert.ToInt32(reader["Balance"])
                                };
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return account;
        }

        public static UserProfile GetByUserName(string userName)
        {
            UserProfile user = null;
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT * FROM UserProfileTable WHERE UserName = @UserName";
                        command.Parameters.AddWithValue("@UserName", userName);
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                user = new UserProfile
                                {
                                    UserName = reader["UserName"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    Password = reader["Password"].ToString(),
                                    FirstName = reader["FirstName"].ToString(),
                                    LastName = reader["LastName"].ToString(),
                                    Phone = reader["Phone"].ToString(),
                                    AccountNo = Convert.ToUInt32(reader["AccountNo"]),
                                };
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return user;
        }

        public static UserProfile GetByEmail(string email)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT * FROM UserProfileTable WHERE Email = @Email";
                        command.Parameters.AddWithValue("@Email", email);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                UserProfile userProfile = new UserProfile
                                {
                                    UserName = reader["UserName"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    Password = reader["Password"].ToString(),
                                    FirstName = reader["FirstName"].ToString(),
                                    LastName = reader["LastName"].ToString(),
                                    Phone = reader["Phone"].ToString(),
                                    AccountNo = Convert.ToUInt32(reader["AccountNo"]),
                                };
                                return userProfile;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return null;
        }


        public static int CountAll()
        {
            int count = 0;
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT COUNT(*) FROM AccountTable";
                    count = Convert.ToInt32(command.ExecuteScalar());
                }
                connection.Close();
            }
            return count;
        }

        private static Random random = new Random();

        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private static int RandomNumber(int min, int max)
        {
            return random.Next(min, max);
        }

        public static void SeedAccounts(int numberOfAccounts)
        {
            for (int i = 0; i < numberOfAccounts; i++)
            {
                Account account = new Account
                {
                    AccountNo = (uint)(i + 1),
                    FirstName = RandomString(5),
                    LastName = RandomString(7),
                    PIN = (uint)RandomNumber(1000, 9999),
                    Balance = RandomNumber(0, 5000)
                };
                DBManager.Insert(account);
            }
        }

        public static void SeedUserProfiles(int numberOfUsers)
        {
            for (int i = 0; i < numberOfUsers; i++)
            {
                UserProfile user = new UserProfile
                {
                    UserName = RandomString(8),
                    Email = $"{RandomString(5)}@email.com",
                    Password = RandomString(10),
                    FirstName = RandomString(5),
                    LastName = RandomString(7),
                    Phone = $"{RandomNumber(100, 999)}-{RandomNumber(100, 999)}-{RandomNumber(1000, 9999)}"
                };
                DBManager.InsertUserProfile(user);
            }
        }

        public static void SeedTransactions(int numberOfTransactions)
        {
            for (int i = 0; i < numberOfTransactions; i++)
            {
                Transaction transaction = new Transaction
                {
                    AccountNo = (uint)RandomNumber(1, 10),
                    Amount = RandomNumber(10, 500),
                    TransactionType = RandomNumber(0, 2) == 0 ? "Deposit" : "Withdraw",
                    TransactionDate = DateTime.Now
                };
                DBManager.InsertTransaction(transaction);
            }
        }
        public static void SeedData()
        {
            SeedAccounts(10);          
            SeedUserProfiles(10);
            SeedTransactions(50);
        }

        public static void ClearTable(string tableName)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = $"DELETE FROM {tableName}";
                        int rowsDeleted = command.ExecuteNonQuery();
                        Console.WriteLine($"Deleted {rowsDeleted} rows from {tableName}.");
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error clearing table {tableName}: " + ex.Message);
            }
        }




        public static void DBInitialize()
        {
            if (CreateTable())
            {

                ClearTable("AccountTable");
                ClearTable("UserProfileTable");
                ClearTable("TransactionTable");

                Account account1 = new Account();
                account1.FirstName = "Robert";
                account1.LastName = "James";
                account1.PIN = 1011;
                account1.AccountNo = 0001;
                account1.Balance = 0;
                Insert(account1);

                Account account2 = new Account();
                account2.FirstName = "Mia";
                account2.LastName = "Williams";
                account2.PIN = 2033;
                account2.AccountNo = 0002;
                account2.Balance = 1000;
                Insert(account2);

                Account account3 = new Account();
                account3.FirstName = "Adam";
                account3.LastName = "Smith";
                account3.PIN = 4218;
                account3.AccountNo = 0003;
                account3.Balance = 2000;
                Insert(account3);

                UserProfile user1 = new UserProfile();
                user1.UserName = "RobertJames";
                user1.Email = "robert.james@email.com";
                user1.Password = "Robert1011";
                user1.FirstName = "Robert";
                user1.LastName = "James";
                user1.Phone = "123-456-7890";
                user1.AccountNo = 0001;
                InsertUserProfile(user1);

                UserProfile user2 = new UserProfile();
                user2.UserName = "MiaWilliams";
                user2.Email = "mia.williams@email.com";
                user2.Password = "Mia2033";
                user2.FirstName = "Mia";
                user2.LastName = "Williams";
                user2.Phone = "234-567-8901";
                user2.AccountNo = 0002;
                InsertUserProfile(user2);

                UserProfile user3 = new UserProfile();
                user3.UserName = "AdamSmith";
                user3.Email = "adam.smith@email.com";
                user3.Password = "Adam4218";
                user3.FirstName = "Adam";
                user3.LastName = "Smith";
                user3.Phone = "345-678-9012";
                user3.AccountNo = 0003;
                InsertUserProfile(user3);

                UserProfile user4 = new UserProfile();
                user4.UserName = "admin";
                user4.Email = "admin@email.com";
                user4.Password = "admin111";
                user4.FirstName = "admin";
                user4.LastName = "admin";
                user4.Phone = "123-456-7890";
                InsertUserProfile(user4);

                SeedData();


            }
        }
    }
}