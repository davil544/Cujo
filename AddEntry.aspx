<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddEntry.aspx.cs" Inherits="CujoPasswordManager.AddEntry" %>
<asp:Content ID="HeadContent" ContentPlaceHolderID="head" runat="server">
    <style>
        .pwForm {
            margin: 10px;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="body" runat="server">
    <p></p>New Password Entry:
    <div align="center">
        <asp:TextBox ID="txtItemName" runat="server" CssClass="form-control pwForm" placeholder="Item name"></asp:TextBox>
        <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control pwForm" placeholder="Username"></asp:TextBox>
        <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control pwForm" placeholder="Item Password" TextMode="Password"></asp:TextBox>
        <asp:TextBox ID="txtURL" runat="server" CssClass="form-control pwForm" placeholder="URL" TextMode="Url"></asp:TextBox>
        <asp:TextBox ID="txtCategory" runat="server" CssClass="form-control pwForm" placeholder="Category"></asp:TextBox>
        <asp:TextBox ID="txtNotes" runat="server" CssClass="form-control pwForm" placeholder="Notes" TextMode="MultiLine"></asp:TextBox>
        <br />
        <asp:Button ID="btnSubmit" runat="server" CssClass="" Text="Submit" OnClick="BtnAdd_Click"></asp:Button>
        <asp:Button ID="btnCancel" runat="server" CssClass="" Text="Cancel"></asp:Button>
    </div>
</asp:Content>
