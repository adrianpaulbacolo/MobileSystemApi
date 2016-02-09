<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Index.aspx.cs" Inherits="_Index" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <title>Sports</title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/Js/Main.js"></script>
</head>
<body>
    <!--#include virtual="~/_static/splash.shtml" -->
    <div data-role="page" id="sports">
        <header data-role="header" data-theme="b" data-position="fixed" id="header">
            <a class="btn-clear ui-btn-left ui-btn" href="#divPanel" data-role="none" id="aMenu" data-load-ignore-splash="true">
                <i class="icon-navicon"></i>
            </a>
            <h1 class="title"><%=commonCulture.ElementValues.getResourceString("sports", commonVariables.LeftMenuXML)%></h1>
        </header>

        <div class="ui-content" role="main" >
            <ul class="row banner-lists banner-odd-even">
                <li class="col">
                    <figure class="banner">
                        <img src="_Static/Images/sports/a-sports-banner.jpg" class="img-responsive img-bg">
                        <figcaption class="banner-caption">
                            <h3 class="title"><%=commonCulture.ElementValues.getResourceXPathString("Products/ASports/Label", commonVariables.ProductsXML)%></h3>
                            <p><%=commonCulture.ElementValues.getResourceXPathString("Products/ASports/Description", commonVariables.ProductsXML)%></p>
                            <a href="<%=(string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId) ? "_Secure/Login" : commonASports.getSportsbookUrl)%>" data-ajax="false" class="ui-btn btn-primary"><%=commonCulture.ElementValues.getResourceString("playNow", commonVariables.LeftMenuXML)%></a>
                        </figcaption>
                    </figure>
                </li>
                <li class="col">
                    <figure class="banner">
                        <img src="_Static/Images/sports/e-sports-banner.jpg" class="img-responsive img-bg">
                        <figcaption class="banner-caption">
                            <h3 class="title"><%=commonCulture.ElementValues.getResourceXPathString("Products/ESports/Label", commonVariables.ProductsXML)%></h3>
                            <p><%=commonCulture.ElementValues.getResourceXPathString("Products/ESports/Description", commonVariables.ProductsXML)%></p>
                            <a href="<%=(string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId) ? "_Secure/Login" : commonESports.getSportsbookUrl)%>" data-ajax="false" class="ui-btn btn-primary"><%=commonCulture.ElementValues.getResourceString("playNow", commonVariables.LeftMenuXML)%></a>
                        </figcaption>
                    </figure>
                </li>
            </ul>
        </div>

        <!--#include virtual="~/_static/navMenu.shtml" -->
    </div>
</body>
</html>
<!-- <ul class="hide">
    <li class="li-pokerIOS" runat="server" id ="pokerIOS_link">
        <a rel="PokerIOS" href="#" data-ajax="false" target="_blank" runat="server" id ="pokerIOS">
            <div><%=commonCulture.ElementValues.getResourceXPathString("Products/Poker/Label", commonVariables.ProductsXML)%></div>
        </a>
    </li>
    <li class="li-pokerAndroid" runat="server" id ="pokerAndroid_link" >
        <a rel="PokerAndroid" href="#" data-ajax="false" target="_blank" runat="server" id ="pokerAndroid">
            <div><%=commonCulture.ElementValues.getResourceXPathString("Products/Poker/Label", commonVariables.ProductsXML)%></div>
        </a>
    </li>
</ul> -->
