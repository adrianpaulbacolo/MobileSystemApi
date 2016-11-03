function FastDeposit() {

    var gatewayId = "110101";
    var token = "";

    var fastdeposit = {
        GetBankDetails: get,
        ToogleBank: toogleBank
    };

    return fastdeposit;

    function send(resource, method, data, success) {
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
            success: success,
            error: function () {
                console.log("Error connecting to api");
            }
        });
    }

    function get() {
        send("/user/banks", "GET", "",
            function (response) {
                if (!_.isEqual(response.ResponseCode, 0)) {
                    load(response.ResponseData);
                }
            }
        );
    }

    function load(data) {
        $('#drpBank').val(data.Bank.Value).selectmenu("refresh");

        toogleBank($('#drpBank').val());
        $('#txtBankName').val(data.BankName);
        $('#txtAccountName').val(data.AccountName);
        $('#txtAccountNumber').val(data.AccountNumber);
    }

    function toogleBank(bankId) {
        if (bankId && _.isEqual(bankId.toUpperCase(), "OTHER")) {
            $('#divBankName').show();
        }
        else {
            $('#divBankName').hide();
        }
    }
}

window.w88Mobile.Gateways.FastDeposit = FastDeposit();