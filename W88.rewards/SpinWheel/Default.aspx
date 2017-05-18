<%@ Page Language="C#" MasterPageFile="~/MasterPages/Base.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="SpinWheel_Default" Async="true"%>
<%@ Import Namespace="W88.BusinessLogic.Rewards.Helpers" %>
<%@ Import Namespace="W88.BusinessLogic.Rewards.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link rel="stylesheet" href="/_Static/Css/spinwheel/spinwheel.css" />
    <script type="text/javascript" src="/_Static/JS/Mobile/jquery-ui.min.js"></script>
    <script type="text/javascript" src="/_Static/JS/modules/w88.mrewards.pointlevelinfo.min.js"></script>
    <script type="text/javascript" src="/_Static/JS/dist/w88.mrewards.sw.min.js"></script>
    <script type="text/javascript">
        var translations = {
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
            md = JSON.parse('<%=SpinWheelRequest%>'),
            sw = new SW(md, translations, null, null, true, <%=Convert.ToString(HasSession).ToLower()%>, '<%=Language%>');
        $(function() {
            var pointLevelInfo = new PointLevelInfo(<%=PointLevelInfo%>, {
                from: '/_Static/Images/Levels/{0}a.png',
                to: '/_Static/Images/Levels/{0}b.png',
                barBackground: '/_Static/Images/Levels/blink.png'
            });
            pointLevelInfo.getPointLevelInfo();
        });
    </script>
    <div class="main-content" role="main">
        <div id="spinWheelContainer">
            <div id="spinWheelHeader">
                <h3><%=RewardsHelper.GetTranslation(TranslationKeys.SpinWheel.WheelLabel, Language).ToUpper()%></h3>
                <h5><%=HttpUtility.HtmlDecode(RewardsHelper.GetTranslation(TranslationKeys.SpinWheel.Header, Language).Replace("&lt;br /&gt;", " "))%></h5>
                <div id="levelBar">
                    <div id="bar">
                        <ul>
                            <li><img class="ImgFrom" alt="" src="" style="text-align: left"/></li>
                            <li><div class="barBackground"><div class="PointsLevelBar"></div></div></li> 
                            <li><img class="ImgTo" alt="" src="" style="text-align: right"/></li>
                        </ul>
                    </div>           
                    <div class="levelDesc" style="display:none;">                         
                        <span class="levelNormal" id="pointlevelNext"><%=RewardsHelper.GetTranslation(TranslationKeys.SpinWheel.PointLevelBarLabel, Language)%></span>
                        <span class="level8" style="display:none;"><%=RewardsHelper.GetTranslation(TranslationKeys.SpinWheel.PointLevelBarTopLevel, Language)%></span>  
                    </div>                     
                </div>
                <hr />
            </div>
            <div id="spinWheelContent">               
                <p id="spinsLeft">
                    <%=RewardsHelper.GetTranslation(TranslationKeys.SpinWheel.HeaderLabel1, Language)%><span></span><%=RewardsHelper.GetTranslation(TranslationKeys.SpinWheel.HeaderLabel2, Language)%>
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