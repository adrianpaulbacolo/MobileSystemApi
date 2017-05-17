<%@ Page Language="C#" MasterPageFile="~/v2/MasterPages/Main.master" AutoEventWireup="true" CodeFile="DownloadItem.aspx.cs" Inherits="v2_DownloadItem" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContentHolder" runat="Server">
    <div class="download-coverphoto">
        <img src="" alt="">
    </div>
    <div class="download-header">
        <div class="container container-small" id="instructionHeader">
            <h3 class="title"></h3>
        </div>
    </div>
    <div class="download-instructions">
        <div class="container">
        </div>
    </div>
    <div class="download-button">
        <div class="container">
            <a href="#" class="btn btn-block btn-primary" id="downloadlink"></a>
        </div>
    </div>
</asp:Content>
<asp:Content ID="InnerScript" ContentPlaceHolderID="InnerScriptPlaceHolder" runat="Server">
    <script type="text/javascript">
        pubsub.subscribe("contentsLoaded", downloadItemsContentsLoaded);
        $("section.body").hide();

        function downloadItemsContentsLoaded(data) {
            if ($.i18n("DOWNLOADS_INSTRUCTIONS_CONTENT") == "DOWNLOADS_INSTRUCTIONS_CONTENT") return;
            $("section.body").show();
         <%
        switch (Item)
        {
            case "palazzo-casino":
        %>

            $("#instructionHeader h3.title").html($.i18n("DOWNLOADS_INSTRUCTIONS_HEADER"));
            $("div.download-instructions div.container").html($.i18n("DOWNLOADS_INSTRUCTIONS_CONTENT"));
            $("div.download-button a#downloadlink").html($.i18n("BUTTON_DOWNLOAD_NOW"));
            $("div.download-button a#downloadlink").attr("href", "http://mlive.w88palazzo.com").attr("target", "_blank");
            $("div.download-coverphoto img").attr("src", "/_static/v2/assets/images/downloads/PT-LiveCasino-DownloadPage.jpg");
        <%
                break;
            case "palazzo-slots":
        %>
            $("#instructionHeader h3.title").html($.i18n("DOWNLOADS_INSTRUCTIONS_HEADER"));
            $("div.download-instructions div.container").html($.i18n("DOWNLOADS_INSTRUCTIONS_CONTENT"));
            $("div.download-button a#downloadlink").html($.i18n("BUTTON_DOWNLOAD_NOW"));
            $("div.download-button a#downloadlink").attr("href", "http://mgames.w88palazzo.com").attr("target", "_blank");
            $("div.download-coverphoto img").attr("src", "/_static/v2/assets/images/downloads/PT-Slots-DownloadPage.jpg");
         <%
                break;
            case "texas-mahjong-ios":
        %>
            $("#instructionHeader h3.title").html($.i18n("DOWNLOADS_INSTRUCTIONS_TMSIOS_HEADER"));
            $("div.download-instructions div.container").html($.i18n("DOWNLOADS_INSTRUCTIONS_TMSIOS_CONTENT"));
            $("div.download-button a#downloadlink").html($.i18n("BUTTON_DOWNLOAD_NOW"));
            $("div.download-button a#downloadlink").attr("href", "<%=ConfigurationManager.AppSettings["TexasMahjongIOS_URL"] %>");
            $("div.download-coverphoto img").attr("src", "/_static/v2/assets/images/downloads/TM-DownloadPage.jpg");
         <%
                break;
            case "super-bull-ios":
        %>
            $("#instructionHeader h3.title").html($.i18n("DOWNLOADS_INSTRUCTIONS_SBULLIOS_HEADER"));
            $("div.download-instructions div.container").html($.i18n("DOWNLOADS_INSTRUCTIONS_SBULLIOS_CONTENT"));
            $("div.download-button a#downloadlink").html($.i18n("BUTTON_DOWNLOAD_NOW"));
            var sbullLink = "<%=(commonVariables.SelectedLanguage == "zh-cn") ? ConfigurationManager.AppSettings["SuperBull_IOS_URL"] : ConfigurationManager.AppSettings["SuperBull_IOS_URL_EN"] %>";
            $("div.download-button a#downloadlink").attr("href", sbullLink);
            $("div.download-coverphoto img").attr("src", "/_Static/Images/Download/Superbull-iOS-Mobile-Download.jpg");
         <%
                break;
            }
        %>
        }

        if (!_.isEmpty(_w88_contents.items)) downloadItemsContentsLoaded();

    </script>
</asp:Content>
