function DinPayTopUp() {

    var gatewayId = "";
    var cards = "";

    var dinpaytopup = {
        Initialize: init,
        SetFee: setFee,
        SetDenom: setDenom,
    };

    return dinpaytopup;

    function init(gateway) {
        gatewayId = gateway;
        setTranslations();
        getDenomination();
    }

    function getDenomination() {
        _w88_paymentSvc.SendDeposit("/payments/denomination/" + gatewayId, "GET", "", function (response) {
            cards = response.ResponseData.Cards;

            $('select[id$="drpCardType"]').append($('<option>').text(_w88_contents.translate("LABEL_SELECT_DEFAULT")).attr('value', '-1'));
            $('select[id$="drpCardType"]').val("-1").selectmenu("refresh");

            _.forOwn(cards, function (data) {
                $('select[id$="drpCardType"]').append($('<option>').text(data.Name + "-" + data.Fee).attr('value', data.Id));
            });

            $('select[id$="drpAmount"]').append($('<option>').text(_w88_contents.translate("LABEL_SELECT_DEFAULT")).attr('value', '-1'));
            $('select[id$="drpAmount"]').val("-1").selectmenu("refresh");
        });
    }

    function setDenom(selectedValue) {
        var card = _.find(cards, function (data) {
            return data.Id == selectedValue;
        });

        $('select[id$="drpAmount"]').empty();

        $('select[id$="drpAmount"]').append($('<option>').text(_w88_contents.translate("LABEL_SELECT_DEFAULT")).attr('value', '-1'));
        $('select[id$="drpAmount"]').val("-1").selectmenu("refresh");

        _.forOwn(card.Denominations, function (data) {
            $('select[id$="drpAmount"]').append($('<option>').text(data.Text).attr('value', data.Value));
        });
    }

    function setFee(selectedValue) {

        var fee = "";

        _.forEach(cards, function (i) {
            if (i.Id == selectedValue) {
                fee = i.Fee;
            }
        });

        $('p[id$="IndicatorMsg"]').html(sessionStorage.getItem("indicator") + fee);
    }


    function setTranslations() {
        if (_w88_contents.translate("LABEL_CARD_TYPE") != "LABEL_CARD_TYPE") {
            $('label[id$="lblCardType"]').html(_w88_contents.translate("LABEL_CARD_TYPE"));
            $('label[id$="lblDepositAmount"]').html(_w88_contents.translate("LABEL_CARD_AMOUNT"));
            $('label[id$="lblCardNo"]').html(_w88_contents.translate("LABEL_CARD_NUMBER"));
            $('label[id$="lblPin"]').html(_w88_contents.translate("LABEL_CARD_PIN"));

            $("#paymentNote").text(_w88_contents.translate("LABEL_PAYMENT_NOTE"));
            $("#paymentNoteContent").html(_w88_contents.translate("LABEL_MSG_1202112"));

            sessionStorage.setItem("indicator", _w88_contents.translate("LABEL_INDICATOR_MSG"));
            $('p[id$="IndicatorMsg"]').html(sessionStorage.getItem("indicator"));
        } else {
            window.setInterval(function () {
                setTranslations();
            }, 500);
        }
    }
}

window.w88Mobile.Gateways.DinPayTopUp = DinPayTopUp();