<%@ Page Title="Entry Details" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EntryDetails.aspx.cs" Inherits="CujoPasswordManager.EntryDetails" %>
<asp:Content ID="HeadContent" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="body" runat="server">
    <div align="center" style="padding-top: 10px;">
        <table>
            <tr>
                <td><h3>Item Name:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</h3></td>
                <td><h5><asp:Label ID="lblName" runat="server"></asp:Label></h5></td>
            </tr>
            <tr>
                <td><h3>URL:</h3></td>
                <td><h5><asp:Label ID="lblURL" runat="server"></asp:Label></h5></td>
            </tr>
            <tr>
                <td><h3>Username:</h3></td>
                <td><h5><asp:Label ID="lblUser" runat="server"></asp:Label></h5></td>
            </tr>
            <tr>
                <td><h3>Password</h3></td>
                <td><h5><asp:LinkButton ID="lnkPassword" runat="server" OnClick="LnkPassword_Click"><i>Show Password</i></asp:LinkButton></h5></td>
            </tr>
            <tr>
                <td><h3>Category:</h3></td>
                <td><h5><asp:Label ID="lblCat" runat="server"></asp:Label></h5></td>
            </tr>
            <tr>
                <td><h3>Notes:</h3></td>
                <td><h5><asp:Label ID="lblNotes" runat="server"></asp:Label></h5></td>
            </tr>
        </table>
        <br /><asp:Button ID="btnEdit" runat="server" Text="Edit Entry" OnClick="BtnEdit_Click" />
        &nbsp;<asp:Button ID="btnDelete" runat="server" Text="Delete Entry" Enabled="false" /> <!-- This feature will be implemented in a future update! -->
    </div>
</asp:Content>