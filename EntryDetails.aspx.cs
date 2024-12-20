﻿using CujoPasswordManager.DataAccessLayer;
using CujoPasswordManager.DataModels;
using System;

namespace CujoPasswordManager
{
    public partial class EntryDetails : System.Web.UI.Page
    {
        private Account account;    private Vault entry;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["account"] == null)
            {
                Session["vaultEntry"] = null;
                Response.Redirect("/Login.aspx");
            }
            else if (Request.QueryString["ID"] == "" || Request.QueryString["ID"] == null)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "window.alert('No entry selected, redirecting to the home page...');", true);
                Response.Redirect("/");
            }
            else
            {
                account = (Account)Session["account"];
                string entryID = Request.QueryString["ID"];
                entry = AccountManager.GetVault(account.ID, int.Parse(entryID), account.password);

                lblName.Text = entry.ItemName;
                lblURL.Text = entry.URL;
                lblUser.Text = entry.Username;
                lblCat.Text = entry.Category;
                lblNotes.Text = entry.Notes;
            }
        }

        protected void LnkPassword_Click(object sender, EventArgs e)
        {
            //toggle show password here
            string placeholder = "<i>Show Password</i>";
            if (lnkPassword.Text.Equals(placeholder))
            {
                lnkPassword.Text = entry.Password;
            }
            else
            {
                lnkPassword.Text = placeholder;
            }
        }

        protected void BtnEdit_Click(object sender, EventArgs e)
        {
            //pass entry data to add page via Add Entry page, possibly make a second form for edits
            Session["vaultEntry"] = entry;
            Response.Redirect("/AddEntry.aspx");
        }
    }
}