<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Slots.master" AutoEventWireup="true" CodeFile="ClubBravado2.aspx.cs" Inherits="Slots_ClubBravado" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

        <div class="ui-content" role="main">
            <div id="divContainer" runat="server"></div>
        </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsPlaceHolder" runat="Server">
    <script type="text/javascript">
            $(function () {
                w88Mobile.Slots.club = "ClubBravado";
                w88Mobile.Slots.init();
            });
        </script>
</asp:Content>

