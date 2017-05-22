<%@ Page Title="" Language="C#" MasterPageFile="~/v2/MasterPages/Main.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="v2_Account_Default" %>
<%@ Import Namespace="Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContentHolder" runat="Server">
    <div class="wallets">
        <div class="row">
            <div class="col-xs-6">
                <div class="wallet-main">
                    <p class="wallet-title"></p>
                    <h4 class="wallet-value"></h4>
                    <p class="wallet-currency"></p>
                </div>
            </div>
            <div class="col-xs-6">
                <div class="rewards-main">
                    <p data-i18n="LABEL_MENU_REWARDS" class="rewards-title"></p>
                    <h4 class="rewards-value"></h4>
                    <p data-i18n="LABEL_POINTS" class="rewards-currency"></p>
                </div>
            </div>
        </div>
    </div>
    <div class="dashboard">
        <div class="dashboard-row">
            <div class="dashboard-col">
                <a href="<%=W88Constant.PageNames.BankDetails%>">
                    <span class="icon icon-banking"></span>
                    <span data-i18n="LABEL_MENU_BANK_DETAILS"></span>
                </a>
            </div>
            <div class="dashboard-col">
                <a href="<%=W88Constant.PageNames.Rebates%>">
                    <span class="icon icon-rebates"></span>
                    <span data-i18n="LABEL_MENU_REBATES"></span>
                </a>
            </div>
        </div>
        <div class="dashboard-row">
            <div class="dashboard-col">
                <a href="<%=W88Constant.PageNames.LiveChat%>">
                    <span class="icon icon-chat"></span>
                    <span data-i18n="LABEL_MENU_LIVE_CHAT"></span>
                </a>
            </div>
            <div class="dashboard-col">
                <a href="<%=W88Constant.PageNames.Upload%>">
                    <span class="icon icon-submit"></span>
                    <span data-i18n="LABEL_MENU_UPLOAD"></span>
                </a>
            </div>
        </div>
        <div class="dashboard-row">
            <div class="dashboard-col">
                <a href="<%=W88Constant.PageNames.ChangePassword%>">
                    <span class="icon icon-password"></span>
                    <span data-i18n="LABEL_PASSWORD_CHANGE"></span>
                </a>
            </div>
            <div class="dashboard-col">
                <a href="<%=W88Constant.PageNames.ContactUs%>">
                    <span class="icon icon-phone"></span>
                    <span data-i18n="LABEL_MENU_CONTACT_US"></span>
                </a>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="InnerScriptPlaceHolder" runat="Server">
    <script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/modules/wallets.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>

    <script>

        $(document).ready(function () {
            $('.header-title').first().text(_w88_contents.translate("LABEL_MENU_PROFILE"));

            pubsub.subscribe('mainWalletLoadedOnly', onMainWalletLoadedOnly);

            _w88_wallets.mainWalletInit({ wallets: "wallets" });
            _w88_wallets.rewardsPointsInit({ wallets: {} });

            function onMainWalletLoadedOnly(topic, data) {

                $(".wallet-title").html(_.toUpper(data.Name));
                $(".wallet-value").html(data.Balance);
                $(".wallet-currency").html(data.CurrencyLabel);
                $(".wallets").addClass('wallet-auto');
            }
         
        });
    </script>

</asp:Content>

