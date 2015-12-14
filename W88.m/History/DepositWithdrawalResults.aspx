<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DepositWithdrawalResults.aspx.cs" Inherits="History_DepositWithdrawalResults" %>

<!DOCTYPE html>

<html>
<head>
    <title><%=commonCulture.ElementValues.getResourceString("depositwithdrawal", commonVariables.LeftMenuXML)%></title>
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
            <h1 class="title"><%=commonCulture.ElementValues.getResourceString("depositwithdrawal", commonVariables.LeftMenuXML)%></h1>
        </header>

        <div class="ui-content" role="main">
            
            <div class="wallet main-wallet">
                <label class="label">Main Wallet</label>
                <h2 class="value"><%=Session["Main"].ToString()%></h2>
                <small class="currency"><%=commonVariables.GetSessionVariable("CurrencyCode")%></small>
            </div>

            
        </div>
        <!-- /content -->
        <!--#include virtual="~/_static/footer.shtml" -->
        <!--#include virtual="~/_static/navMenu.shtml" -->
       
    </div>
</body>
</html>
