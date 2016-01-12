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

                $('.div-product').each(function () { var scrollObj = new IScroll('#' + $(this).attr('id'), { eventPassthrough: true, scrollX: true, scrollY: false, preventDefault: false, speedRatioX: 9000 }); cache.push(scrollObj); });
                $('.bkg-game').each(function () {
                    var $this = $(this);
                    $this.prepend('<img src="/_Static/Images/ClubPalazzo/' + $this.attr('rel') + '" class="img-responsive-full">')
                });
                $("img").error(function () {
                    $(this).unbind("error").attr("src", "/_Static/Images/broken-lt.gif");
                });

                var isSafari = navigator.userAgent.indexOf('Safari') != -1 && navigator.userAgent.indexOf('Chrome') == -1 && navigator.userAgent.indexOf('Android') == -1;
                if (isSafari)
                {
                    var palazzoDL = Cookies().getCookie('palazzo_download')

                    if (palazzoDL == '' || palazzoDL == '0' || parseInt(palazzoDL) == 0) {
                        $('#palazzoModal').popup();
                        $('#palazzoModal').popup('open');
                    }
                    else {
                        $('#noShowPalazzoModal').attr('checked', 'checked');
                    }
                }
                
                $('#noShowPalazzoModal').click(function () {
                    if ($('#noShowPalazzoModal').is(':checked')) {
                        Cookies().setCookie('palazzo_download', '1', 365);
                    }
                    else {
                        Cookies().setCookie('palazzo_download', '0', 0);
                    }
                });

            });
        </script>
        <!--#include virtual="~/_static/navMenu.shtml" -->

    </div>
</body>
</html>
