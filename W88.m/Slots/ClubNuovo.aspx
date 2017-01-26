<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Slots.master" AutoEventWireup="true" CodeFile="ClubNuovo.aspx.cs" Inherits="Slots_ClubNuovo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <div class="ui-content" role="main">
            <div id="divContainer" runat="server"></div>
        </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsPlaceHolder" Runat="Server">
    <script type="text/javascript">
        $(function () {
            w88Mobile.Slots.club = "ClubNuovo";
            w88Mobile.Slots.init();
        });

        $(document).ready(function () {

            setTranslations();
            function setTranslations() {
                if (_w88_contents.translate("LABEL_PRODUCTS_CLUB_NUOVO") != "LABEL_PRODUCTS_CLUB_NUOVO") {
                    $("#header .ui-title").text(_w88_contents.translate("LABEL_PRODUCTS_CLUB_NUOVO"));
                } else {
                    window.setInterval(function () {
                        setTranslations();
                    }, 500);
                }
            }
        });
    </script>

</asp:Content>

