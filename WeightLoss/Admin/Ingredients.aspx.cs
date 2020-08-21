using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Ingredients : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            gridIngredients.SelectedIndex = 0;
            Page.DataBind();
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {

    }
    protected void Update(object sender, EventArgs e)
    {
        // Update gridview and detailsview with database info
        Page.DataBind();
    }

    protected void detailsIngredients_DataBound(object sender, EventArgs e)
    {
    }
}