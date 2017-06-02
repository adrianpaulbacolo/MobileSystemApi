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
        <style type="text/css">.div-nav-header { background-position: center center; } .div-content-wrapper { margin: .5em; } </style> 
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
                    <div class="ui-field-contain ui-hide-label" style="display:none">
                        <asp:Label ID="lblCaptcha" runat="server" AssociatedControlID="txtCaptcha" Text="code" CssClass="ui-hidden-accessible" />
                        <div class="ui-grid-a">
                            <div class="ui-block-a"><asp:TextBox ID="txtCaptcha" runat="server" MaxLength="4" type="tel" data-mini="true" /></div>
                            <div class="ui-block-b"><asp:Image ID="imgCaptcha" runat="server" ImageUrl="/Captcha" CssClass="imgCaptcha" /></div>
                        </div>
                    </div>
                    <div>
                        <asp:Button ID="btnSubmit" runat="server" Text="login" CssClass="button-blue" data-corners="false" />
                    </div>
                    <div class="label-white"><asp:Literal ID="lblRegister" runat="server" /></div>
            		<asp:HiddenField runat="server" ID="ioBlackBox" Value="" />
                </div>
            </form>
        </div>
        <script type="text/javascript">   
            $(function () {
                var login_attemps = localStorage["login_attemps"] == undefined ? 0 : parseInt(localStorage["login_attemps"]);
                if (login_attemps >= 3) {
                    $('#lblCaptcha').parent().show();
                }
            });

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
                else if ("undefined,0,1,2".indexOf(localStorage["login_attemps"]) < 0 && $('#txtCaptcha').val().trim().length == 0) {
                    alert('<%=commonCulture.ElementValues.getResourceString("MissingVCode", xeErrors)%>');
                    $('#btnSubmit').attr("disabled", false);
                    e.preventDefault();
                    return;
                }
                else {
                    GPINTMOBILE.ShowSplash();
                    $.ajax({
                        contentType: "application/json; charset=utf-8",
                        url: "http://w88uat.com/IP2LOC?v=" + new Date().getTime(),
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

            $('#imgCaptcha').click(function () { $(this).attr('src', '/Captcha'); });

            function initiateLogin(postData) {
                $.ajax({
                    type: "POST",
                    url: '/_Secure/Login',
                    beforeSend: function () { GPINTMOBILE.ShowSplash(); },
                    timeout: function () {
                        $('#<%=btnSubmit.ClientID%>').prop('disabled', false);
                        alert('<%=commonCulture.ElementValues.getResourceString("Exception", xeErrors)%>');
                        window.location.replace('/Default.aspx');
                    },
                    data: {
                        txtUsername: $('#txtUsername').val(), txtPassword: $('#txtPassword').val(), txtCaptcha: $('#txtCaptcha').val(), txtIPAddress: postData.ip, txtCountry: postData.country, txtPermission: postData.permission, ioBlackBox: $('#ioBlackBox').val()
                        ,login_attemps: localStorage["login_attemps"] == undefined ? 0 : localStorage["login_attemps"]
                    },
                    success: function (xml) {
                        //alert($(xml).find('ErrorCode').text());
                        switch ($(xml).find('ErrorCode').text()) {
                            case "1":
                                window.location.replace('<%=strRedirect%>');
                                //window.location.replace('/Overview.aspx');
                                localStorage["login_attemps"] = 0;
                                break;
                            default:
                                alert($(xml).find('Message').text());

                                $('#<%=imgCaptcha.ClientID%>').attr('src', '/Captcha');
                                $('#<%=txtCaptcha.ClientID%>').val('');
                                $('#<%=txtPassword.ClientID%>').val('');
                                
                                GPINTMOBILE.HideSplash();

                                switch ($(xml).find('ErrorCode').text()) {
                                    case "21":
                                    case "23":
                                        console.log($(xml).find('ErrorCode').text());
                                        var login_attemps = localStorage["login_attemps"] == undefined ? 0 : parseInt(localStorage["login_attemps"]);
                                        login_attemps++;
                                        localStorage["login_attemps"] = login_attemps;
                                        console.log(localStorage["login_attemps"]);
                                        if (login_attemps >= 3) {
                                            $('#lblCaptcha').parent().show();
                                        }
                                        break;
                                    default:
                                        break;

                                }

                                break;
                        }
                    },
                    error: function (err) {
                        //alert("Error:initiateLogin");
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
