using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WeightLoss
{
    public partial class MP_Groceries : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            MealPlanner mpPage = PreviousPage as MealPlanner;

            if (mpPage != null)
            {
                // Sent here from meal planner
                MultiView mvMealPlanner = mpPage.FindControl("mvMealPlanner") as MultiView;

            }

        }
    }
}