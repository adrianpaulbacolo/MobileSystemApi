<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Payments.master" AutoEventWireup="true" CodeFile="PayGo.aspx.cs" Inherits="Withdrawal_PayGo" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
    <ul class="list fixed-tablet-size">
        <li class="item item-input">
            <asp:Label ID="lblAmount" runat="server" AssociatedControlID="txtAmount" />
            <asp:TextBox ID="txtAmount" runat="server" type="number" step="any" min="1" data-clear-btn="true" />
        </li>
        <li class="item item-input">
            <asp:Label ID="lblAccountName" runat="server" AssociatedControlID="txtAccountName" />
            <asp:TextBox ID="txtAccountName" runat="server" data-clear-btn="true" />
        </li>
        <li class="item item-input">
            <asp:Label ID="lblAccountNumber" runat="server" AssociatedControlID="txtAccountNumber" />
            <asp:TextBox ID="txtAccountNumber" runat="server" data-clear-btn="true" />
        </li>
        <li class="item item-select">
            <asp:Label ID="lblContact" runat="server" AssociatedControlID="txtContact" />
            <div class="row">
                <div class="col col-25">
                    <asp:DropDownList ID="drpContactCountry" runat="server" data-icon="false" data-mini="true" />
                </div>
                <div class="col col-75">
                    <asp:TextBox ID="txtContact" runat="server" type="tel" data-mini="true" data-clear-btn="true" />
                </div>
            </div>
        </li>
    </ul>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsPlaceHolder1" runat="Server">
    <script type="text/javascript" src="/_Static/JS/modules/gateways/paygo.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            var payments = new w88Mobile.Gateways.Payments("<%=base.PaymentMethodId %>");

            payments.init();

            window.w88Mobile.Gateways.PayGo.InitWithdraw();

            $('#form1').submit(function (e) {
                e.preventDefault();
                window.w88Mobile.FormValidator.disableSubmitButton('#ContentPlaceHolder1_btnSubmit');

                var data = {
                    Amount: $('#<%=txtAmount.ClientID%>').val(),
                    AccountName: $('#<%=txtAccountName.ClientID%>').val(),
                    AccountNumber: $('#<%=txtAccountNumber.ClientID%>').val(),
                    CountryCode: $('#<%=drpContactCountry.ClientID%>').val(),
                    Phone: $('#<%=txtContact.ClientID%>').val()
                };

                payments.send(data, function (response) {
                    switch (response.ResponseCode) {
                        case 1:
                            w88Mobile.Growl.shout("<p>" + response.ResponseMessage + "</p> <p>" + '<%=lblTransactionId%>' + ": " + response.ResponseData.TransactionId + "</p>");

                            window.location.reload();

                            break;
                        default:
                            if (_.isArray(response.ResponseMessage)) {
                                w88Mobile.Growl.shout(w88Mobile.Growl.bulletedList(response.ResponseMessage));
                            }
                            else {
                                w88Mobile.Growl.shout(response.ResponseMessage);
                            }
                            break;
                    }
                },
                function () {
                    window.w88Mobile.FormValidator.enableSubmitButton('#ContentPlaceHolder1_btnSubmit');
                    GPInt.prototype.HideSplash();
                });
            });
        });
    </script>
</asp:Content>
