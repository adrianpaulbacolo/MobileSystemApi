<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Site.master" AutoEventWireup="true" CodeFile="Slots.aspx.cs" Inherits="_Static.Palazzo.StaticPalazzoSlots" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    <div class="ui-content" role="main">
        <div class="static-content">
            <div class="banner slider">
                <img src="/_Static/Images/Download/W88-Mobile-ClubPalazzo-Slots.jpg" alt="banner" class="img-responsive">
            </div>
            <div class="downloadmsg">
                <span runat="server" id="spanMsg"></span>
                <a href="http://mgames.w88palazzo.com" id="sDownload" class="ui-btn btn-primary" runat="server" data-ajax="false" target="_blank"></a>
            </div>
        </div>
    </div>

</asp:Content>

