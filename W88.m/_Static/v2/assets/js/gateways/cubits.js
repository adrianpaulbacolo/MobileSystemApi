window.w88Mobile.Gateways.Cubits = Cubits();
var _w88_cubits = window.w88Mobile.Gateways.Cubits;

function Cubits() {

    var cubits;

    try {
        cubits = Object.create(new w88Mobile.Gateway(_w88_paymentSvcV2));
    } catch (err) {
        cubits = {};
    }

    cubits.init = function (gatewayId) {

        setTranslations();

        function setTranslations() {
            $('label[id$="lblAddress"]').text(_w88_contents.translate("LABEL_ADDRESS"));

            $("#paymentNote").text(_w88_contents.translate("LABEL_PAYMENT_NOTE"));
            $("#paymentNoteContent").html(_w88_contents.translate("LABEL_MSG_" + gatewayId));
        }
    };

    cubits.createDeposit = function () {
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
                    if (response.ResponseData.VendorRedirectionUrl) {
                        window.open(response.ResponseData.VendorRedirectionUrl, '_blank');
                    } else {
                        if (response.ResponseData.PostUrl) {
                            w88Mobile.PostPaymentForm.createv2(response.ResponseData.FormData, response.ResponseData.PostUrl, "body");
                            w88Mobile.PostPaymentForm.submit();
                        } else if (response.ResponseData.DummyURL) {
                            window.open(response.ResponseData.DummyURL, '_blank');
                        }
                    }

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

    return cubits;
}
