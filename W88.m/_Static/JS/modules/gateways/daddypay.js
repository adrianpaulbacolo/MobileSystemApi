function DaddyPay() {

    var daddypay = {
        Deposit: deposit,
        Withdraw: withdraw,
        PopulateWeChatNickName: populateWeChatNickName,
        TooglePaymentMethod: tooglePaymentMethod
    };

    return daddypay;

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
        var gateway = _.isEqual(data.Method, "QR") ? "120244" : "120243";

        validate(data, "deposit");
        send("/payments/" + gateway, "POST", data, function () { GPInt.prototype.ShowSplash() }, successCallback, completeCallback);
    }

    // withdraw
    function withdraw(data, successCallback, errorCallback, completeCallback) {
        send("/payments/220895", "POST", data, function () { GPInt.prototype.ShowSplash() }, successCallback, completeCallback);
    }

    function validate(data, method) {
        // @todo add validation here
        return;
    }

    function tooglePaymentMethod(bId) {
        $("#txtAccountName").val('');
        if (bId == "40") { //WeChat
            $("#txtAmount").hide();
            $("#drpAmount").show();
            $("#accountNo").hide();
            populateWeChatNickName();
        }
        else { //QR
            $("#txtAmount").show();
            $("#drpAmount").hide();
            $("#accountNo").show();
        }
    }

    function populateWeChatNickName() {
        $.ajax({
            type: "POST",
            async: false,
            url: "DaddyPay.aspx/ProcessWeChatNickname",
            data: JSON.stringify({ action: "getNickname", nickname: "" }),
            contentType: "application/json;",
            dataType: "json",
            success: function (response) {
                var result = response.d;
                $('#txtAccountName').val(result);
                $('#hfWCNickname').val(result); //store original nickname if any.
            }
        })
    }

}

window.w88Mobile.Gateways.DaddyPay = DaddyPay();