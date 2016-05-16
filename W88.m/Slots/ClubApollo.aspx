<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Slots.master" AutoEventWireup="true" CodeFile="ClubApollo.aspx.cs" Inherits="Slots_ClubApollo" %>
<%@OutputCache Duration="21600" VaryByParam="none" Location="Client" %>

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
                w88Mobile.Slots.club = "ClubApollo";
                w88Mobile.Slots.init();
            });
        </script>
</asp:Content>

