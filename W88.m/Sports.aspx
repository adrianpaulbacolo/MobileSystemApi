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
            <h1 class="title"><%=commonCulture.ElementValues.getResourceString("sports", commonVariables.LeftMenuXML)%></h1>
        </header>

        <div class="ui-content" role="main" id="sports">

            <ul class="row row-wrap bg-gradient">
<%--                <li class="col col-50">
                    <a href="">
                        <img src="_Static/Images/sports/wsports-<%=commonVariables.SelectedLanguageShort%>.png" alt="a-Sports" class="img-responsive">
                    </a>
                </li>--%>
                <li class="col col-50">
                    <a href="<%=commonASports.getSportsbookUrl%>" target="_blank">
                        <img src="_Static/Images/sports/asports-<%=commonVariables.SelectedLanguageShort%>.png" alt="a-Sports" class="img-responsive">
                    </a>
                </li>
                <li class="col col-50">
                    <a href="<%=commonESports.getSportsbookUrl%>" target="_blank">
                        <img src="_Static/Images/sports/esports-<%=commonVariables.SelectedLanguageShort%>.png" alt="a-Sports" class="img-responsive">
                    </a>
                </li>
                <%--<li class="col col-50">
                    <a href="/V-Sports.aspx" data-ajax="false" target="_blank">
                        <img src="_Static/Images/vsports_<%=commonVariables.SelectedLanguageShort%>.png" alt="a-Sports" class="img-responsive">
                    </a>
                </li>--%>
<%--                <li class="col col-50">
                    <a href="">
                        <img src="_Static/Images/sports/isports-<%=commonVariables.SelectedLanguageShort%>.png" alt="a-Sports" class="img-responsive">
                    </a>
                </li>--%>
                <!-- <li class="col col-50">
                    <a href="">
                        <img src="_Static/Images/sports/xsports-<%=commonVariables.SelectedLanguageShort%>.png" alt="a-Sports" class="img-responsive">
                    </a>
                </li> -->
<%--                <li class="col col-50">
                    <a href="">
                        <img src="_Static/Images/sports/fantasyepl-<%=commonVariables.SelectedLanguageShort%>.png" alt="a-Sports" class="img-responsive">
                    </a>
                </li>--%>
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
