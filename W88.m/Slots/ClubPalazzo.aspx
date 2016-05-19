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
        $(function () {

            w88Mobile.Slots.club = "ClubPalazzo";
            w88Mobile.Slots.initPalazzo();

        });
    </script>
</asp:Content>

