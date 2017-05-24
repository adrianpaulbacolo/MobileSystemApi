<%@ Page Language="C#" MasterPageFile="~/v2/MasterPages/Main.master" AutoEventWireup="true" CodeFile="Funds.aspx.cs" Inherits="v2_Funds" %>

<asp:Content ID="PaymentContent" ContentPlaceHolderID="MainContentHolder" runat="Server">

    <div class="wallets">
    </div>

    <div class="dashboard dashboard-funds">
        <div class="dashboard-row">
            <% var depositLinkId = (commonFunctions.isExternalPlatform()) ? "launch-deposit" : ""; %>
            <div class="dashboard-col <%=depositLinkId %>">
                <a href="/v2/Deposit/Default.aspx">
                    <span class="icon icon-deposit"></span>
                    <span data-i18n="LABEL_FUNDS_DEPOSIT"></span>
                </a>
            </div>
            <div class="dashboard-col">
                <a href="/v2/FundTransfer/">
                    <span class="icon icon-transfer"></span>
                    <span data-i18n="LABEL_FUNDS_TRANSFER"></span>
                </a>
            </div>
        </div>
        <div class="dashboard-row">
            <div class="dashboard-col">
                <a href="/v2/Withdrawal/">
                    <span class="icon icon-withdraw"></span>
                    <span data-i18n="LABEL_FUNDS_WIDRAW"></span>
                </a>
            </div>
            <div class="dashboard-col">
                <a href="/v2/History/Default.aspx">
                    <span class="icon icon-history"></span>
                     <span data-i18n="LABEL_FUNDS_HISTORY"></span>
                </a>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="ScriptHolder" ContentPlaceHolderID="InnerScriptPlaceHolder" runat="server">
    <script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/modules/wallets.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
    <script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/modules/funds.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"> </script>

    <script>
        $(document).ready(function () {
            _w88_funds.init();
        });

        // hackish way to communicate in between iframes lol, check slots page, surprisingly, it has too
        if ((!_.isUndefined(inIframe)) && inIframe()) {
            var parentOrigin = window.location.origin;
            var parentWindow = window.parent;

            parentWindow.postMessage('funds', parentOrigin);
        }

    </script>
</asp:Content>