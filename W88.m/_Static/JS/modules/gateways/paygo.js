window.w88Mobile.Gateways.PayGoV2 = PayGoV2();
var _w88_paygo = window.w88Mobile.Gateways.PayGoV2;

function PayGoV2() {

    var paygo;
    var defaultBank;

    try {
        paygo = Object.create(new w88Mobile.Gateway(_w88_paymentSvcV2));
    } catch (err) {
        paygo = {};
    }

    paygo.init = function () {

        setTranslations();

        function setTranslations() {

            if (_w88_contents.translate("LABEL_SYSTEM_ACCOUNT") != "LABEL_SYSTEM_ACCOUNT") {

                $('label[id$="lblDepositAmount"]').text(_w88_contents.translate("LABEL_AMOUNT"));
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

        _w88_paymentSvcV2.SendDeposit("/banks/money/110394", "GET", "", function (response) {
            if (!_.isEqual(response.ResponseCode, 0)) {
                $('select[id$="drpSystemAccount"]').append($("<option></option>").attr("value", "-1").text(defaultBank));
                $('select[id$="drpSystemAccount"]').val("-1").change();

                _.forEach(response.ResponseData, function (data) {
                    $('select[id$="drpSystemAccount"]').append($("<option></option>").attr("value", data.Value).text(data.Text));
                });
            }
        });
    };

    paygo.createDeposit = function() {
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
                    w88Mobile.Growl.shout(w88Mobile.Growl.bulletedList(response.ResponseMessage), function () { window.close(); });
                else
                    w88Mobile.Growl.shout(response.ResponseMessage, function () { window.close(); });
            },
            function() {
                pubsub.publish('stopLoadItem', { selector: "" });
            });
    };

    return paygo;
}

function PayGo() {

    var defaultBank = "";

    var paygo = {
        InitDeposit: initDeposit,
        InitWithdraw: initWithdraw,
    };

    return paygo;

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

    function initDeposit() {
        translations();
        loadMoneyAccount();
    }

    function initWithdraw() {
        countryphone();
    }

    function translations() {
        send("/contents", "GET", "", "", function (response) {
            if (response && _.isEqual(response.ResponseCode, 1)) {
                setTranslation(response.ResponseData);
            }
        }, "");
    }

    function setTranslation(data) {
        $('#lblSystemAccount').text(data.LABEL_SYSTEM_ACCOUNT);
        $('#lblDepositDateTime').text(data.LABEL_DEPOSIT_DATETIME);
        $('#lblReferenceId').text(data.LABEL_REFERENCE_ID);

        defaultBank = data.LABEL_SELECT_DEFAULT + " " + data.LABEL_SYSTEM_ACCOUNT;

        $('#lblReferenceId').text(data.LABEL_REFERENCE_ID);
    }

    function loadMoneyAccount() {
        send("/banks/money/110394", "GET", "",
            "",
            function (response) {
                $('#drpSystemAccount').append($("<option></option>").attr("value", "-1").text(defaultBank));

                $('#drpSystemAccount').val("-1").selectmenu("refresh");

                _.forEach(response.ResponseData, function (data) {
                    $('#drpSystemAccount').append($("<option></option>").attr("value", JSON.stringify(data)).text(data.Text))
                })
            },
            ""
        );
    }

    function countryphone() {
        send("/countryphonelist", "GET", "",
            "",
            function (response) {
                _.forEach(response.ResponseData.PhoneList, function (data) {
                    $('#drpContactCountry').append($("<option></option>").attr("value", data.Value).text(data.Text))
                })

                $('#drpContactCountry').val(response.ResponseData.PhoneSelected).selectmenu("refresh");
            },
            ""
        );
    }

}

window.w88Mobile.Gateways.PayGo = PayGo()