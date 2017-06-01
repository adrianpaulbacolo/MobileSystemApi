window.w88Mobile.Gateways.BaokimV2 = BaokimV2();
var _w88_baokim = window.w88Mobile.Gateways.BaokimV2;

function BaokimV2() {

    var baokim;
    var methodId;

    try {
        baokim = Object.create(new w88Mobile.Gateway(_w88_paymentSvcV2));
    } catch (err) {
        baokim = {};
    }

    baokim.method = {};
    baokim.initEWALLET = function (id, method) {
        methodId = id;
        baokim.method = method;

        $(".pay-note").show();
        $("#paymentNote").text($.i18n("LABEL_PAYMENT_NOTE"));
        $("#paymentNoteContent").html($.i18n("LABEL_PAYMENT_NOTE_" + baokim.method));

        $('label[id$="lblAmount"]').text($.i18n("LABEL_AMOUNT"));
        $('label[id$="lblBank"]').text($.i18n("LABEL_BANK"));
        $('label[id$="lblEmail"]').text($.i18n("LABEL_EMAIL"));

        var amount = getQueryStringValue("requestAmount");
        if (!_.isEmpty(amount)) {
            $('input[id$="txtAmount"]').autoNumeric('set', getQueryStringValue("requestAmount"));
            $('input[id$="txtAmount"]').attr('disabled', 'disabled');

            baokim.showOTP();
            baokim.method = "EWALLETCB";
        }
        else {
            baokim.hideOTP();
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

    baokim.showOTP = function () {
        $('.otp').show();
        $('label[id$="lblOtp"]').text($.i18n("LABEL_OTP"));
        $('input[id$="txtOtp"]').attr({ required: '', 'data-require': '' });
        $('#form1').validator('update')
    };

    baokim.hideOTP = function () {
        $('.otp').hide();
        $('input[id$="txtOtp"]').removeAttr('required data-require');
        $('#form1').validator('update')
    };

    baokim.initATM = function (id, method) {
        methodId = id;
        baokim.method = method;

        $(".pay-note").show();
        $("#paymentNote").text($.i18n("LABEL_PAYMENT_NOTE"));
        $("#paymentNoteContent").html($.i18n("LABEL_PAYMENT_NOTE_" + baokim.method));

        $('label[id$="lblAmount"]').text($.i18n("LABEL_AMOUNT"));
        $('label[id$="lblBank"]').text($.i18n("LABEL_BANK"));
        $('label[id$="lblEmail"]').text($.i18n("LABEL_EMAIL"));
        $('label[id$="lblContact"]').text($.i18n("LABEL_CONTACT"));

        baokim.getBanks();
    };

    baokim.initWithdraw = function () {
        $('label[id$="lblEmail"]').text($.i18n("LABEL_EMAIL"));
    };

    baokim.createWalletDeposit = function (data) {
        var _self = this;

        _self.send("/payments/" + methodId, "POST", data, function (response) {
            switch (response.ResponseCode) {
                case 1:
                    window.location.replace(response.ResponseData.PostUrl);
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
                        window.open(response.ResponseData.VendorRedirectionUrl);
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
        },
            function () {
                pubsub.publish('stopLoadItem', { selector: "" });
            });
    };

    baokim.getBanks = function () {
        var _self = this;

        _self.send("/banks/vendor/" + methodId, "GET", "", function (response) {
            if (response && _.isEqual(response.ResponseCode, 1)) {
                $('select[id$="drpBank"]').append($('<option>').text($.i18n("LABEL_SELECT_DEFAULT")).attr('value', '-1'));

                _.forOwn(response.ResponseData, function (data) {
                    $('select[id$="drpBank"]').append($('<option>').text(data.Text).attr('value', data.Value));
                });
            }
        }, "");
    };

    baokim.verifyOtp = function (data, successCallback) {
        var _self = this;

        _self.send("/payments/" + methodId, "POST", data, successCallback);
    };

    baokim.validateWallet = function (data, transactionId) {
        var _self = this;

        _self.send("/payments/" + methodId + "/" + transactionId, "GET", data, function (response) {
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

    baokim.createWithdraw = function (data) {
        var _self = this;
        _self.methodId = methodId;
        _self.withdraw(data);
    };

    return baokim;
}
