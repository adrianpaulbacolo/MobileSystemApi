<%@ Page Title="" Language="C#" MasterPageFile="~/v2/MasterPages/Main.master" AutoEventWireup="true" CodeFile="Sports.aspx.cs" Inherits="v2_Sports" %>
<%@ Import Namespace="Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContentHolder" runat="Server">

    <div class="banner-lists banner-odd-even">
        <div class="row">
            <figure class="banner">
                <img src="/_Static/Images/sports/a-sports-banner.jpg" class="img-responsive img-bg">
                <figcaption class="banner-caption">
                    <h3 class="title" data-i18n="LABEL_PRODUCTS_ASPORTS"></h3>
                    <p data-i18n="LABEL_PRODUCTS_ASPORTS_DESC"></p>
                    <a href="<%=ASportsUrl%>" data-i18n="BUTTON_PLAY_NOW" class="btn btn-block btn-primary"></a>
                </figcaption>
            </figure>
        </div>

        <div class="row">
            <figure class="banner">
                <img src="/_Static/Images/sports/e-sports-banner.jpg" class="img-responsive img-bg">
                <figcaption class="banner-caption">
                    <h3 class="title" data-i18n="LABEL_PRODUCTS_ESPORTS"></h3>
                    <p data-i18n="LABEL_PRODUCTS_ESPORTS_DESC"></p>
                    <a href="<%=ESportsUrl%>" data-i18n="BUTTON_PLAY_NOW" class="btn btn-block btn-primary"></a>
                </figcaption>
            </figure>
        </div>

        <div class="row">
            <figure class="banner">
                <img src="/_Static/Images/sports/v-sports-banner.jpg" class="img-responsive img-bg">
                <figcaption class="banner-caption">
                    <h3 class="title" data-i18n="LABEL_PRODUCTS_VSPORTS"></h3>
                    <p data-i18n="LABEL_PRODUCTS_VSPORTS_DESC"></p>
                    <a href="<%=W88Constant.PageNames.VSports%>" data-i18n="BUTTON_PLAY_NOW" class="btn btn-block btn-primary"></a>
                </figcaption>
            </figure>
        </div>

        <% if (DisplayXSports)
           { %>
        <div class="row">
            <figure class="banner">
                <img src="/_Static/Images/sports/x-sports-banner.jpg" class="img-responsive img-bg">
                <figcaption class="banner-caption">
                    <h3 class="title" data-i18n="LABEL_PRODUCTS_XSPORTS"></h3>
                    <p data-i18n="LABEL_PRODUCTS_XSPORTS_DESC"></p>
                    <a href="<%=XSportsUrl%>" data-i18n="BUTTON_PLAY_NOW" class="btn btn-block btn-primary"></a>
                </figcaption>
            </figure>
        </div>
        <% } %>
        
        <% if (!commonFunctions.isExternalPlatform()) { %>

        <% try
           { %>

        <%if (DeviceId == 1) {%>
        <div class="row">
            <figure class="banner">
                <img src='<%=DisplayXSports ? "/_Static/Images/sports/ClubW88-iOS-banner-l.jpg" : "/_Static/Images/sports/ClubW88-iOS-banner-r.jpg" %>' class="img-responsive img-bg">
                <figcaption class="banner-caption">
                    <h3 class="title" data-i18n="LABEL_PRODUCTS_CLUB_W"></h3>
                    <p data-i18n="LABEL_PRODUCTS_CLUB_W_DESC"></p>
                    <a href="<%=W88Constant.PageNames.Downloads%>" data-i18n="BUTTON_PLAY_NOW" class="btn btn-block btn-primary"></a>
                </figcaption>
            </figure>
        </div>
        <% } else if (DeviceId == 2) {%>
        <div class="row">
            <figure class="banner">
                <img src='<%=DisplayXSports ? "/_Static/Images/sports/ClubW88-Android-banner-l.jpg" : "/_Static/Images/sports/ClubW88-Android-banner-r.jpg" %>' class="img-responsive img-bg">
                <figcaption class="banner-caption">
                    <h3 class="title" data-i18n="LABEL_PRODUCTS_CLUB_W"></h3>
                    <p data-i18n="LABEL_PRODUCTS_CLUB_W_DESC"></p>
                    <a href="<%=W88Constant.PageNames.Downloads%>" data-i18n="BUTTON_PLAY_NOW" class="btn btn-block btn-primary"></a>
                </figcaption>
            </figure>
        </div>
        <%} else if (DeviceId == 3) { %>
        <div class="row">
            <figure class="banner">
                <img src='<%=DisplayXSports ? "/_Static/Images/sports/ClubW88-iOS-banner-l.jpg" : "/_Static/Images/sports/ClubW88-iOS-banner-r.jpg" %>' class="img-responsive img-bg">
                <figcaption class="banner-caption">
                    <h3 class="title" data-i18n="LABEL_PRODUCTS_CLUB_W"></h3>
                    <p data-i18n="LABEL_PRODUCTS_CLUB_W_DESC"></p>
                    <a href="<%=W88Constant.PageNames.Downloads%>" data-i18n="BUTTON_PLAY_NOW" class="btn btn-block btn-primary"></a>
                </figcaption>
            </figure>
        </div>
        <div class="row">
            <figure class="banner">
                <img src='<%=DisplayXSports ? "/_Static/Images/sports/ClubW88-Android-banner-r.jpg" :"/_Static/Images/sports/ClubW88-Android-banner-l.jpg"  %>' class="img-responsive img-bg">
                <figcaption class="banner-caption">
                    <h3 class="title" data-i18n="LABEL_PRODUCTS_CLUB_W"></h3>
                    <p data-i18n="LABEL_PRODUCTS_CLUB_W_DESC"></p>
                    <a href="<%=W88Constant.PageNames.Downloads%>" data-i18n="BUTTON_PLAY_NOW" class="btn btn-block btn-primary"></a>
                </figcaption>
            </figure>
        </div>
        <%}%>

        <%}
           catch (Exception) { }%>
        
        <% } %>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="InnerScriptPlaceHolder" runat="Server">

    <script>
        $(document).ready(function () {
            $('.header-title').html($.i18n("LABEL_MENU_SPORTS"));
        });
    </script>
</asp:Content>

