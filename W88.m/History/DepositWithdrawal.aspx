<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DepositWithdrawal.aspx.cs" Inherits="History_DepositWithdrawal" %>

<%@ Register TagPrefix="uc" TagName="Wallet" Src="~/UserControls/MainWalletBalance.ascx" %>
<%@ Register Src="~/UserControls/AppFooterMenu.ascx" TagPrefix="uc" TagName="AppFooterMenu" %>


<!DOCTYPE html>
<html>
<head>
    <title><%=string.Format("{0} {1}", commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML), commonCulture.ElementValues.getResourceString("depositwithdrawal", commonVariables.HistoryXML))%></title>
    <!--#include virtual="~/_static/head.inc" -->
</head>
<body>
    <div data-role="page" data-theme="b">
        <header data-role="header" data-theme="b" data-position="fixed" id="header">
            <a class="btn-clear ui-btn-left ui-btn" href="#divPanel" data-role="none" id="aMenu" data-load-ignore-splash="true">
                <% if (commonFunctions.isNativeAgent(Request))
                   { %>
                <i class="icon icon-back"></i>
                <% }
                   else
                   { %>
                <i class="icon icon-navicon"></i>
                <% } %>
            </a>
            <h1 class="title"><%=string.Format("{0} - {1}", commonCulture.ElementValues.getResourceString("history", commonVariables.HistoryXML), commonCulture.ElementValues.getResourceString("depositwithdrawal", commonVariables.HistoryXML))%></h1>
        </header>

        <div class="ui-content" role="main">
            
            <div class="wallet main-wallet">
                <uc:Wallet ID="uMainWallet" runat="server" />
            </div>

            <form class="form" id="form1" runat="server" data-ajax="false">
                <p>&nbsp;</p>
                <ul class="list fixed-tablet-size">
                    <li class="item item-input">
                        <asp:Label ID="lblDateFrom" runat="server" AssociatedControlID="txtDateFrom" />
                        <asp:TextBox ID="txtDateFrom" type="date" runat="server"></asp:TextBox>
                    </li>
                    <li class="item item-input">
                        <asp:Label ID="lblDateTo" runat="server" AssociatedControlID="txtDateTo" />
                        <asp:TextBox ID="txtDateTo" type="date" runat="server"></asp:TextBox>
                    </li>
                    <li class="item item-select">
                        <asp:Label ID="lblType" runat="server" AssociatedControlID="ddlType" />
                        <asp:DropDownList ID="ddlType" runat="server" data-corners="false" />
                    </li>
                    <li class="item item-select">
                        <asp:Label ID="lblStatus" runat="server" AssociatedControlID="ddlStatus" />
                        <asp:DropDownList ID="ddlStatus" runat="server" data-corners="false" />
                    </li>
                    <li class="item row" >
                        <div class="col" id="NonAppMenu">
                            <a href="/History" role="button" class="ui-btn btn-bordered" id="btnCancel" runat="server" data-ajax="false"><%=commonCulture.ElementValues.getResourceString("cancel", commonVariables.LeftMenuXML)%></a>
                        </div>
                        <div class="col">
                            <asp:Button data-theme="b" ID="btnSubmit" runat="server" CssClass="button-blue" OnClick="btnSubmit_Click" />
                        </div>
                    </li>
                    
                    <uc:AppFooterMenu runat="server" ID="AppFooterMenu" />
                </ul>
            </form>
        </div>
        <!-- /content -->
        <!--#include virtual="~/_static/navMenu.shtml" -->

        <script type="text/javascript">

            $('#<%= txtDateFrom.ClientID %>').on('focusout', function () {

                $('#<%= txtDateTo.ClientID %>').val("");
                $('#<%= txtDateTo.ClientID %>').attr("min", $('#<%= txtDateFrom.ClientID %>').val());

                var date = new Date($('#<%= txtDateFrom.ClientID %>').val());
                var maxDays = parseInt(90);
                date.setDate(date.getDate() + maxDays);

                var month = date.getMonth() + 1;
                var day = date.getDate();

                var maxDate = date.getFullYear() + '-' +
                    (month < 10 ? '0' : '') + month + '-' +
                    (day < 10 ? '0' : '') + day;

                $('#<%= txtDateTo.ClientID %>').attr("max", maxDate);
            });

            $('#form1').submit(function (e) {
                if ($('#<%= txtDateTo.ClientID %>').val().length == 0) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("Deposit/InvalidDateTime", commonVariables.ErrorsXML)%>');
                    e.preventDefault();
                    return;
                }

                if ($('#<%= txtDateFrom.ClientID %>').val().length == 0) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("Deposit/InvalidDateTime", commonVariables.ErrorsXML)%>');
                    e.preventDefault();
                    return;
                }
                GPINTMOBILE.ShowSplash();
            });
        </script>
    </div>
</body>
</html>
