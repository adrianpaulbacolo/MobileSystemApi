<%@ Page Language="C#" MasterPageFile="~/v2/MasterPages/Main.master" AutoEventWireup="true" CodeFile="Lottery.aspx.cs" Inherits="v2_Lottery" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContentHolder" runat="Server">
    <div class="lottery-main">
        <div class="container container-small container-extra-thin">
            <div class="row extra-thin-gutter">
                <div class="col-xs-12 lottery-item">
                    <img src="/_Static/Images/lottery/keno-banner.jpg" class="img-responsive img-bg  pull-left col-xs-12" style="position: absolute" />
                    <div class="col-xs-12">
                        <div class="col-xs-6 pull-left"></div>
                        <div class="col-xs-6 pull-right desc">
                            <h3 class="title" data-i18n="LABEL_PRODUCTS_KENO"></h3>
                            <p><%=commonCulture.ElementValues.getResourceString("kenoMessage", commonVariables.LeftMenuXML)%></p>
                            <a href="<%=(string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId) ? "/_Secure/Login.aspx" : commonLottery.getKenoUrl)%>" class="btn btn-primary" target="_blank" data-i18n="BUTTON_PLAY_NOW"></a>
                        </div>
                    </div>
                </div>
                <div class="col-xs-12 lottery-item">
                    <img src="/_Static/Images/lottery/lottery-PK10.jpg" class="img-responsive img-bg  pull-left col-xs-12" style="position: absolute" />
                    <div class="col-xs-12">
                        <div class="col-xs-6 pull-left desc">
                            <h3 class="title" data-i18n="LABEL_PRODUCTS_PK10"></h3>
                            <p data-i18n="PK10_DESCRIPTION"></p>
                            <a href="<%=commonLottery.getPK10Url(true)%>" class="btn btn-primary" target="_blank" data-i18n="BUTTON_PLAY_NOW"></a>
                            <a href="<%=commonLottery.getPK10Url(false)%>" class="btn btn-default" target="_blank" data-i18n="BUTTON_TRY_NOW"></a>
                        </div>
                        <div class="col-xs-6 pull-right"></div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <style>
        .lottery-main > div.container{
            margin: 0 auto;
            padding: 0;
        }
        .lottery-main img.img-responsive{
            padding: 0;
        }
        .lottery-main h3.title {
            color: #dab867;
            font-size: 1.5rem;
            margin-top: 1rem;
        }
        .lottery-main div.desc p {
            font-size: .7rem;
        }
        .lottery-main div.desc a {
            width: 100%;
            padding: 8px;
            margin: 1px 0;
        }
        .lottery-item{
            display: block;
            background-color: #0b0b0b;
            position: relative;
            overflow: hidden;
            height: 0;
            padding-bottom: 50%;
        }
    </style>
</asp:Content>

<asp:Content ID="InnerScriptBottom" ContentPlaceHolderID="InnerScriptPlaceHolder" runat="server">
</asp:Content>
