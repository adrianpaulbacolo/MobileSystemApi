<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ClubPalazzo.aspx.cs" Inherits="Slots_ClubPalazzo" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title><%=commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML) + commonCulture.ElementValues.getResourceXPathString("Products/ClubPalazzoSlots/Label", commonVariables.ProductsXML)%></title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/Js/Main.js"></script>

    <script type="text/javascript">
        function load_palazzo_link(type, name, mode) {
            palazzo_window = window.open("/Slots/ClubPalazzoLauncher.aspx?type=" + type + "&name=" + name + "&mode=" + mode, 'Palazzo');
        }
    </script>
</head>
<body>
    <!--#include virtual="~/_static/splash.shtml" -->
    <div data-role="page" data-theme="b" id="slots">
        <header data-role="header" data-theme="b" data-position="fixed" id="header">
            <a class="btn-clear ui-btn-left ui-btn" href="#divPanel" data-role="none" role="button" id="aMenu" data-load-ignore-splash="true">
                <i class="icon-navicon"></i>
            </a>
            <h1 class="title"><%=commonCulture.ElementValues.getResourceXPathString("/Products/ClubPalazzoSlots/Label", commonVariables.ProductsXML)%></h1>
        </header>
        <div class="ui-content" role="main">
            <div id="divContainer" runat="server"></div>
        </div>

        <script type="text/javascript">
            var cache = [];
            $(function () {
                // $('.bkg-game').each(function () { var $this = $(this); $this.css({ backgroundImage: "url('/_Static/Images/ClubBravado/" + $this.attr('rel') + "')" }); });
                $('.div-product').each(function () { var scrollObj = new IScroll('#' + $(this).attr('id'), { eventPassthrough: true, scrollX: true, scrollY: false, preventDefault: false, speedRatioX: 9000 }); cache.push(scrollObj); });
                $('.bkg-game').each(function () {
                    var $this = $(this);
                    $this.prepend('<img src="/_Static/Images/ClubPalazzo/' + $this.attr('rel') + '" class="img-responsive-full">')
                });
                $("img").error(function () {
                    $(this).unbind("error").attr("src", "/_Static/Images/broken-lt.gif");
                });

                $.mobile.changePage("#palazzoDialog", {
                    role: "dialog",
                    changeHash: false
                });

            });
        </script>
        <!--#include virtual="~/_static/navMenu.shtml" -->

    </div>

        <div id="palazzoDialog" data-rel="page" data-dialog="true" data-close-btn="right">
            <div data-role="header">
                <h1 class="title">
                    <img src="/_Static/Images/logo-<%=commonVariables.SelectedLanguageShort%>.png" class="logo" alt="logo">
                </h1>
            </div>
            <div role="main" class="ui-content">
                <div class="title blue">3 Easy Steps to Play Club Palazzo Download Version</div>
                <ul>
                    <li><b>Step 1:</b>&nbsp;&nbsp;Click the Download button below.</li>
                    <li><b>Step 2:</b>&nbsp;&nbsp;Install by clicking Run or Save it in your local hard drive.</li>
                    <li><b>Step 3:</b>&nbsp;&nbsp;Login by adding "<span class="blue">w88</span>" prefix in front of your web username.</li>
                </ul>
                <br />
                Example:
            <br />
                If your web username is "luckyplayer", then login as "<span class="blue">W88</span>LUCKYPLAYER".
            <br />
                <br />
                <i>Installation process requires the latest <a href="http://get.adobe.com/flashplayer/otherversions" target="_blank">Adobe Flash Player</a> (IE browser).</i>
                <br />
                <br />
                <div class="center">
                    <a href="http://cdn.w88palazzo.com/d/setupglx.exe" class="button big">DOWNLOAD NOW</a>
                </div>
                <div class="clear" style="margin: 8px 0 0;">
                    <input type="checkbox" id="palazzo_msg_chk" style="margin: 0;" />&nbsp;<label for="palazzo_msg_chk">Do not show again</label>
                </div>
            </div>
        </div>
</body>
</html>
