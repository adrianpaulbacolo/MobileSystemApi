window.w88Mobile.Gateways.IWallet = IWallet();
var _w88_iwallet = window.w88Mobile.Gateways.IWallet;

function IWallet() {

    var iwallet = Object.create(new w88Mobile.Gateway(_w88_paymentSvc));

    iwallet.createDeposit = function () {
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
            GPInt.prototype.HideSplash();
        });
    }

    return iwallet;
}

