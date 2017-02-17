window.w88Mobile.Gateways.AutoRouteV2 = AutoRouteV2();
var _w88_autoroute = window.w88Mobile.Gateways.AutoRouteV2;

function AutoRouteV2() {

    var autoroute = {};

    autoroute.init = function () {

        autoroute = Object.create(new w88Mobile.Gateway(_w88_paymentSvcV2));

        setTranslations();
        function setTranslations() {
            if (_w88_contents.translate("LABEL_PAYMENT_NOTE") != "LABEL_PAYMENT_NOTE") {

                $('label[id$="lblDepositAmount"]').text(_w88_contents.translate("LABEL_AMOUNT"));
                $('label[id$="lblBank"]').text(_w88_contents.translate("LABEL_BANK"));

            } else {
                window.setInterval(function () {
                    setTranslations();
                }, 500);
            }
        }
    };

    autoroute.createDeposit = function () {
        var _self = this;
        var params = _self.getUrlVars();
        var data = {
            Amount: params.Amount,
            Bank: { Text: params.BankText, Value: params.BankValue },
            AccountName: params.AccountName
        };

        _self.methodId = params.MethodId;
        _self.changeRoute();
        _self.deposit(data, function (response) {
            switch (response.ResponseCode) {
                case 1:
                    w88Mobile.PostPaymentForm.createv2(response.ResponseData.FormData, response.ResponseData.PostUrl, "body");
                    w88Mobile.PostPaymentForm.submit();

                    break;
                default:
                    if (_.isArray(response.ResponseMessage))
                        w88Mobile.Growl.shout(w88Mobile.Growl.bulletedList(response.ResponseMessage));
                    else
                        w88Mobile.Growl.shout(response.ResponseMessage);

                    break;
            }
        },
            function () {
                pubsub.publish('stopLoadItem', { selector: "" });
            });
    }

    return autoroute;
}

window.w88Mobile.Gateways.AutoRoute = AutoRoute();

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