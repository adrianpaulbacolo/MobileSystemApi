function PayGo() {

    var defaultBank = "";

    var paygo = {
        InitDeposit: initDeposit,
        InitWithdraw: initWithdraw,
    };

    return paygo;

    function send(resource, method, data, beforeSend, success, complete) {
        var url = w88Mobile.APIUrl + resource;

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
            error: function () {
                console.log("Error connecting to api");
            },
            complete: complete
        });
    }

    function initDeposit() {
        translations();
        loadMoneyAccount();
    }

    function initWithdraw() {
        countryphone();
    }

    function translations() {
        send("/contents", "GET", "", "", function (response) {
            if (response && _.isEqual(response.ResponseCode, 1)) {
                setTranslation(response.ResponseData);
            }
        }, "");
    }

    function setTranslation(data) {
        $('#lblSystemAccount').text(data.LABEL_SYSTEM_ACCOUNT);
        $('#lblDepositDateTime').text(data.LABEL_DEPOSIT_DATETIME);
        $('#lblReferenceId').text(data.LABEL_REFERENCE_ID);

        defaultBank = data.LABEL_SELECT_DEFAULT + " " + data.LABEL_SYSTEM_ACCOUNT;

        $('#lblReferenceId').text(data.LABEL_REFERENCE_ID);
    }

    function loadMoneyAccount() {
        send("/banks/money/110394", "GET", "",
            "",
            function (response) {
                $('#drpSystemAccount').append($("<option></option>").attr("value", "-1").text(defaultBank));

                $('#drpSystemAccount').val("-1").selectmenu("refresh");

                _.forEach(response.ResponseData, function (data) {
                    $('#drpSystemAccount').append($("<option></option>").attr("value", JSON.stringify(data)).text(data.Text))
                })
            },
            ""
        );
    }

    function countryphone() {
        send("/countryphonelist", "GET", "",
            "",
            function (response) {
                _.forEach(response.ResponseData.PhoneList, function (data) {
                    $('#drpContactCountry').append($("<option></option>").attr("value", data.Value).text(data.Text))
                })

                $('#drpContactCountry').val(response.ResponseData.PhoneSelected).selectmenu("refresh");
            },
            ""
        );
    }

}

window.w88Mobile.Gateways.PayGo = PayGo()