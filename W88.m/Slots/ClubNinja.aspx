<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Slots.master" AutoEventWireup="true" CodeFile="ClubNinja.aspx.cs" Inherits="Slots_ClubNinja" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="https://login.goldenphoenix88.com/jswrapper/integration.js.php?casino=blacktiger88"></script>
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

