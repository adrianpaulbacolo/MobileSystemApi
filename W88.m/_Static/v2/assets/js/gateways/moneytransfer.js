window.w88Mobile.Gateways.MoneyTransfer = MoneyTransfer();
var _w88_moneytransfer = window.w88Mobile.Gateways.MoneyTransfer;

function MoneyTransfer() {

    var moneytransfer;
    var methodId;
    try {
        moneytransfer = Object.create(new w88Mobile.Gateway(_w88_paymentSvcV2));
    } catch (err) {
        moneytransfer = {};
    }

    moneytransfer.init = function (type, id) {
        methodId = id;

        var isDeposit = _.includes(type.toLowerCase(), "deposit");
        moneytransfer.setTranslations(isDeposit);

        if (isDeposit) {
            moneytransfer.getDepositLastTransaction();
            moneytransfer.getSystemAccount();
        }
        else {
            moneytransfer.getWithdrawalLastTransaction();
            moneytransfer.getCountryPhoneList();
        }
    };

    moneytransfer.setTranslations = function (isDeposit) {
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
            $('input[id$="txtPhoneNumber"]').mask('999999999999');
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
    }

    moneytransfer.getSystemAccount = function () {
        var _self = this;

        _self.send("/Banks/money/" + methodId, "GET", "", function (response) {
            if (!_.isEqual(response.ResponseCode, 0)) {
                $('select[id$="drpSystemAccount"]').append($("<option></option>").attr("value", "-1").text(_w88_contents.translate("LABEL_SELECT_DEFAULT")));

                _.forEach(response.ResponseData, function (data) {
                    $('select[id$="drpSystemAccount"]').append($("<option></option>").attr("value", data.Value).text(data.Text))
                })

                if (response.ResponseData.length > 0)
                    moneytransfer.showSystemAccount();
                else
                    moneytransfer.hideSystemAccount();
            }
        },
      function () {
          GPInt.prototype.HideSplash();
      });
    }

    moneytransfer.showSystemAccount = function () {
        $('.systemAccount').show();
        $('select[id$="drpSystemAccount').attr({ required: '', 'data-selectequals': '-1' });
        $('#form1').validator('update');
    };

    moneytransfer.hideSystemAccount = function () {
        $('.systemAccount').hide();
        $('select[id$="drpSystemAccount"]').removeAttr('required data-selectequals');
        $('#form1').validator('update');
    }

    moneytransfer.getCountryPhoneList = function () {
        var _self = this;

        _self.send("/CountryPhoneList", "GET", "", function (response) {
            if (!_.isEqual(response.ResponseCode, 0)) {
                $('select[id$="drpContactCountry"]').append($("<option></option>").attr("value", "-1").text(_w88_contents.translate("LABEL_SELECT_DEFAULT")));

                _.forEach(response.ResponseData.PhoneList, function (data) {
                    $('select[id$="drpContactCountry"]').append($("<option></option>").attr("value", data.Value).text(data.Text));
                });

                $('select[id$="drpContactCountry"]').val(response.ResponseData.PhoneSelected).change();
            }
        });
    };

    moneytransfer.getDepositLastTransaction = function () {
        var _self = this;

        _self.send("/payments/deposit/lasttrans/moneytransfer/" + methodId, "GET", "", function (response) {
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

    moneytransfer.getWithdrawalLastTransaction = function () {
        var _self = this;

        _self.send("/payments/withdrawal/lasttrans/moneytransfer/" + methodId, "GET", "", function (response) {
            switch (response.ResponseCode) {
                case 1:
                    $('input[id$="txtAccountName"]').val(response.ResponseData.AccountName);
                    $('input[id$="txtAccountNumber"]').val(response.ResponseData.AccountNumber);
                    $('input[id$="txtContact"]').val(response.ResponseData.Phone);

                    $('select[id$="drpContactCountry"]').val(response.ResponseData.CountryCode).change();
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

    moneytransfer.createDeposit = function (form, data) {
        var _self = this;

        _self.methodId = methodId;
        _self.offlineDeposit(form, data);
    };

    moneytransfer.createWithdraw = function (data) {
        var _self = this;
        _self.methodId = methodId;
        _self.withdraw(data);
    };

    return moneytransfer;
}