window.w88Mobile.Gateways.FastDepositv2 = FastDepositv2();
var _w88_fastdep = window.w88Mobile.Gateways.FastDepositv2;

function FastDepositv2() {

    var fastdeposit = Object.create(new w88Mobile.Gateway(_w88_paymentSvcV2));

    fastdeposit.init = function () {
        var _self = this;

        setTranslations();

        function setTranslations() {

            if (Cookies().getCookie('language').toLowerCase() == 'vn' && Cookies().getCookie('currencyCode').toLowerCase() == 'vnd') {
                if (_w88_contents.translate("LABEL_PAYMENT_NOTE_FASTDEPOSIT") != "LABEL_PAYMENT_NOTE_FASTDEPOSIT") {
                    $("#paymentNoteContent").text(_w88_contents.translate("LABEL_PAYMENT_NOTE_FASTDEPOSIT"));
                    $('label[id$="lblDepositAmount"]').text(_w88_contents.translate("LABEL_AMOUNT"));
                } else {
                    window.setInterval(function () {
                        setTranslations();
                    }, 500);
                }
            }

            _w88_paymentSvcV2.SendDeposit("/user/banks", "GET", "", function (response) {
                if (!_.isEqual(response.ResponseCode, 0)) {
                    if (!_.isEmpty(response.ResponseData.Bank)) $('select[id$="drpBank"]').val(response.ResponseData.Bank.Value).selectmenu("refresh");

                    fastdeposit.toogleBank($('select[id$="drpBank"]').val());
                    $('input[id$="txtBankName"]').val(response.ResponseData.BankName);
                    $('input[id$="txtAccountName"]').val(response.ResponseData.AccountName);
                    $('input[id$="txtAccountNumber"]').val(response.ResponseData.AccountNumber);
                }
            });
        }

    };

    fastdeposit.toogleBank = function (bankId) {
        if (bankId && _.isEqual(bankId.toUpperCase(), "OTHER")) {
            $('#divBankName').show();
        } else {
            $('#divBankName').hide();
        }
    };

    fastdeposit.createDeposit = function () {
        var _self = this;
        var params = _self.getUrlVars();
        var data = {
            Amount: params.Amount,
            Bank: { Text: params.BankText, Value: params.BankValue },
            AccountName: params.AccountName,
            AccountNumber: params.AccountNumber,
            SystemBank: { Text: params.SystemBankText, Value: params.SystemBankValue },
            BankName: params.BankName,
            ReferenceId: params.ReferenceId,
            DepositChannel: { Text: params.DepositChannelText, Value: params.DepositChannelValue },
            DepositDateTime: params.DepositDateTime.replace('+', ' '),
        };

        _self.methodId = params.MethodId;
        _self.changeRoute();
        _self.deposit(data, function(response) {
                switch (response.ResponseCode) {
                case 1:
                    w88Mobile.PostPaymentForm.createv2(response.ResponseData.FormData, response.ResponseData.PostUrl, "body");
                    w88Mobile.PostPaymentForm.submit();

                    fastdeposit.toogleBank($('select[id$="drpBank"]').val());
                    break;
                default:
                    if (_.isArray(response.ResponseMessage))
                        w88Mobile.Growl.shout(w88Mobile.Growl.bulletedList(response.ResponseMessage));
                    else
                        w88Mobile.Growl.shout(response.ResponseMessage);

                    break;
                }
            },
            function() {
                GPInt.prototype.HideSplash();
            });
    };

    return fastdeposit;
}

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
