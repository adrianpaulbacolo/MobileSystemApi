<%@ Page Title="" Language="C#" MasterPageFile="~/v2/MasterPages/Payment.master" AutoEventWireup="true" CodeFile="Pay120218.aspx.cs" Inherits="v2_Deposit_Pay120218" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PaymentMainContent" runat="Server">
    <div class="form-group ali-pay-note">
        <span id="paymentNote"></span>
        <p id="paymentNoteContent"></p>
    </div>
    <div class="form-group">
        <asp:Label ID="lblDepositAmount" runat="server" AssociatedControlID="txtAmount" />
        <asp:TextBox ID="txtAmount" runat="server" type="number" step="any" min="1" CssClass="form-control" />
    </div>
    <div class="form-group">
        <asp:Label ID="lblBank" runat="server" AssociatedControlID="drpBank" />
        <asp:DropDownList ID="drpBank" runat="server" CssClass="form-control" />
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptsHolder" runat="Server">
    <script type="text/javascript" src="/_static/v2/assets/js/gateways/quickonline.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            _w88_paymentSvcV2.setPaymentTabs("<%=base.PaymentType %>", "<%=base.PaymentMethodId %>");
            _w88_paymentSvcV2.DisplaySettings("<%=base.PaymentMethodId %>", { type: "<%=base.PaymentType %>" });

            window.w88Mobile.Gateways.QuickOnlineV2.init("<%=base.PaymentMethodId %>", true);

            $('#form1').submit(function (e) {
                e.preventDefault();
                var data = {
                    Amount: $('input[id$="txtAmount"]').val(),
                    BankText: $('select[id$="drpBank"] option:selected').val(),
                    BankValue: $('select[id$="drpBank"]').val(),
                    ThankYouPage: location.protocol + "//" + location.host + "/Index",
                    MethodId: "<%=base.PaymentMethodId%>"
                };

                var params = decodeURIComponent($.param(data));
                window.open(_w88_paymentSvcV2.payRoute + "?" + params, "<%=base.PageName%>");
                _w88_paymentSvcV2.onTransactionCreated($(this));
                return;
            });

        });

    </script>
</asp:Content>

