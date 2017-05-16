window.w88Mobile.Gateways.DaddyPayV2 = DaddyPayV2();
var _w88_daddypay = window.w88Mobile.Gateways.DaddyPayV2;

function DaddyPayV2() {

    var daddypay;
    var methodId;
    try {
        daddypay = Object.create(new w88Mobile.Gateway(_w88_paymentSvcV2));
    } catch (err) {
        daddypay = {};
    }

    daddypay.init = function (id) {
        methodId = id;

        $('label[id$="lblBank"]').text(_w88_contents.translate("LABEL_BANK"));
        $('label[id$="lblAccountName"]').text(_w88_contents.translate("LABEL_ACCOUNT_NAME"));
        $('label[id$="lblAccountNumber"]').text(_w88_contents.translate("LABEL_ACCOUNT_NUMBER"));

        daddypay.getBanks();

        daddypay.setDaddyPayQR();
        $('select[id$="drpBank"]').change(function () {
            daddypay.togglePayment(this.value);
        });
    };

    daddypay.getnickname = function () {
        var data = {
            action: "getNickname",
            nickname: ""
        };

        _w88_paymentSvcV2.Send("/user/wechatnickname/", "GET", data, function (response) {
            if (response && _.isEqual(response.ResponseCode, 1)) {
                $('input[id$="txtAmount"]').val(response.responseData);
                $('[id$="hfWCNickname"]').val(response.responseData); //store original nickname if any.    
            }
        });
    };

    daddypay.getBanks = function () {
        _w88_paymentSvcV2.Send("/Banks/vendor/" + methodId, "GET", "", function (response) {
            $('select[id$="drpBank"]').append($('<option>').text(_w88_contents.translate("LABEL_SELECT_DEFAULT")).attr('value', '-1'));

            _.forOwn(response.ResponseData, function (data) {
                $('select[id$="drpBank"]').append($('<option>').text(data.Text).attr('value', data.Value));
            });
        });
    };

    daddypay.togglePayment = function (bankId) {
        $('input[id$="txtAccountName"]').val('');

        if (bankId == "40") { //WeChat
            daddypay.getDenomination();
            daddypay.hideAmount();
            daddypay.showAmountList();
            daddypay.hideAccountNumber();
            _w88_daddypay.getnickname();
        }
        else { //QR
            daddypay.setDaddyPayQR();
        }
    };

    daddypay.setDaddyPayQR = function () {
        daddypay.showAmount()
        daddypay.hideAmountList();
        daddypay.showAccountNumber();
    }

    daddypay.showAmount = function () {
        $('.amount').show();
        $('input[id$="txtAmount"]').attr({ required: '', 'data-require': '' });
        $('#form1').validator('update')
    };

    daddypay.hideAmount = function () {
        $('.amount').hide();
        $('input[id$="txtAmount"]').removeAttr('required data-require');
        $('#form1').validator('update')
    };

    daddypay.showAmountList = function () {
        $('.amountlist').show();
        $('select[id$="drpAmount').attr({ required: '', 'data-selectequals': '-1' });
        $('#form1').validator('update')
    };

    daddypay.hideAmountList = function () {
        $('.amountlist').hide();
        $('select[id$="drpAmount"]').removeAttr('required data-selectequals');
        $('#form1').validator('update')
    }

    daddypay.showAccountNumber = function () {
        $('.accountno').show();
        $('input[id$="txtAccountNumber"]').attr({ required: '', 'data-require': '' });
        $('#form1').validator('update')
    };

    daddypay.hideAccountNumber = function () {
        $('.accountno').hide();
        $('input[id$="txtAccountNumber"]').removeAttr('required data-require');
        $('#form1').validator('update')
    };

    daddypay.getDenomination = function () {
        _w88_paymentSvcV2.Send("/payments/denomination/" + methodId, "GET", "", function (response) {
            if (response && _.isEqual(response.ResponseCode, 1)) {
                $('select[id$="drpAmount"]').append($('<option>').text(defaultSelect).attr('value', '-1'));

                _.forOwn(response.ResponseData.Denominations, function (data) {
                    $('select[id$="drpAmount"]').append($('<option>').text(data.Text).attr('value', data.Value));
                });
            }
        }, undefined);
    };

    daddypay.createDeposit = function () {
        var _self = this;
        var params = _self.getUrlVars();
        var data = {
            ThankYouPage: params.ThankYouPage
        };

        if (!_.isUndefined(params.Amount)) {
            data.Amount = params.Amount;
        }

        if (!_.isUndefined(params.AccountName)) {
            data.AccountName = params.AccountName;
        }

        if (!_.isUndefined(params.AccountNumber)) {
            data.AccountNumber = params.AccountNumber;
        }


        if (!_.isUndefined(params.BankValue)) {
            data.Bank = { Text: params.BankText, Value: params.BankValue };
        }

        _self.methodId = params.MethodId;
        _self.changeRoute();
        _self.deposit(data, function (response) {
            switch (response.ResponseCode) {
                case 1:
                    if (response.ResponseData.VendorRedirectionUrl) {
                        window.open(response.ResponseData.VendorRedirectionUrl, '_blank');
                    } else {
                        if (response.ResponseData.PostUrl) {
                            w88Mobile.PostPaymentForm.createv2(response.ResponseData.FormData, response.ResponseData.PostUrl, "body");
                            w88Mobile.PostPaymentForm.submit();
                        } else if (response.ResponseData.DummyURL) {
                            w88Mobile.PostPaymentForm.createv2(response.ResponseData.FormData, response.ResponseData.DummyURL, "body");
                            w88Mobile.PostPaymentForm.submit();
                        }
                    }

                    break;
                default:
                    if (_.isArray(response.ResponseMessage))
                        w88Mobile.Growl.shout(w88Mobile.Growl.bulletedList(response.ResponseMessage), _self.shoutCallback);
                    else
                        w88Mobile.Growl.shout(response.ResponseMessage, _self.shoutCallback);

                    break;
            }
        },
        function () {
            pubsub.publish('stopLoadItem', { selector: "" });
        });
    }

    return daddypay;
}