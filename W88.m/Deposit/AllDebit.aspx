<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AllDebit.aspx.cs" Inherits="Deposit_AllDebit" %>

<%@ Register TagPrefix="uc" TagName="Wallet" Src="~/UserControls/MainWalletBalance.ascx" %>

<!DOCTYPE html>
<html>
<head>
    <title></title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/JS/modules/gateways/defaultpayments.js"></script>
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
                    <li class="item item-input">
                        <asp:Label ID="lblDepositAmount" runat="server" AssociatedControlID="txtDepositAmount" />
                        <asp:TextBox ID="txtDepositAmount" runat="server" type="number" step="any" min="1" data-clear-btn="true" />
                    </li>
                    <li class="item item-select">
                        <asp:Label ID="lblCardType" runat="server" AssociatedControlID="ddlCardType" />
                        <asp:DropDownList ID="ddlCardType" runat="server">
                            <asp:ListItem Value="20751003">VISA</asp:ListItem>
                            <asp:ListItem Value="20751004">MASTER</asp:ListItem>
                        </asp:DropDownList>
                    </li>
                    <li class="item item-input">
                        <asp:Label ID="lblCardName" runat="server" AssociatedControlID="txtCardName" />
                        <asp:TextBox ID="txtCardName" runat="server" />
                    </li>
                    <li class="item item-input">
                        <asp:Label ID="lblCardNo" runat="server" AssociatedControlID="txtCardNo" />
                        <asp:TextBox ID="txtCardNo" runat="server" />
                    </li>
                    <li class="item item-select">
                        <asp:Label ID="lblExpiry" runat="server" AssociatedControlID="ddlExpiryMonth" />
                        <div class="row">
                            <div class="col">
                                <asp:DropDownList ID="ddlExpiryMonth" runat="server" />
                            </div>
                            <div class="col">
                                <asp:DropDownList ID="ddlExpiryYear" runat="server" />
                            </div>
                        </div>
                    </li>
                    <li class="item item-input">
                        <asp:Label ID="lblSecurityCode" runat="server" AssociatedControlID="txtSecurityCode" />
                        <asp:TextBox ID="txtSecurityCode" runat="server" />
                        <a href="#" id="ccvHelp"><%=strCCVHelp%></a>
                    </li>
                    <li class="item row">
                        <div class="col">
                            <asp:Button data-theme="b" ID="btnSubmit" runat="server" CssClass="button-blue" data-corners="false" OnClick="btnSubmit_Click" />
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

                $('#form1').submit(function (e) {
                    window.w88Mobile.FormValidator.disableSubmitButton('#btnSubmit');
                });

                var responseCode = '<%=strAlertCode%>';
                var responseMsg = '<%=strAlertMessage%>';
                if (responseCode.length > 0 && responseMsg.length > 0) {
                    alert(responseMsg);
                }

                $('#txtCardNo').mask('9999-9999-9999-9999');
                $('#txtSecurityCode').mask('999');

                $('#ccvHelp').on('click', function () {
                    $('#ccvModal').popup();
                    $('#ccvModal').popup('open');
                })

            });
        </script>
    </div>

    <asp:Literal ID="litForm" runat="server"></asp:Literal>

    <div id="ccvModal" data-role="popup" data-overlay-theme="b" data-theme="b" data-history="false">
        <a href="#" data-rel="back" class="close close-enhanced">&times;</a>
        <br>
        <h1 class="title">
            <img src="/_Static/Images/logo-<%=commonVariables.SelectedLanguageShort%>.png" width="220" class="logo img-responsive" alt="logo">
        </h1>
        <div class="padding">
            <div class="download-app padding">
                <span>
                    <img src="/_Static/Images/CVV-back.jpg" class="img-responsive" /></span>
            </div>
        </div>
    </div>
</body>
</html>
