<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="_Secure_Login" Async="true"%>
<%@ Import Namespace="W88.BusinessLogic.Rewards.Helpers" %>
<%@ Import Namespace="W88.BusinessLogic.Rewards.Models" %>
<%@ Import Namespace="W88.BusinessLogic.Shared.Helpers" %>

<!DOCTYPE html>
<html>
<head>
    <title><%=RewardsHelper.GetTranslation(TranslationKeys.Label.Brand)%></title>
    <!--#include virtual="~/_static/head.inc" -->
    <script src="/_Static/JS/modules/login.js"></script>
</head>
<body>
    <div data-role="page" data-theme="b">
        <!--#include virtual="~/_static/header.shtml" -->
        <form id="form1" runat="server">
            <div role="main" class="main-content has-footer ui-content">
                <div class="container">
                    <div class="form-container login">
                        <div class="form-group form-group-line">
                            <asp:Label ID="lblUsername" runat="server" AssociatedControlID="txtUsername" Text="username" />
                            <asp:TextBox ID="txtUsername" runat="server" data-corners="false" autofocus="on" MaxLength="16" CssClass="form-control" />
                        </div>
                        <div class="form-group form-group-line">
                            <asp:Label ID="lblPassword" runat="server" AssociatedControlID="txtPassword" Text="password" />
                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" data-corners="false" MaxLength="10" CssClass="form-control" />
                        </div>
                        <div id="captchaDiv" class="form-group form-group-line" style="display: none;">
                            <asp:Label ID="lblCaptcha" runat="server" AssociatedControlID="txtCaptcha" Text="code" />
                            <asp:TextBox ID="txtCaptcha" runat="server" MaxLength="4" CssClass="form-control"/>
                            <asp:Image ID="imgCaptcha" runat="server" CssClass="imgCaptcha" />
                        </div>
                        <div class="text-center no-account">
                            <asp:Literal ID="lblRegister" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="footer">
                <asp:Button ID="btnSubmit" runat="server" Text="login" CssClass="btn btn-block btn-primary" data-corners="false" />
            </div>
        </form>
    </div>

    <script type="text/javascript">
        $(function () {
            var elems = {
                submitButton: $('#btnSubmit'),
                captchaDiv: $('#captchaDiv'),
                username: $('#txtUsername'),
                password: $('#txtPassword'),
                captcha: $('#<%=txtCaptcha.ClientID%>'),
                captchaImg: $('#<%=imgCaptcha.ClientID%>')
            },
            translations = {
                Exception: '<%=RewardsHelper.GetTranslation(TranslationKeys.Errors.Exception)%>',
                IncorrectVCode: '<%=RewardsHelper.GetTranslation(TranslationKeys.Errors.IncorrectVCode)%>',
                InvalidUsernamePassword: '<%=RewardsHelper.GetTranslation(TranslationKeys.Errors.InvalidUsernamePassword)%>',
                MissingPassword: '<%=RewardsHelper.GetTranslation(TranslationKeys.Errors.MissingPassword)%>',
                MissingUsername: '<%=RewardsHelper.GetTranslation(TranslationKeys.Errors.MissingUsername)%>'
            },
            login = new Login(translations, elems, '/Index.aspx');
            login.initializeButtons();

            $('#<%=imgCaptcha.ClientID%>').click(function () { $(this).attr('src', '/_Secure/Captcha.aspx'); });
            var splitHost = window.location.hostname.split('.'),
                domain = splitHost.length == 3 ? splitHost[1] + '.' + splitHost[2] : splitHost[0] + '.' + splitHost[1],
                registerUri = window.location.protocol + '//m.' + domain + '/_secure/register.aspx';
            $('#register').attr('href', registerUri);
        });
    </script>
</body>
</html>