window.w88Mobile.Gateways.BankTransferv2 = BankTransferv2();
var _w88_banktransfer = window.w88Mobile.Gateways.BankTransferv2;

function BankTransferv2() {

    var banktransfer;

    try {
        banktransfer = Object.create(new w88Mobile.Gateway(_w88_paymentSvcV2));
    } catch (err) {
        banktransfer = {};
    }

    banktransfer.init = function () {

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

            _w88_paymentSvcV2.Send("/user/banks", "GET", "", function (response) {
                if (!_.isEqual(response.ResponseCode, 0)) {
                    if (!_.isEmpty(response.ResponseData.Bank)) $('select[id$="drpBank"]').val(response.ResponseData.Bank.Value).change();

                    _w88_banktransfer.toogleBank($('select[id$="drpBank"]').val());
                    $('input[id$="txtBankName"]').val(response.ResponseData.BankName);
                    $('input[id$="txtAccountName"]').val(response.ResponseData.AccountName);
                    $('input[id$="txtAccountNumber"]').val(response.ResponseData.AccountNumber);
                }
            });
        }

    };

    banktransfer.toogleBank = function (bankId) {
        if (bankId && _.isEqual(bankId.toUpperCase(), "OTHER")) {
            $('#divBankName').show();
        } else {
            $('#divBankName').hide();
        }
    };

    banktransfer.createDeposit = function () {
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
        _self.deposit(data, function (response) {

            if (_.isArray(response.ResponseMessage))
                w88Mobile.Growl.shout(w88Mobile.Growl.bulletedList(response.ResponseMessage), _self.shoutCallback);
            else
                w88Mobile.Growl.shout(response.ResponseMessage, _self.shoutCallback);

            _w88_banktransfer.toogleBank($('select[id$="drpBank"]').val());

        },
            function () {
                pubsub.publish('stopLoadItem', { selector: "" });
            });
    };

    return banktransfer;
}