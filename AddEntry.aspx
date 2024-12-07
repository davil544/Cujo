<%@ Page Title="Entry Editor" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddEntry.aspx.cs" Inherits="CujoPasswordManager.AddEntry" %>
<asp:Content ID="HeadContent" ContentPlaceHolderID="head" runat="server">
    <style>
        .pwForm {
            margin: 10px;
        }
    </style>
    <script>
        // This function allows only numbers to be enterred in a TextBox
        function onlyNumberKey(evt) {
            let ASCIICode = (evt.which) ? evt.which : evt.keyCode
            if (ASCIICode > 31 && (ASCIICode < 48 || ASCIICode > 57))
                return false;
            return true;
        }
    </script>
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
        <asp:Button ID="btnShowPass" runat="server" Text="Show Password" OnClick="BtnShowPass_CLick" />
        <br /><br />
        Generate a password:<br />

        <table class="pwForm">
            <tr>
                <td>Password Length:</td>
                <td><asp:TextBox ID="txtPassLength" runat="server" type="number" min="0" max="99" value="24" onkeypress="return onlyNumberKey(event)" ></asp:TextBox></td>
            </tr>
            <tr>
                <td>Included Characters:&nbsp;&nbsp;&nbsp;&nbsp;</td>
                <td><asp:CheckBoxList ID="cblSupportedChars" runat="server"></asp:CheckBoxList></td>
            </tr>
        </table>
        <asp:Button ID="btnGenPass" runat="server" Text="Generate Password" OnClick="BtnGenPass_Click" />
        <br /><br /><br /><br />
    </div>
</asp:Content>