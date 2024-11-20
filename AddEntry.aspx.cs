using CujoPasswordManager.DataAccessLayer;
using CujoPasswordManager.DataModels;
using System;

namespace CujoPasswordManager
{
    public partial class AddEntry : System.Web.UI.Page
    {
        private Account account; protected Vault entry; private int entryID;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["account"] == null)
            {
                Response.Redirect("/Login.aspx");
            }
            else
            {
                account = (Account)Session["account"];

                // If loading edit info here, load it from session variable then null it out for security
                
                if ((Vault)Session["vaultEntry"] != null)
                {
                    pnlAdd.Visible = false;
                    pnlUpdate.Visible = true;
                    btnAdd.Visible = false;
                    btnUpdate.Visible = true;

                    entry = (Vault)Session["vaultEntry"];
                    entryID = entry.ID;
                    if (!IsPostBack)
                    {
                        txtEditItemName.Text = entry.ItemName;
                        txtEditUserName.Text = entry.Username;
                        //txtEditPassword.Text = entry.Password;
                        txtEditCategory.Text = entry.Category;
                        txtEditURL.Text = entry.URL;
                        txtEditNotes.Text = entry.Notes;
                    }

                    /* if (IsPostBack)
                    {
                        Session["vaultEntry"] = null;
                    } */
                }
            }
        }

        protected void BtnAdd_Click(object sender, EventArgs e)
        {
            entry = new Vault();
            entry.ItemName = txtItemName.Text;
            entry.Username = txtUserName.Text;
            entry.Password = txtPassword.Text;
            entry.Category = txtCategory.Text;
            entry.URL = txtURL.Text;
            entry.Notes = txtNotes.Text;

            // send data to db here
            string status = AccountManager.AddVaultEntry(entry, account.ID);
            if (status == "success") {
                // Commented out as this doesn't work well with Response.Redirect()
                // Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "window.alert('Password entry added successfully!');", true);
                Response.Redirect("/");
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "window.alert('" + status + "');", true);
            }
        }

        protected void BtnUpdate_Click(object sender, EventArgs e)
        {
            Vault updatedEntry = new Vault();
            updatedEntry.ID = entryID;
            updatedEntry.UserID = account.ID;
            if (updatedEntry.ItemName != entry.ItemName) { updatedEntry.ItemName = txtEditItemName.Text; }
            updatedEntry.Username = txtEditUserName.Text;
            if (updatedEntry.Password != entry.Password) { updatedEntry.Password = txtEditPassword.Text; }
            updatedEntry.Category = txtEditCategory.Text;
            updatedEntry.URL = txtEditURL.Text;
            updatedEntry.Notes = txtEditNotes.Text;

            //TODO: Add null check for pw field, if so tell the function to skip updating the password, possibly remove null check from it after

            // send data to db here
            string status = AccountManager.UpdateVaultEntry(updatedEntry, account.ID);
            if (status == "success")
            {
                // Commented out as this doesn't work well with Response.Redirect()
                // Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "window.alert('Password entry added successfully!');", true);
                Response.Redirect("/");
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "window.alert('" + status + "');", true);
            }
        }

        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/");
        }
    }
}