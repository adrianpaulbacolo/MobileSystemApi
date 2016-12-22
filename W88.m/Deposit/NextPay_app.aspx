<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NextPay.aspx.cs" Inherits="Deposit_NextPay" %>
<%@ Register TagPrefix="uc" TagName="Wallet" Src="~/UserControls/MainWalletBalance.ascx" %>
<%@ Register TagPrefix="uc" TagName="AppFooterMenu" Src="~/UserControls/AppFooterMenu.ascx" %>

<!DOCTYPE html>
<html>
<head>
    <title><%=string.Format("{0} {1}", commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML), commonCulture.ElementValues.getResourceString("dNextPay", commonVariables.PaymentMethodsXML))%></title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/Js/Main.js"></script>
</head>
<body>
    <div data-role="page" data-theme="b">
        <header data-role="header" data-theme="b" data-position="fixed" id="header">
            <h1 class="title"><%=string.Format("{0} - {1}", commonCulture.ElementValues.getResourceString("deposit", commonVariables.LeftMenuXML), commonCulture.ElementValues.getResourceString("dNextPay", commonVariables.PaymentMethodsXML))%></h1>
        </header>

        <div class="ui-content" role="main">
            <div class="wallet main-wallet">
                <uc:Wallet id="uMainWallet" runat="server" />
            </div>

            <div data-role="navbar" id="depositTabs" runat="server">
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
                        <asp:Label ID="lblDepositAmount" runat="server" AssociatedControlID="txtDepositAmount" />
                        <asp:TextBox ID="txtDepositAmount" runat="server" type="number" step="any" min="1" data-clear-btn="true" />
                    </li>
                    <li class="item item-select">
                        <asp:Label ID="lblBank" runat="server" AssociatedControlID="bankDropDownList" />
                        <asp:DropDownList ID="bankDropDownList" runat="server">
                            <asp:ListItem Value="SCB">Siam Commercial Bank</asp:ListItem>
                            <asp:ListItem Value="BAY">Bank Of Ayudaha</asp:ListItem>
                            <asp:ListItem Value="KTB">Krungthai</asp:ListItem>
                        </asp:DropDownList>
                    </li>
                    <li class="item row">
                        <div class="col">
                            <asp:Button data-theme="b" ID="btnSubmit" runat="server" CssClass="button-blue" data-corners="false" OnClick="btnSubmit_Click" /></div>
                    </li>
                </ul>

                <uc:AppFooterMenu runat="server" ID="AppFooterMenu" />

            </form>
        </div>

        <script type="text/javascript">
            $('#form1').submit(function (e) {
                window.w88Mobile.FormValidator.disableSubmitButton('#btnSubmit');
            });
            $(function () {
                window.history.forward();

                if ($('#depositTabs li').length == 0) {
                    window.location.reload();
                }

                var responseCode = '<%=strAlertCode%>';
                var responseMsg = '<%=strAlertMessage%>';
                if (responseCode.length > 0 && responseMsg.length > 0) {
                    alert(responseMsg);
                }
            });
        </script>
    </div>

    <asp:Literal ID="litForm" runat="server"></asp:Literal>

</body>
</html>
