using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (User.Identity.IsAuthenticated)
        {
            lbl_Default_Status.Text = "Status: Logged in";
        }
        else
        {
            lbl_Default_Status.Text = "Status: Not logged in";
        }
    }
}