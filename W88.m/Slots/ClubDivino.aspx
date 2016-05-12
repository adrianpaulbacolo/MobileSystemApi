<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Slots.master" AutoEventWireup="true" CodeFile="ClubDivino.aspx.cs" Inherits="Slots_ClubDivino" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="ui-content" role="main">
        <div id="divContainer" runat="server"></div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsPlaceHolder" runat="Server">
    <script type="text/javascript">
        var cache = [];
        $(function () {
            // $('.bkg-game').each(function () { var $this = $(this); $this.css({ backgroundImage: "url('/_Static/Images/ClubDivino/" + $this.attr('rel') + "')" }); });
            $('.div-product').each(function () { var scrollObj = new IScroll('#' + $(this).attr('id'), { eventPassthrough: true, scrollX: true, scrollY: false, preventDefault: false, speedRatioX: 9000 }); cache.push(scrollObj); });
            $('.bkg-game').each(function () {
                var $this = $(this);
                $this.prepend('<img src="/_Static/Images/ClubDivino/' + $this.attr('rel') + '" class="img-responsive-full">')
            });
            $("img").error(function () {
                $(this).unbind("error").attr("src", "/_Static/Images/broken-lt.gif");
            });

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

</asp:Content>

