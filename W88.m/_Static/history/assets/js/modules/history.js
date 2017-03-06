function historyReport() {
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

    return history;

    var selection = {};
    var status = {}, results = {};
    var reportStatus = {};
    var defaultSelect = null, defaultAll = null;

    var _w88_templates = null;

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

        $('.history-modal').empty();
        $('.history-modal').append(innerHtml);
        $('.history-modal').show();

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
        $('.history-modal').empty();
    }

    function init() {

        status = {
            NotStarted: "notstarted", Started: "started", Done: "done"
        };

        reportStatus = {
            Adjustment: status.NotStarted,
            DepositWithdraw: status.NotStarted,
            FundTransfer: status.NotStarted,
            PromoClaim: status.NotStarted,
        };

        result = {
            Adjustment: {},
            DepositWithdraw: {},
            FundTransfer: {},
        };

        subscription();

        setTranslations();

        getSelection();

        hideSelection();

        _w88_templates = new window.w88Mobile.Templates();
        _w88_templates.init();
    }

    function setTranslations() {
        if (_w88_contents.translate("LABEL_STARTDATE") != "LABEL_STARTDATE") {
            defaultSelect = _w88_contents.translate("LABEL_SELECT_DEFAULT")
            defaultAll = _w88_contents.translate("LABEL_ALL_DEFAULT")

            $('#headerTitle').text(_w88_contents.translate("LABEL_FUNDS_HISTORY"));
            $('input[id$="btnSubmit"]').val(_w88_contents.translate("BUTTON_SUBMIT")).button("refresh");;

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
                filterResult(response.ResponseCode, {
                    Type: data.ReportType.toLowerCase(),
                    Result: response.ResponseData,
                    Message: response.ResponseMessage,
                    DateFrom: data.DateFrom,
                    DateTo: data.DateTo
                });
            });
        }
    }

    function filterResult(responseCode, data) {
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

        if (!_.isEqual(responseCode, 1)) {
            w88Mobile.Growl.shout(response.ResponseMessage);
        } else {
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

        $('.history-result').on('beforeChange', function (event, slick, nextSlide, currentSlide) {
            $(".history-nav").find("span").removeClass("initial");
            $(".history-nav").find("span").removeClass("current");
        });

        $('.history-result').on('afterChange', function (event, slick, nextSlide) {
            $(".history-nav").find("span").eq(nextSlide).addClass("current");
        });
    }

    function hideSelection() {
        $('li[id$="type"]').hide();
        $('li[id$="status"]').hide();
    };

    function onSetTransType(topic, data) {
        $('select[id$="ddlTransactionType"]').append($("<option></option>").attr("value", data.Default).text(defaultSelect))

        _.forEach(data.Type, function (item) {
            $('select[id$="ddlTransactionType"]').append($("<option></option>").attr("value", item.Value).text(item.Text))
            pubsub.publish("fetchHistory", item);
        })

        $('select[id$="ddlTransactionType"]').val(data.Default).selectmenu("refresh");
    }

    function onSetType(topic, data) {
        $('li[id$="type"]').show();

        $('select[id$="ddlType"]').empty();

        _.forEach(data.Type, function (item) {
            $('select[id$="ddlType"]').append($("<option></option>").attr("value", item.Value).text(item.Text))
        });

        $('select[id$="ddlType"]').val(data.Default).selectmenu("refresh");
    }

    function onSetStatus(topic, data) {
        $('li[id$="status"]').show();

        $('select[id$="ddlStatus"]').empty();

        _.forEach(data.Status, function (item) {
            $('select[id$="ddlStatus"]').append($("<option></option>").attr("value", item.Value).text(item.Text))
        })

        $('select[id$="ddlStatus"]').val(data.Default).selectmenu("refresh");
    }

    function onSetWallet(topic, data) {
        $('li[id$="type"]').show();

        $('select[id$="ddlType"]').empty();

        $('select[id$="ddlType"]').append($("<option></option>").attr("value", data.Default).text(defaultAll))

        _.forEach(data.Type, function (item) {
            $('select[id$="ddlType"]').append($("<option></option>").attr("value", item.Id).text(item.Name))
        });

        $('select[id$="ddlType"]').val(data.Default).selectmenu("refresh");
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
                GPInt.prototype.ShowSplash(true);
            },
            headers: headers,
            success: success,
            error: function (resp) {
                console.log("Error connecting to api");
            },
            complete: function () {
                if (!_.isUndefined(complete)) complete();
                GPInt.prototype.HideSplash();
            }
        });
    };
}

window.w88Mobile.History = historyReport;