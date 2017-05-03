<%@ Page Title="" Language="C#" MasterPageFile="~/v2/MasterPages/Payment.master" AutoEventWireup="true" CodeFile="Pay210602.aspx.cs" Inherits="v2_Withdrawal_Pay210602" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PaymentMainContent" runat="Server">
    <div class="form-group">
        <asp:Label ID="lblAmount" runat="server" AssociatedControlID="txtAmount" />
        <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" required data-paylimit="0" data-numeric />
    </div>
    <div class="form-group">
        <asp:Label ID="lblBank" runat="server" AssociatedControlID="drpBank" />
        <asp:DropDownList ID="drpBank" runat="server" CssClass="form-control" data-bankequals="-1" />
    </div>
    <div class="form-group" id="divOtherBank" runat="server" style="display: none;">
        <asp:Label ID="lblSecondBank" runat="server" AssociatedControlID="drpSecondaryBank" />
        <asp:DropDownList ID="drpSecondaryBank" runat="server" CssClass="form-control">
        </asp:DropDownList>
    </div>
    <div class="form-group location" id="divBankLocation" runat="server" style="display: none;">
        <asp:Label ID="lblBankLocation" runat="server" AssociatedControlID="txtBankName" />
        <asp:DropDownList ID="drpBankLocation" runat="server" CssClass="form-control">
        </asp:DropDownList>
    </div>
    <div class="form-group branch" id="divBankNameSelection" runat="server" style="display: none;">
        <asp:Label ID="lblBranch" runat="server" AssociatedControlID="txtBankName" />
        <asp:DropDownList ID="drpBankBranchList" runat="server" CssClass="form-control">
        </asp:DropDownList>
    </div>
    <div class="form-group" id="divBankName" style="display: none;">
        <asp:Label ID="lblBankName" runat="server" AssociatedControlID="txtBankName" />
        <asp:TextBox ID="txtBankName" runat="server" CssClass="form-control" />
    </div>
    <div class="form-group" id="divBankBranch" runat="server">
        <asp:Label ID="lblBankBranch" runat="server" AssociatedControlID="txtBankBranch" />
        <asp:TextBox ID="txtBankBranch" runat="server" CssClass="form-control" required />
    </div>
    <div class="form-group" id="divAddress" runat="server">
        <asp:Label ID="lblAddress" runat="server" AssociatedControlID="txtAddress" />
        <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" required />
    </div>
    <div class="form-group">
        <asp:Label ID="lblAccountName" runat="server" AssociatedControlID="txtAccountName" />
        <asp:TextBox ID="txtAccountName" runat="server" CssClass="form-control" required data-accountNo="0" />
    </div>
    <div class="form-group">
        <asp:Label ID="lblAccountNumber" runat="server" AssociatedControlID="txtAccountNumber" />
        <asp:TextBox ID="txtAccountNumber" runat="server" CssClass="form-control" type="number" required data-accountName="0" />
    </div>
    <div class="form-group" runat="server">
        <asp:Label ID="lblContact" runat="server" AssociatedControlID="drpCountryCode" />
        <div class="row thin-gutter">
            <div class="col-xs-6">
                <asp:DropDownList ID="drpCountryCode" runat="server" CssClass="form-control" data-selectequals="-1"/>
            </div>
            <div class="col-xs-6">
                <asp:TextBox ID="txtPhoneNumber" runat="server" CssClass="form-control" type="number" required />
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptsHolder" runat="Server">
    <script type="text/javascript" src="/_static/v2/assets/js/gateways/banktransfer.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>

    <script type="text/javascript">
        $(document).ready(function () {

            var currency = '<%= commonCookie.CookieCurrency.ToLower() %>';
            _w88_paymentSvcV2.setPaymentTabs("<%=base.PaymentType %>", "<%=base.PaymentMethodId %>");
            _w88_paymentSvcV2.DisplaySettings("<%=base.PaymentMethodId %>", { type: "<%=base.PaymentType %>" });

            window.w88Mobile.Gateways.BankTransferv2.initWidraw();
            window.w88Mobile.Gateways.BankTransferv2.loadSecondaryBankWidraw();

            $('select[id$="drpBank"]').change(function () {
                window.w88Mobile.Gateways.BankTransferv2.toggleBankWidraw(this.value, currency);
            });

            $('select[id$="drpSecondaryBank"]').change(function () {
                window.w88Mobile.Gateways.BankTransferv2.toggleSecondaryBankWidraw(this.value, $('select[id$="drpBankLocation"]').val());
            });

            $('select[id$="drpBankLocation"]').change(function () {
                if (this.value != '-1') {
                    window.w88Mobile.Gateways.BankTransferv2.toogleBankBranchWidraw(this.value, "");
                }
            });

            $('#form1').validator().on('submit', function (e) {

                if (!e.isDefaultPrevented()) {

                    e.preventDefault();

                    var data = {
                        Amount: $('input[id$="txtAmount"]').autoNumeric('get'),
                        AccountName: $('input[id$="txtAccountName"]').val(),
                        AccountNumber: $('input[id$="txtAccountNumber"]').val(),
                        CountryCode: $('select[id$="drpCountryCode"]').val(),
                        Phone: $('input[id$="txtPhoneNumber"]').val(),
                        Bank : {
                            Text: $('select[id$="drpBank"] option:selected').text(),
                            Value: $('select[id$="drpBank"]').val()
                        }
                    };

                    if ($('select[id$="drpBank"]').val() == 'OTHER') {
                        data.SecondBank =
                        {
                            Text: $('select[id$="drpSecondaryBank"] option:selected').text(),
                            Value: $('select[id$="drpSecondaryBank"]').val()
                        };
                    } else {
                        data.BankAddress = $('input[id$="txtAddress"]').val();
                        data.BankBranch = $('input[id$="txtBankBranch"]').val();
                    }

                    if ($('select[id$="drpSecondaryBank"]').val() != 'OTHER' && currency == 'vnd') {
                        data.BankAddressId = $('select[id$="drpBankLocation"]').val();
                        data.BankBranchId = $('select[id$="drpBankBranchList"]').val();
                    } else {
                        data.BankAddress = $('input[id$="txtAddress"]').val();
                        data.BankBranch = $('input[id$="txtBankBranch"]').val();
                        data.BankName = $('input[id$="txtBankName"]').val();
                    }

                    _w88_paymentSvcV2.CreateWithdraw(data, "<%=base.PaymentMethodId %>");
                }
            });
        });
    </script>
</asp:Content>

