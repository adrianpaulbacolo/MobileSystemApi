<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Site.master" AutoEventWireup="true" CodeFile="ContactUs.aspx.cs" Inherits="_ContactUs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="ui-content contact-us" role="main">
        <ul class="list fixed-tablet-size" data-role="listview" data-icon="false">
            <li class="item item-icon-left">
                <i class="icon icon-chat"></i>
                <a id="aLiveChat" runat="server" href="/LiveChat" data-ajax="false">
                    <h4 class="title"><%=commonCulture.ElementValues.getResourceString("lblLiveChat", xeResources)%></h4>
                    <p><%=commonCulture.ElementValues.getResourceString("lblLiveChatMessage", xeResources)%></p>
                </a>
            </li>
            <li class="item item-icon-left">
                <i class="icon icon-skype"></i>
                <a id="aSkype" runat="server" href="javascript:void(0);">
                    <h4 class="title"><%=commonCulture.ElementValues.getResourceString("lblSkype", xeResources)%></h4>
                    <p><%=commonCulture.ElementValues.getResourceString("lblSkypeMessage", xeResources)%></p>
                </a>
            </li>
            <li class="item item-icon-left">
                <i class="icon icon-mail"></i>
                <a id="aEmail" runat="server" href="javascript:void(0);">
                    <h4 class="title"><%=commonCulture.ElementValues.getResourceString("lblEmail", xeResources)%></h4>
                    <p><%=commonCulture.ElementValues.getResourceString("lblEmailMessage", xeResources)%></p>
                </a>
            </li>
            <li class="item item-icon-left">
                <i class="icon icon-banking"></i>
                <a id="aBanking" runat="server" href="javascript:void(0);">
                    <h4 class="title"><%=commonCulture.ElementValues.getResourceString("lblBanking", xeResources)%></h4>
                    <p><%=commonCulture.ElementValues.getResourceString("lblBankingMessage", xeResources)%></p>
                </a>
            </li>
            <li class="item item-icon-left" id="liPhone" runat="server">
                <i class="icon icon-phone"></i>
                <a id="aPhone" runat="server" href="/_Secure/Login.aspx">
                    <h4 class="title"><%=commonCulture.ElementValues.getResourceString("lblPhone", xeResources)%></h4>
                    <p id="phoneMessage" runat="server"></p>
                </a>
            </li>
            <%--
                <li class="item item-icon-left">
                    <a id="aLine" runat="server" href="javascript:void(0);" data-rel="dialog" data-transition="slidedown">
                        <h4><%=commonCulture.ElementValues.getResourceString("lblLine", xeResources)%></h4>
                    </a>
                </li>
                --%>
        </ul>
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsPlaceHolder" runat="server">


</asp:Content>
