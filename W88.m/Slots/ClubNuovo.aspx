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
            var translations = amplify.store("translations");
            setTranslations(translations);

            function setTranslations(data) {

                if (!_.isUndefined(data)) {
                    $("#header .ui-title").first().text(data.LABEL_PRODUCTS_CLUB_NUOVO);
                }
            }
        });

    </script>

</asp:Content>

