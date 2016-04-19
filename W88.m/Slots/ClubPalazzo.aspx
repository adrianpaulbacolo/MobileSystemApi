<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Slots.master" AutoEventWireup="true" CodeFile="ClubPalazzo.aspx.cs" Inherits="Slots_ClubPalazzo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function load_palazzo_link(type, name, mode) {
            palazzo_window = window.open("/Slots/ClubPalazzoLauncher.aspx?type=" + type + "&name=" + name + "&mode=" + mode, 'Palazzo');
        }
    </script>
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

            $('.div-product').each(function () { var scrollObj = new IScroll('#' + $(this).attr('id'), { eventPassthrough: true, scrollX: true, scrollY: false, preventDefault: false, speedRatioX: 9000 }); cache.push(scrollObj); });
            $('.bkg-game').each(function () {
                var $this = $(this);
                $this.prepend('<img src="/_Static/Images/ClubPalazzo/' + $this.attr('rel') + '" class="img-responsive-full">')
            });
            $("img").error(function () {
                $(this).unbind("error").attr("src", "/_Static/Images/broken-lt.gif");
            });

            var isSafari = navigator.userAgent.indexOf('Safari') != -1 && navigator.userAgent.indexOf('Chrome') == -1 && navigator.userAgent.indexOf('Android') == -1;
            if (isSafari) {
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

            var isCloseByClicked = false;

            $('#palazzoModalClose').click(function () {
                isCloseByClicked = true;
            });

            $("#palazzoModal").on("popupafterclose", function () {
                if (!isCloseByClicked) {
                    $('#palazzoModal').popup('open');
                }
            });

        });
    </script>
</asp:Content>

