window.w88Mobile.Gateways.BaokimScratchCardV2 = BaokimScratchCardV2();
var _w88_baokimscratchcard = window.w88Mobile.Gateways.BaokimScratchCardV2;

function BaokimScratchCardV2() {

    var defaultSelect = "";
    var telcos = "";
    var baokimscratchcard;

    try {
        baokimscratchcard = Object.create(new w88Mobile.Gateway(_w88_paymentSvcV2));
    } catch (err) {
        baokimscratchcard = {};
    }

    baokimscratchcard.init = function () {

        setTranslations();
        function setTranslations() {
            if (_w88_contents.translate("LABEL_PAYMENT_NOTE") != "LABEL_PAYMENT_NOTE") {
                $('label[id$="lblBanks"]').text(_w88_contents.translate("LABEL_TELCO_NAME"));
                $('label[id$="lblPin"]').text(_w88_contents.translate("LABEL_CARD_PIN"));
                $('label[id$="lblCardSerialNo"]').text(_w88_contents.translate("LABEL_CARD_SERIAL"));

                sessionStorage.setItem("indicator", _w88_contents.translate("LABEL_INDICATOR_MSG"));
                $('p[id$="IndicatorMsg"]').html(sessionStorage.getItem("indicator"));

                defaultSelect = _w88_contents.translate("LABEL_SELECT_DEFAULT");

                _w88_baokimscratchcard.getBanks();

            } else {
                window.setInterval(function () {
                    setTranslations();
                }, 500);
            }
        }
    };

    baokimscratchcard.getBanks = function () {
        _w88_paymentSvcV2.Send("/payments/baokindenom/", "GET", "", function (response) {
            if (response && _.isEqual(response.ResponseCode, 1)) {
                telcos = response.ResponseData.Telcos;

                $('select[id$="drpBanks"]').append($('<option>').text(defaultSelect).attr('value', '-1'));
                $('select[id$="drpBanks"]').val("-1").change();

                _.forOwn(response.ResponseData.Telcos, function (data) {
                    $('select[id$="drpBanks"]').append($('<option>').text(data.Name).attr('value', data.Id));
                });

                $('select[id$="drpAmount"]').append($('<option>').text(defaultSelect).attr('value', '-1'));
                $('select[id$="drpAmount"]').val("-1").change();
            }
        }, undefined);
    };

    baokimscratchcard.setDenom = function (selectedValue) {
        var telco = _.find(telcos, function (data) {
            return data.Id == selectedValue;
        });

        $('select[id$="drpAmount"]').empty();

        $('select[id$="drpAmount"]').append($('<option>').text(defaultSelect).attr('value', '-1'));
        $('select[id$="drpAmount"]').val("-1").change();

        if (!_.isUndefined(telco)) {
            _.forOwn(telco.Denominations, function (data) {
                $('select[id$="drpAmount"]').append($('<option>').text(data.Text).attr('value', data.Value));
            });
        }
    };

    baokimscratchcard.setFee = function (selectedValue) {

        var fee = "";

        _.forEach(telcos, function (i) {
            if (i.Id == selectedValue) {
                fee = i.Fee;
            }
        });

        $('p[id$="IndicatorMsg"]').html(sessionStorage.getItem("indicator") + fee);
    };

    baokimscratchcard.createDeposit = function () {
        var _self = this;
        var params = _self.getUrlVars();
        var data = {
            Amount: params.Amount,
            CardNumber: params.CardNumber,
            ReferenceId: params.ReferenceId,
            CCV: params.CCV
        };

        _self.methodId = params.MethodId;
        _self.changeRoute();
        _self.deposit(data, function (response) {
            switch (response.ResponseCode) {
                case 1:
                    w88Mobile.PostPaymentForm.createv2(response.ResponseData.FormData, response.ResponseData.PostUrl, "body");
                    w88Mobile.PostPaymentForm.submit();

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

    return baokimscratchcard;
}