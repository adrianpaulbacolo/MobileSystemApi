<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DaddyPay.aspx.cs" Inherits="Deposit_DaddyPay" %>
<%@ Register TagPrefix="uc" TagName="Wallet" Src="~/UserControls/MainWalletBalance.ascx" %>

<!DOCTYPE html>
<html>
<head>
    <title><%=string.Format("{0} {1}", commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML), commonCulture.ElementValues.getResourceString("daddyPay", commonVariables.LeftMenuXML))%></title>
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
            <h1 class="title"><%=commonCulture.ElementValues.getResourceString("depositDaddyPay", commonVariables.LeftMenuXML)%></h1>
        </header>

        <div class="ui-content" role="main">
            <div class="wallet main-wallet">
                <uc:Wallet id="uMainWallet" runat="server" />
            </div>

            <div data-role="navbar">
                <ul id="depositTabs" runat="server">
                </ul>
            </div>

            <form class="form" id="form1" runat="server" data-ajax="false">
                <br>
                <ul class="list fixed-tablet-size">
                    <li class="row">
                        <div class="col">
                            <asp:Literal ID="lblMode" runat="server" /></div>
                        <div class="col">
                            <asp:Literal ID="txtMode" runat="server" /></div>
                    </li>
                    <li class="row">
                        <div class="col">
                            <asp:Literal ID="lblMinMaxLimit" runat="server" /></div>
                        <div class="col">
                            <asp:Literal ID="txtMinMaxLimit" runat="server" /></div>
                    </li>
                    <li class="row">
                        <div class="col">
                            <asp:Literal ID="lblDailyLimit" runat="server" /></div>
                        <div class="col">
                            <asp:Literal ID="txtDailyLimit" runat="server" /></div>
                    </li>
                    <li class="row">
                        <div class="col">
                            <asp:Literal ID="lblTotalAllowed" runat="server" /></div>
                        <div class="col">
                            <asp:Literal ID="txtTotalAllowed" runat="server" /></div>
                    </li>
                    <li class="item item-input">
                        <asp:TextBox ID="amount_txt" runat="server" placeholder="amount" type="number" step="any" min="1" data-clear-btn="true" />
                    </li>
                    <li class="item item-select">
                        <asp:DropDownList ID="bankDropDownList" runat="server" />
                        <asp:TextBox ID="accountName_txt" runat="server" placeholder="Account Name" data-clear-btn="true" />
                        <asp:TextBox ID="account_txt" runat="server" placeholder="Account" data-clear-btn="true" />
                    </li>
                    <li class="row">
                        <div class="col">
                            <a href="/Funds.aspx" role="button" class="ui-btn btn-bordered" id="btnCancel" runat="server" data-ajax="false"><%=commonCulture.ElementValues.getResourceString("cancel", commonVariables.LeftMenuXML)%></a>
                        </div>
                        <div class="col">
                            <asp:Button data-theme="b" ID="btnSubmit" runat="server" CssClass="button-blue" data-corners="false" OnClick="btnSubmit_Click" /></div>
                    </li>
                </ul>
            </form>
        </div>

        <!--#include virtual="~/_static/navMenu.shtml" -->
        <script type="text/javascript">
            $('#form1').submit(function (e) {
                window.w88Mobile.FormValidator.disableSubmitButton('#btnSubmit');
            });
            $(function () {
                window.history.forward();
            });
        </script>
    </div>
</body>
</html>
