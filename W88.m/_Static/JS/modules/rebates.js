function Rebates() {

    var Rebates = {
        Initialize: initialize,
        Statement: statement,
        ClaimQuery: claimQuery,
        ClaimNow: claimNow
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

                sessionStorage.setItem("monday", response.ResponseData.LABEL_MONDAY);
                sessionStorage.setItem("sunday", response.ResponseData.LABEL_SUNDAY);
                sessionStorage.setItem("summary", response.ResponseData.LABEL_REBATE_SUMMARY);
                sessionStorage.setItem("rebateNote1", response.ResponseData.LABEL_REBATE_NOTE1);
                sessionStorage.setItem("rebateNote2", response.ResponseData.LABEL_REBATE_NOTE2);
                sessionStorage.setItem("rebateAmount", response.ResponseData.LABEL_REBATE_AMOUNT);
                sessionStorage.setItem("rebateBets", response.ResponseData.LABEL_REBATE_BETS);
                sessionStorage.setItem("rebatePercent", response.ResponseData.LABEL_REBATE_PERCENT);
                sessionStorage.setItem("congrats", response.ResponseData.LABEL_CONGRATS);
            }
        }, "");
    }

    function weeks() {
        send("/rebates/week", "GET", "", "", function (response) {
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

        send("/rebates/result", "GET", sDate, function () { GPInt.prototype.ShowSplash(); }, function (response) {
            if (response && _.isEqual(response.ResponseCode, 1)) {

                $.get('/_Static/templates/rebates/ClaimGroups.html', function(data) {
                    var template = _.template(data);

                    $("#group").html(template({
                        data: response.ResponseData,
                        LabelRebateAmount: sessionStorage.getItem("rebateAmount"),
                        LabelRebatePercent: sessionStorage.getItem("rebatePercent"),
                        LabelRebateBets: sessionStorage.getItem("rebateBets")
                    })).enhanceWithin();

                }, 'html');

            } else {
                w88Mobile.Growl.shout(response.ResponseMessage, function() {
                    window.location.replace("/Profile");
                });
            }
        }, "");
    }

    function claimQuery(productCode, allowClaim, bets, percent, amount) {

        if (allowClaim) {

            var strtDate = sessionStorage.getItem("startdate").split("|");
            var sDate = strtDate[0].replace("/", "-").replace("/", "-");
            var query = { startdate: sDate, code: productCode };

            send("/rebates/query", "GET", query, function () { GPInt.prototype.ShowSplash(); }, function (response) {
                if (response && _.isEqual(response.ResponseCode, 1)) {

                    var d = {
                        ProductCode: productCode,
                        EligibleBets: bets,
                        rebatePercent: percent,
                        Amount: amount,
                        AllowClaim: allowClaim,
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
                };
                    $.get('/_Static/templates/rebates/claimModal.html', function(data) {
                        var template = _.template(data);

                        $("#modalContent").parent().html(template({
                            data: d
                        })).enhanceWithin();

                    }, 'html');

                    $('#rebatesModal').popup('open');

                } else {
                    w88Mobile.Growl.shout(response.ResponseMessage, function () {
                        window.location.replace("/Profile");
                    });
                }
            }, "");
        }
    }

    function claimNow(productCode, amount, allowClaim) {
        
        if (allowClaim) {

            var strtDate = sessionStorage.getItem("startdate").split("|");
            var sDate = strtDate[0].replace("/", "-").replace("/", "-");
            var claim = { startdate: sDate, code: productCode, amount : amount };

            send("/rebates/claim", "POST", claim, function () { GPInt.prototype.ShowSplash(); }, function (response) {
                if (response && _.isEqual(response.ResponseCode, 1)) {

                    var d = {
                        msg: response.ResponseMessage,
                        congrats: sessionStorage.getItem("congrats")
                    };
                    $.get('/_Static/templates/rebates/claimMessage.html', function (data) {
                        var template = _.template(data);

                        $("#modalContent").parent().html(template({
                            data: d
                        })).enhanceWithin();

                    }, 'html');

                    $('#rebatesModal').popup('open');

                } else {
                    w88Mobile.Growl.shout(response.ResponseMessage, function () {
                        window.location.replace("/Profile");
                    });
                }
            }, "");

        }
    }


    return Rebates;
}

window.w88Mobile.Rebates = Rebates();