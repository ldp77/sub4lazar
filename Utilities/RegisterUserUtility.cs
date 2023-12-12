using Microsoft.Data.Sqlite;
using System.Net.Mail;

namespace sub4lazar.Utilities
{
    public static class RegisterUserUtility
    {
        public static string dbConnectionString()
        {
            return "Data Source = /database/lazar.db";
        }
        public static bool validateUser(sub4lazar.Models.User user)
        {
            bool isUserValid = true;

            if (user.email == "")
            {
                isUserValid = false;
            }

            return isUserValid;
        }

        public static void uploadUserToDB(sub4lazar.Models.User user)
        {
            int verifiedInt = user.verified ? 1 : 0;
            string insertQuery = $"INSERT INTO users VALUES ('{user.email}', {verifiedInt}, '{user.subscribeCode}', '{user.unsubscribeCode}')";
            using (var connection = new SqliteConnection(dbConnectionString()))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = insertQuery;
                    System.Console.WriteLine("Executing Query:");
                    System.Console.WriteLine(command.CommandText);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void sendVerificationEmail(sub4lazar.Models.User user)
        {
            // Retrieve from env
            string smtpKey = System.Environment.GetEnvironmentVariable("SMTP_KEY");
            string fromEmail = System.Environment.GetEnvironmentVariable("SMTP_EMAIL");
            string validationLink = "thedomain.com/Validate";
            var smtpClient = new SmtpClient("smtp-relay.brevo.com")
            {
                Port = 587,
                Credentials = new System.Net.NetworkCredential(fromEmail, smtpKey),
                EnableSsl = true,
            };

            string emailSubject = "Verify Your Email";
            
            string emailBody = $@"Thank you for subscribing to Lazar Notifications.
            
Please verify your email at {validationLink}
            
Your verification code is {user.subscribeCode}";

            System.Console.WriteLine("EmailBody:");
            System.Console.WriteLine(emailBody);

            smtpClient.Send(fromEmail, user.email, emailSubject, emailBody);
        }

        public static bool verifyUserIfValid(string email, string subscribeCode)
        {
            // Query the DB for information matching user input
            string selectQuery = $"SELECT * FROM users WHERE (email='{email}' AND subscribeCode='{subscribeCode}');";
            using (var connection = new SqliteConnection(dbConnectionString()))
            {
                connection.Open();
                using (var selectCommand = new SqliteCommand(selectQuery, connection))
                {
                    using (var reader = selectCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string updateQuery = $"UPDATE users SET verified=1 WHERE email='{email}';";
                            using (var updateCommand = new SqliteCommand(updateQuery, connection))
                            {
                                int rowsAffected = updateCommand.ExecuteNonQuery();
                                if (rowsAffected > 0)
                                {
                                    Console.WriteLine("Email verified successfully");
                                }
                                else
                                {
                                    Console.WriteLine("A problem occurred during verification");
                                }
                            }
                            return true;
                        }
                        else
                        {
                            Console.WriteLine("No Record Found");
                            return false;
                        }
                    }
                }
            }
        }
    }
}