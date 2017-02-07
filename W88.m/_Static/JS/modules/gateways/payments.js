function payments(gatewayId) {

    this.gatewayId = gatewayId;

    this.resource = "/payments/";

    this.init = function () {
        var _self = this;

        var isDeposit = _.includes(window.location.pathname.toLowerCase(), "deposit");

        var storage = (isDeposit) ? w88Mobile.Keys.depositSettings : w88Mobile.Keys.withdrawalSettings;
        var payment = isDeposit ? amplify.store(storage) : amplify.store(storage);

        if (payment) {
            _self.setLimits(payment);
        } else {

            if (!payment || (payment && window.User.lang != payment.language)) {
                var headers = {
                    'Token': window.User.token,
                    'LanguageCode': window.User.lang
                };
                var url = w88Mobile.APIUrl + _self.resource + "settings/" + ((isDeposit) ? "deposit" : "withdrawal");

                $.ajax({
                    type: "GET",
                    url:  url,
                    headers: headers,
                    success: function (response) {
                        switch (response.ResponseCode) {
                            case 1:
                                var data = { settings: response.ResponseData, language: window.User.lang };
                                amplify.store(storage, data, window.User.storageExpiration);
                                _self.setLimits(data);
                            default:
                                break;
                        }
                    },
                    error: function () {
                        console.log("Error connecting to api");
                    },
                });
            }

        }
    };

    this.setLimits = function (paymentSettings) {
        var _self = this;

        var setting = _.find(paymentSettings.settings, function (data) {
            return data.Id == _self.gatewayId;
        });

        if (!_.isEmpty(setting)) {
            $('#txtMode').text(": " + setting.PaymentMode)
            $('#txtMinMaxLimit').text(": " + setting.MinAmount.toLocaleString(undefined, { minimumFractionDigits: 2 }) + " / " + setting.MaxAmount.toLocaleString(undefined, { minimumFractionDigits: 2 }))
            $('#txtDailyLimit').text(": " + setting.LimitDaily)
            $('#txtTotalAllowed').text(": " + setting.TotalAllowed)
        }
    }

    this.send = function (data, success, complete) {
        var _self = this;

        var url = w88Mobile.APIUrl + _self.resource + _self.gatewayId;

        var headers = {
            'Token': window.User.token,
            'LanguageCode': window.User.lang
        };

        $.ajax({
            type: "POST",
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

window.w88Mobile.Gateways.Payments = payments;
