window.w88Mobile.Gateways.NganLuongV2 = NganLuongV2();
var _w88_nganluong = window.w88Mobile.Gateways.NganLuongV2;

function NganLuongV2() {

    var nganloung;

    try {
        nganloung = Object.create(new w88Mobile.Gateway(_w88_paymentSvcV2));
    } catch (err) {
        nganloung = {};
    }

    nganloung.init = function () {

        setTranslations();
        function setTranslations() {
            if (_w88_contents.translate("BUTTON_PROCEED") != "BUTTON_PROCEED") {
                $("#btnSubmitPlacement").text(_w88_contents.translate("BUTTON_PROCEED"));
            } else {
                window.setInterval(function () {
                    setTranslations();
                }, 500);
            }
        }
    };

    nganloung.createDeposit = function() {
        var _self = this;
        var params = _self.getUrlVars();

        _self.methodId = params.MethodId;
        _self.changeRoute();
        _self.deposit("", function(response) {
                switch (response.ResponseCode) {
                case 1:
                    window.open(response.ResponseData.VendorRedirectionUrl, '_blank');

                    break;
                default:
                    if (_.isArray(response.ResponseMessage))
                        w88Mobile.Growl.shout(w88Mobile.Growl.bulletedList(response.ResponseMessage));
                    else
                        w88Mobile.Growl.shout(response.ResponseMessage);

                    break;
                }
            },
            function() {
                pubsub.publish('stopLoadItem', { selector: "" });
            });
    };

    return nganloung;
}


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