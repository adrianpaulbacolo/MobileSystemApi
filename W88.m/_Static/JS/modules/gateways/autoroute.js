function AutoRoute() {

    var autoroute = {
        Deposit: deposit,
    };

    return autoroute;

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
    function deposit(methodId, data, successCallback, completeCallback) {
        send("/payments/" + methodId, "POST", data, function () { GPInt.prototype.ShowSplash() }, successCallback, completeCallback);
    }
}

window.w88Mobile.Gateways.AutoRoute = AutoRoute();