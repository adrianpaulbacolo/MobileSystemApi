window.w88Mobile.Gateways.BaokimV2 = BaokimV2();
var _w88_baokim = window.w88Mobile.Gateways.BaokimV2;

function BaokimV2() {

    var baokim;

    try {
        baokim = Object.create(new w88Mobile.Gateway(_w88_paymentSvcV2));
    } catch (err) {
        baokim = {};
    }

    baokim.method = {};
    baokim.init = function (method) {

        baokim.method = method;

        $(".pay-note").show();
        $("#paymentNote").text(_w88_contents.translate("LABEL_PAYMENT_NOTE"));
        $("#paymentNoteContent").html(_w88_contents.translate("LABEL_PAYMENT_NOTE_" + baokim.method));

        $('label[id$="lblAmount"]').text(_w88_contents.translate("LABEL_AMOUNT"));
        $('label[id$="lblBank"]').text(_w88_contents.translate("LABEL_BANK"));
        $('label[id$="lblEmail"]').text(_w88_contents.translate("LABEL_EMAIL"));

        if (_.isEqual(baokim.method, "EWALLET"))

            var amount = getQueryStringValue("requestAmount");
        if (!_.isEmpty(amount)) {
            $('input[id$="txtAmount"]').autoNumeric('set', getQueryStringValue("requestAmount"));
            $('input[id$="txtAmount"]').attr('disabled', 'disabled');

            $(".otp").show();
            $('label[id$="lblOtp"]').text(_w88_contents.translate("LABEL_OTP"));
            baokim.method = "EWALLETCB";
        }
        else {
            $(".otp").hide();
            $('input[id$="txtAmount"]').removeAttr('disabled');
        }

        var email = getQueryStringValue("email");
        if (!_.isEmpty(amount)) {
            $('input[id$="txtEmail"]').val(email);
            $('input[id$="txtEmail"]').attr('disabled', 'disabled');
        }
        else {
            $('input[id$="txtEmail"]').removeAttr('disabled');
        }



    };

    baokim.initATM = function (method, getBank) {

        baokim.method = method;

        $(".pay-note").show();
        $("#paymentNote").text(_w88_contents.translate("LABEL_PAYMENT_NOTE"));
        $("#paymentNoteContent").html(_w88_contents.translate("LABEL_PAYMENT_NOTE_" + baokim.method));

        $('label[id$="lblAmount"]').text(_w88_contents.translate("LABEL_AMOUNT"));
        $('label[id$="lblBank"]').text(_w88_contents.translate("LABEL_BANK"));
        $('label[id$="lblEmail"]').text(_w88_contents.translate("LABEL_EMAIL"));
        $('label[id$="lblContact"]').text(_w88_contents.translate("LABEL_CONTACT"));


        _w88_baokim.getBanks();
    };

    baokim.createWalletDeposit = function (methodId, data) {
        _w88_paymentSvcV2.Send("/payments/" + methodId, "POST", data, function (response) {
            switch (response.ResponseCode) {
                case 1:
                    if (response.ResponseData.VendorRedirectionUrl) {
                        window.open(response.ResponseData.VendorRedirectionUrl, '_blank');
                    } else {
                        if (response.ResponseData.PostUrl) {
                            w88Mobile.PostPaymentForm.create(response.ResponseData.FormData, response.ResponseData.PostUrl, "body");
                            w88Mobile.PostPaymentForm.submit();
                        } else if (response.ResponseData.DummyURL) {
                            w88Mobile.PostPaymentForm.create(response.ResponseData.FormData, response.ResponseData.DummyURL, "body");
                            w88Mobile.PostPaymentForm.submit();
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
        });
    };

    baokim.createDeposit = function () {
        var _self = this;
        var params = _self.getUrlVars();
        var data;

        data = {
            Amount: params.Amount,
            ThankYouPage: params.ThankYouPage,
            Method: params.Method,
            Email: params.Email,
            Phone: params.Phone,
            Bank: { Text: params.BankText, Value: params.BankValue }
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
                            w88Mobile.PostPaymentForm.createv2(response.ResponseData.FormData, response.ResponseData.DummyURL, "body");
                            w88Mobile.PostPaymentForm.submit();
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
    };

    baokim.getBanks = function () {
        _w88_paymentSvcV2.Send("/banks/vendor/120272", "GET", "", function (response) {
            if (response && _.isEqual(response.ResponseCode, 1)) {
                $('select[id$="drpBank"]').append($('<option>').text(_w88_contents.translate("LABEL_SELECT_DEFAULT")).attr('value', '-1'));

                _.forOwn(response.ResponseData, function (data) {
                    $('select[id$="drpBank"]').append($('<option>').text(data.Text).attr('value', data.Value));
                });

                $('select[id$="drpBank"]').val('-1').change();
            }
        }, undefined);
    };

    baokim.verifyOtp = function (data) {
        _w88_paymentSvcV2.Send("/payments/120272", "POST", data, function (response) {
            if (response && _.isEqual(response.ResponseCode, 1)) {
                return response;
            }
        }, undefined);
    };

    baokim.validateWallet = function (data, transactionId) {

        _w88_paymentSvcV2.Send("/payments/120272/" + transactionId, "GET", data, function (response) {
            if (response && _.isEqual(response.ResponseCode, 1)) {
                switch (response.ResponseCode) {
                    case 1:
                        w88Mobile.Growl.shout(response.ResponseMessage, function () {
                            window.location.replace('/v2/Deposit/Pay120272EWALLET.aspx');
                        });
                        break;
                    default:
                        w88Mobile.Growl.shout(response.ResponseMessage);
                        break;
                }
            }
        }, undefined);
    };

    return baokim;
}
