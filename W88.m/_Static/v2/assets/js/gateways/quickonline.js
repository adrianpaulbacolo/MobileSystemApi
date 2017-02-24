window.w88Mobile.Gateways.QuickOnlineV2 = QuickOnlineV2();
var _w88_quickonline = window.w88Mobile.Gateways.QuickOnlineV2;

function QuickOnlineV2() {

    var quickonline;

    try {
        quickonline = Object.create(new w88Mobile.Gateway(_w88_paymentSvcV2));
    } catch (err) {
        quickonline = {};
    }

    quickonline.init = function(gateway, getBank) {

        setTranslations();

        function setTranslations() {
            if (_w88_contents.translate("LABEL_PAYMENT_NOTE") != "LABEL_PAYMENT_NOTE") {

                $('label[id$="lblDepositAmount"]').text(_w88_contents.translate("LABEL_AMOUNT"));
                $('label[id$="lblBank"]').text(_w88_contents.translate("LABEL_BANK"));

                //payment note is for ECPSS
                $("#paymentNote").text(_w88_contents.translate("LABEL_PAYMENT_NOTE"));
                $("#paymentNoteContent").text(_w88_contents.translate("LABEL_MSG_BANK_NOT_SUPPORTED"));

            } else {
                window.setInterval(function() {
                    setTranslations();
                }, 500);
            }
        }

        if (getBank) {
            _w88_paymentSvcV2.Send("/Banks/vendor/" + gateway + "/" + new Cookies().getCookie("currencyCode"), "GET", "", function (response) {
                var banks = response.ResponseData;
                var defaultSelect = _w88_contents.translate("LABEL_SELECT_DEFAULT");
                $('select[id$="drpBank"]').append($('<option>').text(defaultSelect).attr('value', '-1'));
                $('select[id$="drpBank"]').val("-1").change();

                _.forOwn(banks, function (data) {
                    $('select[id$="drpBank"]').append($('<option>').text(data.Text).attr('value', data.Value));
                });
            });
        }
    };

    quickonline.nganluongInit = function () {

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

    quickonline.createDeposit = function () {
        var _self = this;
        var params = _self.getUrlVars();
        var data = {
            ThankYouPage: params.ThankYouPage
        };

        if (!_.isUndefined(params.Amount)) {
            data.Amount = params.Amount;
        }

        if (!_.isUndefined(params.BankValue)) {
            data.Bank = { Text: params.BankText, Value: params.BankValue };
        }

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

    return quickonline;
}