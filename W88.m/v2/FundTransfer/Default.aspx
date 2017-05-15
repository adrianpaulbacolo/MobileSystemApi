<%@ Page Title="" Language="C#" MasterPageFile="~/v2/MasterPages/Main.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="v2_FundTransfer_Default" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContentHolder" runat="Server">
    <div class="wallets">
        <div class="wallet-main">
            <p class="wallet-title"></p>
            <h4 class="wallet-value"></h4>
            <p class="wallet-currency"></p>
        </div>
    </div>

    <div class="balance-collapse">
        <button id="showBalance" type="button" class="btn btn-block show-balance collapsed" data-toggle="collapse" data-target="#walletBalances" aria-expanded="false" aria-controls="walletBalances"></button>
        <div class="collapse" id="walletBalances"></div>
    </div>
    <div class="form-container">
        <div class="container">
            <form class="form" id="form1" runat="server">
                <div class="form-group walletFrom">
                    <asp:Label ID="lblTransferFrom" runat="server" AssociatedControlID="drpTransferFrom" />
                    <asp:DropDownList ID="drpTransferFrom" runat="server" data-corners="false" CssClass="form-control" required data-selectequals="" data-balance="0" />
                </div>
                <div class="text-center">
                    <button id="btnSwap" type="button" value="" class="btn-swap">
                        <span class="icon icon-swap"></span>
                    </button>
                </div>
                <div class="form-group walletFrom">
                    <asp:Label ID="lblTransferTo" runat="server" AssociatedControlID="drpTransferTo" />
                    <asp:DropDownList ID="drpTransferTo" runat="server" data-corners="false" CssClass="form-control" required data-selectequals="" />
                </div>
                <div class="form-group">
                    <asp:Label ID="lblTransferAmount" runat="server" AssociatedControlID="txtTransferAmount" />
                    <asp:TextBox ID="txtTransferAmount" runat="server" type="number" step="any" min="1" CssClass="form-control" required />
                </div>
                <div class="form-group">
                    <asp:Label ID="lblPromoCode" runat="server" AssociatedControlID="txtPromoCode" />
                    <asp:TextBox ID="txtPromoCode" runat="server" CssClass="form-control" />
                </div>
                <div class="form-group">
                    <button type="submit" id="btnSubmit" class="btn btn-block btn-primary"></button>
                </div>
            </form>
        </div>
    </div>

</asp:Content>
<asp:Content ID="ScriptBottom" ContentPlaceHolderID="InnerScriptPlaceHolder" runat="Server">
    <script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/gateways/gateway.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
    <script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/funds.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
    <script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/fundtransfer.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>

    <script type="text/javascript">

        $('#form1').validator({
            custom: {
                selectequals: function ($el) {
                    $el.parent("div.form-group").removeClass('has-error');
                    $el.parent("div.form-group").children("span.help-block").remove();
                    var matchValue = $el.data("bankequals");
                    if ($el.val() == matchValue) {
                        $el.parent("div.form-group").addClass('has-error');
                        return true;
                    }
                },
                balance: function ($el) {
                    $el.parent("div.form-group").removeClass('has-error');
                    $el.parent("div.form-group").children("span.help-block").remove();
                    var matchValue = $el.data("balance");
                    if ($el.attr('balance') <= matchValue) {
                        $el.parent("div.form-group").addClass('has-error');
                        return true;
                    }
                }
            }
        });

        $(document).ready(function () {

            _w88_fundtransfer.init();

            $('select[id$="drpTransferFrom"]').change(function () {
                var option = $('option:selected', this).attr('balance');
                $(this).attr('balance', option);
                _w88_fundtransfer.changeWalletTo(this.value);
            });

            $('select[id$="drpTransferTo"]').change(function () {
                var option = $('option:selected', this).attr('balance');
                $(this).attr('balance', option);
            });

            $('#btnSwap').click(function () {
                _w88_fundtransfer.swap($('select[id$="drpTransferFrom"]'), $('select[id$="drpTransferTo"]'));
            });

            $('#form1').validator().on('submit', function (e) {

                if (!e.isDefaultPrevented()) {

                    e.preventDefault();
                    var data = {
                        TransferFrom: $('select[id$="drpTransferFrom"]').val(),
                        TransferTo: $('select[id$="drpTransferTo"]').val(),
                        TransferAmount: $('input[id$="txtTransferAmount"]').val(),
                        PromoCode: $('input[id$="txtPromoCode"]').val()
                    };

                    _w88_fundtransfer.create(data);
                }
            });

            $('#walletBalances').on('show.bs.collapse', function () {
                $('#showBalance').text(_w88_contents.translate("BUTTON_HIDE_BALANCE"));
            });

            $('#walletBalances').on('hide.bs.collapse', function () {
                $('#showBalance').text(_w88_contents.translate("BUTTON_SHOW_BALANCE"));
            });

        });
    </script>

    <script type="text/template" id='walletBalance'>
        <div class="balances">
            <table>
                {% _.forEach( tplData.wallets, function( wallet ){ %}
                <tr>
                    <td>{%-wallet.Name%}
                    </td>
                    <td class="text-right">{%-wallet.Balance%}
                    </td>
                </tr>
                {% }); %}
            </table>
        </div>
    </script>

    <script type="text/template" id='mainWallet'>

        <div class="wallet-main">
            <p class="wallet-title">{%-tplData.Name%}</p>
            <h4 class="wallet-value">{%-tplData.Balance%}</h4>
            <p class="wallet-currency">{%-tplData.CurrencyLabel%}</p>
        </div>

    </script>

</asp:Content>

