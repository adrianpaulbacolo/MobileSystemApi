<%@ Page Language="C#" MasterPageFile="~/MasterPages/Payments.master" AutoEventWireup="true" CodeFile="SDAPay.aspx.cs" Inherits="Deposit_SDAPay" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
    <ul class="list fixed-tablet-size">
        <li class="item-text-wrap">
            <asp:Label ID="lblIndicatorMessage" runat="server" />
        </li>
        <li class="item item-input">
            <asp:Label ID="lblAmount" runat="server" AssociatedControlID="txtAmount" />
            <asp:TextBox ID="txtAmount" runat="server" type="number" step="any" min="1" data-clear-btn="true" />
        </li>
        <li class="item item-select" hidden>
            <asp:Label ID="lblBank" runat="server" AssociatedControlID="drpBank" />
            <asp:DropDownList ID="drpBank" runat="server" data-corners="false" />
        </li>
    </ul>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsPlaceHolder1" runat="Server">

    <script type="text/javascript">
        $(document).ready(function () {
            var payments = new w88Mobile.Gateways.Payments("<%=base.PaymentMethodId %>");

            payments.init();

            window.w88Mobile.Gateways.DefaultPayments.Deposit("<%=base.strCountryCode %>", "<%=base.strMemberID %>", '<%= commonCulture.ElementValues.getResourceString("paymentNotice", commonVariables.PaymentMethodsXML)%>', "<%=base.PaymentMethodId %>");

            $('#form1').submit(function (e) {
                e.preventDefault();
                window.w88Mobile.FormValidator.disableSubmitButton('#ContentPlaceHolder1_btnSubmit');

                var data = {
                    Amount: $('#<%=txtAmount.ClientID%>').val(),
                }

                payments.send(data, function (response) {
                    switch (response.ResponseCode) {
                        case 1:
                            w88Mobile.Growl.shout("<p>" + response.ResponseMessage + "</p> <p>" + '<%=lblTransactionId%>' + ": " + response.ResponseData.TransactionId + "</p>", function(){
                                window.location.replace('SDAPay2.aspx?id=' + response.ResponseData.TransactionId)
                            });

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
                    w88Mobile.FormValidator.enableSubmitButton('#ContentPlaceHolder1_btnSubmit');
                    GPINTMOBILE.HideSplash();
                });
            });
        });
    </script>
</asp:Content>