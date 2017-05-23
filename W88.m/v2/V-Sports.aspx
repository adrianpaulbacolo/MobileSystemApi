<%@ Page Title="" Language="C#" MasterPageFile="~/v2/MasterPages/Main.master" AutoEventWireup="true" CodeFile="V-Sports.aspx.cs" Inherits="v2_V_Sports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContentHolder" runat="Server">
    <div class="dashboard">
        <div class="dashboard-row">
            <div class="dashboard-col">
                <a href="<%=VBasketballUrl%>">
                    <img src="/_Static/Images/sports/bnr-vSports-Basketball.jpg" class="img-responsive">
                    <span data-i18n="LABEL_VSPORTS_BASKETBALL"></span>
                </a>
            </div>
            <div class="dashboard-col">
                <a href="<%=VFootballlUrl%>">
                    <img src="/_Static/Images/sports/bnr-vSports-soccer.jpg" class="img-responsive">
                    <span data-i18n="LABEL_VSPORTS_FOOTBALL"></span>
                </a>
            </div>
        </div>
    </div>
    <div class="row text-center">
        <div class="note" hidden data-i18n="LABEL_VSPORTS_NOTICE"></div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="InnerScriptPlaceHolder" runat="Server">

    <script type="text/javascript">
        $(function () {

            (function (a) { (jQuery.browser = jQuery.browser || {}).android = /android|(android|bb\d+|meego).+mobile/i.test(a) })(navigator.userAgent || navigator.vendor || window.opera);

            if ($.browser.android) {
                $('.note').show();
            } else {
                $('.note').hide();
            }
        });

        $(document).ready(function () {
            $('.header-title').first().text($.i18n("LABEL_PRODUCTS_VSPORTS"));

        });
    </script>
</asp:Content>

