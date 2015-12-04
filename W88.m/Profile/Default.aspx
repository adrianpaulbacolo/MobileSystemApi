<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Profile_Default" %>

<!DOCTYPE html>
<html>
<head>
    <title>Profile</title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/Js/Main.js"></script>
    <script type="application/javascript" src="/_Static/Js/add2home.js"></script>
</head>
<body>
    <!--#include virtual="~/_static/splash.shtml" -->
    <div id="divMain" data-role="page" data-theme="b" data-ajax="false">

        <header data-role="header" data-theme="b" data-position="fixed" id="header">
            <a class="btn-clear ui-btn-left ui-btn" href="#divPanel" data-role="none" id="aMenu" data-load-ignore-splash="true">
                <i class="icon-navicon"></i>
            </a>
            <h1 class="title">Profile</h1>
        </header>

        <div class="ui-content" role="main">

            <div class="row row-no-padding">
                <div class="col">
                    <div class="wallet main-wallet">
                        <label class="label">Main Wallet</label>
                        <h2 class="value"><%=Session["Main"].ToString()%></h2>
                        <small class="currency"><%=commonVariables.GetSessionVariable("CurrencyCode")%></small>
                    </div>
                </div>
                <div class="col">
                    <div class="wallet main-wallet rewards">
                        <label class="label">Rewards Club</label>
                        <h2 class="value">2983</h2>
                        <small class="currency">Points</small>
                    </div>
                </div>
            </div>

            <ul class="row row-bordered bg-gradient">
                <li class="col col-33">
                    <a href="../Funds.aspx" class="tile" data-ajax="false" data-transition="slidedown">
                        <span class="icon-wallet"></span>
                        <h4 class="title"><%=commonCulture.ElementValues.getResourceString("fundmanagement", commonVariables.LeftMenuXML)%></h4>
                    </a>
                </li>
                <li class="col col-33">
                    <a href="/LiveChat/Default.aspx" class="tile" runat="server" data-ajax="false">
                        <span class="icon-chat"></span>
                        <h4 class="title">Live Chat</h4>
                    </a>
                </li>
                <li class="col col-33">
                    <a href="/Upload/Default.aspx" class="tile" data-ajax="false">
                        <span class="icon-submit"></span>
                        <h4 class="title">Submit/Upload</h4>
                    </a>
                </li>
                <li class="col col-33">
                    <a href="../Settings" class="tile" data-ajax="false">
                        <span class="icon-settings"></span>
                        <h4 class="title">Settings</h4>
                    </a>
                </li>
            </ul>

        </div>

        <!--#include virtual="~/_static/footer.shtml" -->
        <!--#include virtual="~/_static/navMenu.shtml" -->

    </div>
</body>
</html>
