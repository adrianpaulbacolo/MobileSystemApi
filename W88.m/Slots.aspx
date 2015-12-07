<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Index.aspx.cs" Inherits="_Index" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <title>Slots</title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/Js/Main.js"></script>
</head>
<body>
    <!--#include virtual="~/_static/splash.shtml" -->
    <div data-role="page" data-theme="b" data-ajax="false">
        <header data-role="header" data-theme="b" data-position="fixed" id="header">
            <a class="btn-clear ui-btn-left ui-btn" href="#divPanel" data-role="none" id="aMenu" data-load-ignore-splash="true">
                <i class="icon-navicon"></i>
            </a>
            <h1 class="title">Slots</h1>
        </header>

        <div class="ui-content" role="main" id="sports">

            <ul class="row row-wrap bg-gradient">
                <li class="col col-33">
                    <a href="/ClubBravado" class="card" data-ajax="false">
                        <img src="/_Static/Images/bnr-clubbravado.jpg" class="img-responsive">
                        <div class="title">Bravado</div>
                    </a>
                </li>
                <li class="col col-33">
                    <a href="/_static/palazzo/slots.aspx" class="card" data-ajax="false">
                        <img src="/_Static/Images/bnr-clubpalazzo-slots.jpg" class="img-responsive">
                        <div class="title">Palazzo</div>
                    </a>
                </li>
                <li class="col col-33">
                    <a href="/ClubMassimo" class="card" data-ajax="false">
                        <img src="/_Static/Images/bnr-clubmassimo-slots.jpg" class="img-responsive">
                        <div class="title">Massimo</div>
                    </a>
                </li>
                <li class="col col-33">
                    <a href="/ClubDivino" class="card" data-ajax="false">
                        <img src="/_Static/Images/bnr-clubdivino.jpg" class="img-responsive">
                        <div class="title">Divino</div>
                    </a>
                </li>
                <li class="col col-33">
                    <a href="/ClubCrescendo" class="card" data-ajax="false">
                        <img src="/_Static/Images/bnr-clubcrescendo.png" class="img-responsive">
                        <div class="title">Cresendo</div>
                    </a>
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
