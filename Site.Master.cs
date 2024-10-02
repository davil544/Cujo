using System;

namespace CujoPasswordManager
{
    public partial class Site : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String currentPage = System.IO.Path.GetFileName(Request.PhysicalPath);
            if (currentPage.ToLower() == "default.aspx")
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
            String fileName = "Current Page: " + System.IO.Path.GetFileName(Request.PhysicalPath) + "\\n";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "window.alert('" + fileName + "This will search the vault for passwords one day!');", true);
        }
    }
}