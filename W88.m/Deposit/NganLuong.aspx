<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NganLuong.aspx.cs" Inherits="Deposit_NganLuong" %>

<%@ Register TagPrefix="uc" TagName="Wallet" Src="~/UserControls/MainWalletBalance.ascx" %>

<!DOCTYPE html>
<html>
<head>
    <title><%=string.Format("{0} {1}", commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML), commonCulture.ElementValues.getResourceString("dNganLuong", commonVariables.PaymentMethodsXML))%></title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/Js/Main.js"></script>
</head>
<body>
    <div data-role="page" data-theme="b">
        <header data-role="header" data-theme="b" data-position="fixed" id="header">
            <% if (commonCookie.CookieIsApp != "1")
               { %>
            <a class="btn-clear ui-btn-left ui-btn" href="#divPanel" data-role="none" id="aMenu" data-load-ignore-splash="true">
                <i class="icon-navicon"></i>
            </a>
            <% } %>

            <h1 class="title"><%=string.Format("{0} - {1}", commonCulture.ElementValues.getResourceString("deposit", commonVariables.LeftMenuXML), commonCulture.ElementValues.getResourceString("dNganLuong", commonVariables.PaymentMethodsXML))%></h1>
        </header>

        <div class="ui-content" role="main">
            <div class="wallet main-wallet">
                <uc:Wallet ID="uMainWallet" runat="server" />
            </div>

            <div data-role="navbar">
                <ul id="depositTabs" runat="server">
                </ul>
            </div>

            <form class="form" id="form1" runat="server" data-ajax="false">
                <br>
                <ul class="list fixed-tablet-size">
                    <li class="item-text-wrap">
                         <asp:Label ID="lblMessage" runat="server" />
                    </li>
                    <li class="item row">
                        <div class="col">
                            <asp:Button data-theme="b" ID="btnSubmit" runat="server" CssClass="button-blue" data-corners="false" />
                        </div>
                    </li>
                </ul>
            </form>
        </div>

         <% if (commonCookie.CookieIsApp != "1")
               { %>
            <!--#include virtual="~/_static/navMenu.shtml" -->
            <% } %>

        <script type="text/javascript">
            $('#form1').submit(function (e) {
                window.w88Mobile.FormValidator.disableSubmitButton('#btnSubmit');

                window.w88Mobile.Gateways.NganLuong.deposit("", function (response) {
                    switch (response.ResponseCode) {
                        case 1:
                            window.open(response.ResponseData.VendorRedirectionUrl);
                            break;
                        default:
                            w88Mobile.Growl.shout(response.ResponseMessage);
                            break;
                    }
                },
                function () { console.log("Error connecting to api"); },
                function () {
                    window.w88Mobile.FormValidator.enableSubmitButton('#btnSubmit');
                    GPINTMOBILE.HideSplash();
                });

            });

        </script>
    </div>
</body>
</html>
