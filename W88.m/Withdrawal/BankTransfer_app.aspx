<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Withdrawal_Default" %>

<!DOCTYPE html>
<html>
<head>
    <title><%=string.Format("{0} {1}", commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML), commonCulture.ElementValues.getResourceString("wBankTransfer", commonVariables.PaymentMethodsXML))%></title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/Js/Main.js"></script>
    <script type="text/javascript" src="/_Static/JS/jquery.mask.min.js"></script>
</head>
<body>
    <!--#include virtual="~/_static/splash.shtml" -->
    <div data-role="page" data-theme="b">
        <header data-role="header" data-theme="b" data-position="fixed" id="header">
            <a class="btn-clear ui-btn-left ui-btn" href="#divPanel" data-role="none" id="aMenu" data-load-ignore-splash="true">
                <i class="icon-navicon"></i>
            </a>
            <h1 class="title"><%=string.Format("{0} - {1}", commonCulture.ElementValues.getResourceString("withdrawal", commonVariables.LeftMenuXML), commonCulture.ElementValues.getResourceString("wBankTransfer", commonVariables.PaymentMethodsXML))%></h1>
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
                            <div><asp:Literal ID="lblDailyLimit" runat="server" /></div>
                            <div><asp:Literal ID="lblTotalAllowed" runat="server" /></div>
                        </div>
                    </li>
                    <li class="item item-select">
                        <asp:Label ID="lblBank" runat="server" AssociatedControlID="drpBank" Text="to" />
                        <asp:DropDownList ID="drpBank" runat="server" data-corners="false" />
                    </li>
                    <li class="item item-input" id="divBankName" style="display:none;">
                        <asp:Label ID="lblBankName" runat="server" AssociatedControlID="txtBankName" Text="other" />
                        <asp:TextBox ID="txtBankName" runat="server" />
                    </li>
                    <li class="item item-input" id="divBankBranch" runat="server">
                        <asp:Label ID="lblBankBranch" runat="server" AssociatedControlID="txtBankBranch" Text="other" />
                        <asp:TextBox ID="txtBankBranch" runat="server" />
                    </li>
                    <li class="item item-input" id="divAddress" runat="server">
                        <asp:Label ID="lblAddress" runat="server" AssociatedControlID="txtAddress" Text="other" />
                        <asp:TextBox ID="txtAddress" runat="server" />
                    </li>
                    <li class="item item-input">
                        <asp:Label ID="lblAccountName" runat="server" AssociatedControlID="txtAccountName" Text="to" />
                        <asp:TextBox ID="txtAccountName" runat="server" />
                    </li>
                    <li class="item item-input">
                        <asp:Label ID="lblAccountNumber" runat="server" AssociatedControlID="txtAccountNumber" Text="to" />
                        <asp:TextBox ID="txtAccountNumber" runat="server" />
                    </li>
                    <% if (string.Compare(commonVariables.GetSessionVariable("CurrencyCode"), "myr", true) == 0) { %>
                    <li class="item item-input">
                        <asp:Label ID="lblMyKad" runat="server" AssociatedControlID="txtMyKad" Text="to" />
                        <asp:TextBox ID="txtMyKad" runat="server" />
                    </li>
                    <% } %>
                    <!--
                    <li class="item item-input">
                        <asp:Label ID="lblMobile" runat="server" AssociatedControlID="txtMobile" Text="to" />
                        <asp:TextBox ID="txtMobile" runat="server" />
                    </li>
                    -->
                    <li class="row">
                        <div class="col"><asp:Button data-theme="b" ID="btnSubmit" runat="server" Text="login" CssClass="button-blue" OnClick="btnSubmit_Click" data-corners="false" /></div>
                    </li>
                    <asp:HiddenField runat="server" ID="_repostcheckcode" />
                </ul>

                <div class="row">
                    <div class="col">
                        <input type="button" data-theme="b" onclick="location.href = '/Deposit/Default_app.aspx';" value="Deposit" class="ui-btn btn-bordered" />
                    </div>
                    <div class="col">
                        <input type="button" data-theme="b" onclick="location.href = '/FundTransfer/FundTransfer.aspx';" value="Fund Transfer" class="ui-btn btn-bordered" />
                    </div>
                </div>

            </form>
        </div>

        <!--#include virtual="~/_static/navMenu.shtml" -->
        <script type="text/javascript">
            //$(document).ready(function () { });
            $(function () {
                <% if (string.Compare(commonVariables.GetSessionVariable("CurrencyCode"), "myr", true) == 0) { %>
                $('#txtMyKad').mask('999999-99-9999');
                <% } %>

                var strMethodsUnavailable = '<%=strMethodsUnAvailable%>';

                if (strMethodsUnavailable.length > 0) {
                    var arrMethodsUnavailable = strMethodsUnavailable.split('|');
                    for (var intCount = 0 ; intCount < arrMethodsUnavailable.length; intCount++) {
                        var strMethodId = arrMethodsUnavailable[intCount].toString();
                        if (strMethodId == '<%=Convert.ToInt32(commonVariables.WithdrawalMethod.BankTransfer)%>') { document.location.assign('/Index'); }
                        $('#w' + strMethodId + ' > a').attr('href', 'javascript:void(0)').html('&nbsp;').click(false);
                    }
                }

                if ('<%=strAlertCode%>'.length > 0) {
                    switch ('<%=strAlertCode%>') {
                        case '-1':
                            alert('<%=strAlertMessage%>');
                            break;
                        case '0':
                            alert('<%=strAlertMessage%>');
                            window.location.replace('/Withdrawal/Withrawal.aspx?source=app');
                            break;
                        default:
                            break;
                    }
                }
            });

            $('#drpBank').change(function () {
                $('#divBankName').hide();

                if ($(this).val() == 'OTHER') {
                    $('#divBankName').show();
                }
            });

            $('#form1').submit(function (e) {

                if ($('#txtWithdrawAmount').val().length == 0) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("Withdrawal/MissingWithdrawAmount", xeErrors)%>');
                    e.preventDefault();
                    return;
                }
                else if (isNaN($('#txtWithdrawAmount').val()))
                {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("Withdrawal/MissingWithdrawAmount", xeErrors)%>');
                    e.preventDefault();
                    return;
                }
                else if ($('#drpBank').val() == '-1') {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("Withdrawal/SelectBank", xeErrors)%>');
                    e.preventDefault();
                    return;
                }
                if ($('#txtBankBranch').val().length == 0 && '<%=commonVariables.GetSessionVariable("CurrencyCode").ToUpper()%>' != 'KRW') {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("Withdrawal/MissingBankBranch", xeErrors)%>');
                    e.preventDefault();
                    return;
                }
                else if ($('#txtAddress').val().length == 0 && '<%=commonVariables.GetSessionVariable("CurrencyCode").ToUpper()%>' != 'KRW') {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("Withdrawal/MissingBankAddress", xeErrors)%>');
                    e.preventDefault();
                    return;
                } else if ($('#txtAccountName').val().length == 0) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("Withdrawal/MissingAccountName", xeErrors)%>');
                    e.preventDefault();
                    return;
                }
                else if ($('#txtAccountNumber').val().length == 0) {

                    if ($('#drpBank').val() == 'VIETIN') {
                        //if ($('#txtAccountNumber').val().length != 16 || isNaN($('#txtAccountNumber').val())) {
                            alert('<%=commonCulture.ElementValues.getResourceXPathString("Withdrawal/InvalidAccountNumber", xeErrors)%>');
                            e.preventDefault();
                            return;
                        //}
                    } else {
                        alert('<%=commonCulture.ElementValues.getResourceXPathString("Withdrawal/MissingAccountNumber", xeErrors)%>');
                        e.preventDefault();
                        return;
                    }
                }
                else if ($('#drpBank').val() == 'OTHER') {
                    if ($('#txtBankName').val().length == 0) {
                        alert('<%=commonCulture.ElementValues.getResourceXPathString("Withdrawal/MissingBankName", xeErrors)%>');
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
