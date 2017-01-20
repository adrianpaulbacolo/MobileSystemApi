function payments(gatewayId) {

    this.gatewayId = gatewayId;

    this.init = function () {
        var _self = this;

        var isDeposit = _.includes(window.location.pathname.toLowerCase(), "deposit");

        var payment = isDeposit ? amplify.store("depositSettings") : amplify.store("withdrawalSettings");

        if (payment) {
            var setting = _.find(payment.settings, function (data) {
                return data.Id == _self.gatewayId;
            });

            if (setting) {
                $('#txtMode').text(": " + setting.PaymentMode)
                $('#txtMinMaxLimit').text(": " + setting.MinAmount.toLocaleString(undefined, { minimumFractionDigits: 2 }) + " / " + setting.MaxAmount.toLocaleString(undefined, { minimumFractionDigits: 2 }))
                $('#txtDailyLimit').text(": " + setting.LimitDaily)
                $('#txtTotalAllowed').text(": " + setting.TotalAllowed)
            }
        }
    };

    this.resource = "/payments/";

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
