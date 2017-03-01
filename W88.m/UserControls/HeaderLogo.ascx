<%@ Control Language="C#" AutoEventWireup="true" CodeFile="HeaderLogo.ascx.cs" Inherits="UserControls.UserControlsHeaderonLogo" ClientIDMode="static" %>

<header id="header" data-role="header" data-position="fixed" data-theme="b" data-tap-toggle="false">
    <% if (commonCookie.CookieIsApp != "1")
       { %>
    <a class="btn-clear ui-btn-left ui-btn" href="#divPanel" data-role="none" role="button" id="aMenu" data-load-ignore-splash="true">
        <% if (commonFunctions.isNativeAgent(Request) && commonFunctions.isMainPage(Request) && !Request.Url.AbsolutePath.ToLower().Contains("index"))
           { %>
        <i class="icon-back"></i>
        <% }
           else
           { %>
        <i class="icon-navicon"></i>
        <% } %>
    </a>
    <asp:HyperLink ID="aMenu" runat="server" Visible="false" href="" role="button" data-rel="back" class="btn-clear ui-btn-left ui-btn ion-ios-arrow-back" data-load-ignore-splash="true">Back</asp:HyperLink>
    <asp:HyperLink ID="cancel" runat="server" Visible="false" role="button" data-rel="back" class="btn-clear ui-btn-right ui-btn">Cancel &nbsp;</asp:HyperLink>
    <% } %>
    <h1 class="title" id="headerTitle">
        <asp:Panel ID="logo" runat="server" Visible="False">
            <img src="/_Static/Images/logo-<%=commonVariables.SelectedLanguageShort%>.png" class="logo" alt="logo"></asp:Panel>
        <asp:Literal runat="server" ID="ltrTitle" Visible="False" />
    </h1>
</header>
