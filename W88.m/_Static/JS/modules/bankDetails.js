function BankDetails() {

    var defaultBank = { "Text": "", "Value": "-1" };

    var BankDetails = {
        Initialize: init,
        ToogleBank: toogleBank,
        CreateBankDetails: create,
        Translations: translations
    };

    return BankDetails;

    function init() {
        translations(function (response) {
            if (response && _.isEqual(response.ResponseCode, 1)) {
                setTranslation(response.ResponseData);
            }
        });

        get();
    }

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

    function toogleBank(data) {
        if (data && _.isEqual(JSON.parse(data).Value.toUpperCase(), "OTHER")) {
            $('#divBankName').show();
        } else {
            $('#divBankName').hide();
        }
    }

    function translations(successCallback) {
        send("/contents", "GET", "", "", successCallback, "");
    }

    function setTranslation(data) {
        defaultBank.Text = data.LABEL_SELECT_DEFAULT;

        $('#h1title').text(data.LABEL_MENU_BANK_DETAILS);
        $('#lblBank').text(data.LABEL_BANK);
        $('#lblBankName').text(data.LABEL_BANK_NAME);
        $('#lblBankBranch').text(data.LABEL_BANK_BRANCH);
        $('#lblBankAddress').text(data.LABEL_BANK_ADDRESS);
        $('#lblAcctName').text(data.LABEL_ACCOUNT_NAME);
        $('#lblAcctNumber').text(data.LABEL_ACCOUNT_NUMBER);
        $('#btnSubmit').val(data.BUTTON_SUBMIT).button("refresh");
        $('#lblIsPreferred').text(data.LABEL_IS_PREFERRED);
    }

    function loadBank(response) {
        send("/banks/member", "GET", "",
            function () {
                GPInt.prototype.ShowSplash();
            },
            function (response) {
                $('#drpBank').append($("<option></option>").attr("value", JSON.stringify(defaultBank)).text(defaultBank.Text));
                setSelectedBank(defaultBank);

                _.forEach(response.ResponseData, function (data) {
                    $('#drpBank').append($("<option></option>").attr("value", JSON.stringify(data)).text(data.Text))
                })
            },
            function () {
                GPInt.prototype.HideSplash();

                if (response, !_.isEqual(response.ResponseCode, 0)) {
                    loadValues(response.ResponseData);
                }

                toogleBank($('#drpBank').val());
            }
        );
    }

    function setSelectedBank(data) {
        $('#drpBank').val(JSON.stringify(data)).selectmenu("refresh");
    }

    function create(data, successCallback) {
        send("/user/banks", "POST", data,
            function () {
                GPInt.prototype.ShowSplash();
            },
        successCallback,
            function () {
                GPInt.prototype.HideSplash();
            }
        );
    }

    function get() {
        send("/user/banks", "GET", "",
            function () {
                GPInt.prototype.ShowSplash();
            },
            function (response) {
                loadBank(response);
            },
            function () {
                GPInt.prototype.HideSplash();
            }
        );
    }

    function loadValues(data) {
        setSelectedBank(data.Bank);
        $('#txtBankName').val(data.BankName);
        $('#txtBankBranch').val(data.BankBranch);
        $('#txtBankAddress').val(data.BankAddress);
        $('#txtAccountName').val(data.AccountName);
        $('#txtAccountNumber').val(data.AccountNumber);
        $("#isPreferred").prop("checked", data.IsPreferred).checkboxradio("refresh");
    }
}

window.w88Mobile.BankDetails = BankDetails();