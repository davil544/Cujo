<%@ Page Title="Vault Search Results" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SearchResults.aspx.cs" Inherits="CujoPasswordManager.SearchResults" %>
<asp:Content ID="HeadContent" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="body" runat="server">
    <h2 id="heading2" runat="server">Password Entry Search Results for:&nbsp<asp:Literal ID="litSearchQuery" runat="server"></asp:Literal></h2>
    <asp:Literal ID="litResults" runat="server"></asp:Literal>
</asp:Content>
