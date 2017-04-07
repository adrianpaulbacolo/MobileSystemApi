function BaokimScratchCard() {

    var defaultSelect = "";
    var telcos = "";
    var gatewayId = "";

    var baokim = {
        Deposit: deposit,
        SetFee: setFee,
        SetDenom: setDenom,
        Initialize: init,
        };

    return baokim;

    function init() {
        setTranslations();
        function setTranslations() {
            if (_w88_contents.translate("LABEL_PAYMENT_NOTE") != "LABEL_PAYMENT_NOTE") {
                $('label[id$="lblDepositAmount"]').text(_w88_contents.translate("LABEL_CARD_AMOUNT"));
                $('label[id$="lblBanks"]').text(_w88_contents.translate("LABEL_TELCO_NAME"));
                $('label[id$="lblPin"]').text(_w88_contents.translate("LABEL_CARD_PIN"));
                $('label[id$="lblCardSerialNo"]').text(_w88_contents.translate("LABEL_CARD_SERIAL"));

                sessionStorage.setItem("indicator", _w88_contents.translate("LABEL_INDICATOR_MSG"));
                $('p[id$="IndicatorMsg"]').html(sessionStorage.getItem("indicator"));

                defaultSelect = _w88_contents.translate("LABEL_SELECT_DEFAULT");

                getBanks();

            } else {
                window.setInterval(function () {
                    setTranslations();
                }, 500);
            }
            }
    }

    function getBanks() {
        var url = w88Mobile.APIUrl + "/payments/denomination/120286";

        $.ajax({
            type: "GET",
            url: url,
            success: function (d) {
                telcos = d.ResponseData.Telcos;

                $('#drpBank').append($('<option>').text(defaultSelect).attr('value', '-1'));
                $('#drpBank').val("-1").selectmenu("refresh");

                _.forOwn(d.ResponseData.Telcos, function (data) {
                    $('#drpBank').append($('<option>').text(data.Name).attr('value', data.Id));
                });


                $('select[id$="drpAmount"]').append($('<option>').text(defaultSelect).attr('value', '-1'));
                $('select[id$="drpAmount"]').val("-1").selectmenu("refresh");
            }
        });
    }

    function setDenom(selectedValue) {
        var telco = _.find(telcos, function (data) {
            return data.Id == selectedValue;
        });

        $('select[id$="drpAmount"]').empty();

        $('select[id$="drpAmount"]').append($('<option>').text(defaultSelect).attr('value', '-1'));
        $('select[id$="drpAmount"]').val("-1").selectmenu("refresh");

        _.forOwn(telco.Denominations, function (data) {
            $('select[id$="drpAmount"]').append($('<option>').text(data.Text).attr('value', data.Value));
        });
    }

    function setFee(selectedValue) {

        var fee = "";

        _.forEach(telcos, function (i) {
            if (i.Id == selectedValue) {
                fee = i.Fee;
            }
        });

        $('#paymentNoteContent').html(sessionStorage.getItem("indicator") + fee);
    }

    function send(method, data, beforeSend, success, error, complete) {
        var url = w88Mobile.APIUrl + "/payments/" + gatewayId;

        var headers = {
            'Token': window.User.token,
            'LanguageCode': window.User.lang
        };
        $.ajax({
            type: method,
            url: url,
            data: data,
            beforeSend: beforeSend,
            headers: headers,
            success: success,
            error: error,
            complete: complete
        });

    }

    function deposit(data, successCallback, errorCallback, completeCallback) {
        gatewayId = "120286";
        send("POST", data, function () { GPInt.prototype.ShowSplash(); }, successCallback, errorCallback, completeCallback);
    }


}

window.w88Mobile.Gateways.BaokimScratchCard = BaokimScratchCard();