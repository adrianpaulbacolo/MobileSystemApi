window.w88Mobile.Gateways.Aifu = Aifu();
var _w88_aifu = window.w88Mobile.Gateways.Aifu;

function Aifu() {

    var aifu = Object.create(new w88Mobile.Gateway(_w88_paymentSvc));

    aifu.createDeposit = function () {
        var _self = this;
        var params = _self.getUrlVars();
        var data = {
            Amount: params.Amount
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

    return aifu;
}

