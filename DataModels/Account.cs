namespace CujoPasswordManager.DataModels
{
    public class Account
    {
        public string username { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public string status { get; set; }
        public Vault vault { get; set; }

        public Account() {
            username = string.Empty;
            password = string.Empty;
            name = string.Empty;
            status = string.Empty;
            vault = new Vault();
        }
    }
}