window.w88Mobile.Gateways.VenusPointV2 = VenusPointV2();
var _w88_venuspoint = window.w88Mobile.Gateways.VenusPointV2;

function VenusPointV2() {

    var venuspoint;

    try {
        venuspoint = Object.create(new w88Mobile.Gateway(_w88_paymentSvcV2));
    } catch (err) {
        venuspoint = {};
    }

    venuspoint.init = function () {

        setTranslations();
        function setTranslations() {
            if (_w88_contents.translate("LABEL_PAYMENT_NOTE") != "LABEL_PAYMENT_NOTE") {
                $('label[id$="lblDepositAmount"]').text(_w88_contents.translate("LABEL_AMOUNT"));
                $('label[id$="lblAcctName"]').text("Venus Point " + _w88_contents.translate("LABEL_ACCOUNT_ID"));
                $('label[id$="lblAcctNumber"]').text("Venus Point " + _w88_contents.translate("LABEL_PASSWORD"));
            } else {
                window.setInterval(function () {
                    setTranslations();
                }, 500);
            }
        }
    };

    venuspoint.exchangeRate = function(data) {
        _w88_paymentSvcV2.SendDeposit("/payments/exchangerate", "GET", data, function (response) {
            if (response && _.isEqual(response.ResponseCode, 1)) {
                var venusPoint = 'JPY Amount = ' + response.ResponseData.Amount + ' Venus Points';
                $('span[id$="lblVenusPoints"]').text(venusPoint);
                var exchange = '1 JPY = ' + response.ResponseData.ExchangeRate + ' USD';
                $('span[id$="lblExchangeRate"]').text(exchange);
            }
        }, undefined);
    };

    venuspoint.createDeposit = function () {
        var _self = this;
        var params = _self.getUrlVars();
        var data = {
            Amount: params.Amount,
            AccountName: params.AccountName,
            AccountNumber: params.AccountNumber,
            ThankYouPage: params.ThankYouPage
        };

        _self.methodId = params.MethodId;
        _self.changeRoute();
        _self.deposit(data, function (response) {
            switch (response.ResponseCode) {
                case 1:
                    if (response.ResponseData.VendorRedirectionUrl) {
                        window.open(response.ResponseData.VendorRedirectionUrl, '_blank');
                    } else {
                        if (response.ResponseData.PostUrl) {
                            w88Mobile.PostPaymentForm.createv2(response.ResponseData.FormData, response.ResponseData.PostUrl, "body");
                            w88Mobile.PostPaymentForm.submit();
                        } else if (response.ResponseData.DummyURL) {
                            window.open(response.ResponseData.DummyURL, '_blank');
                        }
                    }

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

    return venuspoint;
}

function VenusPoint() {

    var venuspoint = {
        Initialize: init,
        Deposit: deposit,
        Withdraw: withdraw,
        ExchangeRate: exchangeRate
    };

    return venuspoint;

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

    // deposit
    function deposit(data, successCallback, completeCallback) {
        validate(data, "deposit");
        send("/payments/120296", "POST", data, function () { GPInt.prototype.ShowSplash() }, successCallback, completeCallback);
    }

    // withdraw
    function withdraw(data, successCallback, completeCallback) {
        send("/payments/220895", "POST", data, function () { GPInt.prototype.ShowSplash() }, successCallback, completeCallback);
    }

    function validate(data, method) {
        // @todo add validation here
        return;
    }

    function init() {
        translations();
    }

    function translations() {
        send("/contents", "GET", "", "", function (response) {
            if (response && _.isEqual(response.ResponseCode, 1)) {
                setTranslation(response.ResponseData);
            }
        }, "");
    }

    function setTranslation(data) {
        $('label[id$="lblAcctName"]').text("Venus Point " + data.LABEL_ACCOUNT_ID);
        $('label[id$="lblAcctNumber"]').text("Venus Point " + data.LABEL_PASSWORD);
    }

    function exchangeRate(data) {
        send("/payments/exchangerate", "GET", data, "", function (response) {
            if (response && _.isEqual(response.ResponseCode, 1)) {
                var venusPoint = 'JPY Amount = ' + response.ResponseData.Amount + ' Venus Points'
                $('span[id$="lblVenusPoints"]').text(venusPoint);
                var exchange = '1 JPY = ' + response.ResponseData.ExchangeRate + ' USD'
                $('span[id$="lblExchangeRate"]').text(exchange);
            }
        }, "");
    }
}

window.w88Mobile.Gateways.VenusPoint = VenusPoint();