using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class ForgotPassword : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (User.Identity.IsAuthenticated)
            // Password resets not allowed when logged in
            {
                Response.Redirect("~/Default.aspx");
            }

            // Set default button in panel to submit
            pnlContainer.DefaultButton = PasswordRecovery1.UserNameTemplateContainer.FindControl("SubmitButton").UniqueID;
            PasswordRecovery1.UserNameTemplateContainer.FindControl("Username").Focus();
        }
    }
    protected void PasswordRecovery1_SendingMail(object sender, MailMessageEventArgs e)
    {
        e.Cancel = true;
        PasswordRecovery1.SuccessText = e.Message.Body;

        MembershipUser theUser = Membership.GetUser(PasswordRecovery1.UserName);
        theUser.Comment = "PasswordHasBeenReset";
        Membership.UpdateUser(theUser);
    }
    protected void PasswordRecovery1_UserLookupError(object sender, EventArgs e)
    {
        // Reset default button in panel to submit again, since at this point default button is now the answer question button
        pnlContainer.DefaultButton = PasswordRecovery1.UserNameTemplateContainer.FindControl("SubmitButton").UniqueID;
        PasswordRecovery1.UserNameTemplateContainer.FindControl("Username").Focus();
    }
    protected void PasswordRecovery1_VerifyingUser(object sender, EventArgs e)
    {
        // Set default button to answer button. If user is invalid, UserLookupError() event will undo this  
        pnlContainer.DefaultButton = PasswordRecovery1.QuestionTemplateContainer.FindControl("SubmitButton").UniqueID;
        PasswordRecovery1.QuestionTemplateContainer.FindControl("Answer").Focus();
    }
    protected void PasswordRecovery1_AnswerLookupError(object sender, EventArgs e)
    {
        // Error finding answer so do same thing as when user verification successful
        PasswordRecovery1_VerifyingUser(sender, e);
    }
}