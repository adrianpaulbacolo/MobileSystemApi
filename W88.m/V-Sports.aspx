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
    <div data-role="page" data-theme="b" data-ajax="false">
        <header data-role="header" data-theme="b" data-position="fixed" id="header">
            <a class="btn-clear ui-btn-left ui-btn" href="#divPanel" data-role="none" id="aMenu" data-load-ignore-splash="true">
                <i class="icon-navicon"></i>
            </a>
            <h1 class="title"><%=commonCulture.ElementValues.getResourceXPathString("Products/VSports/Label", commonVariables.ProductsXML)%></h1>
        </header>

        <div class="ui-content" role="main" id="sports">

            <ul class="row row-wrap bg-gradient">
                <%--<li class="col col-33">
                    <a href="/_Secure/Login.aspx" class="card" data-rel="sports" rel="vsportsBasketball" target="_blank">
                        <img src="/_Static/Images/sports/bnr-vSports-Basketball.jpg" class="img-responsive">
                        <div class="title"><%=commonCulture.ElementValues.getResourceXPathString("Products/VSports-basketball/Label", commonVariables.ProductsXML)%></div>
                    </a>
                </li>--%>
                <%--<li class="col col-33">
                    <a href="/_Secure/Login.aspx" class="card" data-rel="sports" rel="vsportsDogRacing">
                        <img src="_Static/Images/bnr-vSPORTS-dogracing.jpg" alt="a-Sports" class="img-responsive">
                       <div class="title"><%=commonCulture.ElementValues.getResourceXPathString("Products/VSports-dogracing/Label", commonVariables.ProductsXML)%></div>
                    </a>
                </li>--%>
                <li class="col col-33">
                    <a href="/_Secure/Login.aspx" class="card" data-rel="sports" rel="vsportsFootBall" target="_blank">
                        <img src="_Static/Images/sports/bnr-vSports-soccer.jpg" class="img-responsive">
                        <div class="title"><%=commonCulture.ElementValues.getResourceXPathString("Products/VSports-football/Label", commonVariables.ProductsXML)%></div>
                    </a>
                </li>
                <%--<li class="col col-33">
                    <a href="/_Secure/Login.aspx" class="card" data-rel="sports" rel="vsportsHorseRacing">
                        <img src="_Static/Images/bnr-vSPORTS-horseracing.jpg" alt="a-Sports" class="img-responsive">
                        <div class="title"><%=commonCulture.ElementValues.getResourceXPathString("Products/VSports-horseracing/Label", commonVariables.ProductsXML)%></div>
                    </a>
                </li>--%>
                <%--<li class="col col-33">
                    <a href="/_Secure/Login.aspx" class="card" data-rel="sports" rel="vsportsTennis">
                        <img src="_Static/Images/bnr-vSPORTS-tennis.jpg" alt="a-Sports" class="img-responsive">
                        <div class="title"><%=commonCulture.ElementValues.getResourceXPathString("Products/VSports-tennis/Label", commonVariables.ProductsXML)%></div>
                    </a>
                </li>--%>
            </ul>
            <div class="note text-center" hidden>
                <%=commonCulture.ElementValues.getResourceXPathString("Products/VSports/Notice", commonVariables.ProductsXML)%>
            </div>
        </div>

        <!--#include virtual="~/_static/navMenu.shtml" -->

        <script type="text/javascript">
            $(function () {

                (function (a) {(jQuery.browser = jQuery.browser || {}).android = /android|(android|bb\d+|meego).+mobile/i.test(a)})(navigator.userAgent || navigator.vendor || window.opera);

                if ($.browser.android) {
                    $('.note').show();
                } else {
                    $('.note').hide();
                }
            });
        </script>

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
