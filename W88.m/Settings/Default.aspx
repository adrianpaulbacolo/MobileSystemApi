<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Site.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Settings.SettingsDefault" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="ui-content" role="main">

        <ul class="list fixed-tablet-size">
            <li class="item item-icon-left item-icon-right item-border-btm">
                <a href="ChangePassword.aspx" data-ajax="false">
                    <i class="icon icon-password"></i>
                    <h4 class="title"><%=commonCulture.ElementValues.getResourceString("changePassword", commonVariables.LeftMenuXML)%></h4>
                    <i class="icon ion-ios-arrow-right"></i>
                </a>
            </li>
            <li class="item item-icon-left item-icon-right item-border-btm">
                <a href="../ContactUs.aspx" data-ajax="false">
                    <i class="icon icon-phone"></i>
                    <h4 class="title"><%=commonCulture.ElementValues.getResourceString("contactUs", commonVariables.LeftMenuXML)%></h4>
                    <i class="icon ion-ios-arrow-right"></i>
                </a>
            </li>
        </ul>

    </div>
</asp:Content>

