using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class ChangePassword : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (User.Identity.IsAuthenticated)
            // Password changes only allowed when logged in, and when user has recently had password reset
            {
                MembershipUser theUser = Membership.GetUser();

                if (theUser.Comment != "PasswordHasBeenReset")
                {
                    FormsAuthentication.RedirectToLoginPage();
                }
            }
            else
            // No user logged in, redirect back to login page
            {
                Response.Redirect("~/Default.aspx");
            }

            // If we got this far we can proceed with the password change. Set default button and focus
            pnlContent.DefaultButton = ChangePassword1.ChangePasswordTemplateContainer.FindControl("ChangePasswordPushButton").UniqueID;
            ChangePassword1.ChangePasswordTemplateContainer.FindControl("CurrentPassword").Focus();
        }


    }
    protected void ChangePassword1_ContinueButtonClick(object sender, EventArgs e)
    {
        MembershipUser theUser = Membership.GetUser();

        // No longer requires password change
        theUser.Comment = "";
        Membership.UpdateUser(theUser);
        Response.Redirect("~/Default.aspx");
    }
    protected void ChangePassword1_ChangedPassword(object sender, EventArgs e)
    {
        // Password changed, set default button & focus on continue button
        pnlContent.DefaultButton = ChangePassword1.SuccessTemplateContainer.FindControl("ContinuePushButton").UniqueID;
        ChangePassword1.SuccessTemplateContainer.FindControl("ContinuePushButton").Focus();
    }
    protected void ChangePassword1_ChangePasswordError(object sender, EventArgs e)
    {
        ChangePassword1.ChangePasswordTemplateContainer.FindControl("CurrentPassword").Focus();
    }
}