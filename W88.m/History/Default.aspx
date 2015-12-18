<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_History_Default" %>

<!DOCTYPE html>
<html>
<head>
    <title>History</title>
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
            <h1 class="title">History</h1>
        </header>

        <div class="ui-content" role="main">

            <div class="row row-no-padding">
                <div class="col">
                    <div class="wallet main-wallet">
                        <label class="label"><%=commonCulture.ElementValues.getResourceString("mainWallet", commonVariables.LeftMenuXML)%></label>
                        <h2 class="value"><%=Session["Main"].ToString()%></h2>
                        <small class="currency"><%=commonVariables.GetSessionVariable("CurrencyCode")%></small>
                    </div>
                </div>
            </div>

            <ul class="list fixed-tablet-size">
                <li class="item item-icon-left item-icon-right item-border-btm">
                    <a href="" data-ajax="false">
                        <i class="icon icon-deposit"></i>
                        <h4 class="title">Deposit/Withdrawal</h4>
                        <i class="icon ion-ios-arrow-right"></i>
                    </a>
                </li>
                <li class="item item-icon-left item-icon-right item-border-btm">
                    <a href="" data-ajax="false">
                        <i class="icon icon-transfer"></i>
                        <h4 class="title">Fund Transfer</h4>
                        <i class="icon ion-ios-arrow-right"></i>
                    </a>
                </li>
                <li class="item item-icon-left item-icon-right item-border-btm">
                    <a href="" data-ajax="false">
                        <i class="icon icon-referral-bonus"></i>
                        <h4 class="title">Referral Bonus</h4>
                        <i class="icon ion-ios-arrow-right"></i>
                    </a>
                </li>
                <li class="item item-icon-left item-icon-right item-border-btm">
                    <a href="" data-ajax="false">
                        <i class="icon icon-promo"></i>
                        <h4 class="title">Promotion Claim</h4>
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
