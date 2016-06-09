<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Slots.master" AutoEventWireup="true" CodeFile="ClubMassimo2.aspx.cs" Inherits="Slots_ClubMassimo" %>

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
            w88Mobile.Slots.club = "ClubMassimo";
            w88Mobile.Slots.init();
            w88Mobile.Slots.filterDisplay();
        });
    </script>
</asp:Content>

