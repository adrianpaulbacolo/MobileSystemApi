<%@ Page Language="C#" MasterPageFile="~/v2/MasterPages/Main.master" AutoEventWireup="true" CodeFile="Lottery.aspx.cs" Inherits="v2_Lottery" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContentHolder" runat="Server">
    <div class="lottery-main">
        <div class="container container-small container-extra-thin">
            <div class="lottery-banner">
                <img src="/_Static/Images/lottery/keno-banner.jpg">
                <div class="banner-caption">
                    <div class="banner-caption-content">
                        <h3 class="title"><%=commonCulture.ElementValues.getResourceXPathString("Products/Keno/Label", commonVariables.ProductsXML)%></h3>
                        <p><%=commonCulture.ElementValues.getResourceString("kenoMessage", commonVariables.LeftMenuXML)%></p>
                        <a href="<%=(string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId) ? "_Secure/Login.aspx" : commonLottery.getKenoUrl)%>" class="btn btn-primary" target="_blank"><%=commonCulture.ElementValues.getResourceString("playNow", commonVariables.LeftMenuXML)%></a>
                    </div>
                </div>
            </div>
            <div class="lottery-banner">
                <img src="/_Static/Images/lottery/lottery-PK10.jpg">
                <div class="banner-caption banner-caption-2">
                    <div class="banner-caption-content">
                        <h3 class="title"><%=commonCulture.ElementValues.getResourceXPathString("Products/PK10/Label", commonVariables.ProductsXML)%></h3>
                        <p><%=commonCulture.ElementValues.getResourceString("pk10Message", commonVariables.LeftMenuXML)%></p>
                        <a href="<%=commonLottery.getPK10Url(true)%>" class="btn btn-primary" target="_blank"><%=commonCulture.ElementValues.getResourceString("playNow", commonVariables.LeftMenuXML)%></a>
                        <a href="<%=commonLottery.getPK10Url(false)%>" class="btn btn-gray" target="_blank"><%=commonCulture.ElementValues.getResourceString("tryNow", commonVariables.LeftMenuXML)%></a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="InnerScriptBottom" ContentPlaceHolderID="InnerScriptPlaceHolder" runat="server">
</asp:Content>
