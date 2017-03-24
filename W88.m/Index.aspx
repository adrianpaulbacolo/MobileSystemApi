<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Index.aspx.cs" Inherits="_Index" %>

<% var mobileDeviceId = commonFunctions.getMobileDevice(Request); %>
<!DOCTYPE html>
<html>
<head>
    <title><%=commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML).Replace(" -", "")/* + commonCulture.ElementValues.getResourceString("home", commonVariables.LeftMenuXML)*/%></title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="application/javascript" src="/_Static/JS/add2home.js"></script>
</head>
<body>
            
        <div class="download-app-box">
        <%if (mobileDeviceId == 2)
          {%>
        <div class="download-app">
                    <div class="row">
                        <div class="col col-25 download-icon">
                            <span class="icon icon-android"></span>
                        </div>
                        <div class="col col-75 download-summary">
                            <h5 class="title"><%=commonCulture.ElementValues.getResourceString("w88Android", commonVariables.LeftMenuXML)%></h5>
                            <span><%=commonCulture.ElementValues.getResourceString("w88AndroidDesc", commonVariables.LeftMenuXML)%></span>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col">
                            <a href="javascript:hideDownload();" role="button" class="ui-btn btn-bordered"><%=commonCulture.ElementValues.getResourceString("NoThanks", commonVariables.LeftMenuXML)%></a>
                        </div>
                        <div class="col">
                    <a href="<%=commonClubWAPK.getDownloadUrl %>" target="_blank" class="ui-btn btn-primary"><%=commonCulture.ElementValues.getResourceString("DownloadNow", commonVariables.LeftMenuXML)%></a>
                        </div>
                    </div>
                </div>
            <%}%>
        <%if (mobileDeviceId == 1)
          {%>
                <div class="download-app">
                    <div class="row">
                        <div class="col col-25 download-icon">
                            <span class="icon icon-apple"></span>
                        </div>
                        <div class="col col-75 download-summary">
                            <h5 class="title"><%=commonCulture.ElementValues.getResourceXPathString("Products/iOSSports/Label", commonVariables.ProductsXML)%></h5>
                            <p><%=commonCulture.ElementValues.getResourceXPathString("Products/iOSSports/Description", commonVariables.ProductsXML)%></p>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col">
                            <a href="javascript:hideDownload();" role="button" class="ui-btn btn-bordered"><%=commonCulture.ElementValues.getResourceString("NoThanks", commonVariables.LeftMenuXML)%></a>
                        </div>
                        <div class="col">
                            <a href="/_Static/Downloads/w88.aspx" class="ui-btn btn-primary"><%=commonCulture.ElementValues.getResourceString("DownloadNow", commonVariables.LeftMenuXML)%></a>
                        </div>
                    </div>
                </div>
            <%}%>
            </div>

    <div id="divMain" class="download-app-visible" data-role="page" data-theme="b" data-ajax="false">
        <!--#include virtual="~/_static/header.shtml" -->
        <div class="ui-content" role="main">

            <section class="section banner-slider">
                <%=getPromoBanner() %>
                <!--<div class="slide">
                    <a rel="clbW" href="/_static/ClubW/casino.aspx" data-ajax="false">
                        <img src="/_Static/Images/Download/W88-Mobile-ClubW-Casino.jpg" alt="banner" class="img-responsive">
                    </a>
                </div>-->
            </section>
            <% if (string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId))
               {
            var queryString = commonVariables.GetSessionVariable("AffiliateId") == string.Empty ? "" : "?affiliateId=" + commonVariables.GetSessionVariable("AffiliateId"); %>
            <div class="row row-no-padding action-btn">
                <div class="col front-btn">
                    <a href="/_Secure/Register.aspx<%= queryString %>" class="ui-btn btn-secondary" role="button" data-ajax="false">
                        <%=commonCulture.ElementValues.getResourceString("joinnow", commonVariables.LeftMenuXML)%>
                    </a>
                </div>
                <div class="col front-btn">
                    <a href="/_Secure/Login.aspx" class="ui-btn btn-primary" role="button" data-rel="dialog" data-ajax="false">
                        <%=commonCulture.ElementValues.getResourceString("login", commonVariables.LeftMenuXML)%>
                    </a>
                </div>
            </div>
             <% } %>
            <% if (!string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId))
               { %>
             <div class="row row-no-padding action-btn action-button-lg" data-ajax="false">
                <div class="col">
                    <a href="Funds.aspx" class="ui-btn btn-primary" role="button" data-ajax="false">
                        <span class="icon icon-currency-circle" ></span>
                        <%=commonCulture.ElementValues.getResourceString("fundmanagement", commonVariables.LeftMenuXML)%>
                    </a>
                </div>
            </div>
             <% } %>


            <ul class="row row-bordered bg-gradient row-uc row-dashboard">

                <li class="col col-33 product">
                    <a href="Sports.aspx?lang=<%=commonVariables.SelectedLanguage.ToLower() %>" class="tile" data-ajax="false">
                        <span class="icon icon-soccer"></span>
                        <h4 class="title"><%=commonCulture.ElementValues.getResourceString("sports", commonVariables.LeftMenuXML)%></h4>
                    </a>
                </li>
                <li class="col col-33 product">
                    <a href="Casino.aspx?lang=<%=commonVariables.SelectedLanguage.ToLower() %>" class="tile" data-ajax="false">
                        <span class="icon icon-casino"></span>
                        <h4 class="title"><%=commonCulture.ElementValues.getResourceString("livecasino", commonVariables.LeftMenuXML)%></h4>
                    </a>
                </li>
                <li class="col col-33 product">
                    <%--<a href="#divPanel" class="tile nav-slots">--%>
                    <a href="Slots.aspx?lang=<%=commonVariables.SelectedLanguage.ToLower()%>" class="tile" data-ajax="false">
                        <span class="icon icon-slots"></span>
                        <h4 class="title"><%=commonCulture.ElementValues.getResourceString("slots", commonVariables.LeftMenuXML)%></h4>
                    </a>
                </li>
                
                <!-- Fishing Game -->
                <% if (!string.IsNullOrWhiteSpace(commonCookie.CookieCurrency))
                   {
                %>
                            <li class="col col-33 product">
                                <a href="#divPanel" class="tile nav-fish">
                                    <span class="icon icon-fish"></span>
                                    <h4 class="title"><%=commonCulture.ElementValues.getResourceString("FishingTitle", commonVariables.LeftMenuXML)%></h4>
                                </a>
                            </li>
                    <% }
                   else 
                   { %>
                        <li class="col col-33 product">
                            <a href="#divPanel" class="tile nav-fish">
                                <span class="icon icon-fish"></span>
                                <h4 class="title"><%=commonCulture.ElementValues.getResourceString("FishingTitle", commonVariables.LeftMenuXML)%></h4>
                            </a>
                        </li>
                <% } %>
                
                <li class="col col-33 product">                    
                    <a href="Lottery.aspx?lang=<%=commonVariables.SelectedLanguage%>" class="tile" data-ajax="false">
                        <span class="icon icon-keno"></span>
                        <h4 class="title"><%=commonCulture.ElementValues.getResourceString("lottery", commonVariables.LeftMenuXML)%></h4>
                    </a>
                </li>
                <li class="col col-33 product">
                    <a href="#divPanel" class="tile nav-poker">
                        <span class="icon icon-spade"></span>
                        <h4 class="title"><%=commonCulture.ElementValues.getResourceString("poker", commonVariables.LeftMenuXML)%></h4>
                    </a>
                </li>

                <li class="col col-33 product">
                    <a href="#divPanel" class="tile nav-pmahjong">
                        <span class="icon icon-mahjong"></span>
                        <h4 class="title"><%=commonCulture.ElementValues.getResourceString("texasmahjong", commonVariables.LeftMenuXML)%></h4>
                    </a>
                </li>

                <!-- Profile -->
                <% if (!string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId))
                   { %>
                <li class="col col-33">
                    <a href="/Profile/Default.aspx" class="tile" data-ajax="false">
                        <span class="icon icon-profile"> </span>
                        <h4 class="title">
                            <%--<%=commonVariables.GetSessionVariable("MemberCode")%>--%>
                             <%=commonCulture.ElementValues.getResourceString("profile", commonVariables.LeftMenuXML)%>
                        </h4>
                    </a>
                </li>
                <% }
                   else
                   {
                   var queryString = commonVariables.GetSessionVariable("AffiliateId") == string.Empty ? "" : "?affiliateId=" + commonVariables.GetSessionVariable("AffiliateId"); %>
                <% } %>

                <li class="col col-33">
                    <a href="/Promotions" data-ajax="false" class="tile">
                        <span class="icon icon-promo"></span>
                        <h4 class="title"><%=commonCulture.ElementValues.getResourceString("promotions", commonVariables.LeftMenuXML)%></h4>
                    </a>
                </li>

                 <li class="col col-33">
                    <a href="#" id="icon-rewards" class="tile" role="button" data-ajax="false" target="_blank">
                        <span class="icon icon-rewards"></span>
                        <h4 class="title"><%=commonCulture.ElementValues.getResourceString("rewards", commonVariables.LeftMenuXML)%></h4>
                    </a>
                </li>

                <%--

                <li class="col col-33">
                    <a href="#download" runat="server" class="tile notify" data-rel="popup" data-position-to="window" data-transition="fade">
                        <span class="icon-bell"></span>
                        <span class="badge">13</span>
                        <h4 class="title">Announcements</h4>
                    </a>
                </li>--%>

                <%if (mobileDeviceId == 2)
                  {%>
                <li class="col col-33">
                    <a href="/_Static/ClubW/casino.aspx" id="downloadButton" runat="server" class="tile notify" data-ajax="false">
                        <span class="icon icon-download"></span>
                        <h4 class="title"><%=commonCulture.ElementValues.getResourceString("download", commonVariables.LeftMenuXML)%></h4>
                    </a>
                </li>
                <%}%>

                <li class="col col-33">
                    <a href="/LiveChat/Default.aspx" class="tile" role="button" runat="server" data-ajax="false" target="_blank">
                        <span class="icon icon-chat"></span>
                        <h4 class="title"><%=commonCulture.ElementValues.getResourceString("liveHelp", commonVariables.LeftMenuXML)%></h4>
                    </a>
                </li>

                 <!-- Zalo -->
                <% if (commonVariables.SelectedLanguage.Equals("vi-vn", StringComparison.OrdinalIgnoreCase))
                   { %>

                    <%if (mobileDeviceId == 1)
                      {%>
                    <li class="col col-33">
                    <a href="zalo://639989602209" class="tile" data-ajax="false">
                        <span class="icon icon-zalo"></span>
                        <h4 class="title">(+63) 9989602209</h4>
                    </a>
                </li>
                    <%}
                      else if (mobileDeviceId == 2)
                      {%>
                    <li class="col col-33">
                    <a href="http://zaloapp.com/qr/p/tkz0l05n8qu5" class="tile" data-ajax="false">
                        <span class="icon icon-zalo"></span>
                        <h4 class="title">Zalo</h4>
                    </a>
                </li>
                    <% } %>
                <% } %>

                <li class="col col-33">
                    <a href="/Lang.aspx" class="tile" role="button" data-ajax="false">
                        <span class="icon icon-world"></span>
                        <h4 class="title"><%=commonCulture.ElementValues.getResourceString("language", commonVariables.LeftMenuXML)%></h4>
                    </a>
                </li>

                <li class="col col-33">
                    <a href="#" id="icon-desktop" class="tile" role="button" data-ajax="false">
                        <span class="icon icon-desktop"></span>
                        <h4 class="title"><%=commonCulture.ElementValues.getResourceString("desktopIcon", commonVariables.LeftMenuXML)%></h4>
                    </a>
                </li>

            </ul>
            
        <!--#include virtual="~/_static/footer.shtml" -->
        </div>

        <!--#include virtual="~/_static/navMenu.shtml" -->

        <script src="/_Static/JS/vendor/slick.min.js"></script>
        <script>
            // Slick - Slider Banner
            $(document).ready(function () {
                $('.banner-slider').slick({
                    dots: true,
                    responsive: [
                    {
                        breakpoint: 768,
                        settings: {
                            arrows: false
                        }
                    }
                    ]
                });

                var url = window.location.protocol + '//www.' + '<%=commonIp.DomainName %>' + '?nomobile=true';
                 
                <% if (!string.IsNullOrWhiteSpace(commonCookie.CookieAffiliateId))
                    {%>
                        url += '&affiliateID=' + <%=commonCookie.CookieAffiliateId%>;
                <%  } %>

                $("#icon-desktop").attr('href', url);
                $("#divPanel.ui-panel .sub-menu").css("top", $(".download-app").height());

                var urlRewards = window.location.protocol + '//mrewards.' + '<%=commonIp.DomainName %>' + '?lang=' + '<%=commonVariables.SelectedLanguage%>' + '&token=' + '<%=commonVariables.EncryptedCurrentMemberSessionId%>';
                $("#icon-rewards").attr('href', urlRewards);
            });

            function hideDownload() {
                $(".download-app").addClass("hide");
                $("div#divMain").removeClass("download-app-visible");
                $("#divPanel.ui-panel").css("top", 0);
                $("#divPanel.ui-panel .sub-menu").css("top", 0);
            }

            function adjustMenu() {
                $("#divPanel.ui-panel").css("top", $(".download-app").height());
            }
        </script>

    </div>
</body>
</html>
