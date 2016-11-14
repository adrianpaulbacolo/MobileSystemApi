<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WingMoney.aspx.cs" Inherits="Withdrawal_WingMoney" %>

<%@ Register TagPrefix="uc" TagName="Wallet" Src="~/UserControls/MainWalletBalance.ascx" %>
<%@ Register Src="~/UserControls/AppFooterMenu.ascx" TagPrefix="uc" TagName="AppFooterMenu" %>

<!DOCTYPE html>
<html>
<head>
    <title><%=string.Format("{0} {1}", commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML), commonCulture.ElementValues.getResourceString("wWingMoney", commonVariables.PaymentMethodsXML))%></title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/Js/Main.js"></script>
    <script type="text/javascript" src="/_Static/JS/jquery.mask.min.js"></script>
</head>
<body>
    <div data-role="page" data-theme="b">
        <header data-role="header" data-theme="b" data-position="fixed" id="header">
            <h1 class="title"><%=string.Format("{0} - {1}", commonCulture.ElementValues.getResourceString("withdrawal", commonVariables.LeftMenuXML), commonCulture.ElementValues.getResourceString("wWingMoney", commonVariables.PaymentMethodsXML))%></h1>
        </header>

        <div class="ui-content" role="main">
            <div class="wallet main-wallet">
                <uc:Wallet ID="uMainWallet" runat="server" />
            </div>

            <div data-role="navbar">
                <ul id="withdrawalTabs" runat="server">
                </ul>
            </div>

            <form class="form" id="form1" runat="server" data-ajax="false">
                <br />
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
                        <asp:Label ID="lblWithdrawAmount" runat="server" AssociatedControlID="txtWithdrawAmount" />
                        <asp:TextBox ID="txtWithdrawAmount" runat="server" type="number" step="any" min="1" />
                    </li>
                    <li class="item item-input">
                        <asp:Label ID="lblAccountName" runat="server" AssociatedControlID="txtAccountName" />
                        <asp:TextBox ID="txtAccountName" runat="server" />
                    </li>
                    <li class="item item-input">
                        <asp:Label ID="lblAccountNumber" runat="server" AssociatedControlID="txtAccountNumber" />
                        <asp:TextBox ID="txtAccountNumber" runat="server" />
                    </li>
                    <!--
                    <li class="item item-input">
                        <asp:Label ID="lblMobile" runat="server" AssociatedControlID="txtMobile" Text="to" />
                        <asp:TextBox ID="txtMobile" runat="server" />
                    </li>
                    -->
                    <li class="row">
                        <div class="col">
                            <asp:Button data-theme="b" ID="btnSubmit" runat="server" CssClass="button-blue" OnClick="btnSubmit_Click" data-corners="false" />
                        </div>
                    </li>
                </ul>

                <uc:AppFooterMenu runat="server" ID="AppFooterMenu" />

            </form>
        </div>
        <!-- /content -->
        <script type="text/javascript">
            $(function () {

                if ($('#withdrawalTabs li').length == 0) {
                    window.location.reload();
                }

                var responseCode = '<%=strAlertCode%>';
                var responseMsg = '<%=strAlertMessage%>';
                if (responseCode.length > 0) {
                    switch (responseCode) {
                        case '-1':
                            alert(responseMsg);
                            break;
                        case '0':
                            alert(responseMsg);
                            window.location.replace('/Withdrawal/Default_app.aspx');
                            break;
                        default:
                            break;
                    }
                }
            });

            $('#form1').submit(function (e) {

                if ($('#txtWithdrawAmount').val().length == 0) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("Withdrawal/MissingWithdrawAmount", xeErrors)%>');
                    e.preventDefault();
                    return;
                }
                else if (isNaN($('#txtWithdrawAmount').val())) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("Withdrawal/MissingWithdrawAmount", xeErrors)%>');
                    e.preventDefault();
                    return;
                }
                else if ($('#txtReferenceId').val().length == 0) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("Deposit/MissingReferenceId", xeErrors)%>');
                    e.preventDefault();
                    return;
                }
                else if ($('#txtAccountName').val().length == 0) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("Withdrawal/MissingAccountName", xeErrors)%>');
                    e.preventDefault();
                    return;
                }
                else if ($('#txtAccountNumber').val().length == 0) {
                    if ($('#drpBank').val() == 'VIETIN') {
                        alert('<%=commonCulture.ElementValues.getResourceXPathString("Withdrawal/MissingAccountNumber", xeErrors)%>');
                        e.preventDefault();
                        return;
                    } else {
                        alert('<%=commonCulture.ElementValues.getResourceXPathString("Withdrawal/MissingAccountNumber", xeErrors)%>');
                        e.preventDefault();
                        return;
                    }
                } else {
                    window.w88Mobile.FormValidator.disableSubmitButton('#btnSubmit');
                }
                GPINTMOBILE.ShowSplash();
            });
        </script>
    </div>
</body>
</html>
