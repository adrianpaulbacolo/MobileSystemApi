<%@ Page Title="" Language="C#" MasterPageFile="~/v2/MasterPages/Payment.master" AutoEventWireup="true" CodeFile="Pay120236.aspx.cs" Inherits="v2_Deposit_Pay120236" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PaymentMainContent" runat="Server">
    <ul class="list fixed-tablet-size">
        <li class="item item-input">
            <asp:Label ID="lblDepositAmount" runat="server" AssociatedControlID="txtAmount" />
            <asp:TextBox ID="txtAmount" runat="server" type="number" step="any" min="1" data-clear-btn="true" />
        </li>
        <li class="item item-select">
            <asp:Label ID="lblCardType" runat="server" AssociatedControlID="ddlCardType" />
            <asp:DropDownList ID="ddlCardType" runat="server">
            </asp:DropDownList>
        </li>
        <li class="item item-input">
            <asp:Label ID="lblCardName" runat="server" AssociatedControlID="txtCardName" />
            <asp:TextBox ID="txtCardName" runat="server" />
        </li>
        <li class="item item-input">
            <asp:Label ID="lblCardNo" runat="server" AssociatedControlID="txtCardNo" />
            <asp:TextBox ID="txtCardNo" runat="server" />
        </li>
        <li class="item item-select">
            <asp:Label ID="lblExpiry" runat="server" AssociatedControlID="ddlExpiryMonth" />
            <div class="row">
                <div class="col">
                    <asp:DropDownList ID="ddlExpiryMonth" runat="server" />
                </div>
                <div class="col">
                    <asp:DropDownList ID="ddlExpiryYear" runat="server" />
                </div>
            </div>
        </li>
        <li class="item item-input">
            <asp:Label ID="lblSecurityCode" runat="server" AssociatedControlID="txtSecurityCode" />
            <asp:TextBox ID="txtSecurityCode" runat="server" />
            <a href="#" data-toggle="modal" data-target="#ccvModal"></a>
        </li>
    </ul>


    <div id="ccvModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <%-- <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
        <h4 class="modal-title" id="exampleModalLabel">New message</h4>
      </div>--%>
                <div class="modal-body">
                    <img src="/_Static/Images/CVV-back.jpg" class="img-responsive" /></span>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ScriptsHolder" runat="Server">
    <script type="text/javascript" src="/_Static/JS/modules/gateways/alldebit.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            var payments = new w88Mobile.Gateways.Payments("<%=base.PaymentMethodId %>");
            payments.init();

            window.w88Mobile.Gateways.DefaultPayments.Deposit("<%=base.strCountryCode %>", "<%=base.strMemberID %>", '<%= commonCulture.ElementValues.getResourceString("paymentNotice", commonVariables.PaymentMethodsXML)%>', "<%=base.PaymentMethodId %>");
            window.w88Mobile.Gateways.AllDebit.Initialize();

            $('#<%=txtCardNo.ClientID%>').mask('9999-9999-9999-9999');
            $('#<%=txtSecurityCode.ClientID%>').mask('999');

            $('#form1').submit(function (e) {
                e.preventDefault();

                window.w88Mobile.FormValidator.disableSubmitButton('button[id$="btnSubmit"]');

                var data = {
                    Amount: $('#<%=txtAmount.ClientID%>').val(),
                    CardType: { Text: $('#<%=ddlCardType.ClientID%> option:selected').text(), Value: $('#<%=ddlCardType.ClientID%>').val() },
                    AccountName: $('#<%=txtCardName.ClientID%>').val(),
                    CardNumber: $('#<%=txtCardNo.ClientID%>').val(),
                    CardExpiryMonth: $('#<%=ddlExpiryMonth.ClientID%>').val(),
                    CardExpiryYear: $('#<%=ddlExpiryYear.ClientID%>').val(),
                    CCV: $('#<%=txtSecurityCode.ClientID%>').val()
                };

                payments.send(data, function (response) {
                    switch (response.ResponseCode) {
                        case 1:
                            w88Mobile.Growl.shout("<p>" + response.ResponseMessage + "</p> <p>" + '<%=lblTransactionId%>' + ": " + response.ResponseData.TransactionId + "</p>");
                            w88Mobile.PostPaymentForm.create(response.ResponseData.FormData, response.ResponseData.PostUrl, "body");
                            w88Mobile.PostPaymentForm.submit();
                            $('#form1')[0].reset();
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
        });
    </script>
</asp:Content>

