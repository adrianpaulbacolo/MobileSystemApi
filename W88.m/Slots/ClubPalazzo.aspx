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

        <!--#include virtual="~/_static/footer.shtml" -->
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
            });
        </script>
        <!--#include virtual="~/_static/navMenu.shtml" -->

    </div>
</body>
</html>
