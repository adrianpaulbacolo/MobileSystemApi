<%@ Page Language="C#" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="_Secure_VIP_login" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <!-- The above 3 meta tags *must* come first in the head; any other head content must come *after* these tags -->
    <title>W88 Vip</title>

    <!-- Bootstrap -->
    <link href="js/jquery.modal.min.css" rel="stylesheet" />
    <link href="css/style.css" rel="stylesheet">

    <!-- HTML5 shim and Respond.js for IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
    <script src="https://oss.maxcdn.com/html5shiv/3.7.3/html5shiv.min.js"></script>
    <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
    <![endif]-->

    <script src="/_Static/JS/jquery-1.10.2.min.js"></script>
    <script src="js/jquery.modal.min.js"></script>
    <script src="/_Static/JS/vendor/lodash.js"></script>
    <script src="/_Static/JS/GPINT.js"></script>
    <script src="/_Static/JS/Cookie.js"></script>
    <script src="/_Static/JS/vendor/amplify.min.js"></script>

    <script type="text/javascript">
        window.w88Mobile = {}; 
        window.User = {};
        window.User.hasSession = <%= (!String.IsNullOrEmpty(commonVariables.CurrentMemberSessionId)) ? 1 : 0 %>;
        window.User.token = '<%= commonVariables.CurrentMemberSessionId %>';
        window.User.sessionInterval = '<%=ConfigurationManager.AppSettings.Get("sessionInterval") %>';
        window.User.lang = '<%=commonVariables.SelectedLanguage%>';
        window.User.storageExpiration = { expires: 1200000 };
    </script>

    <script src="/_Static/JS/i18n/contents-<%=commonVariables.SelectedLanguageShort%>.js"></script>
    <script src="/_Static/JS/modules/translate.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>

    <script>
        w88Mobile.APIUrl = '<%= ConfigurationManager.AppSettings.Get("APIUrl") %>';

        var _w88_contents = new w88Mobile.Translate();
        _w88_contents.init();
    </script>

</head>
<body>
    <section class="viplogin">
        <div class="viplogin-container">
            <div class="viplogin-box">
                <img src="img/w88-vip.png" alt="" class="viplogo">
                <form action="" class="viplogin-form" runat="server">
                    <h3><span id="formHeader"></span></h3>
                    <div class="input-group">
                        <div class="input-box">
                            <input type="text" class="input" placeholder="" id="txtUsername">
                            <span class="input-box-icon">
                                <img src="img/icon-user.png" alt=""></span>
                        </div>
                    </div>
                    <div class="input-group">
                        <div class="input-box">
                            <input type="password" class="input" placeholder="" id="txtPassword">
                            <span class="input-box-icon">
                                <img src="img/icon-password.png" alt=""></span>
                        </div>
                    </div>
                    <div class="input-group captcha">
                        <asp:Image ID="imgCaptcha" runat="server" CssClass="imgCaptcha" />
                        <div class="input-box">
                            <asp:TextBox ID="txtCaptcha" runat="server" MaxLength="4" type="tel" data-mini="true" data-corners="false" CssClass="input"  />
                             <span class="input-box-icon"><img src="img/icon-password.png" alt=""></span>
                        </div>
                    </div>
                    <div class="input-group">
                        <input id="btnSubmit" type="submit" class="button" value="">
                    </div>
                    <div class="text-center">
                        <a id="forgot" href="/_Secure/ForgotPassword.aspx"></a>
                    </div>
                </form>
                <div class="text-center">
                    <p>
                        <span id="loginNote0"></span>
                        <a id="csLink" href="/LiveChat/Default.aspx"></a>
                        <span id="loginNote1"></span>
                    </p>
                    <div id="sslNote"></div>
                </div>
            </div>
        </div>
    </section>
</body>

<script type="text/javascript">
    $(function () { $('#<%=imgCaptcha.ClientID%>').attr('src', '/_Secure/Captcha.aspx?t=' + new Date().getTime()); });
    $('#<%=imgCaptcha.ClientID%>').click(function () { $(this).attr('src', '/_Secure/Captcha.aspx?t=' + new Date().getTime()); });

    var counter = 0;
    $('.captcha').hide();

    $(document).ready(function () {
       
        if (Cookies().getCookie('language') == "zh-cn") {
            $("body").addClass("ch");
        }
        else {
            $("body").removeClass("ch");
        }

        setTranslations();

        function setTranslations() {
            if (_w88_contents.translate("LABEL_VIP_LOGIN") != "LABEL_VIP_LOGIN") {
                $("#formHeader").text(_w88_contents.translate("LABEL_VIP_LOGIN"));
                $("#txtUsername").attr("placeholder", _w88_contents.translate("LABEL_USERNAME"));
                $("#txtPassword").attr("placeholder", _w88_contents.translate("LABEL_PASSWORD"));
                $("#txtCaptcha").attr("placeholder", _w88_contents.translate("LABEL_CAPTCHA"));
                $("#btnSubmit").val(_w88_contents.translate("BUTTON_LOGIN"));

                var note0 = _w88_contents.translate("LABEL_VIP_LOGIN_NOTE_0").split('{0}');
                $("#loginNote0").text(note0[0]);
                $("#loginNote1").text(note0[1]);
                $("#sslNote").text(_w88_contents.translate("LABEL_VIP_LOGIN_NOTE_1"));
                $("#forgot").text(_w88_contents.translate("LABEL_FORGOTPASSWORD"));
                $("#csLink").text(_w88_contents.translate("LABEL_CS_LINK"));
            } else {
                window.setInterval(function() {
                    setTranslations();
                }, 500);
            }
        }

        $('#txtUsername').keyup(function () {
            $(this).val($(this).val().toLowerCase());
        });

        $('#btnSubmit').click(function (e) {
            var message = ('<ul>');
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
                $('#ModalMessage').html(message);
                $('#PopUpModal').modal();
                return;
            } else {
                e.preventDefault();
                initiateLogin();
            }
        });
    });

    function notAllow() {
        $('#ModalMessage').html('<%=commonCulture.ElementValues.getResourceXPathString("Login/MembersOnly", xeErrors)%>');
        $('#PopUpModal').modal();
    }

    function initiateLogin() {
        var udata = { Username: $('#txtUsername').val(), Password: $('#txtPassword').val(), Captcha: $('#txtCaptcha').val() };
        $.ajax({
            type: 'POST',
            contentType: "application/json",
            url: '/_Secure/AjaxHandlers/Login.ashx',
            beforeSend: function () { GPINTMOBILE.ShowSplash(true); },
            timeout: function () {
                $('#ModalMessage').html('<%=commonCulture.ElementValues.getResourceString("Exception", xeErrors)%>');
                window.location.replace('/Default.aspx');
            },
            data: JSON.stringify(udata),
            success: function (xml) {

                GPINTMOBILE.HideSplash();

                var message = xml.Message;

                if (xml.Code == undefined) {
                    initiateLogin();
                    return;
                }

                switch (xml.Code) {

                    case "1":

                        if (Cookies().getCookie('isvp') == 'true') {
                            window.location.replace('/Index');
                        } else {
                            notAllow();
                        }
                    break;

                case "22":
                    $('#PopUpModal').modal();
                    $('#ModalMessage').html('<div>' + message + '</div>');
                    break;

                    case "resetPassword":
                        if (Cookies().getCookie('isvp') == 'true') {
                            window.location.replace('/Settings/ChangePassword.aspx?lang=<%=commonVariables.SelectedLanguage.ToLower()%>');
                        } else {
                            notAllow();
                        }

                        break;

                    default:
                        counter += 1;

                        if (counter >= 3) {
                            $('.captcha').show();
                            $('#<%=imgCaptcha.ClientID%>').attr('src', '/_Secure/Captcha.aspx?t=' + new Date().getTime());
                            $('#<%=txtCaptcha.ClientID%>').val('');
                            $('#txtPassword').val('');
                        }

                    GPINTMOBILE.HideSplash();
                    $('#PopUpModal').modal();
                    $('#ModalMessage').html('<div>' + message + '</div>');
                    break;
            }
            },
            error: function (err) {
                $('#PopUpModal').modal();
                $('#ModalMessage').html('<%=commonCulture.ElementValues.getResourceString("Exception", xeErrors)%>');
            }
        });
    }

</script>

<div id="PopUpModal" class="modal" style="display: none;">
    <div id="ModalMessage" class="modal-content padding text-center"></div>
</div>

</html>
