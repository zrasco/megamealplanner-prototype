using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using WeightLoss;

namespace WeightLoss
{
    public partial class UserMeals : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            MasterPage master = Page.Master as MasterPage;

            if (Page.User.Identity.IsAuthenticated && Membership.GetUser(Page.User.Identity.Name) != null)
            {
                // User is signed in, get UserId
                User foodUser = (from f in master.foodData.Users
                                where (f.UserName == Page.User.Identity.Name)
                                select f).Single();
                dataSourceUserMeals.WhereParameters["UserId"].DefaultValue = foodUser.UserId.ToString();
            }
            else
            {
                dataSourceUserMeals.WhereParameters["UserId"].DefaultValue = "";
            }
        }
    }
}