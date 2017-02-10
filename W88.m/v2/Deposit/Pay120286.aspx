<%@ Page Title="" Language="C#" MasterPageFile="~/v2/MasterPages/Payment.master" AutoEventWireup="true" CodeFile="Pay120286.aspx.cs" Inherits="v2_Deposit_Pay120286" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PaymentMainContent" runat="Server">
    <ul class="list fixed-tablet-size">
        <li class="item-text-wrap" runat="server">
            <p id="IndicatorMsg" style="color: #ff0000"></p>
        </li>
        <li class="item item-select" runat="server">
            <asp:Label ID="lblBanks" runat="server" AssociatedControlID="drpBanks" />
            <asp:DropDownList ID="drpBanks" runat="server" data-corners="false">
            </asp:DropDownList>
        </li>
        <li class="item item-select">
            <asp:Label ID="lblDepositAmount" runat="server" AssociatedControlID="drpAmount" />
            <asp:DropDownList ID="drpAmount" runat="server" data-corners="false">
            </asp:DropDownList>
        </li>
        <li class="item item-input">
            <asp:Label ID="lblPin" runat="server" AssociatedControlID="txtPin" />
            <asp:TextBox ID="txtPin" runat="server" data-mini="true" data-clear-btn="true" />
        </li>
        <li class="item item-select">
            <asp:Label ID="lblCardSerialNo" runat="server" AssociatedControlID="txtCardSerialNo" />
            <asp:TextBox ID="txtCardSerialNo" runat="server" data-mini="true" data-clear-btn="true" />
        </li>
    </ul>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptsHolder" runat="Server">
    <script type="text/javascript" src="/_Static/JS/modules/gateways/baokimSc.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            var payments = new w88Mobile.Gateways.Payments("<%=base.PaymentMethodId %>");
            payments.init();

            window.w88Mobile.Gateways.DefaultPayments.Deposit("<%=base.strCountryCode %>", "<%=base.strMemberID %>", '<%= commonCulture.ElementValues.getResourceString("paymentNotice", commonVariables.PaymentMethodsXML)%>', "<%=base.PaymentMethodId %>");
            window.w88Mobile.Gateways.BaokimScratchCard.Initialize();

            $('#form1').submit(function (e) {
                window.w88Mobile.FormValidator.disableSubmitButton('button[id$="btnSubmit"]');
                e.preventDefault();
                var data = {
                    Amount: $('#<%=drpAmount.ClientID%>').val(),
                    CardNumber: $('#<%=drpBanks.ClientID%>').val(),
                    CCV: $('#<%=txtPin.ClientID%>').val(),
                    ReferenceId: $('#<%=txtCardSerialNo.ClientID%>').val()
                };
                payments.send(data, function (response) {
                    switch (response.ResponseCode) {
                        case 1:
                            w88Mobile.Growl.shout(response.ResponseMessage);
                            break;
                        default:
                            if (_.isArray(response.ResponseMessage))
                                w88Mobile.Growl.shout(w88Mobile.Growl.bulletedList(response.ResponseMessage));
                            else
                                w88Mobile.Growl.shout(response.ResponseMessage);
                            break;
                    }
                },
                    function () {
                        w88Mobile.FormValidator.enableSubmitButton('button[id$="btnSubmit"]');
                        GPINTMOBILE.HideSplash();
                    });
            });

            $('#<%=drpBanks.ClientID%>').change(function () {
                window.w88Mobile.Gateways.BaokimScratchCard.SetFee($('#<%=drpBanks.ClientID%>').val());
                window.w88Mobile.Gateways.BaokimScratchCard.SetDenom($('#<%=drpBanks.ClientID%>').val());
            });
        });
    </script>
</asp:Content>

