<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ContactUs.aspx.cs" Inherits="_ContactUs" %>

<!DOCTYPE html>
<html>
<head>
    <title><%=commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML) + commonCulture.ElementValues.getResourceString("contactUs", commonVariables.LeftMenuXML)%></title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/Js/Main.js"></script>
</head>
<body>
    <!--#include virtual="~/_static/splash.shtml" -->
    <div data-role="page" data-theme="b">

        <header data-role="header" data-theme="b" data-position="fixed" id="header">
            <a class="btn-clear ui-btn-left ui-btn" href="#divPanel" data-role="none" role="button" id="aMenu" data-load-ignore-splash="true">
                <i class="icon-navicon"></i>
            </a>
            <h1 class="title"><%=commonCulture.ElementValues.getResourceString("contactUs", commonVariables.LeftMenuXML)%></h1>
        </header>

        <div class="ui-content contact-us" role="main">
            <ul class="list fixed-tablet-size" data-role="listview" data-icon="false">
                <li class="item item-icon-left">
                    <a id="aLiveChat" runat="server" href="/LiveChat" data-ajax="false">
                        <i class="icon ion-ios-chatboxes-outline"></i>
                        <h4><%=commonCulture.ElementValues.getResourceString("lblLiveChat", xeResources)%></h4>
                        <p>Get an instant live chat from our support team</p>
                    </a>
                </li>
                <li class="item item-icon-left">
                    <a id="aSkype" runat="server" href="javascript:void(0);">
                        <i class="icon icon-skype"></i>
                        <h4><%=commonCulture.ElementValues.getResourceString("lblSkype", xeResources)%></h4>
                        <p>Should you prefer to use Skype to chat with us</p>
                    </a>
                </li>
                <li class="item item-icon-left">
                    <a id="aEmail" runat="server" href="javascript:void(0);">
                        <i class="icon icon-mail"></i>
                        <h4><%=commonCulture.ElementValues.getResourceString("lblEmail", xeResources)%></h4>
                        <p>For General Betting and Account Enquiry only.</p>
                    </a>
                </li>
                <li class="item item-icon-left">
                    <a id="aBanking" runat="server" href="javascript:void(0);">
                        <i class="icon ion-social-usd-outline"></i>
                        <h4><%=commonCulture.ElementValues.getResourceString("lblBanking", xeResources)%></h4>
                        <p>For Banking Enquiry only.</p>
                    </a>
                </li>
                <li class="item item-icon-left" id="liPhone" runat="server">
                    <a id="aPhone" runat="server" href="/_Secure/Login.aspx">
                        <i class="icon ion-ios-telephone-outline"></i>
                        <h4><%=commonCulture.ElementValues.getResourceString("lblPhone", xeResources)%></h4>
                        <p>Call (+60) 166770468 or (+44) 2036085161 and we will be pleased to assist you (International charges may apply).</p>
                    </a>
                </li>
                <%--
                <li class="item item-icon-left">
                    <a id="aLine" runat="server" href="javascript:void(0);" data-rel="dialog" data-transition="slidedown">
                        <h4><%=commonCulture.ElementValues.getResourceString("lblLine", xeResources)%></h4>
                    </a>
                </li>
                --%>
            </ul>
        </div>

        <!--#include virtual="~/_static/footer.shtml" -->
        <!--#include virtual="~/_static/navMenu.shtml" -->
    </div>
</body>
</html>
