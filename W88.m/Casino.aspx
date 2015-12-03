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
            <h1 class="title">Live Casino</h1>
        </header>

        <div class="ui-content" role="main">
            <ul class="row row-wrap row-no-padding responsive-lg">
                <li class="col">
                    <figure class="banner">
                        <img src="_Static/Images/casino/casino-banner-1.png" class="img-responsive-full img-bg">
                        <figcaption class="banner-caption">
                            <h3 class="title">Club W</h3>
                            <P>Enjoy live Casino instant cash rebate up to 0.8%. No payout limit &amp; rollover.</P>
                            <a href="/_Static/ClubW/casino.aspx" data-ajax="false" class="ui-btn btn-primary">Play Now</a>
                        </figcaption>
                    </figure>
                </li>
                <li class="col">
                    <figure class="banner">
                        <img src="_Static/Images/casino/casino-banner-2.png" data-rel="dialog" class="img-responsive-full img-bg">
                        <figcaption class="banner-caption">
                            <h3 class="title">Club Palazzo</h3>
                            <P>Innovative casino games with Asian &amp; European live dealers!</P>
                            <a href="/_Static/Palazzo/casino.aspx"  data-ajax="false" data-rel="dialog" class="ui-btn btn-primary">Play Now</a>
                        </figcaption>
                    </figure>
                </li>
                <li class="col">
                    <figure class="banner">
                        <img src="_Static/Images/casino/casino-banner-3.png" class="img-responsive-full img-bg">
                        <figcaption class="banner-caption">
                            <h3 class="title">Club Massimo</h3>
                            <P>Popular live casino games with high-quality video!</P>
                            <a href="https://livegames.cdn.gameassists.co.uk/AIR/Poria/Installer/V20021/w88/Download.html"  class="ui-btn btn-primary">Play Now</a>
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
