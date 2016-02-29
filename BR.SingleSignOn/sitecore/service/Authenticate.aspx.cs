namespace BR.SingleSignOn
{
    using System;
    using System.Configuration;
    using System.Web;

    public partial class Authenticate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            /*If an authenticated user is set and gets to this page it means they don't have permission to view the page
            e.g. student trying to access staff page. In that case take them to the access-denied page.*/
            if (Sitecore.Context.User != null && Sitecore.Context.User.IsAuthenticated)
            {
                Response.Redirect(ConfigurationManager.AppSettings["access-denied-page"]);
            }

            Response.Redirect("/AuthServices/SignIn?ReturnUrl=" + HttpUtility.UrlEncode(Sitecore.Context.RawUrl));
        }
    }
}