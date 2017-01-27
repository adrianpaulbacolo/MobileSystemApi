function JTPay() {

    var gatewayId = "120262";
    var token = "";

    var jtpay = {
        deposit: deposit
        , withdraw: withdraw
        , gatewayId: gatewayId
        , Initialize: init
    };

    return jtpay;

    function send(method, data, success, error, complete) {
        var url = w88Mobile.APIUrl + "/payments/" + w88Mobile.Gateways.JTPay.gatewayId;

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

    function init(version) {
        $("#paymentNote").text(_w88_contents.translate("LABEL_PAYMENT_NOTE"));

        if (version == "0") {
            $("#paymentNoteContent").text(_w88_contents.translate("LABEL_PAYMENT_NOTE0"));
        }
        else {
            $("#paymentNoteContent").text(_w88_contents.translate("LABEL_PAYMENT_NOTE1"));
        }
    }

}

window.w88Mobile.Gateways.JTPay = JTPay();