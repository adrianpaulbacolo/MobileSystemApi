<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Palazzo.aspx.cs" Inherits="_Secure_Palazzo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width,height=device-height,initial-scale=1.0" />
    <title></title>
    <link rel="stylesheet" href="/_Static/Css/ptlogin.css">
    <script src="/_Static/JS/jquery-1.10.2.min.js"></script>
    <script src="/_Static/JS/GPINT.js"></script>
    <script type="text/javascript" src="/_Static/Js/Main.js"></script>
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
                        <div class="form-col form-col-30">
                            <asp:Label ID="lblUsername" runat="server" Text="username" />
                        </div>
                        <div class="form-col form-col-70">
                            <div class="form-row">
                                <div class="form-col form-col-20 text-right">
                                    <span class="blue">W88</span>
                                </div>
                                <div class="form-col form-col-80">
                                    <input type="text" class="form-input" id="username" name="username" required />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-col form-col-30">
                            <asp:Label ID="lblPassword" runat="server" Text="password" />
                        </div>
                        <div class="form-col form-col-70">
                            <input type="password" class="form-input" id="password" name="password" required />
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-col form-col-70 form-col-offset-30 form-col-xs-100">
                            <a href="javascript: login();" class="button" data-corners="false" role="button">login</a>
                        </div>
                    </div>
                    <div class="form-row forgot">
                        <a href="<%=commonIp.DomainName%>/ForgotPassword.aspx" target="_top" type="forgot_login"><%=commonCulture.ElementValues.getResourceString("forgotpassword", commonVariables.LeftMenuXML)%></a>
                    </div>
                    <p class="text-center">
                       <br />
                       <asp:Literal ID="lblRegister" runat="server" />
                       <br />
                       <br />
                       <asp:Literal ID="lblRegNote" runat="server" />
                   </p> 
                </form>
            </div>
        </div>
    </div>


    <script type="text/javascript">

        function login() {
            window.location = 'htcmd:login?username=W88' + document.getElementById('username').value + '&password=' + document.getElementById('password').value + '';
        }
        $(function () {

            var lang = '<%=commonVariables.SelectedLanguageShort%>';
            if (lang == 'cn') {
                $('.form-logo img').attr('src', '/_Static/Images/logo-cn.png');
            }
        });
    </script>

</body>
</html>
