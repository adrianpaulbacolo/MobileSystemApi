<%@ Page Language="C#" MasterPageFile="~/MasterPages/Base.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="SpinWheel_Default" Async="true"%>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link rel="stylesheet" href="/_Static/Css/spinwheel/spinwheel.css" />
    <script type="text/javascript" src="/_Static/JS/modules/spinwheel.js"></script>
    <script type="text/javascript"> 
        var translations = {
            message5: '',
            claimMessage: '',
            wonNothing: '',
            countdownDay: '',
            countdownHour: '',
            countdownMin: '',
            countdownSec: '',
            successfulClaim: '',
            spinsLeftLabel1: '',
            spinsLeftLabel2: '',
        },
        md = JSON.parse('<%=SpinWheelRequest%>'),
        pic = {
            w: 408,
            h: 328,
            a: -103,
            b: -25
        };   
        var sw = new SW(md, translations, null, pic, true, <%=Convert.ToString(HasSession).ToLower()%>);
    </script>
    <div class="main-content" role="main">
        <div id="spinWheelContainer">
            <div id="spinWheelContent">
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
            </div>
        </div>
    </div>    
</asp:Content>