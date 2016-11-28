function ShengPay() {

    var gatewayId = "1202111";
    var token = "";

    var shengpay = {
        deposit: deposit
        , withdraw: withdraw
        , gatewayId: gatewayId
    };

    return shengpay;

    function send(method, data, success, error, complete) {
        var url = w88Mobile.APIUrl + "/payments/" + gatewayId;

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
            error: error,
            complete: complete
        });

    }

    // deposit
    function deposit(data, successCallback, errorCallback, completeCallback) {
        validate(data, "deposit");
        send("POST", data, successCallback, errorCallback, completeCallback);
    }

    // withdraw
    function withdraw(data, successCallback, errorCallback, completeCallback) {
    }

    function validate(data, method) {
        // @todo add validation here
        return;
    }

}

window.w88Mobile.Gateways.ShengPay = ShengPay();