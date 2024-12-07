using CujoPasswordManager.DataAccessLayer;
using CujoPasswordManager.DataModels;
using System;
using System.Web.UI.WebControls;

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
                account = (Account)Session["account"]; string previousPage = null;
                try
                {
                    previousPage = Request.UrlReferrer.LocalPath;
                }
                catch (NullReferenceException) { } //In production, this will stop the server from crashing if this page is directly navigated to with vaultEntry data

                if ((Vault)Session["vaultEntry"] != null && (previousPage == "/AddEntry.aspx" || previousPage == "/EntryDetails.aspx"))
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
                        txtEditPassword.Attributes.Add("value", entry.Password);
                        txtEditCategory.Text = entry.Category;
                        txtEditURL.Text = entry.URL;
                        txtEditNotes.Text = entry.Notes;
                    }
                }
                else
                {
                    Session["vaultEntry"] = null;
                }
                if (!IsPostBack)
                {
                    ListItem li1 = new ListItem("&nbsp;Uppercase Letters", "A", true),
                        li2 = new ListItem("&nbsp;Lowercase Letters", "z", true),
                        li3 = new ListItem("&nbsp;Numbers", "09", true),
                        li4 = new ListItem("&nbsp;Special Characters", "@#", true);
                    li1.Selected = true;
                    li2.Selected = true;
                    li3.Selected = true;
                    li4.Selected = true;
                    cblSupportedChars.Items.Add(li1);
                    cblSupportedChars.Items.Add(li2);
                    cblSupportedChars.Items.Add(li3);
                    cblSupportedChars.Items.Add(li4);
                }
            }
        }

        protected void BtnAdd_Click(object sender, EventArgs e)
        {
            entry = new Vault
            {
                ItemName = txtItemName.Text,
                Username = txtUserName.Text,
                Password = txtPassword.Text,
                Category = txtCategory.Text,
                URL = txtURL.Text,
                Notes = txtNotes.Text
            };

            // send data to db here
            string status = AccountManager.AddVaultEntry(entry, account.ID, account.password);
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
            Vault updatedEntry = new Vault
            {
                ID = entryID,
                UserID = account.ID,
                Username = txtEditUserName.Text,
                Category = txtEditCategory.Text,
                URL = txtEditURL.Text,
                Notes = txtEditNotes.Text
            };
            if (updatedEntry.ItemName != entry.ItemName) { updatedEntry.ItemName = txtEditItemName.Text; }
            if (updatedEntry.Password != entry.Password) { updatedEntry.Password = txtEditPassword.Text; }

            //TODO: Add null check for pw field, if so tell the function to skip updating the password, possibly remove null check from it after

            // send data to db here
            string status = AccountManager.UpdateVaultEntry(updatedEntry, account.ID, account.password);
            if (status == "success")
            {
                // Commented out as this doesn't work well with Response.Redirect()
                // Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "window.alert('Password entry added successfully!');", true);
                Session["vaultEntry"] = null;
                Response.Redirect("/");
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "window.alert('" + status + "');", true);
            }
        }

        protected void BtnGenPass_Click(object sender, EventArgs e)
        {
            string charset = null, password = null;
            foreach (ListItem charType in cblSupportedChars.Items)
            {
                if (charType.Selected)
                {
                    charset += charType.Value;
                }
            }
            if (charset == null || txtPassLength.Text == "") { return; }
            password = CustomFunctions.GeneratePassword(int.Parse(txtPassLength.Text), charset);

            if (pnlAdd.Visible)
            {
                txtPassword.Attributes.Add("value", password);
                if (txtPassword.TextMode != TextBoxMode.Password) {
                    txtPassword.Text = password;
                }
            }
            else
            {
                txtEditPassword.Attributes.Add("value", password);
                if (txtEditPassword.TextMode != TextBoxMode.Password)
                {
                    txtEditPassword.Text = password;
                }
            }
        }

        protected void BtnShowPass_CLick(object sender, EventArgs e)
        {
            const string hide = "Hide Password", show = "Show Password";
            if (pnlAdd.Visible)
            {
                if (txtPassword.TextMode == TextBoxMode.Password)
                {
                    txtPassword.TextMode = TextBoxMode.SingleLine;
                    btnShowPass.Text = hide;
                }
                else
                {
                    txtPassword.Attributes.Add("value", txtEditPassword.Text);
                    txtPassword.TextMode = TextBoxMode.Password;
                    btnShowPass.Text = show;
                }
            }
            else
            {
                if (txtEditPassword.TextMode == TextBoxMode.Password)
                {
                    txtEditPassword.TextMode = TextBoxMode.SingleLine;
                    btnShowPass.Text = hide;
                }
                else
                {
                    txtEditPassword.Attributes.Add("value", txtEditPassword.Text);
                    txtEditPassword.TextMode = TextBoxMode.Password;
                    btnShowPass.Text = show;
                }
            }
            
        }

        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            Session["vaultEntry"] = null;
            Response.Redirect("~/");
        }
    }
}