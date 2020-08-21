using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using WeightLoss;

public partial class MasterPage : System.Web.UI.MasterPage
{
    public foodEntities foodData = new foodEntities();
    public User foodUserRO = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        // Prevent confusion accessing aspnetdb locally
        Roles.ApplicationName = "/WeightLoss";

        // Set button hover effects

        // Top buttons
        imgBtnHome.Attributes.Add("onmouseover", "javascript: this.src='" + VirtualPathUtility.ToAbsolute("~/Images/home2.png") + "'");
        imgBtnHome.Attributes.Add("onmouseout", "javascript: this.src='" + VirtualPathUtility.ToAbsolute("~/Images/home1.png") + "'");

        imgBtnMeals.Attributes.Add("onmouseover", "javascript: this.src='" + VirtualPathUtility.ToAbsolute("~/Images/meals2.png") + "'");
        imgBtnMeals.Attributes.Add("onmouseout", "javascript: this.src='" + VirtualPathUtility.ToAbsolute("~/Images/meals1.png") + "'");

        imgBtnNews.Attributes.Add("onmouseover", "javascript: this.src='" + VirtualPathUtility.ToAbsolute("~/Images/news2.png") + "'");
        imgBtnNews.Attributes.Add("onmouseout", "javascript: this.src='" + VirtualPathUtility.ToAbsolute("~/Images/news1.png") + "'");

        imgBtnFAQ.Attributes.Add("onmouseover", "javascript: this.src='" + VirtualPathUtility.ToAbsolute("~/Images/faq2.png") + "'");
        imgBtnFAQ.Attributes.Add("onmouseout", "javascript: this.src='" + VirtualPathUtility.ToAbsolute("~/Images/faq1.png") + "'");

        imgBtnContactUs.Attributes.Add("onmouseover", "javascript: this.src='" + VirtualPathUtility.ToAbsolute("~/Images/contact2.png") + "'");
        imgBtnContactUs.Attributes.Add("onmouseout", "javascript: this.src='" + VirtualPathUtility.ToAbsolute("~/Images/contact1.png") + "'");

        if (!Page.User.Identity.IsAuthenticated)
        {
            // Hover effects for log in/sign up buttons
            ImageButton imgBtnLogin = LoginView_Master.FindControl("loginCtl").FindControl("imgBtnLogin") as ImageButton;

            if (imgBtnLogin != null)
            {
                imgBtnLogin.Attributes.Add("onmouseover", "javascript: this.src='" + VirtualPathUtility.ToAbsolute("~/Images/login2.png") + "'");
                imgBtnLogin.Attributes.Add("onmouseout", "javascript: this.src='" + VirtualPathUtility.ToAbsolute("~/Images/login1.png") + "'");
                imgBtnLogin.Attributes.Add("style", "outline: none");
            }
            ImageButton imgBtnSignUp = LoginView_Master.FindControl("loginCtl").FindControl("imgBtnSignUp") as ImageButton;
            if (imgBtnSignUp != null)
            {
                imgBtnSignUp.Attributes.Add("onmouseover", "javascript: this.src='" + VirtualPathUtility.ToAbsolute("~/Images/signup2.png") + "'");
                imgBtnSignUp.Attributes.Add("onmouseout", "javascript: this.src='" + VirtualPathUtility.ToAbsolute("~/Images/signup1.png") + "'");
                imgBtnSignUp.Attributes.Add("style", "outline: none");
            }

            // Set log in button as default button
            form1.DefaultButton = imgBtnLogin.UniqueID;
        }
        else
        {
            if (Membership.GetUser(Page.User.Identity.Name) == null)
            {
                FormsAuthentication.SignOut();
                Response.Redirect("~/Default.aspx");
            }
            else
            {
                // Find matching food_User
                User foodUser = (from f in foodData.Users
                            where (f.UserName == Page.User.Identity.Name)
                            select f).Single();

                // Set last login time to now
                foodUser.LastLogin = DateTime.Now;
                foodData.SaveChanges();

                // Make copy in global variable for read-only purposes. Regular foodUser goes out of scope
                foodUserRO = new User();

                foodUserRO.FirstName = foodUser.FirstName;
                foodUserRO.LastName = foodUser.LastName;
                foodUserRO.UserId = foodUser.UserId;
                foodUserRO.UserName = foodUser.UserName;
                foodUserRO.Email = foodUser.Email;
                foodUserRO.DOB = foodUser.DOB;
            }

        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        // If user is logged in, get loginview label and populate with user-specific info
        if (Page.User.Identity.IsAuthenticated && Membership.GetUser(Page.User.Identity.Name) != null)
        {
            Label lblLVStatus = (Label)LoginView_Master.FindControl("lblLVStatus");

            if (lblLVStatus != null)
            // Will only be non-null when user is actually logged in
            {
                StringBuilder welcome = new StringBuilder();
                // Find matching food_User
                User foodUser = (from f in foodData.Users
                                   where (f.UserName == Page.User.Identity.Name)
                                   select f).Single();

                welcome.Append("Welcome, ");
                welcome.Append(foodUser.FirstName);
                welcome.Append("!<br />");
                welcome.Append("Custom meals: ");
                welcome.Append(foodUser.Meals.Count());
                welcome.Append("<br />");

                // Edit meals
                welcome.Append("<a href='");
                welcome.Append(VirtualPathUtility.ToAbsolute("~/Meals.aspx"));
                welcome.Append("'>Meals</a>");
                welcome.Append("<br />");

                // Meal planner
                welcome.Append("<a href='");
                welcome.Append(VirtualPathUtility.ToAbsolute("~/MealPlanner.aspx"));
                welcome.Append("'>Meal planner</a>");
                welcome.Append("<br />");

                if (Page.User.IsInRole("admin"))
                // Add administrator functions
                {
                    // Edit users
                    welcome.Append("<br />");
                    welcome.Append("<a href='");
                    welcome.Append(VirtualPathUtility.ToAbsolute("~/Admin/Users.aspx"));
                    welcome.Append("'>Users</a>");
                    welcome.Append("<br />");

                    // Edit ingredients
                    welcome.Append("<a href='");
                    welcome.Append(VirtualPathUtility.ToAbsolute("~/Admin/Ingredients.aspx"));
                    welcome.Append("'>Ingredients</a>");
                }

                lblLVStatus.Text = welcome.ToString();
            }

            // Check if password needs to be changed. If so, redirect to password change page
            MembershipUser theUser = Membership.GetUser();

            if (theUser != null && theUser.Comment == "PasswordHasBeenReset" &&
                ((((System.Web.UI.Control)(contentMain)).BindingContainer).TemplateControl).AppRelativeVirtualPath != "~/ChangePassword.aspx")
            {
                Response.Redirect("~/ChangePassword.aspx");
            }
        }
        else
        {
            // Set default button of login area to login button (can't do this in markup since button is inside panel)
            //LoginView theLoginView = LoginView_Master;
            //Button theButton = (Button)LoginView_Master.FindControl("btnLogin");
            //pnlLogin.DefaultButton = "loginCtl$LoginButton";
        }
    }

    protected void btnSignOut_Click(object sender, EventArgs e)
    {
        FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx");
    }

    public static T FindControl<T>(System.Web.UI.ControlCollection Controls, string ControlName) where T : class
    {
        T found = default(T);

        if (Controls != null && Controls.Count > 0)
        {
            for (int i = 0; i < Controls.Count; i++)
            {
                if (Controls[i] is T && Controls[i].ID == ControlName)
                {
                    found = Controls[i] as T;
                    break;
                }
                else
                    found = FindControl<T>(Controls[i].Controls, ControlName);
            }
        }

        return found;
    }
    protected void loginCtl_LoggedIn(object sender, EventArgs e)
    {


    }
}
