window.w88Mobile.Gateways.ECPSSPay = ECPSSPay();
var _w88_ecpsspay = window.w88Mobile.Gateways.ECPSSPay;

function ECPSSPay() {

    var ecpss = Object.create(new w88Mobile.Gateway(_w88_paymentSvc));

    ecpss.createDeposit = function () {
        var _self = this;
        var params = _self.getUrlVars();
        var data = {
            Amount: params.Amount,
            Bank: { Text: params.BankText, Value: params.BankValue },
        };

        _self.methodId = params.MethodId;
        _self.changeRoute();
        _self.deposit(data, function (response) {
            switch (response.ResponseCode) {
                case 1:
                    w88Mobile.Growl.shout(response.ResponseMessage);
                    w88Mobile.PostPaymentForm.createv2(response.ResponseData.FormData, response.ResponseData.DummyURL, "body");
                    $(".ui-page").attr("display", "none");
                    w88Mobile.PostPaymentForm.submit();
                    $('#form1')[0].reset();
                    break;
                default:
                    if (_.isArray(response.ResponseMessage))
                        w88Mobile.Growl.shout(w88Mobile.Growl.bulletedList(response.ResponseMessage), _self.shoutCallback);
                    else
                        w88Mobile.Growl.shout(response.ResponseMessage, _self.shoutCallback);
                    $('#form1')[0].reset();
                    break;
            }
        },
        function () {
            window.w88Mobile.FormValidator.enableSubmitButton('#btnSubmit');
            GPInt.prototype.HideSplash();
        });
    }

    return ecpss; 
}

