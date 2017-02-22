window.w88Mobile.Gateways.Neteller = Neteller();
var _w88_neteller = window.w88Mobile.Gateways.Neteller;

function Neteller() {

    var neteller;

    try {
        neteller = Object.create(new w88Mobile.Gateway(_w88_paymentSvcV2));
    } catch (err) {
        neteller = {};
    }

    neteller.init = function () {

        setTranslations();
        function setTranslations() {
            if (_w88_contents.translate("LABEL_PAYMENT_NOTE") != "LABEL_PAYMENT_NOTE") {
            
                $('label[id$="lblAccountName"]').text("Neteller " + _w88_contents.translate("LABEL_USERNAME"));
                $('label[id$="lblAccountNumber"]').text("Neteller " + _w88_contents.translate("LABEL_PASSWORD"));
                $('label[id$="lblDepositAmount"]').text(_w88_contents.translate("LABEL_AMOUNT"));
            } else {
                window.setInterval(function () {
                    setTranslations();
                }, 500);
            }
        }
    };

    neteller.createDeposit = function() {
        var _self = this;
        var params = _self.getUrlVars();
        var data = {
            Amount: params.Amount,
            AccountName: params.AccountName,
            AccountNumber: params.AccountNumber,
            ThankYouPage: params.ThankYouPage,
        };

        _self.methodId = params.MethodId;
        _self.changeRoute();
        _self.deposit(data, function(response) {
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

    return neteller;
}