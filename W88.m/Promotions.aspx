<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Promotions.aspx.cs" Inherits="_Promotions" %>

<!DOCTYPE html>
<html>
<head>
    <title><%=commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML) + commonCulture.ElementValues.getResourceString("promotions", commonVariables.LeftMenuXML)%></title>
    <!--#include virtual="~/_static/head.inc" -->
    <link type="text/css" href="/_Static/Css/Promotions.css" rel="stylesheet" />
    <script type="text/javascript" src="/_Static/Js/Main.js"></script>
    <script type="text/javascript">
        $(function () { getPromos(); });
        $(window).resize(function () { $('.div-promo-row > a > div:last-child > div').css({ maxWidth: ($(window).width() - 200) + 'px' }); });
        function timerV2(pid, start_date, end_date) { if (new Date('<%=System.DateTime.Now.ToString(commonVariables.DateTimeFormat)%>') < new Date(start_date) || new Date('<%=System.DateTime.Now.ToString(commonVariables.DateTimeFormat)%>') > new Date(end_date)) { $('div#' + pid).hide(); } }
        function getPromos() {
            $.get('/_Static/Promotions/promotions.<%=commonVariables.SelectedLanguage + (string.Compare(commonVariables.GetSessionVariable("CountryCode"), "my", true) == 0 ? ".my" : "")%>.htm', function (html) { })
            .done(function (data) {
                data = data.replace(/<img src=/g, '<img rel=');
                var listObj = $("#divPromotions").append('<ul></ul>').find('ul');
                var promo_length = $(data).find('.promotion_group').length;
                $(data).find('.promotion_group').each(function (index) {
                    if (index == promo_length - 1) { return; }
                    var strPromoTitle = $(this).find('div.promotion_title').text();
                    var strPromoContent = $(this).find('div.promotion_content').text();
                    var strPromoDetail = ($(this).find('div.promotion_detail').html().substr(0, 4) == '<br>' ? $(this).find('div.promotion_detail').html().substring(4) : $(this).find('div.promotion_detail').html()).replace(/<img rel=/g, '<img src=');

                    var objImage = $(this).find('img')[0];
                    var strImageSrc = null;
                    if (objImage != null) { if (/\/promotions\/img\/W88-Promotion(s)*-/i.test($(objImage).attr('rel'))) { strImageSrc = $(objImage).attr('rel').replace(/\/promotions\/img\/W88-Promotion(s)*-/i, '/promotions/mobile/images/w88-mobile-'); } }

                    var liPromo = $('<li />');
                    var divPromoWrapper = $('<div />', { id: $(this).attr('id'), class: index % 2 == 0 ? 'div-promo-row' : 'div-promo-row div-promo-row-alt' });
                    var divPromoImg = $('<div />', { class: 'div-promo-img' });

                    var imgPromo = $('<img />', { src: strImageSrc });
                    var hrefPromo = $('<a />', { href: "javascript:void(0)", onclick: "javascript:OpenPromoDetails(this);" });
                    var divPromoTitle = $('<div />', { class: 'div-promo-header' }).text(strPromoTitle);
                    var divPromoContent = $('<div />', { class: 'div-promo-desc' }).text(strPromoContent);
                    var divPromoDetail = $('<div />', { class: 'div-promo-content' }).html(strPromoDetail);

                    var pViewMore = null;

                    if ($(this).find('div.p-tnc').length > 0) { pViewMore = $('<p />').append($('<a />', { href: 'javascript:void(0);', onclick: 'javascript:$(this).parents(".div-promo-content").find(".p-tnc").next().andSelf().slideDown();$(this).remove();' }).text('<%=commonCulture.ElementValues.getResourceString("lblMoreInfo", xeResources)%>')); }
                    if ($(this).find('.promo_join_btn').length > 0) {
                        if ('<%=commonVariables.CurrentMemberSessionId%>'.trim() == '') {
                            var hrefJoin = $('<a />', { class: 'ui-btn ui-blue ui-mini', 'data-transition': 'flip', href: '/_Secure/Register.aspx' }).text('<%=commonCulture.ElementValues.getResourceString("joinnow", commonVariables.LeftMenuXML)%>');
                            $(divPromoDetail).append(pViewMore).append(hrefJoin);
                        }
                        else {
                            var objCode = $(this).find('.promo_join_btn[href^="/promotions/promo_claim.aspx?code="]');
                            if ($(objCode).length > 0) {
                                var strCode = $(objCode).attr('href').replace(/\/promotions\/promo_claim.aspx\?code=/, '');
                                var hrefClaim = $('<a />', { class: 'ui-btn ui-blue ui-mini', href: 'javascript:void(0)', onclick: 'javascript:PromoClaimNow(this, \'' + strCode + '\', \'\')' }).text($(objCode).text());
                                $(divPromoDetail).append(pViewMore).append(hrefClaim);
                            }
                            else {
                                var objCode = $(this).find('.promo_join_btn[href^="/promotions/promo_apply_v2.aspx?code="]');
                                if ($(objCode).length > 0) {
                                    $obj = $(objCode).attr('href');
                                    var strCode = $obj.substring($obj.indexOf('=') + 1, $obj.indexOf('&'));
                                    var strProducts = $obj.substr($obj.lastIndexOf('=') + 1, $obj.length);
                                    var hrefClaim = $('<a />', { class: 'ui-btn ui-blue ui-mini', href: 'javascript:void(0)', onclick: 'javascript:PromoClaimNow(this, \'' + strCode + '\',  \'' + strProducts + '\')' }).text($(objCode).text());
                                    $(divPromoDetail).append(pViewMore).append(hrefClaim);
                                }
                            }
                        }
                    }
                    listObj.append($(liPromo).append($(divPromoWrapper).append($(hrefPromo).append($(divPromoImg).append(imgPromo)).append($('<div />', {}).append(divPromoTitle).append(divPromoContent)))).append(divPromoDetail));
                    $('.div-promo-row > a > div:last-child > div').css({ maxWidth: ($(window).width() - 200) + 'px' });
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

        function OpenPromoDetails(obj) {
            var selected_promo_id = $(obj).parent().attr('id');

            $('.div-promo-row').each(function () {
                if ($(this).attr('id') != selected_promo_id) {
                    $(this).find('a > div:last-child').css('background-image', "url('/_Static/Images/arrow-down.png')");
                    $(this).next().slideUp();
                }
                else {
                    if ($(this).find('a > div:last-child').css('background-image').indexOf('arrow-up') > 0) { $(this).find('a > div:last-child').css('background-image', "url('/_Static/Images/arrow-down.png')"); }
                    else { $(this).find('a > div:last-child').css('background-image', "url('/_Static/Images/arrow-up.png')"); }
                    $(this).next().slideToggle();
                }
            });
        }

        function PromoClaimNow(obj, code, products) {
            if ('<%=commonVariables.CurrentMemberSessionId%>'.trim() == '') {
                location.assign('_Secure/Register.aspx');
            } else {
                $(obj).hide();
                if (products.length == 0) {
                    var divCode = $('<div />', { class: 'div-claim-promo-header' }).text(code);
                    var divPromoClaimWrapper = $('<div />', { class: 'div-claim-promo' });
                    var divPromoClaimData = $('<div />', { class: 'div-claim-promo-data' });
                    var taPromoClaim = $('<textarea />');

                    var divPromoClaimButtons = $('<div />');
                    var hrefClaim = $('<a />', { class: 'ui-btn ui-blue ui-mini', onclick: 'javascript:PromoClaim(this)' }).text('<%=commonCulture.ElementValues.getResourceString("btnSubmit", xeResources)%>');
                    var hrefClaimCancel = $('<a />', { class: 'ui-btn ui-gray ui-mini', onclick: 'javascript:PromoCancelClaim(this)' }).text('<%=commonCulture.ElementValues.getResourceString("btnCancel", xeResources)%>');

                    $(divPromoClaimButtons).append(hrefClaim).append(hrefClaimCancel)
                    $(divPromoClaimData).append(taPromoClaim).append(divPromoClaimButtons);
                    $(divPromoClaimWrapper).append(divCode).append(divPromoClaimData);
                    $(obj).parent().append(divPromoClaimWrapper);
                }
                else {
                    var arrCodes = [];
                    var arrProducts = [];

                    arrCodes = code.split(',');
                    arrProducts = products.split(',');

                    var divCode = $('<div />', { class: 'div-claim-promo-header' }).text('<%=commonCulture.ElementValues.getResourceString("lblMultipleRebateCode", xeResources)%>');
                    var divPromoClaimWrapper = $('<div />', { class: 'div-claim-promo' });
                    var divPromoClaimData = $('<div />', { class: 'div-claim-promo-data' });
                    //var taPromoClaim = $('<textarea />');

                    var divPromoClaimButtons = $('<div />');

                    var hrefClaim = $('<a />', { class: 'ui-btn ui-blue ui-mini', onclick: 'javascript:PromoClaim(this)' }).text('<%=commonCulture.ElementValues.getResourceString("btnSubmit", xeResources)%>');
                    var hrefClaimCancel = $('<a />', { class: 'ui-btn ui-gray ui-mini', onclick: 'javascript:PromoCancelClaim(this)' }).text('<%=commonCulture.ElementValues.getResourceString("btnCancel", xeResources)%>');

                    $(divPromoClaimButtons).append(hrefClaim).append(hrefClaimCancel)

                    $.each(arrProducts, function (index, value) {
                        var code = arrCodes.length > 1 ? arrCodes[index] : arrCodes[0];
                        var divRadio = $('<div />', { class: 'div-promo-radio' });
                        var taPromoRadio = $('<input />', { type: 'radio', name: 'comment', value: code + '|' + arrProducts[index], id: 'rad' + code });
                        var taPromoLabel = $('<label />', { for: 'rad' + code }).text(value + ' - ' + code);

                        switch (arrProducts[index]) {
                            case 'asports':
                                taPromoLabel.text('<%=commonCulture.ElementValues.getResourceXPathString("/Products/ASports/Label", commonVariables.ProductsXML)%> - ' + code);
                                break;
                            case 'esports':
                                taPromoLabel.text('<%=commonCulture.ElementValues.getResourceXPathString("/Products/ESports/Label", commonVariables.ProductsXML)%> - ' + code);
                                break;
                            case 'isports':
                                taPromoLabel.text('<%=commonCulture.ElementValues.getResourceXPathString("/Products/ISports/Label", commonVariables.ProductsXML)%> - ' + code);
                                break;
                            case 'usports':
                                taPromoLabel.text('<%=commonCulture.ElementValues.getResourceXPathString("/Products/USports/Label", commonVariables.ProductsXML)%> - ' + code);
                                break;
                            case 'casino':
                                taPromoLabel.text('<%=commonCulture.ElementValues.getResourceXPathString("/Products/ClubW/Label", commonVariables.ProductsXML)%> - ' + code);
                                break;
                            case 'bravado':
                                taPromoLabel.text('<%=commonCulture.ElementValues.getResourceXPathString("/Products/ClubBravado/Label", commonVariables.ProductsXML)%> - ' + code);
                                break;
                            case 'crescendo':
                                taPromoLabel.text('<%=commonCulture.ElementValues.getResourceXPathString("/Products/ClubCrescendo/Label", commonVariables.ProductsXML)%> - ' + code);
                                break;
                            case 'divino':
                                taPromoLabel.text('<%=commonCulture.ElementValues.getResourceXPathString("/Products/ClubDivino/Label", commonVariables.ProductsXML)%> - ' + code);
                                break;
                        }
                        $(divPromoClaimData).append($(divRadio).append(taPromoRadio).append(taPromoLabel));
                    });

                    $(divPromoClaimWrapper).append(divCode).append(divPromoClaimData).append(divPromoClaimButtons);
                    $(obj).parent().append(divPromoClaimWrapper);
                }
            }
        }

        function PromoCancelClaim(obj) { if (obj != null) { $obj = $(obj).parents('.div-claim-promo'); $obj.hide(); $obj.prev().show(); } }

        function PromoClaim(obj) {
            var $obj = $(obj).parents('.div-claim-promo');
            var strCode = $obj.find('.div-claim-promo-header').text();
            var strComment = null; // $(obj).parent().children().find('textarea').val();

            if ($obj.find('textarea').length == 0) {
                var radValue = $obj.find('input[type="radio"]:checked').val();
                strCode = radValue.split('|')[0];
                strComment = radValue.split('|')[1];
            }
            else { strComment = $obj.find('textarea').val(); }

            $.ajax({
                type: 'POST',
                url: '/AjaxHandlers/RegisterPromo.ashx',
                data: { sCode: strCode, Comment: strComment },
                beforeSend: function () {
                    PromoCancelClaim(obj);
                },
                success: function (data) {
                    switch (parseInt(data)) {
                        case 1: // success
                            alert('<%=commonCulture.ElementValues.getResourceXPathString("/Promotion/RegisterSuccess", xeErrors)%>');
                            $obj.hide();
                            //$(obj).parent().prev().show();
                            break;
                        case 10: // multiple submit
                            alert('<%=commonCulture.ElementValues.getResourceXPathString("/Promotion/SubmitOnce", xeErrors)%>');
                            $obj.hide();
                            break;

                        default: // error
                            alert('<%=commonCulture.ElementValues.getResourceString("ServerError", xeErrors)%>');
                                $obj.hide();
                                break;
                        }
                },
                error: function (data) { },
                complete: function (data) { PromoCancelClaim($obj); }
            });
            }
    </script>
</head>
<body>
    <!--#include virtual="~/_static/splash.shtml" -->
    <div id="divMain" data-role="page" data-theme="b" data-ajax="false">
        <!--#include virtual="~/_static/header.shtml" -->

        <div class="ui-content" role="main">
            <img id="promoLoader" src="/_Static/Css/images/ajax-loader.gif" style="display: none;" />
            <div class="div-page-header"><span><%=commonCulture.ElementValues.getResourceString("promotions", commonVariables.LeftMenuXML)%></span></div>
            <div class="page-content div-promotions-wrapper" id="divPromotions"></div>
        </div>

        <!-- /content -->
        <!--#include virtual="~/_static/footer.shtml" -->
        <!--#include virtual="~/_static/navMenu.shtml" -->
    </div>
    <!-- /page -->
</body>
</html>
