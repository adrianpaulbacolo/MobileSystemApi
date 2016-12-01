<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BankTransfer.aspx.cs" Inherits="Withdrawal_BankTransfer" %>

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

            <h1 class="title" id="headerTitle"><%=commonCulture.ElementValues.getResourceString("withdrawal", commonVariables.LeftMenuXML)%></h1>
        </header>

        <div class="ui-content" role="main">
            <div class="wallet main-wallet">
                <uc:Wallet ID="uMainWallet" runat="server" />
            </div>

            <div class="toggle-list-box">
                <button class="toggle-list-btn btn-active" id="activeWithdrawalTabs"></button>
                <ul class="toggle-list hidden" id="withdrawalTabs">
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
                        <asp:TextBox ID="txtWithdrawAmount" runat="server" type="number" step="any" min="1" data-clear-btn="true" />
                    </li>

                    <li class="item item-select">
                        <asp:Label ID="lblBank" runat="server" AssociatedControlID="drpBank" />
                        <asp:DropDownList ID="drpBank" runat="server" data-corners="false" />
                    </li>

                    <li class="item item-select" id="divOtherBank" runat="server" style="display: none;">
                        <asp:Label ID="lblSecondBank" runat="server" AssociatedControlID="drpSecondaryBank" />
                        <asp:DropDownList ID="drpSecondaryBank" runat="server" data-corners="false">
                        </asp:DropDownList>
                    </li>
                    <li class="item item-select" id="divBankLocation" runat="server" style="display: none;">
                        <asp:Label ID="lblBankLocation" runat="server" AssociatedControlID="txtBankName" />
                        <span id="loader1" class="select-loader"></span>
                        <select id="drpBankLocation"></select>
                    </li>
                    <li class="item item-select" id="divBankNameSelection" runat="server" style="display: none;">
                        <asp:Label ID="lblBranch" runat="server" AssociatedControlID="txtBankName" />
                        <span id="loader2" class="select-loader"></span>
                        <select id="drpBankBranchList"></select>
                    </li>
                    <li class="item item-input" id="divBankName" style="display: none;">
                        <asp:Label ID="lblBankName" runat="server" AssociatedControlID="txtBankName" />
                        <asp:TextBox ID="txtBankName" runat="server" />
                    </li>
                    <li class="item item-input" id="divBankBranch" runat="server">
                        <asp:Label ID="lblBankBranch" runat="server" AssociatedControlID="txtBankBranch" />
                        <asp:TextBox ID="txtBankBranch" runat="server" />
                    </li>
                    <li class="item item-input" id="divAddress" runat="server">
                        <asp:Label ID="lblAddress" runat="server" AssociatedControlID="txtAddress" />
                        <asp:TextBox ID="txtAddress" runat="server" />
                    </li>
                    <li class="item item-input">
                        <asp:Label ID="lblAccountName" runat="server" AssociatedControlID="txtAccountName" />
                        <asp:TextBox ID="txtAccountName" runat="server" />
                    </li>
                    <li class="item item-input">
                        <asp:Label ID="lblAccountNumber" runat="server" AssociatedControlID="txtAccountNumber" />
                        <asp:TextBox ID="txtAccountNumber" runat="server" />
                    </li>
                    <li class="item row">
                        <div class="col">
                            <asp:Button data-theme="b" ID="btnSubmit" runat="server" CssClass="button-blue" OnClick="btnSubmit_Click" />
                        </div>
                    </li>
                </ul>


                <asp:HiddenField ID="hfBLId" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hfBBId" runat="server" ClientIDMode="Static" />
            </form>
        </div>

        <% if (commonCookie.CookieIsApp != "1")
           { %>
        <!--#include virtual="~/_static/navMenu.shtml" -->
        <% } %>

        <script type="text/javascript">

            var selectName = '<%=strdrpBank%>';

            $(document).ready(function () {
                window.w88Mobile.Gateways.DefaultPayments.Withdraw("<%=base.strCountryCode %>", "<%=base.strMemberID %>", '<%= commonCulture.ElementValues.getResourceString("paymentNotice", commonVariables.PaymentMethodsXML)%>', "<%=base.PaymentMethodId %>");

                $('#form1').submit(function (e) {
                    window.w88Mobile.FormValidator.disableSubmitButton('#btnSubmit');
                });

                <% if (string.Compare(commonCookie.CookieCurrency, "myr", true) == 0)
                   { %>
                $('#txtMyKad').mask('999999-99-9999');
                <% } %>

                var responseCode = '<%=strAlertCode%>';
                var responseMsg = '<%=strAlertMessage%>';
                if (responseCode.length > 0) {
                    switch (responseCode) {
                        case '-1':
                            alert(responseMsg);
                            window.w88Mobile.BankTransfer.ToogleBank($('#drpBank').val(), '<%= commonCookie.CookieCurrency.ToLower() %>', selectName);
                            break;
                        case '0':
                            alert(responseMsg);
                            window.location.replace('/Withdrawal/Default.aspx');
                            break;
                        default:
                            break;
                    }
                }

                if (sessionStorage.getItem("hfBLId") != null || sessionStorage.getItem("hfBBId") != null) {

                    if ($('#drpBank').val() != "-1") {
                        var blId = $('#<%=hfBLId.ClientID%>').val();
                        var bbId = $('#<%=hfBBId.ClientID%>').val();
                        window.w88Mobile.BankTransfer.ReloadValues(selectName, blId, bbId);
                        sessionStorage.removeItem("hfBLId");
                        sessionStorage.removeItem("hfBBId");
                    }
                }



                $('#drpBank').change(function () {
                    $('#<%=hfBLId.ClientID%>').val('');
                    $('#<%=hfBBId.ClientID%>').val('');
                    sessionStorage.removeItem("hfBLId");
                    sessionStorage.removeItem("hfBBId");
                    window.w88Mobile.BankTransfer.ToogleBank(this.value, '<%= commonCookie.CookieCurrency.ToLower() %>', selectName);
                });

                $('#drpSecondaryBank').change(function () {
                    $('#<%=hfBLId.ClientID%>').val('');
                    $('#<%=hfBBId.ClientID%>').val('');
                    sessionStorage.removeItem("hfBLId");
                    sessionStorage.removeItem("hfBBId");
                    window.w88Mobile.BankTransfer.ToogleSecondaryBank(this.value, selectName, $('#<%=hfBLId.ClientID%>').val());
                });

                $('#drpBankLocation').change(function () {
                    if (this.value != '-1') {

                        $('#<%=hfBBId.ClientID%>').val('');
                        sessionStorage.removeItem("hfBBId");

                        $('#<%=hfBLId.ClientID%>').val(this.value);
                        sessionStorage.setItem("hfBLId", this.value);

                        window.w88Mobile.BankTransfer.ToogleBankBranch(selectName, $('#<%=hfBLId.ClientID%>').val(), $('#<%=hfBBId.ClientID%>').val());
                        }
                });

                $('#drpBankBranchList').change(function () {
                    if (this.value != '-1') {
                        sessionStorage.setItem("hfBBId", this.value);
                        $('#<%=hfBBId.ClientID%>').val(this.value);
                    }
                });

            });
        </script>
    </div>
</body>
</html>
