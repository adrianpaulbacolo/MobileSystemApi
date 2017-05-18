<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Site.master" AutoEventWireup="true" CodeFile="Lottery.aspx.cs" Inherits="Lottery" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="ui-content" role="main">
        <div class="lottery-banner">
            <img src="/_Static/Images/lottery/keno-banner.jpg" class="img-responsive img-bg">
            <div class="banner-caption">
                <div class="banner-caption-content">
                    <h3 class="title"><%=commonCulture.ElementValues.getResourceXPathString("Products/Keno/Label", commonVariables.ProductsXML)%></h3>
                    <p><%=commonCulture.ElementValues.getResourceString("kenoMessage", commonVariables.LeftMenuXML)%></p>
                    <a href="<%=(string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId) ? "_Secure/Login.aspx" : commonLottery.getKenoUrl)%>" class="ui-btn btn-primary" target="_blank"><%=commonCulture.ElementValues.getResourceString("playNow", commonVariables.LeftMenuXML)%></a>
                </div>
            </div>
        </div>
        <div class="lottery-banner">
            <img src="/_Static/Images/lottery/lottery-PK10.jpg" class="img-responsive img-bg">
            <div class="banner-caption banner-caption-2">
                <div class="banner-caption-content">
                    <h3 class="title"><%=commonCulture.ElementValues.getResourceXPathString("Products/PK10/Label", commonVariables.ProductsXML)%></h3>
                    <p><%=commonCulture.ElementValues.getResourceString("pk10Message", commonVariables.LeftMenuXML)%></p>
                    <a href="<%=commonLottery.getPK10Url(true)%>" class="ui-btn btn-primary" target="_blank"><%=commonCulture.ElementValues.getResourceString("playNow", commonVariables.LeftMenuXML)%></a>
                    <a href="<%=commonLottery.getPK10Url(false)%>" class="ui-btn btn-secondary" target="_blank"><%=commonCulture.ElementValues.getResourceString("tryNow", commonVariables.LeftMenuXML)%></a>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

