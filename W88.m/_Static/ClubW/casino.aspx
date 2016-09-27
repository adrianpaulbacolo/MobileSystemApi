<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Site.master" AutoEventWireup="true" CodeFile="Casino.aspx.cs" Inherits="_Static.ClubW.StaticClubWCasino" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="ui-content" role="main">
        <div class="static-content">
            <div class="banner slider">
                <img src="/_Static/Images/Download/ClubW-Android-iOS-Download-Page.jpg" alt="banner" class="img-responsive">
            </div>
            <div class="downloadmsg">
                <span runat="server" id="spanMsg"></span>
                <a href="#" runat="server" target="_blank" rel="CW" data-ajax="false" id="sDownload"></a>
            </div>
        </div>
    </div>
</asp:Content>

