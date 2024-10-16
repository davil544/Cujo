<%@ Page Title="Vault" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CujoPasswordManager.Default" %>
<asp:Content ID="HeadContent" ContentPlaceHolderID="head" runat="server">
    <style>
        /*May not be necessary, test before deleting
        * {
          box-sizing: border-box;
        }*/

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
    <!-- <br /><br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;TODO:<br />
    1)  Build the login page and vault pages<br />
    2)  Configure SQL database or some other way to store data<br />
    3)  Create functions to login, read and write vault data<br />
    4)  Make a way to encrypt and decrypt the data at rest<br /> -->
</asp:Content>