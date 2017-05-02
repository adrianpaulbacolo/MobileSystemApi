<%@ Page Language="C#" MasterPageFile="~/MasterPages/Base.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="SpinWheel_Default" Async="true"%>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="/_Static/JS/modules/spinwheel.js"></script>
    <style type="text/css">
        .main-content {
            background-image: url("/_Static/Images/spinwheel/BG.jpg");
            background-repeat: repeat;
        }
        #spinWheelContainer {
            width: 100%;
            position: relative;
        }
        #spinWheelContent {
            width: 100%;
            height: 100%;
            text-align: center;
        }
        #roulette {
            width: 100%;
            height: auto;
            position: absolute;
            top: 0;
            bottom: 0;
            left: 0;
            right: 0;
            margin: 190px auto 0;
        }
        #wheelPointer {
            width: 100%;
            height: auto;
            position: absolute;
            top: 0;
            bottom: 0;
            left: 0;
            right: 0;
            margin: 190px auto 0;
        }
        @media (max-width: 1024px) {
            #spinWheelContainer {
                width: 100%;
                height: 80vh;
                position: relative;
                margin: 0 auto;
                top: -125px;
            }
            #roulette {
                width: 59.9%;
            }
            #wheelPointer {
                width: 59.9%;
            }
            #prizes {
                position: absolute;
                top: 207px;
                bottom: 0;
                left: 201px;
                right: 0;
            }
            #prizes p {
                height: 60px;
                width: 106px;
                color: #fff;
                z-index: 1000;
                position: absolute;
                text-align: center;
            }
            #prize1 {
                top: 124px;
                left: 249px;
            } 
            #prize2 {
                top: 163px;
                left: 344px;
            } 
            #prize3 {
                top: 258px;
                left: 384px;
            } 
            #prize4 {
                top: 354px;
                left: 345px;
            } 
            #prize5 {
                top: 393px;
                left: 249px;
            } 
            #prize6 {
                top: 354px;
                left: 154px;
            } 
            #prize7 {
                top: 258px;
                left: 113px;
            } 
            #prize8 {
                top: 162px;
                left: 154px;
            } 
        }
        @media (max-width: 768px) {
            #spinWheelContainer {
                width: 100%;
                height: 80vh;
                position: relative;
                margin: 0 auto;
                top: -210px;
            }
            #roulette {
                width: 80.3%;
            }
            #wheelPointer {
                width: 80.3%;
            }
            #prizes {
                position: absolute;
                top: 205px;
                bottom: 0;
                left: 74px;
                right: 0;
            }
            #prizes p {
                height: 60px;
                width: 106px;
                color: #fff;
                z-index: 1000;
                position: absolute;
                text-align: center;
            }
            #prize1 {
                top: 124px;
                left: 249px;
            } 
            #prize2 {
                top: 163px;
                left: 344px;
            } 
            #prize3 {
                top: 258px;
                left: 384px;
            } 
            #prize4 {
                top: 354px;
                left: 345px;
            } 
            #prize5 {
                top: 393px;
                left: 249px;
            } 
            #prize6 {
                top: 354px;
                left: 154px;
            } 
            #prize7 {
                top: 258px;
                left: 113px;
            } 
            #prize8 {
                top: 162px;
                left: 154px;
            } 
        }
        @media (max-width: 414px) {
            #spinWheelContainer {
                height: 90vh;
            }
            #roulette {
                width: 100%;
            }
            #wheelPointer {
                width: 100%;
            }
            #prizes {
                position: absolute;
                top: 95px;
                bottom: 0px;
                left: -116px;
                right: 0;
            }
            #prizes p {
                height: 20px;
                width: 75px;
                color: #fff;
                z-index: 1000;
                position: absolute;
                text-align: center;
                font-size: 8px;
            }
        }
        @media (max-width: 375px) {
            #spinWheelContainer {
                height: 85vh;
                top: -208px;
            }
            #roulette {
                width: 100%;
            }
            #wheelPointer {
                width: 100%;
            }
            #prizes {
                position: absolute;
                top: 103px;
                bottom: 0px;
                left: -107px;
                right: 0;
            }
            #prizes p {
                height: 20px;
                width: 75px;
                color: #fff;
                z-index: 1000;
                position: absolute;
                text-align: center;
                font-size: 8px;
            }
            #prize1 {
                top: 182px;
                left: 251px;
            }
            #prize2 {
                top: 204px;
                left: 305px;
            } 
            #prize3 {
                top: 258px;
                left: 326px;
            }
            #prize4 {
                top: 310px;
                left: 304px;
            }
            #prize5 {
                top: 332px;
                left: 250px;
            }
        }
        @media (max-width: 360px) {
            #spinWheelContainer {
                height: 85vh;
                top: -208px;
            }
            #roulette {
                width: 100%;
            }
            #wheelPointer {
                width: 100%;
            }
            #prizes {
                position: absolute;
                top: 95px;
                bottom: 0px;
                left: -116px;
                right: 0;
            }
            #prizes p {
                height: 20px;
                width: 75px;
                color: #fff;
                z-index: 1000;
                position: absolute;
                text-align: center;
                font-size: 8px;
            }
            #prize1 {
                top: 182px;
                left: 251px;
            }
            #prize2 {
                top: 204px;
                left: 305px;
            } 
            #prize3 {
                top: 258px;
                left: 326px;
            }
            #prize4 {
                top: 310px;
                left: 304px;
            }
            #prize5 {
                top: 332px;
                left: 250px;
            }
        }
        .spinWheel {
            margin: 190px auto 0;
            position: relative;
            -ms-transform-origin: 50% 50%;
            -webkit-transform-origin: 50% 50%;
            -moz-transform-origin: 50% 50%;
            -o-transform-origin: 50% 50%;
            transform-origin: 50% 50%;
        }
        #swc {
            z-index: 1;
            position: relative;
            margin: 190px auto 0;
        }
        .rotate45 {
            -ms-transform: rotate(45deg);
            -webkit-transform: rotate(45deg);
            -moz-transform: rotate(45deg);
            -o-transform: rotate(45deg);
            transform: rotate(45deg);
        }
        .rotate90 {
            -ms-transform: rotate(90deg);
            -webkit-transform: rotate(90deg);
            -moz-transform: rotate(90deg);
            -o-transform: rotate(90deg);
            transform: rotate(90deg);
        }
        .rotate135 {
            -ms-transform: rotate(135deg);
            -webkit-transform: rotate(135deg);
            -moz-transform: rotate(135deg);
            -o-transform: rotate(135deg);
            transform: rotate(135deg);
        }
        .rotate180 {
            -ms-transform: rotate(180deg);
            -webkit-transform: rotate(180deg);
            -moz-transform: rotate(180deg);
            -o-transform: rotate(180deg);
            transform: rotate(180deg);
        }
        .rotate225 {
            -ms-transform: rotate(225deg);
            -webkit-transform: rotate(225deg);
            -moz-transform: rotate(225deg);
            -o-transform: rotate(225deg);
            transform: rotate(225deg);
        }
        .rotate270 {
            -ms-transform: rotate(270deg);
            -webkit-transform: rotate(270deg);
            -moz-transform: rotate(270deg);
            -o-transform: rotate(270deg);
            transform: rotate(270deg);
        }
        .rotate315 {
            -ms-transform: rotate(315deg);
            -webkit-transform: rotate(315deg);
            -moz-transform: rotate(315deg);
            -o-transform: rotate(315deg);
            transform: rotate(315deg);
        }
    </style>
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
            ca = [],
            md = JSON.parse('<%=SpinWheelRequest%>'),
            pic = {
                w: 408,
                h: 328,
                a: -103,
                b: -25
            };
            ca[0] = { a: 0, b: 0 };
            ca[1] = { a: 125, b: -313 };
            ca[2] = { a: 0, b: -605 };
            ca[3] = { a: -301, b: -730 };
            ca[4] = { a: -606, b: -615 };
            ca[5] = { a: -730, b: -300 };
            ca[6] = { a: -605, b: 0 };
            ca[7] = { a: -305, b: 125 };
            var sw = new SW(md, translations, ca, null, pic, true, <%=Convert.ToString(HasSession).ToLower()%>);
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