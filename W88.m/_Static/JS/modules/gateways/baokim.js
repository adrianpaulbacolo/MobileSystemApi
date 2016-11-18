function Baokim() {

    var gatewayId = "";
    var token = "";
    var method = "";

    var baokim = {
        deposit: deposit,
        withdraw: withdraw,
        getBanks: function(selectName) {
            return getBanks(selectName);
        },
        getTranslations: function() {
            return translations();
        },
        method: method,
        validateWallet: function () {
            return validateWallet();
        },
    };

    return baokim;

    function send(method, data, beforeSend, success, error, complete, url) {

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
            success: function(d) {
                $('#lblBanks').html(d.ResponseData.LABEL_BANK);
                $('#lblEmail').html(d.ResponseData.LABEL_EMAIL);
                $('#lblDepositAmount').html(d.ResponseData.LABEL_FUNDS_DEPOSIT + ' ' + d.ResponseData.LABEL_AMOUNT);
                $('#lblContact').html(d.ResponseData.LABEL_CONTACT);
                $('#lblWithdrawAmount').html(d.ResponseData.LABEL_FUNDS_WIDRAW + ' ' + d.ResponseData.LABEL_AMOUNT);
                sessionStorage.setItem("noticeWallet", d.ResponseData.LABEL_NOTICEEWALLET);
                sessionStorage.setItem("noticeAtm", d.ResponseData.LABEL_NOTICEATM);
            }
        });
    }

    function getBanks(selectName) {
        gatewayId = "120272";
        var url = w88Mobile.APIUrl + "/banks/vendor/" + gatewayId;

        var headers = {
            'Token': window.User.token,
            'LanguageCode': window.User.lang
        };

        $.ajax({
            type: "GET",
            url: url,
            headers: headers,
            success: function(d) {

                $('#drpBanks').append($('<option>').text(selectName).attr('value', '-1'));

                _.forOwn(d.ResponseData, function(data) {
                    $('#drpBanks').append($('<option>').text(data.Text).attr('value', data.Value));
                });

                $('#drpBanks').val('-1').change();
            }
        });
    }

    // deposit
    function deposit(data, successCallback, errorCallback, completeCallback) {
        var url = w88Mobile.APIUrl + "/payments/120272";
        validate(data, "deposit");
        send("POST", data, function () { GPInt.prototype.ShowSplash(); }, successCallback, errorCallback, completeCallback, url);
    }

    // withdraw
    function withdraw(data, successCallback, errorCallback, completeCallback) {
        var url = w88Mobile.APIUrl + "/payments/220874";
        validate(data, "widraw");
        send("POST", data, function () { GPInt.prototype.ShowSplash(); }, successCallback, errorCallback, completeCallback, url);
    }

    function validateWallet(data, successCallback, errorCallback, completeCallback) {
        var url = w88Mobile.APIUrl + "/payments/validatewallet";
        send("POST", data, function () { GPInt.prototype.ShowSplash(); }, successCallback, errorCallback, completeCallback, url);
    }

    function validate(data, method) {
        // @todo add validation here
        return;
    }
}

window.w88Mobile.Gateways.Baokim = Baokim();