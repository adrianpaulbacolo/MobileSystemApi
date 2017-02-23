function NganLuong() {

    var gatewayId = "";
    var token = "";

    var nganluong = {
        deposit: deposit
        , withdraw: withdraw
    };

    return nganluong;

    function send(method, data, success, error, complete) {
        var url = w88Mobile.APIUrl + "/payments/" + gatewayId;

        var headers = {
            'Token': window.User.token
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
        gatewayId = "120212";
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

window.w88Mobile.Gateways.NganLuong = NganLuong();