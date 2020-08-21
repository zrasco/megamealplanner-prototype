<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="ChangePassword" Codebehind="ChangePassword.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentMain" Runat="Server">
    <asp:Panel runat="server" id="pnlContent">
        <asp:ChangePassword ID="ChangePassword1" runat="server"
            OnChangedPassword="ChangePassword1_ChangedPassword"
            OnChangePasswordError="ChangePassword1_ChangePasswordError" 
            OnContinueButtonClick="ChangePassword1_ContinueButtonClick" CancelButtonType="Link" CancelButtonText="">
        </asp:ChangePassword>
    </asp:Panel>
</asp:Content>

