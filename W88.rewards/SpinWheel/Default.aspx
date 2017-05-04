<%@ Page Language="C#" MasterPageFile="~/MasterPages/Base.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="SpinWheel_Default" Async="true"%>
<%@ Import Namespace="W88.BusinessLogic.Rewards.Helpers" %>
<%@ Import Namespace="W88.BusinessLogic.Rewards.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link rel="stylesheet" href="/_Static/Css/spinwheel/spinwheel.css" />
    <script type="text/javascript" src="/_Static/JS/Mobile/jquery-ui.min.js"></script>
    <script type="text/javascript" src="/_Static/JS/vendor/amplify.min.js"></script>
    <script type="text/javascript" src="/_Static/JS/modules/spinwheel.js"></script>
    <script type="text/javascript"> 
        var date = new Date('<%=DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")%>'),
            translations = {
                message5: '<%=RewardsHelper.GetTranslation(TranslationKeys.SpinWheel.ApplicationError, Language)%>',
                claimMessage: '<%=RewardsHelper.GetTranslation(TranslationKeys.SpinWheel.ClaimPrize, Language)%>',
                wonNothing: '<%=RewardsHelper.GetTranslation(TranslationKeys.SpinWheel.NoPrizeWon, Language)%>',
                countdownDay: '<%=RewardsHelper.GetTranslation(TranslationKeys.Label.Day, Language)%>',
                countdownHour: '<%=RewardsHelper.GetTranslation(TranslationKeys.Label.Hour, Language)%>',
                countdownMin: '<%=RewardsHelper.GetTranslation(TranslationKeys.Label.Minute, Language)%>',
                countdownSec: '<%=RewardsHelper.GetTranslation(TranslationKeys.Label.Second, Language)%>',
                successfulClaim: '<%=RewardsHelper.GetTranslation(TranslationKeys.SpinWheel.RedemptionSuccessful, Language)%>',
                spinsLeftLabel1: '<%=RewardsHelper.GetTranslation(TranslationKeys.SpinWheel.HeaderLabel1, Language)%>',
                spinsLeftLabel2: '<%=RewardsHelper.GetTranslation(TranslationKeys.SpinWheel.HeaderLabel2, Language)%>',
                prizeListUpdated: '<%=RewardsHelper.GetTranslation(TranslationKeys.SpinWheel.PrizesUpdated, Language)%>'
            },
            md = JSON.parse('<%=SpinWheelRequest%>');  
        var sw = new SW(md, translations, null, null, true, <%=Convert.ToString(HasSession).ToLower()%>);
        setInterval(function() {
            var current = date.getTime();
            current += 1000;
            date = new Date(current);
        }, 1000);
    </script>
    <div class="main-content" role="main">
        <div id="spinWheelContainer">
            <div id="spinWheelHeader">
                <h3><%=RewardsHelper.GetTranslation(TranslationKeys.SpinWheel.WheelLabel, Language).ToUpper()%></h3>
                <h5><%=HttpUtility.HtmlDecode(RewardsHelper.GetTranslation(TranslationKeys.SpinWheel.Header, Language).Replace("&lt;br /&gt;", " "))%></h5>
                <hr />
            </div>
            <div id="spinWheelContent">               
                <p id="spinsLeft">
                    <%=RewardsHelper.GetTranslation(TranslationKeys.SpinWheel.HeaderLabel1)%><span></span><%=RewardsHelper.GetTranslation(TranslationKeys.SpinWheel.HeaderLabel2, Language)%>
                </p>                    
                <img id="roulette" src="/_Static/Images/spinwheel/spinwheel1.png"/>                
                <div id="spinWheel" class="spinWheel">
                    <canvas id="swc"></canvas>
                    <div id="prizes">
                        <p id="prize1"></p>
                        <p id="prize2" class="rotate45"></p>
                        <p id="prize3" class="rotate90"></p>
                        <p id="prize4" class="rotate135"></p>
                        <p id="prize5" class="rotate180"></p>
                        <p id="prize6" class="rotate225"></p>
                        <p id="prize7" class="rotate270"></p>
                        <p id="prize8" class="rotate315"></p>
                    </div>
                </div>      
                <img id="wheelPointer" src="/_Static/Images/spinwheel/spinwheel3.png"/>
                <a id="spinButton" class="ui-btn btn-primary"><%=RewardsHelper.GetTranslation(TranslationKeys.SpinWheel.SpinButtonLabel, Language).ToUpper()%></a>    
                <div id="countdownContainer">
                    <div class="countdownElement"></div>
                    <div class="countdownElement"></div>
                    <div class="countdownElement"></div>
                    <div class="countdownElement"></div>
                    <div id="nextSpinLabel"><%=RewardsHelper.GetTranslation(TranslationKeys.SpinWheel.NextSpinLabel, Language).ToUpper()%></div>
                </div> 
                <p id="disclaimer">
                    <%=RewardsHelper.GetTranslation(TranslationKeys.SpinWheel.RemarkFree, Language)%><br/>
                    <%=RewardsHelper.GetTranslation(TranslationKeys.SpinWheel.RemarkNonRefundable, Language)%>
                </p>     
                <div id="prizeModal" class="fade left modal" tabindex="-1" data-backdrop="static">
	                <div class="prizeModal">
	                    <div id="prizeContainer">
                            <canvas id="pic"></canvas>
                            <p id="spinMessage"></p>
                            <img src="/_static/Images/Spinwheel/spinwheel4.png"/>  
                            <a id="claimButton" href="#" class="ui-btn btn-primary" onclick="javascript: _rp_();"><%=RewardsHelper.GetTranslation(TranslationKeys.Label.Claim, Language).ToUpper()%></a> 
                            <a id="okButton" href="#" class="ui-btn btn-primary" onclick="javascript: _tp_();"><%=RewardsHelper.GetTranslation(TranslationKeys.Label.Ok, Language).ToUpper()%></a> 
                        </div>                    
	                </div>
                </div>
            </div>
        </div>
    </div>    
</asp:Content>