<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="ForgotPassword" Codebehind="ForgotPassword.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentMain" Runat="Server">
    <asp:Panel ID="pnlContainer" runat="server">
        <asp:PasswordRecovery OnVerifyingUser="PasswordRecovery1_VerifyingUser"
                                OnUserLookupError="PasswordRecovery1_UserLookupError" 
                                OnAnswerLookupError="PasswordRecovery1_AnswerLookupError"
                                OnSendingMail="PasswordRecovery1_SendingMail"
                                ID="PasswordRecovery1" runat="server"></asp:PasswordRecovery>
    </asp:Panel>
</asp:Content>

