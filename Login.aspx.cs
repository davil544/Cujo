using CujoPasswordManager.DataAccessLayer;
using CujoPasswordManager.DataModels;
using System;

namespace CujoPasswordManager
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void BtnLogin_Click(object sender, EventArgs e)
        {
            Account account = new Account();
            account.username = "foo";  account.password = "bar";
            //AccountManager.Login(account);
            Response.Redirect("/");
            // Place Login Validation function here
        }
    }
}