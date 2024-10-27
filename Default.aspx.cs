using CujoPasswordManager.DataAccessLayer;
using CujoPasswordManager.DataModels;
using System;
using System.Web.UI.WebControls;

namespace CujoPasswordManager
{
    public partial class Default : System.Web.UI.Page
    {
        private Account account;

        protected void Page_Load(object sender, EventArgs e)
        {
            //if logged in = false, redirect to Login.aspx and clear session variable here
            if (Session["account"] == null)
            {
                Response.Redirect("/Login.aspx");
            }
            else
            {
                account = (Account)Session["account"];
                Vault[] passwords = AccountManager.GetVault(account.ID);
                Literal litVaultEntries = new Literal();

                foreach (Vault entry in passwords)
                {
                    if (entry != null)
                    {
                        litVaultEntries.Text +=  "<tr>\r\n" +
                            "                         <td><a href=\"EntryDetails.aspx?id=" + entry.ID + "\">" + entry.ItemName + "</a></td>\r\n" +
                            "                         <td><a href=\"EntryDetails.aspx?id=" + entry.ID + "\">" + entry.Username + "</a></td>\r\n" +
                            "                         <td><a href=\"EntryDetails.aspx?id=" + entry.ID + "\">" + CustomFunctions.TruncateString(entry.URL, 30) + "</a></td>\r\n" +
                            "                     </tr>";
                    }
                    else
                    {
                        litVaultEntries.Text += "<tr>\r\n" +
                            "                       <td>No Items Found</td>\r\n" +
                            "                       <td></td>\r\n" +
                            "                       <td></td>\r\n" +
                            "                     </tr>";
                    }
                }
                pnlVaultContents.Controls.Add(litVaultEntries);
            }
        }

        protected void BtnAddEntry_Click(object sender, EventArgs e)
        {
            Response.Redirect("AddEntry.aspx");
        }
    }
}