window.w88Mobile.Register = Register();
var _w88_register = window.w88Mobile.Register;

function Register() {

    var register = {};
    var paymentCache;

    register.init = function (gatewayId) {

        _w88_validator.initiateValidator("#form1");

        $('header .header-title').append($.i18n("LABEL_MENU_REGISTER"));
        $('.url_terms').attr('href', _constants.TERMS_URL);

        register.getCountryPhoneList();

        register.getCurrencyList();

        var affiliateId = getQueryStringValue('AffiliateId');

        if (!_.isEmpty(affiliateId)) {
            $('[id$="txtAffiliateID"]').val(affiliateId);
            $('[id$="txtAffiliateID"]').attr("disabled", true);
        }

        if (_.isEqual(siteCookie.getCookie('language').toLowerCase(), 'th-th')) {
            $('.lineid').show();
        } else {
            $('.lineid').hide();
        }
    };

    register.initSuccess = function () {
        // piwik tracking signup
        w88Mobile.PiwikManager.trackSignup();

        $('#reg-contact').html($.i18n('LABEL_REGISTER_CONTACT_US', _constants.TERMS_URL));

        fetchDepositSettings(function () {
            if (paymentCache.settings.length == 0) {
                // track accounts with no gateways
                w88Mobile.PiwikManager.trackEvent({
                    category: "RegSuccess",
                    action: window.User.countryCode,
                    name: window.User.memberId
                });

                $('#paymentNote').html($.i18n('LABEL_PAYMENT_NOTE_NO_GATEWAY'));
            }
            else {
                // payment cache variable is now present once callback is triggered
                setDepositPaymentTab(paymentCache.settings);

                $('#paymentNote').html($.i18n('LABEL_REGISTER_DEPOSIT_NOTE', _constants.TERMS_URL));
            }
        });
    };

    function fetchDepositSettings(callback) {

        var cacheKey = w88Mobile.Keys.depositSettings;

        paymentCache = amplify.store(cacheKey);

        if (!_.isEmpty(paymentCache) && User.lang == paymentCache.language) {
            callback();
        } else {
            _w88_send("/payments/settings/deposit", "GET", {}, function (response) {
                switch (response.ResponseCode) {
                    case 1:
                        paymentCache = {
                            settings: response.ResponseData
                            , language: window.User.lang
                        };
                        amplify.store(cacheKey, paymentCache, User.storageExpiration);
                        callback();
                    default:
                        break;
                }
            });
        }
    }

    function setDepositPaymentTab(responseData) {
        if (responseData.length > 0) {
            var title = "", page = null;

            for (var i = 0; i < responseData.length; i++) {
                var data = responseData[i];

                data.Method = _.isEmpty(data.Method) ? "" : data.Method;
                data.Id = data.Id + data.Method;

                if (_.isEmpty(data.Method) && _.isEqual(data.Name, "Baokim"))
                    continue;

                page = "/v2/Deposit/Pay" + data.Id + ".aspx";

                var anchor = $('<a />', { class: 'list-group-item', id: data.Id, href: page }).text(data.Title);

                if ($('#paymentTabs').length > 0)
                    $('#paymentTabs').append(anchor);

            }
        }
    }

    register.getCountryPhoneList = function () {
        _w88_send("/CountryPhoneList", "GET", "", function (response) {
            if (!_.isEqual(response.ResponseCode, 0)) {
                $('select[id$="drpCountryCode"]').append($('<option>').text($.i18n("LABEL_SELECT_DEFAULT")).attr('value', '-1'));

                _.forEach(response.ResponseData.PhoneList, function (data) {
                    $('select[id$="drpCountryCode"]').append($("<option></option>").attr("value", data.Value).text(data.Text));
                });

                if (!_.isEmpty(response.ResponseData.PhoneSelected))
                    $('select[id$="drpCountryCode"]').val(response.ResponseData.PhoneSelected).change();
            }
        });
    };

    register.getCurrencyList = function () {
        _w88_send("/CurrencyList", "GET", "", function (response) {
            if (!_.isEqual(response.ResponseCode, 0)) {
                $('select[id$="drpCurrency"]').append($('<option>').text($.i18n("LABEL_SELECT_DEFAULT")).attr('value', '-1'));

                _.forEach(response.ResponseData.CurrencyList, function (data) {
                    $('select[id$="drpCurrency"]').append($("<option></option>").attr("value", data.Value).text(data.Text));
                });

                if (!_.isEmpty(response.ResponseData.CurrencySelected))
                    $('select[id$="drpCurrency"]').val(response.ResponseData.CurrencySelected).change();
            }
        });
    };

    register.createAccount = function (data) {
        _w88_send("/user/Register", "POST", data, function (response) {
            switch (response.ResponseCode) {
                case 1:
                    w88Mobile.Growl.shout(response.ResponseMessage, function () {
                        location.href = _constants.REGISTER_SUCCESS_URL;
                    });
                    break;
                default:
                    if (_.isArray(response.ResponseMessage))
                        w88Mobile.Growl.shout(w88Mobile.Growl.bulletedList(response.ResponseMessage));
                    else
                        w88Mobile.Growl.shout(response.ResponseMessage);
                    break;
            }
        });
    };

    return register;
}