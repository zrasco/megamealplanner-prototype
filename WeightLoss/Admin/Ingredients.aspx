﻿<%@ Page MaintainScrollPositionOnPostback="true" Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="Admin_Ingredients" Codebehind="Ingredients.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentMain" runat="Server">
    <div style="font-size: x-small">
        <table style="width: 100%">
            <tr style="vertical-align: top;">
                <td>
        <asp:GridView HorizontalAlign="Left" ID="gridIngredients" DataKeyNames="IngredientId" runat="server" AutoGenerateColumns="False" DataSourceID="dataSourceIngredients" AutoGenerateSelectButton="True" BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black" GridLines="Vertical">
            <AlternatingRowStyle BackColor="#CCCCCC" />
            <Columns>
                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                <asp:BoundField DataField="Quantity_Nbr" HeaderStyle-Width="50px" HeaderText="Quantity" SortExpression="Quantity_Nbr" />
                <asp:BoundField DataField="Quantity_Measurement" HeaderStyle-Width="50px" HeaderText="Measurement" SortExpression="Quantity_Measurement" />
                <asp:BoundField DataField="Category" HeaderText="Category" SortExpression="Category" />
                <asp:BoundField DataField="Calories" HeaderText="Calories" SortExpression="Calories" />
            </Columns>
            <FooterStyle BackColor="#CCCCCC" />
            <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#000099" Font-Bold="False" ForeColor="White" />
            <SortedAscendingCellStyle BackColor="#F1F1F1" />
            <SortedAscendingHeaderStyle BackColor="#808080" />
            <SortedDescendingCellStyle BackColor="#CAC9C9" />
            <SortedDescendingHeaderStyle BackColor="#383838" />
        </asp:GridView>
                </td>
                <td>
        <asp:DetailsView OnDataBound="detailsIngredients_DataBound" ID="detailsIngredients" runat="server" Height="50px"
            AutoGenerateRows="False" DataKeyNames="IngredientId" DataSourceID="dataSourceIngredientDetails"
            OnItemDeleted="Update" OnItemInserted="Update" OnItemUpdated="Update" HeaderText="Ingredient details"
            AutoGenerateDeleteButton="True" AutoGenerateEditButton="True" AutoGenerateInsertButton="True" 
            BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" 
            ForeColor="Black" GridLines="Vertical" Width="250px">
            <AlternatingRowStyle BackColor="#CCCCCC" />
            <EditRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
            <Fields>
                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                <asp:BoundField DataField="Quantity_Nbr" HeaderText="Quantity" SortExpression="Quantity_Nbr" />
                <asp:BoundField DataField="Quantity_Measurement" HeaderText="Measurement" SortExpression="Quantity_Measurement" />
                <asp:BoundField DataField="Category" HeaderText="Category" SortExpression="Category" />
                <asp:BoundField DataField="Calories" HeaderText="Calories" SortExpression="Calories" />
            </Fields>
            <FooterStyle BackColor="#CCCCCC" />
            <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
        </asp:DetailsView>
                </td>
            </tr>
        </table>

        <asp:EntityDataSource ID="dataSourceIngredients" runat="server" ConnectionString="name=foodEntities" DefaultContainerName="foodEntities" EnableFlattening="False" EntitySetName="Ingredients"></asp:EntityDataSource>
        <asp:EntityDataSource ID="dataSourceIngredientDetails" runat="server"
            ConnectionString="name=foodEntities"
            DefaultContainerName="foodEntities"
            Select=""
            Where="it.IngredientId == @IngredientId"
            EnableDelete="True" EnableFlattening="False" EnableInsert="True" EnableUpdate="True"
            EntitySetName="Ingredients">
            <WhereParameters>
                <asp:ControlParameter ControlID="gridIngredients" Name="IngredientId" Type="Int32" PropertyName="SelectedValue" />
            </WhereParameters>
        </asp:EntityDataSource>
    </div>
</asp:Content>

