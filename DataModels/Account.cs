namespace CujoPasswordManager.DataModels
{
    public class Account
    {
        public int ID { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public string status { get; set; }

        public Account() {
            //ID = 0;
            username = string.Empty;
            password = string.Empty;
            name = string.Empty;
            status = string.Empty;
        }
    }
}