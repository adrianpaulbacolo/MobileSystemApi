<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Site.master" AutoEventWireup="true" CodeFile="Promotions.aspx.cs" Inherits="Promotions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
        _.templateSettings = {
            interpolate: /\{\{(.+?)\}\}/g,      // print value: {{ value_name }}
            evaluate: /\{%([\s\S]+?)%\}/g,   // excute code: {% code_to_execute %}
            escape: /\{%-([\s\S]+?)%\}/g
        };
        _.templateSettings.variable = "promo";

        var lang = '<%=(string.IsNullOrEmpty(commonVariables.SelectedLanguage) ? "en-us" : commonVariables.SelectedLanguage)%>',
            isCategoryHash = false,
            domain,
            categories = [];
        if (lang == '') { lang = 'en-us'; }
        $(function () {
            if(!_.isEmpty(window.location.hash)) {
                getPromos(window.location.hash.substring(1));
            } else {
                getPromos('ALL');
            }
            });
        // temporarily restrict promo
        var restrictedPromos = {};
        restrictedPromos.DAILYSLOTS = {
            allowed: ["RMB"],
            langAllowed: ["zh-cn"]
        };
        var currentCCode = '<%= commonCookie.CookieCurrency%>';

        function timerV2(pid, start_date, end_date) { if (new Date('<%=DateTime.Now.ToString(commonVariables.DateTimeFormat)%>') < new Date(start_date) || new Date('<%=DateTime.Now.ToString(commonVariables.DateTimeFormat)%>') > new Date(end_date)) { $('div#' + pid).hide(); } }
            function filterPromos(category) {
                $("#divPromotions").html('');
                var listObj = $("#divPromotions").append('<ul class="row row-uc row-no-padding row-wrap"></ul>').find('ul'),
                    _category = _.find(categories, { id: category });

                if (!_category) {
                    isCategoryHash = false;
                    _category = _.find(categories, function (cat) {
                        var promo = _.find(cat.promos, function (_promos) {
                            if ($(_promos).attr('id') == category)
                                return _promos;
                        });
                        if (promo) return cat;                       
                    });
                }
                if (!_category) return;
                category = _category.id;

                var promos = _category.promos;
                if (!promos || promos.length == 0) return;
                promos.each(function (index) {
                    if (index == promos.length - 1 && category == 'ALL') { return; }
                    var currentPromoId = $(this).attr('id');
                    if (!_.isUndefined(restrictedPromos[currentPromoId])) {
                        if (_.isEmpty(currentCCode)) {
                            if (_.indexOf(restrictedPromos[currentPromoId].langAllowed, lang) == -1) {
                                return;
                            }
                        } else {
                            if (!_.isUndefined(restrictedPromos[currentPromoId].allowed)) {
                                if (_.indexOf(restrictedPromos[currentPromoId].allowed, currentCCode) == -1) {
                                    return;
                                }
                            }
                        }
                    }

                    var hostName = window.location.host;
                    var firstDot = hostName.indexOf('.') + 1;
                    domain = hostName.substr(firstDot, hostName.length - firstDot);

                    $(this).find('.promotion_detail a').each(function (index, item) {
                        if (_.includes(item.href.toLowerCase(), 'leaderboard')) {
                            item.href = window.location.protocol + '//www.' + domain + item.pathname + item.search + '&nomobile=true';
                        }
                    });

                    var strPromoTitle = $(this).find('div.promotion_title').text();
                    var strPromoContent = $(this).find('div.promotion_content').text();
                    var promoDetailHtml = $(this).find('div.promotion_detail').html();
                    var strPromoDetail;
                    if (promoDetailHtml != undefined) {
                        strPromoDetail = promoDetailHtml.substr(0, 4) == '<br>' ? promoDetailHtml.substring(4) : promoDetailHtml.replace(/<img rel=/g, '<img src=');
                    }
                    var objImage = $(this).find('img')[0];
                    var strImageSrc = null;
                    if (objImage != null) {
                        if (/\/promotions\/img\/W88(-vip)?-Promotion(s)*-/i.test($(objImage).attr('rel'))) {
                            strImageSrc = $(objImage).attr('rel').replace(/-small/i, '-big');
                        }
                    }

                    var liPromo = $('<li class="col" />');
                    var divPromoWrapper = $('<div />', { id: $(this).attr('id'), class: index % 2 == 0 ? 'div-promo-row' : 'div-promo-row' });
                    var divPromoImg = $('<div />', { class: 'div-promo-img' });

                    var imgPromo = $('<img />', { src: strImageSrc, onclick: "javascript:OpenPromoDetails(this);" });
                    var hrefPromo = $('<a />', { href: "javascript:void(0)", onclick: "javascript:OpenPromoDetails(this);" });

                    var divJoinButton = $('<div />', { class: 'div-promo-join' });
                    var divPromoContent = $('<div />', { class: 'div-promo-desc' }).text(strPromoContent);
                    var divPromoDetail = $('<div />', { class: 'div-promo-content' }).html(/<img rel=/g.test(strPromoDetail) ? strPromoDetail.replace(/<img rel=/g, '<img src=') : strPromoDetail);

                    var pViewMore = null;

                    if ($(this).find('div.p-tnc').length > 0) { pViewMore = $('<p />').append($('<a />', { href: 'javascript:void(0);', onclick: 'javascript:$(this).parents(".div-promo-content").find(".p-tnc").next().andSelf().slideDown();$(this).remove();' }).text('<%=commonCulture.ElementValues.getResourceString("lblMoreInfo", xeResources)%>')); $(divPromoDetail).append(pViewMore); }

                    if ($(this).find('.promo_join_btn').length > 0) {
                        if ('<%=commonVariables.CurrentMemberSessionId%>'.trim() == '') {
                            var hrefJoin = $('<a />', { class: 'ui-btn btn-primary', href: '/_Secure/Register.aspx', 'data-ajax': 'false' }).text('<%=commonCulture.ElementValues.getResourceString("joinnow", commonVariables.LeftMenuXML)%>');
                            //$(divPromoDetail).append(hrefJoin);
                            $(divJoinButton).append(hrefJoin);
                        }
                        else {

                            var dailySlot = $(this).find('.promo_join_btn[type="mobile"]');
                            if ($(dailySlot).length > 0) {
                                var hrefApply = $('<a />', { class: 'ui-btn btn-primary', 'data-transition': 'flip', href: dailySlot.attr('href') }).text($(dailySlot).text());
                                $(divJoinButton).append(hrefApply);
                            }

                            var objCode = $(this).find('.promo_join_btn[href^="/promotions/promo_claim.aspx?code="]');
                            if ($(objCode).length > 0) {
                                var strCode = $(objCode).attr('href').replace(/\/promotions\/promo_claim.aspx\?code=/, '');
                                var hrefClaim = $('<a />', { class: 'ui-btn btn-primary', href: 'javascript:void(0)', onclick: 'javascript:PromoClaimNow(this, \'' + strCode + '\', \'\')' }).text($(objCode).text());
                                $(divJoinButton).append(hrefClaim);
                            }
                            else {
                                var objCode = $(this).find('.promo_join_btn[href^="/promotions/promo_apply_v2.aspx?code="]');
                                if ($(objCode).length > 0) {
                                    $obj = $(objCode).attr('href');
                                    var strCode = $obj.substring($obj.indexOf('=') + 1, $obj.indexOf('&'));
                                    var strProducts = $obj.substr($obj.lastIndexOf('=') + 1, $obj.length);
                                    var hrefClaim = $('<a />', { class: 'ui-btn btn-primary', href: 'javascript:void(0)', onclick: 'javascript:PromoClaimNow(this, \'' + strCode + '\',  \'' + strProducts + '\')' }).text($(objCode).text());
                                    $(divJoinButton).append(hrefClaim);
                                }

                                var objCode = $(this).find('.promo_join_btn[href^="/promotions/promo_apply_v3.aspx?promoid="]');
                                if ($(objCode).length > 0) {
                                    $obj = $(objCode).attr('href');
                                    var strCode = $obj.substring($obj.indexOf('=') + 1, $obj.indexOf('&'));
                                    $.get('/_Static/Promotions/' + strCode + '.' + lang + '.xml', function (xml) {
                                        var title = $(xml).find('lbl').text();
                                        strProducts =
                                            $(xml).find('item').map(function () {
                                                return $(this).attr('name');
                                            }).get().join();
                                        var hrefClaim = $('<a />', { class: 'ui-btn btn-primary', href: 'javascript:void(0)', onclick: 'javascript:PromoClaimNow(this, \'' + strCode + '\',  \'' + strProducts + '\',  \'' + title + '\')' }).text($(objCode).text());

                                        if ('<%=commonVariables.GetSessionVariable("RiskId")%>'.search(/vip(b|p|g|d)/i) > -1) {
                                            $(divJoinButton).append(hrefClaim);
                                        }
                                    });
                                }

                                var objCode = $(this).find('.promo_join_btn[href^="/promotions/promo_apply_v4.aspx?promoid="]');
                                if ($(objCode).length > 0) {
                                    $obj = $(objCode).attr('href');
                                    var strCode = $obj.substring($obj.indexOf('=') + 1);

                                    var hrefClaim = $('<a />', { class: 'ui-btn btn-primary', href: 'javascript:void(0)', onclick: 'javascript:PromoClaimNowMatch(this, \'' + strCode + '\',  \'' + lang + '\', "v4")' }).text($(objCode).text());
                                    $(divJoinButton).append(hrefClaim);
                                }

                                var objCode = $(this).find('.promo_join_btn[href^="/promotions/promo_apply_v5.aspx?promoid="]');
                                if ($(objCode).length > 0) {
                                    $obj = $(objCode).attr('href');
                                    var strCode = $obj.substring($obj.indexOf('=') + 1);

                                    var hrefClaim = $('<a />', { class: 'ui-btn btn-primary', href: 'javascript:void(0)', onclick: 'javascript:PromoClaimNowMatch(this, \'' + strCode + '\',  \'' + lang + '\', "v5")' }).text($(objCode).text());
                                    $(divJoinButton).append(hrefClaim);
                            }

                        }
                    }
                    }

                    var divPromoTitle = $('<div />', { class: 'div-promo-header' }).text(strPromoTitle);

                    var divSecond = $('<div />', { class: 'div-promo-second', id: 'div-promo-second' }).append(divJoinButton).append(hrefPromo.append(divPromoTitle));

                    listObj.append($(liPromo).append($(divPromoWrapper).append($(divPromoImg).append(imgPromo)).append(divSecond)).append(divPromoDetail));
                    $(this).find('script').each(function () { $.globalEval(this.text || this.textContent || this.innerHTML || ''); });
                });
                scrollToTab(category);
                hashOpen();
            }

        function getPromos(category) {
            $.get('/AjaxHandlers/Promotion.ashx', function (html) { })
            .done(function (data) {
                data = data.replace(/<img src=/g, '<img rel=');
                data = data.replace('[domain]', '.' + location.hostname.split('.').slice(-2).join('.'));
                categories = getCategories(data);
                filterPromos(category);
            })
            .always(function (data) {
                $('#promoLoader').hide();
            });
        }

        function hashOpen() {
            if (location.hash != '') {
                $(location.hash).next().slideToggle();
                var divObj = $(location.hash).find('div')[1];
                if (divObj == undefined) return;
                if ($(divObj).css('background-image').indexOf('arrow-up') > 0) { $(divObj).css('background-image', "url('/_Static/Images/arrow-down.png')"); }
                else { $(divObj).css('background-image', "url('/_Static/Images/arrow-up.png')"); }

                setTimeout(function () {
                    var yPos = $(location.hash).get(0).offsetTop;
                    if (yPos < 0) yPos = 0;
                    $('#divPromotions').scrollTop(yPos);
                    var moreInfo = $(divObj).parent().siblings().find('p a');
                    if (moreInfo)
                        moreInfo.trigger('click');
                }, 400);
            }
        }

        function OpenPromoDetails(obj) {
            var selected_promo_id = $(obj).parent().parent().attr('id');
            $('.div-promo-row').each(function () {
                if ($(this).attr('id') != selected_promo_id) {
                    $(this).next().slideUp();
                }
                else {
                    $(this).next().slideToggle();
                }
            });
        }

        function PromoClaimNow(obj, code, products, title) {
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
                    var hrefClaim = $('<a />', { class: 'ui-btn btn-primary', onclick: 'javascript:PromoClaim(this)' }).text('<%=commonCulture.ElementValues.getResourceString("btnSubmit", xeResources)%>');
                    var hrefClaimCancel = $('<a />', { class: 'ui-btn btn-secondary', onclick: 'javascript:PromoCancelClaim(this)' }).text('<%=commonCulture.ElementValues.getResourceString("btnCancel", xeResources)%>');

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

                    var hrefClaim = $('<a />', { class: 'ui-btn btn-primary', onclick: 'javascript:PromoClaim(this)' }).text('<%=commonCulture.ElementValues.getResourceString("btnSubmit", xeResources)%>');
                    var hrefClaimCancel = $('<a />', { class: 'ui-btn btn-secondary', onclick: 'javascript:PromoCancelClaim(this)' }).text('<%=commonCulture.ElementValues.getResourceString("btnCancel", xeResources)%>');

                    $(divPromoClaimButtons).append(hrefClaim).append(hrefClaimCancel)

                    $.each(arrProducts, function (index, value) {
                        var code = arrCodes.length > 1 ? arrCodes[index] : arrCodes[0];
                        var divRadio = $('<div />', { class: 'div-promo-radio' });
                        var id = 'rad' + code + value;
                        var taPromoRadio = $('<input />', { type: 'radio', name: 'comment', value: code + '|' + arrProducts[index], id: id });
                        var taPromoLabel = $('<label />', { for: id }).text(value + ' - ' + code);

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
                            case 'massimo':
                                taPromoLabel.text('<%=commonCulture.ElementValues.getResourceXPathString("/Products/ClubMassimoCasino/Label", commonVariables.ProductsXML)%> - ' + code);
                                break;
                            case 'palazzo':
                                taPromoLabel.text('<%=commonCulture.ElementValues.getResourceXPathString("/Products/ClubPalazzoCasino/Label", commonVariables.ProductsXML)%> - ' + code);
                                break;
                            case 'keno':
                                taPromoLabel.text('<%=commonCulture.ElementValues.getResourceXPathString("/Products/Keno/Label", commonVariables.ProductsXML)%> - ' + code);
                                break;
                            case 'ilotto':
                                taPromoLabel.text('<%=commonCulture.ElementValues.getResourceXPathString("/Products/Lottery/Label", commonVariables.ProductsXML)%> - ' + code);
                                break;
                            case 'gallardo':
                                taPromoLabel.text('<%=commonCulture.ElementValues.getResourceXPathString("/Products/ClubGallardo/Label", commonVariables.ProductsXML)%> - ' + code);
                                break;
                            case 'nuovo':
                                taPromoLabel.text('<%=commonCulture.ElementValues.getResourceXPathString("/Products/ClubNuovo/Label", commonVariables.ProductsXML)%> - ' + code);
                                break;
                            case 'apollo':
                                taPromoLabel.text('<%=commonCulture.ElementValues.getResourceXPathString("/Products/ClubApollo/Label", commonVariables.ProductsXML)%> - ' + code);
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
            var strComment = ""; // $(obj).parent().children().find('textarea').val();

            if ($obj.find('textarea').length == 0) {
                if ($obj.find('input[type="radio"]:checked').length != 0) {
                    var radValue = $obj.find('input[type="radio"]:checked').val();
                    strCode = radValue.split('|')[0];
                    strComment = radValue.split('|')[1];
                } else {

                    var selectComment = $obj.find('select');
                    if (selectComment.length != 0) {
                        $.each(selectComment, function() {
                            strComment += selectComment.val() + " | ";
                        });
                    }

                    var matchComment = $obj.find('input[name="comment"]');

                    var emptyInputs = $(matchComment).filter(function (index, item) {
                        return item.value == "";
                    });

                    if (emptyInputs.length) {
                        emptyInputs[0].focus();
                        return;
                    }

                    $.each(matchComment, function (index) {
                        strComment += this.value;

                        if (index != matchComment.length - 1)
                            strComment += " | ";
                    });
                }
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

        function promoClaimTemplate(obj, code, lang) {
            $.get('/_Static/Promotions/templates/v4.html', function (data) {
                var template = _.template(data, {
                    data: {}
                });
                $(obj).parent().append(template).enhanceWithin();
            }, 'html');
        }

        function PromoClaimNowMatch(obj, code, lang, promoType) {
            if ('<%=commonVariables.CurrentMemberSessionId%>'.trim() == '') {
                location.assign('_Secure/Register.aspx');
            } else {
                $(obj).hide();
                var template = '';

                switch(promoType){
                    case 'v4':

                        $.get('/_Static/Promotions/' + code + '.' + lang + '.xml', function (xml) {
                            var promoData = {
                                code: code,
                                team_msg: $(xml).find('team_msg').text(),
                                score_msg: $(xml).find('score_msg').text(),
                                score_checking: $(xml).find('score_checking').text().trim(),
                                score_msg: $(xml).find('score_msg').text(),
                                team_setting: $(xml).find('team_setting team').map(function () {
                                    return $(this).text();
                                }).get(),
                                additional_column: $(xml).find('additional_column column').map(function (index, value) {
                                    return { field: $(value).find('field').text(), regex: $(value).find('regex').text().trim() };
                                }).get()
                            };

                            $.get('/_Static/Promotions/templates/v4.html', function (data) {
                                template = _.template(data);
                                $(obj).parent().append(template({
                                    data: promoData
                                })).enhanceWithin();
                            }, 'html');
                        });

                        break;
                    case 'v5':

                        $.get('/_Static/Promotions/' + code + '.' + lang + '.xml', function (xml) {
                            var promoData = {};
                            promoData.username = '<%= base.userInfo.MemberCode %>';
                            $(xml).find('column').each(function (index, value) {
                                if (!_.isEmpty($(value).find('field'))) promoData['label' + index] = $(value).find('field').text();
                                if (!_.isEmpty($(value).find('regex'))) promoData['regex' + index] = $(value).find('regex').text();
                                if (!_.isEmpty($(value).find('options'))) {
                                    promoData['option' + index] = [];
                                    $(value).find('options').find('option').each(function(i, v) {
                                        promoData['option' + index].push($(v).text());
                                    });
                                }
                            });
                            $.get('/_Static/Promotions/templates/v5.html', function (data) {
                                var btnSubmit = '<%=commonCulture.ElementValues.getResourceString("btnSubmit", xeResources)%>';
                                var btnCancel = '<%=commonCulture.ElementValues.getResourceString("btnCancel", xeResources)%>';
                                template = _.template(data);
                                promoData['code'] = code,
                                $(obj).parent().append(template({
                                    data: promoData,
                                    btnSubmit: btnSubmit,
                                    btnCancel: btnCancel
                                })).enhanceWithin();
                            }, 'html');
                        });

                    default:
                        break;
                }


            }
        }

        function DataRegex(obj) {
            if ($(obj).attr('data-regex')) {
                var regex = new RegExp($(obj).attr('data-regex'));
                var match = regex.exec(obj.value);
                if (match) {
                    obj.value = match[1];
                }
                else {
                    obj.value = "";
                }
            }
        }

        function getCategories(data) {
            var categories = [];
            // For 'ALL' category
            categories.push({
                id: 'ALL',
                text: '<%=commonCulture.ElementValues.GetResourceXPathAttribute("status", "id", "ALL", commonVariables.HistoryXML)%>',
                promos: $(data).find('.promotion_group')
            });
            $('#categories').append('<div class="btn-group" role="group"><a id="' + categories[0].id + '"class="btn" href="#' + categories[0].id + '">' + categories[0].text + '</a></div>');
            $('#' + categories[0].id).on('click', function () {
                isCategoryHash = true;
                window.location.hash = categories[0].id;
                filterPromos(window.location.hash.substring(1));
            });

            $(data).find('.promotion_group_header').each(function (index, div) {
                var _category = {},
                    siblings = $(div).siblings();

                if (siblings.length == 0) return;
                _category['promos'] = siblings;
                _category['id'] = $(div).attr('id');
                _category['text'] = $(div).html().trim();

                var otherPromos = $(data).find('.grp_' + _category.id);
                if (otherPromos && otherPromos.length > 0)
                    _category.promos.push.apply(_category.promos, otherPromos);

                categories.push(_category);
                $('#categories').append('<div class="btn-group" role="group"><a id="' + _category.id + '"class="btn" href="#' + _category.id + '">' + _category.text + '</a></div>');
                $('#' + _category.id).on('click', function () {
                    isCategoryHash = true;
                    window.location.hash = _category.id;
                    filterPromos(window.location.hash.substring(1));
                });
            });
            return categories;
        }

        function scrollToTab(category) {
            if (_.isEmpty(category)) return;

            var button = $('#' + category).parent();
            if (!button) return;
            if (!button.hasClass('active')) {
                button.addClass('active');
            }

            $('#categories').children().children().each(function(i, div) {
                if ($(div).parent().hasClass('active') && $(div).attr('id') !== category)
                    $(div).parent().removeClass('active');
            });
            var index = _.findIndex(categories, { id: category });

            $('#categories').show();
            $('div.footer.footer-div.footer-generic').scrollLeft(index * 80);
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="ui-content" role="main">
        <img id="promoLoader" src="/_Static/Css/images/ajax-loader.gif" style="display: none;" />
        <div id="divPromotions" class="main-content"></div>
        <div class="footer footer-div footer-generic">
            <div id="categories" style="display: none;" class="btn-group btn-group-justified btn-group-sliding"></div>
        </div>
    </div>
</asp:Content>




