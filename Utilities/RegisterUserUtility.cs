using Microsoft.Data.Sqlite;
using System.Net.Mail;

namespace sub4lazar.Utilities
{
    public static class RegisterUserUtility
    {
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
            string dbConnectionString = "Data Source = /database/lazar.db";
            int verifiedInt = user.verified ? 1 : 0;
            string insertQuery = $"INSERT INTO users VALUES ('{user.email}', {verifiedInt}, '{user.subscribeCode}', '{user.unsubscribeCode}')";
            using (var connection = new SqliteConnection(dbConnectionString))
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
    }
}