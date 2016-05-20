<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Deposit_Default" %>

<!DOCTYPE html>
<html>
<head>
    <title><%=string.Format("{0}", commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML))%></title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/Js/Main.js"></script>
</head>
<body>
    <!--#include virtual="~/_static/splash.shtml" -->
    <div data-role="page" data-theme="b">
        <header data-role="header" data-theme="b" data-position="fixed" id="header">
            <h1 class="title"><%=string.Format("{0}", commonCulture.ElementValues.getResourceString("deposit", commonVariables.LeftMenuXML))%></h1>
        </header>

        <div class="ui-content" role="main">
            <div class="wallet main-wallet">
                <label class="label"><%=commonCulture.ElementValues.getResourceString("mainWallet", commonVariables.LeftMenuXML)%></label>
                <h2 class="value"><%=Session["Main"].ToString()%></h2>
                <small class="currency"><%=commonVariables.GetSessionVariable("CurrencyCode")%></small>
            </div>

            <div data-role="navbar">
                <ul id="depositTabs" runat="server">
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

        <script type="text/javascript">
            $(function () {
                window.history.forward();

                $('#depositTabs li').first().remove();

                if ($('#depositTabs li a.btn-primary').length == 0) {
                    if ($('#depositTabs li').first().children().attr('href') != undefined) {
                        window.location.replace($('#depositTabs li').first().children().attr('href'));
                    } else {
                        $('#paymentNote').append('<%= commonCulture.ElementValues.getResourceString("paymentNotice", commonVariables.PaymentMethodsXML)%>');
                    }
                }
            });
        </script>
    </div>
</body>
</html>
