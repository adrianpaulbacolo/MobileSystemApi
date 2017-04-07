window.w88Mobile.Gateways.DaddyPayV2 = DaddyPayV2();
var _w88_daddypay = window.w88Mobile.Gateways.DaddyPayV2;

function DaddyPayV2() {

    var daddypay;

    try {
        daddypay = Object.create(new w88Mobile.Gateway(_w88_paymentSvcV2));
    } catch (err) {
        daddypay = {};
    }

    daddypay.init = function (gateway, getBank) {

        setTranslations();

        function setTranslations() {
            if (_w88_contents.translate("LABEL_PAYMENT_NOTE") != "LABEL_PAYMENT_NOTE") {

                $('label[id$="lbldrpDepositAmount"]').text(_w88_contents.translate("LABEL_AMOUNT"));
                $('label[id$="lblBank"]').text(_w88_contents.translate("LABEL_BANK"));
                $('label[id$="lblAccountName"]').text(_w88_contents.translate("LABEL_ACCOUNT_NAME"));
                $('label[id$="lblAccountNumber"]').text(_w88_contents.translate("LABEL_ACCOUNT_NUMBER"));

            } else {
                window.setInterval(function () {
                    setTranslations();
                }, 500);
            }
        }

        if (getBank) {
            _w88_paymentSvcV2.Send("/Banks/vendor/" + gateway + "/" + new Cookies().getCookie("currencyCode"), "GET", "", function (response) {
                var banks = response.ResponseData;
                var defaultSelect = _w88_contents.translate("LABEL_SELECT_DEFAULT");
                $('select[id$="drpBank"]').append($('<option>').text(defaultSelect).attr('value', '-1'));
                $('select[id$="drpBank"]').val("-1").change();

                _.forOwn(banks, function (data) {
                    $('select[id$="drpBank"]').append($('<option>').text(data.Text).attr('value', data.Value));
                });
            });
        }
    };

    daddypay.getnickname = function () {
        var data = {
            action: "getNickname",
            nickname: ""
        };

        _w88_paymentSvcV2.Send("/user/wechatnickname/", "GET", data, function (response) {
            if (response && _.isEqual(response.ResponseCode, 1)) {
                $('#txtAccountName').val(response.responseData);
                $('#hfWCNickname').val(response.responseData); //store original nickname if any.    
            }
            
        });
    };

    daddypay.togglePayment = function (bankId) {
    
        $("#txtAccountName").val('');
        if (bankId == "40") { //WeChat
            $("#amount").hide();
            $("#drpAmount").show();
            $("#accountNo").hide();
            _w88_daddypay.getnickname();
        }
        else { //QR
            $("#amount").show();
            $("#drpAmount").hide();
            $("#accountNo").show();
        }
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
                            window.open(response.ResponseData.DummyURL, '_blank');
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