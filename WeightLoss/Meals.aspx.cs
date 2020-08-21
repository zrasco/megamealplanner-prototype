using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Collections;
using System.Web.Security;

namespace WeightLoss.Admin
{
    public partial class Meals : System.Web.UI.Page
    {
        private static List<Ingredient> IngredientsList;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                // Get list from entity data source in code (as opposed to binding to a control)
                DataEntityReader.EntityDataSourceReader<Ingredient> entityReader =
                    new DataEntityReader.EntityDataSourceReader<Ingredient>(dataSourceIngredients);

                // Get ingredients data and sort by name
                IngredientsList = entityReader.GetData();
                IngredientsList.Sort((x, y) => string.Compare(x.Name, y.Name));

                detailsIngredients_In_Meals.PageIndex = 0;
                gridIngredients_In_Meals.SelectedIndex = 0;
                gridMeals.SelectedIndex = 0;
                Page.DataBind();

                // With no modifications the meal editor allows admin access, so we'll restrict access here

                if (!(Page.User.Identity.IsAuthenticated && Membership.GetUser(Page.User.Identity.Name) != null))
                {
                    // User is not logged in. This feature is only available to members
                    pnlContainer.Visible = false;
                    pnlVisitor.Visible = true;
                }
                else if (User.IsInRole("admin") == false)
                {
                    // User is not an admin. Modify queries to only show meals by user ID
                    User foodUser = (from f in (Page.Master as MasterPage).foodData.Users
                                     where (f.UserName == Page.User.Identity.Name)
                                     select f).Single();

                    // Step #1: Change meal selector to only show meals from this user
                    //dataSourceMeals.Where = "(it.UserId = cast(@UserId as System.Int32))";
                    dataSourceMeals.WhereParameters.Add("UserId", TypeCode.Int32, foodUser.UserId.ToString());
                    dataSourceMeals.AutoGenerateWhereClause = true;

                    // Step #2: Hide UserId field (and inject foodUser.UserId again later
                    // to prevent client-side page modifications)

                    // UserId column & row has index of two
                    gridMeals.Columns.RemoveAt(2);
                    detailsMeal.Fields.RemoveAt(1);
                    

                    // Step #3: Do the same for add new meal

                    gridMeals.SelectRow(0);
                    Update(null, null);
                }
            }

        }

        protected void Update(object sender, EventArgs e)
        {
            // Update gridview and detailsview with database info
            gridMeals.DataBind();
            detailsMeal.DataBind();
            gridIngredients_In_Meals.DataBind();
            detailsIngredients_In_Meals.DataBind();
            
        }
        protected void detailsMeal_DataBound(object sender, EventArgs e)
        {
        }
        protected void detailsIngredients_In_Meals_DataBound(object sender, EventArgs e)
        {
            if (detailsIngredients_In_Meals.CurrentMode == DetailsViewMode.Edit ||
                detailsIngredients_In_Meals.CurrentMode == DetailsViewMode.Insert)
            // Populate dropdown list with ingredients
            {
                // Select dropdown list name based on edit mode
                string modeString = "";
                switch (detailsIngredients_In_Meals.CurrentMode)
                {
                    case DetailsViewMode.Edit: modeString = "Edit"; break;
                    case DetailsViewMode.Insert: modeString = "New"; break;
                }

                // Clear & re-populate dropdown list with fully qualified ingredient names
                DropDownList ddl = detailsIngredients_In_Meals.FindControl("lstIngredients_In_Details_" + modeString) as DropDownList;
                Ingredients_In_Meals ing = detailsIngredients_In_Meals.DataItem as Ingredients_In_Meals;
                StringBuilder str = new StringBuilder();

                if (ddl != null)
                {
                    ddl.Items.Clear();

                    for (int counter = 0; counter < IngredientsList.Count(); counter++)
                    {
                        Ingredient i = IngredientsList[counter];
                        ListItem item = new ListItem();

                        item.Value = i.IngredientId.ToString();
                        item.Text = FullyQuallifiedIngredientName(i);

                        ddl.Items.Add(item);

                        if (ing != null && i.IngredientId == ing.IngredientId)
                        {
                            // Select current ingredient
                            ddl.SelectedIndex = counter;
                        }
                    }
                }

                // Set default value of quantity to 1
                TextBox txtQuantity = detailsIngredients_In_Meals.FindControl("txtQuantity_New") as TextBox;

                if (txtQuantity != null)
                {
                    txtQuantity.Text = "1";
                }
            }
            else if (detailsIngredients_In_Meals.CurrentMode == DetailsViewMode.ReadOnly &&
                    detailsIngredients_In_Meals.DataItem == null)
            {
                // Meal with no ingredients
            }
        }

        protected string FullyQuallifiedIngredientName(Ingredient i)
        {
            StringBuilder str = new StringBuilder();

            str.Append(i.Name);
            str.Append("(");
            str.Append(((i.Quantity_Nbr % 1) == 0 ? Convert.ToInt32(i.Quantity_Nbr) : i.Quantity_Nbr));
            str.Append(" ");
            str.Append(i.Quantity_Measurement);
            str.Append(")");

            return str.ToString();
        }

        protected string GetIngredientFromId(int IngredientId)
        {
            foreach (Ingredient i in IngredientsList)
            {
                if (i.IngredientId == IngredientId)
                    return FullyQuallifiedIngredientName(i);
            }

            return "";
        }

        protected void lstMeals_SelectedIndexChanged(object sender, EventArgs e)
        {
            detailsIngredients_In_Meals.SetPageIndex(0);
            gridIngredients_In_Meals.SetPageIndex(0);
            gridIngredients_In_Meals.SelectRow(0);
        }
        protected void detailsIngredients_In_Meals_PageIndexChanged(object sender, EventArgs e)
        {
            // Change row of Ingredients list on the right
            int index = ((detailsIngredients_In_Meals.PageIndex) / gridIngredients_In_Meals.PageSize);
            gridIngredients_In_Meals.PageIndex = index;
            gridIngredients_In_Meals.SelectedIndex = detailsIngredients_In_Meals.PageIndex % gridIngredients_In_Meals.PageSize;
        }

        protected void detailsIngredients_In_Meals_ItemCommand(object sender, DetailsViewCommandEventArgs e)
        {
            Page.Validate();

            if (!Page.IsValid && e.CommandName != "Cancel")
            {
                e.Handled = true;
                return;
            }
            switch (e.CommandName)
            {
                case "New":
                    if (gridMeals.Rows.Count == 0)
                        e.Handled = true;
                    else
                    {
                        gridIngredients_In_Meals.Enabled = false;
                        gridMeals.Enabled = false;
                        detailsMeal.Enabled = false;
                    }
                    break;
                case "Edit":
                    gridIngredients_In_Meals.Enabled = false;
                    gridMeals.Enabled = false;
                    detailsMeal.Enabled = false;
                    break;
                case "Update":
                case "Insert":
                case "Cancel":
                    gridIngredients_In_Meals.Enabled = true;
                    gridMeals.Enabled = true;
                    detailsMeal.Enabled = true;
                    break;
                case "Delete":
                    detailsIngredients_In_Meals.PageIndex = 0;
                    gridIngredients_In_Meals.PageIndex = 0;
                    gridIngredients_In_Meals.SelectedIndex = 0;
                    break;
            }

        }

        protected void gridIngredients_In_Meals_PageIndexChanged(object sender, EventArgs e)
        {
            // Change DetailsView ingredient when user changes page in GridView
            detailsIngredients_In_Meals.PageIndex = gridIngredients_In_Meals.PageSize * gridIngredients_In_Meals.PageIndex;
            gridIngredients_In_Meals.SelectRow(0);
        }
        protected void dataSourceIngredients_In_Meals_Inserting(object sender, EntityDataSourceChangingEventArgs e)
        {
            // MealId doesn't get added from markup alone, so we'll add it here
            Ingredients_In_Meals iim = (e.Entity as Ingredients_In_Meals);

            if (iim != null)
                iim.MealId = (int)(gridMeals.SelectedDataKey.Value);

        }

        protected void dataSourceMealDetails_Inserting(object sender, EntityDataSourceChangingEventArgs e)
        {

            if ((Page.User.Identity.IsAuthenticated && Membership.GetUser(Page.User.Identity.Name) != null) &&
                User.IsInRole("admin") == false)
            {
                // Get foodUser
                User foodUser = (from f in (Page.Master as MasterPage).foodData.Users
                                 where (f.UserName == Page.User.Identity.Name)
                                 select f).Single();

                // If user is not an admin, inject UserId into meal
                Meal theMeal = (e.Entity as Meal);

                if (theMeal != null)
                    theMeal.UserId = foodUser.UserId;
            }
        }

        protected void dataSourceMealDetails_Deleting(object sender, EntityDataSourceChangingEventArgs e)
        {
            Meal theMeal = e.Entity as Meal;
            MasterPage master = Page.Master as MasterPage;

            // Delete all ingredients in meals before deleting meal

            // Find entries
            var iim = from ingim in master.foodData.Ingredients_In_Meals
                        where (ingim.MealId == theMeal.MealId)
                        select ingim;

            // Delete all ingredients in meals that have this meal id
            iim.ToList().ForEach(r => master.foodData.Ingredients_In_Meals.DeleteObject(r));
            master.foodData.SaveChanges();
        }

        protected void detailsMeal_ItemCommand(object sender, DetailsViewCommandEventArgs e)
        {
            // Check validation controls
            Page.Validate();
            if (!Page.IsValid && e.CommandName != "Cancel")
            {
                e.Handled = true;
                return;
            }
            switch (e.CommandName)
            {
                //case "New":
                //case "Edit":
                //    gridIngredients_In_Meals.Enabled = false;
                //    gridMeals.Enabled = false;
                //    detailsMeal.Enabled = false;
                //    break;
                //case "Update":
                //case "Insert":
                //case "Cancel":
                //    gridIngredients_In_Meals.Enabled = true;
                //    gridMeals.Enabled = true;
                //    detailsMeal.Enabled = true;
                //    break;
                case "Delete":
                    gridMeals.PageIndex = 0;
                    gridMeals.SelectedIndex = 0;
                    break;
            }

        }
    }
}