<%@ Page Title="" Language="C#" MasterPageFile="~/v2/MasterPages/Payment.master" AutoEventWireup="true" CodeFile="Pay120244.aspx.cs" Inherits="v2_Deposit_Pay120244" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PaymentMainContent" runat="Server">
    <div class="form-group" id="amount">
        <asp:Label ID="lblAmount" runat="server" AssociatedControlID="txtAmount" />
        <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" required data-paylimit="0" data-numeric />
    </div>
    <div class="form-group" id="drpAmount" style="display: none;">
        <asp:Label ID="lbldrpDepositAmount" runat="server" AssociatedControlID="drpDepositAmount" />
        <asp:DropDownList ID="drpDepositAmount" runat="server" CssClass="form-control"/>
    </div>
    <div class="form-group">
        <asp:Label ID="lblBank" runat="server" AssociatedControlID="drpBank" />
        <asp:DropDownList ID="drpBank" runat="server" CssClass="form-control"/>
    </div>
    <div class="form-group" id="accountName">
        <asp:Label ID="lblAccountName" runat="server" AssociatedControlID="txtAccountName" />
        <asp:TextBox ID="txtAccountName" runat="server" CssClass="form-control" required data-accountName=""/>
        <asp:HiddenField ID="hfWCNickname" runat="server" ClientIDMode="Static" />
    </div>
    <div class="form-group" id="accountNo">
        <asp:Label ID="lblAccountNumber" runat="server" AssociatedControlID="txtAccountNumber" />
        <asp:TextBox ID="txtAccountNumber" runat="server" CssClass="form-control" required data-accountNo=""/>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptsHolder" runat="Server">
    <script type="text/javascript" src="/_static/v2/assets/js/gateways/daddypay.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            _w88_paymentSvcV2.setPaymentTabs("<%=base.PaymentType %>", "<%=base.PaymentMethodId %>");
            _w88_paymentSvcV2.DisplaySettings("<%=base.PaymentMethodId %>", { type: "<%=base.PaymentType %>" });

            window.w88Mobile.Gateways.DaddyPayV2.init("<%=base.PaymentMethodId %>", true);

            $('#form1').validator().on('submit', function (e) {

                if (!e.isDefaultPrevented()) {
                    e.preventDefault();
                    var data = {
                        Amount: $('select[id$="drpBank"] option:selected').val() == "40" ? $('select[id$="drpDepositAmount"] option:selected').val() : $('input[id$="txtAmount"]').autoNumeric('get'),
                        BankText: $('select[id$="drpBank"] option:selected').val(),
                        BankValue: $('select[id$="drpBank"]').val(),
                        AccountName: $('input[id$="txtAccountName"]').val(),
                        AccountNumber: $('input[id$="txtAccountNumber"]').val(),
                        ThankYouPage: location.protocol + "//" + location.host + "/Index",
                        MethodId: "<%=base.PaymentMethodId%>"
                    };

                    var params = decodeURIComponent($.param(data));
                    window.open(_w88_paymentSvcV2.payRoute + "?" + params, "<%=base.PageName%>");
                    _w88_paymentSvcV2.onTransactionCreated($(this));
                    return;
                }
            });

            $('#<%=drpBank.ClientID%>').change(function () {
                var bankId = this.value;
                _w88_daddypay.togglePayment(bankId);
            });

        });

    </script>

</asp:Content>

