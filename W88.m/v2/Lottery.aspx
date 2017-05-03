<%@ Page Language="C#" MasterPageFile="~/v2/MasterPages/Main.master" AutoEventWireup="true" CodeFile="Lottery.aspx.cs" Inherits="v2_Lottery" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContentHolder" runat="Server">
    <div class="lottery-main">
        <div class="container container-small container-extra-thin">
            <div class="row extra-thin-gutter">
                <div class="col-xs-12">
                    <img src="/_Static/Images/lottery/keno-banner.jpg" class="img-responsive img-bg  pull-left" style="position: absolute" />
                    <div class="col-xs-12">
                        <div class="col-xs-6 pull-left"></div>
                        <div class="col-xs-6 pull-right desc">
                            <h3 class="title"><%=commonCulture.ElementValues.getResourceXPathString("Products/Keno/Label", commonVariables.ProductsXML)%></h3>
                            <p><%=commonCulture.ElementValues.getResourceString("kenoMessage", commonVariables.LeftMenuXML)%></p>
                            <a href="<%=(string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId) ? "_Secure/Login.aspx" : commonLottery.getKenoUrl)%>" class="btn btn-primary" target="_blank"><%=commonCulture.ElementValues.getResourceString("playNow", commonVariables.LeftMenuXML)%></a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <style>
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
        }
    </style>
</asp:Content>

<asp:Content ID="InnerScriptBottom" ContentPlaceHolderID="InnerScriptPlaceHolder" runat="server">
</asp:Content>
