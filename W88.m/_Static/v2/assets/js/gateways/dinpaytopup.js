window.w88Mobile.Gateways.DinpayTopUp = DinpayTopUp();
var _w88_dinpaytopup = window.w88Mobile.Gateways.DinpayTopUp;

function DinpayTopUp() {

    var defaultSelect = "";
    var cards = "";
    var dinpaytopup;

    try {
        dinpaytopup = Object.create(new w88Mobile.Gateway(_w88_paymentSvcV2));
    } catch (err) {
        dinpaytopup = {};
    }

    dinpaytopup.init = function (gatewayId) {

        $('label[id$="lblCardType"]').html(_w88_contents.translate("LABEL_CARD_TYPE"));
        $('label[id$="lblDepositAmount"]').html(_w88_contents.translate("LABEL_CARD_AMOUNT"));
        $('label[id$="lblCardNo"]').html(_w88_contents.translate("LABEL_CARD_NUMBER"));
        $('label[id$="lblPin"]').html(_w88_contents.translate("LABEL_CARD_PIN"));

        $("#paymentNote").text(_w88_contents.translate("LABEL_PAYMENT_NOTE"));
        $("#paymentNoteContent").html(_w88_contents.translate("LABEL_MSG_1202112"));

        defaultSelect = _w88_contents.translate("LABEL_SELECT_DEFAULT");

        _w88_dinpaytopup.getDenomination(gatewayId);

    };

    dinpaytopup.getDenomination = function (gatewayId) {
        _w88_paymentSvcV2.Send("/payments/denomination/" + gatewayId, "GET", "", function (response) {
            if (response && _.isEqual(response.ResponseCode, 1)) {
                cards = response.ResponseData.Cards;

                $('select[id$="drpCardType"]').append($('<option>').text(defaultSelect).attr('value', '-1'));
                $('select[id$="drpCardType"]').val("-1").change();

                _.forOwn(cards, function (data) {
                    $('select[id$="drpCardType"]').append($('<option>').text(data.Name + "-" + data.Fee).attr('value', data.Id));
                });

                $('select[id$="drpAmount"]').append($('<option>').text(defaultSelect).attr('value', '-1'));
                $('select[id$="drpAmount"]').val("-1").change();
            }
        }, undefined);
    };

    dinpaytopup.setDenom = function (selectedValue) {
        var card = _.find(cards, function (data) {
            return data.Id == selectedValue;
        });

        $('select[id$="drpAmount"]').empty();

        $('select[id$="drpAmount"]').append($('<option>').text(defaultSelect).attr('value', '-1'));
        $('select[id$="drpAmount"]').val("-1").change();

        if (!_.isUndefined(card)) {
            _.forOwn(card.Denominations, function (data) {
                $('select[id$="drpAmount"]').append($('<option>').text(data.Text).attr('value', data.Value));
            });
        }
    };

    dinpaytopup.setFee = function (selectedValue) {

        var fee = "";

        _.forEach(cards, function (i) {
            if (i.Id == selectedValue) {
                fee = i.Fee;
            }
        });
    };

    dinpaytopup.createDeposit = function () {
        var _self = this;
        var params = _self.getUrlVars();
        var data = {
            Amount: params.Amount,
            CardNumber: params.CardNumber,
            CardType: { Text: params.CardTypeText, Value: params.CardTypeValue },
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
    };

    return dinpaytopup;
}