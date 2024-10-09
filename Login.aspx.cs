using CujoPasswordManager.DataAccessLayer;
using CujoPasswordManager.DataModels;
using System;

namespace CujoPasswordManager
{
    public partial class Login : System.Web.UI.Page
    {
        private Account account;
        protected void Page_Load(object sender, EventArgs e)
        {
            account = new Account();
        }

        protected void BtnLogin_Click(object sender, EventArgs e)
        {
            account.username = txtUsername.Text;    account.password = txtPassword.Text;
            // Maybe add rate limiting here?
            account = AccountManager.Login(account);
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "window.alert('" + account.status + "');", true);

            if (account.status == "success")
            {
                Session["account"] = account;
                Response.Redirect("/");
            }
        }

        protected void BtnRegister_Click(object sender, EventArgs e)
        {
            // Place Registration function Here
            // Check if DB is initialized, do so if not
            // Create Entries in DB for user credentials and vault data
            account.username = txtUsernameReg.Text; account.password = txtPasswordReg.Text;
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "window.alert('" + AccountManager.Register(account) + "');", true);
        }

        protected void BtnFormToggle_Click(object sender, EventArgs e)
        {
            if (pnlRegistration.Visible)
            {
                pnlRegistration.Visible = false;
                pnlLogin.Visible = true;
                btnToggleForm.Text = "Create a new account";
            }
            else
            {
                pnlLogin.Visible = false;
                pnlRegistration.Visible = true;
                btnToggleForm.Text = "Cancel";
            }
        }
    }
}