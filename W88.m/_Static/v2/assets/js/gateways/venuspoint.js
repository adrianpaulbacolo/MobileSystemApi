window.w88Mobile.Gateways.VenusPointV2 = VenusPointV2();
var _w88_venuspoint = window.w88Mobile.Gateways.VenusPointV2;

function VenusPointV2() {

    var venuspoint;

    try {
        venuspoint = Object.create(new w88Mobile.Gateway(_w88_paymentSvcV2));
    } catch (err) {
        venuspoint = {};
    }

    venuspoint.init = function () {

        setTranslations();
        function setTranslations() {
            if (_w88_contents.translate("LABEL_PAYMENT_NOTE") != "LABEL_PAYMENT_NOTE") {
                $('label[id$="lblDepositAmount"]').text(_w88_contents.translate("LABEL_AMOUNT"));
                $('label[id$="lblAcctName"]').text("Venus Point " + _w88_contents.translate("LABEL_ACCOUNT_ID"));
                $('label[id$="lblAcctNumber"]').text("Venus Point " + _w88_contents.translate("LABEL_PASSWORD"));
            } else {
                window.setInterval(function () {
                    setTranslations();
                }, 500);
            }
        }
    };

    venuspoint.exchangeRate = function (data) {
        _w88_paymentSvcV2.SendDeposit("/payments/exchangerate", "GET", data, function (response) {
            if (response && _.isEqual(response.ResponseCode, 1)) {
                var venusPoint = 'JPY Amount = ' + response.ResponseData.Amount + ' Venus Points';
                $('span[id$="lblVenusPoints"]').text(venusPoint);
                var exchange = '1 JPY = ' + response.ResponseData.ExchangeRate + ' USD';
                $('span[id$="lblExchangeRate"]').text(exchange);
            }
        }, undefined);
    };

    venuspoint.createDeposit = function () {
        var _self = this;
        var params = _self.getUrlVars();
        var data = {
            Amount: params.Amount,
            AccountName: params.AccountName,
            AccountNumber: params.AccountNumber,
            ThankYouPage: params.ThankYouPage
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

    return venuspoint;
}