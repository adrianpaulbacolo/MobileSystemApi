<%@ Page Title="" Language="C#" MasterPageFile="~/v2/MasterPages/Payment.master" AutoEventWireup="true" CodeFile="Pay120286.aspx.cs" Inherits="v2_Deposit_Pay120286" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PaymentMainContent" runat="Server">
    <div class="form-group">
        <p id="IndicatorMsg" style="color: #ff0000"></p>
    </div>
    <div class="form-group">
        <asp:Label ID="lblBanks" runat="server" AssociatedControlID="drpBanks" />
        <asp:DropDownList ID="drpBanks" runat="server" CssClass="form-control">
        </asp:DropDownList>
    </div>
    <div class="form-group">
        <asp:Label ID="lblDepositAmount" runat="server" AssociatedControlID="drpAmount" />
        <asp:DropDownList ID="drpAmount" runat="server" CssClass="form-control">
        </asp:DropDownList>
    </div>
    <div class="form-group">
        <asp:Label ID="lblPin" runat="server" AssociatedControlID="txtPin" />
        <asp:TextBox ID="txtPin" runat="server" CssClass="form-control" />
    </div>
    <div class="form-group">
        <asp:Label ID="lblCardSerialNo" runat="server" AssociatedControlID="txtCardSerialNo" />
        <asp:TextBox ID="txtCardSerialNo" runat="server" CssClass="form-control" />
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptsHolder" runat="Server">
    <script type="text/javascript" src="/_Static/JS/modules/gateways/baokimSc.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>

    <script type="text/javascript">
        $(document).ready(function () {

            _w88_paymentSvcV2.setPaymentTabs("deposit", "<%=base.PaymentMethodId %>", "<%=base.strMemberID %>");
            _w88_paymentSvcV2.DisplaySettings("<%=base.PaymentMethodId %>", {
                type: "deposit",
                countryCode: "<%=base.strCountryCode %>",
                memberId: "<%=base.strMemberID %>",
                notice: '<%= commonCulture.ElementValues.getResourceString("paymentNotice", commonVariables.PaymentMethodsXML)%>'
            });

            window.w88Mobile.Gateways.BaokimScratchCardV2.init();

            $('#<%=drpBanks.ClientID%>').change(function () {
                window.w88Mobile.Gateways.BaokimScratchCardV2.setFee($('#<%=drpBanks.ClientID%>').val());
                window.w88Mobile.Gateways.BaokimScratchCardV2.setDenom($('#<%=drpBanks.ClientID%>').val());
            });

            $('#form1').submit(function (e) {
                e.preventDefault();

                var data = {
                    Amount: $('select[id$="drpAmount"]').val(),
                    CardNumber: $('select[id$="drpBanks"]').val(),
                    ReferenceId: $('[id$="txtCardSerialNo"]').val(),
                    CCV: $('[id$="txtPin"]').val(),
                    MethodId: "<%=base.PaymentMethodId%>"
                };

                var action = "/Deposit/Pay.aspx";
                var params = decodeURIComponent($.param(data));
                window.open(action + "?" + params, "<%=base.PageName%>");
                _w88_paymentSvcV2.onTransactionCreated($(this));
                return;
            });
        });
    </script>
</asp:Content>

