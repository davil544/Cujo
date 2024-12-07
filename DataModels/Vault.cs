namespace CujoPasswordManager.DataModels
{
    public class Vault
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string ItemName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string URL { get; set; }
        public string Category { get; set; }
        public string Notes { get; set; }

    public Vault()
        {
            ItemName = string.Empty;
            Username = string.Empty;
            Password = string.Empty;
            URL = string.Empty;
            Category = string.Empty;
            Notes = string.Empty;
        } 
    }
}