function Rebates() {

    var Rebates = {
        Initialize: initialize,
        Statement: statement,
        ClaimQuery: claimQuery,
        ClaimNow: claimNow,
        GetWeeklySettings: getWeeklySettings,
        SubmitWeeklyClaim: submitWeeklyClaim
    };

    _.templateSettings = {
        interpolate: /\{\{(.+?)\}\}/g,      // print value: {{ value_name }}
        evaluate: /\{%([\s\S]+?)%\}/g,   // excute code: {% code_to_execute %}
        escape: /\{%-([\s\S]+?)%\}/g
    };
    _.templateSettings.variable = "rebate";

    function send(resource, method, data, beforeSend, success, complete) {
        var url = w88Mobile.APIUrl + resource;

        var headers = {
            'Token': window.User.token,
            'LanguageCode': window.User.lang
        };

        $.ajax({
            type: method,
            url: url,
            data: data,
            beforeSend: beforeSend,
            headers: headers,
            success: success,
            error: function () {
                console.log("Error connecting to api");
            },
            complete: complete
        });
    }

    function initialize() {
        translations();
        weeks();
    }

    function translations() {
        send("/contents", "GET", "", "", function (response) {
            if (response && _.isEqual(response.ResponseCode, 1)) {
                
                $('#labelPeriod').html(response.ResponseData.LABEL_REBATE_PERIOD);
                $('#monday').html(response.ResponseData.LABEL_MONDAY);
                $('#sunday').html(response.ResponseData.LABEL_SUNDAY);
                $('#rebateDisclaimer').html(response.ResponseData.LABEL_REBATE_DISCLAIMER);
                $('#rebateDisclaimerMin').html(response.ResponseData.LABEL_REBATE_NOTE1);
                $('#rebateDisclaimerNoteCurrent').html(response.ResponseData.LABEL_REBATE_DISCLAIMER_CONTENT_CURRENT);
                $('#rebateDisclaimerNote1').html(response.ResponseData.LABEL_REBATE_DISCLAIMER_CONTENT1);
                $('#rebateDisclaimerNote2').html(response.ResponseData.LABEL_REBATE_DISCLAIMER_CONTENT2);
                
                sessionStorage.setItem("weeklyClaim", response.ResponseData.BUTTON_WEEKLY_CLAIM);
                sessionStorage.setItem("promoOption", response.ResponseData.LABEL_PROMO_OPTION);

                sessionStorage.setItem("monday", response.ResponseData.LABEL_MONDAY);
                sessionStorage.setItem("sunday", response.ResponseData.LABEL_SUNDAY);
                sessionStorage.setItem("summary", response.ResponseData.LABEL_REBATE_SUMMARY);
                sessionStorage.setItem("rebateNote1", response.ResponseData.LABEL_REBATE_NOTE1);
                sessionStorage.setItem("rebateNote2", response.ResponseData.LABEL_REBATE_NOTE2);
                sessionStorage.setItem("rebateAmount", response.ResponseData.LABEL_REBATE_AMOUNT);
                sessionStorage.setItem("rebateBets", response.ResponseData.LABEL_REBATE_BETS);
                sessionStorage.setItem("rebatePercent", response.ResponseData.LABEL_REBATE_PERCENT);
                sessionStorage.setItem("congrats", response.ResponseData.LABEL_CONGRATS);
                sessionStorage.setItem("claimedAmount", response.ResponseData.LABEL_REBATE_CLAIMED_AMOUNT);
                sessionStorage.setItem("balanceAmount", response.ResponseData.LABEL_REBATE_BALANCE_AMOUNT);
                sessionStorage.setItem("btnClaim", response.ResponseData.BUTTON_CLAIM);
                sessionStorage.setItem("btnSubmit", response.ResponseData.BUTTON_SUBMIT);
                sessionStorage.setItem("btnInstant", response.ResponseData.BUTTON_INSTANT_CLAIM);
                sessionStorage.setItem("weeklySuccess", response.ResponseData.LABEL_REBATE_WEEKLYCLAIM_SUCCESS);
                sessionStorage.setItem("weeklyMultiple", response.ResponseData.LABEL_REBATE_WEEKLYCLAIM_MULTIPLE);
                sessionStorage.setItem("weeklyError", response.ResponseData.LABEL_REBATE_WEEKLYCLAIM_DEFAULT);
            }
        }, "");
    }

    function weeks() {
        send("/rebates/week", "GET", "", function () { GPInt.prototype.ShowSplash(false); }, function (response) {
            if (response && _.isEqual(response.ResponseCode, 1)) {

                _.forOwn(response.ResponseData, function (data) {
                    $('#weeks').append($('<option>').text(data.Text).attr('value', data.Value));
                });

                $("#weeks").val($("#weeks option:first").val()).change();
                sessionStorage.setItem("startdate", $("#weeks").val());
            }
        }, "");
    }

    function statement() {
        sessionStorage.setItem("startdate", $("#weeks").val());

        var strtDate = sessionStorage.getItem("startdate").split("|");
        var sDate = { startdate: strtDate[0].replace("/", "-").replace("/", "-") };
        
        $("#startdate").html(strtDate[0]);
        $("#endDate").html(strtDate[1]);

        send("/rebates/result", "GET", sDate, function () {
            GPInt.prototype.HideSplash();
            GPInt.prototype.ShowSplash(false);
        }, function (response) {
            if (response && _.isEqual(response.ResponseCode, 1)) {

                $.get('/_Static/templates/rebates/ClaimGroups.html', function(data) {
                    var template = _.template(data);

                    $("#group").html(template({
                        data: response.ResponseData.RebateRow,
                        LabelRebateAmount: sessionStorage.getItem("rebateAmount"),
                        LabelRebatePercent: sessionStorage.getItem("rebatePercent"),
                        LabelRebateBets: sessionStorage.getItem("rebateBets"),
                        BtnClaim: sessionStorage.getItem("btnClaim"),
                    })).enhanceWithin();

                    $('#rebateDisclaimerMin').html(sessionStorage.getItem("rebateNote1") + " " + Cookies().getCookie("currencyCode") + " " + response.ResponseData.MinimumClaim);
                    $("#weeklyBtn").html(sessionStorage.getItem("weeklyClaim"));

                    $(".collapsible-btn").click(function(e) {
                        e.preventDefault();

                        $(this).parent().find('.collapsible-table').slideToggle("fast");

                        if($(this).hasClass('collapsed')){
                            $(this).removeClass('collapsed');
                            $(this).find('span').html('See More');
                        }
                        else{
                            $(this).addClass('collapsed');
                            $(this).find('span').html('See Less');
                        }
                    });

                }, 'html');

                GPInt.prototype.HideSplash();

            } else {
                w88Mobile.Growl.shout(response.ResponseMessage, function() {
                    window.location.replace("/Profile");
                });
            }
        }, "");
    }

    function claimQuery(productCode, allowClaim) {

        _.templateSettings.variable = "rebate";

        if (allowClaim) {

            var strtDate = sessionStorage.getItem("startdate").split("|");
            var sDate = strtDate[0].replace("/", "-").replace("/", "-");
            var query = { startdate: sDate, code: productCode };

            send("/rebates/query", "GET", query, function () { GPInt.prototype.ShowSplash(false); }, function (response) {
                if (response && _.isEqual(response.ResponseCode, 1)) {

                    var d = {
                        ProductCode: productCode,
                        EligibleBets: response.ResponseData.TotalEligibleBet,
                        rebatePercent:  response.ResponseData.RebatePercent,
                        Amount:  response.ResponseData.RebateAmount,
                        AllowClaim: allowClaim,
                        ClaimedAmount: response.ResponseData.ClaimedAmount,
                        BalanceRebateAmount: response.ResponseData.BalanceRebateAmount,
                        CurrencyCode: response.ResponseData.CurrencyCode,
                        MinimumClaim: response.ResponseData.MinimumClaim,
                        Monday: sessionStorage.getItem("monday"),
                        Sunday: sessionStorage.getItem("sunday"),
                        StartDate: strtDate[0],
                        EndDate: strtDate[1],
                        LabelSummary: sessionStorage.getItem("summary"),
                        note1: sessionStorage.getItem("rebateNote1"),
                        note2: sessionStorage.getItem("rebateNote2"),
                        LabelRebateAmount: sessionStorage.getItem("rebateAmount"),
                        LabelRebatePercent: sessionStorage.getItem("rebatePercent"),
                        LabelRebateBets: sessionStorage.getItem("rebateBets"),
                        LabelClaimedAmount: sessionStorage.getItem("claimedAmount"),
                        LabelBalanceAmount: sessionStorage.getItem("balanceAmount"),
                        btnInstantClaim: sessionStorage.getItem("btnInstant")
                };
                    $.get('/_Static/templates/rebates/claimModal.html', function(data) {
                        var template = _.template(data);

                        $("#modalContent").html(template({
                            data: d
                        })).enhanceWithin();

                    }, 'html');

                    $('#rebatesModal').popup('open');
                    GPInt.prototype.HideSplash();

                } else {
                    w88Mobile.Growl.shout(response.ResponseMessage, function () {
                        window.location.replace("/Profile");
                    });
                }
            }, "");
        }
    }

    function claimNow(productCode, amount, allowClaim) {
        _.templateSettings.variable = "rebate";

        if (allowClaim) {

            var strtDate = sessionStorage.getItem("startdate").split("|");
            var sDate = strtDate[0].replace("/", "-").replace("/", "-");
            var claim = { startdate: sDate, code: productCode, amount : amount };

            send("/rebates/claim", "POST", claim, function () { GPInt.prototype.ShowSplash(false); }, function (response) {
                if (response && _.isEqual(response.ResponseCode, 1)) {

                    GPInt.prototype.HideSplash();

                    var d = {
                        msg: response.ResponseMessage,
                        congrats: sessionStorage.getItem("congrats"),
                        statusCode: response.ResponseCode
                    };
                    $.get('/_Static/templates/rebates/claimMessage.html', function (data) {
                        var template = _.template(data);

                        $("#modalContent").html(template({
                            data: d
                        })).enhanceWithin();

                    }, 'html');

                    $('#rebatesModal').popup('open');
                    GPInt.prototype.HideSplash();

                } else {
                    w88Mobile.Growl.shout(response.ResponseMessage, function () {
                        window.location.replace("/Profile");
                    });
                }
            }, "");

        }
    }

    function getWeeklySettings(member) {
        send("/rebates/settings", "GET", "", function () { GPInt.prototype.ShowSplash(false); }, function (response) {
            if (response && _.isEqual(response.ResponseCode, 1)) {

                var d = {
                    Products: response.ResponseData,
                    btnSubmit: sessionStorage.getItem("btnSubmit"),
                    optionLabel: sessionStorage.getItem("promoOption"),
                    username: member
                };

                showWeeklyClaim(d);
            }
        }, "");
    }

    function showWeeklyClaim(d) {
        _.templateSettings.variable = "promo";

        $.get('/_Static/Promotions/templates/v2.html', function (data) {
            var template = _.template(data);

            $("#modalContent").html(template({
                data: d
            })).enhanceWithin();

        }, 'html');

        $('#rebatesModal').popup('open');
        GPInt.prototype.HideSplash();
    }

    function submitWeeklyClaim() {

        var msg;
        var selectedValue = [];
        $('#productCheckbox input:checked').each(function () {
            selectedValue.push($(this).attr('value'));
        });
       
        _.each(selectedValue, function (item) {

            $.ajax({
                type: 'POST',
                url: '/AjaxHandlers/RegisterPromo.ashx',
                data: { sCode: item.split("|")[0], Comment: item.split("|")[1] },
                success: function (data) {
                    switch (parseInt(data)) {
                        case 1: // success
                            msg = sessionStorage.getItem("weeklySuccess");
                            break;
                        case 10: // multiple submit
                            msg = sessionStorage.getItem("weeklyMultiple");
                            break;

                        default: // error
                            msg = sessionStorage.getItem("weeklyError");
                            break;
                    }
                    
                    $(".div-claim-promo-data").html("<p>" + msg + "</p>");
                }
            });
        });

    }


    return Rebates;
}

window.w88Mobile.Rebates = Rebates();