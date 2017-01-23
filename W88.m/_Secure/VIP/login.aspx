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
</head>
<body>
    <section class="viplogin">
        <div class="viplogin-container">
            <div class="viplogin-box">
                <img src="img/w88-vip.png" alt="" class="viplogo">
                <form action="" class="viplogin-form">
                    <h3>贵宾登录</h3>
                    <div class="input-group">
                        <div class="input-box">
                            <input type="text" class="input" placeholder="用户名" id="txtUsername">
                            <span class="input-box-icon">
                                <img src="img/icon-user.png" alt=""></span>
                        </div>
                    </div>
                    <div class="input-group">
                        <div class="input-box">
                            <input type="password" class="input" placeholder="密码" id="txtPassword">
                            <span class="input-box-icon">
                                <img src="img/icon-password.png" alt=""></span>
                        </div>
                    </div>
                    <div class="input-group">
                        <input id="btnSubmit" type="submit" class="button" value="登录">
                    </div>
                    <div class="text-center">
                        <a href="/_Secure/ForgotPassword.aspx">忘记密码?</a>
                    </div>
                </form>
                <div class="text-center">
                    <p>登录时遇到任何问题，请及时联<a href="/LiveChat/Default.aspx">系在线客服</a>获取帮助。 本网站采用最先进的 256 BIT SSL 服务器加密机制。</p>
                </div>
            </div>
        </div>
    </section>
</body>


<script type="text/javascript">
    $(document).ready(function () {
        $('#btnSubmit').click(function (e) {
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
        var udata = { Username: $('#txtUsername').val(), Password: $('#txtPassword').val() };
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

                            $('#ModalMessage').html('<%=commonCulture.ElementValues.getResourceXPathString("Login/MembersOnly", xeErrors)%>');
                            $('#PopUpModal').modal();
                        }
                        break;

                    case "22":
                        $('#btnSubmit').attr("disabled", false);
                        $('#PopUpModal').modal();
                        $('#ModalMessage').html('<div>' + message + '</div>');
                        break;

                    case "resetPassword":
                        window.location.replace('/Settings/ChangePassword.aspx?lang=<%=commonVariables.SelectedLanguage.ToLower()%>');
                        break;

                    default:
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

<div id="PopUpModal" class="modal" style="display:none;">
    <div class="padding">
        <div id="ModalMessage" class="download-app padding"></div>
    </div>
</div>

</html>
