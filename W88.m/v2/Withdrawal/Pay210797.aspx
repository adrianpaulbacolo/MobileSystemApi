<%@ Page Title="" Language="C#" MasterPageFile="~/v2/MasterPages/Payment.master" AutoEventWireup="true" CodeFile="Pay210797.aspx.cs" Inherits="v2_Withdrawal_Pay210797" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PaymentMainContent" runat="Server">
    <div class="form-group">
        <asp:Label ID="lblAmount" runat="server" AssociatedControlID="txtAmount" />
        <asp:TextBox ID="txtAmount" runat="server" type="number" step="any" min="1" CssClass="form-control" required data-paylimit="0" />
    </div>
    <div class="form-group">
        <asp:Label ID="lblAccountName" runat="server" AssociatedControlID="txtAccountName" />
        <asp:TextBox ID="txtAccountName" runat="server" CssClass="form-control" required data-accountName="0" />
    </div>
    <div class="form-group">
        <asp:Label ID="lblAccountNumber" runat="server" AssociatedControlID="txtAccountNumber" />
        <asp:TextBox ID="txtAccountNumber" runat="server" CssClass="form-control" required data-accountNo="0" />
    </div>
    <div class="form-group">
        <asp:Label ID="lblContact" runat="server" AssociatedControlID="txtContact" />
        <div class="row thin-gutter">
            <div class="col-xs-6">
                <asp:DropDownList ID="drpContactCountry" runat="server" CssClass="form-control" />
            </div>
            <div class="col-xs-6">
                <asp:TextBox ID="txtContact" runat="server" type="tel" CssClass="form-control" required />
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptsHolder" runat="Server">
    <script type="text/javascript" src="/_static/v2/assets/js/gateways/moneytransfer.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>

    <script type="text/javascript">
        $(document).ready(function() {

            _w88_paymentSvcV2.setPaymentTabs("<%=base.PaymentType %>", "<%=base.PaymentMethodId %>");
            _w88_paymentSvcV2.DisplaySettings("<%=base.PaymentMethodId %>", { type: "<%=base.PaymentType %>" });

            window.w88Mobile.Gateways.MoneyTransfer.countryphone();
            window.w88Mobile.Gateways.MoneyTransfer.init("<%=base.PaymentMethodId %>", false);

            $('#form1').validator().on('submit', function(e) {

                if (!e.isDefaultPrevented()) {

                    e.preventDefault();

                    var data = {
                        Amount: $('input[id$="txtAmount"]').val(),
                        AccountName: $('input[id$="txtAccountName"]').val(),
                        AccountNumber: $('input[id$="txtAccountNumber"]').val(),
                        CountryCode: {
                            Text: $('select[id$="drpContactCountry"] option:selected').text(),
                            Value: $('select[id$="drpContactCountry"]').val()
                        },
                        Phone: $('input[id$="txtContact"]').val()
                    };

                    _w88_paymentSvcV2.CreateWithdraw(data, "<%=base.PaymentMethodId %>");
                }
            });
        });
    </script>
</asp:Content>

