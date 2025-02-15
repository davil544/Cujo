<%@ Page Title="Vault" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CujoPasswordManager.Default" %>
<asp:Content ID="HeadContent" ContentPlaceHolderID="head" runat="server">
    <style>
        /* TODO: Move this into a CSS file so it can be used by the Search Results page */

        .column-left {
          float: left;
          width: 30%;
          padding: 5px;
        }

        .column-right {
          float: right;
          width: 70%;
          padding: 5px;
}

        /* Clearfix (clear floats) */
        .row::after {
          content: "";
          clear: both;
          display: table;
        }

        .table {
          padding: 150px;
          width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="body" runat="server">
    <p></p><!-- Main page headers go here -->
    <asp:Button CssClass="btn btn-success my-2 my-sm-0" Text="Add Entry" runat="server" ID="btnAddEntry" OnClick="BtnAddEntry_Click" CausesValidation="false" />
    <!-- <div class="column-left">
        Menu
    </div>
    <div class="column-right">
        Vault Data 
    </div> -->
    <br /><hr />

    <!-- User Data and Menu Items go here -->
    <div class="column-left">
        <table class="table">
            <tr style="border-bottom: 1px solid #000000;">
                <td>Categories</td>
            </tr>
            <tr><td>All Items</td></tr>
        </table>
    </div>
    <div class="column-right">
        <table class="table">
            <tr style="border-bottom: 1px solid #000000;">
                <td>Item Name</td>
                <td>Username</td>
                <td>URL</td>
            </tr>
            <asp:Panel ID="pnlVaultContents" runat="server"></asp:Panel>
            <!-- <tr>
                <td>No Items Found</td>
                <td>SampleUser</td>
                <td id="testURL" runat="server"> </td>
            </tr> -->
        </table>
    </div> <br /><br /><br />
</asp:Content>