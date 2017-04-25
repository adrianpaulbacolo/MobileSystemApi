<%@ Page Title="" Language="C#" MasterPageFile="~/v2/MasterPages/Payment.master" AutoEventWireup="true" CodeFile="Pay120272.aspx.cs" Inherits="v2_Deposit_Pay120272" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PaymentMainContent" runat="Server">
    <div id="selectAtmWallet" class="form-group">
        <div class="row thin-gutter">
            <div class="col-xs-6">
                <label>
                    <input type="radio" name="depOption" value="EWALLET" />Ví Điện Tử Nội Địa</label>
            </div>
            <div class="col-xs-6">
                <label>
                    <input type="radio" name="depOption" value="ATM" />Thẻ ATM Nội Địa</label>
            </div>
        </div>
    </div>
    <div class="form-group pay-note">
        <span id="paymentNote"></span>
        <p id="paymentNoteContent"></p>
    </div>
    <div id="atm">
        <div class="form-group">
            <asp:Label ID="lblAmountAtm" runat="server" AssociatedControlID="txtAmountAtm" />
            <asp:TextBox ID="txtAmountAtm" runat="server" CssClass="form-control" required data-paylimit="0" data-numeric />
        </div>
        <div class="form-group">
            <asp:Label ID="lblBanks" runat="server" AssociatedControlID="drpBank" />
            <asp:DropDownList ID="drpBank" runat="server" CssClass="form-control"></asp:DropDownList>
        </div>
        <div class="form-group">
            <asp:Label ID="lblEmailAtm" runat="server" AssociatedControlID="txtEmailAtm" />
            <asp:TextBox ID="txtEmailAtm" runat="server" type="email" CssClass="form-control" />
        </div>
        <div class="form-group">
            <asp:Label ID="lblContact" runat="server" AssociatedControlID="txtContact" />
            <asp:TextBox ID="txtContact" runat="server" type="tel" CssClass="form-control" />
        </div>
    </div>
    <div id="ewallet">
        <div class="form-group">
            <asp:Label ID="lblAmountWallet" runat="server" AssociatedControlID="txtAmountWallet" />
            <asp:TextBox ID="txtAmountWallet" runat="server" CssClass="form-control" required data-paylimit="0" data-numeric />
        </div>
        <div class="form-group">
            <asp:Label ID="lblEmail" runat="server" AssociatedControlID="txtEmailWallet" />
            <asp:TextBox ID="txtEmailWallet" runat="server" type="email" CssClass="form-control" />
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptsHolder" runat="Server">
    <script type="text/javascript" src="/_static/v2/assets/js/gateways/baokim.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            var selectName = '<%=strdrpBank%>';

            _w88_paymentSvcV2.setPaymentTabs("<%=base.PaymentType %>", "<%=base.PaymentMethodId %>");
            _w88_paymentSvcV2.DisplaySettings("<%=base.PaymentMethodId %>", { type: "<%=base.PaymentType %>" });

            window.w88Mobile.Gateways.BaokimV2.init(selectName);

            $("input[name='depOption']").change(function (e) {
                e.preventDefault();
                var value = $(this).val();

                $('#btnSubmitPlacement').show();
                $('#selectAtmWallet').hide();

                if (value == "EWALLET") {
                    $('#atm').hide();
                    $('#ewallet').show();
                    window.w88Mobile.Gateways.BaokimV2.method = "EWALLET";
                    $("#paymentNote").text(_w88_contents.translate("LABEL_PAYMENT_NOTE"));
                    $("#notice").html(_w88_contents.translate("LABEL_PAYMENT_NOTE_EWALLET"));
                } else {
                    $('#ewallet').hide();
                    $('#atm').show();
                    $(this).hide();
                    window.w88Mobile.Gateways.BaokimV2.method = "ATM";
                    $("#paymentNote").text(_w88_contents.translate("LABEL_PAYMENT_NOTE"));
                    $("#notice").html(_w88_contents.translate("LABEL_PAYMENT_NOTE_ATM"));
                }
            });

            $('#form1').validator().on('submit', function (e) {

                if (!e.isDefaultPrevented()) {
                    e.preventDefault();
                    var data;

                    if (window.w88Mobile.Gateways.BaokimV2.method == "EWALLET") {
                        data = {
                            Method: window.w88Mobile.Gateways.BaokimV2.method,
                            Amount: $('input[id$="txtAmountWallet"]').autoNumeric('get'),
                            Email: $('input[id$="txtEmailWallet"]').val(),
                            MethodId: "<%=base.PaymentMethodId%>",
                            ThankYouPage: location.protocol + "//" + location.host + "/v2/Deposit/Pay120272ew.aspx?requestAmount=" + $('input[id$="txtAmountWallet"]').val()
                        };
                    } else {
                        data = {
                            Method: window.w88Mobile.Gateways.BaokimV2.method,
                            Amount: $('input[id$="txtAmountAtm"]').autoNumeric('get'),
                            Email: $('input[id$="txtEmailAtm"]').val(),
                            Phone: $('input[id$="txtContact"]').val(),
                            BankText: $('select[id$="drpBank"] option:selected').text(),
                            BankValue: $('select[id$="drpBank"]').val(),
                            MethodId: "<%=base.PaymentMethodId%>",
                            ThankYouPage: location.protocol + "//" + location.host + "/Deposit/Thankyou.aspx"
                        };
                    }

                    var params = decodeURIComponent($.param(data));
                    window.open(_w88_paymentSvcV2.payRoute + "?" + params, "<%=base.PageName%>");
                    _w88_paymentSvcV2.onTransactionCreated($(this));
                    return;
                }
            });

        });

    </script>
</asp:Content>

