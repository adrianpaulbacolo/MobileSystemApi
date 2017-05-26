window.w88Mobile.Gateways.QuickOnlineV2 = QuickOnlineV2();
var _w88_quickonline = window.w88Mobile.Gateways.QuickOnlineV2;

function QuickOnlineV2() {

    var quickonline;
    var methodId;

    try {
        quickonline = Object.create(new w88Mobile.Gateway(_w88_paymentSvcV2));
    } catch (err) {
        quickonline = {};
    }

    quickonline.init = function (id, getBank) {
        methodId = id;
        $('[id$="lblSwitchLine"]').text(_w88_contents.translate("LABEL_SWITCH_LINE"));
        $('label[id$="lblBank"]').text(_w88_contents.translate("LABEL_BANK"));

        $("#paymentNote").text(_w88_contents.translate("LABEL_PAYMENT_NOTE"));

        if (_.isEqual(methodId, "120265")) { //EGHL
            if (siteCookie.getCookie('currencyCode') == 'MYR') {
                $(".pay-note").show();
                $('#paymentNoteContent').html(_w88_contents.translate("LABEL_MSG_" + methodId));
                quickonline.showBank();
                getBank = true;
            }
            else {
                $(".pay-note").hide();
                quickonline.hideBank();
            }
        } else {
            $(".pay-note").show();
            $("#paymentNoteContent").html(_w88_contents.translate("LABEL_MSG_BANK_NOT_SUPPORTED"));
        }

        if (getBank == true) {
            quickonline.getBank();
        }
    };

    quickonline.getBank = function () {
        var _self = this;

        var resource = "/vendor/" + methodId;
        if (_.isEqual(methodId, "999999"))
            resource = "/gateway";

        _self.send("/Banks" + resource, "GET", "", function (response) {
            var banks = response.ResponseData;
            var defaultSelect = _w88_contents.translate("LABEL_SELECT_DEFAULT");
            $('select[id$="drpBank"]').append($('<option>').text(defaultSelect).attr('value', '-1'));
            $('select[id$="drpBank"]').val("-1").change();

            _.forOwn(banks, function (data) {
                if (_.isEqual(data.Value, "ICBC") || _.isEqual(data.Value, "ECITIC"))
                    data.Text = data.Text + " (*)";

                $('select[id$="drpBank"]').append($('<option>').text(data.Text).attr('value', data.Value));
            });
        });
    };

    quickonline.showBank = function () {
        $('.bank').show();
        $('select[id$="drpBank').attr({ required: '', 'data-selectequals': '-1' });
        $('#form1').validator('update')
    };

    quickonline.hideBank = function () {
        $('.bank').hide();
        $('select[id$="drpBank"]').removeAttr('required data-selectequals');
        $('#form1').validator('update')
    };

    quickonline.nganluongInit = function () {
        $(".pay-note").show();
        $("#paymentNote").text(_w88_contents.translate("LABEL_PAYMENT_NOTE"));
        $('#paymentNoteContent').html(_w88_contents.translate("LABEL_MSG_120212"));

        $("#btnSubmitPlacement").text(_w88_contents.translate("BUTTON_PROCEED"));

        quickonline.getNganLuongVendorUrl();
    };

    quickonline.getNganLuongVendorUrl = function () {
        var _self = this;

        _self.send("/payments/120212", "POST", "", function (response) {
            switch (response.ResponseCode) {
                case 1:
                    $('#btnSubmitPlacement').click(function (e) {
                        window.open(response.ResponseData.VendorRedirectionUrl, '_blank');
                    });
                    break;
                default:
                    if (_.isArray(response.ResponseMessage))
                        w88Mobile.Growl.shout(w88Mobile.Growl.bulletedList(response.ResponseMessage));
                    else
                        w88Mobile.Growl.shout(response.ResponseMessage);

                    break;
            }
        });
    }

    quickonline.createDeposit = function () {
        var _self = this;
        var params = _self.getUrlVars();
        var data = {
            ThankYouPage: params.ThankYouPage,
            SwitchLine: params.SwitchLine,
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
    }

    return quickonline;
}