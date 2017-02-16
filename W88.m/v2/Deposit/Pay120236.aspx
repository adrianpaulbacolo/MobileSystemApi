<%@ Page Title="" Language="C#" MasterPageFile="~/v2/MasterPages/Payment.master" AutoEventWireup="true" CodeFile="Pay120236.aspx.cs" Inherits="v2_Deposit_Pay120236" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PaymentMainContent" runat="Server">
    <div class="form-group">
        <asp:Label ID="lblDepositAmount" runat="server" AssociatedControlID="txtAmount" />
        <asp:TextBox ID="txtAmount" runat="server" type="number" step="any" min="1"  CssClass="form-control"  />
    </div>
    <div class="form-group">
        <asp:Label ID="lblCardType" runat="server" AssociatedControlID="ddlCardType" />
        <asp:DropDownList ID="ddlCardType" runat="server" CssClass="form-control" >
        </asp:DropDownList>
    </div>
    <div class="form-group">
        <asp:Label ID="lblCardName" runat="server" AssociatedControlID="txtCardName" />
        <asp:TextBox ID="txtCardName" runat="server"  CssClass="form-control" />
    </div>
    <div class="form-group">
        <asp:Label ID="lblCardNo" runat="server" AssociatedControlID="txtCardNo" />
        <asp:TextBox ID="txtCardNo" runat="server"  CssClass="form-control" />
    </div>
    <div class="form-group">
        <asp:Label ID="lblExpiry" runat="server" AssociatedControlID="ddlExpiryMonth" />
        <div class="row thin-gutter">
            <div class="col-xs-6">
                <asp:DropDownList ID="ddlExpiryMonth" runat="server" CssClass="form-control" />
            </div>
            <div class="col-xs-6">
                <asp:DropDownList ID="ddlExpiryYear" runat="server"  CssClass="form-control" />
            </div>
        </div>
    </div>
    <div class="form-group">
        <asp:Label ID="lblSecurityCode" runat="server" AssociatedControlID="txtSecurityCode" />
        <asp:TextBox ID="txtSecurityCode" runat="server"  CssClass="form-control" />
        <a href="#" data-toggle="modal" data-target="#ccvModal"></a>
    </div>

    <div id="ccvModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-body">
                    <span>
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
            _w88_paymentSvcV2.setPaymentTabs("<%=base.PaymentType %>", "<%=base.PaymentMethodId %>");
            _w88_paymentSvcV2.DisplaySettings("<%=base.PaymentMethodId %>", { type: "<%=base.PaymentType %>" });

            window.w88Mobile.Gateways.AllDebit.init();

            $('#<%=txtCardNo.ClientID%>').mask('9999-9999-9999-9999');
            $('#<%=txtSecurityCode.ClientID%>').mask('999');

            $('#form1').submit(function (e) {
                e.preventDefault();

                var data = {
                    Amount: $('input[id$="txtAmount"]').val(),
                    CardTypeText: $('select[id$="ddlCardType"] option:selected').text(),
                    CardTypeValue: $('select[id$="ddlCardType"]').val(),
                    AccountName: $('[id$="txtCardName"]').val(),
                    CardNumber: $('[id$="txtCardNo"]').val(),
                    CardExpiryMonth: $('select[id$="ddlExpiryMonth"]').val(),
                    CardExpiryYear: $('select[id$="ddlExpiryYear"]').val(),
                    CCV: $('[id$="txtSecurityCode"]').val(),
                    MethodId: "<%=base.PaymentMethodId%>"
                };

                var params = decodeURIComponent($.param(data));
                window.open(_w88_paymentSvcV2.payRoute + "?" + params, "<%=base.PageName%>");
                _w88_paymentSvc.onTransactionCreated($(this));
                return;
            });
        });
    </script>
</asp:Content>

