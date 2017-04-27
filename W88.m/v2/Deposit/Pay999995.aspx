<%@ Page Title="" Language="C#" MasterPageFile="~/v2/MasterPages/Payment.master" AutoEventWireup="true" CodeFile="Pay999995.aspx.cs" Inherits="v2_Deposit_Pay999995" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PaymentMainContent" runat="Server">
    <div class="pay-note">
        <p><small  class="pay-note-label" id="paymentNote"></small><br>
            <small id="paymentNoteContent"></small>
        </p>
    </div>
    <div class="form-group">
        <asp:Label ID="lblAmount" runat="server" AssociatedControlID="txtAmount" />
        <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" required data-paylimit="0" data-numeric="1" />
    </div>
    <div class="form-group">
        <div class="checkbox checkbox-custom">
            <label>
                <input type="checkbox" type="checkbox" ID="isSwitchLine">
                <span ID="lblSwitchLine">Switch Line</span>
            </label>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptsHolder" runat="Server">
    <script type="text/javascript" src="/_static/v2/assets/js/gateways/wechat.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
    <script type="text/javascript" src="/_static/v2/assets/js/gateways/autoroute.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
    <script type="text/javascript" src="/_static/v2/assets/js/gateways/banner.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>

    <script type="text/javascript">
        pubsub.subscribe('contentsLoaded', onContentsLoaded);
        function onContentsLoaded() {
            window.w88Mobile.Gateways.WeChatV2.init();
        }

        $(document).ready(function () {
            _w88_paymentbanner.init("Wechat");

            _w88_paymentSvcV2.setPaymentTabs("<%=base.PaymentType %>", "<%=base.PaymentMethodId %>");
             _w88_paymentSvcV2.DisplaySettings("<%=base.PaymentMethodId %>", { type: "<%=base.PaymentType %>" });

             window.w88Mobile.Gateways.WeChatV2.init();

             $('#form1').validator().on('submit', function (e) {

                 if (!e.isDefaultPrevented()) {
                     e.preventDefault();
                     var data = {
                         Amount: $('input[id$="txtAmount"]').autoNumeric('get'),
                         ThankYouPage: location.protocol + "//" + location.host + "/Index",
                         SwitchLine: $('input[id$="isSwitchLine"]').is(':checked'),
                         MethodId: "<%=base.PaymentMethodId%>",
                         AutoRoute: true
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

