var _w88_paymentSvcV2 = window.w88Mobile.Gateways.DefaultPaymentsV2 = DefaultPaymentsV2();

function DefaultPaymentsV2() {

    var autorouteIds = {
        QuickOnline: "999999",
        UnionPay: "999998",
        TopUpCard: "999997",
        AliPay: "999996",
        WeChat: "999995",
    };

    var defaultpayments = {
        AutoRouteIds: autorouteIds,
        Send: send,
        DisplaySettings: displaySettings,
        setPaymentTabs: setPaymentTabs,
        onTransactionCreated: onTransactionCreated,
        formatDateTime: formatDateTime,
        init: init,
        payRoute: "/v2/Deposit/Pay.aspx",
        CreateWithdraw: createWithdraw
    };

    var paymentCache = {};

    var paymentOptions = {};

    return defaultpayments;

    function init(isDeposit) {

        var type = isDeposit ? "deposit" : "withdrawal";

        fetchSettings(type, function () { });
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

    function displaySettings(methodId, options) {
        paymentOptions = options;
        initiateValidator(methodId);
        fetchSettings(paymentOptions.type, function () {
            if (!_.isEmpty(paymentCache)) {
                var setting = _.find(paymentCache.settings, function (data) {
                    return data.Id == methodId;
                });

                if (setting) {
                    $('#txtMode').text(": " + setting.PaymentMode);
                    $('#txtMinMaxLimit').text(": " + setting.MinAmount.toLocaleString(undefined, { minimumFractionDigits: 2 }) + " / " + setting.MaxAmount.toLocaleString(undefined, { minimumFractionDigits: 2 }));
                    $('#txtDailyLimit').text(": " + setting.LimitDaily);
                    $('#txtTotalAllowed').text(": " + setting.TotalAllowed);

                    $('#lblTotalAllowed').html(_w88_contents.translate("LABEL_TOTAL_ALLOWED"));
                    $('#lblDailyLimit').html(_w88_contents.translate("LABEL_DAILY_LIMIT"));
                    $('#lblMinMaxLimit').html(_w88_contents.translate("LABEL_MINMAX_LIMIT"));
                    $('#lblMode').html(_w88_contents.translate("LABEL_MODE"));
                }

                setTranslations(paymentOptions.type);
            }
        });
    }

    function setTranslations(paymentOptions) {
        if (_w88_contents.translate("LABEL_PAYMENT_NOTE") != "LABEL_PAYMENT_NOTE") {
            $('label[id$="lblAmount"]').text(_w88_contents.translate("LABEL_AMOUNT"));

            var headerTitle = paymentOptions == "Deposit" ? _w88_contents.translate("LABEL_FUNDS_DEPOSIT") : _w88_contents.translate("LABEL_FUNDS_WIDRAW");
            $("header .header-title").text(headerTitle);
            $('span[id$="lblMode"]').text(_w88_contents.translate("LABEL_MODE"));
            $('span[id$="lblMinMaxLimit"]').text(_w88_contents.translate("LABEL_MINMAX_LIMIT"));
            $('span[id$="lblDailyLimit"]').text(_w88_contents.translate("LABEL_DAILY_LIMIT"));
            $('span[id$="lblTotalAllowed"]').text(_w88_contents.translate("LABEL_TOTAL_ALLOWED"));

            $('#btnSubmitPlacement').text(_w88_contents.translate("BUTTON_SUBMIT"));

        } else {
            window.setInterval(function () {
                setTranslations(paymentOptions);
            }, 500);
        }
    }

    function setPaymentTabs(type, activeMethodId) {
        if (type.toLowerCase() == "deposit") {
            fetchSettings(type, function () {
                if (paymentCache.settings.length == 0) {
                    // track accounts with no gateways
                    w88Mobile.PiwikManager.trackEvent({
                        category: type,
                        action: window.User.countryCode,
                        name: window.User.memberId
                    });

                    nogateway();
                }
                else {
                    // payment cache variable is now present once callback is triggered
                    setDepositPaymentTab(paymentCache.settings, activeMethodId);
                }
            });
        } else {
            fetchSettings(type, function () {
                if (paymentCache.settings.length == 0) {
                    nogateway();
                }
                else {

                    send("/payments/withdrawal/pending", "GET", "", function (response) {
                        switch (response.ResponseCode) {
                            case 1:

                                var pendingWithdrawal = {
                                    Name: response.ResponseData.Name,
                                    TransactionId: response.ResponseData.TransactionId,
                                    MethodId: response.ResponseData.MethodId,
                                    Amount: response.ResponseData.Amount,
                                    RequestDateTime: response.ResponseData.RequestDateTime
                                };

                                var widrawKey = w88Mobile.Keys.withdrawalSettings + "-pending";
                                amplify.store(widrawKey, pendingWithdrawal, User.storageExpiration);

                                window.location = "/v2/Withdrawal/Pending.aspx";
                                break;

                            default:
                                setWithdrawalPaymentTab(paymentCache.settings, activeMethodId);
                                break;
                        }
                    });

                }
            });
        }
    }

    function fetchSettings(type, callback) {

        var url = "/payments/settings/" + type;
        cacheKey = (type.toLowerCase() == "deposit") ? w88Mobile.Keys.depositSettings : w88Mobile.Keys.withdrawalSettings;

        paymentCache = amplify.store(cacheKey);

        if (!_.isEmpty(paymentCache) && User.lang == paymentCache.language) {
            callback();
        } else {
            send(url, "GET", {},
                    function (response) {
                        switch (response.ResponseCode) {
                            case 1:
                                paymentCache = {
                                    settings: response.ResponseData
                                    , language: window.User.lang
                                };
                                amplify.store(cacheKey, paymentCache, User.storageExpiration);
                                callback();
                            default:
                                break;
                        }
                    }
                );
        }
    }

    function send(resource, method, data, success, complete) {
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
            error: function () {
                console.log("Error connecting to api");
            },
            complete: function () {
                if (!_.isUndefined(complete)) complete();
                pubsub.publish('stopLoadItem', { selector: "" });
            }
        });
    }

    function onTransactionCreated(form) {
        if (!_.isUndefined(form)) _.first(form).reset();
        w88Mobile.Growl.shout(_w88_contents.translate("MESSAGES_CHECK_HISTORY"));
    }

    function initiateValidator(methodId) {
        setNumericValidator();

        $('#form1').validator({
            custom: {
                bankequals: function ($el) {
                    $el.parent("div.form-group").removeClass('has-error');
                    $el.parent("div.form-group").children("span.help-block").remove();
                    var matchValue = $el.data("bankequals");
                    if ($el.val() == matchValue) {
                        $el.parent("div").append('<span class="help-block">' + _w88_contents.translate("Pay_MissingBank") + '</span>');
                        $el.parent("div.form-group").addClass('has-error');
                        return true;
                    }
                },
                selectequals: function ($el) {
                    $el.parent("div.form-group").removeClass('has-error');
                    $el.parent("div.form-group").children("span.help-block").remove();
                    var matchValue = $el.data("selectequals");
                    if ($el.val() == matchValue) {
                        $el.parent("div.form-group").addClass('has-error');
                        return true;
                    }
                },
                paylimit: function ($el) {
                    $el.parent("div.form-group").children("span.help-block").remove();
                    $el.parent(".form-group").removeClass('has-error');

                    if (!_.isEmpty($el)) {

                        var setting = _.find(paymentCache.settings, function (data) {
                            return data.Id == methodId;
                        });

                        if ($el.val() < setting.MinAmount) {
                            $el.parent("div").append('<span class="help-block">' + _w88_contents.translate("Pay_AmountMinLimit") + '</span>');
                            return true;
                        }
                        else if ($el.val() > setting.MaxAmount) {
                            $el.parent("div").append('<span class="help-block">' + _w88_contents.translate("Pay_AmountMaxLimit") + '</span>');
                            return true;
                        }
                    }
                },
                onedecimal: function ($el) {
                    $el.parent("div.form-group").children("span.help-block").remove();
                    $el.parent("div.form-group").removeClass('has-error');

                    if (!PositiveOneDecimalValidation($el.val(), $el)) {
                        $el.parent("div.form-group").addClass('has-error');
                        return true;
                    }
                },
                accountNo: function ($el) {
                    $el.parent("div.form-group").children("span.help-block").remove();
                    $el.parent("div.form-group").removeClass('has-error');

                    if (_.isEmpty($el.val())) {
                        $el.parent("div.form-group").addClass('has-error');
                        $el.parent("div").append('<span class="help-block">' + _w88_contents.translate("Pay_MissingAccountNumber") + '</span>');
                        return true;
                    }
                },
                accountName: function ($el) {
                    $el.parent("div.form-group").children("span.help-block").remove();
                    $el.parent("div.form-group").removeClass('has-error');

                    if (_.isEmpty($el.val())) {
                        $el.parent("div.form-group").addClass('has-error');
                        $el.parent("div").append('<span class="help-block">' + _w88_contents.translate("Pay_MissingAccountName") + '</span>');
                        return true;
                    }
                },
                address: function ($el) {
                    $el.parent("div.form-group").children("span.help-block").remove();
                    $el.parent("div.form-group").removeClass('has-error');

                    if (_.isEmpty($el.val())) {
                        $el.parent("div.form-group").addClass('has-error');
                        $el.parent("div").append('<span class="help-block">' + _w88_contents.translate("Pay_MissingAddress") + '</span>');
                        return true;
                    }
                }

            }
        });
    }

    function setNumericValidator() {
        _.forEach($('[data-numeric]'), function (item, index) {
            var numeric = item.getAttribute('data-numeric');

            if (_.isEmpty(numeric))
                $(item).autoNumeric('init');
            else
                $(item).autoNumeric('init', { mDec: numeric });
        });
    }

    function setDepositPaymentTab(responseData, activeTabId) {
        if (responseData.length > 0) {
            var routing = [
                autorouteIds.QuickOnline,
                autorouteIds.UnionPay,
                autorouteIds.TopUpCard,
                autorouteIds.AliPay,
                autorouteIds.WeChat
            ];

            var isAutoRoute = false, title = "", page = null, deposit = "/Deposit/";

            for (var i = 0; i < responseData.length; i++) {
                var data = responseData[i];

                page = setPaymentPage(data.Id);

                if (page)
                    page = deposit + page;
                else
                    continue;

                if (activeTabId) {
                    if (_.isEqual(data.Id, activeTabId))
                        title = data.Title;

                    var anchor = $('<a />', { class: 'list-group-item', id: data.Id, href: page }).text(data.Title);

                    if ($('#paymentTabs').length > 0)
                        $('#paymentTabs').append(anchor);

                    $('#' + activeTabId).addClass('active');
                }
                else if (!activeTabId && _.includes(routing, data.Id)) {
                    if (!_.includes(window.location.pathname, page)) {
                        window.location.href = page;
                        isAutoRoute = true;
                        break;
                    }
                }
            }

            if (activeTabId) {

                if ($('#activeTab').length > 0)
                    $('#activeTab').text(title);

                $('header .header-title').append(' - ' + title);
            }
            else {
                if (!isAutoRoute) {
                    page = setPaymentPage(_.first(responseData).Id);
                    if (page)
                        window.location.href = deposit + page;
                }
            }

        } else {
            if (activeTabId) {
                window.location.href = deposit;
            }
            else {

                // track accounts with no gateways
                w88Mobile.PiwikManager.trackEvent({
                    category: "Deposit",
                    action: window.User.countryCode,
                    name: window.User.memberId
                });

                nogateway();
            }
        }
    }

    function setWithdrawalPaymentTab(responseData, activeTabId) {
        if (responseData.length > 0) {
            var title = "", withdraw = "/Withdrawal/";
            _.forEach(responseData, function (data) {
                var page = setPaymentPage(data.Id);

                if (page)
                    page = withdraw + page;
                else
                    return;

                if (activeTabId) {
                    if (_.isEqual(data.Id, activeTabId))
                        title = data.Title;

                    var anchor = $('<a />', { class: 'list-group-item', id: data.Id, href: page }).text(data.Title);

                    if ($('#paymentTabs').length > 0)
                        $('#paymentTabs').append(anchor);

                    $('#' + activeTabId).addClass('active');
                }
            })

            if (activeTabId) {
                if ($('#activeTab').length > 0)
                    $('#activeTab').text(title);

                $('header .header-title').append(' - ' + title);
            }
            else {
                page = setPaymentPage(_.first(responseData).Id);
                if (page)
                    window.location.href = withdraw + page;
            }

        } else {
            if (activeTabId) {
                window.location.href = withdraw;
            }
            else {
                nogateway();
            }
        }
    }

    function nogateway() {
        $('.empty-state').show();
        $('#paymentNote').html(_w88_contents.translate("LABEL_PAYMENT_NOTE_NO_GATEWAY"));
        $('#btnSubmitPlacement').hide();
        $('#paymentSettings').hide();
        $('#paymentList').hide();
        $('.gateway-select').hide();
        $('.gateway-restrictions').hide();

        pubsub.publish('stopLoadItem', { selector: "" });
    }

    function setPaymentPage(id) {
        switch (id) {

            // withdrawal
            case "210602":
                return "210602"; // BankTransfer

            case "220815":
                return "220815"; // Neteller

            case "210709":
                return "210709"; // WingMoney

            case "2107138":
                return "2107138"; // TrueMoney

            case "220895":
                return "220895"; // VenusPoint

            case "2208102":
                return "2208102"; // IWallet

            case "2208121":
                return "2208121"; // Cubits

                // deposit
            case "120272":
                return "120272"; // Baokim

            case "110101":
                return "110101"; // FastDeposit

            case "120204":
                return "120204"; // NextPay

            case "120248":
                return "120248"; // NextPayGV

            case "120280":
                return "120280"; // JutaPay

            case "110308":
                return "110308"; // WingMoney

            case "1103132":
                return "1103132"; // TrueMoney

            case "120223":
                return "120223"; // SDPay

            case "120227":
                return "120227"; // Help2Pay

            case "1202114":
                return "1202114"; // KDPayWechat

            case "120243":
                return "120243?value=1"; // DaddyPay

            case "120244":
                return "120244?value=2"; // DaddyPayQR

            case "120214":
                return "120214"; // Neteller

            case "120290":
                return "120290"; // PaySec

            case "120254":
                return "120254"; // SDAPayAlipay

            case "1204131":
                return "1204131"; // AlipayTransfer

            case "1202111":
                return "1202111"; // ShengPayAliPay

            case "120218":
                return "120218"; // ECPSS

            case "120231":
                return "120231"; // BofoPay

            case "1202127":
                return "1202127"; // KexunPayWeChat

            case "120275":
                return "120275"; // TongHuiPay

            case "120293":
                return "120293"; // TongHuiAlipay

            case "120277":
                return "120277"; // TongHuiWeChat

            case "1202123":
                return "1202123"; // JTPayWeChat

            case "1202122":
                return "1202122"; // JTPayAliPay

            case "120236":
                return "120236"; // AllDebit

            case "120265":
                return "120265"; // EGHL

            case "120212":
                return "120212"; // NganLuong

            case "1202103":
                return "1202103"; // IWallet

            case "120296":
                return "120296"; // VenusPoint

            case "120286":
                return "120286"; // BaokimScratchCard

            case "999999":
                return "QuickOnline.aspx";

            case "999996":
                return "Alipay.aspx";

            case "999995":
                return "WeChat.aspx";

            case "1202113":
                return "1202113"; // JuyPayAlipay

            case "1202105":
                return "1202105"; // NineVPayAlipay

            case "1202133":
                return "1202133"; // AifuWeChat

            case "1202134":
                return "1202134"; // AifuAlipay

            case "1202120":
                return "1202120"; // Cubits

            case "1202112":
                return "1202112"; // DinPayTopUp

            case "1202139":
                return "1202139"; // PayTrust
            case "1202154":
                return "1202154"; // AloGatewayWechat  

            default:
                break;
        }
    }

    function createWithdraw(data, methodId) {
        send("/payments/" + methodId, "POST", data, function (response) {
            switch (response.ResponseCode) {
                case 1:
                    w88Mobile.Growl.shout("<p>" + response.ResponseMessage + "</p> <p>" + _w88_contents.translate("LABEL_TRANSACTION_ID") + ": " + response.ResponseData.TransactionId + "</p>", function () {
                        window.location = "/v2/Withdrawal/";
                    });

                    break;
                default:
                    if (_.isArray(response.ResponseMessage))
                        w88Mobile.Growl.shout(w88Mobile.Growl.bulletedList(response.ResponseMessage));
                    else
                        w88Mobile.Growl.shout(response.ResponseMessage);

                    break;
            }
        },
            function () {
                pubsub.publish('stopLoadItem', { selector: "" });
            });
    }

}