<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="_Secure_Login" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <meta name="viewport" content="width=device-width,height=device-height,initial-scale=1.0" />
    <title><%=commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML) + commonCulture.ElementValues.getResourceString("login", commonVariables.LeftMenuXML)%></title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/JS/PreLoad.js"></script>
</head>
<body>
    <div data-role="page" data-close-btn="right" data-corners="false" id="login">
        <header id="header" data-role="header" data-position="fixed" data-theme="b" data-tap-toggle="false">
            <a href="" role="button" data-rel="back" class="btn-clear ui-btn-left ui-btn" id="aMenu" data-load-ignore-splash="true">
                <span class="icon icon-back"></span>
                <%=commonCulture.ElementValues.getResourceString("back", commonVariables.LeftMenuXML)%>
            </a>
            <h1 class="title"><%=commonCulture.ElementValues.getResourceString("login", commonVariables.LeftMenuXML)%></h1>
        </header>
        <div class="ui-content" role="main">
            <form class="form" id="form1" runat="server" data-ajax="false">
                <ul class="list fixed-tablet-size">
                    <li class="item item-icon-left item-input">
                        <i class="icon icon-profile"></i>
                        <asp:Label ID="lblUsername" runat="server" AssociatedControlID="txtUsername" Text="username" />
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
                        <asp:Image ID="imgCaptcha" runat="server" CssClass="imgCaptcha" />
                        <asp:TextBox ID="txtCaptcha" runat="server" MaxLength="4" type="tel" data-mini="true" data-corners="false" />
                    </li>
                    <li class="item row">
                        <div class="col cancel">
                            <a href="" role="button" data-rel="back" class="ui-btn btn-bordered"><%=commonCulture.ElementValues.getResourceString("cancel", commonVariables.LeftMenuXML)%></a>
                        </div>
                        <div class="col">
                            <asp:Button ID="btnSubmit" runat="server" Text="login" data-corners="false" />
                        </div>
                    </li>
                    <li class="item item-text-wrap">
                        <asp:Literal ID="lblRegister" runat="server" />
                        <br />
                        <a href="ForgotPassword.aspx" data-ajax="false"><%=commonCulture.ElementValues.getResourceString("forgotpassword", commonVariables.LeftMenuXML)%></a>
                    </li>
                </ul>
                <asp:HiddenField runat="server" ID="ioBlackBox" Value="" />
            </form>
        </div>
        

        <script type="text/javascript">
            $(function () { $('#<%=imgCaptcha.ClientID%>').attr('src', '/_Secure/Captcha.aspx?t=' + new Date().getTime()); });
            $('#<%=imgCaptcha.ClientID%>').click(function () { $(this).attr('src', '/_Secure/Captcha.aspx?t=' + new Date().getTime()); });

            if ('<%=isSlotRedirect%>' === 'True') {
                $('#aMenu').attr('class', 'hide');
                $('.cancel').attr('class', 'hide');
            }

            var counter = 0;
            $('#<%=imgCaptcha.ClientID%>').attr('class', 'hide');
            $('#<%=lblCaptcha.ClientID%>').attr('class', 'hide');
            $('#<%=txtCaptcha.ClientID%>').attr('class', 'hide');

            $(document).ready(function () {

                var gameTemplate = '<div class="free-rounds"><img src="/_Static/images/v2/freerounds/Popup-free-round-<%=commonVariables.SelectedLanguageShort.ToLower()%>.jpg"> </img> <div class="free-round-btns"><a id="btnClaimNow" href="{0}" data-ajax="false" class="ui-btn btn-primary"></a><a id="btnClaimLater" href="{1}" data-ajax="false" class="ui-btn btn-primary"></a></div></div>';
                gameTemplate = gameTemplate.replace("{0}", _w88_products.FreeRoundsGameUrl);
                gameTemplate = gameTemplate.replace("{1}", "/ClubBravado");

                window.w88Mobile.Growl.init(gameTemplate, '');

                $('#<%=btnSubmit.ClientID%>').click(function (e) {
                    var message = ('<ul>');
                    $('#btnSubmit').attr("disabled", true);
                    var username = _.trim($('#txtUsername').val()),
                        password = _.trim($('#txtPassword').val());

                    var hasError = false;

                    if (_.isEmpty(username)) {
                        message += ('<li><%=commonCulture.ElementValues.getResourceXPathString("Login/MissingUsername", xeErrors)%></li>');
                        hasError = true;
                        e.preventDefault();
                    } else {
                        if (!/^[a-zA-Z0-9]+$/.test(username) || username.indexOf(' ') >= 0) {
                            message += ('<li><%=commonCulture.ElementValues.getResourceXPathString("Login/InvalidUsername", xeErrors)%></li>');
                            hasError = true;
                            e.preventDefault();
                        }
                    }
                    if (password.length == 0) {
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

                        amplify.clearStore();

                    }
                });
            });

            function initiateLogin() {
                var udata = { Username: $('#txtUsername').val(), Password: $('#txtPassword').val(), Captcha: $('#txtCaptcha').val(), ioBlackBox: $('#ioBlackBox').val() };
                $.ajax({
                    type: 'POST',
                    contentType: "application/json",
                    url: '/_Secure/AjaxHandlers/Login.ashx',
                    beforeSend: function() {
                        pubsub.publish('startLoadItem', { selector: '' });
                    },
                    timeout: function () {
                        $('#<%=btnSubmit.ClientID%>').prop('disabled', false);
                        window.w88Mobile.Growl.shout('<%=commonCulture.ElementValues.getResourceString("Exception", xeErrors)%>');
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

                            case "resetPassword":
                            case "1":

                                Cookies().setCookie('is_app', '0', 0);

                                window.User.token = Cookies().getCookie('s');

                                pubsub.subscribe('checkFreeRounds', onCheckFreeRounds);
                                _w88_products.checkFreeRounds();

                                function onCheckFreeRounds() {

                                    if (!_.isUndefined(_w88_products.FreeRoundsGameUrl)) {
                                        var gameTemplate = '<div class="free-rounds"><img src="/_Static/images/v2/freerounds/Popup-free-round-<%=commonVariables.SelectedLanguageShort.ToLower()%>.jpg"> </img> <div class="free-round-btns"><a id="btnClaimNow" href="{0}" data-ajax="false" class="ui-btn btn-primary"></a><a id="btnClaimLater" href="{1}" data-ajax="false" class="ui-btn btn-primary"></a></div></div>';
                                        gameTemplate = gameTemplate.replace("{0}", _w88_products.FreeRoundsGameUrl);
                                        gameTemplate = gameTemplate.replace("{1}", "/ClubBravado");

                                        window.w88Mobile.Growl.shout(gameTemplate, function () { window.location = "/index"; });

                                        $("#btnClaimNow").text(_w88_contents.translate("BUTTON_CLAIM"));
                                        $("#btnClaimLater").text(_w88_contents.translate("BUTTON_CLAIM_LATER"));
                                    } else {

                                        if (xml.Code == "resetPassword")
                                            window.location.replace('/Settings/ChangePassword.aspx?lang=<%=commonVariables.SelectedLanguage.ToLower()%>');
                                        else {
                                            if ('<%=strRedirect%>' !== '') {
                                                switch ('<%=strRedirect%>') {
                                                case 'mlotto':
                                                    window.location.replace('<%=commonLottery.getKenoUrl%>');
                                                    break;
                                                default:
                                                    window.location.replace('<%=strRedirect%>');
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }

                                break;

                            case "22":
                                $('#btnSubmit').attr("disabled", false);
                                window.w88Mobile.Growl.shout('<div>' + message + '</div>');
                                break;

                            default:
                                counter += 1;

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
                                window.w88Mobile.Growl.shout('<div>' + message + '</div>');
                                break;
                        }
                    },
                    error: function (err) {
                        pubsub.publish('stopLoadItem', { selector: '' });
                        window.w88Mobile.Growl.shout('<%=commonCulture.ElementValues.getResourceString("Exception", xeErrors)%>');
                        window.location.replace('<%=strRedirect%>');
                    }
                });
            }

        </script>

    </div>
</body>
</html>
