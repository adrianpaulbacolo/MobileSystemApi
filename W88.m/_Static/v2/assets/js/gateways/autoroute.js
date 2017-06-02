window.w88Mobile.Gateways.AutoRouteV2 = AutoRouteV2();
var _w88_autoroute = window.w88Mobile.Gateways.AutoRouteV2;

function AutoRouteV2() {

    var autoroute;

    try {
        autoroute = Object.create(new w88Mobile.Gateway(_w88_paymentSvcV2));
    } catch (err) {
        autoroute = {};
    }

    autoroute.init = function () {
        $('[id$="lblSwitchLine"]').text($.i18n("LABEL_SWITCH_LINE"));
        $('label[id$="lblBank"]').text($.i18n("LABEL_BANK"));
    };

    autoroute.createDeposit = function () {
        var _self = this;
        var params = _self.getUrlVars();
        var data = {
            Amount: params.Amount,
            Bank: { Text: params.BankText, Value: params.BankValue },
            SwitchLine: params.SwitchLine,
            AccountName: params.AccountName
        };

        _self.methodId = params.MethodId;
        _self.changeRoute();
        _self.deposit(data, function (response) {
            switch (response.ResponseCode) {
                case 1:
                    w88Mobile.PostPaymentForm.create(response.ResponseData.FormData, response.ResponseData.PostUrl, "body");
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
                pubsub.publish('stopLoadItem', { selector: "" });
            });
    }

    return autoroute;
}