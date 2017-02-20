function MoneyTransfer() {

    var methodId, selectDefault;
    var moneytransfer = {
        Initialize: init
    };

    return moneytransfer;

    function init(type, id) {
        methodId = id;

        var isDeposit = _.includes(type.toLowerCase(), "deposit");
        setTranslations(isDeposit, methodId);

        if (isDeposit){
            getDepositLastTransaction();
            getSystemAccount();
        }
        else {
            getWithdrawalLastTransaction();
            getCountryPhoneList();
        }
    }

    function setTranslations(isDeposit, methodId) {
        if (_w88_contents.translate("LABEL_DEPOSIT_AMOUNT") != "LABEL_DEPOSIT_AMOUNT") {
            selectDefault = _w88_contents.translate("LABEL_SELECT_DEFAULT");

            $('label[id$="lblAccountName"]').text(_w88_contents.translate("LABEL_ACCOUNT_NAME"));
            $('label[id$="lblAccountNumber"]').text(_w88_contents.translate("LABEL_ACCOUNT_NUMBER"));

            if (isDeposit) {
                $('label[id$="lblAmount"]').text(_w88_contents.translate("LABEL_DEPOSIT_AMOUNT"));
                $('label[id$="lblDepositDateTime"]').text(_w88_contents.translate("LABEL_DEPOSIT_DATETIME"));
                $('label[id$="lblSystemAccount"]').text(_w88_contents.translate("LABEL_SYSTEM_ACCOUNT"));
            }
            else {
                $('label[id$="lblAmount"]').text(_w88_contents.translate("LABEL_WITHDRAWAL_AMOUNT"));
                $('label[id$="lblContact"]').text(_w88_contents.translate("LABEL_MOBILE_NUMBER"));
            }

            switch (methodId) {
                case "1103132":
                    $('label[id$="lblReferenceId"]').text(_w88_contents.translate("LABEL_TRANSACTION_ID"));
                    break;
                case "110308":
                default:
                    $('label[id$="lblReferenceId"]').text(_w88_contents.translate("LABEL_REFERENCE_ID"));
                    break;

            }
        } else {
            window.setInterval(function () {
                setTranslations();
            }, 500);
        }
    }

    function getSystemAccount() {
        _w88_paymentSvc.SendDeposit("/Banks/money/" + methodId, "GET", "", function (response) {
            switch (response.ResponseCode) {
                case 1:
                    $('select[id$="drpSystemAccount"]').append($("<option></option>").attr("value", "-1").text(selectDefault));
                    $('select[id$="drpSystemAccount"]').val("-1").selectmenu("refresh");

                    _.forEach(response.ResponseData, function (data) {
                        $('select[id$="drpSystemAccount"]').append($("<option></option>").attr("value", data.Value).text(data.Text))
                    })
                    break;
                default:
                    if (_.isArray(response.ResponseMessage))
                        w88Mobile.Growl.shout(w88Mobile.Growl.bulletedList(response.ResponseMessage));
                    else
                        w88Mobile.Growl.shout(response.ResponseMessage);

                    break;
            }
        },
      function () {
          GPInt.prototype.HideSplash();
      });
    }

    function getCountryPhoneList() {
        _w88_paymentSvc.SendDeposit("/CountryPhoneList", "GET", "", function (response) {
            switch (response.ResponseCode) {
                case 1:
                    $('select[id$="drpContactCountry"]').append($("<option></option>").attr("value", "-1").text(selectDefault));

                    _.forEach(response.ResponseData.PhoneList, function (data) {
                        $('select[id$="drpContactCountry"]').append($("<option></option>").attr("value", data.Value).text(data.Text))
                    })

                    $('select[id$="drpContactCountry"]').val(response.ResponseData.PhoneSelected).selectmenu("refresh");
                    break;
                default:
                    if (_.isArray(response.ResponseMessage))
                        w88Mobile.Growl.shout(w88Mobile.Growl.bulletedList(response.ResponseMessage));
                    else
                        w88Mobile.Growl.shout(response.ResponseMessage);

                    break;
            }
        },
      function () {
          GPInt.prototype.HideSplash();
      });
    }

    function getDepositLastTransaction() {
        _w88_paymentSvc.SendDeposit("/payments/deposit/lasttrans/moneytransfer/" + methodId, "GET", "", function (response) {
            switch (response.ResponseCode) {
                case 1:
                    $('input[id$="txtAccountName"]').val(response.ResponseData.AccountName);
                    $('input[id$="txtAccountNumber"]').val(response.ResponseData.AccountNumber);
                    break;
                default:
                    if (_.isArray(response.ResponseMessage))
                        w88Mobile.Growl.shout(w88Mobile.Growl.bulletedList(response.ResponseMessage));
                    else
                        w88Mobile.Growl.shout(response.ResponseMessage);

                    break;
            }
        },
        function () {
            GPInt.prototype.HideSplash();
        });
    }

    function getWithdrawalLastTransaction() {
        _w88_paymentSvc.SendDeposit("/payments/withdrawal/lasttrans/moneytransfer/" + methodId, "GET", "", function (response) {
            switch (response.ResponseCode) {
                case 1:
                    $('input[id$="txtAccountName"]').val(response.ResponseData.AccountName);
                    $('input[id$="txtAccountNumber"]').val(response.ResponseData.AccountNumber);
                    $('input[id$="txtContact"]').val(response.ResponseData.Phone);

                    $('select[id$="drpContactCountry"]').val(response.ResponseData.CountryCode).selectmenu("refresh");
                    break;
                default:
                    if (_.isArray(response.ResponseMessage))
                        w88Mobile.Growl.shout(w88Mobile.Growl.bulletedList(response.ResponseMessage));
                    else
                        w88Mobile.Growl.shout(response.ResponseMessage);

                    break;
            }
        },
        function () {
            GPInt.prototype.HideSplash();
        });
    }
}

window.w88Mobile.Gateways.MoneyTransfer = MoneyTransfer();