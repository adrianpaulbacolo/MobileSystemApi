function BaokimScratchCard() {

    var gatewayId = "";
    var telcos = "";

    var baokim = {
        deposit: deposit,
        getBanks: function(selectName) {
            return getBanks(selectName);
        },
        getTranslations: function() {
            return translations();
        },
        SetFee: function (selectedValue) {
            return setFee(selectedValue);
        }
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
                $('#lblDepositAmount').html(d.ResponseData.LABEL_FUNDS_DEPOSIT + ' ' + d.ResponseData.LABEL_AMOUNT);
                $('#lblBanks').html(d.ResponseData.LABEL_TELCO_NAME);
                $('#lblPin').html(d.ResponseData.LABEL_CARD_PIN);
                $('#lblCardSerialNo').html(d.ResponseData.LABEL_CARD_SERIAL);

                sessionStorage.setItem("indicator", d.ResponseData.LABEL_INDICATOR_MSG);
                $('#IndicatorMsg').html(sessionStorage.getItem("indicator"));
            }
        });
    }

    function getBanks(selectName) {
        var url = w88Mobile.APIUrl + "/payments/baokindenom/" ;

        $.ajax({
            type: "GET",
            url: url,
            success: function(d) {
                telcos = d.ResponseData.Telcos;

                $('#drpBanks').append($('<option>').text(selectName).attr('value', '-1'));

                _.forOwn(d.ResponseData.Telcos, function (data) {
                    $('#drpBanks').append($('<option>').text(data.Name).attr('value', data.Id));
                });

                $('#drpBanks').val('-1').change();

            }
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