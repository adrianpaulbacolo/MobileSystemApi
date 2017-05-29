window.w88Mobile.Register = Register();
var _w88_register = window.w88Mobile.Register;

function Register() {

    var register = {};

    register.init = function (gatewayId) {

        _w88_validator.initiateValidator("#form1");

        $('header .header-title').append($.i18n("LABEL_MENU_REGISTER"));

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
            if (_.isArray(response.ResponseMessage))
                w88Mobile.Growl.shout(w88Mobile.Growl.bulletedList(response.ResponseMessage));
            else
                w88Mobile.Growl.shout(response.ResponseMessage);
        });
    };

    return register;
}