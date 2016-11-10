﻿function VenusPoint() {

    var venuspoint = {
        Initialize: init,
        Deposit: deposit,
        Withdraw: withdraw,
        ExchangeRate: exchangeRate
    };

    return venuspoint;

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
        send("/payments/120296", "POST", data, successCallback, completeCallback);
    }

    // withdraw
    function withdraw(data, successCallback, errorCallback, completeCallback) {
    }

    function validate(data, method) {
        // @todo add validation here
        return;
    }

    function init() {
        translations();
    }

    function translations() {
        send("/contents", "GET", "", function (response) {
            if (response && _.isEqual(response.ResponseCode, 1)) {
                setTranslation(response.ResponseData);
            }
        }, "");
    }

    function setTranslation(data) {
        $('#lblAcctName').text("Venus Point " + data.LABEL_USERNAME);
        $('#lblAcctNumber').text("Venus Point " + data.LABEL_PASSWORD);
    }

    function exchangeRate(data) {
        send("/payments/exchangerate", "GET", data, function (response) {
            if (response && _.isEqual(response.ResponseCode, 1)) {
                var venusPoint = 'JPY Amount = ' + response.ResponseData.Amount + ' Venus Points'
                $('#lblVenusPoints').text(venusPoint);
                var exchange = '1 JPY = ' + response.ResponseData.ExchangeRate + ' USD'
                $('#lblExchangeRate').text(exchange);
            }
        }, "");
    }
}

window.w88Mobile.Gateways.VenusPoint = VenusPoint();