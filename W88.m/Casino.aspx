<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Index.aspx.cs" Inherits="_Index" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <title>Live Casino</title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/Js/Main.js"></script>
</head>
<body>
    <!--#include virtual="~/_static/splash.shtml" -->
    <div data-role="page" id="casino">
        <header data-role="header" data-theme="b" data-position="fixed" id="header">
            <a class="btn-clear ui-btn-left ui-btn" href="#divPanel" data-role="none" id="aMenu" data-load-ignore-splash="true">
                <i class="icon-navicon"></i>
            </a>
            <h1 class="title"><%=commonCulture.ElementValues.getResourceString("liveCasinoTitle", commonVariables.LeftMenuXML)%></h1>
        </header>

        <div class="ui-content" role="main">
            <ul class="row row-wrap row-no-padding responsive-lg">
                <li class="col">
                    <figure class="banner">
                        <img src="_Static/Images/casino/clubwpremierbanner.jpg" class="img-responsive-full img-bg">
                        <figcaption class="banner-caption">
                            <h3 class="title"><%=commonCulture.ElementValues.getResourceString("clubwpremier", commonVariables.LeftMenuXML)%></h3>
                            <P><%=commonCulture.ElementValues.getResourceString("liveCasinoMessage", commonVariables.LeftMenuXML)%></P>
                            <a href="/_Static/ClubW/casino.aspx" data-ajax="false" class="ui-btn btn-primary"><%=commonCulture.ElementValues.getResourceString("playNow", commonVariables.LeftMenuXML)%></a>
                        </figcaption>
                    </figure>
                </li>
                <li class="col">
                    <figure class="banner">
                        <img src="_Static/Images/casino/clubwbanner.jpg" class="img-responsive-full img-bg">
                        <figcaption class="banner-caption">
                            <h3 class="title"><%=commonCulture.ElementValues.getResourceString("liveCasino", commonVariables.LeftMenuXML)%></h3>
                            <P><%=commonCulture.ElementValues.getResourceString("liveCasinoMessage", commonVariables.LeftMenuXML)%></P>
                            <a href="/_Static/ClubW/casino.aspx" data-ajax="false" class="ui-btn btn-primary"><%=commonCulture.ElementValues.getResourceString("playNow", commonVariables.LeftMenuXML)%></a>
                        </figcaption>
                    </figure>
                </li>
                <li class="col">
                    <figure class="banner">
                        <img src="_Static/Images/casino/clubpalazzobanner.jpg" class="img-responsive-full img-bg">
                        <figcaption class="banner-caption">
                            <h3 class="title"><%=commonCulture.ElementValues.getResourceString("clubPalazo", commonVariables.LeftMenuXML)%></h3>
                            <P><%=commonCulture.ElementValues.getResourceString("clubPalazoMessage", commonVariables.LeftMenuXML)%></P>
                            <a href="/_Static/Palazzo/casino.aspx"  data-ajax="false" class="ui-btn btn-primary"><%=commonCulture.ElementValues.getResourceString("playNow", commonVariables.LeftMenuXML)%></a>
                        </figcaption>
                    </figure>
                </li>
                <li class="col">
                    <figure class="banner">
                        <img src="_Static/Images/casino/clubmassimobanner.jpg" class="img-responsive-full img-bg">
                        <figcaption class="banner-caption">
                            <h3 class="title"><%=commonCulture.ElementValues.getResourceString("clubMassimo", commonVariables.LeftMenuXML)%></h3>
                            <P><%=commonCulture.ElementValues.getResourceString("clubMassimoMessage", commonVariables.LeftMenuXML)%></P>
                            <a href="https://livegames.cdn.gameassists.co.uk/AIR/Poria/Installer/V20021/w88/Download.html"  data-ajax="false"  class="ui-btn btn-primary" target="_blank"><%=commonCulture.ElementValues.getResourceString("playNow", commonVariables.LeftMenuXML)%></a>
                        </figcaption>
                    </figure>
                </li>
            </ul>
        </div>

        <!--#include virtual="~/_static/footer.shtml" -->
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
