using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using WeightLoss;

public partial class Admin_Users : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            gridUsers.SelectedIndex = 0;
            Page.DataBind();
        }
    }
    protected void Update(object sender, EventArgs e)
    {
        // Update gridview and detailsview with database info
        Page.DataBind();
    }

    protected void detailsUsers_DataBound(object sender, EventArgs e)
    {
    }

    protected void detailsUsers_ItemDeleted(object sender, DetailsViewDeletedEventArgs e)
    {
        // Also remove user from ASP.NET user database
        Membership.DeleteUser(e.Values["UserName"].ToString());

        Update(sender, e as EventArgs);
    }

    protected void detailsUsers_ItemDeleting(object sender, DetailsViewDeleteEventArgs e)
    {
        string username = e.Values["UserName"].ToString();

        if (username == Page.User.Identity.Name)
        // Can't remove self
        {
            e.Cancel = true;
        }
        else
        {
            // Remove all meal plans belonging to user
            MasterPage master = Page.Master as MasterPage;
            int userId = Int32.Parse(((Label)detailsUsers.Rows[0].FindControl("lblUserId")).Text);

            // Get entity for selected user
            //var theUser = (from user in master.foodData.Users
            //               where user.UserId == userId
            //               select user).Single();

            var mealPlans = from mp in master.foodData.MealPlanLists
                            where mp.UserId == userId
                            select mp;

            // Trick to delete objects (MealPlanLists.Remove() sets NULL UserId for some reason)
            mealPlans.ToList().ForEach(r => master.foodData.MealPlanLists.DeleteObject(r));

            master.foodData.SaveChanges();
        }

        
    }
}