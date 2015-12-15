<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Settings_Default" %>

<!DOCTYPE html>
<html>
<head>
    <title>Change Password</title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/Js/Main.js"></script>
</head>
<body>
    <!--#include virtual="~/_static/splash.shtml" -->
    <div data-role="page" data-theme="b">

        <header data-role="header" data-theme="b" data-position="fixed" id="header">
            <a class="btn-clear ui-btn-left ui-btn" href="#divPanel" data-role="none" id="aMenu" data-load-ignore-splash="true">
                <i class="icon-navicon"></i>
            </a>
            <h1 class="title">Settings</h1>
        </header>

        <div class="ui-content" role="main">

            <ul class="list fixed-tablet-size">
                <li class="item item-icon-left item-icon-right item-border-btm">
                    <a href="ChangePassword.aspx" data-ajax="false">
                        <i class="icon icon-password"></i>
                        <h4 class="title"><%=commonCulture.ElementValues.getResourceString("changePassword", commonVariables.LeftMenuXML)%></h4>
                        <i class="icon ion-ios-arrow-right"></i>
                    </a>
                </li>
                <li class="item item-icon-left item-icon-right item-border-btm">
                    <a href="../ContactUs.aspx" data-ajax="false">
                        <i class="icon icon-phone"></i>
                        <h4 class="title"><%=commonCulture.ElementValues.getResourceString("contactUs", commonVariables.LeftMenuXML)%></h4>
                        <i class="icon ion-ios-arrow-right"></i>
                    </a>
                </li>
            </ul>

        </div>

        <!--#include virtual="~/_static/footer.shtml" -->
        <!--#include virtual="~/_static/navMenu.shtml" -->
    </div>
</body>
</html>
