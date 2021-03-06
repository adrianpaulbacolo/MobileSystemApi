﻿<%@ Page Language="C#" MasterPageFile="~/v2/MasterPages/Main.master" AutoEventWireup="true" CodeFile="Dashboard.aspx.cs" Inherits="v2_Dashboard" %>
<%@ Import Namespace="Helpers" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContentHolder" runat="Server">
    <div class="home-banner slick-slider">
    </div>
    <% if (!commonFunctions.isExternalPlatform() && string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId)) { %>
    <div class="row no-gutter">
        <div class="col-xs-6">
            <a class="btn btn-block btn-gray" href="<%=Pages.Register%>" data-i18n="LABEL_MENU_REGISTER"></a>
        </div>
        <div class="col-xs-6">
            <a class="btn btn-block btn-primary" href="<%=Pages.Login%>" data-i18n="LABEL_MENU_LOGIN"></a>
        </div>
    </div>
    <% } %>
    <div class="dashboard dashboard-home">
        <div class="dashboard-row">
            <div class="dashboard-col">
                <a href="<%=Pages.Sports%>?lang=<%=commonVariables.SelectedLanguage.ToLower() %>"><span class="icon icon-soccer"></span>
                    <span data-i18n="LABEL_MENU_SPORTS"></span>
                </a>
            </div>
            <div class="dashboard-col">
                <a href="#"><span class="icon icon-casino"></span>
                    <span data-i18n="LABEL_PRODUCTS_CASINO"></span>
                    </a>
            </div>
        </div>
        <div class="dashboard-row">
            <div class="dashboard-col">
                <a href="<%=Pages.Slots%>"><span class="icon icon-slots"></span>
                    <span data-i18n="LABEL_SLOTS"></span>
                </a>
            </div>
            <div class="dashboard-col">
                <a href="<%=FishingLink %>" target="_blank"><span class="icon icon-fish"></span>
                    <span data-i18n="LABEL_PRODUCTS_FISHING_MASTER"></span>
                </a>
            </div>
        </div>
        <div class="dashboard-row">
            <div class="dashboard-col">
                <a href="<%=Pages.Lottery%>?lang=<%=commonVariables.SelectedLanguage%>"><span class="icon icon-keno"></span>
                    <span data-i18n="LABEL_WALLET_LOTTERY"></span>
                </a>
            </div>
            <div class="dashboard-col">
                <a href="<%=Pages.Downloads%>"><span class="icon icon-download"></span>
                    <span data-i18n="LABEL_DOWNLOAD"></span>
                </a>
            </div>
        </div>
    </div>

    <div class="home-footer">
        <img src="/_Static/v2/assets/images/GPI-logo2.png" alt="">
        <p data-i18n="LABEL_FOOTER"></p>
        <p data-i18n="LABEL_FOOTER_COPYRIGHT"></p>
    </div>

</asp:Content>

<asp:Content ID="ScriptBottom" ContentPlaceHolderID="InnerScriptPlaceHolder" runat="Server">
    <script>
        $(".dashboard-home a > span.icon-casino").click(function (e) {
            e.preventDefault();
            try {
                Native.openLiveCasino()
            } catch (e) {
                console.log("Native does not exist!");
            }
        });

        var loadBanner = function () {
            $(".home-banner.slick-slider").append('<%=base.BannerDiv %>').slick({
                arrows: false,
                dots: true,
                autoplay: true
            }
            );
        }

        if (!_.isUndefined($(window).load)) {
            $(window).load(loadBanner);
        } else $(document).ready(loadBanner);


        $(document).ready(function () {
            var $el = $("div.home-footer").find("[data-i18n='LABEL_FOOTER_COPYRIGHT']");
            var yr = $el[0].innerHTML.replace("[year]", new Date().getFullYear());
            $el.html(yr);
        });

    </script>
</asp:Content>
