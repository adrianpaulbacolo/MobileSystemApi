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
        Send :send
    };

    return defaultpayments;

    function send(resource, method, success, data, complete) {
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


    // deposit
    function deposit(countryCode, memberid, paymentNotice, activeTabId) {
        send("/payments/settings/deposit", "GET",
            function (response) {
                switch (response.ResponseCode) {
                    case 1:
                        if (response.ResponseData.length > 0) {
                            var routing = [
                                autorouteIds.QuickOnline,
                                autorouteIds.UnionPay,
                                autorouteIds.TopUpCard,
                                autorouteIds.AliPay,
                                autorouteIds.WeChat
                            ];

                            var isAutoRoute = false, title = "", page = null, deposit = "/Deposit/";

                            for (var i = 0; i < response.ResponseData.length; i++) {
                                var data = response.ResponseData[i];

                                page = setPaymentPage(data.Id);

                                if (page)
                                    page = deposit + page;
                                else
                                    continue;

                                if (activeTabId) {
                                    if (_.isEqual(data.Id, activeTabId))
                                        title = data.Title;

                                    var anchor = $('<a />', { class: 'ui-btn ui-shadow ui-corner-all', id: data.Id, href: page, 'data-ajax': false }).text(data.Title);

                                    $('#depositTabs').append($('<li />').append(anchor));
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
                                $('#activeDepositTabs').text(title);
                                $('#headerTitle').append(' - ' + title);
                            }
                            else {
                                if (!isAutoRoute) {
                                    page = setPaymentPage(_.first(response.ResponseData).Id);
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
                            var title = "", withdraw = "/Withdrawal/";
                            _.forEach(response.ResponseData, function (data) {
                                var page = setPaymentPage(data.Id);

                                if (page)
                                    page = withdraw + page;
                                else
                                    return;

                                if (activeTabId) {
                                    if (_.isEqual(data.Id, activeTabId))
                                        title = data.Title;

                                    var anchor = $('<a />', { class: 'ui-btn ui-shadow ui-corner-all', id: data.Id, href: page, 'data-ajax': false }).text(data.Title);
                                    $('#withdrawalTabs').append($('<li />').append(anchor));
                                }
                            })

                            if (activeTabId) {
                                $('#activeWithdrawalTabs').text(title);
                                $('#headerTitle').append(' - ' + title);
                            }
                            else {
                                page = setPaymentPage(_.first(response.ResponseData).Id);
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

            case "1202111":
                return "ShengPayAliPay.aspx";

            case "120218":
                return "ECPSS.aspx";

            case "120231":
                return "BofoPay.aspx";

            case "1202123":
                return "WeChat";

            case "1202122":
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

            case "999996":
                return "Alipay.aspx";

            case "1202113":
                return "JuyPayAlipay.aspx";

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