function DefaultPayments() {

    var autorouteIds = {
        QuickOnline: "999999",
        UnionPay: "999998",
        TopUpCard: "999997",
        AliPay: "999996",
        WeChat: "999995",
    };

    var defaultpayments = {
        Deposit: deposit,
        Withdraw: withdraw,
        AutoRouteIds: autorouteIds,
        Send: sendv1,
        SendDeposit: send,
        DisplaySettings: displaySettings,
        setPaymentTabs: setPaymentTabs
    };

    var paymentCache = {};

    var paymentOptions = {};

    return defaultpayments;

    function displaySettings(methodId, options) {
        paymentOptions = options;
        fetchSettings(paymentOptions.type, function () {
            if (!_.isEmpty(paymentCache)) {
                var setting = _.find(paymentCache.settings, function (data) {
                    return data.Id == methodId;
                });

                if (setting) {
                    $('#txtMode').text(": " + setting.PaymentMode)
                    $('#txtMinMaxLimit').text(": " + setting.MinAmount.toLocaleString(undefined, { minimumFractionDigits: 2 }) + " / " + setting.MaxAmount.toLocaleString(undefined, { minimumFractionDigits: 2 }))
                    $('#txtDailyLimit').text(": " + setting.LimitDaily)
                    $('#txtTotalAllowed').text(": " + setting.TotalAllowed)
                }
            }
        })
    }

    function setPaymentTabs(type, activeMethodId) {
        if (type == "deposit") {
            fetchSettings(type, function () {
                // payment cache variable is now present once callback is triggered
                setDepositPaymentTab(paymentCache.settings, activeMethodId);
                togglePayment();
            });
        } else {
            fetchSettings(type, function () {
                setWithdrawalPaymentTab(paymentCache.settings, activeMethodId);
                togglePayment();
            });
        }
    }

    function fetchSettings(type, callback) {

        var url = "/payments/settings/" + type;
        cacheKey = (type == "deposit") ? w88Mobile.Keys.depositSettings : w88Mobile.Keys.withdrawalSettings;

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
                GPInt.prototype.ShowSplash(true);
            },
            headers: headers,
            success: success,
            error: function () {
                console.log("Error connecting to api");
            },
            complete: complete
        });
    }

    // to be deprecated, use "send"
    function sendv1(resource, method, success, data, complete) {
        send(resource, method, data, success, complete);
    }


    // deposit to be deprecated once new flow is applied use fetchSettings
    function deposit(countryCode, memberid, paymentNotice, activeTabId) {

        var payment = amplify.store(w88Mobile.Keys.depositSettings);

        if (payment && window.User.lang == payment.language) {
            setDepositPaymentTab(payment.settings, activeTabId)
        }
        else {
            send("/payments/settings/deposit", "GET", {},
                function (response) {
                    switch (response.ResponseCode) {
                        case 1:
                            var data = { settings: response.ResponseData, language: window.User.lang };

                            amplify.store(w88Mobile.Keys.depositSettings, data, window.User.storageExpiration);

                            setDepositPaymentTab(response.ResponseData, activeTabId)
                        default:
                            break;
                    }
                }
            );
        }

        togglePayment();
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

                    var anchor = $('<a />', { class: 'ui-btn ui-shadow ui-corner-all', id: data.Id, href: page, 'data-ajax': false }).text(data.Title);

                    if ($('#depositTabs').length > 0)
                        $('#depositTabs').append($('<li />').append(anchor));

                    if ($('#paymentTabs').length > 0)
                        $('#paymentTabs').append($('<li />').append(anchor));

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
                if ($('#activeDepositTabs').length > 0)
                    $('#activeDepositTabs').text(title);

                if ($('#activeTab').length > 0)
                    $('#activeTab').text(title);

                $('#headerTitle').append(' - ' + title);
            }
            else {
                if (!isAutoRoute) {
                    page = setPaymentPage(_.first(responseData).Id);
                    if (page)
                        window.location.href = deposit + page;
                }
            }

            GPInt.prototype.HideSplash();
        } else {
            if (activeTabId) {
                window.location.href = deposit;
            }
            else {

                // track accounts with no gateways
                w88Mobile.PiwikManager.trackEvent({
                    category: "Deposit",
                    action: countryCode,
                    name: memberid
                });

                $('.empty-state').show();
                $('#paymentNote').append(paymentNotice);

                GPInt.prototype.HideSplash();
            }
        }
    }

    // withdraw to be deprecated once new flow is applied use fetchSettings
    function withdraw(countryCode, memberid, paymentNotice, activeTabId) {

        var payment = amplify.store(w88Mobile.Keys.withdrawalSettings);

        if (payment && window.User.lang == payment.language) {
            setWithdrawalPaymentTab(payment.settings, activeTabId)
        }
        else {
            send("/payments/settings/Withdrawal", "GET", {},
                function (response) {
                    switch (response.ResponseCode) {
                        case 1:
                            var data = { settings: response.ResponseData, language: window.User.lang };

                            amplify.store(w88Mobile.Keys.withdrawalSettings, data, window.User.storageExpiration);

                            setWithdrawalPaymentTab(response.ResponseData, activeTabId)
                            break;
                        default:
                            break;
                    }
                }
            );
        }

        togglePayment();
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

                    var anchor = $('<a />', { class: 'ui-btn ui-shadow ui-corner-all', id: data.Id, href: page, 'data-ajax': false }).text(data.Title);

                    if ($('#withdrawalTabs').length > 0)
                        $('#withdrawalTabs').append($('<li />').append(anchor));

                    if ($('#paymentTabs').length > 0)
                        $('#paymentTabs').append($('<li />').append(anchor));
                }
            })

            if (activeTabId) {
                if ($('#activeWithdrawalTabs').length > 0)
                    $('#activeWithdrawalTabs').text(title);

                if ($('#activeTab').length > 0)
                    $('#activeTab').text(title);

                $('#headerTitle').append(' - ' + title);
            }
            else {
                page = setPaymentPage(_.first(responseData).Id);
                if (page)
                    window.location.href = withdraw + page;
            }

            GPInt.prototype.HideSplash();
        } else {
            if (activeTabId) {
                window.location.href = withdraw;
            }
            else {
                $('.empty-state').show();
                $('#paymentNote').append(paymentNotice);

                GPInt.prototype.HideSplash();
            }
        }
    }

    function setPaymentPage(id) {
        switch (id) {

            // withdrawal
            case "210602":
                return "BankTransfer.aspx";

            case "220815":
                return "Neteller.aspx";

            case "210709":
                return "WingMoney.aspx";

            case "220895":
                return "VenusPoint.aspx";

            case "2208102":
                return "IWallet.aspx";

                // deposit
            case "110101":
                return "FastDeposit.aspx";

            case "120204":
                return "NextPay.aspx";

            case "120280":
                return "JutaPay.aspx";

            case "110308":
                return "WingMoney.aspx";

            case "120223":
                return "SDPay.aspx";

            case "120227":
                return "Help2Pay.aspx";

            case "1202114":
                return "KDPayWechat.aspx";

            case "120243":
                return "DaddyPay.aspx?value=1";

            case "120244":
                return "DaddyPay.aspx?value=2";

            case "120214":
                return "Neteller.aspx";

            case "120290":
                return "PaySec.aspx";

            case "120254":
                return "SDAPay.aspx";

            case "1202111":
                return "ShengPayAliPay.aspx";

            case "120218":
                return "ECPSS.aspx";

            case "120231":
                return "BofoPay.aspx";

            case "1202123":
                return "WeChat";

            case "1202127":
                return "KexunPay.aspx";

            case "1202122":
                return "Alipay";

            case "120236":
                return "AllDebit.aspx";

            case "120265":
                return "EGHL.aspx";

            case "120212":
                return "NganLuong.aspx";

            case "1202103":
                return "IWallet.aspx";

            case "120296":
                return "VenusPoint.aspx";

            case "120286":
                return "BaokimScratchCard.aspx";

            case "999999":
                return "QuickOnline.aspx";

            case "999996":
                return "Alipay.aspx";

            case "999995":
                return "WeChat.aspx";

            case "1202113":
                return "JuyPayAlipay.aspx";

            case "1202105":
                return "NineVPayAlipay.aspx";

            default:
                break
        }
    }

    function togglePayment() {
        $(".toggle-list-btn").click(function () {
            $(this).parent().find('.toggle-list').slideToggle("fast", function () {
                if (!$('.toggle-list-btn').hasClass('toggled')) {
                    $(this).parent().find('.toggle-list-btn').addClass('toggled');
                }
                else {
                    $(this).parent().find('.toggle-list-btn').removeClass('toggled');
                }
            });
        });
    }
}

var _w88_paymentSvc = window.w88Mobile.Gateways.DefaultPayments = DefaultPayments();