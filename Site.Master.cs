using System;
using System.Web.UI;

namespace CujoPasswordManager
{
    public partial class Site : System.Web.UI.MasterPage
    {
        private String currentPage;

        protected void Page_Load(object sender, EventArgs e)
        {
            currentPage = System.IO.Path.GetFileName(Request.PhysicalPath.ToLower());
            if (currentPage == "default.aspx" || currentPage == "searchresults.aspx")
            {
                txtSearch.Visible = true;
                btnSearch.Visible = true;
            }
            else
            {
                txtSearch.Visible = false;
                btnSearch.Visible = false;
            }
        }

        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            //Place Search Function Here
            //String fileName = "Current Page: " + System.IO.Path.GetFileName(Request.PhysicalPath) + "\\n";
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "window.alert('" + fileName + "This will search the vault for passwords one day!');", true);

            if (currentPage == "default.aspx" || currentPage == "searchresults.aspx"){
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "window.alert('This is the default page');", true);
                //run search function and change panel function here
                //Vault[] entries = AccountManager.GetVault(0, (string)Session["query"]);
                //Literal litSearchResults = new Literal();
                //litSearchResults.Text = "It works!";
                //Default.DisplaySearchResults(litSearchResults);
                //txtSearch.Text = (String)Session["query"];
                Response.Redirect("~/SearchResults.aspx?query=" + txtSearch.Text);
            }

            //Session["query"] = txtSearch.Text;
            //Response.Redirect("/SearchResults.aspx");
        }
    }
}