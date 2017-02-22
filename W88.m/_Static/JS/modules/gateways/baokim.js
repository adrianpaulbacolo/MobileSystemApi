window.w88Mobile.Gateways.BaokimV2 = BaokimV2();
var _w88_baokim = window.w88Mobile.Gateways.BaokimV2;

function BaokimV2() {

    var baokim;

    try {
        baokim = Object.create(new w88Mobile.Gateway(_w88_paymentSvcV2));
    } catch (err) {
        baokim = {};
    }

    baokim.method = {};
    baokim.init = function (selectName) {

        setTranslations();
        function setTranslations() {
            if (_w88_contents.translate("LABEL_PAYMENT_NOTE") != "LABEL_PAYMENT_NOTE") {

                $('label[id$="lblBanks"]').text(_w88_contents.translate("LABEL_BANK"));
                $('label[id$="lblEmailAtm"]').text(_w88_contents.translate("LABEL_EMAIL"));
                $('label[id$="lblEmail"]').text(_w88_contents.translate("LABEL_EMAIL"));
                $('label[id$="lblDepositAmountAtm"]').text(_w88_contents.translate("LABEL_AMOUNT"));
                $('label[id$="lblDepositAmountWallet"]').text(_w88_contents.translate("LABEL_AMOUNT"));
                $('label[id$="lblContact"]').text(_w88_contents.translate("LABEL_CONTACT"));
                $('label[id$="lblWithdrawAmount"]').text(_w88_contents.translate("LABEL_AMOUNT"));
                $('label[id$="lblOtp"]').text(_w88_contents.translate("LABEL_OTP"));
                sessionStorage.setItem("noticeWallet", _w88_contents.translate("LABEL_NOTICEEWALLET"));
                sessionStorage.setItem("noticeAtm", _w88_contents.translate("LABEL_NOTICEATM"));

                $('#ewallet').hide();
                $('#atm').hide();
                $('#btnSubmitPlacement').hide();

                _w88_baokim.getBanks(selectName);

            } else {
                window.setInterval(function () {
                    setTranslations();
                }, 500);
            }
        }
    };

    baokim.createDeposit = function() {
        var _self = this;
        var params = _self.getUrlVars();
        var data = {};

        if (params.Method == 'EWALLET') {
            data = {
                Amount: params.Amount,
                ThankYouPage: params.ThankYouPage,
                Method: params.Method,
                Email: params.Email
            };
        } else {
            data = {
                Amount: params.Amount,
                ThankYouPage: params.ThankYouPage,
                Method: params.Method,
                Email: params.Email,
                Phone: params.Phone,
                Bank: { Text: params.BankText, Value: params.BankValue }
            };
        }

        _self.methodId = params.MethodId;
        _self.changeRoute();
        _self.deposit(data, function(response) {
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
                        w88Mobile.Growl.shout(w88Mobile.Growl.bulletedList(response.ResponseMessage));
                    else
                        w88Mobile.Growl.shout(response.ResponseMessage);

                    break;
                }
            },
            function() {
                pubsub.publish('stopLoadItem', { selector: "" });
            });
    };

    baokim.getBanks = function (selectName) {
        _w88_paymentSvcV2.SendDeposit("/banks/vendor/120272", "GET", "", function (response) {
            if (response && _.isEqual(response.ResponseCode, 1)) {
                $('select[id$="drpBank"]').append($('<option>').text(selectName).attr('value', '-1'));

                _.forOwn(response.ResponseData, function (data) {
                    $('select[id$="drpBank"]').append($('<option>').text(data.Text).attr('value', data.Value));
                });

                $('select[id$="drpBank"]').val('-1').change();
            }
        }, undefined);
    };

    baokim.validateWallet = function (data, transactionId) {

        _w88_paymentSvcV2.SendDeposit("/payments/120272/" + transactionId, "GET", data, function (response) {
            if (response && _.isEqual(response.ResponseCode, 1)) {
                switch (response.ResponseCode) {
                    case 1:
                        w88Mobile.Growl.shout(response.ResponseMessage);
                        window.location.replace('/v2/Funds.aspx');
                        break;
                    default:
                        w88Mobile.Growl.shout(response.ResponseMessage);
                        break;
                }
            }
        }, undefined);
    };

    return baokim;
}


function Baokim() {

    var gatewayId = "";
    var method = "";

    var baokim = {
        Initialize: init,
        deposit: deposit,
        withdraw: withdraw,
        getBanks: function(selectName) {
            return getBanks(selectName);
        },
        method: method,
        validateWallet: validateWallet
    };

    return baokim;

    function send(method, data, beforeSend, success, error, complete, url) {

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
            error: error,
            complete: complete
        });

    }

    function init() {
        setTranslations();
        function setTranslations() {
            if (_w88_contents.translate("LABEL_PAYMENT_NOTE") != "LABEL_PAYMENT_NOTE") {
                
                $('#ContentPlaceHolder1_ContentPlaceHolder2_lblBanks').html(_w88_contents.translate("LABEL_BANK"));
                $('#ContentPlaceHolder1_ContentPlaceHolder2_lblEmail').html(_w88_contents.translate("LABEL_EMAIL"));
                $('#ContentPlaceHolder1_ContentPlaceHolder2_lblDepositAmount').html(_w88_contents.translate("LABEL_FUNDS_DEPOSIT") + ' ' + _w88_contents.translate("LABEL_AMOUNT"));
                $('#ContentPlaceHolder1_ContentPlaceHolder2_lblContact').html(_w88_contents.translate("LABEL_CONTACT"));
                $('#ContentPlaceHolder1_ContentPlaceHolder2_lblWithdrawAmount').html(_w88_contents.translate("LABEL_FUNDS_WIDRAW") + ' ' + _w88_contents.translate("LABEL_AMOUNT"));
                $('#ContentPlaceHolder1_ContentPlaceHolder2_lblOtp').html(_w88_contents.translate("LABEL_OTP"));
                sessionStorage.setItem("noticeWallet", _w88_contents.translate("LABEL_NOTICEEWALLET"));
                sessionStorage.setItem("noticeAtm", _w88_contents.translate("LABEL_NOTICEATM"));

            } else {
                window.setInterval(function () {
                    setTranslations();
                }, 500);
            }
        }
    }

    function getBanks(selectName) {
        gatewayId = "120272";
        var url = w88Mobile.APIUrl + "/banks/vendor/" + gatewayId;

        var headers = {
            'Token': window.User.token,
            'LanguageCode': window.User.lang
        };

        $.ajax({
            type: "GET",
            url: url,
            headers: headers,
            success: function(d) {

                $('#ContentPlaceHolder1_ContentPlaceHolder2_drpBank').append($('<option>').text(selectName).attr('value', '-1'));

                _.forOwn(d.ResponseData, function(data) {
                    $('#ContentPlaceHolder1_ContentPlaceHolder2_drpBank').append($('<option>').text(data.Text).attr('value', data.Value));
                });

                $('#ContentPlaceHolder1_ContentPlaceHolder2_drpBank').val('-1').change();
            }
        });
    }

    // deposit
    function deposit(data, successCallback, errorCallback, completeCallback) {
        var url = w88Mobile.APIUrl + "/payments/120272";
        validate(data, "deposit");
        send("POST", data, function () { GPInt.prototype.ShowSplash(); }, successCallback, errorCallback, completeCallback, url);
    }

    // withdraw
    function withdraw(data, successCallback, errorCallback, completeCallback) {
        var url = w88Mobile.APIUrl + "/payments/220874";
        validate(data, "widraw");
        send("POST", data, function () { GPInt.prototype.ShowSplash(); }, successCallback, errorCallback, completeCallback, url);
    }

    function validateWallet(walletData, transactionId, successCallback, errorCallback, completeCallback) {
        var url = w88Mobile.APIUrl + "/payments/120272/" + transactionId;
        send("GET", walletData, function () { GPInt.prototype.ShowSplash(); }, successCallback, errorCallback, completeCallback, url);
    }

    function validate(data, method) {
        // @todo add validation here
        return;
    }
}

window.w88Mobile.Gateways.Baokim = Baokim();