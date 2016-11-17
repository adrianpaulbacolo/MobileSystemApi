<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Deposit_Default" %>

<%@ Register TagPrefix="uc" TagName="Wallet" Src="~/UserControls/MainWalletBalance.ascx" %>

<!DOCTYPE html>
<html>
<head>
    <title><%=string.Format("{0}", commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML))%></title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/Js/Main.js"></script>
</head>
<body>
    <div data-role="page" data-theme="b">
        <header data-role="header" data-theme="b" data-position="fixed" id="header">
            <a class="btn-clear ui-btn-left ui-btn" href="#divPanel" data-role="none" id="aMenu" data-load-ignore-splash="true">
                <i class="icon-navicon"></i>
            </a>
            <h1 class="title"><%=string.Format("{0}", commonCulture.ElementValues.getResourceString("deposit", commonVariables.LeftMenuXML))%></h1>
        </header>

        <div class="ui-content" role="main">
            <div class="wallet main-wallet">
                <uc:Wallet ID="uMainWallet" runat="server" />
            </div>

            <div data-role="navbar" id="depositTabs" runat="server">
            </div>

            <form class="form" id="form1" runat="server" data-ajax="false">
                <div class="empty-state" id="loader"></div>
                <div class="empty-state" hidden>
                    <div class="empty-state-icon">
                        <i class="ion ion-alert"></i>
                    </div>
                    <p id="paymentNote">
                    </p>
                </div>
            </form>
        </div>
        <!-- /content -->
        <!--#include virtual="~/_static/navMenu.shtml" -->
        <script type="text/javascript">
            $(function () {
                window.history.forward();

                var loader = GPInt.prototype.GetLoaderScafold();

                $("#loader").html(loader);

                if ($('#depositTabs li a.btn-primary').length == 0) {
                    if ($('#depositTabs li').first().children().attr('href') != undefined) {
                        window.location.replace($('#depositTabs li').first().children().attr('href'));
                    } else {
                        // track accounts with no gateways
                        w88Mobile.PiwikManager.trackEvent({
                            category: "Deposit",
                            action: "<%=base.strCountryCode %>",
                            name: "<%=base.strMemberID %>"
                            });

                        $('.empty-state').show();
                        $('#paymentNote').append('<%= commonCulture.ElementValues.getResourceString("paymentNotice", commonVariables.PaymentMethodsXML)%>');

                        $("#loader").hide();
                    }
                }
            });
        </script>
    </div>
</body>
</html>
