window.w88Mobile.Gateways.BankTransferv2 = BankTransferv2();
var _w88_banktransfer = window.w88Mobile.Gateways.BankTransferv2;

function BankTransferv2() {
    var banktransfer;
    var methodId;
    var isDeposit;
    try {
        banktransfer = Object.create(new w88Mobile.Gateway(_w88_paymentSvcV2));
    } catch (err) {
        banktransfer = {};
    }

    banktransfer.defaultSelect = "";

    banktransfer.init = function (type) {
        isDeposit = _.includes(type.toLowerCase(), "deposit");
        banktransfer.setTranslations(isDeposit);

        pubsub.subscribe('bankingDetails', onBankingDetailsLoaded);

        if (isDeposit === true) {
            banktransfer.getSystemAccount();
            banktransfer.getBank();
            banktransfer.getDepositChannel();
        }
        else {
            banktransfer.getBank();

            if (_.isEqual(siteCookie.getCookie('currencyCode'), 'VND'))
                banktransfer.getSecondaryBank();

            banktransfer.getCountryPhoneList();
        }
    };

    function onBankingDetailsLoaded(topic, data) {
        if (data === false) {
            if (isDeposit === true)
                banktransfer.getDepositLastTransaction();
            else
                banktransfer.getWithdrawalLastTransaction();
        }

    }

    banktransfer.setTranslations = function (isDeposit) {
        $('label[id$="lblBank"]').text(_w88_contents.translate("LABEL_BANK"));
        $('label[id$="lblAccountName"]').text(_w88_contents.translate("LABEL_ACCOUNT_NAME"));
        $('label[id$="lblAccountNumber"]').text(_w88_contents.translate("LABEL_ACCOUNT_NUMBER"));
        $('label[id$="lblBankName"]').text(_w88_contents.translate("LABEL_BANK_NAME"));

        if (isDeposit) {
            if (_.isEqual(siteCookie.getCookie('language').toLowerCase(), 'vi-vn') && _.isEqual(siteCookie.getCookie('currencyCode'), 'VND')) {
                $(".pay-note").show();
                $("#paymentNote").text(_w88_contents.translate("LABEL_PAYMENT_NOTE"));
                $("#paymentNoteContent").html(_w88_contents.translate("LABEL_MSG_110101"));
            } else if (siteCookie.getCookie('currencyCode') == 'KRW') {
                $(".depositdatetime").hide();
            }

            $('label[id$="lblReferenceId"]').text(_w88_contents.translate("LABEL_REFERENCE_ID"));
            $('label[id$="lblSystemAccount"]').text(_w88_contents.translate("LABEL_BANK_ACCOUNT"));
            $('label[id$="lblDepositDateTime"]').text(_w88_contents.translate("LABEL_DEPOSIT_DATETIME"));
            $('label[id$="lblDepositChannel"]').text(_w88_contents.translate("LABEL_DEPOSIT_CHANNEL"));

            $('select[id$="drpBank"]').change(function () {
                banktransfer.toogleBank(this.value);
            });

            $('select[id$="drpDepositChannel"]').change(function () {
                banktransfer.toogleDepositChannel(this.value);
            });
        }
        else {
            $('label[id$="lblSecondBank"]').text(_w88_contents.translate("LABEL_BANK_OTHER"));
            $('label[id$="lblBankLocation"]').text(_w88_contents.translate("LABEL_BANK_LOCATION"));
            $('label[id$="lblBranch"]').text(_w88_contents.translate("LABEL_BANK_BRANCH"));
            $('label[id$="lblBankBranch"]').text(_w88_contents.translate("LABEL_BANK_BRANCH"));
            $('label[id$="lblAddress"]').text(_w88_contents.translate("LABEL_BANK_ADDRESS"));
            $('label[id$="lblContact"]').text(_w88_contents.translate("LABEL_MOBILE_NUMBER"));
            $('input[id$="txtPhoneNumber"]').mask('999999999999');

            $('select[id$="drpBank"]').change(function () {
                banktransfer.toggleBankWidraw(this.value, siteCookie.getCookie('currencyCode'));
            });

            if (_.isEqual(siteCookie.getCookie('currencyCode'), 'VND')) {


                $('select[id$="drpSecondaryBank"]').change(function () {
                    banktransfer.toggleSecondaryBankWidraw(this.value, $('select[id$="drpBankLocation"]').val());
                });

                $('select[id$="drpBankLocation"]').change(function () {
                    if (this.value != '-1') {
                        banktransfer.loadBankBranchWidraw(this.value);
                    }
                });
            }
        }
    };

    banktransfer.getSystemAccount = function (id) {
        var _self = this;

        _self.send("/Banks/system", "GET", "", function (response) {
            var banks = response.ResponseData;
            $('select[id$="drpSystemAccount"]').append($('<option>').text(_w88_contents.translate("LABEL_SELECT_DEFAULT")).attr('value', '-1'));

            _.forOwn(banks, function (data) {
                $('select[id$="drpSystemAccount"]').append($('<option>').text(data.Text).attr('value', data.Value));
            });
        });
    };

    banktransfer.getBank = function (id) {
        var _self = this;

        _self.send("/Banks/member", "GET", "", function (response) {
            $('select[id$="drpBank"]').append($('<option>').text(_w88_contents.translate("LABEL_SELECT_DEFAULT")).attr('value', '-1'));

            _.forOwn(response.ResponseData, function (data) {
                $('select[id$="drpBank"]').append($('<option>').text(data.Text).attr('value', data.Value));
            });

            banktransfer.getUserBank();
        });
    };

    banktransfer.getUserBank = function (id) {
        var _self = this;

        _self.send("/user/banks", "GET", "", function (response) {
            if (!_.isEqual(response.ResponseCode, 0)) {
                if (!_.isEmpty(response.ResponseData.Bank)) $('select[id$="drpBank"]').val(response.ResponseData.Bank.Value).change();

                banktransfer.toogleBank($('select[id$="drpBank"]').val());
                $('input[id$="txtBankName"]').val(response.ResponseData.BankName);
                $('input[id$="txtBankBranch"]').val(response.ResponseData.BankBranch);
                $('input[id$="txtAddress"]').val(response.ResponseData.BankAddress);
                $('input[id$="txtAccountName"]').val(response.ResponseData.AccountName);
                $('input[id$="txtAccountNumber"]').val(response.ResponseData.AccountNumber);

                pubsub.publish('bankingDetails', response.ResponseData.IsPreferred);
            }
        });
    };

    banktransfer.getDepositChannel = function (id) {
        var _self = this;

        _self.send("/depositchannel", "GET", "", function (response) {
            $('select[id$="drpDepositChannel"]').append($('<option>').text(_w88_contents.translate("LABEL_SELECT_DEFAULT")).attr('value', '-1'));

            _.forOwn(response.ResponseData, function (data) {
                if (_.isEqual(siteCookie.getCookie('currencyCode'), 'THB') && _.isEqual(data.Value, 'cBanking'))
                    return;

                $('select[id$="drpDepositChannel"]').append($('<option>').text(data.Text).attr('value', data.Value));
            });
        });
    };

    banktransfer.getSecondaryBank = function (id) {
        var _self = this;

        _self.send("/Banks/member/secondary", "GET", "", function (response) {
            if (!_.isEqual(response.ResponseCode, 0)) {
                $('select[id$="drpSecondaryBank"]').append($('<option>').text(banktransfer.defaultSelect).attr('value', '-1'));

                _.forOwn(response.ResponseData, function (data) {
                    $('select[id$="drpSecondaryBank"]').append($('<option>').text(data.Text).attr('value', data.Value));
                });
            }
        });
    };

    banktransfer.getCountryPhoneList = function () {
        var _self = this;

        _self.send("/CountryPhoneList", "GET", "", function (response) {
            if (!_.isEqual(response.ResponseCode, 0)) {
                $('select[id$="drpCountryCode"]').append($('<option>').text(_w88_contents.translate("LABEL_SELECT_DEFAULT")).attr('value', '-1'));

                _.forEach(response.ResponseData.PhoneList, function (data) {
                    $('select[id$="drpCountryCode"]').append($("<option></option>").attr("value", data.Value).text(data.Text));
                });

                $('select[id$="drpCountryCode"]').val(response.ResponseData.PhoneSelected).change();
            }
        });
    };

    banktransfer.toogleBank = function (id) {
        if (!_.isEmpty(id)) {
            if (_.isEqual(id.toUpperCase(), "OTHER")) {
                banktransfer.showBankName();
            } else {
                banktransfer.hideBankName();
            }
        }
    };

    banktransfer.showBankName = function () {
        $('.bankname').show();
        $('input[id$="txtBankName"]').attr({ required: '', 'data-require': '' });
        $('#form1').validator('update')
    };

    banktransfer.hideBankName = function () {
        $('.bankname').hide();
        $('input[id$="txtBankName"]').removeAttr('required data-require');
        $('#form1').validator('update')
    };

    banktransfer.toogleDepositChannel = function (id) {
        if (!_.isEmpty(id)) {
            if ((_.isEqual(id.toUpperCase(), "CDM")) || _.isEqual(id.toUpperCase(), "CBANKING")) {
                banktransfer.hideAccountNumber();
            } else {
                banktransfer.showAccountNumber();
            }
        }
    };

    banktransfer.showAccountNumber = function () {
        $('.accountnumber').show();
        $('input[id$="txtAccountNumber"]').attr({ required: '', 'data-require': '' });
        $('#form1').validator('update')
    };

    banktransfer.hideAccountNumber = function () {
        $('.accountnumber').hide();
        $('input[id$="txtAccountNumber"]').removeAttr('required data-require');
        $('#form1').validator('update')
    };

    banktransfer.getDepositLastTransaction = function () {
        var _self = this;

        _self.send("/payments/deposit/lasttrans", "GET", "", function (response) {
            switch (response.ResponseCode) {
                case 1:
                    $('select[id$="drpBank"]').val(response.ResponseData.BankId);
                    $('input[id$="txtBankName"]').val(response.ResponseData.BankName);
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
        });
    }

    banktransfer.createDeposit = function (form, data, methodId) {
        var _self = this;

        _self.methodId = methodId;
        _self.offlineDeposit(form, data);
    };

    banktransfer.toggleBankWidraw = function (bankId, currency) {
        if (bankId == "OTHER") {
            if (currency == "VND") {
                banktransfer.setWithTransactStateWidraw();
                $('select[id$="drpSecondaryBank').val('-1').change();
                banktransfer.disableLocationWidraw();
                banktransfer.disableBranchWidraw();
            } else {
                banktransfer.showBankName();
            }
        } else {
            banktransfer.setOriginStateWidraw();
        }
    };

    banktransfer.setOriginStateWidraw = function () {
        banktransfer.showBranch();
        banktransfer.showAddress();

        banktransfer.hideBankName();
        banktransfer.hideOtherBank();
        banktransfer.hideLocation();
        banktransfer.hideBranchList();
    };

    banktransfer.showOtherBank = function () {
        $('.otherbank').show();
        $('select[id$="drpSecondaryBank').attr({ required: '', 'data-selectequals': '-1' });
        $('#form1').validator('update')
    };

    banktransfer.hideOtherBank = function () {
        $('.otherbank').hide();
        $('select[id$="drpSecondaryBank"]').removeAttr('required data-selectequals');
        $('#form1').validator('update')
    }

    banktransfer.showBranchList = function () {
        $('.branchlist').show();
        $('select[id$="drpBankBranchList').attr({ required: '', 'data-selectequals': '-1' });
        $('#form1').validator('update')
    };

    banktransfer.hideBranchList = function () {
        $('.branchlist').hide();
        $('select[id$="drpBankBranchList"]').removeAttr('required data-selectequals');
        $('#form1').validator('update')
    }

    banktransfer.showLocation = function () {
        $('.location').show();
        $('select[id$="drpBankLocation').attr({ required: '', 'data-selectequals': '-1' });
        $('#form1').validator('update')
    };

    banktransfer.hideLocation = function () {
        $('.location').hide();
        $('select[id$="drpBankLocation"]').removeAttr('required data-selectequals');
        $('#form1').validator('update')
    }

    banktransfer.showBranch = function () {
        $('.branch').show();
        $('input[id$="txtBankBranch"]').attr({ required: '', 'data-require': '' });
        $('#form1').validator('update')
    };

    banktransfer.hideBranch = function () {
        $('.branch').hide();
        $('input[id$="txtBankBranch"]').removeAttr('required data-require');
        $('#form1').validator('update')
    };

    banktransfer.showAddress = function () {
        $('.address').show();
        $('input[id$="txtAddress"]').attr({ required: '', 'data-require': '' });
        $('#form1').validator('update')
    };

    banktransfer.hideAddress = function () {
        $('.address').hide();
        $('input[id$="txtAddress"]').removeAttr('required data-require');
        $('#form1').validator('update')
    };

    banktransfer.setWithTransactStateWidraw = function () {
        banktransfer.hideBankName();
        banktransfer.hideBranch();
        banktransfer.hideAddress();

        banktransfer.showOtherBank();
        banktransfer.showLocation();
        banktransfer.showBranchList();
    };

    banktransfer.disableLocationWidraw = function () {
        $('select[id$="drpBankLocation"]').attr('disabled', 'disabled');
        $('select[id$="drpBankLocation"]').append($('<option>').text(_w88_contents.translate("LABEL_SELECT_DEFAULT")).attr('value', '-1'));
    };

    banktransfer.enableLocationWidraw = function () {
        $('select[id$="drpBankLocation"]').removeAttr('disabled', 'disabled');
    };

    banktransfer.toggleSecondaryBankWidraw = function (bankId, bankLocationId) {
        banktransfer.setWithTransactStateWidraw();
        banktransfer.disableLocationWidraw();
        banktransfer.disableBranchWidraw();

        if (bankId == "OTHER") {
            banktransfer.setOriginStateWidraw();
            banktransfer.showOtherBank();
            banktransfer.showBankName();
        } else if (bankId != "-1") {
            banktransfer.loadBankLocationWidraw(bankLocationId);
            banktransfer.disableBranchWidraw();
        }
    };

    banktransfer.disableBranchWidraw = function () {
        $('select[id$="drpBankBranchList"]').attr('disabled', 'disabled');
        $('select[id$="drpBankBranchList"]').append($('<option>').text(_w88_contents.translate("LABEL_SELECT_DEFAULT")).attr('value', '-1'));
    };

    banktransfer.enableBranchWidraw = function () {
        $('select[id$="drpBankBranchList"]').removeAttr('disabled', 'disabled');
    };

    banktransfer.loadBankLocationWidraw = function (blId) {
        var bankId = $('select[id$="drpSecondaryBank"]').val();

        if (bankId != '-1') {
            _w88_paymentSvcV2.Send("/Banks/member/location/" + bankId, "GET", { selector: "location" }, function (response) {
                if (!_.isEqual(response.ResponseCode, 0)) {

                    $('select[id$="drpBankLocation"]').append($('<option>').text(_w88_contents.translate("LABEL_SELECT_DEFAULT")).attr('value', '-1'));
                    $('select[id$="drpBankLocation"]').children('option:not(:first)').remove();

                    _.forOwn(response.ResponseData, function (data) {
                        $('select[id$="drpBankLocation"]').append($('<option>').text(data.Text).attr('value', data.Value));
                    });

                    banktransfer.enableLocationWidraw();
                }

            });
        }
    };

    banktransfer.loadBankBranchWidraw = function (bankLocationId) {
        var bankId = $('select[id$="drpSecondaryBank"]').val();

        if (bankId != '-1') {
            _w88_paymentSvcV2.Send("/Banks/member/branch/" + bankId + "/" + bankLocationId, "GET", { selector: "branch" }, function (response) {
                if (!_.isEqual(response.ResponseCode, 0)) {

                    $('select[id$="drpBankBranchList"]').append($('<option>').text(banktransfer.defaultSelect).attr('value', '-1'));
                    $('select[id$="drpBankBranchList"]').children('option:not(:first)').remove();

                    $.each(response.ResponseData, function (index, v) {
                        $('select[id$="drpBankBranchList"]').append($('<option>').text(v.Text).attr('value', v.Value));
                    });

                    banktransfer.enableBranchWidraw();
                }
            });
        }
    };

    banktransfer.getWithdrawalLastTransaction = function () {
        var _self = this;

        _self.send("/payments/withdrawal/lasttrans", "GET", "", function (response) {
            switch (response.ResponseCode) {
                case 1:
                    $('select[id$="drpBank"]').val(response.ResponseData.BankId);
                    $('input[id$="txtBankName"]').val(response.ResponseData.BankName);
                    $('input[id$="txtBankBranch"]').val(response.ResponseData.BankBranch);
                    $('input[id$="txtAddress"]').val(response.ResponseData.BankAddress);
                    $('input[id$="txtAccountName"]').val(response.ResponseData.AccountName);
                    $('input[id$="txtAccountNumber"]').val(response.ResponseData.AccountNumber);

                    if (!_.isEmpty(response.ResponseData.CountryCode))
                        $('select[id$="drpCountryCode"]').val(response.ResponseData.CountryCode);

                    $('input[id$="txtPhoneNumber"]').val(response.ResponseData.Phone);
                    break;
                default:
                    if (_.isArray(response.ResponseMessage))
                        w88Mobile.Growl.shout(w88Mobile.Growl.bulletedList(response.ResponseMessage));
                    else
                        w88Mobile.Growl.shout(response.ResponseMessage);

                    break;
            }
        });
    }

    banktransfer.createWithdraw = function (data, methodId) {
        var _self = this;
        _self.methodId = methodId;
        _self.withdraw(data);
    };

    return banktransfer;
}