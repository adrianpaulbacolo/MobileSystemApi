<%@ Page Title="" Language="C#" MasterPageFile="~/v2/MasterPages/Main.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="v2_Account_Default" %>

<%@ Import Namespace="Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContentHolder" runat="Server">
    <div class="dashboard">
        <div class="wallets deposit">
            <div class="row">
                <div class="col-xs-6 wallet-placeholder">
                </div>
                <div class="col-xs-6 rewards-placeholder" >
                </div>
            </div>
        </div>
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
                <a href="<%=W88Constant.PageNames.LiveChat%>" target="_blank">
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
    <script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/modules/rewards.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
    <script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/modules/wallets.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>

    <script>

        $(document).ready(function () {
            $('.header-title').first().text($.i18n("LABEL_MENU_PROFILE"));

            pubsub.subscribe('wallets', onMainWalletLoaded);
            pubsub.subscribe('rewardsPointLoaded', onRewardsPointLoaded);

            _w88_wallets.init(undefined, "wallet-placeholder");
            _w88_rewards.init(option = { selector: "rewards-placeholder" });

            function onMainWalletLoaded(topic, data) {
                $(".wallets").addClass('wallet-auto');
            }

            function onRewardsPointLoaded(topic, data) {
                var template = _.template($("script#RewardsTemplate").html());
                $(".rewards-placeholder").html(template({
                    data: data
                }));
                $(".wallets").i18n();
            }
        });
    </script>
    
    <script type="text/template" id='RewardsTemplate'>
        <div class="rewards-main">
            <p data-i18n="LABEL_MENU_REWARDS" class="rewards-title"></p>
            <h4 class="rewards-value">{%-tplData.data%}</h4>
            <p data-i18n="LABEL_POINTS" class="rewards-currency"></p>
        </div>
    </script>
</asp:Content>

