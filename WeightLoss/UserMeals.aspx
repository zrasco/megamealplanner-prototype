<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="UserMeals.aspx.cs" Inherits="WeightLoss.UserMeals" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentMain" runat="server">
    <div style="font-size : x-small">
        <asp:GridView ID="gridMeals" runat="server" AutoGenerateColumns="False" DataKeyNames="MealId" DataSourceID="dataSourceUserMeals" BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black" GridLines="Vertical">
            <AlternatingRowStyle BackColor="#CCCCCC" />
            <Columns>
                <asp:TemplateField Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblMealId" Text="<% Bind('MealId') %>" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                <asp:BoundField DataField="UserId" HeaderText="UserId" SortExpression="UserId" />
                <asp:BoundField DataField="Cuisine" HeaderText="Cuisine" SortExpression="Cuisine" />
                <asp:CheckBoxField DataField="Breakfast" HeaderText="Breakfast" SortExpression="Breakfast" />
                <asp:CheckBoxField DataField="Lunch" HeaderText="Lunch" SortExpression="Lunch" />
                <asp:CheckBoxField DataField="Dinner" HeaderText="Dinner" SortExpression="Dinner" />
                <asp:CheckBoxField DataField="Snack" HeaderText="Snack" SortExpression="Snack" />
                <asp:BoundField DataField="Rating" HeaderText="Rating" SortExpression="Rating" />
            </Columns>
            <FooterStyle BackColor="#CCCCCC" />
            <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
            <SortedAscendingCellStyle BackColor="#F1F1F1" />
            <SortedAscendingHeaderStyle BackColor="#808080" />
            <SortedDescendingCellStyle BackColor="#CAC9C9" />
            <SortedDescendingHeaderStyle BackColor="#383838" />
        </asp:GridView>
    </div>
    <asp:EntityDataSource ID="dataSourceUserMeals"
        runat="server"
        ConnectionString="name=foodEntities"
        DefaultContainerName="foodEntities"
        EnableDelete="True"
        EnableFlattening="False"
        EnableInsert="True"
        EnableUpdate="True"
        EntitySetName="Meals"
        Where="(it.UserId = cast(@UserId as System.Int32)) or (it.UserId IS NULL)"
        EntityTypeFilter="Meal">
        <WhereParameters>
            <asp:Parameter Name="UserId" Type="Int32" />
        </WhereParameters>
    </asp:EntityDataSource>
</asp:Content>
