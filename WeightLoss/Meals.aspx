<%@ Page MaintainScrollPositionOnPostback="true" Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Meals.aspx.cs" Inherits="WeightLoss.Admin.Meals" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentMain" runat="server">
    <asp:Panel ID="pnlContainer" runat="server">
        <div style="font-size: x-small">
            <table style="width: 100%">
                <tr style="vertical-align: top; width: 100%;">
                    <td>
                        <asp:GridView ShowHeaderWhenEmpty="true" HorizontalAlign="Left" ID="gridMeals" DataKeyNames="MealId" runat="server" AutoGenerateColumns="False" DataSourceID="dataSourceMeals" AutoGenerateSelectButton="True" BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black" GridLines="Vertical">
                            <AlternatingRowStyle BackColor="#CCCCCC" />
                            <EmptyDataTemplate>
                                <p>Add some meals!</p>
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMealId" runat="server" Text='<%# Bind("MealId") %>' />
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
                            <SelectedRowStyle BackColor="#000099" Font-Bold="False" ForeColor="White" />
                            <SortedAscendingCellStyle BackColor="#F1F1F1" />
                            <SortedAscendingHeaderStyle BackColor="#808080" />
                            <SortedDescendingCellStyle BackColor="#CAC9C9" />
                            <SortedDescendingHeaderStyle BackColor="#383838" />
                        </asp:GridView>
                    </td>
                    <td>
                        <asp:DetailsView OnDataBound="detailsMeal_DataBound" ID="detailsMeal" runat="server" Height="50px"
                            AutoGenerateRows="False" DataKeyNames="MealId" DataSourceID="dataSourceMealDetails"
                            OnItemDeleted="Update" OnItemInserted="Update" OnItemUpdated="Update" HeaderText="Meal details"
                            BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3"
                            ForeColor="Black" GridLines="Vertical" Width="250px" OnItemCommand="detailsMeal_ItemCommand"
                            HorizontalAlign="Right" FieldHeaderStyle-Width="60px">
                            <AlternatingRowStyle BackColor="#CCCCCC" />
                            <EditRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                            <Fields>
                                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                                <asp:BoundField DataField="UserId" HeaderText="UserId" SortExpression="UserId" />
                                <asp:BoundField DataField="Cuisine" HeaderText="Cuisine" SortExpression="Cuisine" />
                                <asp:CheckBoxField DataField="Breakfast" HeaderText="Breakfast" SortExpression="Breakfast" />
                                <asp:CheckBoxField DataField="Lunch" HeaderText="Lunch" SortExpression="Lunch" />
                                <asp:CheckBoxField DataField="Dinner" HeaderText="Dinner" SortExpression="Dinner" />
                                <asp:CheckBoxField DataField="Snack" HeaderText="Snack" SortExpression="Snack" />
                                <asp:BoundField DataField="Rating" HeaderText="Rating" SortExpression="Rating" />
                                <asp:CommandField CausesValidation="true" ShowEditButton="true" ShowInsertButton="true" ShowDeleteButton="true" NewText="Add meal" InsertText="Add" />
                            </Fields>
                            <EmptyDataTemplate>
                                You don't have any meals!<br />
                                <asp:LinkButton ForeColor="Black" Text="Add meal" ID="InsertButton" runat="server" CommandName="New" />
                            </EmptyDataTemplate>
                            <FooterStyle BackColor="#CCCCCC" />
                            <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                        </asp:DetailsView>
                    </td>
                </tr>
            </table>
        </div>
        <div style="font-size: x-small; background: #333333" class="Rounded">
            <table style="width: 100%">
                <tr style="width: 100%; vertical-align: top;">
                    <td>
                        <asp:GridView OnPageIndexChanged="gridIngredients_In_Meals_PageIndexChanged" HorizontalAlign="Left" ID="gridIngredients_In_Meals" DataKeyNames="InstanceId" runat="server" AutoGenerateColumns="False" DataSourceID="dataSourceIngredients_In_Meals"
                            BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black" GridLines="Vertical" AllowPaging="true" PageSize="5" ShowHeaderWhenEmpty="true">
                            <EmptyDataTemplate>
                                <p>Add some ingredients!</p>
                            </EmptyDataTemplate>
                            <AlternatingRowStyle BackColor="#CCCCCC" />
                            <Columns>
                                <asp:TemplateField HeaderText="Ingredient">
                                    <ItemTemplate>
                                        <%# GetIngredientFromId((int)Eval("IngredientId")) %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Quantity" HeaderText="Quantity" SortExpression="Quantity" />
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
                        <asp:DetailsView OnItemCommand="detailsIngredients_In_Meals_ItemCommand" OnDataBound="detailsIngredients_In_Meals_DataBound" ID="detailsIngredients_In_Meals" runat="server" Height="50px"
                            AutoGenerateRows="False" DataKeyNames="InstanceId" DataSourceID="dataSourceIngredients_In_Meals"
                            OnItemDeleted="Update" OnItemInserted="Update" OnItemUpdated="Update" HeaderText="Ingredients editor"
                            BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3"
                            ForeColor="Black" GridLines="Vertical" Width="250px" HorizontalAlign="Right"
                            AllowPaging="true" OnPageIndexChanged="detailsIngredients_In_Meals_PageIndexChanged"
                            FieldHeaderStyle-Width="60px">
                            <AlternatingRowStyle BackColor="#CCCCCC" />
                            <EditRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                            <Fields>

                                <asp:TemplateField HeaderText="Ingredient">
                                    <ItemTemplate>
                                        <asp:Label ID="lblIngredient_In_Details" runat="server" Text='<%# GetIngredientFromId((int)Eval("IngredientId")) %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList DataSourceID="dataSourceIngredients"
                                            DataTextField="Name"
                                            DataValueField="IngredientId"
                                            SelectedValue='<%# Bind("IngredientId") %>'
                                            ID="lstIngredients_In_Details_Edit" runat="server" />
                                    </EditItemTemplate>
                                    <InsertItemTemplate>
                                        <asp:DropDownList DataSourceID="dataSourceIngredients"
                                            DataTextField="Name"
                                            DataValueField="IngredientId"
                                            SelectedValue='<%# Bind("IngredientId") %>'
                                            ID="lstIngredients_In_Details_New" runat="server" />
                                    </InsertItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Quantity" SortExpression="Quantity">
                                    <ItemTemplate>
                                        <asp:Label ID="lblQuantity" runat="server" Text='<%# Eval("Quantity") %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox Text='<%# Bind("Quantity") %>'
                                            ID="txtQuantity_Edit" runat="server" TextMode="Number" />
                                        <asp:RangeValidator ID="rngValidator_Edit" MinimumValue="1" MaximumValue="16000" ControlToValidate="txtQuantity_Edit" runat="server" Type="Integer" ErrorMessage="Invalid quantity" ValidationGroup="Ingredients_In_Meals_Validation" Display="None" />
                                    </EditItemTemplate>
                                    <InsertItemTemplate>
                                        <asp:TextBox Text='<%# Bind("Quantity") %>'
                                            ID="txtQuantity_New" runat="server" TextMode="Number" />
                                        <asp:RangeValidator ID="rngValidator_New" MinimumValue="1" MaximumValue="16000" ControlToValidate="txtQuantity_New" runat="server" Type="Integer" ErrorMessage="Invalid quantity" ValidationGroup="Ingredients_In_Meals_Validation" Display="None" />
                                    </InsertItemTemplate>
                                </asp:TemplateField>

                                <asp:CommandField CausesValidation="true" ShowEditButton="true" ShowInsertButton="true" ShowDeleteButton="true" NewText="Add ingredient" InsertText="Add" />
                            </Fields>
                            <EmptyDataTemplate>
                                This meal has no ingredients!<br />
                                <asp:LinkButton ForeColor="Black" Text="Add ingredients" ID="InsertButton" runat="server" CommandName="New" />
                            </EmptyDataTemplate>
                            <FooterStyle BackColor="#CCCCCC" />
                            <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                        </asp:DetailsView>
                    </td>

                </tr>
            </table>
            <br />
            <asp:Label ID="lblError" runat="server" ForeColor="Red" />
            <asp:ValidationSummary ForeColor="Red" ID="vldErrors" ValidationGroup="Ingredients_In_Meals_Validation" runat="server" />
            <asp:EntityDataSource ID="dataSourceMeals"
                runat="server"
                ConnectionString="name=foodEntities"
                DefaultContainerName="foodEntities"
                EnableFlattening="False"
                EntitySetName="Meals"
                AutoGenerateWhereClause="true">
            </asp:EntityDataSource>
            <asp:EntityDataSource ID="dataSourceMealDetails"
                runat="server"
                ConnectionString="name=foodEntities"
                DefaultContainerName="foodEntities"
                Select=""
                Where="it.MealId == @MealId"
                EnableDelete="True" 
                EnableFlattening="False" 
                EnableInsert="True" 
                EnableUpdate="True"
                EntitySetName="Meals"
                OnInserting="dataSourceMealDetails_Inserting"
                OnUpdating="dataSourceMealDetails_Inserting"
                OnDeleting="dataSourceMealDetails_Deleting">
                <WhereParameters>
                    <asp:ControlParameter ControlID="gridMeals" Name="MealId" Type="Int32" PropertyName="SelectedValue" />
                </WhereParameters>
            </asp:EntityDataSource>
            <asp:EntityDataSource ID="dataSourceIngredients"
                runat="server"
                ConnectionString="name=foodEntities"
                DefaultContainerName="foodEntities"
                EnableFlattening="False"
                EntitySetName="Ingredients">
            </asp:EntityDataSource>
            <asp:EntityDataSource AutoGenerateWhereClause="true"
                ID="dataSourceIngredients_In_Meals"
                runat="server"
                ConnectionString="name=foodEntities"
                DefaultContainerName="foodEntities"
                OnInserting="dataSourceIngredients_In_Meals_Inserting"
                EnableFlattening="False"
                EntitySetName="Ingredients_In_Meals"
                EnableDelete="True"
                EnableInsert="True"
                EnableUpdate="True">
                <WhereParameters>
                    <asp:ControlParameter ControlID="gridMeals" Name="MealId" Type="Int32" DefaultValue="0" />
                </WhereParameters>
            </asp:EntityDataSource>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlVisitor" runat="server" Visible="false">
        <asp:Label Text="Hello,<br>While you do have to sign up to use the meal editor, you can freely use the " runat="server" />
        <asp:HyperLink runat="server" Text="meal planner!" NavigateUrl="~/MealPlanner.aspx" />
    </asp:Panel>
</asp:Content>
