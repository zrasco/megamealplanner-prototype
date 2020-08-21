using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Text;
using WeightLoss;

public partial class SignUp : System.Web.UI.Page
{
    private Button SignupBtn;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            SignupBtn = (Button)CreateUserWizard1.FindControl("StepNavigationTemplateContainerID").FindControl("StepNextButton");

            CreateUserWizard1.ActiveStep.FindControl("CreateUserStepContainer").FindControl("UserName").Focus();
        }
    }
    protected void CreateUserWizard1_ContinueButtonClick(object sender, EventArgs e)
    {
        Response.Redirect("~/Default.aspx");
    }


    protected void CreateUserButtonRender(object sender, EventArgs e)
    {
        // Hack to set enter button to next step button
        pnlContainer.DefaultButton = (sender as Button).UniqueID;
    }
    protected void CreateUserWizard1_CreatedUser(object sender, EventArgs e)
    {
        MasterPage master = this.Master as MasterPage;

        CreateUserWizard1.FindControl("CompleteStepContainer").FindControl("ContinueButtonButton").Focus();

        // Create new user in website database (seperate from ASP.NET forms authentication database)

        if (master != null)
        {
            if (master.foodData == null) 
                master.foodData = new foodEntities();

            string txtDOB = (CreateUserWizard1.ActiveStep.FindControl("CreateUserStepContainer").FindControl("txtDOB") as TextBox).Text;
            string FirstName = (CreateUserWizard1.ActiveStep.FindControl("CreateUserStepContainer").FindControl("txtFirstName") as TextBox).Text;
            string LastName = (CreateUserWizard1.ActiveStep.FindControl("CreateUserStepContainer").FindControl("txtLastName") as TextBox).Text;
            // Add user to website database
            master.foodData.Users.AddObject(new User { 
                                            UserName = CreateUserWizard1.UserName,
                                            LastLogin = DateTime.Now,
                                            DateJoined = DateTime.Now,
                                            FirstName = FirstName,
                                            LastName = LastName,
                                            Email = CreateUserWizard1.Email,
                                            DOB = (txtDOB != "" ? DateTime.Parse(txtDOB) : default(DateTime))
            });

            master.foodData.SaveChanges();
        }
    }

    protected void CreateUserWizard1_CreatingUser(object sender, LoginCancelEventArgs e)
    {
        string FirstName = (CreateUserWizard1.ActiveStep.FindControl("CreateUserStepContainer").FindControl("txtFirstName") as TextBox).Text;
        string LastName = (CreateUserWizard1.ActiveStep.FindControl("CreateUserStepContainer").FindControl("txtLastName") as TextBox).Text;
        Literal ErrorMessage = (CreateUserWizard1.ActiveStep.FindControl("CreateUserStepContainer").FindControl("ErrorMessage") as Literal);

        // Server-side validation
        if (FirstName == null)
        {
            ErrorMessage.Text = "First name is required.";
            e.Cancel = true;
        }
        else if (LastName == null)
        {
            ErrorMessage.Text = "Last name is required.";
            e.Cancel = true;
        }
            
    }
}


