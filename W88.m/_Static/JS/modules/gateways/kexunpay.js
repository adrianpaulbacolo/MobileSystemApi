window.w88Mobile.Gateways.KexunPayV2 = KexunPayV2();
var _w88_kexunpayV2 = window.w88Mobile.Gateways.KexunPayV2;

function KexunPayV2() {

    var kexunpay = Object.create(new w88Mobile.Gateway(_w88_paymentSvcV2));

    kexunpay.init = function (version) {

        setTranslations();

        function setTranslations() {
            if (_w88_contents.translate("LABEL_PAYMENT_NOTE") != "LABEL_PAYMENT_NOTE") {
                $("#paymentNote").text(_w88_contents.translate("LABEL_PAYMENT_NOTE"));
                if (version.toLowerCase() == "wechat") {
                    $("#paymentNoteContent").text(_w88_contents.translate("LABEL_PAYMENT_NOTE_WECHAT"));
                }
                else {
                    $("#paymentNoteContent").text(_w88_contents.translate("LABEL_PAYMENT_NOTE_ALIPAY"));
                }
            } else {
                window.setInterval(function () {
                    setTranslations();
                }, 500);
            }
        }

    };

    kexunpay.createDeposit = function () {
        var _self = this;
        var params = _self.getUrlVars();
        var data = {
            Amount: params.Amount,
            ThankYouPage: params.ThankYouPage,
        };

        _self.methodId = params.MethodId;
        _self.changeRoute();
        _self.deposit(data, function (response) {
            switch (response.ResponseCode) {
                case 1:
                    w88Mobile.PostPaymentForm.createv2(response.ResponseData.FormData, response.ResponseData.PostUrl, "body");
                    w88Mobile.PostPaymentForm.submit();

                    $('#form1')[0].reset();
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
            window.w88Mobile.FormValidator.enableSubmitButton('#btnSubmit');
            GPInt.prototype.HideSplash();
        });
    }

    return kexunpay;
}



window.w88Mobile.Gateways.KexunPay = KexunPay();
var _w88_kexunpay = window.w88Mobile.Gateways.KexunPay;

function KexunPay() {

    var kexunpay = Object.create(new w88Mobile.Gateway(_w88_paymentSvc));

    kexunpay.createDeposit = function () {
        var _self = this;
        var params = _self.getUrlVars();
        var data = {
            Amount: params.Amount,
            ThankYouPage: params.ThankYouPage,
        };

        _self.methodId = params.MethodId;
        _self.changeRoute();
        _self.deposit(data, function (response) {
            switch (response.ResponseCode) {
                case 1:
                    w88Mobile.PostPaymentForm.createv2(response.ResponseData.FormData, response.ResponseData.PostUrl, "body");
                    w88Mobile.PostPaymentForm.submit();

                    $('#form1')[0].reset();
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
            window.w88Mobile.FormValidator.enableSubmitButton('#btnSubmit');
            GPInt.prototype.HideSplash();
        });
    }

    return kexunpay;
}

