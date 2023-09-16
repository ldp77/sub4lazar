namespace sub4lazar.Models
{
    public class User
    {
        public string email { get; set; }
        public bool verified { get; set; }
        public string subscribeCode { get; set; }
        public string unsubscribeCode { get; set; }

        public User()
        {
            
        }

        public User(string email)
        {
            // User email comes from the form
            this.email = email;
            
            // At creation, no user is verified
            this.verified = false;
            
            // Create a 6 digit code sent to user for email verification
            Random rand = new Random();
            string code = "";
            for (int i = 0; i < 6; i++) {
                code += rand.Next(10).ToString();
            }
            this.subscribeCode = code;

            // Placeholder for code used to verify unsubscribe
            this.unsubscribeCode = "";
        }
    }
}
