<%@ Page Language="C#" MasterPageFile="~/v2/MasterPages/Main.master" AutoEventWireup="true" CodeFile="DownloadItem.aspx.cs" Inherits="v2_DownloadItem" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContentHolder" runat="Server">
    <div class="download-coverphoto">
        <img src="<%=ItemBanner %>" alt="">
    </div>
    <div class="download-header">
        <div class="container container-small" runat="server" id="instructionHeader">
        </div>
    </div>
    <div class="download-instructions">
        <div class="container">
            <p runat="server" id="instructions"></p>
        </div>
    </div>
    <div class="download-button">
        <div class="container">
            <a href="<%=ItemLink %>" class="btn btn-block btn-primary" runat="server" id="downloadlink"></a>
        </div>
    </div>
</asp:Content>
