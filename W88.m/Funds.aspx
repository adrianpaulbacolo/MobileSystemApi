<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Site.master" AutoEventWireup="true" CodeFile="Funds.aspx.cs" Inherits="Funds" %>

<%@ Register TagPrefix="uc" TagName="Wallet" Src="~/UserControls/MainWalletBalance.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="ui-content" role="main" id="funds">
        <div class="wallet main-wallet">
            <uc:Wallet ID="uMainWallet" runat="server" />
            <a href="#" id="refesh" class="reload">
                <span class="icon icon-refresh"></span>
            </a>
        </div>
        <div id="tabs">
            <div data-role="navbar">
                <ul>
                    <li><a href="/Deposit/Default.aspx" data-ajax="false"><%=commonCulture.ElementValues.getResourceString("deposit", commonVariables.LeftMenuXML)%></a></li>
                    <li><a href="/FundTransfer/Default<%=getAppSuffix() %>.aspx" data-ajax="false"><%=commonCulture.ElementValues.getResourceString("transfer", commonVariables.LeftMenuXML)%></a></li>
                    <li><a href="/Withdrawal/Default.aspx" data-ajax="false"><%=commonCulture.ElementValues.getResourceString("withdrawal", commonVariables.LeftMenuXML)%></a></li>
                    <li><a href="/v2/History/Default.aspx" data-ajax="false"><%=commonCulture.ElementValues.getResourceString("history", commonVariables.HistoryXML)%></a></li>
                </ul>
            </div>
        </div>
        <form runat="server">
            
            <asp:Literal ID="ltlFunds" runat="server"></asp:Literal>
          
        </form>
        <br>
        <p class="note text-center">
            <small>*<asp:Literal ID="ltlNote" runat="server"></asp:Literal></small>
        </p>
    </div>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsPlaceHolder" runat="Server">

    <script type="text/javascript">

        $(document).ready(function () {

            window.w88Mobile.Wallets.getWallets();

            $("#refesh").click(function () {
                $("#mainwallet").html(loader);
                var fetch = window.w88Mobile.Wallets.getMain().done();

                fetch.done(function (data) {
                    $("#mainwallet").html(data);
                });

                window.w88Mobile.Wallets.getWallets();
            });

            $(".fundsType").click(function () {
                var id = $(this).attr('walletId');
                sessionStorage.setItem('selectedWalletId', id);
                redirecToFundTransfer();
            });

            function redirecToFundTransfer() {
                window.location = 'FundTransfer/Default<%=getAppSuffix()%>.aspx';
            }
        });
    </script>
</asp:Content>

