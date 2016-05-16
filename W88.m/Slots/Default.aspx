<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Slots.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Slots_Default" %>

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
            // $('.bkg-game').each(function () { var $this = $(this); $this.css('background-image', "url('/_Static/Images/ClubCrescendo/" + $this.attr('rel') + "')"); });
            $('.div-product').each(function () { var scrollObj = new IScroll('#' + $(this).attr('id'), { eventPassthrough: true, scrollX: true, scrollY: false, preventDefault: false, speedRatioX: 9000 }); cache.push(scrollObj); });
            $('.bkg-game').each(function () {
                var $this = $(this);
                $this.prepend('<img src="/_Static/Images/ClubCrescendo/' + $this.attr('rel') + '" class="img-responsive-full">')
            });
            $("img").error(function () {
                $(this).unbind("error").attr("src", "/_Static/Images/broken-lt.gif");
            });
        });
        </script>
</asp:Content>

