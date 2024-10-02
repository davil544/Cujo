using CujoPasswordManager.DataAccessLayer;
using System;

namespace CujoPasswordManager
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if logged in = false, redirect to Login.aspx and clear session variable here
            testURL.InnerText = CustomFunctions.TruncateString("https://www.sampleWebsite.lan/", 30);
        }
    }
}