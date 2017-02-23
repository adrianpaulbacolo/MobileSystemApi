function FastDeposit() {

    var token = "";

    var fastdeposit = {
        GetBankDetails: get,
        ToogleBank: toogleBank,
        Deposit: deposit,
        Withdraw: withdraw
    };

    return fastdeposit;

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
            headers: headers,
            beforeSend: beforeSend,
            success: success,
            error: function () {
                console.log("Error connecting to api");
            },
            complete: complete
        });
    }

    function get() {
        send("/user/banks", "GET", "", "",
            function (response) {
                if (!_.isEqual(response.ResponseCode, 0)) {
                    load(response.ResponseData);
                }
            },
            ""
        );
    }

    function load(data) {
        if (data) {
            if (!_.isEmpty(data.Bank)) $('#drpBank').val(data.Bank.Value).selectmenu("refresh");

            toogleBank($('#drpBank').val());
            $('#txtBankName').val(data.BankName);
            $('#txtAccountName').val(data.AccountName);
            $('#txtAccountNumber').val(data.AccountNumber);
        }
    }

    function toogleBank(bankId) {
        if (bankId && _.isEqual(bankId.toUpperCase(), "OTHER")) {
            $('#divBankName').show();
        }
        else {
            $('#divBankName').hide();
        }
    }

    // deposit
    function deposit(data, successCallback, completeCallback) {
        validate(data, "deposit");
        send("/payments/110101", "POST", data, function () { GPInt.prototype.ShowSplash() }, successCallback, completeCallback);
    }

    // withdraw
    function withdraw(data, successCallback, completeCallback) {
        send("/payments/210602", "POST", data, function () { GPInt.prototype.ShowSplash() }, successCallback, completeCallback);
    }

    function validate(data, method) {
        // @todo add validation here
        return;
    }
}
window.w88Mobile.Gateways.FastDeposit = FastDeposit();
