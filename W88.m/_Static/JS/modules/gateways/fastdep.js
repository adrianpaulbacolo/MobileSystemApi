function FastDeposit() {

    var fastdeposit = {
        GetBankDetails: get,
        ToogleBank: toogleBank
    };

    return fastdeposit;

    function get() {

        setTranslations();

        function setTranslations() {

            if (Cookies().getCookie('language').toLowerCase() == 'vn' && Cookies().getCookie('currencyCode').toLowerCase() == 'vnd') {
                if (_w88_contents.translate("LABEL_PAYMENT_NOTE_FASTDEPOSIT") != "LABEL_PAYMENT_NOTE_FASTDEPOSIT") {
                    $("#paymentNoteContent").text(_w88_contents.translate("LABEL_PAYMENT_NOTE_FASTDEPOSIT"));
                } else {
                    window.setInterval(function() {
                        setTranslations();
                    }, 500);
                }
            }

            _w88_paymentSvc.SendDeposit("/user/banks", "GET", "", function (response) {
                if (!_.isEqual(response.ResponseCode, 0)) {
                    if (!_.isEmpty(data.Bank)) $('select[id$="ContentPlaceHolder1_ContentPlaceHolder2_drpBank"]').val(data.Bank.Value).selectmenu("refresh");

                    toogleBank($('select[id$="ContentPlaceHolder1_ContentPlaceHolder2_drpBank"]')).val();
                    $('input[id$="ContentPlaceHolder1_ContentPlaceHolder2_txtBankName"]').val(data.BankName);
                    $('input[id$="ContentPlaceHolder1_ContentPlaceHolder2_txtAccountName"]').val(data.AccountName);
                    $('input[id$="ContentPlaceHolder1_ContentPlaceHolder2_txtAccountNumber"]').val(data.AccountNumber);
                }
            });
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