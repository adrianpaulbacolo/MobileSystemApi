<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WingMoney.aspx.cs" Inherits="Withdrawal_WingMoney" %>

<!DOCTYPE html>
<html>
<head>
    <title><%=string.Format("{0} {1}", commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML), commonCulture.ElementValues.getResourceString("wWingMoney", commonVariables.PaymentMethodsXML))%></title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/Js/Main.js"></script>
    <script type="text/javascript" src="/_Static/JS/jquery.mask.min.js"></script>
</head>
<body>
    <!--#include virtual="~/_static/splash.shtml" -->
    <div data-role="page" data-theme="b">
        <header data-role="header" data-theme="b" data-position="fixed" id="header">
            <h1 class="title"><%=string.Format("{0} - {1}", commonCulture.ElementValues.getResourceString("withdrawal", commonVariables.LeftMenuXML), commonCulture.ElementValues.getResourceString("wWingMoney", commonVariables.PaymentMethodsXML))%></h1>
        </header>

        <div class="ui-content" role="main">
            <div class="wallet main-wallet">
                <label class="label"><%=commonCulture.ElementValues.getResourceString("mainWallet", commonVariables.LeftMenuXML)%></label>
                <h2 class="value"><%=Session["Main"].ToString()%></h2>
                <small class="currency"><%=commonVariables.GetSessionVariable("CurrencyCode")%></small>
            </div>

            <div data-role="navbar">
                <ul id="withdrawalTabs" runat="server">
                </ul>
            </div>

            <form class="form" id="form1" runat="server" data-ajax="false">
                <br />
                <ul class="list fixed-tablet-size">
                    <li class="item item-input">
                        <asp:Label ID="lblWithdrawAmount" runat="server" AssociatedControlID="txtWithdrawAmount" Text="from" />
                        <asp:TextBox ID="txtWithdrawAmount" runat="server" type="number" step="any" min="1" />
                    </li>
                    <li class="item item-text-wrap">
                        <div class="div-limit">
                            <div>
                                <asp:Literal ID="lblDailyLimit" runat="server" /></div>
                            <div>
                                <asp:Literal ID="lblTotalAllowed" runat="server" /></div>
                        </div>
                    </li>
                    <li class="item item-input">
                        <asp:Label ID="lblAccountName" runat="server" AssociatedControlID="txtAccountName" Text="to" />
                        <asp:TextBox ID="txtAccountName" runat="server" />
                    </li>
                    <li class="item item-input">
                        <asp:Label ID="lblAccountNumber" runat="server" AssociatedControlID="txtAccountNumber" Text="to" />
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
                            <asp:Button data-theme="b" ID="btnSubmit" runat="server" Text="login" CssClass="button-blue" OnClick="btnSubmit_Click" data-corners="false" /></div>
                    </li>
                    <asp:HiddenField runat="server" ID="_repostcheckcode" />
                </ul>

                <div class="row">
                    <div class="col">
                        <input type="button" data-theme="b" onclick="location.href = '/Deposit/Default_app.aspx';" value="<%=commonCulture.ElementValues.getResourceString("deposit", commonVariables.LeftMenuXML)%>" class="button-blue" data-corners="false" />
                    </div>
                    <div class="col">
                        <input type="button" data-theme="b" onclick="location.href = '/FundTransfer/FundTransfer.aspx';" value="<%=commonCulture.ElementValues.getResourceString("fundTransfer", commonVariables.LeftMenuXML)%>" class="button-blue" data-corners="false" />
                    </div>
                </div>

            </form>
        </div>
        <!-- /content -->
        <script type="text/javascript">
            $(function () {
                if ('<%=strAlertCode%>'.length > 0) {
                    switch ('<%=strAlertCode%>') {
                        case '-1':
                            alert('<%=strAlertMessage%>');
                            break;
                        case '0':
                            alert('<%=strAlertMessage%>');
                            window.location.replace('/Withdrawal/WingMoney_app.aspx');
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
                        //if ($('#txtAccountNumber').val().length != 16 || isNaN($('#txtAccountNumber').val())) {
                        alert('<%=commonCulture.ElementValues.getResourceXPathString("Withdrawal/MissingAccountNumber", xeErrors)%>');
                        e.preventDefault();
                        return;
                        //}
                    } else {
                        alert('<%=commonCulture.ElementValues.getResourceXPathString("Withdrawal/MissingAccountNumber", xeErrors)%>');
                        e.preventDefault();
                        return;
                    }
                }
                GPINTMOBILE.ShowSplash();
            });
        </script>
    </div>
</body>
</html>
