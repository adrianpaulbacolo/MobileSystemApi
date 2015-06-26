<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="_Secure_Login" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title><%=commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML) + commonCulture.ElementValues.getResourceString("login", commonVariables.LeftMenuXML)%></title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/Js/PreLoad.js"></script>
</head>
<body>
    <!--#include virtual="~/_static/splash.shtml" -->
    <div data-role="page" data-close-btn="right" data-corners="false">
        <!--#include virtual="~/_static/logoOnly.inc" -->
        <style type="text/css">
            .div-nav-header {
                background-position: center center;
            }

            .div-content-wrapper {
                margin: .5em;
            }
        </style>
        <div class="ui-content" role="main">
            <form id="form1" runat="server" data-ajax="false">
                <div class="div-content-wrapper">
                    <div class="ui-field-contain ui-hide-label">
                        <asp:Label ID="lblUsername" runat="server" AssociatedControlID="txtUsername" Text="username" CssClass="ui-hidden-accessible" />
                        <asp:TextBox ID="txtUsername" runat="server" data-corners="false" autofocus="on" MaxLength="16" data-clear-btn="true" />
                    </div>
                    <div class="ui-field-contain ui-hide-label">
                        <asp:Label ID="lblPassword" runat="server" AssociatedControlID="txtPassword" Text="password" CssClass="ui-hidden-accessible" />
                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" data-corners="false" MaxLength="10" data-clear-btn="true" />
                    </div>
                    <div class="ui-field-contain ui-hide-label">
                        <asp:Label ID="lblCaptcha" runat="server" AssociatedControlID="txtCaptcha" Text="code" CssClass="ui-hidden-accessible" />
                        <div class="ui-grid-a">
                            <div class="ui-block-a">
                                <asp:TextBox ID="txtCaptcha" runat="server" MaxLength="4" type="tel" data-mini="true" />
                            </div>
                            <div class="ui-block-b">
                                <asp:Image ID="imgCaptcha" runat="server" CssClass="imgCaptcha" />
                            </div>
                        </div>
                    </div>
                    <div>

                        <asp:Button ID="btnSubmit" runat="server" Text="login" CssClass="button-blue" data-corners="false" />
                    </div>
                    <div class="label-white">
                        <asp:Literal ID="lblRegister" runat="server" />
                    </div>
                    <asp:HiddenField runat="server" ID="ioBlackBox" Value="" />
                </div>
            </form>
        </div>
        <script type="text/javascript">
            $(function () { $('#<%=imgCaptcha.ClientID%>').attr('src', '/_Secure/Captcha.aspx?t=' + new Date().getTime()); });

            $('#form1').submit(function (e) {
                $('#btnSubmit').attr("disabled", true);
                if ($('#txtUsername').val().trim().length == 0) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("Login/MissingUsername", xeErrors)%>');
                    $('#btnSubmit').attr("disabled", false);
                    e.preventDefault();
                    return;
                }
                else if (!/^[a-zA-Z0-9]+$/.test($('#txtUsername').val().trim())) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("Login/InvalidUsername", xeErrors)%>');
                    $('#btnSubmit').attr("disabled", false);
                    e.preventDefault();
                    return;
                }
                else if ($('#txtUsername').val().trim().indexOf(' ') >= 0) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("Login/InvalidUsername", xeErrors)%>');
                    $('#btnSubmit').attr("disabled", false);
                    e.preventDefault();
                    return;
                }
                else if ($('#txtPassword').val().trim().length == 0) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("Login/MissingPassword", xeErrors)%>');
                    $('#btnSubmit').attr("disabled", false);
                    e.preventDefault();
                    return;
                }
                else if ($('#txtCaptcha').val().trim().length == 0) {
                    alert('<%=commonCulture.ElementValues.getResourceString("MissingVCode", xeErrors)%>');
                        $('#btnSubmit').attr("disabled", false);
                        e.preventDefault();
                        return;
                    }
                    else {
                        GPINTMOBILE.ShowSplash();
                        $.ajax({
                            contentType: "application/json; charset=utf-8",
                            url: "https://ip2loc.w2script.com/IP2LOC?v=" + new Date().getTime(),
                            dataType: "jsonp",
                            success: function (data) {
                                initiateLogin(data);
                                $('#btnSubmit').attr("disabled", false);
                                e.preventDefault();
                                //hideSplash();
                                return;
                            },
                            error: function (err) {
                                window.location.replace('/Default.aspx');
                                GPINTMOBILE.HideSplash();
                            }
                        });
                    }
                e.preventDefault();
                return;
            });

$('#<%=imgCaptcha.ClientID%>').click(function () { $(this).attr('src', '/_Secure/Captcha.aspx'); });

            function initiateLogin(postData) {
                // alert("going to process login");
                console.log('txt: ' + $('#txtCaptcha').val());
                $.ajax({
                    type: "POST",
                    url: '/_Secure/Login',
                    beforeSend: function () {
                        GPINTMOBILE.ShowSplash();
                    },
                    timeout: function () {
                        $('#<%=btnSubmit.ClientID%>').prop('disabled', false);
                        alert('<%=commonCulture.ElementValues.getResourceString("Exception", xeErrors)%>');
                        window.location.replace('/Default.aspx');
                    },
                    data: { txtUsername: $('#txtUsername').val(), txtPassword: $('#txtPassword').val(), txtCaptcha: $('#txtCaptcha').val(), txtIPAddress: postData.ip, txtCountry: postData.country, txtPermission: postData.permission, ioBlackBox: $('#ioBlackBox').val() },
                    success: function (xml) {
                        switch ($(xml).find('ErrorCode').text()) {
                            case "1":
                                switch ('<%=strRedirect%>') {
                                    default:
                                        // alert('<%=strRedirect%>');
                                        window.location.replace('<%=strRedirect%>');
                                        break;
                                }
                                break;
                            default:
                                alert($(xml).find('Message').text());
                                $('#<%=imgCaptcha.ClientID%>').attr('src', '/_Secure/Captcha.aspx?t=' + new Date().getTime());
                                $('#<%=txtCaptcha.ClientID%>').val('');
                                $('#<%=txtPassword.ClientID%>').val('');
                                GPINTMOBILE.HideSplash();
                                break;
                        }
                    },
                    error: function (err) {
                        alert('<%=commonCulture.ElementValues.getResourceString("Exception", xeErrors)%>');
                        window.location.replace('<%=strRedirect%>');
                    }
                });
            }
        </script>
        <script type="text/javascript" id="iovs_script">
            var io_operation = 'ioBegin';
            var io_bbout_element_id = 'ioBlackBox';
            //var io_submit_element_id = 'btnSubmit';
            var io_submit_form_id = 'form1';
            var io_max_wait = 5000;
            var io_install_flash = false;
            var io_install_stm = false;
            var io_exclude_stm = 12;
        </script>
        <script type="text/javascript" src="//ci-mpsnare.iovation.com/snare.js"></script>
    </div>
</body>
</html>