using Microsoft.Data.Sqlite;

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
            string insertQuery = $"INSERT INTO users VALUES ('{user.email}', {user.verified}, '{user.subscribeCode}', '{user.unsubscribeCode}')";
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
    }
}