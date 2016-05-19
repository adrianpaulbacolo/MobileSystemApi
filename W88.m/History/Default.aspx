<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="History_Default" %>

<%@ Register TagPrefix="uc" TagName="Wallet" Src="~/UserControls/MainWalletBalance.ascx" %>

<!DOCTYPE html>
<html>
<head>
    <title><%=string.Format("{0} {1}", commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML), commonCulture.ElementValues.getResourceString("history", commonVariables.HistoryXML))%></title>
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
            <h1 class="title"><%=commonCulture.ElementValues.getResourceString("history", commonVariables.HistoryXML)%></h1>
        </header>

        <div class="ui-content" role="main">
                    <div class="wallet main-wallet">
                        <uc:Wallet ID="uMainWallet" runat="server" />
                    </div>

            <form class="form" id="form1" runat="server" data-ajax="false">
                <br />
            <ul class="list fixed-tablet-size">
                <li class="item item-icon-left item-icon-right item-border-btm">
                    <a href="/History/DepositWithdrawal.aspx" data-ajax="false">
                        <i class="icon icon-deposit"></i>
                            <h4 class="title"><%=commonCulture.ElementValues.getResourceString("depositwithdrawal", commonVariables.HistoryXML)%></h4>
                        <i class="icon ion-ios-arrow-right"></i>
                    </a>
                </li>
                <li class="item item-icon-left item-icon-right item-border-btm">
                    <a href="/History/FundTransfer.aspx" data-ajax="false">
                        <i class="icon icon-transfer"></i>
                            <h4 class="title"><%=commonCulture.ElementValues.getResourceString("fundtransfer", commonVariables.HistoryXML)%></h4>
                        <i class="icon ion-ios-arrow-right"></i>
                    </a>
                </li>
                <li class="item item-icon-left item-icon-right item-border-btm">
                    <a href="/History/ReferralBonus.aspx" data-ajax="false">
                        <i class="icon icon-referral-bonus"></i>
                            <h4 class="title"><%=commonCulture.ElementValues.getResourceString("referralbonus", commonVariables.HistoryXML)%></h4>
                        <i class="icon ion-ios-arrow-right"></i>
                    </a>
                </li>
                <li class="item item-icon-left item-icon-right item-border-btm">
                    <a href="/History/PromotionClaim.aspx" data-ajax="false">
                        <i class="icon icon-promo"></i>
                            <h4 class="title"><%=commonCulture.ElementValues.getResourceString("promotionclaim", commonVariables.HistoryXML)%></h4>
                        <i class="icon ion-ios-arrow-right"></i>
                    </a>
                </li>
                <li class="item row">
                    <div class="col">
                        <a href="/Funds.aspx" role="button" class="ui-btn btn-bordered" data-ajax="false"><%=commonCulture.ElementValues.getResourceString("cancel", commonVariables.LeftMenuXML)%></a>
                    </div>
                </li>
            </ul>
            </form>
        </div>
        <!--#include virtual="~/_static/navMenu.shtml" -->
    </div>
</body>
</html>
