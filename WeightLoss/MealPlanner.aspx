<%@ Page MaintainScrollPositionOnPostback="true" Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="MealPlanner.aspx.cs" Inherits="WeightLoss.MealPlanner" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentMain" runat="server">
    <script type="text/javascript">
        function pageLoad() {

            // printOutput contains our grocery list, generated server-side
            var printOutput = document.getElementById('<%=lblPrintOutput.ClientID%>');

            if (printOutput.textContent != "") {
                newwindow = popupWindow();
                newwindow.document.body.innerHTML = printOutput.innerHTML;
                printOutput.textContent = "";
            }
        }
        function CheckAll(check) {
            var chkControlId = document.getElementById('<%=chklistCuisine.ClientID%>');
            var options = chkControlId.getElementsByTagName('input');


            for (i = 0; i < options.length; i++) {
                var opt = options[i];

                if (opt.type == "checkbox") {
                    opt.checked = check;
                }
            }
        }

        function fillPrintLabel() {
            var divPanel = document.getElementById('<%=pnlMealPlanList.ClientID%>');
            var theRows = divPanel.getElementsByTagName("tr");
            var dropDowns = divPanel.getElementsByClassName("MealPlanDropDown");
            var printOutput = document.getElementById('<%=lblPrintOutput.ClientID%>');
            var outputString = "";

            // Start of the table
            outputString += "<table>";

            // Make the print table
            for (i = 0; i < theRows.length; i++) {
                var cellsInRow = theRows[i].cells.length;

                if (cellsInRow == 1) {
                    // Header
                    // Put spacer (on all header rows except 1st)
                    if (i != 0)
                        outputString += "<tr><td colspan=2><br /></td></tr>";

                    outputString += "<tr><td colspan=2>" + theRows[i].cells[0].innerHTML + "</td>"
                }
                else {
                    // Regular row w/dropdown list (ddl below)
                    var ddl = theRows[i].cells[1].getElementsByTagName("select");

                    // Meal type + meal itself
                    outputString += "<tr><td>" + theRows[i].cells[0].innerHTML + "</td>";
                    outputString += "<td>" + ddl[0].options[ddl[0].selectedIndex].text + "</td>";
                }

                outputString += "</tr>"
            }

            // End of the table
            outputString += "</table>";

            // Create popup window and adjust settings
            newwindow = popupWindow();
            newwindow.document.body.innerHTML += outputString;

        }
        function popupWindow() {
            newwindow = window.open(null, "_blank", "width=800,height=600,menubar=yes,toolbar=yes,scrollbars=yes");
            newwindow.document.body.style.background = "#a09e91";
            newwindow.document.body.style.color = "#1c1c1a";
            newwindow.document.body.style.fontFamily = "Calibri";
            return newwindow;
        }
    </script>
    <asp:MultiView ID="mvMealPlanner" runat="server" ActiveViewIndex="0">
        <asp:View ID="View1" runat="server">
            <p>
                Number of meals:&nbsp;
        <asp:DropDownList ID="lstNbrofMeals" runat="server">
            <asp:ListItem Text="3" Selected="True" />
            <asp:ListItem Text="5" />
            <asp:ListItem Text="6" />
        </asp:DropDownList>
            </p>
            <asp:RadioButtonList ID="radioUseCustomMeals" runat="server">
                <asp:ListItem Text="Use custom meals only" Value="0" Selected="True" />
                <asp:ListItem Text="Use custom and preset meals" Value="1" Selected="False" />
            </asp:RadioButtonList>
            <p>
                Cuisine:
        <asp:CheckBoxList ID="chklistCuisine" runat="server" RepeatColumns="4">
            <asp:ListItem Text="American" Selected="True" />
            <asp:ListItem Text="Mexican" Selected="True" />
            <asp:ListItem Text="Chinese" Selected="True" />
            <asp:ListItem Text="Italian" Selected="True" />
            <asp:ListItem Text="Indian" Selected="True" />
            <asp:ListItem Text="Japanese" Selected="True" />
            <asp:ListItem Text="Uncategorized" Selected="True" />
        </asp:CheckBoxList>
                <input id="btnSelectAll" type="button" value="Select All" onclick="CheckAll(true)" style="width: 80px" />
                <input id="btnSelectNone" type="button" value="Select None" onclick="CheckAll(false)" style="width: 80px" />
            </p>
            <p>
                How long?&nbsp;
        <asp:DropDownList ID="lstDuration" runat="server">
            <asp:ListItem Text="1 week" Value="1" Selected="True" />
            <asp:ListItem Text="2 weeks" Value="2" />
            <asp:ListItem Text="4 weeks" Value="4" />
        </asp:DropDownList>
            </p>
            <asp:Button ID="btnGo" runat="server" Text="Go!" OnClick="btnGo_Click" />
            <br />
            <asp:Label ForeColor="Red" runat="server" ID="lblError" />
        </asp:View>
        <asp:View ID="View2" runat="server">
            <asp:Panel ID="pnlMealPlanList" CssClass="Rounded" runat="server" Height="250px" ScrollBars="Auto" />
            <hr style="height: 2px; margin-bottom: 2px; margin-top: 1px; padding-top: 1px; padding-bottom: 2px; " />
            <asp:Panel ID="pnlMealPlanButtons" CssClass="Rounded" runat="server" Height="85px">
                <p>
                    <asp:Button ID="btnPrintable" runat="server" Text="Printable version" OnClientClick="fillPrintLabel(); return false" />
                    <asp:Button ID="btnGroceries" runat="server" Text="Grocery list" PostBackUrl="~/MealPlanner.aspx" OnClick="btnGroceries_Click" />
                </p>
                <p>
                    <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" />
                </p>
            </asp:Panel>
        </asp:View>
    </asp:MultiView>
    <asp:Label ID="lblPrintOutput" Visible="true" runat="server" Text="" />
</asp:Content>
