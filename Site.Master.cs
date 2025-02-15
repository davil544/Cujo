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
            if (currentPage == "default.aspx" || currentPage == "searchresults.aspx"){
                Response.Redirect("~/SearchResults.aspx?query=" + txtSearch.Text);
            }
        }
    }
}