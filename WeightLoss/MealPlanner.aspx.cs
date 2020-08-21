using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Text;

namespace WeightLoss
{
    public struct Meal_Tally_IngredientListItem
    {
        public int IngredientId { get; set; }
        public int Quantity_In_Meal { get; set; }
    }
    public class Meal_Tally_Entry
    // MealId is hashkey
    {
        public int Meal_Occurances { get; set; }                                    // Number of total occurances for meal
        public List<Meal_Tally_IngredientListItem> ListIngredients { get; set; }    // List of ingredients in meal

        public Meal_Tally_Entry()
        // Constructor
        {
            Meal_Occurances = 1;
            ListIngredients = new List<Meal_Tally_IngredientListItem>();
        }
    }
    public class Ingredient_Tally_Entry
    // IngredientId is hash key
    {
        public int Meal_Occurances { get; set; }        // Number of meal occurances containing ingredient
        public int Sum_Of_Quantities { get; set; }      // Sum of all quantities of ingredient per unique Meal ID
        public decimal Qty_Nbr { get; set; }            // Qty_Nbr from Ingredients table
        public string Qty_Measurement { get; set; }     // Type of measurement from Ingredients table
        public string Name { get; set; }                // Name of the ingredient
        public string Category { get; set; }            // Category of the ingredient
    }

    public partial class MealPlanner : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Set startup script
            Type type = GetType();
            const string scriptName = "pageLoad";
            if (!ClientScript.IsStartupScriptRegistered(type, scriptName))
                ClientScript.RegisterStartupScript(type, scriptName, "pageLoad()", true);

            if (!(Page.User.Identity.IsAuthenticated && Membership.GetUser(Page.User.Identity.Name) != null))
            {
                // User not logged in
                radioUseCustomMeals.Visible = false;
                radioUseCustomMeals.SelectedIndex = 1;
            }

            if (mvMealPlanner.ActiveViewIndex == 1 && mvMealPlanner.FindControl("Breakfast_0") == null)
            {
                CreateTable(false);
            }

            if (ViewState["ShowGroceries"] == null)
                lblPrintOutput.Text = "";


        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (ViewState["ShowGroceries"] != null)
            {
                // Print grocery list
                CreateGroceryList(lblPrintOutput);

                ViewState["ShowGroceries"] = null;
            }
        }

        private void CreateGroceryList(Label lblTarget)
        {
            // Make grocery list and output to target label
            MasterPage master = Page.Master as MasterPage;
            Hashtable hashMfg = new Hashtable();
            Hashtable hashIngredients = new Hashtable();
            List<Ingredient_Tally_Entry> ingList = new List<Ingredient_Tally_Entry>();
            int numberofMeals = Int32.Parse((mvMealPlanner.FindControl("lstNbrOfMeals") as DropDownList).SelectedValue);
            int numberOfDays = Int32.Parse((mvMealPlanner.FindControl("lstDuration") as DropDownList).SelectedValue) * 7;
            DropDownList ddl = null;
            string[] controlPrefixes;
            Ingredient_Tally_Entry ite = null;
            StringBuilder outputSB = new StringBuilder();

            // Step 1: Get list of all MealIds and number of occurances
            switch (numberofMeals)
            {
                default:
                case 3:
                    controlPrefixes = new string[] { "Breakfast_", "Lunch_", "Dinner_" };
                    break;
                case 5:
                    controlPrefixes = new string[] { "Breakfast_", "Snack1_", "Lunch_", "Snack2_", "Dinner_" };
                    break;
                case 6:
                    controlPrefixes = new string[] { "Breakfast_", "Snack1_", "Lunch_", "Snack2_", "Dinner_", "Snack3_" };
                    break;
            }

            // Step #1 - Get number of occurances per meal
            for (int count = 0; count < numberOfDays; count++)
            {
                // Fill hashtable with meals from dropdown lists
                for (int count2 = 0; count2 < controlPrefixes.Count(); count2++)
                {
                    int MealId;
                    ddl = mvMealPlanner.FindControl(controlPrefixes[count2] + count.ToString()) as DropDownList;
                    MealId = Int32.Parse(ddl.SelectedValue);

                    if (hashMfg[MealId] == null)
                        hashMfg[MealId] = new Meal_Tally_Entry();
                    else
                        (hashMfg[MealId] as Meal_Tally_Entry).Meal_Occurances++;
                }
            }

            // Step #2 - Get ingredients in each meal
            foreach (int MealId in hashMfg.Keys)
            {
                // Go through each unique meal
                Meal_Tally_Entry mte = hashMfg[MealId] as Meal_Tally_Entry;
                Meal_Tally_IngredientListItem mtil = default(Meal_Tally_IngredientListItem);

                var iimList = from iim in master.foodData.Ingredients_In_Meals
                              where iim.MealId == MealId
                              select iim;

                foreach (Ingredients_In_Meals iim in iimList)
                {
                    // Add ingredients to meal hash table
                    mtil.IngredientId = iim.IngredientId;
                    mtil.Quantity_In_Meal = iim.Quantity;
                    (hashMfg[MealId] as Meal_Tally_Entry).ListIngredients.Add(mtil);

                    // Add ingredients to ingredients hash table
                    if (hashIngredients[iim.IngredientId] == null)
                        hashIngredients[iim.IngredientId] = new Ingredient_Tally_Entry();

                    ite = hashIngredients[iim.IngredientId] as Ingredient_Tally_Entry;

                    ite.Sum_Of_Quantities += (iim.Quantity * mte.Meal_Occurances);
                    ite.Meal_Occurances += mte.Meal_Occurances;
                }
            }
            
            // Step 3: Get quantity and measurement of ingredients from ingredients table
            foreach (int IngredientId in hashIngredients.Keys)
            {
                Ingredient entry = (from ing in master.foodData.Ingredients
                                    where ing.IngredientId == IngredientId
                                    select ing).Single();

                ite = hashIngredients[IngredientId] as Ingredient_Tally_Entry;

                ite.Qty_Nbr = entry.Quantity_Nbr;
                ite.Qty_Measurement = entry.Quantity_Measurement;
                ite.Name = entry.Name;
                ite.Category = (entry.Category == null ? "Uncategorized" : entry.Category);

                ingList.Add(ite);
            }

            // Step 4: Sort list by category descending, then by name ascending
            List<Ingredient_Tally_Entry> ingList_Sorted = ingList.
                                            OrderByDescending(i => i.Category).
                                            ThenBy(i => i.Name).ToList();

            // Step 5: Output list to target label
            Ingredient_Tally_Entry iFirst = ingList_Sorted.First();
            string category = iFirst.Category;

            // Start making table
            outputSB.Append("<table>");

            foreach (Ingredient_Tally_Entry i in ingList_Sorted)
            {
                if (category != i.Category || i == iFirst)
                // Category has changed, output category first
                {
                    category = i.Category;

                    outputSB.Append((i == iFirst ? "" : "<tr style='height: 10px'></tr>") +"<tr><td colspan=2><b><u>");
                    outputSB.Append(category);
                    outputSB.Append("</u></b></td></tr>");
                }

                // 10 jumbo Egg
                // 20 tbsp peanut butter
                // 5.5 cups baked beans
                outputSB.Append(string.Format("<tr><td style='width: 80px'>{0} {1}</td><td>{2}</td>",
                                                            ((i.Qty_Nbr % 1) == 0 ? Convert.ToInt32(i.Qty_Nbr * i.Sum_Of_Quantities) : ((decimal)(i.Qty_Nbr * (decimal)i.Sum_Of_Quantities))),
                                                            i.Qty_Measurement,
                                                            i.Name));
            }

            outputSB.Append("</table>");

            lblTarget.Text = outputSB.ToString();
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            MasterPage master = (MasterPage)Page.Master;
            List<string> cuisineList = new List<string>();
            var foodUserSelect = (from f in master.foodData.Users
                                  where (f.UserName == Page.User.Identity.Name)
                                  select f);
            User foodUser = null;

            // LinQ expressions won't work without this
            int UserId = 0;

            // Being a registered user is optional, we'll check if foodUser is NULL in conditions
            if (foodUserSelect.Count() > 0)
            {
                foodUser = foodUserSelect.Single();
                UserId = foodUser.UserId;
            }

            // Clear error
            lblError.Text = "";

            // Make cuisine list from checked cuisines
            foreach (ListItem ckItem in chklistCuisine.Items)
            {
                if (ckItem.Selected)
                {
                    if (ckItem.Text == "Uncategorized")
                        cuisineList.Add(null);
                    else
                        cuisineList.Add(ckItem.Text);
                }
            }

            if (cuisineList.Count == 0)
            {
                // No cuisines selected, do nothing
                lblError.Text = "No cuisines selected!";
                return;
            }

            // Make lists of meals & snacks
            IEnumerable<Meal> aList = null;        // All meals
            IEnumerable<Meal> bList = null;        // Breakfasts
            IEnumerable<Meal> dList = null;        // Lunches
            IEnumerable<Meal> lList = null;        // Dinners
            IEnumerable<Meal> sList = null;        // Snacks

            if (foodUser != null && radioUseCustomMeals.SelectedValue == "0")
            {
                // Use custom meals only
                aList = from m in foodUser.Meals
                        where cuisineList.Contains(m.Cuisine)
                        select m;
            }
            else
            {
                // Use custom + preset meals
                aList = from m in master.foodData.Meals
                            where cuisineList.Contains(m.Cuisine) && (m.UserId == UserId || m.UserId == null)
                            select m;
            }

            if (aList.Count() == 0)
            // No meals whatsoever
            {
                lblError.Text = "No matching meals!";
                return;
            }

            // Breakfasts
            bList = from m in aList
                    where m.Breakfast == true
                    select m;

            if (bList.Count() == 0)
            // No breakfasts
            {
                lblError.Text = "No matching breakfasts!";
                return;
            }

            // Lunches
            lList = from m in aList
                    where m.Lunch == true
                    select m;

            if (lList.Count() == 0)
            // No lunches
            {
                lblError.Text = "No matching lunches!";
                return;
            }

            // Dinners
            dList = from m in aList
                    where m.Dinner == true
                    select m;

            if (dList.Count() == 0)
            // No dinners
            {
                lblError.Text = "No matching dinners!";
                return;
            }

            // Snacks
            sList = from m in aList
                    where m.Snack == true
                    select m;

            if (sList.Count() == 0)
            // No snacks
            {
                lblError.Text = "No matching snacks!";
                return;
            }

            // Copy lists to session variables
            Session["aList"] = aList.ToList();
            Session["bList"] = bList.ToList();
            Session["lList"] = lList.ToList();
            Session["dList"] = dList.ToList();
            Session["sList"] = sList.ToList();

            // Set view index to meal plan
            mvMealPlanner.ActiveViewIndex = 1;

            // Create the table here and randomize (table will also be created on postbacks)
            CreateTable(true);
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            mvMealPlanner.ActiveViewIndex = 0;
            mvMealPlanner.Controls.Remove(mvMealPlanner.FindControl("tblMealPlanList"));
        }
        protected void CreateTable(bool randomize)
        {
            DateTime currentDate = DateTime.Today.AddDays(1);
            Random rnd = new Random(DateTime.Now.Millisecond);
            Table tblDay = new Table();

            // Get lists of meals and snacks from session variables
            List<Meal> aList = Session["aList"] as List<Meal>;        // All meals
            List<Meal> bList = Session["bList"] as List<Meal>;        // Breakfasts
            List<Meal> dList = Session["dList"] as List<Meal>;        // Lunches
            List<Meal> lList = Session["lList"] as List<Meal>;        // Dinners
            List<Meal> sList = Session["sList"] as List<Meal>;        // Snacks

            // Start tomorrow
            for (int count = 0; count < Int32.Parse(lstDuration.SelectedValue) * 7; count++)
            {
                TableRow tblRow = new TableRow();
                TableCell tblCell = new TableCell();

                // Create bold header for current date
                tblCell.Controls.Add(new Literal
                {
                    Text = "<b>" + currentDate.Date.ToString("d") + "(" +
                    currentDate.DayOfWeek.ToString() + ")</b>"

                });

                tblCell.ColumnSpan = 2;
                tblRow.Cells.Add(tblCell);
                tblDay.Rows.Add(tblRow);

                // Add breakfast
                tblRow = new TableRow();
                tblCell = new TableCell();
                tblCell.Width = 80;
                tblCell.Controls.Add(new Literal
                {
                    Text = "Breakfast:"
                });

                tblRow.Cells.Add(tblCell);
                tblCell = new TableCell();

                DropDownList ddlBreakfast = new DropDownList();
                ddlBreakfast.CssClass = "MealPlanDropDown";
                ddlBreakfast.ID = "Breakfast_" + count.ToString();
                ddlBreakfast.DataSource = bList;
                ddlBreakfast.DataTextField = "Name";
                ddlBreakfast.DataValueField = "MealId";
                ddlBreakfast.DataBind();
                if (randomize) ddlBreakfast.SelectedIndex = rnd.Next(0, ddlBreakfast.Items.Count);

                tblCell.Controls.Add(ddlBreakfast);
                tblRow.Cells.Add(tblCell);
                tblDay.Rows.Add(tblRow);

                // Add snack #1 if user selected 5 or 6 meals
                if (Int32.Parse(lstNbrofMeals.SelectedItem.Text) >= 5)
                {
                    tblRow = new TableRow();
                    tblCell = new TableCell();
                    tblCell.Width = 80;
                    tblCell.Controls.Add(new Literal
                    {
                        Text = "Snack #1:"
                    });

                    tblRow.Cells.Add(tblCell);
                    tblCell = new TableCell();

                    DropDownList ddlSnack1 = new DropDownList();
                    ddlSnack1.CssClass = "MealPlanDropDown";
                    ddlSnack1.ID = "Snack1_" + count.ToString();
                    ddlSnack1.DataSource = sList;
                    ddlSnack1.DataTextField = "Name";
                    ddlSnack1.DataValueField = "MealId";
                    ddlSnack1.DataBind();
                    if (randomize) ddlSnack1.SelectedIndex = rnd.Next(0, ddlSnack1.Items.Count);

                    tblCell.Controls.Add(ddlSnack1);
                    tblRow.Cells.Add(tblCell);
                    tblDay.Rows.Add(tblRow);
                }


                // Add lunch
                tblRow = new TableRow();
                tblCell = new TableCell();
                tblCell.Width = 80;
                tblCell.Controls.Add(new Literal
                {
                    Text = "Lunch:"
                });

                tblRow.Cells.Add(tblCell);
                tblCell = new TableCell();

                DropDownList ddlLunch = new DropDownList();
                ddlLunch.CssClass = "MealPlanDropDown";
                ddlLunch.ID = "Lunch_" + count.ToString();
                ddlLunch.DataSource = lList;
                ddlLunch.DataTextField = "Name";
                ddlLunch.DataValueField = "MealId";
                ddlLunch.DataBind();
                if (randomize) ddlLunch.SelectedIndex = rnd.Next(0, ddlLunch.Items.Count);

                tblCell.Controls.Add(ddlLunch);
                tblRow.Cells.Add(tblCell);
                tblDay.Rows.Add(tblRow);

                // Add snack #2 if user selected 5 or 6 meals
                if (Int32.Parse(lstNbrofMeals.SelectedItem.Text) >= 5)
                {
                    tblRow = new TableRow();
                    tblCell = new TableCell();
                    tblCell.Width = 80;
                    tblCell.Controls.Add(new Literal
                    {
                        Text = "Snack #2:"
                    });

                    tblRow.Cells.Add(tblCell);
                    tblCell = new TableCell();

                    DropDownList ddlSnack2 = new DropDownList();
                    ddlSnack2.CssClass = "MealPlanDropDown";
                    ddlSnack2.ID = "Snack2_" + count.ToString();
                    ddlSnack2.DataSource = sList;
                    ddlSnack2.DataTextField = "Name";
                    ddlSnack2.DataValueField = "MealId";
                    ddlSnack2.DataBind();
                    if (randomize) ddlSnack2.SelectedIndex = rnd.Next(0, ddlSnack2.Items.Count);

                    tblCell.Controls.Add(ddlSnack2);
                    tblRow.Cells.Add(tblCell);
                    tblDay.Rows.Add(tblRow);
                }

                // Add dinner
                tblRow = new TableRow();
                tblCell = new TableCell();
                tblCell.Width = 80;
                tblCell.Controls.Add(new Literal
                {
                    Text = "Dinner:"
                });

                tblRow.Cells.Add(tblCell);
                tblCell = new TableCell();

                DropDownList ddlDinner = new DropDownList();
                ddlDinner.CssClass = "MealPlanDropDown";
                ddlDinner.ID = "Dinner_" + count.ToString();
                ddlDinner.DataSource = dList;
                ddlDinner.DataTextField = "Name";
                ddlDinner.DataValueField = "MealId";
                ddlDinner.DataBind();
                if (randomize) ddlDinner.SelectedIndex = rnd.Next(0, ddlDinner.Items.Count);

                tblCell.Controls.Add(ddlDinner);
                tblRow.Cells.Add(tblCell);
                tblDay.Rows.Add(tblRow);

                // Add snack #3 if user selected 5 or 6 meals
                if (Int32.Parse(lstNbrofMeals.SelectedItem.Text) >= 6)
                {
                    tblRow = new TableRow();
                    tblCell = new TableCell();
                    tblCell.Width = 80;
                    tblCell.Controls.Add(new Literal
                    {
                        Text = "Snack #3:"
                    });

                    tblRow.Cells.Add(tblCell);
                    tblCell = new TableCell();

                    DropDownList ddlSnack3 = new DropDownList();
                    ddlSnack3.CssClass = "MealPlanDropDown";
                    ddlSnack3.ID = "Snack3_" + count.ToString();
                    ddlSnack3.DataSource = sList;
                    ddlSnack3.DataTextField = "Name";
                    ddlSnack3.DataValueField = "MealId";
                    ddlSnack3.DataBind();
                    if (randomize) ddlSnack3.SelectedIndex = rnd.Next(0, ddlSnack3.Items.Count);

                    tblCell.Controls.Add(ddlSnack3);
                    tblRow.Cells.Add(tblCell);
                    tblDay.Rows.Add(tblRow);
                }

                // Advance to next date
                currentDate = currentDate.AddDays(1);
            }

            // Add table to panel
            tblDay.Width = 600;
            tblDay.EnableViewState = true;
            tblDay.ViewStateMode = ViewStateMode.Enabled;
            tblDay.ID = "tblMealPlanList";
            pnlMealPlanList.Controls.Add(tblDay);
        }

        protected void btnGroceries_Click(object sender, EventArgs e)
        {
            ViewState["ShowGroceries"] = true;
        }
    }
}