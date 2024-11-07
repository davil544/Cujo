using CujoPasswordManager.DataAccessLayer;
using CujoPasswordManager.DataModels;
using System;
using System.Security.Principal;

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
                //TODO:  Fix search query so it actually works
                litSearchQuery.Text = query;
                litResults.Text = br;
                Vault[] entries = AccountManager.GetVault(account.ID, query);
                if (entries[0] == null)
                {
                    //This runs when a query returns 0 movies
                    litResults.Text += ErrorHandler.noResults;
                }
                else foreach (Vault entry in entries)
                    {
                        if (entry != null) { litResults.Text += "<a href=\"EntryDetails.aspx?ID=" + entry.ID + "\">" + entry.ItemName + "</a><br />"; }
                    }
                //Session["query"] = null;
            }
            else
            {
                //This runs when the page is loaded directly, without a search query
                heading2.Attributes["hidden"] = "hidden";
                litResults.Text = br + ErrorHandler.noQuery;
                //litResults.Text = ErrorHandler.noEntries;
            }
        }
    }
}