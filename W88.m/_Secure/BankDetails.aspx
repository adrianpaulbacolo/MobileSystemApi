<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BankDetails.aspx.cs" Inherits="_Secure_BankDetails" %>

<%@ Register TagPrefix="uc" TagName="Wallet" Src="~/UserControls/MainWalletBalance.ascx" %>

<!DOCTYPE html>
<html>
<head>
    <title><%=string.Format("{0}", commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML))%></title>
    <!--#include virtual="~/_static/head.inc" -->
</head>
<body>
    <div data-role="page" data-theme="b">
        <header data-role="header" data-theme="b" data-position="fixed" id="header">
            <a class="btn-clear ui-btn-left ui-btn" href="#divPanel" data-role="none" id="aMenu" data-load-ignore-splash="true">
                <i class="icon-navicon"></i>
            </a>
            <h1 class="title" id="h1title"></h1>
        </header>

        <div class="ui-content" role="main">
            <div class="wallet main-wallet">
                <uc:Wallet ID="uMainWallet" runat="server" />
            </div>

            <form class="form" id="form1" runat="server">
                <br />
                <ul class="list fixed-tablet-size">
                    <li class="item item-select">
                        <asp:Label ID="lblBank" runat="server" AssociatedControlID="drpBank" />
                        <asp:DropDownList ID="drpBank" runat="server" data-corners="false" />
                    </li>
                    <li class="item item-input" id="divBankName" style="display: none;">
                        <asp:Label ID="lblBankName" runat="server" AssociatedControlID="txtBankName" />
                        <asp:TextBox ID="txtBankName" runat="server" data-clear-btn="true" />
                    </li>
                    <li class="item item-input">
                        <asp:Label ID="lblBankBranch" runat="server" AssociatedControlID="txtBankBranch" />
                        <asp:TextBox ID="txtBankBranch" runat="server" data-clear-btn="true" />

                    </li>
                    <li class="item item-input">
                        <asp:Label ID="lblBankAddress" runat="server" AssociatedControlID="txtBankAddress" />
                        <asp:TextBox ID="txtBankAddress" runat="server" data-clear-btn="true" />

                    </li>
                    <li class="item item-input">
                        <asp:Label ID="lblAcctName" runat="server" AssociatedControlID="txtAccountName" />
                        <asp:TextBox ID="txtAccountName" runat="server" data-clear-btn="true" />

                    </li>
                    <li class="item item-input">
                        <asp:Label ID="lblAcctNumber" runat="server" AssociatedControlID="txtAccountNumber" />
                        <asp:TextBox ID="txtAccountNumber" runat="server" data-clear-btn="true" />

                    </li>
                    <li class="item item-checkbox">
                        <asp:Label ID="lblIsPreferred" runat="server" AssociatedControlID="isPreferred" />
                        <asp:CheckBox type="checkbox" ID="isPreferred" runat="server" />
                    </li>
                    <li class="item row">
                        <div class="col">
                            <asp:Button data-theme="b" ID="btnSubmit" runat="server" CssClass="button-blue" data-corners="false" />
                        </div>
                    </li>
                </ul>
            </form>
        </div>

        <!--#include virtual="~/_static/navMenu.shtml" -->
        <script type="text/javascript">
            $(document).ready(function () {
                window.w88Mobile.BankDetails.Initialize();

                $('#drpBank').change(function () {
                    window.w88Mobile.BankDetails.ToogleBank(this.value);
                });

                $('#form1').submit(function (e) {
                    e.preventDefault();
                    window.w88Mobile.FormValidator.disableSubmitButton('#btnSubmit');

                    var bank = JSON.parse($('#drpBank').val());

                    if (!_.isEqual(bank.Value.toUpperCase(), 'OTHER'))
                        $('#txtBankName').val('')

                    var data = {
                        Bank: bank
                       , BankName: $('#txtBankName').val()
                       , BankBranch: $('#txtBankBranch').val()
                       , BankAddress: $('#txtBankAddress').val()
                       , AccountName: $('#txtAccountName').val()
                       , AccountNumber: $('#txtAccountNumber').val()
                       , IsPreferred: $('#isPreferred').is(':checked')
                    }

                    window.w88Mobile.BankDetails.CreateBankDetails(data, function (response) {
                        if (_.isArray(response.ResponseMessage)) {
                            var message = "<ul>";
                            for (var i = 0; i < response.ResponseMessage.length; i++) {
                                message = message + "<li>" + response.ResponseMessage[i] + "</li>";
                            }

                            message = message + "</ul>";

                            w88Mobile.Growl.notif(message);
                        }
                        else {
                            w88Mobile.Growl.notif(response.ResponseMessage);
                        }

                        window.w88Mobile.FormValidator.enableSubmitButton('#btnSubmit');
                    });
                });
            });
    </script>
    </div>
</body>
</html>
