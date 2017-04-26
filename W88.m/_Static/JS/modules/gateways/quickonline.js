window.w88Mobile.Gateways.QuickOnline = QuickOnline();
var _w88_quickonline = window.w88Mobile.Gateways.QuickOnline;

function QuickOnline() {

    var quickonline;

    try {
        quickonline = Object.create(new w88Mobile.Gateway(_w88_paymentSvc));
    } catch (err) {
        quickonline = {};
    }

    quickonline.init = function (gateway, getBank) {

        $("#paymentNote").text(_w88_contents.translate("LABEL_PAYMENT_NOTE"));
        $("#paymentNoteContent").html(_w88_contents.translate("LABEL_MSG_BANK_NOT_SUPPORTED"));

        if (getBank) {

            var currencyCode = new Cookies().getCookie("currencyCode");

            _w88_paymentSvc.SendDeposit("/Banks/vendor/" + gateway + "/" + currencyCode, "GET", "", function (response) {
                $('select[id$="drpBank"]').append($('<option>').text(_w88_contents.translate("LABEL_SELECT_DEFAULT")).attr('value', '-1'));
                $('select[id$="drpBank"]').val("-1").change();

                _.forOwn(response.ResponseData, function (data) {
                    $('select[id$="drpBank"]').append($('<option>').text(data.Text).attr('value', data.Value));
                });
            });
        }
    };

    quickonline.createDeposit = function () {
        var _self = this;
        var params = _self.getUrlVars();
        var data = {
            Amount: params.Amount,
            Bank: { Text: params.BankText, Value: params.BankValue },
            SwitchLine: params.SwitchLine,
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
                            if (response.ResponseData.FormData) {
                                w88Mobile.PostPaymentForm.createv2(response.ResponseData.FormData, response.ResponseData.DummyURL, "body");
                                w88Mobile.PostPaymentForm.submit();
                            } else {
                                window.open(response.ResponseData.DummyURL, '_blank');
                            }
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
            GPInt.prototype.HideSplash();
        });
    }

    return quickonline;
}

