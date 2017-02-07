function historyReport() {
    var history = {
        init: init,
        toggleType: toggleType,
        send: send,
        getReport: getReport,
        getHistoryReport: getHistoryReport
    }

    return history;

    var reportType = null, paymentType = null, status = null, adjType = null, ftStatus = null, wallets = null;
    var defaultSelect = null, defaultAll = null;
    var _w88_templates = null;

    function init() {
        setTranslations();

        getHistorySelection();

        hideSelection();

        _w88_templates = new window.w88Mobile.Templates();
        _w88_templates.init();
    }

    function setTranslations() {
        if (_w88_contents.translate("LABEL_FIELDS_STARTDATE") != "LABEL_FIELDS_STARTDATE") {
            defaultSelect = _w88_contents.translate("LABEL_SELECT_DEFAULT")
            defaultAll = _w88_contents.translate("LABEL_ALL_DEFAULT")

            $('#headerTitle').text(_w88_contents.translate("LABEL_FUNDS_HISTORY"));
            $('input[id$="btnSubmit"]').val(_w88_contents.translate("BUTTON_SUBMIT")).button("refresh");;

            $('label[id$="lblTransactionType"]').text(_w88_contents.translate("LABEL_TRANSACTION"));
            $('label[id$="lblDateFrom"]').text(_w88_contents.translate("LABEL_FIELDS_STARTDATE"));
            $('label[id$="lblDateTo"]').text(_w88_contents.translate("LABEL_FIELDS_ENDDATE"));
            $('label[id$="lblType"]').text(_w88_contents.translate("LABEL_FIELDS_TYPE"));
            $('label[id$="lblStatus"]').text(_w88_contents.translate("LABEL_FIELDS_STATUS"));
        } else {
            window.setInterval(function () {
                setTranslations();
            }, 500);
        }
    }

    function getReport(currentReport) {
        var data = null;

        switch (currentReport) {

            case "history-Adjustment":

                if (_.isEmpty($('#history-Adjustment .table-container')) && _.isEmpty($('#history-Adjustment .notFound').html()))
                    data = setParams("adjustment");

                break;

            case "history-DepositWidraw":

                if (_.isEmpty($('#history-DepositWidraw .table-container')) && _.isEmpty($('#history-DepositWidraw .notFound').html()))
                    data = setParams("depositwidraw");

                break;

            case "history-FundTransfer":

                if (_.isEmpty($('#history-FundTransfer .table-container')) && _.isEmpty($('#history-FundTransfer .notFound').html())) {
                    data = setParams("fundtransfer");
                    data.Status = "-1";
                    data.Type = "-1";
                }

                break;

            case "history-RefferalBonus":

                if (_.isEmpty($('#history-RefferalBonus .table-container')) && _.isEmpty($('#history-RefferalBonus .notFound').html()))
                    data = setParams("refferalbonus");

                break;

            case "history-PromoClaim":

                if (_.isEmpty($('#history-PromoClaim .table-container')) && _.isEmpty($('#history-PromoClaim .notFound').html()))
                    data = setParams("promoclaim");

                break;

            default:
        }

        getHistoryReport(data);
    }

    function filterResult(currentReport, data) {

        switch (currentReport) {

            case "Adjustment":

                setTemplate(_w88_templates.History_Adjustment, data, currentReport);

                break;

            case "DepositWidraw":

                setTemplate(_w88_templates.History_DepositWithdraw, data, currentReport);

                break;

            case "FundTransfer":
                setTemplate(_w88_templates.History_FundTransfer, data, currentReport)

                break;

            case "RefferalBonus":
                setTemplate(_w88_templates.History_RefferalBonus, data, currentReport)

                break;

            case "PromoClaim":
                setTemplate(_w88_templates.History_PromoClaim, data, currentReport)

                break;

            default:
        }
    }

    function notFound(type, message) {
        $('#history-' + type + ' .notFound').empty();
        $('#history-' + type + ' .notFound').append(message);
        $('#history-' + type + ' .notFound').show();
    }

    function setParams(type) {
        var today = new Date();
        var dateFrom = today.setDate(today.getDate() - 10);
        var dateTo = new Date();

        var reports = _.find(reportType, function (item) {
            return item.Value.toLowerCase() == type;
        });

        var data = {
            DateFrom: new Date(dateFrom).toLocaleDateString(),
            DateTo: dateTo.toLocaleDateString(),
            ReportType: reports.Value,
            Status: "All",
            Type: "0",
        }

        return data;
    }

    function setTemplate(template, result, type) {
        var content = _.template(template);
        var innerHtml = content({
            result: result
        });

        $('#history-' + type).append(innerHtml);

        $('#history-' + type + ' .notFound').hide();

        if ($('#totalInvitees').length > 0) {
            if (_w88_contents.translate("LABEL_TOTAL_INVITEES") != "LABEL_TOTAL_INVITEES") {
                $('#totalInvitees').text(_w88_contents.translate("LABEL_TOTAL_INVITEES"));
                $('#totalRegistered').text(_w88_contents.translate("LABEL_TOTAL_REGISTERED"));
                $('#totalSuccess').text(_w88_contents.translate("LABEL_TOTAL_SUCCESS_REFFERAL"));
                $('#totalBonus').text(_w88_contents.translate("LABEL_TOTAL_REFFERAL_BONUS").replace('[cur]', Cookies().getCookie('currencyCode')));
            }
        }
    }

    function getHistoryReport(data) {
        if (data) {
            send(data, "/payments/history", "POST", function (response) {
                switch (response.ResponseCode) {
                    case 1:
                        filterResult(data.ReportType, response.ResponseData);

                        break;
                    default:

                        var message = data.DateFrom + "-" + data.DateTo + ": " + response.ResponseMessage;

                        notFound(data.ReportType, message);

                        break;
                }
            },
            function () {
                GPInt.prototype.HideSplash();
            });
        }
    }

    function getHistorySelection() {
        var _self = this;

        send("", "/history", "GET", function (response) {
            reportType = response.ResponseData.ReportType;
            paymentType = response.ResponseData.PaymentType;
            adjType = response.ResponseData.AdjustmentType;
            status = response.ResponseData.Status;
            ftStatus = response.ResponseData.FT_Status

            setTransactionSelection();

            var content = _.template(_w88_templates.History_Report_Title);
            var innerHtml = content({
                reportType: reportType
            });

            $(".history-result").append(innerHtml);

            $('.history-result').slick({
                autoplay: false,
                arrows: false,
                infinite: false,
                asNavFor: '.history-nav',
                focusOnSelect: true,
            });


            $( "#adj-btn" ).bind( "click", function() {
                 $(".history-result").slick('slickGoTo', 0, false );
            });
           
            $( "#dep-btn" ).bind( "click", function() {
                 $(".history-result").slick('slickGoTo', 1, false );
            });

            $( "#fund-btn" ).bind( "click", function() {
                 $(".history-result").slick('slickGoTo', 2, false );
            });
           
            $( "#ref-btn" ).bind( "click", function() {
                 $(".history-result").slick('slickGoTo', 3, false );
            });
           
            $( "#promo-btn" ).bind( "click", function() {
                 $(".history-result").slick('slickGoTo', 4, false );
            });


            $('.history-nav').slick({
              initialSlide: 0,
              slidesToShow: 5,
              slidesToScroll: 1,
              dots: false,
              arrows: false,
              infinite: false,
              draggable: false,
              swipeToSlide: false,
              responsive: [
                  {
                    breakpoint: 600,
                    settings: {
                        slidesToShow: 3,
                        arrows: true,
                    }
                  },
                  {
                    breakpoint: 517,
                    settings: {
                        slidesToShow: 2,
                        arrows: true,
                    }
                  },
              ]
            });

            $('.history-result').on('beforeChange', function(event, slick, nextSlide){
                $(".history-nav").find("span").removeClass("initial");
                $(".history-nav").find("span").removeClass("current");
            });

            $('.history-result').on('afterChange', function(event, slick, nextSlide){
                $(".history-nav").find("span").eq(nextSlide).addClass("current");
            });

            _.forEach(reportType, function (item) {
                getReport("history-" + item.Value);
            });

            $('#filterHistory').show();
        },
        function () {
            GPInt.prototype.HideSplash();
        })

        send({ isSelection: true }, "/user/Wallets", "GET", function (response) {
            wallets = response.ResponseData;
        },
        function () {
            GPInt.prototype.HideSplash();
        })


    }

    function toggleType(type) {

        switch (type.toLowerCase()) {

            case "adjustment":

                setTypeSelection(adjType, "0");
                setStatusSelection(status, "All");

                break;

            case "depositwidraw":

                setTypeSelection(paymentType, "0");
                setStatusSelection(status, "All");

                break;

            case "fundtransfer":

                setWalletSelection();
                setStatusSelection(ftStatus, "-1");

                break;
            case "refferalbonus":

                hideSelection();

                break;
            case "promoclaim":

                hideSelection();

                break;

            default:

        }
    }

    function hideSelection() {
        $('li[id$="type"]').hide();
        $('li[id$="status"]').hide();
    };

    function setTypeSelection(selection, defaultValue) {
        $('li[id$="type"]').show();

        $('select[id$="ddlType"]').empty();

        _.forEach(selection, function (data) {
            $('select[id$="ddlType"]').append($("<option></option>").attr("value", data.Value).text(data.Text))
        });

        $('select[id$="ddlType"]').val(defaultValue).selectmenu("refresh");
    }

    function setStatusSelection(selection, defaultValue) {
        $('li[id$="status"]').show();

        $('select[id$="ddlStatus"]').empty();

        _.forEach(selection, function (data) {
            $('select[id$="ddlStatus"]').append($("<option></option>").attr("value", data.Value).text(data.Text))
        })

        $('select[id$="ddlStatus"]').val(defaultValue).selectmenu("refresh");
    }

    function setWalletSelection() {
        $('li[id$="type"]').show();

        $('select[id$="ddlType"]').empty();

        $('select[id$="ddlType"]').append($("<option></option>").attr("value", "-1").text(defaultAll))

        _.forEach(wallets, function (data) {
            $('select[id$="ddlType"]').append($("<option></option>").attr("value", data.Id).text(data.Name))
        });

        $('select[id$="ddlType"]').val("-1").selectmenu("refresh");
    }

    function setTransactionSelection() {
        $('select[id$="ddlTransactionType"]').append($("<option></option>").attr("value", "-1").text(defaultSelect))

        _.forEach(reportType, function (data) {
            $('select[id$="ddlTransactionType"]').append($("<option></option>").attr("value", data.Value).text(data.Text))
        })

        $('select[id$="ddlTransactionType"]').val("-1").selectmenu("refresh");
    }

    function send(data, resource, method, success, complete) {
        var _self = this;

        var url = w88Mobile.APIUrl + resource;

        var headers = {
            'Token': window.User.token,
            'LanguageCode': window.User.lang
        };

        $.ajax({
            type: method,
            url: url,
            data: data,
            beforeSend: function () {
                GPInt.prototype.ShowSplash(true);
            },
            headers: headers,
            success: success,
            error: function () {
                console.log("Error connecting to api");
            },
            complete: complete
        });
    };
}

window.w88Mobile.History = historyReport;