<%@ Page Title="" Language="C#" MasterPageFile="~/v2/MasterPages/Main.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="v2_FundTransfer_Default" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContentHolder" runat="Server">
    <div class="wallets deposit">
    </div>

    <div class="balance-collapse">
        <button id="showBalance" type="button" class="btn btn-block show-balance collapsed" data-toggle="collapse" data-target=".wallet-balance" 
            aria-expanded="false" aria-controls="wallet-balance" data-i18n="BUTTON_SHOW_BALANCE"></button>
        <div class="collapse wallet-balance"></div>
    </div>
    <div class="form-container">
        <div class="container">
            <form class="form" id="form1" runat="server">
                <div class="form-group walletFrom">
                    <asp:Label ID="lblTransferFrom" runat="server" AssociatedControlID="drpTransferFrom" data-i18n="LABEL_FROM" />
                    <asp:DropDownList ID="drpTransferFrom" runat="server" data-corners="false" CssClass="form-control" required data-selectequals="-1" data-balance="0" />
                </div>
                <div class="text-center">
                    <button id="btnSwap" type="button" value="" class="btn-swap">
                        <span class="icon icon-swap"></span>
                    </button>
                </div>
                <div class="form-group walletFrom">
                    <asp:Label ID="lblTransferTo" runat="server" AssociatedControlID="drpTransferTo" data-i18n="LABEL_TO" />
                    <asp:DropDownList ID="drpTransferTo" runat="server" data-corners="false" CssClass="form-control" required data-selectequals="-1" />
                </div>
                <div class="form-group">
                    <asp:Label ID="lblAmount" runat="server" AssociatedControlID="txtAmount" data-i18n="LABEL_AMOUNT" />
                    <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" required data-require="" data-numeric/>
                </div>
                <div class="form-group promocode" hidden>
                    <asp:Label ID="lblPromoCode" runat="server" AssociatedControlID="txtPromoCode" data-i18n="LABEL_PROMO_CODE" />
                    <asp:TextBox ID="txtPromoCode" runat="server" CssClass="form-control" />
                </div>
                <div class="form-group">
                    <button type="submit" id="btnSubmit" class="btn btn-block btn-primary" data-i18n="BUTTON_SUBMIT" ></button>
                </div>
            </form>
        </div>
    </div>

</asp:Content>
<asp:Content ID="ScriptBottom" ContentPlaceHolderID="InnerScriptPlaceHolder" runat="Server">
    <script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/gateways/gateway.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
    <script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/modules/bootstrapvalidator.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
    <script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/modules/wallets.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
    <script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/modules/fundtransfer.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            _w88_fundtransfer.init();

            $('#form1').validator().on('submit', function (e) {
                if (!e.isDefaultPrevented()) {
                    e.preventDefault();

                    var data = {
                        TransferFrom: $('select[id$="drpTransferFrom"]').val(),
                        TransferTo: $('select[id$="drpTransferTo"]').val(),
                        TransferAmount: $('input[id$="txtAmount"]').autoNumeric('get'),
                        PromoCode: $('input[id$="txtPromoCode"]').val()
                    };

                    _w88_fundtransfer.create(data);
                }
            });
        });
    </script>
</asp:Content>