<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="_Secure_Login" Async="true"%>
<%@ Import Namespace="W88.BusinessLogic.Shared.Helpers" %>

<!DOCTYPE html>
<html>
<head>
    <title><%=CultureHelpers.ElementValues.GetResourceString("brand", LeftMenu) + CultureHelpers.ElementValues.GetResourceString("login", LeftMenu)%></title>
    <!--#include virtual="~/_static/head.inc" -->
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
        var counter = 0;
        $(function () {
            $('#<%=imgCaptcha.ClientID%>').attr('src', '/_Secure/Captcha.aspx?t=' + new Date().getTime());

            $('#<%=btnSubmit.ClientID%>').click(function (e) {
                $('#btnSubmit').attr("disabled", true);
                if ($('#txtUsername').val().trim().length == 0) {
                    showMessage('<%=CultureHelpers.ElementValues.GetResourceXPathString("Login/MissingUsername", XeErrors)%>');
                    $('#btnSubmit').attr("disabled", false);
                    e.preventDefault();
                    return;
                }
                if (!/^[a-zA-Z0-9]+$/.test($('#txtUsername').val().trim())) {
                    showMessage('<%=CultureHelpers.ElementValues.GetResourceXPathString("Login/InvalidUsername", XeErrors)%>');
                    $('#btnSubmit').attr("disabled", false);
                    e.preventDefault();
                    return;
                }
                if ($('#txtUsername').val().trim().indexOf(' ') >= 0) {
                    showMessage('<%=CultureHelpers.ElementValues.GetResourceXPathString("Login/InvalidUsername", XeErrors)%>');
                    $('#btnSubmit').attr("disabled", false);
                    e.preventDefault();
                    return;
                }
                if ($('#txtPassword').val().trim().length == 0) {
                    showMessage('<%=CultureHelpers.ElementValues.GetResourceXPathString("Login/MissingPassword", XeErrors)%>');
                    $('#btnSubmit').attr("disabled", false);
                    e.preventDefault();
                    return;
                }
                if ($('#txtCaptcha').val().trim().length == 0 && counter >= 3) {
                    showMessage('<%=CultureHelpers.ElementValues.GetResourceString("MissingVCode", XeErrors)%>');
                    $('#btnSubmit').attr("disabled", false);
                    e.preventDefault();
                    return;
                }

                initiateLogin();
                e.preventDefault();
            });

            $('#<%=imgCaptcha.ClientID%>').click(function () { $(this).attr('src', '/_Secure/Captcha.aspx'); });
        });

        function initiateLogin() {
            $.ajax({
                type: 'POST',
                contentType: 'application/json',
                url: '/api/user/login',
                beforeSend: function () {
                    GPINTMOBILE.ShowSplash();
                },
                timeout: function () {
                    $('#<%=btnSubmit.ClientID%>').prop('disabled', false);
                    showMessage('<%=CultureHelpers.ElementValues.GetResourceString("Exception", XeErrors)%>');
                    window.location.replace('/Default.aspx');
                },
                data: JSON.stringify({ 
                    UserInfo: {
                        Username: $('#txtUsername').val(),
                        Password: $('#txtPassword').val()
                    },
                    Captcha: $('#txtCaptcha').val()
                }),
                success: function (response) {
                    if (!response || response.ResponseCode == undefined) {
                        initiateLogin();
                        return;
                    }

                    if (response.ResponseData.ResetPassword) {
                        loadPage('/Settings/ChangePassword.aspx?lang=<%=LanguageHelpers.SelectedLanguage.ToLower()%>');
                        return;
                    }

                    var message = response.ResponseMessage;
                    switch (response.ResponseCode) {
                        case 1:
                            window.user = (new User()).setProperties(response.ResponseData);
                            window.user.save();
                            if (!_.isEmpty('<%=RedirectUri%>')) {
                                loadPage('<%=RedirectUri%>');
                                return;
                            }
                            window.location.reload();
                            break;
                        case 22:
                            GPINTMOBILE.HideSplash();
                            $('#btnSubmit').attr("disabled", false);
                            showMessage(message);
                            break;
                        default:
                            counter += 1;

                            if (counter >= 3) {
                                $('#captchaDiv').show();
                                $('#<%=imgCaptcha.ClientID%>').attr('src', '/_Secure/Captcha.aspx?t=' + new Date().getTime());
                                $('#<%=txtCaptcha.ClientID%>').val('');
                                $('#<%=txtPassword.ClientID%>').val('');
                            }

                            $('#btnSubmit').attr("disabled", false);
                            GPINTMOBILE.HideSplash();
                            showMessage(message);
                            break;
                    }
                },
                error: function (err) {
                    GPINTMOBILE.HideSplash();
                    showMessage('<%=CultureHelpers.ElementValues.GetResourceString("Exception", XeErrors)%>');
                    $('#btnSubmit').attr("disabled", false);
                }
            });
        }

        function showMessage(message) {
            if(_.isEmpty(message)) return
            window.w88Mobile.Growl.shout('<div>' + message + '</div>');
        }
    </script>
</body>
</html>