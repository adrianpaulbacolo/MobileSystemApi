function PaySec() {

    var gatewayId = "120290";
    var token = "";

    var paysec = {
        deposit: deposit
        , withdraw: withdraw
        , gatewayId: gatewayId
        , callVendor: callVendor
    };

    return paysec;

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

    function callVendor(method, url, data, success, error, complete) {

        var headers = {
            'Access-Control-Allow-Headers': "x-requested-with"
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

}

window.w88Mobile.Gateways.PaySec = PaySec();