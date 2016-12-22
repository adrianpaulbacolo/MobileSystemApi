<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BaokimWallet.aspx.cs" Inherits="Deposit_BaokimWallet" %>

<%@ Register TagPrefix="uc" TagName="Wallet" Src="~/UserControls/MainWalletBalance.ascx" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title><%=string.Format("{0} {1}", commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML), commonCulture.ElementValues.getResourceString("dbaokim", commonVariables.PaymentMethodsXML))%></title>
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

            <h1 class="title"><%=string.Format("{0} - {1}", commonCulture.ElementValues.getResourceString("deposit", commonVariables.LeftMenuXML), commonCulture.ElementValues.getResourceString("dbaokim", commonVariables.PaymentMethodsXML))%></h1>
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
                        <asp:Label ID="lblEmail" runat="server" AssociatedControlID="txtEmail" />
                        <asp:TextBox ID="txtEmail" runat="server" data-mini="true" type="email" Enabled="False" />
                    </li>
                    <li class="item item-input">
                        <asp:Label ID="lblDepositAmount" runat="server" AssociatedControlID="txtDepositAmount" />
                        <asp:TextBox ID="txtDepositAmount" runat="server" type="number" Enabled="False" />
                    </li>
                    <li class="item item-input">
                        <asp:Label ID="lblOtp" runat="server" AssociatedControlID="txtOtp" required />
                        <asp:TextBox ID="txtOtp" runat="server" data-clear-btn="true" />
                    </li>
                    <li class="item item-select">
                        <p id="notice" style="color: #ff0000"></p>
                    </li>
                    <li class="item row ewallet atm">
                        <div class="col">
                            <asp:Button data-theme="b" ID="btnSubmit" runat="server" CssClass="button-blue" data-corners="false" CausesValidation="False" />
                        </div>
                    </li>
                </ul>

                <asp:HiddenField ID="hfAmount" runat="server" />
                <asp:HiddenField ID="hfEmail" runat="server" />
                <asp:HiddenField ID="hfAccepted" runat="server" />
                <asp:HiddenField ID="hfPhone" runat="server" />
                <asp:HiddenField ID="hfMessage" runat="server" />
                <asp:HiddenField ID="hfChkSum" runat="server" />
                <asp:HiddenField ID="TransactionId" runat="server" />
                <asp:HiddenField ID="MchtId" runat="server" />

            </form>
        </div>


        <% if (commonCookie.CookieIsApp != "1")
           { %>
        <!--#include virtual="~/_static/navMenu.shtml" -->
        <% } %>

        <script type="text/javascript">

            $(document).ready(function () {

                $("#txtDepositAmount").val($("#<%=hfAmount.ClientID%>").val());
                $("#txtEmail").val($("#<%=hfEmail.ClientID%>").val());

                window.w88Mobile.Gateways.Baokim.method = "EWALLETCB";
                window.w88Mobile.Gateways.Baokim.getTranslations();

                var data = {
                    Method: window.w88Mobile.Gateways.Baokim.method,
                    Amount: $("#<%=hfAmount.ClientID%>").val(),
                    Email: $("#<%=hfEmail.ClientID%>").val(),
                    Phone: $("#<%=hfPhone.ClientID%>").val(),
                    Accepted: $("#<%=hfAccepted.ClientID%>").val(),
                    Message: $("#<%=hfMessage.ClientID%>").val(),
                    CheckSum: $("#<%=hfChkSum.ClientID%>").val(),
                };

                window.w88Mobile.Gateways.Baokim.deposit(data, function (response) {
                    switch (response.ResponseCode) {
                        case 1:
                            window.w88Mobile.FormValidator.enableSubmitButton('#btnSubmit');

                            $("#<%=TransactionId.ClientID%>").val(response.ResponseData.InvId);
                            $("#<%=MchtId.ClientID%>").val(response.ResponseData.MerchantId);
                            break;
                        default:
                            w88Mobile.Growl.shout(response.ResponseMessage);
                            //window.location.replace('/Funds.aspx');
                            break;
                    }
                });

                $('#btnSubmit').click(function (e) {
                    e.preventDefault();

                    var walletData = {
                        MerchantId: $("#<%=MchtId.ClientID%>").val(),
                        Otp: $("#txtOtp").val()
                    };

                    window.w88Mobile.Gateways.Baokim.validateWallet(walletData, $("#<%=TransactionId.ClientID%>").val(), function (response) {
                        switch (response.ResponseCode) {
                            case 1:
                                window.w88Mobile.FormValidator.enableSubmitButton('#btnSubmit');

                                w88Mobile.Growl.shout(response.ResponseMessage);
                                window.location.replace('/Funds.aspx');
                                break;
                            default:
                                w88Mobile.Growl.shout(response.ResponseMessage);
                                break;
                        }
                    });
                });

            });
        </script>

    </div>
</body>
</html>
