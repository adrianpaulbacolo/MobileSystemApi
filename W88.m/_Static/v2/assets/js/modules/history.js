var _w88_history = w88Mobile.History = History();

function History() {
    var history = {
        init: init,
        toggleType: toggleType,
        send: send,
        getReport: getReport,
        getHistoryReport: getHistoryReport,
        setReportStatus: setReportStatus,
        showModal: showModal,
        closeModal: closeModal,
        minDate: new Date(),
        maxDate: addMonths(new Date(), -3),
        formatDateTime: formatDateTime
    }

    var selection = {};
    var defaultSelect = null, defaultAll = null;

    var status = {
        NotStarted: "notstarted", Started: "started", Done: "done"
    };

    var reportStatus = {
        Adjustment: status.NotStarted,
        DepositWithdraw: status.NotStarted,
        FundTransfer: status.NotStarted,
        PromoClaim: status.NotStarted,
    };

    var result = {
        Adjustment: {},
        DepositWithdraw: {},
        FundTransfer: {},
    };

    function subscription() {
        pubsub.subscribe('fetchHistory', onFetchHistoryByReportType);
        pubsub.subscribe('transType', onSetTransType);
        pubsub.subscribe('wallet', onSetWallet);
        pubsub.subscribe('type', onSetType);
        pubsub.subscribe('status', onSetStatus);

        pubsub.subscribe('adjStatus', onUpdateAdjustmentStatus);
        pubsub.subscribe('depStatus', onUpdateDepositWithdrawalStatus);
        pubsub.subscribe('fundStatus', onUpdateFundTransferStatus);
        pubsub.subscribe('promoStatus', onUpdatePromoClaimStatus);

        pubsub.subscribe('showModal', onShowModal);
    }

    function onUpdateAdjustmentStatus(topic, data) {
        reportStatus.Adjustment = data;
    }

    function onUpdateDepositWithdrawalStatus(topic, data) {
        reportStatus.DepositWithdraw = data;
    }

    function onUpdateFundTransferStatus(topic, data) {
        reportStatus.FundTransfer = data;
    }

    function onUpdatePromoClaimStatus(topic, data) {
        reportStatus.PromoClaim = data;
    }

    function onShowModal(topic, data) {
        var template = null, report = null;

        switch (data.Type) {

            case "adjustment":
                template = _w88_templates.History_AdjustmentModal;

                report = _.find(result.Adjustment, function (item) {
                    return item.TransactionId == data.Id;
                });

                break;

            case "depositwidraw":
                template = _w88_templates.History_DepositWithdrawModal;

                report = _.find(result.DepositWithdraw, function (item) {
                    return item.TransactionId == data.Id;
                });

                break;

            case "fundtransfer":
                template = _w88_templates.History_FundTransferModal;

                report = _.find(result.FundTransfer, function (item) {
                    return item.TransactionId == data.Id;
                });

                break;

            default:
        }

        var content = _.template(template);
        var innerHtml = content({
            result: report,
        });

        $('.history-full').empty();
        $('.history-full').append(innerHtml);
        $('.history-full').show();

        $('#transId-lbl').text(_w88_contents.translate("LABEL_TRANSACTION_ID"));
        $('#status-lbl').text(_w88_contents.translate("LABEL_STATUS"));
        $('#method-lbl').text(_w88_contents.translate("LABEL_PAYMENT_METHOD"));

        $('#cat-lbl').text(_w88_contents.translate("LABEL_CATEGORY"));
        $('#prod-lbl').text(_w88_contents.translate("LABEL_PRODUCT"));
        $('#msg-lbl').text(_w88_contents.translate("LABEL_MESSAGE"));
        $('#source-lbl').text(_w88_contents.translate("LABEL_SOURCE"));

        $('#subAmt-lbl').text(_w88_contents.translate("LABEL_SUBMITTED_AMOUNT"));
        $('#recAmt-lbl').text(_w88_contents.translate("LABEL_RECEIVED_AMOUNT"));
        $('#amt-lbl').text(_w88_contents.translate("LABEL_AMOUNT"));

        $('#dt-lbl').text(_w88_contents.translate("LABEL_DATE_TIME"));
        $('#from-lbl').text(_w88_contents.translate("LABEL_FROM"));
        $('#to-lbl').text(_w88_contents.translate("LABEL_TO"));
    }

    function addMonths(date, months) {
        date.setMonth(date.getMonth() + months);
        return date;
    }

    function formatDateTime(dateTime) {
        //MM/DD/YYYY h:m:s
        var month = (dateTime.getMonth() + 1).toString().length == 1 ? "0" + (dateTime.getMonth() + 1).toString() : (dateTime.getMonth() + 1).toString();
        var day = (dateTime.getDate()).toString().length == 1 ? "0" + dateTime.getDate().toString() : dateTime.getDate().toString();
        var year = dateTime.getFullYear();

        var hours = dateTime.getHours();
        var minutes = dateTime.getMinutes();
        var seconds = dateTime.getSeconds();

        return month + "/" + day + "/" + year + " " + hours + ":" + minutes + ":" + seconds
    }

    function toggleType(type) {

        switch (type.toLowerCase()) {

            case "adjustment":
                pubsub.publish("type", {
                    Type: selection.AdjustmentType, Default: "0"
                });
                pubsub.publish("status", {
                    Status: selection.Status, Default: "All"
                });

                break;

            case "depositwidraw":
                pubsub.publish("type", {
                    Type: selection.PaymentType, Default: "0"
                });
                pubsub.publish("status", {
                    Status: selection.Status, Default: "All"
                });

                break;

            case "fundtransfer":
                pubsub.publish("wallet", {
                    Type: selection.Wallets, Default: "-1"
                });
                pubsub.publish("status", {
                    Status: selection.FT_Status, Default: "-1"
                });

                break;

            case "promoclaim":

                hideSelection();

                break;

            default:

        }
    }

    function setReportStatus(currentReport) {
        switch (currentReport.toLowerCase()) {
            case "adjustment":
                if (_.isEqual(reportStatus.Adjustment, status.Done))
                    pubsub.publish("adjStatus", status.NotStarted);

                break;

            case "depositwidraw":
                if (_.isEqual(reportStatus.DepositWithdraw, status.Done))
                    pubsub.publish("depStatus", status.NotStarted);

                break;

            case "fundtransfer":

                if (_.isEqual(reportStatus.FundTransfer, status.Done))
                    pubsub.publish("fundStatus", status.NotStarted);

                break;

            case "promoclaim":
                if (_.isEqual(reportStatus.PromoClaim, status.Done))
                    pubsub.publish("promoStatus", status.NotStarted);

                break;
        }
    }

    function showModal(type, id) {
        pubsub.publish("showModal", {
            Type: type,
            Id: id
        });
    }

    function closeModal() {
        $('.history-full').empty();
        $('.history-full').hide();
    }

    function init() {
        subscription();

        setTranslations();

        getSelection();

        hideSelection();
    }

    function setTranslations() {
        if (_w88_contents.translate("LABEL_STARTDATE") != "LABEL_STARTDATE") {
            defaultSelect = _w88_contents.translate("LABEL_SELECT_DEFAULT")
            defaultAll = _w88_contents.translate("LABEL_ALL_DEFAULT")

            $("header .header-title").text(_w88_contents.translate("LABEL_FUNDS_HISTORY"));
            $('button[id$="btnSubmit"]').text(_w88_contents.translate("BUTTON_SUBMIT"));

            $('label[id$="lblTransactionType"]').text(_w88_contents.translate("LABEL_TRANSACTION"));
            $('label[id$="lblDateFrom"]').text(_w88_contents.translate("LABEL_STARTDATE"));
            $('label[id$="lblDateTo"]').text(_w88_contents.translate("LABEL_ENDDATE"));
            $('label[id$="lblType"]').text(_w88_contents.translate("LABEL_TYPE"));
            $('label[id$="lblStatus"]').text(_w88_contents.translate("LABEL_STATUS"));

            $('#adj-lbl').text(_w88_contents.translate("LABEL_HISTORY_ADJUSTMENT"));
            $('#dep-lbl').text(_w88_contents.translate("LABEL_HISTORY_DEPOSITWIDRAW"));
            $('#fund-lbl').text(_w88_contents.translate("LABEL_HISTORY_FUND_TRANSFER"));
            $('#promo-lbl').text(_w88_contents.translate("LABEL_HISTORY_PROMO_CLAIM"));

        } else {
            window.setInterval(function () {
                setTranslations();
            }, 500);
        }
    }

    function getSelection() {
        selection = {
            ReportType: null,
            PaymentType: null,
            AdjustmentType: null,
            Status: null,
            FT_Status: null,
            Wallets: null
        };

        send("", "/history", "GET", function (response) {
            selection.ReportType = response.ResponseData.ReportType;
            selection.PaymentType = response.ResponseData.PaymentType;
            selection.AdjustmentType = response.ResponseData.AdjustmentType;
            selection.Status = response.ResponseData.Status;
            selection.FT_Status = response.ResponseData.FT_Status

            bindSlick(selection.ReportType);

            pubsub.publish("transType", {
                Type: selection.ReportType, Default: "-1"
            });

            pubsub.publish("fetchHistory", {
                Type: selection.ReportType, Default: "-1"
            });

            $('#filterHistory').show();
        });

        send({
            isSelection: true
        }, "/user/Wallets", "GET", function (response) {
            selection.Wallets = response.ResponseData;
        })
    }

    function onFetchHistoryByReportType(topic, data) {
        _.forEach(data.Type, function (item) {
            getReport(item.Value);
        });
    }

    function getReport(currentReport, data) {
        currentReport = currentReport.toLowerCase();

        if (getReportStatus(currentReport)) {
            getHistoryReport(currentReport, data);
        }
    }

    function getReportStatus(currentReport) {
        switch (currentReport) {

            case "adjustment":

                if (!_.isEqual(reportStatus.Adjustment, status.NotStarted) && !_.isEqual(reportStatus.Adjustment, status.Done))
                    return false;

                pubsub.publish("adjStatus", status.Started);

                break;

            case "depositwidraw":

                if (!_.isEqual(reportStatus.DepositWithdraw, status.NotStarted) && !_.isEqual(reportStatus.DepositWithdraw, status.Done))
                    return false;

                pubsub.publish("depStatus", status.Started);

                break;

            case "fundtransfer":

                if (!_.isEqual(reportStatus.FundTransfer, status.NotStarted) && !_.isEqual(reportStatus.FundTransfer, status.Done))
                    return false;

                pubsub.publish("fundStatus", status.Started);

                break;

            case "promoclaim":

                if (!_.isEqual(reportStatus.PromoClaim, status.NotStarted) && !_.isEqual(reportStatus.PromoClaim, status.Done))
                    return false;

                pubsub.publish("promoStatus", status.Started);

                break;

            default:
        }

        return true;
    }

    function setParams(type) {
        var today = new Date();
        var dateFrom = today.setDate(today.getDate() - 10);
        var dateTo = new Date();

        var reports = _.find(selection.ReportType, function (item) {
            return item.Value.toLowerCase() == type;
        });

        var data = {
            DateFrom: formatDateTime(new Date(dateFrom)),
            DateTo: formatDateTime(dateTo),
            ReportType: reports.Value,
            Status: "All",
            Type: "0",
        }

        if (_.isEqual(reports.Value.toLowerCase(), "fundtransfer")) {
            data.Status = "-1";
            data.Type = "-1";
        }

        return data;
    }

    function getHistoryReport(currentReport, data) {
        if (_.isUndefined(data))
            data = setParams(currentReport);
        else {
            var selectedIndex = $('select[id$="ddlTransactionType"] option:selected').index() - 1;
            if (selectedIndex >= 0)
                $(".history-result").slick('slickGoTo', selectedIndex, false);
        }

        if (data) {
            send(data, "/payments/history", "POST", function (response) {
                switch (response.ResponseCode) {
                    case 1:
                        filterResult({
                            Type: data.ReportType.toLowerCase(),
                            Result: response.ResponseData,
                            Message: response.ResponseMessage,
                            DateFrom: data.DateFrom,
                            DateTo: data.DateTo
                        });

                        break;

                    default:
                        w88Mobile.Growl.shout(response.ResponseMessage);

                        break;
                }

            });
        }
    }

    function filterResult(data) {
        var template = null;

        switch (data.Type) {

            case "adjustment":
                template = _w88_templates.History_Adjustment;

                result.Adjustment = data.Result;

                pubsub.publish("adjStatus", status.Done);

                break;

            case "depositwidraw":
                template = _w88_templates.History_DepositWithdraw;

                result.DepositWithdraw = data.Result;

                pubsub.publish("depStatus", status.Done);

                break;

            case "fundtransfer":
                template = _w88_templates.History_FundTransfer;

                result.FundTransfer = data.Result;

                pubsub.publish("fundStatus", status.Done);

                break;

            case "promoclaim":
                template = _w88_templates.History_PromoClaim;

                pubsub.publish("promoStatus", status.Done);

                break;

            default:
        }

        var content = _.template(template);
        var innerHtml = content({
            type: data.Type,
            result: data.Result,
            message: data.Message,
            dateFrom: data.DateFrom,
            dateTo: data.DateTo
        });

        $('#' + data.Type + ' .history-data').empty();
        $('#' + data.Type).append(innerHtml);

        $('.to-lbl').text(_w88_contents.translate("LABEL_TO"));
    }

    function bindSlick(reportType) {
        $('.history-result').slick({
            autoplay: false,
            arrows: false,
            infinite: false,
            asNavFor: '.history-nav',
            focusOnSelect: true,
        });

        $("#adj-btn").bind("click", function () {
            $(".history-result").slick('slickGoTo', 0, false);
        });

        $("#dep-btn").bind("click", function () {
            $(".history-result").slick('slickGoTo', 1, false);
        });

        $("#fund-btn").bind("click", function () {
            $(".history-result").slick('slickGoTo', 2, false);
        });

        $("#promo-btn").bind("click", function () {
            $(".history-result").slick('slickGoTo', 3, false);
        });

        $('.history-nav').slick({
            initialSlide: 0,
            slidesToShow: 4,
            slidesToScroll: 1,
            dots: false,
            arrows: false,
            infinite: false,
            draggable: false,
            zIndex: '5',
            swipeToSlide: false,
            prevArrow: '<button type="button" class="slick-prev"><span class="icon icon-arrow-left"></span></button>',
            nextArrow: '<button type="button" class="slick-next"><span class="icon icon-arrow-right"></span></button>',
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
                {
                    breakpoint: 321,
                    settings: {
                        slidesToShow: 1,
                        arrows: true,
                    }
                },
            ]
        });

        $('.history-result').on('beforeChange', function (event, slick, nextSlide, currentSlide) {
            $(".history-nav .slick-slide").find("span").removeClass("initial");
            $(".history-nav .slick-slide").find("span").removeClass("current");
        });

        $('.history-result').on('afterChange', function (event, slick, nextSlide) {
            $(".history-nav .slick-slide").find("span").eq(nextSlide).addClass("current");
        });
    }

    function hideSelection() {
        $('div[id$="type"]').hide();
        $('div[id$="status"]').hide();
    };

    function onSetTransType(topic, data) {
        $('select[id$="ddlTransactionType"]').append($("<option></option>").val(data.Default).text(defaultSelect))

        _.each(data.Type, function (item, index) {
            $('select[id$="ddlTransactionType"]').append($("<option></option>").val(item.Value).text(item.Text))
            pubsub.publish("fetchHistory", item);
        })
    }

    function onSetType(topic, data) {
        $('div[id$="type"]').show();

        $('select[id$="ddlType"]').empty();

        _.each(data.Type, function (item, index) {
            $('select[id$="ddlType"]').append($("<option></option>").val(item.Value).text(item.Text))
        });
    }

    function onSetStatus(topic, data) {
        $('div[id$="status"]').show();

        $('select[id$="ddlStatus"]').empty();

        _.each(data.Status, function (item, index) {
            $('select[id$="ddlStatus"]').append($("<option></option>").val(item.Value).text(item.Text))
        })
    }

    function onSetWallet(topic, data) {
        $('div[id$="type"]').show();

        $('select[id$="ddlType"]').empty();

        $('select[id$="ddlType"]').append($("<option></option>").val(data.Default).text(defaultAll))

        _.each(data.Type, function (item, index) {
            $('select[id$="ddlType"]').append($("<option></option>").val(item.Id).text(item.Name))
        });
    }

    function send(data, resource, method, success, complete) {
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
                pubsub.publish('startLoadItem', { selector: "" });
            },
            headers: headers,
            success: success,
            error: function (resp) {
                console.log("Error connecting to api");
            },
            complete: function () {
                if (!_.isUndefined(complete)) complete();
                pubsub.publish('stopLoadItem', { selector: "" });
            }
        });
    };

    return history;

}