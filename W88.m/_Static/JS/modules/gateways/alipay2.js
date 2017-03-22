window.w88Mobile.Gateways.Alipay2 = Alipay2();
var _w88_alipay2 = window.w88Mobile.Gateways.Alipay2;

function Alipay2() {

    var alipay2 = Object.create(new w88Mobile.Gateway(_w88_paymentSvc));

    alipay2.createDeposit = function () {
        var _self = this;
        var params = _self.getUrlVars();
        var data = {
            Amount: params.Amount,
            SwitchLine: params.SwitchLine,
            ThankYouPage: params.ThankYouPage
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
                        w88Mobile.Growl.shout(w88Mobile.Growl.bulletedList(response.ResponseMessage), _self.shoutCallback);
                    else
                        w88Mobile.Growl.shout(response.ResponseMessage, _self.shoutCallback);
                    break;
            }
        },
        function () {
            GPInt.prototype.HideSplash();
        });
    }

    return alipay2;
}

