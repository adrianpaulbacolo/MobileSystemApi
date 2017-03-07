<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Site.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Profile.ProfileDefault" %>

<%@ Register TagPrefix="uc" TagName="Wallet" Src="~/UserControls/MainWalletBalance.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="/_Static/JS/modules/bankDetails.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="ui-content" role="main">
        <div class="row row-no-padding">
            <div class="col">
                <div class="wallet main-wallet">
                    <uc:Wallet ID="uMainWallet" runat="server" />
                </div>
            </div>
            <div class="col">
                <div class="wallet main-wallet rewards">
                    <label class="label"><%=commonCulture.ElementValues.getResourceString("rewardsClub", commonVariables.LeftMenuXML)%></label>
                    <h2 class="value"><%=getCurrentPoints().ToString() %></h2>
                    <small class="currency"><%=commonCulture.ElementValues.getResourceString("points", commonVariables.LeftMenuXML)%></small>
                </div>
            </div>
        </div>

        <ul class="row row-bordered bg-gradient">
            <li class="col col-33">
                <a href="../Funds.aspx" class="tile" data-ajax="false" data-transition="slidedown">
                    <span class="icon- ion-social-usd-outline"></span>
                    <h4 class="title"><%=commonCulture.ElementValues.getResourceString("fundmanagement", commonVariables.LeftMenuXML)%></h4>
                </a>
            </li>
            <li class="col col-33">
                <a href="../_Secure/BankDetails.aspx" class="tile" runat="server" data-ajax="false">
                    <span class="icon icon-banking"></span>
                    <h4 class="title" id="bankDetails"></h4>
                </a>
            </li>
            <li class="col col-33">
                <a href="Rebates.aspx" class="tile" runat="server" data-ajax="false">
                    <span class="icon icon-rebates"></span>
                    <h4 class="title" id="rebates"></h4>
                </a>
                <li class="col col-33">
                    <a href="/LiveChat/Default.aspx" class="tile" runat="server" data-ajax="false">
                        <span class="icon-chat"></span>
                        <h4 class="title"><%=commonCulture.ElementValues.getResourceString("liveHelp", commonVariables.LeftMenuXML)%></h4>
                    </a>
                </li>
            <li class="col col-33">
                <a href="/Upload/Default.aspx" class="tile" data-ajax="false">
                    <span class="icon-submit"></span>
                    <h4 class="title"><%=commonCulture.ElementValues.getResourceString("submitUpload", commonVariables.LeftMenuXML)%></h4>
                </a>
            </li>
            <%--<li class="col col-33">
                    <a href="../Settings" class="tile" data-ajax="false">
                        <span class="icon-settings"></span>
                        <h4 class="title">Settings</h4>
                    </a>
                </li>--%>
            <li class="col col-33">
                <a href="../Settings/ChangePassword.aspx" class="tile" runat="server" data-ajax="false">
                    <span class="icon-password"></span>
                    <h4 class="title"><%=commonCulture.ElementValues.getResourceString("changePassword", commonVariables.LeftMenuXML)%></h4>
                </a>
            </li>
            <li class="col col-33">
                <a href="../ContactUs.aspx" class="tile" runat="server" data-ajax="false">
                    <span class="icon-phone"></span>
                    <h4 class="title"><%=commonCulture.ElementValues.getResourceString("contactUs", commonVariables.LeftMenuXML)%></h4>
                </a>
            </li>
        </ul>

        <script type="text/javascript">
            window.w88Mobile.BankDetails.Translations(function (response) {
                if (response && _.isEqual(response.ResponseCode, 1)) {
                    $('#bankDetails').text(response.ResponseData.LABEL_MENU_BANK_DETAILS); 
                    $('#rebates').text(response.ResponseData.LABEL_MENU_REBATES);
                }
            });
        </script>
    </div>
</asp:Content>
