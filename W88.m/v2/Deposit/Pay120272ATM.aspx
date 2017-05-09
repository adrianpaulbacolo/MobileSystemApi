<%@ Page Title="" Language="C#" MasterPageFile="~/v2/MasterPages/Payment.master" AutoEventWireup="true" CodeFile="Pay120272ATM.aspx.cs" Inherits="v2_Deposit_Pay120272" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PaymentMainContent" runat="Server">
    <div class="form-group">
        <asp:Label ID="lblAmount" runat="server" AssociatedControlID="txtAmount" />
        <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" required data-paylimit="0" data-numeric />
    </div>
    <div class="form-group">
        <asp:Label ID="lblBank" runat="server" AssociatedControlID="drpBank" />
        <asp:DropDownList ID="drpBank" runat="server" CssClass="form-control"></asp:DropDownList>
    </div>
    <div class="form-group">
        <asp:Label ID="lblEmail" runat="server" AssociatedControlID="txtEmail" />
        <asp:TextBox ID="txtEmail" runat="server" type="email" CssClass="form-control" />
    </div>
    <div class="form-group">
        <asp:Label ID="lblContact" runat="server" AssociatedControlID="txtContact" />
        <asp:TextBox ID="txtContact" runat="server" type="tel" CssClass="form-control" />
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptsHolder" runat="Server">
    <script type="text/javascript" src="/_static/v2/assets/js/gateways/baokim.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            var method = "ATM";
                
            _w88_paymentSvcV2.setPaymentTabs("<%=base.PaymentType %>", "<%=base.PaymentMethodId %>", method);
            _w88_paymentSvcV2.DisplaySettings("<%=base.PaymentMethodId %>", { type: "<%=base.PaymentType %>" });

            window.w88Mobile.Gateways.BaokimV2.initATM(method);

            $('#form1').validator().on('submit', function (e) {

                if (!e.isDefaultPrevented()) {
                    e.preventDefault();
                    var data = {
                        Method: window.w88Mobile.Gateways.BaokimV2.method,
                        Amount: $('input[id$="txtAmount"]').autoNumeric('get'),
                        Email: $('input[id$="txtEmail"]').val(),
                        Phone: $('input[id$="txtContact"]').val(),
                        BankText: $('select[id$="drpBank"] option:selected').text(),
                        BankValue: $('select[id$="drpBank"]').val(),
                        MethodId: "<%=base.PaymentMethodId%>",
                        ThankYouPage: location.protocol + "//" + location.host + "/Deposit/Thankyou.aspx"
                    };

                    var params = decodeURIComponent($.param(data));
                    window.open(_w88_paymentSvcV2.payRoute + "?" + params, "<%=base.PageName%>");
                    _w88_paymentSvcV2.onTransactionCreated($(this));
                    return;
                }
            });

        });

    </script>
</asp:Content>

