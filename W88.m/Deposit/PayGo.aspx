<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayGo.aspx.cs" Inherits="Deposit_PayGo" %>

<%@ Register TagPrefix="uc" TagName="Wallet" Src="~/UserControls/MainWalletBalance.ascx" %>

<!DOCTYPE html>
<html>
<head>
    <title><%=string.Format("{0} {1}", commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML), commonCulture.ElementValues.getResourceString("dPayGo", commonVariables.PaymentMethodsXML))%></title>
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

            <h1 class="title"><%=string.Format("{0} - {1}", commonCulture.ElementValues.getResourceString("deposit", commonVariables.LeftMenuXML), commonCulture.ElementValues.getResourceString("dPayGo", commonVariables.PaymentMethodsXML))%></h1>
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
                    <li class="item item-input">
                        <asp:Label ID="lblDepositAmount" runat="server" AssociatedControlID="txtDepositAmount" />
                        <asp:TextBox ID="txtDepositAmount" runat="server" data-clear-btn="true" />
                    </li>
                    <li class="item item-select">
                        <asp:Label ID="lblSystemAccount" runat="server" AssociatedControlID="drpSystemAccount" />
                        <asp:DropDownList ID="drpSystemAccount" runat="server" data-corners="false" />
                    </li>
                    <li class="item item-input">
                        <asp:Label ID="lblReferenceId" runat="server" AssociatedControlID="txtReferenceId" />
                        <asp:TextBox ID="txtReferenceId" runat="server" data-clear-btn="true" />
                    </li>
                    <li class="item item-select" runat="server">
                        <div class="row">
                            <asp:Label ID="lblDepositDateTime" runat="server" AssociatedControlID="drpDepositDate" />
                        </div>
                        <div class="row">
                            <div class="col">
                                <asp:DropDownList ID="drpDepositDate" runat="server" />
                            </div>
                            <div class="col">
                                <asp:DropDownList ID="drpHour" runat="server" />
                            </div>
                            <div class="col">
                                <asp:DropDownList ID="drpMinute" runat="server" />
                            </div>
                        </div>
                    </li>
                    <li class="item item-input">
                        <asp:Label ID="lblAccountName" runat="server" AssociatedControlID="txtAccountName" />
                        <asp:TextBox ID="txtAccountName" runat="server" data-clear-btn="true" />
                    </li>
                    <li class="item item-input">
                        <asp:Label ID="lblAccountNumber" runat="server" AssociatedControlID="txtAccountNumber" />
                        <asp:TextBox ID="txtAccountNumber" runat="server" data-clear-btn="true" />
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
            $(document).ready(function () {
                window.w88Mobile.Gateways.PayGo.InitDeposit();

                $('#form1').submit(function (e) {
                    e.preventDefault();
                    window.w88Mobile.FormValidator.disableSubmitButton('#btnSubmit');

                    var depositDateTime = new Date($('#drpDepositDate').val());
                    depositDateTime.setHours($('#drpHour').val());
                    depositDateTime.setMinutes($('#drpMinute').val());

                    var data = {
                        Amount: $('#txtDepositAmount').val(),
                        DepositDateTime: depositDateTime.toLocaleDateString() + " " + depositDateTime.toLocaleTimeString(),
                        ReferenceId: $('#txtReferenceId').val(),
                        SystemBank: JSON.parse($('#drpSystemAccount').val()),
                        AccountName: $('#txtAccountName').val(),
                        AccountNumber: $('#txtAccountNumber').val()
                    };

                    window.w88Mobile.Gateways.PayGo.Deposit(data, function (response) {
                        switch (response.ResponseCode) {
                            case 1:
                                w88Mobile.Growl.shout("<p>" + response.ResponseMessage + "</p> <p>" + '<%=lblTransactionId%>' + ": " + response.ResponseData.TransactionId + "</p>");
                                $('#form1')[0].reset();
                                break;
                            default:
                                if (_.isArray(response.ResponseMessage)) {
                                    w88Mobile.Growl.shout(w88Mobile.Growl.bulletedList(response.ResponseMessage));
                                }
                                else {
                                    w88Mobile.Growl.shout(response.ResponseMessage);
                                }
                                break;
                        }
                    },
                    function () {
                        window.w88Mobile.FormValidator.enableSubmitButton('#btnSubmit');
                        GPInt.prototype.HideSplash();
                    });
                });
            });
        </script>
    </div>
</body>
</html>
