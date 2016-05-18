<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Index.aspx.cs" Inherits="_Index" %>

<!DOCTYPE html>
<html>
<head>
    <title><%=commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML).Replace(" -", "")/* + commonCulture.ElementValues.getResourceString("home", commonVariables.LeftMenuXML)*/%></title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/Js/Main.js"></script>
    <script type="application/javascript" src="/_Static/Js/add2home.js"></script>
    <script type="text/javascript" src="/_Static/Js/radar.js"></script>
</head>
<body>
    <!--#include virtual="~/_static/splash.shtml" -->
    <div id="divMain" data-role="page" data-theme="b" data-ajax="false">
        <!--#include virtual="~/_static/header.shtml" -->
        <div class="ui-content" role="main">

            <section class="section banner-slider">
                <div class="slide">
                    <a href="#divPanel" class="nav-pmahjong">
                        <img src="/_Static/Images/Download/W88-Mobile-TexasMahjong.jpg" alt="banner" class="img-responsive">
                    </a>
                </div>
                <div class="slide">
                    <a rel="clbW" href="/_static/ClubW/casino.aspx" data-ajax="false">
                        <img src="/_Static/Images/Download/W88-Mobile-ClubW-Casino.jpg" alt="banner" class="img-responsive">
                    </a>
                </div>
                <div class="slide">
                    <a rel="clubpalazzo" href="/_static/palazzo/casino.aspx" data-ajax="false">
                        <img src="/_Static/Images/Download/W88-Mobile-ClubPalazzo-Casino.jpg" alt="banner" class="img-responsive">
                    </a>
                </div>
                <div class="slide">
                    <a href="https://livegames.gameassists.co.uk/MobileClient/MobileRedirector/index.aspx?AppID=W88Diamond&ClientID=5&UL=en" data-ajax="false" target="_blank">
                        <img src="/_Static/Images/Download/W88-Mobile-ClubPalazzo-Slots.jpg" alt="banner" class="img-responsive">
                    </a>
                </div>
                <%if (DetectMobileDevice() == 1) {%>
                <div class="slide">
                    <a class="slide" href="/_Static/Downloads/w88.aspx">
                        <img src="/_Static/Images/Download/Bnr-ClubW88-iOS.jpg" alt="banner" class="img-responsive">
                    </a>
                </div>
                <%}%>
            </section>

        <% if (string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId)) {
            var queryString = commonVariables.GetSessionVariable("AffiliateId") == string.Empty ? "" : "?affiliateId=" + commonVariables.GetSessionVariable("AffiliateId"); %>
            <div class="row row-no-padding action-btn">
                <div class="col">
                    <a href="/_Secure/Register.aspx<%= queryString %>" class="ui-btn btn-secondary" role="button" data-ajax="false">
                        <%=commonCulture.ElementValues.getResourceString("joinnow", commonVariables.LeftMenuXML)%>
                    </a>
                </div>
                <div class="col">
                    <a href="/_Secure/Login.aspx" class="ui-btn btn-primary" role="button" data-rel="dialog" data-transition="slidedown">
                        <%=commonCulture.ElementValues.getResourceString("login", commonVariables.LeftMenuXML)%>
                    </a>
                </div>
            </div>
        <% } %>


            <ul class="row row-bordered bg-gradient">

                <li class="col col-33 product">
                    <a href="Sports.aspx?lang=<%=commonVariables.SelectedLanguage.ToLower() %>" class="tile" data-ajax="false" data-transition="slidedown">
                        <span class="icon-sports"></span>
                        <h4 class="title"><%=commonCulture.ElementValues.getResourceString("sports", commonVariables.LeftMenuXML)%></h4>
                    </a>
                </li>
                <li class="col col-33 product">
                    <a href="Casino.aspx?lang=<%=commonVariables.SelectedLanguage.ToLower() %>" class="tile" data-ajax="false" data-transition="slidedown">
                        <span class="icon-casino"></span>
                        <h4 class="title"><%=commonCulture.ElementValues.getResourceString("livecasino", commonVariables.LeftMenuXML)%></h4>
                    </a>
                </li>
                <li class="col col-33 product">
                    <a href="#divPanel" class="tile nav-pmahjong">
                        <span class="icon-mahjong"></span>
                        <h4 class="title"><%=commonCulture.ElementValues.getResourceString("texasmahjong", commonVariables.LeftMenuXML)%></h4>
                    </a>
                </li>
                <li class="col col-33 product">
                    <%--<a href="#divPanel" class="tile nav-slots">--%>
                    <a href="Slots.aspx?lang=<%=commonVariables.SelectedLanguage.ToLower()%>" class="tile" data-ajax="false" data-transition="slidedown">
                        <span class="icon-slots"></span>
                        <h4 class="title"><%=commonCulture.ElementValues.getResourceString("slots", commonVariables.LeftMenuXML)%></h4>
                    </a>
                </li>
                <li class="col col-33 product">                    
                    <a href="Lottery.aspx?lang=<%=commonVariables.SelectedLanguage%>" class="tile" data-ajax="false" data-transition="slidedown">
                        <span class="icon-keno"></span>
                        <h4 class="title"><%=commonCulture.ElementValues.getResourceString("lottery", commonVariables.LeftMenuXML)%></h4>
                    </a>
                </li>
                <li class="col col-33 product">
                    <a href="#divPanel" class="tile nav-poker">
                        <span class="icon-spade"></span>
                        <h4 class="title"><%=commonCulture.ElementValues.getResourceString("poker", commonVariables.LeftMenuXML)%></h4>
                    </a>
                </li>
                <!-- Profile -->
                <% if (!string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId)) { %>
                <li class="col col-33">
                    <a href="/Profile" class="tile" data-ajax="false" data-transition="slidedown">
                        <span class="icon-profile"> </span>
                        <h4 class="title">
                            <%--<%=commonVariables.GetSessionVariable("MemberCode")%>--%>
                             <%=commonCulture.ElementValues.getResourceString("profile", commonVariables.LeftMenuXML)%>
                        </h4>
                    </a>
                </li>
                <% } else {
                   var queryString = commonVariables.GetSessionVariable("AffiliateId") == string.Empty ? "" : "?affiliateId=" + commonVariables.GetSessionVariable("AffiliateId"); %>
                <% } %>

                <!-- Funds -->
                <% if (!string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId)) { %>
                <li class="col col-33">
                    <a href="Funds.aspx" class="tile" data-ajax="false" data-transition="slidedown">
                        <span class="icon- ion-social-usd-outline"></span>
                        <h4 class="title"><%=commonCulture.ElementValues.getResourceString("fundmanagement", commonVariables.LeftMenuXML)%></h4>
                    </a>
                </li>
                <% } %>

                <li class="col col-33">
                    <a href="/Promotions" data-ajax="false" class="tile">
                        <span class="icon-promo"></span>
                        <h4 class="title"><%=commonCulture.ElementValues.getResourceString("promotions", commonVariables.LeftMenuXML)%></h4>
                    </a>
                </li>
                <%--<li class="col col-33">
                    <a href="" class="tile">
                        <span class="icon-rewards"></span>
                        <h4 class="title"><%=commonCulture.ElementValues.getResourceString("rewards", commonVariables.LeftMenuXML)%></h4>
                    </a>
                </li>

                <li class="col col-33">
                    <a href="#download" runat="server" class="tile notify" data-rel="popup" data-position-to="window" data-transition="fade">
                        <span class="icon-bell"></span>
                        <span class="badge">13</span>
                        <h4 class="title">Announcements</h4>
                    </a>
                </li>--%>

                <%if(DetectMobileDevice() == 2){%>
                <li class="col col-33">
                    <a href="/_Static/ClubW/casino.aspx" id="downloadButton" runat="server" class="tile notify" data-ajax="false">
                        <span class="icon- ion-ios-download-outline"></span>
                        <h4 class="title"><%=commonCulture.ElementValues.getResourceString("download", commonVariables.LeftMenuXML)%></h4>
                    </a>
                </li>
                <%}%>

                <li class="col col-33">
                    <a href="/Lang.aspx" class="tile" role="button" data-transition="slideup">
                        <span class="icon- ion-ios-world-outline"></span>
                        <h4 class="title"><%=commonCulture.ElementValues.getResourceString("language", commonVariables.LeftMenuXML)%></h4>
                    </a>
                </li>
                <li class="col col-33">
                    <a href="/LiveChat/Default.aspx" class="tile" role="button" runat="server" data-ajax="false" target="_blank">
                        <span class="icon-chat"></span>
                        <h4 class="title"><%=commonCulture.ElementValues.getResourceString("liveHelp", commonVariables.LeftMenuXML)%></h4>
                    </a>
                </li>

                <!-- Zalo -->
                <% if (commonVariables.SelectedLanguage.Equals("vi-vn", StringComparison.OrdinalIgnoreCase))
                   { %>
                <li class="col col-33">

                    <%if (DetectMobileDevice() == 1)
                      {%>
                    <a href="zalo://639989602209" class="tile" data-ajax="false" data-transition="slidedown">
                        <span class="icon-zalo"></span>
                        <h4 class="title">(+63) 9989602209</h4>
                    </a>
                    <%}
                      else if (DetectMobileDevice() == 2)
                      {%>
                    <a href="http://zaloapp.com/qr/p/tkz0l05n8qu5" class="tile" data-ajax="false" data-transition="slidedown">
                        <span class="icon-zalo"></span>
                        <h4 class="title">Zalo</h4>
                    </a>
                    <% }%>
                </li>
                <% } %>

            </ul>

            <!-- <ul class="hide">
                <li class="li-pokerIOS" runat="server" id ="pokerIOS_link">
                    <a rel="PokerIOS" href="#" data-ajax="false" target="_blank" runat="server" id ="pokerIOS">
                        <div><%=commonCulture.ElementValues.getResourceXPathString("Products/Poker/Label", commonVariables.ProductsXML)%></div>
                    </a>
                </li>
                <li class="li-pokerAndroid" runat="server" id ="pokerAndroid_link" >
                    <a rel="PokerAndroid" href="#" data-ajax="false" target="_blank" runat="server" id ="pokerAndroid">
                        <div><%=commonCulture.ElementValues.getResourceXPathString("Products/Poker/Label", commonVariables.ProductsXML)%></div>
                    </a>
                </li>
            </ul> -->

           <%if(DetectMobileDevice() == 2){%>
                <div class="download-app">
                    <div class="row">
                        <div class="col col-25 download-icon">
                            <span class="ion-social-android"></span>
                        </div>
                        <div class="col col-75 download-summary">
                            <h5 class="title">W88 Android Download</h5>
                            <p>Sports, Live Casino, Slots in 1 App</p>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col">
                            <a href="javascript:hideDownload();" role="button" class="ui-btn btn-bordered">No Thanks</a>
                        </div>
                        <div class="col">
                            <a href="<%=commonClubWAPK.getDownloadUrl %>" class="ui-btn btn-primary">Download Now</a>
                        </div>
                    </div>
                </div>
            <%}%>

            <%if(DetectMobileDevice() == 1){%>
                <div class="download-app">
                    <div class="row">
                        <div class="col col-25 download-icon">
                            <span class="ion-social-apple"></span>
                        </div>
                        <div class="col col-75 download-summary">
                            <h5 class="title">W88 iOS Download</h5>
                            <p>Sports, Live Casino, Slots in 1 App</p>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col">
                            <a href="javascript:hideDownload();" role="button" class="ui-btn btn-bordered">No Thanks</a>
                        </div>
                        <div class="col">
                            <a href="/_Static/Downloads/w88.aspx" class="ui-btn btn-primary">Download Now</a>
                        </div>
                    </div>
                </div>
            <%}%>
        </div>

        <!--#include virtual="~/_static/navMenu.shtml" -->

        <script src="/_Static/Js/vendor/slick.min.js"></script>
        <script>
            // Slick - Slider Banner
            $(document).ready(function(){
                $('.banner-slider').slick({
                    dots: true
                });
            });

            function hideDownload() {
                $(".download-app").addClass("hide");
            }
        </script>

    </div>
</body>
</html>
