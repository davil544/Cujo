using CujoPasswordManager.DataAccessLayer;
using CujoPasswordManager.DataModels;
using System;
using System.Security.Principal;

namespace CujoPasswordManager
{
    public partial class AddEntry : System.Web.UI.Page
    {
        private Account account;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["account"] == null)
            {
                Response.Redirect("/Login.aspx");
            }
            else
            {
                account = (Account)Session["account"];
            }
        }

        protected void BtnAdd_Click(object sender, EventArgs e)
        {
            Vault entry = new Vault();
            entry.ItemName = txtItemName.Text;
            entry.Username = txtUserName.Text;
            entry.Password = txtPassword.Text;
            entry.Category = txtCategory.Text;
            entry.URL = txtURL.Text;
            entry.Notes = txtNotes.Text;

            //send data to db here
            string status = AccountManager.AddVaultEntry(entry, account.ID);
            if (status == "success") {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "window.alert('Password entry added successfully!');", true);
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "window.alert('" + status + "');", true);
            }
            Response.Redirect("/");
        }
    }
}