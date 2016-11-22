function BofoPay() {

    var bofopay = {
        Deposit: deposit,
        Withdraw: withdraw,
    };

    return bofopay;

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
        send("/payments/120231", "POST", data, function () { GPInt.prototype.ShowSplash() }, successCallback, completeCallback);
    }

    // withdraw
    function withdraw(data, successCallback, completeCallback) {
        send("/payments/", "POST", data, function () { GPInt.prototype.ShowSplash() }, successCallback, completeCallback);
    }

    function validate(data, method) {
        // @todo add validation here
        return;
    }
}

window.w88Mobile.Gateways.BofoPay = BofoPay();