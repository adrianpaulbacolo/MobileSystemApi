<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BaokimScratchCard.aspx.cs" Inherits="Deposit_BaokimScratchCard" %>

<%@ Register TagPrefix="uc" TagName="Wallet" Src="~/UserControls/MainWalletBalance.ascx" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/JS/modules/gateways/defaultpayments.js"></script>
    <script type="text/javascript" src="/_Static/JS/modules/gateways/baokimSc.js"></script>
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

            <h1 class="title" id="headerTitle"><%=commonCulture.ElementValues.getResourceString("deposit", commonVariables.LeftMenuXML)%></h1>
        </header>

        <div class="ui-content" role="main">
            <div class="wallet main-wallet">
                <uc:Wallet ID="uMainWallet" runat="server" />
            </div>

            <div class="toggle-list-box">
                <button class="toggle-list-btn btn-active" id="activeDepositTabs"></button>
                <ul class="toggle-list hidden" id="depositTabs">
                </ul>
            </div>

            <form class="form" id="form1" runat="server" data-ajax="false">
                <br>
                <ul class="list fixed-tablet-size">
                    <li class="row">
                        <div class="col">
                            <asp:Literal ID="lblMode" runat="server" />
                        </div>
                        <div class="col">
                            <asp:Literal ID="txtMode" runat="server" />
                        </div>
                    </li>
                    <li class="row">
                        <div class="col">
                            <asp:Literal ID="lblMinMaxLimit" runat="server" />
                        </div>
                        <div class="col">
                            <asp:Literal ID="txtMinMaxLimit" runat="server" />
                        </div>
                    </li>
                    <li class="row">
                        <div class="col">
                            <asp:Literal ID="lblDailyLimit" runat="server" />
                        </div>
                        <div class="col">
                            <asp:Literal ID="txtDailyLimit" runat="server" />
                        </div>
                    </li>
                    <li class="row">
                        <div class="col">
                            <asp:Literal ID="lblTotalAllowed" runat="server" />
                        </div>
                        <div class="col">
                            <asp:Literal ID="txtTotalAllowed" runat="server" />
                        </div>
                    </li>
                    <li class="item-text-wrap" runat="server">
                        <p id="IndicatorMsg" style="color: #ff0000"></p>
                    </li>
                    <li class="item item-select" runat="server">
                        <asp:Label ID="lblBanks" runat="server" AssociatedControlID="drpBanks" />
                        <asp:DropDownList ID="drpBanks" runat="server" data-corners="false">
                        </asp:DropDownList>
                    </li>
                    <li class="item item-select">
                        <asp:Label ID="lblDepositAmount" runat="server" AssociatedControlID="drpAmount" />
                        <asp:DropDownList ID="drpAmount" runat="server" data-corners="false">
                        </asp:DropDownList>
                    </li>
                    <li class="item item-input">
                        <asp:Label ID="lblPin" runat="server" AssociatedControlID="txtPin" />
                        <asp:TextBox ID="txtPin" runat="server" data-mini="true" data-clear-btn="true" />
                    </li>
                    <li class="item item-select">
                        <asp:Label ID="lblCardSerialNo" runat="server" AssociatedControlID="txtCardSerialNo" />
                        <asp:TextBox ID="txtCardSerialNo" runat="server" data-mini="true" data-clear-btn="true" />
                    </li>
                    <li class="item row">
                        <div class="col">
                            <asp:Button data-theme="b" ID="btnSubmit" runat="server" CssClass="button-blue" data-corners="false" CausesValidation="False" />
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
            $(document).ready(function () {
                window.w88Mobile.Gateways.DefaultPayments.Deposit("<%=base.strCountryCode %>", "<%=base.strMemberID %>", '<%= commonCulture.ElementValues.getResourceString("paymentNotice", commonVariables.PaymentMethodsXML)%>', "<%=base.PaymentMethodId %>");

                //window.w88Mobile.FormValidator.disableSubmitButton('#btnSubmit');

                window.w88Mobile.Gateways.BaokimScratchCard.Initialize();

                $('#btnSubmit').click(function (e) {
                    e.preventDefault();

                    var data = { Amount: $('#drpAmount').val(), CardNumber: $('#drpBanks').val(), CCV: $('#txtPin').val(), ReferenceId: $('#txtCardSerialNo').val() };

                    window.w88Mobile.Gateways.BaokimScratchCard.Deposit(data, function (response) {
                        switch (response.ResponseCode) {
                            case 1:
                                window.w88Mobile.FormValidator.enableSubmitButton('#btnSubmit');

                                w88Mobile.Growl.shout(response.ResponseMessage);
                                break;
                            default:
                                if (_.isArray(response.ResponseMessage))
                                    w88Mobile.Growl.shout(w88Mobile.Growl.bulletedList(response.ResponseMessage));
                                else
                                w88Mobile.Growl.shout(response.ResponseMessage);

                                break;
                        }
                    });
                });


                $('#drpBanks').change(function () {
                    window.w88Mobile.Gateways.BaokimScratchCard.SetFee($('#drpBanks').val());

                    window.w88Mobile.Gateways.BaokimScratchCard.SetDenom($('#drpBanks').val());
                });


            });
        </script>

    </div>
</body>
</html>
