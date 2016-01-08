<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="_Secure_Login" %>

<!DOCTYPE html>


<html>
<head runat="server">
    <meta name="viewport" content="width=device-width,height=device-height,initial-scale=1.0"/>
    <title><%=commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML) + commonCulture.ElementValues.getResourceString("login", commonVariables.LeftMenuXML)%></title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/Js/PreLoad.js"></script>

</head>
<body>
    <!--#include virtual="~/_static/splash.shtml" -->
    <div data-role="page" data-close-btn="right" data-corners="false">
        <!--#include virtual="~/_static/logoOnly.inc" -->
        <link href="https://code.jquery.com/ui/1.10.4/themes/blitzer/jquery-ui.css" rel="stylesheet" />
        <style type="text/css">.div-nav-header { background-position: center center; } .div-content-wrapper { margin: .5em; } </style> 
        <style type="text/css">

@media screen and (min-width: 100%)
{

.center-me { 
  top:50%;
  left: 50%;
  transform: translate3d(-50%,-50%, 0);
  position: fixed;
}
}
            .ui-dialog
		{
			height: 200px;
			top: 0px;
		}
	
            .ui-widget-content
            {
	            color: #808080;
	            border: 1px solid #808080;  
            }

            .ui-dialog-titlebar
            {
                display: none;
            }
	       .ui-dialog-content
            {
 		      height: 250px;
            }

            .ui-widget-overlay
            {
                background: black;
            }

            #myDialogText
            {
                font-family: 'Tahoma';
                -webkit-font-smoothing: antialiased;
                font-size: medium;
                font-weight: normal;
                text-shadow: none;                
            }

            #myDialogText a
            {
                color: #2A8FBD;
            }

            #myDialogText a:hover
            {
                color: #2A8FBD;
            }

            .ui-corner-all
            {
                border-radius: 0px;
            }
        </style> 
        <div class="ui-content" role="main">
            <form id="form1" runat="server" data-ajax="false">
                <div class="div-content-wrapper">
                    <div class="ui-field-contain">
                        <asp:Label ID="lblUsername" runat="server" AssociatedControlID="txtUsername" Text="Username" Font-Size="X-Large" />
                        <asp:TextBox ID="txtUsername" runat="server" data-corners="false" autofocus="on" MaxLength="16" data-clear-btn="true" />
                    </div>
                    <div class="ui-field-contain">
                        <asp:Label ID="lblPassword" runat="server" AssociatedControlID="txtPassword" Text="password" Font-Size="X-Large" />
                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" data-corners="false" MaxLength="10" data-clear-btn="true" />
                    </div>
                    <div class="ui-field-contain">
                        <asp:Label ID="lblCaptcha" runat="server" AssociatedControlID="txtCaptcha" Text="code" Font-Size="X-Large" />
                        <div class="ui-grid-a">
                            <div class="ui-block-a"><asp:TextBox BackColor="#ffffcc" ID="txtCaptcha" runat="server" MaxLength="4" type="tel" data-mini="true" data-corners="false" data-clear-btn="true" /></div>
                            <div class="ui-block-b"><asp:Image ID="imgCaptcha" runat="server" CssClass="imgCaptcha" /></div>
                        </div>
                    </div>
                    <div>
                        <asp:Button ID="btnSubmit" runat="server" Text="login" CssClass="button-blue" data-corners="false" />
                    </div>
            		<asp:HiddenField runat="server" ID="ioBlackBox" Value="" />
                    <asp:Literal ID="lblRegister" runat="server" visible="false"/>
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

            function closeModal() {
                var $dialog = $("#myDialog").dialog();
                $dialog.dialog('close');
            }

            function initiateLogin(postData) {
                console.log('txt: ' + $('#txtCaptcha').val());
                $.ajax({
                    type: "POST",
                    url: '/_Secure/Login',
                    beforeSend: function () { GPINTMOBILE.ShowSplash(); },
                    timeout: function () {
                        $('#<%=btnSubmit.ClientID%>').prop('disabled', false);
                        alert('<%=commonCulture.ElementValues.getResourceString("Exception", xeErrors)%>');
                        window.location.replace('/Default.aspx');
                    },
                    data: { txtUsername: $('#txtUsername').val(), txtPassword: $('#txtPassword').val(), txtCaptcha: $('#txtCaptcha').val(), txtIPAddress: postData.ip, txtCountry: postData.country, txtPermission: postData.permission, ioBlackBox: $('#ioBlackBox').val() },
                    success: function (xml) {
                        switch ($(xml).find('ErrorCode').text()) {
                            case "1":
                            case "resetPassword":
                                window.location.replace('/Deposit/Default_app.aspx');
                                break;
                            case "22":
                                var message = $(xml).find('Message').text();

                                $("#myDialogText").html(message);
                                $("#myDialog").dialog({
                                    autoOpen: true,
                                    modal: true,
                                    draggable: false,
                                    resizable: false,
                                    width: 550,
                                    height: 155,
                                    position: { my: 'center', at: 'top+360' },
                                    show: "fade",
                                    hide: "fade"                                    
                                });
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
        <script type="text/javascript" src="https://code.jquery.com/ui/1.11.4/jquery-ui.min.js"></script>
        

<%--        <dialog id="dialog"> This is a test message <a href= "www.google.com">test</a></dialog>--%>
<%--            <div id="myDialog" style="z-index: 5">
                <div id="myDialogText" style="background-color:#252525; color: #FFFFFF; border-color:gray; border-style:solid; border-width:1pt;"></div>         
            </div>--%>
            <div id="myDialog">
                <div id="myDialogText" ></div>         
            </div>

    </div>
</body>
</html>
