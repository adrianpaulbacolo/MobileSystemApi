<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Index.aspx.cs" Inherits="_Index" %>

<!DOCTYPE html>
<html>
<head>
    <title><%=commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML).Replace(" -", "")/* + commonCulture.ElementValues.getResourceString("home", commonVariables.LeftMenuXML)*/%></title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/Js/Main.js"></script>
    <link rel="stylesheet" href="/_Static/Css/add2home.css">
    <script type="application/javascript" src="/_Static/Js/add2home.js"></script>
    <!--[if IE]><link type="text/css" href="/_Static/Css/Index.css" rel="stylesheet"><![endif]-->
    <!--[if !IE]><!-->
    <link type="text/css" href="/_Static/Css/IndexScroll.css" rel="stylesheet">
    <!--<![endif]-->
    <script type="text/javascript">
        $(function () { getPromos(); window.document.referrer.indexOf('aSports'); var prodScroll = new IScroll('.div-product-scroll', { eventPassthrough: true, scrollX: true, scrollY: false, preventDefault: false, speedRatioX: 9000 }); });
        $(window).resize(function () { $('.div-promo-row > a > div:last-child > div').css({ maxWidth: ($(window).width() - 200) + 'px' }); });
        function timerV2(pid, start_date, end_date) { if (new Date('<%=System.DateTime.Now.ToString(commonVariables.DateTimeFormat)%>') < new Date(start_date) || new Date('<%=System.DateTime.Now.ToString(commonVariables.DateTimeFormat)%>') > new Date(end_date)) { $('div#' + pid).hide(); } }
        function getPromos() {
            $.get('/_Static/Promotions/promotions.<%=commonVariables.SelectedLanguage + (string.Compare(commonVariables.GetSessionVariable("CountryCode"), "my", true) == 0 ? ".my" : "")%>.htm', function (html) { })
              .done(function (data) {
                  data = data.replace(/<img src=/g, '<img rel=');
                  var listObj = $("#divPromotions").append('<ul></ul>').find('ul');
                  $(data).find('.mobile_show').each(function (index) {
                      $(this).find('script').each(function () { $.globalEval(this.text || this.textContent || this.innerHTML || ''); });
                      var strPromoTitle = $(this).find('div.promotion_title').text();
                      var strPromoContent = $(this).find('div.promotion_content').text();
                      var strPromoDetail = $(this).find('div.promotion_detail').html().substr(0, 4) == '<br>' ? $(this).find('div.promotion_detail').html().substring(4) : $(this).find('div.promotion_detail').contents();
                      var objImage = $(this).find('img')[0];
                      var strImageSrc = null;
                      if (objImage != null) { if (/\/promotions\/img\/W88-Promotion(s)*-/i.test($(objImage).attr('rel'))) { strImageSrc = $(objImage).attr('rel').replace(/\/promotions\/img\/W88-Promotion(s)*-/i, '/promotions/mobile/images/w88-mobile-'); } }
                      var liPromo = $('<li />');
                      var divPromoWrapper = $('<div />', { id: $(this).attr('id'), class: index % 2 == 0 ? 'div-promo-row' : 'div-promo-row div-promo-row-alt' });
                      var divPromoImg = $('<div />', { class: 'div-promo-img' });
                      var imgPromo = $('<img />', { src: strImageSrc });
                      var hrefPromo = $('<a />', { href: "/Promotions#" + $(this).attr('id'), 'data-ajax': false });
                      var divPromoTitle = $('<div />', { class: 'div-promo-header' }).text(strPromoTitle);
                      var divPromoContent = $('<div />', { class: 'div-promo-desc' }).text(strPromoContent);                      
                      var divPromoDetail = $('<div />', { class: 'div-promo-content' }).html(/<img rel=/g.test(strPromoDetail) ? strPromoDetail.replace(/<img rel=/g, '<img src=') : strPromoDetail);
                      listObj.append($(liPromo).append($(divPromoWrapper).append($(hrefPromo).append($(divPromoImg).append(imgPromo)).append($('<div />', {}).append(divPromoTitle).append(divPromoContent)))));
                      $('.div-promo-row > a > div:last-child > div').css({ maxWidth: ($(window).width() - 160) + 'px' });
                      $(this).find('script').each(function () { $.globalEval(this.text || this.textContent || this.innerHTML || ''); });
                  });
                  if (location.hash != '') {
                      $(location.hash).next().slideToggle();
                      var divObj = $(location.hash).find('div')[1];
                      if ($(divObj).css('background-image').indexOf('arrow-up') > 0) { $(divObj).css('background-image', "url('/_Static/Images/arrow-down.png')"); }
                      else { $(divObj).css('background-image', "url('/_Static/Images/arrow-up.png')"); }
                  }
              })
              .always(function (data) { $('#promoLoader').hide(); });
          }

        function OpenPromoDetails(obj) { $(obj).parent().next().slideToggle(); }
        function PromoClaimNow(obj) { $(obj).hide(); $(obj).next().show(); }
        function PromoCancelClaim(obj) { $(obj).parent().hide(); $(obj).parent().prev().show(); }
        function PromoClaim(obj) {
            var strCode = $(obj).parent().children().first().text();
            var strComment = null;
            if ($(obj).parent().children().find('textarea').length == 0) { strComment = $(obj).parent().children().find('input[type="radio"]:checked').val(); }
            else { strComment = $(obj).parent().children().find('textarea').val(); }

            $.ajax({
                type: 'POST',
                url: '/AjaxHandlers/RegisterPromo.ashx',
                data: { sCode: strCode, Comment: strComment },
                beforeSend: function () { PromoCancelClaim(); },
                success: function (data) {
                    switch (parseInt(data)) {
                        case 1: // success
                            alert('<%=commonCulture.ElementValues.getResourceXPathString("/Promotion/RegisterSuccess", xeErrors)%>');
                            $(obj).parent().hide();
                            //$(obj).parent().prev().show();
                            break;
                        case 10: // multiple submit
                            alert('<%=commonCulture.ElementValues.getResourceXPathString("/Promotion/SubmitOnce", xeErrors)%>');
                            $(obj).parent().hide();
                            break;

                        default: // error
                            alert('<%=commonCulture.ElementValues.getResourceString("ServerError", xeErrors)%>');
                            $(obj).parent().hide();
                            break;
                    }
                },
                error: function (data) { },
                complete: function (data) { PromoCancelClaim(); }
            });
        }
    </script>
</head>
<body>
    <!--#include virtual="~/_static/splash.shtml" -->
    <div id="divMain" data-role="page" data-theme="b" data-ajax="false">
        <!--#include virtual="~/_static/header.shtml" -->
        <div class="ui-content" role="main">
            <div id="divLoginMessage" runat="server"><span id="lblLogin" runat="server">please login to place a bet</span></div>
            <div id="divContent">
                <div class="div-product-scroll">
                    <div>
                        <ul>                  
                            <li class="li-asports">
                                <a rel="asports" href="/_Secure/Login.aspx" data-rel="dialog" data-transition="slidedown">
                                    <div><%=commonCulture.ElementValues.getResourceXPathString("Products/ASports/Label", commonVariables.ProductsXML)%></div>
                                    <div></div>
                                </a>
                            </li>
                            <li class="li-clubw">
                                <a rel="clubw" href="//casino.w88live.com/mob/app-release.apk" data-ajax="false">
                                    <div><%=commonCulture.ElementValues.getResourceXPathString("Products/ClubW/Label", commonVariables.ProductsXML)%></div>
                                    <div></div>
                                </a>
                            </li>
                            <li class="li-clubmassimo-casino">
                                <a rel="clubmassimo-casino" href="//livegames.gameassists.co.uk/MobileClient/MobileRedirector/index.aspx?AppID=W88Diamond&ClientID=5&UL=en" data-ajax="false">
                                    <div><%=commonCulture.ElementValues.getResourceXPathString("Products/ClubMassimoCasino/Label", commonVariables.ProductsXML)%></div>
                                    <div></div>
                                </a>
                            </li>
                            <li class="li-lottery">
                                <a rel="lottery" href="/_Secure/Login.aspx" data-rel="dialog" data-transition="slidedown">
                                    <div><%=commonCulture.ElementValues.getResourceXPathString("Products/Lottery/Label", commonVariables.ProductsXML)%></div>
                                    <div></div>
                                </a>
                            </li>
                            <li class="li-clubmassimo-slots">
                                <a rel="clubmassimo-slots" href="/ClubMassimo" data-ajax="false">
                                    <div><%=commonCulture.ElementValues.getResourceXPathString("Products/ClubMassimoSlots/Label", commonVariables.ProductsXML)%></div>
                                    <div></div>
                                </a>
                            </li>
                            <li class="li-clubdivino">
                                <a rel="clubdivino" href="/ClubDivino" data-ajax="false">
                                    <div><%=commonCulture.ElementValues.getResourceXPathString("Products/ClubDivino/Label", commonVariables.ProductsXML)%></div>
                                    <div></div>
                                </a>
                            </li>
                            <li class="li-clubcrescendo">
                                <a rel="clubcrescendo" href="/ClubCrescendo" data-ajax="false">
                                    <div><%=commonCulture.ElementValues.getResourceXPathString("Products/ClubCrescendo/Label", commonVariables.ProductsXML)%></div>
                                    <div></div>
                                </a>
                            </li>
                            <li class="li-esports">
                                <a rel="esports" href="/_Secure/Login.aspx" data-rel="dialog" data-transition="slidedown">
                                    <div><%=commonCulture.ElementValues.getResourceXPathString("Products/ESports/Label", commonVariables.ProductsXML)%></div>
                                    <div></div>
                                </a>
                            </li>
                            <li class="li-clubbravado">
                                <a rel="clubbravado" href="/ClubBravado" data-ajax="false">
                                    <div><%=commonCulture.ElementValues.getResourceXPathString("Products/ClubBravado/Label", commonVariables.ProductsXML)%></div>
                                    <div></div>
                                </a>
                            </li>

                            <% if (!string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId))
                               { %>

                            <li class="li-deposit">
                                <a rel="deposit" href="/_Secure/Login.aspx" data-rel="dialog" data-transition="slidedown">
                                    <div><%=commonCulture.ElementValues.getResourceString("deposit", commonVariables.LeftMenuXML)%></div>
                                    <div></div>
                                </a>
                            </li>
                            <li class="li-fundtransfer">
                                <a rel="fundtransfer" href="/_Secure/Login.aspx" data-rel="dialog" data-transition="slidedown">
                                    <div><%=commonCulture.ElementValues.getResourceString("transfer", commonVariables.LeftMenuXML)%></div>
                                    <div></div>
                                </a>
                            </li>
                            <li class="li-withdrawal">
                                <a rel="withdrawal" href="/_Secure/Login.aspx" data-rel="dialog" data-transition="slidedown">
                                    <div><%=commonCulture.ElementValues.getResourceString("withdrawal", commonVariables.LeftMenuXML)%></div>
                                    <div></div>
                                </a>
                            </li>
                            <% } %>
                        </ul>
                    </div>
                </div>

                <div id="divProduct">
                    <ul class="lv-Product" data-role="listview" data-icon="false">
                        <!--
                        <li class="li-wcinfo">
                            <a rel="awcinfo" href="//ls.betradar.com/ls/livescore/?/w88/{LANG}/page/worldcup_matchcentermobile" data-ajax="false">
                                <img src="/_Static/Images/bnr-wcinfo.jpg" class="ui-li-thumb" />
                            </a>
                        </li>
                        -->
                        <li class="li-asports">
                            <a id="aASports" runat="server" href="/_Secure/Login.aspx" data-rel="dialog" data-transition="slidedown">
                                <img src="/_Static/Images/bnr-asports.jpg" class="ui-li-thumb" />
                                <h2><%=commonCulture.ElementValues.getResourceXPathString("Products/ASports/Label", commonVariables.ProductsXML)%></h2>
                            </a>
                        </li>
                        <li class="li-clubw">
                            <a id="aClubW" runat="server" rel="clubw" href="//casino.w88live.com/mob/app-release.apk" data-ajax="false">
                                <img src="/_Static/Images/bnr-clubw-android.jpg" class="ui-li-thumb" />
                                <h2><%=commonCulture.ElementValues.getResourceXPathString("Products/ClubW/Label", commonVariables.ProductsXML)%></h2>
                            </a>
                        </li>
                        <li class="li-clubmassimo-casino">
                            <a id="aClubMassimoCasino" runat="server" href="//livegames.gameassists.co.uk/MobileClient/MobileRedirector/index.aspx?AppID=W88Diamond&ClientID=5&UL=en" data-ajax="false">
                                <img src="/_Static/Images/bnr-clubmassimo-casino-android.jpg" class="ui-li-thumb" />
                                <h2><%=commonCulture.ElementValues.getResourceXPathString("Products/ClubMassimoCasino/Label", commonVariables.ProductsXML)%></h2>
                            </a>
                        </li>
                        <li class="li-lottery">
                            <a id="aLottery" runat="server" href="/_Secure/Login.aspx" data-rel="dialog" data-transition="slidedown">
                                <img src="/_Static/Images/bnr-lottery.png" class="ui-li-thumb" />
                                <h2><%=commonCulture.ElementValues.getResourceXPathString("Products/Lottery/Label", commonVariables.ProductsXML)%></h2>
                            </a>
                        </li>
                        <li class="li-clubmassimo-slots">
                            <a id="aClubMassimo" runat="server" href="/ClubMassimo" data-ajax="false">
                                <img src="/_Static/Images/bnr-clubmassimo-slots.jpg" class="ui-li-thumb" />
                                <h2><%=commonCulture.ElementValues.getResourceXPathString("Products/ClubMassimoSlots/Label", commonVariables.ProductsXML)%></h2>
                            </a>
                        </li>
                        <li class="li-clubdivino">
                            <a id="aClubDivino" runat="server" href="/ClubDivino" data-ajax="false">
                                <img src="/_Static/Images/bnr-clubdivino.jpg" class="ui-li-thumb" />
                                <h2><%=commonCulture.ElementValues.getResourceXPathString("Products/ClubDivino/Label", commonVariables.ProductsXML)%></h2>
                            </a>
                        </li>
                        <li class="li-clubcrescendo">
                            <a id="aClubCrescendo" runat="server" href="/ClubCrescendo" data-ajax="false">
                                <img src="/_Static/Images/bnr-clubcrescendo.png" class="ui-li-thumb" />
                                <h2><%=commonCulture.ElementValues.getResourceXPathString("Products/ClubCrescendo/Label", commonVariables.ProductsXML)%></h2>
                            </a>
                        </li>
                        <li class="li-esports">
                            <a id="aESports" runat="server" href="/_Secure/Login.aspx" data-rel="dialog" data-transition="slidedown">
                                <img src="/_Static/Images/bnr-esports.jpg" class="ui-li-thumb" />
                                <h2><%=commonCulture.ElementValues.getResourceXPathString("Products/ESports/Label", commonVariables.ProductsXML)%></h2>
                            </a>
                        </li>
                        <li class="li-clubbravado">
                            <a id="aClubBravado" runat="server" href="/ClubBravado" data-ajax="false">
                                <img src="/_Static/Images/bnr-clubbravado.jpg" class="ui-li-thumb" />
                                <h2><%=commonCulture.ElementValues.getResourceXPathString("Products/ClubBravado/Label", commonVariables.ProductsXML)%></h2>
                            </a>
                        </li>

                        <% if (!string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId))
                           { %>

                        <li class="li-deposit">
                            <a href="/_Secure/Login.aspx" data-rel="dialog" data-transition="slidedown">
                                <img src="/_Static/Images/index-deposit.png" class="ui-li-thumb" />
                                <h2><%=commonCulture.ElementValues.getResourceString("deposit", commonVariables.LeftMenuXML)%></h2>
                            </a>
                        </li>

                        <li class="li-fundtransfer">
                            <a href="/_Secure/Login.aspx" data-rel="dialog" data-transition="slidedown">
                                <img src="/_Static/Images/index-fundtransfer.png" class="ui-li-thumb" />
                                <h2><%=commonCulture.ElementValues.getResourceString("transfer", commonVariables.LeftMenuXML)%></h2>
                            </a>
                        </li>

                        <% } %>
                    </ul>
                </div>
            </div>
            <div id="divPromotions" class="div-promotions-wrapper">
                <img id="promoLoader" src="/_Static/Css/images/ajax-loader.gif" style="display: none;" />
                <div id="divPromoHeader" onclick="javascript:$(this).next().children(':nth-child(n+4)').slideToggle();"><span><%=commonCulture.ElementValues.getResourceString("promotions", commonVariables.LeftMenuXML)%></span></div>
            </div>
            <div class="div-mobile-download"><img src="/_Static/Images/Download/W88-Mobile-ClubW-<%=commonVariables.SelectedLanguageShort%>.jpg" /><img src="/_Static/Images/Download/W88-Mobile-MassimoCasino-<%=commonVariables.SelectedLanguageShort%>.jpg" /></div>
        </div>
        <!-- /content -->
        <!--#include virtual="~/_static/footer.shtml" -->
        <!--#include virtual="~/_static/navMenu.shtml" -->
    </div>
    <!-- /page -->
</body>
</html>
