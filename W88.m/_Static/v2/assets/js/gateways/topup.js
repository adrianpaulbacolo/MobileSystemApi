window.w88Mobile.Gateways.TopUp = TopUp();
var _w88_topup = window.w88Mobile.Gateways.TopUp;

function TopUp() {

    var defaultSelect = "";
    var denomTypes = "";
    var topup;
    var methodId;

    try {
        topup = Object.create(new w88Mobile.Gateway(_w88_paymentSvcV2));
    } catch (err) {
        topup = {};
    }

    topup.init = function (id) {
        methodId = id;

        topup.setTranslations();
        topup.getDenomination();

        $('select[id$="drpDenomType"]').change(function () {
            topup.setFee(this.value);
            topup.setDenom(this.value);
        });
    };

    topup.setTranslations = function () {
        $(".pay-note").show();
        $("#paymentNote").text(_w88_contents.translate("LABEL_PAYMENT_NOTE"));
        $("#paymentNoteContent").html(_w88_contents.translate("LABEL_MSG_" + methodId));

        defaultSelect = _w88_contents.translate("LABEL_SELECT_DEFAULT");
        $('label[id$="lblPin"]').text(_w88_contents.translate("LABEL_CARD_PIN"));
        $('label[id$="lblAmount"]').html(_w88_contents.translate("LABEL_CARD_AMOUNT"));

        switch (methodId) {
            case "120286":
                $('label[id$="lblDenomType"]').text(_w88_contents.translate("LABEL_TELCO_NAME"));
                $('label[id$="lblCardSerialNo"]').text(_w88_contents.translate("LABEL_CARD_SERIAL"));
                break;
            case "1202112":
                $('label[id$="lblDenomType"]').html(_w88_contents.translate("LABEL_CARD_TYPE"));
                $('label[id$="lblCardNo"]').html(_w88_contents.translate("LABEL_CARD_NUMBER"));
                break;
            default:
                break;

        }
    }

    topup.getDenomination = function () {
        _w88_paymentSvcV2.Send("/payments/denomination/" + methodId, "GET", "", function (response) {
            if (response && _.isEqual(response.ResponseCode, 1)) {

                if (!_.isUndefined(response.ResponseData.Cards))
                    denomTypes = response.ResponseData.Cards;
                else if (!_.isUndefined(response.ResponseData.Telcos))
                    denomTypes = response.ResponseData.Telcos;

                $('select[id$="drpDenomType"]').append($('<option>').text(defaultSelect).attr('value', '-1'));
                $('select[id$="drpDenomType"]').val("-1").change();

                _.forOwn(denomTypes, function (data) {
                    $('select[id$="drpDenomType"]').append($('<option>').text(data.Name + " - " + data.Fee).attr('value', data.Id));
                });

                $('select[id$="drpAmount"]').append($('<option>').text(defaultSelect).attr('value', '-1'));
                $('select[id$="drpAmount"]').val("-1").change();
            }
        }, undefined);
    };

    topup.setDenom = function (selectedValue) {
        var denoms = _.find(denomTypes, function (data) {
            return data.Id == selectedValue;
        });

        $('select[id$="drpAmount"]').empty();

        $('select[id$="drpAmount"]').append($('<option>').text(defaultSelect).attr('value', '-1'));
        $('select[id$="drpAmount"]').val("-1").change();

        if (!_.isUndefined(denoms)) {
            _.forOwn(denoms.Denominations, function (data) {
                $('select[id$="drpAmount"]').append($('<option>').text(data.Text).attr('value', data.Value));
            });
        }
    };

    topup.setFee = function (selectedValue) {

        var fee = "";

        _.forEach(denomTypes, function (i) {
            if (i.Id == selectedValue) {
                fee = i.Fee;
            }
        });

        if (!_.isEmpty(fee)) {
            $('#paymentNoteContent').html(_w88_contents.translate("LABEL_MSG_" + methodId) + fee);
        }
    };

    topup.createDeposit = function () {
        var _self = this;
        var params = _self.getUrlVars();
        var data = {
            Amount: params.Amount,
            CardNumber: params.CardNumber,
            CardType: { Text: params.CardTypeText, Value: params.CardTypeValue },
            ReferenceId: params.ReferenceId,
            CCV: params.CCV
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

    return topup;
}