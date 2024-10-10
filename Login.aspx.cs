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
            if (Session["account"] != null)
            {
                Session["account"] = null;
            }
            account = new Account();
        }

        protected void BtnLogin_Click(object sender, EventArgs e)
        {
            account.username = txtUsername.Text;    account.password = txtPassword.Text;
            // Maybe add rate limiting here?
            account = AccountManager.Login(account);

            if (account.status == "valid")
            {
                Session["account"] = account;
                Response.Redirect("/");
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "window.alert('" + account.status + "');", true);
            }
        }

        protected void BtnRegister_Click(object sender, EventArgs e)
        {
            // Check if DB is initialized one day, do so if not
            // Create entries in DB for vault data
            account.username = txtUsernameReg.Text; account.password = txtPasswordReg.Text;
            String status = AccountManager.Register(account);
            

            if (status == "success")
            {
                //Switches back to the login form
                BtnFormToggle_Click(sender, e);
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "window.alert('Your account has been created successfully!');", true);
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "window.alert('" + status + "');", true);
            }
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