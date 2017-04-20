<%@ Page Title="" Language="C#" MasterPageFile="~/v2/MasterPages/Payment.master" AutoEventWireup="true" CodeFile="Pay1202112.aspx.cs" Inherits="v2_Deposit_Pay1202112" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PaymentMainContent" runat="Server">
    <div class="form-group pay-note">
        <span id="paymentNote"></span>
        <p id="paymentNoteContent"></p>
    </div>
    <div class="form-group">
        <asp:Label ID="lblCardType" runat="server" AssociatedControlID="drpCardType" />
        <asp:DropDownList ID="drpCardType" runat="server" CssClass="form-control" required data-selectequals="-1">
        </asp:DropDownList>
    </div>
    <div class="form-group">
        <asp:Label ID="lblAmount" runat="server" AssociatedControlID="drpAmount" />
        <asp:DropDownList ID="drpAmount" runat="server" CssClass="form-control" required data-selectequals="-1">
        </asp:DropDownList>
    </div>
    <div class="form-group">
        <asp:Label ID="lblCardNo" runat="server" AssociatedControlID="txtCardNo" />
        <asp:TextBox ID="txtCardNo" runat="server" CssClass="form-control" required/>
    </div>
    <div class="form-group">
        <asp:Label ID="lblPin" runat="server" AssociatedControlID="txtPin" />
        <asp:TextBox ID="txtPin" runat="server" CssClass="form-control" required/>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptsHolder" runat="Server">
    <script type="text/javascript" src="/_static/v2/assets/js/gateways/dinpaytopup.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>

    <script type="text/javascript">
        pubsub.subscribe('contentsLoaded', onContentsLoaded);

        function onContentsLoaded() {
            window.w88Mobile.Gateways.DinpayTopUp.init();
        }

        $(document).ready(function () {

            _w88_paymentSvcV2.setPaymentTabs("<%=base.PaymentType %>", "<%=base.PaymentMethodId %>");
            _w88_paymentSvcV2.DisplaySettings("<%=base.PaymentMethodId %>", { type: "<%=base.PaymentType %>" });

            window.w88Mobile.Gateways.DinpayTopUp.init("<%=base.PaymentMethodId %>");

            $('select[id$="drpCardType"]').change(function () {
                window.w88Mobile.Gateways.DinpayTopUp.setFee($('select[id$="drpCardType"]').val());
                window.w88Mobile.Gateways.DinpayTopUp.setDenom($('select[id$="drpCardType"]').val());
            });

            $('#form1').submit(function (e) {
                e.preventDefault();

                var data = {
                    Amount: $('select[id$="drpAmount"]').val(),
                    CardTypeText: $('select[id$="drpCardType"] selected').text(),
                    CardTypeValue: $('select[id$="drpCardType"]').val(),
                    CardNumber: $('input[id$="txtCardNo"]').val(),
                    CCV: $('input[id$="txtPin"]').val(),
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

