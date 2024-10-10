<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="CujoPasswordManager.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Login - Cujo Password Manager</title>
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <style>
        /* Begin code for Bootstrap's sign-in.css */
        html,
        body {
          height: 100%;
        }

        .form-signin {
          max-width: 330px;
          padding: 1rem;
        }

        .form-signin .form-floating:focus-within {
          z-index: 2;
        }

        .form-signin input[type="email"] {
          margin-bottom: -1px;
          border-bottom-right-radius: 0;
          border-bottom-left-radius: 0;
        }

        .form-signin input[type="password"] {
          margin-bottom: 10px;
          border-top-left-radius: 0;
          border-top-right-radius: 0;
        }
        /* End code for Bootstrap's sign-in.css */

        .bd-placeholder-img {
          font-size: 1.125rem;
          text-anchor: middle;
          -webkit-user-select: none;
          -moz-user-select: none;
          user-select: none;
        }

        @media (min-width: 768px) {
          .bd-placeholder-img-lg {
            font-size: 3.5rem;
          }
        }

        .b-example-divider {
          width: 100%;
          height: 3rem;
          background-color: rgba(0, 0, 0, .1);
          border: solid rgba(0, 0, 0, .15);
          border-width: 1px 0;
          box-shadow: inset 0 .5em 1.5em rgba(0, 0, 0, .1), inset 0 .125em .5em rgba(0, 0, 0, .15);
        }

        .b-example-vr {
          flex-shrink: 0;
          width: 1.5rem;
          height: 100vh;
        }

        .bi {
          vertical-align: -.125em;
          fill: currentColor;
        }

        .nav-scroller {
          position: relative;
          z-index: 2;
          height: 2.75rem;
          overflow-y: hidden;
        }

        .nav-scroller .nav {
          display: flex;
          flex-wrap: nowrap;
          padding-bottom: 1rem;
          margin-top: -1px;
          overflow-x: auto;
          text-align: center;
          white-space: nowrap;
          -webkit-overflow-scrolling: touch;
        }

        .btn-bd-primary {
          --bd-violet-bg: #712cf9;
          --bd-violet-rgb: 112.520718, 44.062154, 249.437846;

          --bs-btn-font-weight: 600;
          --bs-btn-color: var(--bs-white);
          --bs-btn-bg: var(--bd-violet-bg);
          --bs-btn-border-color: var(--bd-violet-bg);
          --bs-btn-hover-color: var(--bs-white);
          --bs-btn-hover-bg: #6528e0;
          --bs-btn-hover-border-color: #6528e0;
          --bs-btn-focus-shadow-rgb: var(--bd-violet-rgb);
          --bs-btn-active-color: var(--bs-btn-hover-color);
          --bs-btn-active-bg: #5a23c8;
          --bs-btn-active-border-color: #5a23c8;
        }

        .bd-mode-toggle {
          z-index: 1500;
        }

        .bd-mode-toggle .dropdown-menu .active .bi {
          display: block !important;
        }
    </style>
</head>
<body class="d-flex text-center py-4 bg-body-tertiary">
    <main class="form-signin w-100 m-auto">
        <form id="usersForm" runat="server">
            <!-- TODO: Replace this with a photo of a guard dog eventually -->
            <img class="mb-4" src="https://getbootstrap.com/docs/5.0/assets/brand/bootstrap-logo.svg" alt="" width="72" height="57" />

            <asp:Panel ID="pnlLogin" runat="server">
                <h1 class="h3 mb-3 fw-normal">Please sign in</h1>

                <div class="form-floating">
                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="Username"></asp:TextBox><p></p>
                    <label for="txtUsername">Username</label>
                </div>
                <div class="form-floating">
                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Password"></asp:TextBox>
                    <label for="txtPassword">Password</label>
                </div>

                <div class="form-check text-start my-3">
                    <input class="form-check-input" type="checkbox" value="remember-me" id="chkRememberMe" disabled />
                    <label class="form-check-label" for="chkRememberMe">Remember me</label>
                </div>
                <asp:Button ID="btnlogin" CssClass="btn btn-primary w-100 py-2" runat="server" OnClick="BtnLogin_Click" Text="Sign In"/>
            </asp:Panel>

            <asp:Panel ID="pnlRegistration" runat="server" Visible="false">
                <h1 class="h3 mb-3 fw-normal">Please register for an account here</h1>

                <div class="form-floating">
                    <asp:TextBox ID="txtUsernameReg" runat="server" CssClass="form-control" placeholder="Username"></asp:TextBox><p></p>
                    <label for="txtUsernameReg">Username</label>
                </div>
                <div class="form-floating">
                    <asp:TextBox ID="txtPasswordReg" runat="server" CssClass="form-control" TextMode="Password" placeholder="Password"></asp:TextBox>
                    <label for="txtPasswordReg">Password</label>
                </div>
                <asp:Button ID="btnRegister" CssClass="btn btn-primary w-100 py-2" runat="server" OnClick="BtnRegister_Click" Text="Sign Up"/>
            </asp:Panel>

            <p style="padding-top: 5px;"><asp:Button ID="btnToggleForm" CssClass="btn btn-secondary w-100 py-2" runat="server" OnClick="BtnFormToggle_Click" Text="Create a new account"/></p>

            <p class="mt-5 mb-3 text-body-secondary">&copy; 2024 Dylan Aviles, All Rights Reserved</p>
        </form>
    </main>
    <script src="Scripts/bootstrap.bundle.min.js"></script> <!-- Maybe comment this out if not needed? -->
</body>
</html>