<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AutoLogin.aspx.cs" Inherits="_Secure_AutoLogin" %>


<!DOCTYPE html>

<html>
<head runat="server">
    <meta name="viewport" content="width=device-width,height=device-height,initial-scale=1.0" />
    <title></title>
    <link rel="stylesheet" href="/_Static/Css/ptlogin.css">

    <script src="/_Static/JS/Mobile/jquery-1.11.3.min.js"></script>
    <script>
        window.w88Mobile = {};
    </script>
    <script src="/_Static/JS/vendor/lodash.js"></script>
    <script src="/_Static/JS/GPINT.js"></script>
    <script src="/_Static/JS/Cookie.js"></script>
</head>
<body>
    <div class="bg">
        <div class="form-container">
            <div class="form-center">
                <div class="form-logo">
                    <img src="/_Static/Images/logo-en.png" alt="">
                </div>
                <form id="form1" runat="server">
                    <div class="form-row">
                        <div class="form-col form-col-label">
                            <asp:Label ID="lblUsername" runat="server" Text="username" />
                        </div>
                        <div class="form-col form-col-prefix text-right">
                            <span class="blue">W88</span>
                        </div>
                        <div class="form-col form-col-input">
                            <input type="text" class="form-input" id="username" name="username" required />
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-col form-col-label">
                            <asp:Label ID="lblPassword" runat="server" Text="password" />
                        </div>
                        <div class="form-col form-col-input form-col-offset">
                            <input type="password" class="form-input" id="password" name="password" required />
                        </div>
                    </div>
                    <div class="form-row capt">
                        <div class="form-col form-col-label">
                            <asp:Label ID="lblCaptcha" runat="server" Text="code" />
                        </div>
                        <div class="form-col form-col-input form-col-offset">
                            <asp:Image ID="imgCaptcha" runat="server" CssClass="imgCaptcha" Style="width: 100%;" />
                            <input type="text" class="form-input" id="txtCaptcha" name="txtCaptcha" maxlength="4" type="tel" />
                        </div>
                    </div>
                    <div class="form-row">
                        <input id="btnLogin" type="button" value="" class="button" role="button" data-corners="false" style="width: 100%" />
                    </div>
                    <p class="text-center form-text">
                        <a href="/_Secure/ForgotPassword.aspx" target="_top" type="forgot_login"><%=commonCulture.ElementValues.getResourceString("forgotpassword", commonVariables.LeftMenuXML)%></a>
                        <br />
                        <br />
                        <asp:Literal ID="lblRegister" runat="server" />
                        <br />
                        <br />
                        <asp:Literal ID="lblRegNote" runat="server" />
                    </p>
                    <asp:HiddenField ID="hfLoginTranslation" runat="server" />
                </form>
            </div>

        </div>
    </div>


    <script type="text/javascript">
        var counter = 0;

        $(function () {
            $(".ui-loader").hide();

            var lang = '<%=commonVariables.SelectedLanguageShort%>';
            if (lang == 'cn') {
                $('.form-logo img').attr('src', '/_Static/Images/logo-cn.png');
            }

            $('#<%=imgCaptcha.ClientID%>').hide();
            $('#<%=lblCaptcha.ClientID%>').hide();
            $('#txtCaptcha').hide();

        });

        $(function () { $('#<%=imgCaptcha.ClientID%>').attr('src', '/_Secure/Captcha.aspx?t=' + new Date().getTime()); });
        $('#<%=imgCaptcha.ClientID%>').click(function () { $(this).attr('src', '/_Secure/Captcha.aspx?t=' + new Date().getTime()); });


        $(document).ready(function () {
            $('#btnLogin').val($("#hfLoginTranslation").val());

            $('#btnLogin').click(function (e) {
                var message = ('<ul>');
                $('#btnLogin').attr("disabled", true);
                var username = _.trim($('#username').val()),
                    password = _.trim($('#password').val());

                var hasError = false;

                if (_.isEmpty(username)) {
                    message = ('<%=commonCulture.ElementValues.getResourceXPathString("Login/MissingUsername", xeErrors)%>');
                    hasError = true;
                    e.preventDefault();
                } else {
                    if (!/^[a-zA-Z0-9]+$/.test(username) || username.indexOf(' ') >= 0) {
                        message = ('<%=commonCulture.ElementValues.getResourceXPathString("Login/InvalidUsername", xeErrors)%>');
                        hasError = true;
                        e.preventDefault();
                    }
                }
                if (password.length == 0) {
                    message = ('<%=commonCulture.ElementValues.getResourceXPathString("Login/MissingPassword", xeErrors)%>');
                    hasError = true;
                    e.preventDefault();
                }

                if (counter >= 3) {
                    if ($('#txtCaptcha').val().trim().length == 0) {
                        message += ('<%=commonCulture.ElementValues.getResourceXPathString("Register/MissingVCode", xeErrors)%>');
                        hasError = true;
                        e.preventDefault();
                    }
                }

                if (hasError) {
                    $('#btnLogin').attr("disabled", false);
                    alert(message);
                    return;
                } else {
                    e.preventDefault();
                    initiateLogin();

                }
            });
        });

        function initiateLogin() {
            var udata = { Username: $('#username').val(), Password: $('#password').val(), Captcha: $('#txtCaptcha').val(), ioBlackBox: $('#ioBlackBox').val() };
            $.ajax({
                type: 'POST',
                contentType: "application/json",
                url: '/_Secure/AjaxHandlers/Login.ashx',
                beforeSend: function () { GPINTMOBILE.ShowSplash(); },
                timeout: function () {
                    $('#btnLogin').prop('disabled', false);
                    alert('<%=commonCulture.ElementValues.getResourceString("Exception", xeErrors)%>');
                    window.location.replace('/Default.aspx');
                },
                data: JSON.stringify(udata),
                success: function (xml) {

                    var message = xml.Message;

                    if (xml.Code == undefined) {
                        initiateLogin();
                        return;
                    }

                    switch (xml.Code) {
                        case "1":
                        case "resetPassword":
                            Cookies().setCookie('IsApp', '1', 1);
                            window.location.replace('/Funds.aspx?lang=<%=commonVariables.SelectedLanguage.ToLower()%>');
                            break;

                        case "22":
                            $('#btnLogin').attr("disabled", false);
                            alert(message);
                            break;

                        default:
                            counter += 1;

                            if (counter >= 3) {
                                $(".capt").removeClass("hide");
                                $('#<%=imgCaptcha.ClientID%>').show();
                            $('#<%=lblCaptcha.ClientID%>').show();
                            $('#txtCaptcha').show();
                            $('#<%=imgCaptcha.ClientID%>').attr('src', '/_Secure/Captcha.aspx?t=' + new Date().getTime());
                            $('#txtCaptcha').val('');
                            $('#password').val('');
                        }

                        $('#btnLogin').attr("disabled", false);
                        GPINTMOBILE.HideSplash();
                        alert(message);
                        break;
                }
                },
                error: function (err) {
                    alert('<%=commonCulture.ElementValues.getResourceString("Exception", xeErrors)%>');
                    window.location.replace('/Funds.aspx');
                }
            });
            }

    </script>

</body>
</html>
