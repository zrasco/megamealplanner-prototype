<%@ Page Trace="true" Language="C#" AutoEventWireup="true" CodeBehind="MP_Groceries.aspx.cs" Inherits="WeightLoss.MP_Groceries" %>
<%@ PreviousPageType VirtualPath="~/MealPlanner.aspx"  %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Printable meal plan</title>
    <link href="StyleSheet.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:Literal ID="lblContent" Text="<p>Please use meal planner page to generate printable version.</p>" runat="server"/>
    </div>
    </form>
</body>
</html>
