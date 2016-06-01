<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Withdrawal_Default" %>
<%@ Register TagPrefix="uc" TagName="Wallet" Src="~/UserControls/MainWalletBalance.ascx" %>

<!DOCTYPE html>
<html>
<head>
    <title><%=string.Format("{0}", commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML))%></title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/Js/Main.js"></script>
    <script type="text/javascript" src="/_Static/JS/jquery.mask.min.js"></script>
</head>
<body>
    <div data-role="page" data-theme="b">
        <header data-role="header" data-theme="b" data-position="fixed" id="header">
            <a class="btn-clear ui-btn-left ui-btn" href="#divPanel" data-role="none" id="aMenu" data-load-ignore-splash="true">
                <i class="icon-navicon"></i>
            </a>
            <h1 class="title"><%=string.Format("{0}", commonCulture.ElementValues.getResourceString("withdrawal", commonVariables.LeftMenuXML))%></h1>
        </header>

        <div class="ui-content" role="main">
            <div class="wallet main-wallet">
                <uc:Wallet id="uMainWallet" runat="server" />
            </div>

            <div data-role="navbar">
                <ul id="withdrawalTabs" runat="server">
                    <li />
                </ul>
            </div>

            <form class="form" id="form1" runat="server" data-ajax="false">
                <div class="empty-state">
                    <div class="empty-state-icon">
                        <i class="ion ion-alert"></i>
                    </div>
                    <p id="paymentNote">
                    </p>
                </div>
            </form>
        </div>

        <!--#include virtual="~/_static/navMenu.shtml" -->
        <script type="text/javascript">
            $(function () {
                window.history.forward();

                $('#withdrawalTabs li').first().remove();

                if ($('#withdrawalTabs li a.btn-primary').length == 0) {
                    if ($('#withdrawalTabs li').first().children().attr('href') != undefined) {
                        window.location.replace($('#withdrawalTabs li').first().children().attr('href'));
                    } else {
                        $('#paymentNote').append('<%= commonCulture.ElementValues.getResourceString("paymentNotice", commonVariables.PaymentMethodsXML)%>');
                    }
                }
            });

        </script>
    </div>
</body>
</html>
