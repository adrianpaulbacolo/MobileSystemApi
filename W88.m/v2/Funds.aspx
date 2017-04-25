<%@ Page Language="C#" MasterPageFile="~/v2/MasterPages/Main.master" AutoEventWireup="true" CodeFile="Funds.aspx.cs" Inherits="v2_Funds" %>

<asp:Content ID="PaymentContent" ContentPlaceHolderID="MainContentHolder" runat="Server">


    <div class="wallets">
    </div>

    <div class="dashboard dashboard-funds">
        <div class="dashboard-row">
            <% var depositLinkId = (commonFunctions.isNativeAgent(Request)) ? "launch-deposit" : ""; %>
            <div class="dashboard-col <%=depositLinkId %>">
                <a href="/v2/Deposit/Default.aspx">
                    <span class="icon icon-deposit"></span><%=commonCulture.ElementValues.getResourceString("deposit", commonVariables.LeftMenuXML)%>
                </a>
            </div>
            <div class="dashboard-col">
                <a href="/v2/FundTransfer/">
                    <span class="icon icon-transfer"></span><%=commonCulture.ElementValues.getResourceString("transfer", commonVariables.LeftMenuXML)%>
                </a>
            </div>
        </div>
        <div class="dashboard-row">
            <div class="dashboard-col">
                <a href="/v2/Withdrawal/">
                    <span class="icon icon-withdraw"></span><%=commonCulture.ElementValues.getResourceString("withdrawal", commonVariables.LeftMenuXML)%>
                </a>
            </div>
            <div class="dashboard-col">
                <a href="/v2/History/Default.aspx">
                    <span class="icon icon-history"></span><%=commonCulture.ElementValues.getResourceString("history", commonVariables.HistoryXML)%>
                </a>
            </div>
        </div>
    </div>
</asp:Content>
  
<asp:Content ID="ScriptHolder" ContentPlaceHolderID="InnerScriptPlaceHolder" runat="server">
      <script src="/_static/v2/assets/js/funds.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>

       <script>
           $(document).ready(function () {

               pubsub.subscribe('fundsLoaded', onFundsLoaded);

               _w88_funds.init();

               function onFundsLoaded() {

                   var wallets = _w88_funds.wallets()

                   var mainWalletTpl = _.template(
                       $("script#mainWallet").html()
                   );

                   var walletList = _.template(
                       $("script#walletMenu").html()
                   );

                   $("div.wallets").append(
                       mainWalletTpl(_.first(wallets))
                   );

                   $("div.wallets").append(
                       walletList({ wallets: wallets })
                   );
               }

           });

           // hackish way to communicate in between iframes lol, check slots page, surprisingly, it has too
           if ((!_.isUndefined(inIframe)) && inIframe()) {
               var parentOrigin = window.location.origin;
               var parentWindow = window.parent;

               parentWindow.postMessage('funds', parentOrigin);
           }

    </script>
    <script type="text/template" id='walletMenu'>

        <div class="wallet-group">
            <div>
                {% _.forEach( tplData.wallets, function( wallet ){ %}
            <div class="wallet-group-item">
                <p>{%-wallet.Name%}</p>
                <h5>{%-wallet.Balance%}</h5>
            </div>
                {% }); %}
            </div>
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