window.w88Mobile.Rebates = Rebates();
var _w88_Rebates = window.w88Mobile.Rebates;

function Rebates() {

    var rebates = {};

    rebates.init = function () {

        $('.header-title').first().text($.i18n("LABEL_MENU_REBATES"));
        _w88_Rebates.Weeks();
    };

    rebates.Weeks = function () {

        _w88_send("", "/rebates/week", "GET", function (response) {
            if (_.isEqual(response.ResponseCode, 1)) {

                _.forOwn(response.ResponseData, function (data) {
                    $('#weeks').append($('<option>').text(data.Text).attr('value', data.Value));
                });

                $("#weeks").val($("#weeks option:first").val()).change();
                amplify.store("rebates_startdate_translations", $("#weeks").val(), window.User.storageExpiration);
            }
        });
    };

    rebates.Statement = function () {

        amplify.store("rebates_startdate_translations", $("#weeks").val(), window.User.storageExpiration);
        var strtDate = amplify.store("rebates_startdate_translations").split("|");
        var sDate = { startdate: strtDate[0].replace("/", "-").replace("/", "-") };

        $("#startdate").html(strtDate[0]);
        $("#endDate").html(strtDate[1]);

        _w88_send(sDate, "/rebates/result", "GET", function (response) {
            if (_.isEqual(response.ResponseCode, 1)) {

                var groupTemplate = _.template(
                    $("script#ClaimGroup").html()
                );

                $("#group").html(groupTemplate({
                    data: response.ResponseData.RebateRow,
                    ShowWeekClaim: $("#weeks").val() == $("#weeks option:first").val(),
                }));
                $("#group").i18n();

                $('#rebateDisclaimerMin').html($.i18n("LABEL_REBATE_NOTE1") + " " + Cookies().getCookie("currencyCode") + " " + response.ResponseData.MinimumClaim);
                $("#weeklyBtn").html($.i18n("BUTTON_WEEKLY_CLAIM"));

                $(".collapsible-btn").click(function (e) {
                    e.preventDefault();

                    $(this).parent().find('.collapsible-table').slideToggle("fast");

                    if ($(this).hasClass('collapsed')) {
                        $(this).removeClass('collapsed');
                        $(this).find('span').html($.i18n("LABEL_MORE"));
                    }
                    else {
                        $(this).addClass('collapsed');
                        $(this).find('span').html($.i18n("LABEL_LESS"));
                    }
                });

                if ($("#weeks").val() == $("#weeks option:first").val()) {
                    $(".curr_week").show();
                    $(".prev_week").hide();
                }
                else {
                    $(".curr_week").hide();
                    $(".prev_week").show();
                }

            } else {
                window.w88Mobile.Growl.shout(response.ResponseMessage, function () {
                    window.location.replace("/Profile");
                });
            }
        });
    };

    rebates.ClaimQuery = function (productCode, allowClaim) {

        if (allowClaim) {

            var strtDate = amplify.store("rebates_startdate_translations").split("|");
            var sDate = strtDate[0].replace("/", "-").replace("/", "-");
            var query = { startdate: sDate, code: productCode };

            _w88_send(query, "/rebates/query", "GET", function (response) {
                if (_.isEqual(response.ResponseCode, 1)) {

                    var d = {
                        ProductCode: productCode,
                        EligibleBets: response.ResponseData.TotalEligibleBet,
                        rebatePercent: response.ResponseData.RebatePercent,
                        Amount: response.ResponseData.RebateAmount,
                        AllowClaim: response.ResponseData.AllowClaim,
                        ClaimedAmount: response.ResponseData.ClaimedAmount,
                        BalanceRebateAmount: response.ResponseData.BalanceRebateAmount,
                        CurrencyCode: response.ResponseData.CurrencyCode,
                        MinimumClaim: response.ResponseData.MinimumClaim,
                        Monday: $.i18n("LABEL_MONDAY"),
                        Sunday: $.i18n("LABEL_SUNDAY"),
                        StartDate: strtDate[0],
                        EndDate: strtDate[1],
                        note1: $.i18n("LABEL_REBATE_NOTE1"),
                        note2: $.i18n("LABEL_REBATE_NOTE2"),
                };

                    var claimTemplate = _.template(
                        $("script#ClaimModal").html()
                    );

                    $("#rebate-modal div.modal-body").html(claimTemplate({
                        data: d
                    }));

                    $('#rebateClaim').css('opacity', 1);
                    $('#rebateClaim').css('text-indent', 0);

                    $("#rebate-modal").i18n();
                    $('#rebate-modal').modal('show');
                } else {
                    window.w88Mobile.Growl.shout(response.ResponseMessage, function () {
                        window.location.replace("/Profile");
                    });
                }
            });
        }
    };

    rebates.ClaimNow = function (productCode, amount, allowClaim) {

        if (allowClaim) {

            var strtDate = amplify.store("rebates_startdate_translations").split("|");
            var sDate = strtDate[0].replace("/", "-").replace("/", "-");
            var claim = { startdate: sDate, code: productCode, amount: amount };

            _w88_send(claim, "/rebates/claim", "POST", function (response) {
                if (_.isEqual(response.ResponseCode, 1)) {

                    var d = {
                        msg: response.ResponseMessage,
                        statusCode: response.ResponseCode
                    };

                    var claimMessageTemplate = _.template(
                        $("script#ClaimMessage").html()
                    );

                    $("#rebate-modal div.modal-body").html(claimMessageTemplate({
                        data: d
                    }));

                    $("#rebate-modal").i18n();
                    $('#rebate-modal').modal('show');
                    _w88_Rebates.Statement();

                } else {
                    window.w88Mobile.Growl.shout(response.ResponseMessage, function () {
                        window.location.replace("/Profile");
                    });
                }
            });
        }
    };

    rebates.GetWeeklySettings = function (member) {

        _w88_send("", "/rebates/settings", "GET", function (response) {
            if (_.isEqual(response.ResponseCode, 1)) {

                var d = {
                    Products: response.ResponseData,
                    btnSubmit: $.i18n("BUTTON_SUBMIT"),
                    optionLabel: $.i18n("LABEL_PROMO_OPTION"),
                    username: member
                };

                _w88_Rebates.ShowWeeklyClaim(d);
            }
        });
    };

    rebates.ShowWeeklyClaim = function (d) {

        $.get('/_Static/v2/templates/promo/v2.html', function (data) {
            var template = _.template(data);

            $("#rebate-modal div.modal-body").html(template({
                data: d
            }));

            $('#rebate-modal').modal('show');

        }, 'html');
    };

    rebates.SubmitWeeklyClaim = function () {

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
                            msg = $.i18n("LABEL_REBATE_WEEKLYCLAIM_SUCCESS");
                            break;
                        case 10: // multiple submit
                            msg = $.i18n("LABEL_REBATE_WEEKLYCLAIM_MULTIPLE");
                            break;

                        default: // error
                            msg = $.i18n("LABEL_REBATE_WEEKLYCLAIM_DEFAULT");
                            break;
                    }

                    $(".div-claim-promo-data").html("<p>" + msg + "</p>");
                }
            });
        });

    };

    return rebates;
}