window.w88Mobile.Gateways.MoneyTransfer = MoneyTransfer();
var _w88_moneytransfer = window.w88Mobile.Gateways.MoneyTransfer;

function MoneyTransfer() {

    var moneytransfer;
    var defaultBank;

    try {
        moneytransfer = Object.create(new w88Mobile.Gateway(_w88_paymentSvcV2));
    } catch (err) {
        moneytransfer = {};
    }

    moneytransfer.init = function (methodId, getbanks) {

        setTranslations();

        function setTranslations() {

            if (_w88_contents.translate("LABEL_SYSTEM_ACCOUNT") != "LABEL_SYSTEM_ACCOUNT") {

                $('label[id$="lblSystemAccount"]').text(_w88_contents.translate("LABEL_SYSTEM_ACCOUNT"));
                $('label[id$="lblDepositDateTime"]').text(_w88_contents.translate("LABEL_DEPOSIT_DATETIME"));
                $('label[id$="lblReferenceId"]').text(_w88_contents.translate("LABEL_REFERENCE_ID"));
                $('label[id$="lblAccountName"]').text(_w88_contents.translate("LABEL_ACCOUNT_NAME"));
                $('label[id$="lblAccountNumber"]').text(_w88_contents.translate("LABEL_ACCOUNT_NUMBER"));
                defaultBank = _w88_contents.translate("LABEL_SELECT_DEFAULT") + " " + _w88_contents.translate("LABEL_SYSTEM_ACCOUNT");

            } else {
                window.setInterval(function () {
                    setTranslations();
                }, 500);
            }
        }

        if (getbanks) {
            _w88_paymentSvcV2.Send("/banks/money/" + methodId, "GET", "", function(response) {
                if (!_.isEqual(response.ResponseCode, 0)) {
                    $('select[id$="drpSystemAccount"]').append($("<option></option>").attr("value", "-1").text(defaultBank));
                    $('select[id$="drpSystemAccount"]').val("-1").change();

                    _.forEach(response.ResponseData, function(data) {
                        $('select[id$="drpSystemAccount"]').append($("<option></option>").attr("value", data.Value).text(data.Text));
                    });
                }
            });
        }
    };

    moneytransfer.countryphone = function () {
        _w88_paymentSvcV2.Send("/countryphonelist", "GET", "", function (response) {
            if (!_.isEqual(response.ResponseCode, 0)) {
                _.forEach(response.ResponseData.PhoneList, function(data) {
                    $('select[id$="drpContactCountry"]').append($("<option></option>").attr("value", data.Value).text(data.Text));
                });

                $('select[id$="drpContactCountry"]').val(response.ResponseData.PhoneSelected).change();
            }
        });
    };

    moneytransfer.createDeposit = function () {
        var _self = this;
        var params = _self.getUrlVars();
        var data = {
            Amount: params.Amount,
            AccountName: params.AccountName,
            AccountNumber: params.AccountNumber,
            SystemBank: { Text: params.SystemBankText, Value: params.SystemBankValue },
            ReferenceId: params.ReferenceId,
            DepositDateTime: params.DepositDateTime.replace('+', ' '),
        };

        _self.methodId = params.MethodId;
        _self.changeRoute();
        _self.deposit(data, function (response) {

            if (_.isArray(response.ResponseMessage))
                w88Mobile.Growl.shout(w88Mobile.Growl.bulletedList(response.ResponseMessage), _self.shoutCallback);
            else
                w88Mobile.Growl.shout(response.ResponseMessage,  _self.shoutCallback);
        },
            function () {
                pubsub.publish('stopLoadItem', { selector: "" });
            });
    };

    moneytransfer.createWithdrawal = function (data) {

        _w88_paymentSvcV2.Send("/countryphonelist", "GET", "", function (response) {
            if (!_.isEqual(response.ResponseCode, 0)) {
                _.forEach(response.ResponseData.PhoneList, function (data) {
                    $('select[id$="drpContactCountry"]').append($("<option></option>").attr("value", data.Value).text(data.Text));
                });

                $('select[id$="drpContactCountry"]').val(response.ResponseData.PhoneSelected).change();
            }
        });

        var _self = this;

        _self.methodId = params.MethodId;
        _self.changeRoute();
        _self.deposit(data, function (response) {

            if (_.isArray(response.ResponseMessage))
                w88Mobile.Growl.shout(w88Mobile.Growl.bulletedList(response.ResponseMessage),  _self.shoutCallback);
            else
                w88Mobile.Growl.shout(response.ResponseMessage,  _self.shoutCallback);
        },
            function () {
                pubsub.publish('stopLoadItem', { selector: "" });
            });
    };

    return moneytransfer;
}