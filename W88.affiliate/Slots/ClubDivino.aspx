<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ClubDivino.aspx.cs" Inherits="Slots_ClubDivino" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title><%=commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML) + commonCulture.ElementValues.getResourceXPathString("Products/ClubDivino/Label", commonVariables.ProductsXML)%></title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/Js/Main.js"></script>
    <!--[if IE]><link type="text/css" href="/_Static/Css/Slots.css" rel="stylesheet" /><![endif]-->
    <![if !IE]><link type="text/css" href="/_Static/Css/SlotsScroll.css" rel="stylesheet" /><![endif]>
</head>
<body>
    <!--#include virtual="~/_static/splash.shtml" -->
    <div data-role="page" data-theme="a">
        <!--#include virtual="~/_static/header.shtml" -->
        <div class="ui-content" role="main">
            <div class="div-page-header"><span><%=commonCulture.ElementValues.getResourceXPathString("/Products/ClubDivino/Label", commonVariables.ProductsXML)%></span></div>

            <div class="page-content">
                <div class="div-content-wrapper" id="divContainer" runat="server">
                </div>
            </div>
        </div>
        <!-- /content -->
        <!--#include virtual="~/_static/footer.shtml" -->
        <script type="text/javascript">
            var cache = [];
            $(function () {
                $('.bkg-game').each(function () { var $this = $(this); $this.css({ backgroundImage: "url('/_Static/Images/ClubDivino/" + $this.attr('rel') + "')" }); });
                $('.div-product').each(function () { var scrollObj = new IScroll('#' + $(this).attr('id'), { eventPassthrough: true, scrollX: true, scrollY: false, preventDefault: false, speedRatioX: 9000 }); cache.push(scrollObj); });

                (function (a) { (jQuery.browser = jQuery.browser || {}).android = /android|(android|bb\d+|meego).+mobile/i.test(a) })(navigator.userAgent || navigator.vendor || window.opera);
                (function (a) { (jQuery.browser = jQuery.browser || {}).wp = /iemobile|windows (ce|phone)/i.test(a) })(navigator.userAgent || navigator.vendor || window.opera);
                (function (a) { (jQuery.browser = jQuery.browser || {}).ios = /ip(hone|od|ad)/i.test(a) })(navigator.userAgent || navigator.vendor || window.opera);

                if ($.browser.mobile) {
                    $('div[type="IOS"]').hide(); $('div[type="ANDROID"]').hide(); $('div[type="WP"]').hide();
                    //if ($.android) { $('div[type="ANDROID"]').show(); }
                    if ($.browser.ios) { $('div[type="IOS"]').show(); }
                    else if ($.browser.wp) { $('div[type="WP"]').show(); }
                    else { $('div[type="ANDROID"]').show(); }
                } else { $('div[type="ANDROID"]').show(); }
            });
        </script>
        <!--#include virtual="~/_static/navMenu.shtml" -->
    </div>
    <!-- /page -->
</body>
</html>
