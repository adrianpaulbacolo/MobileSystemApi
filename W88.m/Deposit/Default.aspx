<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Deposit_Default" %>

<!DOCTYPE html>
<html>
<head>
    <title><%=string.Format("{0} {1}", commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML), commonCulture.ElementValues.getResourceString("fastdeposit", commonVariables.LeftMenuXML))%></title>
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
            <h1 class="title"><%=string.Format("{0} - {1}", commonCulture.ElementValues.getResourceString("deposit", commonVariables.LeftMenuXML), commonCulture.ElementValues.getResourceString("fastdeposit", commonVariables.LeftMenuXML))%></h1>
        </header>

        <div class="ui-content" role="main">

            <div class="wallet main-wallet">
                <label class="label"><%=commonCulture.ElementValues.getResourceString("mainWallet", commonVariables.LeftMenuXML)%></label>
                <h2 class="value"><%=Session["Main"].ToString()%></h2>
                <small class="currency"><%=commonVariables.GetSessionVariable("CurrencyCode")%></small>
            </div>


            <div data-role="navbar">
                <ul id="depositTabs" runat="server">
                </ul>
            </div>

            <form class="form" id="form1" runat="server" data-ajax="false">
                <p>&nbsp;</p>
                <ul class="list fixed-tablet-size">
                    <li class="item item-input">
                        <asp:Label ID="lblDepositAmount" runat="server" AssociatedControlID="txtDepositAmount" Text="from" />
                        <asp:TextBox ID="txtDepositAmount" runat="server" type="number" step="any" min="1" data-clear-btn="true" />
                    </li>
                    <li class="item item-text-wrap">
                        <div class="div-limit"></div>
                        <div>
                            <asp:Literal ID="lblDailyLimit" runat="server" /></div>
                        <div>
                            <asp:Literal ID="lblTotalAllowed" runat="server" /></div>
                    </li>
                    <li class="item item-input">
                        <asp:Label ID="lblReferenceId" runat="server" AssociatedControlID="txtReferenceId" Text="from" />
                        <asp:TextBox ID="txtReferenceId" runat="server" data-clear-btn="true" />
                    </li>
                    <li class="item item-select">
                        <asp:Label ID="lblSystemAccount" runat="server" AssociatedControlID="drpSystemAccount" Text="to" />
                        <asp:DropDownList ID="drpSystemAccount" runat="server" data-corners="false" />
                    </li>
                    <li class="item item-select div-fastdeposit-depositdatetime" id="divDepositDateTime" runat="server">
                        <div class="row">
                            <div class="col">
                                <asp:DropDownList ID="drpDepositDate" runat="server" /></div>
                            <div class="col">
                                <asp:DropDownList ID="drpHour" runat="server" /></div>
                            <div class="col">
                                <asp:DropDownList ID="drpMinute" runat="server" /></div>
                        </div>
                    </li>
                    <li class="item item-select">
                        <asp:Label ID="lblDepositChannel" runat="server" AssociatedControlID="drpDepositChannel" Text="to" />
                        <asp:DropDownList ID="drpDepositChannel" runat="server" data-corners="false" />
                    </li>
                    <li class="item item-select">
                        <asp:Label ID="lblBank" runat="server" AssociatedControlID="drpBank" Text="to" />
                        <asp:DropDownList ID="drpBank" runat="server" data-corners="false" />
                    </li>
                    <li class="item item-input" id="divBankName" style="display: none;">
                        <asp:Label ID="lblBankName" runat="server" AssociatedControlID="txtBankName" Text="other" />
                        <asp:TextBox ID="txtBankName" runat="server" data-clear-btn="true" />
                    </li>
                    <li class="item item-input">
                        <asp:Label ID="lblAccountName" runat="server" AssociatedControlID="txtAccountName" Text="to" />
                        <asp:TextBox ID="txtAccountName" runat="server" data-clear-btn="true" />
                    </li>
                    <li class="item item-input">
                        <asp:Label ID="lblAccountNumber" runat="server" AssociatedControlID="txtAccountNumber" Text="to" />
                        <asp:TextBox ID="txtAccountNumber" runat="server" data-clear-btn="true" />
                    </li>
                    <li class="item row">
                        <div class="col">
                            <a href="/Funds.aspx" role="button" class="ui-btn btn-bordered" id="btnCancel" runat="server" data-ajax="false"><%=commonCulture.ElementValues.getResourceString("cancel", commonVariables.LeftMenuXML)%></a>
                        </div>
                        <div class="col">
                            <asp:Button data-theme="b" ID="btnSubmit" runat="server" Text="login" CssClass="button-blue" OnClick="btnSubmit_Click" />
                        </div>
                    </li>
                    <asp:HiddenField runat="server" ID="_repostcheckcode" />
                </ul>
            </form>
        </div>
        <!-- /content -->
        <!--#include virtual="~/_static/navMenu.shtml" -->
        <script type="text/javascript">
            $(function () {
                window.history.forward();

                if ('<%=strAlertCode%>'.length > 0) {
                    switch ('<%=strAlertCode%>') {
                        case '-1':
                            alert('<%=strAlertMessage%>');
                            break;
                        case '0':
                            alert('<%=strAlertMessage%>');
                            window.location.replace('/Deposit/Default.aspx');
                            break;
                        default:
                            break;
                    }
                }
            });

            $('#drpBank').change(function () {
                if ($(this).val() == 'OTHER') { $('#divBankName').show(); }
                else { $('#divBankName').hide(); }
            });

            $('#form1').submit(function (e) {
                if ($('#txtDepositAmount').val().length == 0) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("Deposit/MissingDepositAmount", xeErrors)%>');
                    e.preventDefault();
                    return;
                }
                else if (isNaN($('#txtDepositAmount').val())) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("Deposit/InvalidDepositAmount", xeErrors)%>');
                    e.preventDefault();
                    return;
                }
                else if ($('#drpSystemAccount').val() == '-1') {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("Deposit/SelectSystemAccount", xeErrors)%>');
                    e.preventDefault();
                    return;
                }
                else if ($('#drpDepositChannel').val() == '-1') {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("Deposit/SelectDepositChannel", xeErrors)%>');
                    e.preventDefault();
                    return;
                }
                else if ($('#drpBank').val() == '-1') {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("Deposit/SelectBank", xeErrors)%>');
                    e.preventDefault();
                    return;
                }
                else if ($('#drpBank').val() == 'OTHER') {
                    if ($('#txtBankName').val().length == 0) {
                        alert('<%=commonCulture.ElementValues.getResourceXPathString("Deposit/MissingBankName", xeErrors)%>');
                        e.preventDefault();
                        return;
                    }
                }
                if ($('#txtAccountName').val().length == 0) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("Deposit/MissingAccountName", xeErrors)%>');
                    e.preventDefault();
                    return;
                }
                else if ($('#txtAccountNumber').val().length == 0) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("Deposit/MissingAccountNumber", xeErrors)%>');
                        e.preventDefault();
                        return;
                    }

                GPINTMOBILE.ShowSplash();
            });
        </script>
    </div>
</body>
</html>
