<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="_Secure_Login" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta name="viewport" content="width=device-width,height=device-height,initial-scale=1.0" />
    <title><%=commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML) + commonCulture.ElementValues.getResourceString("login", commonVariables.LeftMenuXML)%></title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/Js/PreLoad.js"></script>
    <script type="text/javascript" src="/_Static/Js/Main.js"></script>

</head>
<body>
    <div data-role="page" data-close-btn="right" data-corners="false" id="login">
        <!--#include virtual="~/_static/logoOnly.inc" -->

        <div class="ui-content" role="main">
            <form class="form" id="form1" runat="server" data-ajax="false">
                <ul class="list fixed-tablet-size">
                    <li class="item item-icon-left item-input">
                        <i class="icon icon-profile"></i>
                        <asp:Label ID="lblUsername" runat="server" AssociatedControlID="txtUsername" Text="Username" />
                        <asp:TextBox ID="txtUsername" runat="server" data-corners="false" autofocus="on" MaxLength="16" data-clear-btn="true" />
                    </li>
                    <li class="item item-icon-left item-input">
                        <i class="icon icon-password"></i>
                        <asp:Label ID="lblPassword" runat="server" AssociatedControlID="txtPassword" Text="password" />
                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" data-corners="false" MaxLength="10" data-clear-btn="true" />
                    </li>
                    <li class="item item-icon-left item-input hide capt">
                        <i class="icon icon-check"></i>
                        <asp:Label ID="lblCaptcha" runat="server" AssociatedControlID="txtCaptcha" Text="code" />
                        <div class="ui-grid-a">
                            <div class="ui-block-b">
                                <asp:Image ID="imgCaptcha" runat="server" CssClass="imgCaptcha" />
                            </div>
                        </div>
                        <div class="ui-grid-a">
                            <div class="ui-block-a">
                                <asp:TextBox ID="txtCaptcha" runat="server" MaxLength="4" type="tel" data-mini="true" data-corners="false" data-clear-btn="true" />
                            </div>
                        </div>
                    </li>
                    <li class="item row">
                        <div class="col">
                            <asp:Button ID="btnSubmit" runat="server" Text="login" data-corners="false" />
                        </div>
                    </li>
                     <li class="item row">
                        <div class="col">
                           <a href="ForgotPassword.aspx" data-ajax="false"><%=commonCulture.ElementValues.getResourceString("forgotpassword", commonVariables.LeftMenuXML)%></a>
                        </div>
                    </li>
                </ul>

                <asp:HiddenField runat="server" ID="ioBlackBox" Value="" />
                <asp:Literal ID="lblRegister" runat="server" Visible="false" />
            </form>
        </div>

        <script type="text/javascript">
            $(function () { $('#<%=imgCaptcha.ClientID%>').attr('src', '/_Secure/Captcha.aspx?t=' + new Date().getTime()); });
            $('#<%=imgCaptcha.ClientID%>').click(function () { $(this).attr('src', '/_Secure/Captcha.aspx?t=' + new Date().getTime()); });

            var counter = 0;
            $('#<%=imgCaptcha.ClientID%>').attr('class', 'hide');
            $('#<%=lblCaptcha.ClientID%>').attr('class', 'hide');
            $('#<%=txtCaptcha.ClientID%>').attr('class', 'hide');

            $(document).ready(function () {
                $('#<%=btnSubmit.ClientID%>').click(function (e) {
                    var message = ('<ul>');
                    $('#btnSubmit').attr("disabled", true);

                    var hasError = false;

                    if ($('#txtUsername').val().trim().length == 0) {
                        message += ('<li><%=commonCulture.ElementValues.getResourceXPathString("Login/MissingUsername", xeErrors)%></li>');
                        hasError = true;
                        e.preventDefault();
                    }
                    if (!/^[a-zA-Z0-9]+$/.test($('#txtUsername').val().trim())) {
                        message += ('<li><%=commonCulture.ElementValues.getResourceXPathString("Login/InvalidUsername", xeErrors)%></li>');
                        hasError = true;
                        e.preventDefault();
                    }
                    if ($('#txtUsername').val().trim().indexOf(' ') >= 0) {
                        message += ('<li><%=commonCulture.ElementValues.getResourceXPathString("Login/InvalidUsername", xeErrors)%></li>');
                        hasError = true;
                        e.preventDefault();
                    }
                    if ($('#txtPassword').val().trim().length == 0) {
                        message += ('<li><%=commonCulture.ElementValues.getResourceXPathString("Login/MissingPassword", xeErrors)%></li>');
                        hasError = true;
                        e.preventDefault();
                    }

                    if (counter >= 3) {
                        if ($('#txtCaptcha').val().trim().length == 0) {
                            message += ('<li><%=commonCulture.ElementValues.getResourceXPathString("Register/MissingVCode", xeErrors)%></li>');
                            hasError = true;
                            e.preventDefault();
                        }
                    }

                    if (hasError) {
                        message += ('</ul>');
                        $('#btnSubmit').attr("disabled", false);
                        window.w88Mobile.Growl.shout(message);
                        return;
                    } else {
                        e.preventDefault();
                        initiateLogin();

                    }
                });
            });

            function initiateLogin() {
                var udata = { Username: $('#txtUsername').val(), Password: $('#txtPassword').val(), Captcha: $('#txtCaptcha').val(), ioBlackBox: $('#ioBlackBox').val() };
                $.ajax({
                    type: "POST",
                    contentType: "application/json",
                    url: '/_Secure/AjaxHandlers/Login.ashx',
                    beforeSend: function () { GPINTMOBILE.ShowSplash(); },
                    timeout: function () {
                        $('#<%=btnSubmit.ClientID%>').prop('disabled', false);
                        window.w88Mobile.Growl.shout('<ul><li><%=commonCulture.ElementValues.getResourceString("Exception", xeErrors)%></li></ul>');
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
                                window.location.replace('/Funds.aspx');
                                break;
                            case "22":
                                GPINTMOBILE.HideSplash();
                                $('#btnSubmit').attr("disabled", false);
                                window.w88Mobile.Growl.shout('<div>' + message + '</div>');
                                break;

                            default:
                                counter += 1;
                                console.log(xml.Code);

                                if (counter >= 3) {
                                    $(".capt").removeClass("hide");
                                    $('#<%=imgCaptcha.ClientID%>').attr('class', 'show imgCaptcha');
                                    $('#<%=lblCaptcha.ClientID%>').attr('class', 'show imgCaptcha');
                                    $('#<%=txtCaptcha.ClientID%>').attr('class', 'show imgCaptcha');
                                    $('#<%=imgCaptcha.ClientID%>').attr('src', '/_Secure/Captcha.aspx?t=' + new Date().getTime());
                                    $('#<%=txtCaptcha.ClientID%>').val('');
                                    $('#<%=txtPassword.ClientID%>').val('');
                                }

                                $('#btnSubmit').attr("disabled", false);
                                GPINTMOBILE.HideSplash();
                                window.w88Mobile.Growl.shout('<div>' + message + '</div>');
                                break;
                        }
                    },
                    error: function (err) {
                        GPINTMOBILE.HideSplash();
                        window.w88Mobile.Growl.shout('<%=commonCulture.ElementValues.getResourceString("Exception", xeErrors)%>');
                        window.location.replace('<%=strRedirect%>');
                    }
                });
            }
        </script>

        <script type="text/javascript" src="https://code.jquery.com/ui/1.11.4/jquery-ui.min.js"></script>

    </div>
</body>
</html>
