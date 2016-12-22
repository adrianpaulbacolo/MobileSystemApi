function BaokimScratchCard() {

    var gatewayId = "", defaultSelect = "";
    var telcos = "";

    var baokim = {
        Deposit: deposit,
        SetFee: setFee,
        SetDenom: setDenom,
        Initialize: translations,
    };

    return baokim;

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

    function translations() {
        var url = w88Mobile.APIUrl + "/contents";

        var headers = {
            'LanguageCode': window.User.lang
        };
        $.ajax({
            type: "GET",
            url: url,
            headers: headers,
            success: function (d) {
                $('#lblDepositAmount').html(d.ResponseData.LABEL_CARD_AMOUNT);
                $('#lblBanks').html(d.ResponseData.LABEL_TELCO_NAME);
                $('#lblPin').html(d.ResponseData.LABEL_CARD_PIN);
                $('#lblCardSerialNo').html(d.ResponseData.LABEL_CARD_SERIAL);

                sessionStorage.setItem("indicator", d.ResponseData.LABEL_INDICATOR_MSG);
                $('#IndicatorMsg').html(sessionStorage.getItem("indicator"));

                defaultSelect = d.ResponseData.LABEL_SELECT_DEFAULT;

                getBanks();
            }
        });
    }

    function getBanks() {
        var url = w88Mobile.APIUrl + "/payments/baokindenom/";

        $.ajax({
            type: "GET",
            url: url,
            success: function (d) {
                telcos = d.ResponseData.Telcos;

                $('#drpBanks').append($('<option>').text(defaultSelect).attr('value', '-1'));
                $('#drpBanks').val("-1").selectmenu("refresh");

                _.forOwn(d.ResponseData.Telcos, function (data) {
                    $('#drpBanks').append($('<option>').text(data.Name).attr('value', data.Id));
                });


                $('#drpAmount').append($('<option>').text(defaultSelect).attr('value', '-1'));
                $('#drpAmount').val("-1").selectmenu("refresh");
            }
        });
    }

    function setDenom(selectedValue) {
        var telco = _.find(telcos, function (data) {
            return data.Id == selectedValue;
        });

        $('#drpAmount').empty();

        $('#drpAmount').append($('<option>').text(defaultSelect).attr('value', '-1'));
        $('#drpAmount').val("-1").selectmenu("refresh");

        _.forOwn(telco.Denominations, function (data) {
            $('#drpAmount').append($('<option>').text(data.Text).attr('value', data.Value));
        });
    }

    function setFee(selectedValue) {

        var fee = "";

        _.forEach(telcos, function (i) {
            if (i.Id == selectedValue) {
                fee = i.Fee;
            }
        });

        $('#IndicatorMsg').html(sessionStorage.getItem("indicator") + fee);
    }

    // deposit
    function deposit(data, successCallback, errorCallback, completeCallback) {
        gatewayId = "120286";
        validate(data, "deposit");
        send("POST", data, function () { GPInt.prototype.ShowSplash(); }, successCallback, errorCallback, completeCallback);
    }

    function validate(data, method) {
        // @todo add validation here
        return;
    }
}

window.w88Mobile.Gateways.BaokimScratchCard = BaokimScratchCard();