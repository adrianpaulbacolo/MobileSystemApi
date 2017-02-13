<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Payments.master" AutoEventWireup="true" CodeFile="Aifu.aspx.cs" Inherits="Deposit_Aifu" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
    <ul class="list fixed-tablet-size">
        <li class="item-text-wrap ali-pay-note">
            <span id="paymentNote"></span>
            <p id="paymentNoteContent"></p>
        </li>
        <li class="item item-input">
            <asp:Label ID="lblAmount" runat="server" AssociatedControlID="txtAmount" />
            <asp:TextBox ID="txtAmount" runat="server" type="number" step="any" min="1" data-clear-btn="true" />
        </li>
    </ul>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsPlaceHolder1" runat="Server">
    <script type="text/javascript" src="/_Static/JS/modules/gateways/aifu.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
    <script type="text/javascript" src="/_Static/JS/modules/gateways/wechat.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
    <script type="text/javascript" src="/_Static/JS/modules/gateways/alipay.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
    <link href="/_Static/Css/payment.css?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>" rel="stylesheet" />

    <script type="text/javascript">
        $(document).ready(function () {
            $(document).ready(function () {
                _w88_paymentSvc.setPaymentTabs("deposit", "<%=base.PaymentMethodId %>", "<%=base.strMemberID %>");
                _w88_paymentSvc.DisplaySettings(
                    "<%=base.PaymentMethodId %>"
                   , {
                       type: "deposit"
                       , countryCode: "<%=base.strCountryCode %>"
                       , memberId: "<%=base.strMemberID %>"
                       , notice: '<%= commonCulture.ElementValues.getResourceString("paymentNotice", commonVariables.PaymentMethodsXML)%>'
                   });

                if ("<%=isWechat %>" == "True")
                    window.w88Mobile.Gateways.Wechat.Initialize();
                else
                    window.w88Mobile.Gateways.Alipay.Initialize();

                $('#form1').submit(function (e) {
                    e.preventDefault();
                    var data = {
                        Amount: $('input[id$="txtAmount"]').val(),
                        MethodId: "<%=base.PaymentMethodId%>"
                    };
                    var action = "/Deposit/Pay.aspx";
                    var params = decodeURIComponent($.param(data));
                    window.open(action + "?" + params, "<%=base.PageName%>");
                    _w88_paymentSvc.onTransactionCreated($(this));
                    return;
                });
            });
        });
    </script>
</asp:Content>
