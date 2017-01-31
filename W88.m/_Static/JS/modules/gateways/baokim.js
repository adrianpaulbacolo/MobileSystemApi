function Baokim() {

    var gatewayId = "";
    var method = "";

    var baokim = {
        Initialize: init,
        deposit: deposit,
        withdraw: withdraw,
        getBanks: function(selectName) {
            return getBanks(selectName);
        },
        method: method,
        validateWallet: validateWallet
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

    function init() {
        setTranslations();
        function setTranslations() {
            if (_w88_contents.translate("LABEL_PAYMENT_NOTE") != "LABEL_PAYMENT_NOTE") {
                
                $('#ContentPlaceHolder1_ContentPlaceHolder2_lblBanks').html(_w88_contents.translate("LABEL_BANK"));
                $('#ContentPlaceHolder1_ContentPlaceHolder2_lblEmail').html(_w88_contents.translate("LABEL_EMAIL"));
                $('#ContentPlaceHolder1_ContentPlaceHolder2_lblDepositAmount').html(_w88_contents.translate("LABEL_FUNDS_DEPOSIT") + ' ' + _w88_contents.translate("LABEL_AMOUNT"));
                $('#ContentPlaceHolder1_ContentPlaceHolder2_lblContact').html(_w88_contents.translate("LABEL_CONTACT"));
                $('#ContentPlaceHolder1_ContentPlaceHolder2_lblWithdrawAmount').html(_w88_contents.translate("LABEL_FUNDS_WIDRAW") + ' ' + _w88_contents.translate("LABEL_AMOUNT"));
                $('#ContentPlaceHolder1_ContentPlaceHolder2_lblOtp').html(_w88_contents.translate("LABEL_OTP"));
                sessionStorage.setItem("noticeWallet", _w88_contents.translate("LABEL_NOTICEEWALLET"));
                sessionStorage.setItem("noticeAtm", _w88_contents.translate("LABEL_NOTICEATM"));

            } else {
                window.setInterval(function () {
                    setTranslations();
                }, 500);
            }
        }
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

                $('#ContentPlaceHolder1_ContentPlaceHolder2_drpBanks').append($('<option>').text(selectName).attr('value', '-1'));

                _.forOwn(d.ResponseData, function(data) {
                    $('#ContentPlaceHolder1_ContentPlaceHolder2_drpBanks').append($('<option>').text(data.Text).attr('value', data.Value));
                });

                $('#ContentPlaceHolder1_ContentPlaceHolder2_drpBanks').val('-1').change();
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

    function validateWallet(walletData, transactionId, successCallback, errorCallback, completeCallback) {
        var url = w88Mobile.APIUrl + "/payments/120272/" + transactionId;
        send("GET", walletData, function () { GPInt.prototype.ShowSplash(); }, successCallback, errorCallback, completeCallback, url);
    }

    function validate(data, method) {
        // @todo add validation here
        return;
    }
}

window.w88Mobile.Gateways.Baokim = Baokim();