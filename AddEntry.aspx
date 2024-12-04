<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddEntry.aspx.cs" Inherits="CujoPasswordManager.AddEntry" %>
<asp:Content ID="HeadContent" ContentPlaceHolderID="head" runat="server">
    <style>
        .pwForm {
            margin: 10px;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="body" runat="server">
    <p></p>
    <div align="center">
        <asp:Panel ID="pnlAdd" runat="server">
            New Password Entry:
            <asp:TextBox ID="txtItemName" runat="server" CssClass="form-control pwForm" placeholder="Item name"></asp:TextBox>
            
            <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control pwForm" placeholder="Username"></asp:TextBox>
            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control pwForm" placeholder="Item Password" TextMode="Password"></asp:TextBox>
            <asp:TextBox ID="txtURL" runat="server" CssClass="form-control pwForm" placeholder="URL" TextMode="Url"></asp:TextBox>
            <asp:TextBox ID="txtCategory" runat="server" CssClass="form-control pwForm" placeholder="Category"></asp:TextBox>
            <asp:TextBox ID="txtNotes" runat="server" CssClass="form-control pwForm" placeholder="Notes" TextMode="MultiLine"></asp:TextBox>
        </asp:Panel>

        <asp:Panel ID="pnlUpdate" runat="server" Visible="false">
            Update Password Entry:
            <asp:TextBox ID="txtEditItemName" runat="server" CssClass="form-control pwForm" placeholder="Item name"></asp:TextBox>
            <asp:TextBox ID="txtEditUserName" runat="server" CssClass="form-control pwForm" placeholder="Username"></asp:TextBox>
            <asp:TextBox ID="txtEditPassword" runat="server" CssClass="form-control pwForm" placeholder="Item Password (Unchanged if left empty)" TextMode="Password"></asp:TextBox>
            <asp:TextBox ID="txtEditURL" runat="server" CssClass="form-control pwForm" placeholder="URL" TextMode="Url"></asp:TextBox>
            <asp:TextBox ID="txtEditCategory" runat="server" CssClass="form-control pwForm" placeholder="Category"></asp:TextBox>
            <asp:TextBox ID="txtEditNotes" runat="server" CssClass="form-control pwForm" placeholder="Notes" TextMode="MultiLine"></asp:TextBox>
        </asp:Panel>

        <br />
        <asp:Button ID="btnAdd" runat="server" Text="Add Entry" OnClick="BtnAdd_Click"></asp:Button>
        <asp:Button ID="btnUpdate" runat="server" Text="Update Entry" OnClick="BtnUpdate_Click" Visible="false"></asp:Button>
        <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="BtnCancel_Click"></asp:Button>
    </div>
</asp:Content>