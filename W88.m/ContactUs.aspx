<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ContactUs.aspx.cs" Inherits="_ContactUs" %>

<!DOCTYPE html>
<html>
<head>
    <title><%=commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML) + commonCulture.ElementValues.getResourceString("contactUs", commonVariables.LeftMenuXML)%></title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/Js/Main.js"></script>
    <link type="text/css" rel="stylesheet" href="/_Static/Css/ContactUs.css" />
</head>
<body>
    <!--#include virtual="~/_static/splash.shtml" -->
    <div data-role="page" data-theme="b">
        <!--#include virtual="~/_static/header.shtml" -->
        <div class="ui-content" role="main">
            <div class="div-page-header"><span><%=commonCulture.ElementValues.getResourceString("contactUs", commonVariables.LeftMenuXML)%></span></div>

            <div class="page-content">
                <ul data-role="listview" data-icon="false">
                    <li class="li-livechat">
                        <a id="aLiveChat" runat="server" href="/LiveChat/default.aspx" data-ajax="true">
                            <img src="/_Static/Images/contactus-livechat.png" class="ui-li-thumb" />
                            <h2><%=commonCulture.ElementValues.getResourceString("lblLiveChat", xeResources)%></h2>
                        </a>
                    </li>
                    <li class="li-skype">
                        <a id="aSkype" runat="server" href="javascript:void(0);">
                            <img src="/_Static/Images/contactus-skype.png" class="ui-li-thumb" />
                            <h2><%=commonCulture.ElementValues.getResourceString("lblSkype", xeResources)%></h2>
                        </a>
                    </li>
                    <li class="li-email">
                        <a id="aEmail" runat="server" href="javascript:void(0);">
                            <img src="/_Static/Images/contactus-email.png" class="ui-li-thumb" />
                            <h2><%=commonCulture.ElementValues.getResourceString("lblEmail", xeResources)%></h2>
                        </a>
                    </li>
                    <li class="li-email-banking">
                        <a id="aBanking" runat="server" href="javascript:void(0);">
                            <img src="/_Static/Images/contactus-email-banking.png" class="ui-li-thumb" />
                            <h2><%=commonCulture.ElementValues.getResourceString("lblBanking", xeResources)%></h2>
                        </a>
                    </li>
                    <li id="liPhone" runat="server" class="li-phone">
                        <a id="aPhone" runat="server" href="/_Secure/Login.aspx">
                            <img src="/_Static/Images/contactus-phone.png" class="ui-li-thumb" />
                            <h2><%=commonCulture.ElementValues.getResourceString("lblPhone", xeResources)%></h2>
                        </a>
                    </li>
                    <%--
                    <li class="li-line">
                        <a id="aLine" runat="server" href="javascript:void(0);" data-rel="dialog" data-transition="slidedown">
                            <img src="/_Static/Images/2x/contactus-line.jpg" />
                            <h2><%=commonCulture.ElementValues.getResourceString("lblLine", xeResources)%></h2>
                        </a>
                    </li>
                    --%>
                </ul>
            </div>
        </div>
        <!-- /content -->
        <!--#include virtual="~/_static/footer.shtml" -->
        <!--#include virtual="~/_static/navMenu.shtml" -->
    </div>
    <!-- /page -->
</body>
</html>