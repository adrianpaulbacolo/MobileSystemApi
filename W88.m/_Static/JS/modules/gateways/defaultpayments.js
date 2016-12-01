function DefaultPayments() {

    var defaultpayments = {
        Deposit: deposit,
        Withdraw: withdraw
    };

    return defaultpayments;

    function send(resource, method, success) {
        var url = w88Mobile.APIUrl + resource;

        var headers = {
            'Token': window.User.token,
            'LanguageCode': window.User.lang
        };

        $.ajax({
            type: method,
            url: url,
            beforeSend: function () {
                GPInt.prototype.ShowSplash(true);
            },
            headers: headers,
            success: success,
            error: function () {
                console.log("Error connecting to api");
            }
        });
    }


    // deposit
    function deposit(countryCode, memberid, paymentNotice, activeTabId) {
        send("/payments/settings/deposit", "GET",
            function (response) {
                switch (response.ResponseCode) {
                    case 1:
                        if (response.ResponseData.length > 0) {
                            var routing = ["999999", "999998", "999997", "999996", "999995"];
                            var isAutoRoute = false, title = "", page = null;


                            _.forEach(response.ResponseData, function (data) {
                                page = setPaymentPage(data.Id);

                                if (page)
                                    page = "/Deposit/" + page;
                                else
                                    return;

                                if (activeTabId) {
                                    if (_.isEqual(data.Id, activeTabId))
                                        title = data.Title;

                                    var anchor = $('<a />', { class: 'ui-btn ui-shadow ui-corner-all', id: data.Id, href: page, 'data-ajax': false }).text(data.Title);

                                    $('#depositTabs').append($('<li />').append(anchor));
                                }
                                else if (!activeTabId && _.includes(routing, data.Id)) {
                                    if (!_.includes(window.location.pathname, page))
                                        isAutoRoute = true;
                                }
                            })

                            if (isAutoRoute)
                                window.location.replace(page);
                            else {
                                if (activeTabId)
                                    $('#activeDepositTabs').text(title);
                                else {
                                    page = setPaymentPage(_.first(response.ResponseData).Id);
                                    if (page)
                                        window.location.replace("/Deposit/" + page);
                                }
                            }

                            GPInt.prototype.HideSplash();
                        } else {
                            if (activeTabId) {
                                window.location.replace("/deposit");
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
                    default:
                        break;
                }
            }
        );

        togglePayment();
    }

    // withdraw
    function withdraw(countryCode, memberid, paymentNotice, activeTabId) {
        send("/payments/settings/Withdrawal", "GET",
            function (response) {
                switch (response.ResponseCode) {
                    case 1:
                        if (response.ResponseData.length > 0) {
                            var title = "";
                            _.forEach(response.ResponseData, function (data) {
                                var page = setPaymentPage(data.Id);

                                if (_.isUndefined(page))
                                    return;
                                else
                                    page = "/Withdrawal/" + page;

                                if (activeTabId) {
                                    if (_.isEqual(data.Id, activeTabId))
                                        title = data.Title;

                                    var anchor = $('<a />', { class: 'ui-btn ui-shadow ui-corner-all', id: data.Id, href: page, 'data-ajax': false }).text(data.Title);
                                    $('#withdrawalTabs').append($('<li />').append(anchor));
                                }
                            })

                            if (activeTabId)
                                $('#activeWithdrawalTabs').text(title);
                            else {
                                page = setPaymentPage(_.first(response.ResponseData).Id);
                                if (page)
                                    window.location.replace("/Withdrawal/" + page);
                            }

                            GPInt.prototype.HideSplash();
                        } else {
                            if (activeTabId) {
                                window.location.replace("/withdrawal");
                            }
                            else {
                                $('.empty-state').show();
                                $('#paymentNote').append(paymentNotice);

                                GPInt.prototype.HideSplash();
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        );

        togglePayment();
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

                // deposit
            case "110101":
                return "FastDeposit.aspx";

            case "120203":
                return "SDAPay.aspx";

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

            case "120218":
                return "ECPSS.aspx";

            case "120231":
                return "BofoPay.aspx";

            case "120262":
                return "WeChat";

            case "120263":
                return "Alipay";

            case "120236":
                return "AllDebit.aspx";

            case "120265":
                return "EGHL.aspx";

            case "120212":
                return "NganLuong.aspx";

            case "120296":
                return "VenusPoint.aspx";

            case "120286":
                return "BaokimScratchCard.aspx";

            case "999999":
                return "QuickOnline.aspx";

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

window.w88Mobile.Gateways.DefaultPayments = DefaultPayments();