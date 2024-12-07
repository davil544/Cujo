using CujoPasswordManager.DataAccessLayer;
using CujoPasswordManager.DataModels;
using System;

namespace CujoPasswordManager
{
    public partial class SearchResults : System.Web.UI.Page
    {
        private Account account; const string br = "<br />";
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

            string query = Request.QueryString["query"];
            if (query != null && query != "")
            {
                litSearchQuery.Text = query;    litResults.Text = br;
                Vault[] entries = AccountManager.GetVault(account.ID, account.password), searchResults = null;
                if (entries[0] != null)
                {
                    searchResults = Array.FindAll(entries, s => s.ItemName.Contains(query) || s.Username.Contains(query) || s.URL.Contains(query) || s.Category.Contains(query));
                }

                if (searchResults != null && searchResults.Length > 0)
                {
                    foreach (Vault entry in searchResults)
                    {
                        if (entry != null) { litResults.Text += "<a href=\"EntryDetails.aspx?ID=" + entry.ID + "\">" + entry.ItemName + "</a><br />"; }
                    }
                }
                else
                {
                    // This runs when there are no results found
                    litResults.Text += ErrorHandler.noResults;
                }
                entries = null; searchResults = null;
            }
            else
            {
                // This runs when the page is loaded directly, without a search query
                heading2.Attributes["hidden"] = "hidden";
                litResults.Text = br + ErrorHandler.noQuery;
            }
        }
    }
}