﻿window.w88Mobile.LanguageSelection = LanguageSelection();
var _w88_LanguageSelection = window.w88Mobile.LanguageSelection;

_w88_LanguageSelection.init();

function LanguageSelection() {

    var languages = {};

    languages.init = function () {
        
        var lang = Cookies().getCookie('language');

        if (_.isEmpty(lang)) {
            _w88_LanguageSelection.fetch(false);
            $('#language-modal div.modal-header.close').hide();
            $('#language-modal').modal('show');
        } else {
            _w88_LanguageSelection.fetch(true);
        }
    };

    languages.fetch = function (hasClose) {

        var lang = amplify.store("languages");
        var langTemplate = _.template($("script#LanguageSelectionTemplate").html());

        if (_.isEmpty(lang)) {
            _w88_send("/Languages", "GET", "", function (response) {
                if (_.isEqual(response.ResponseCode, 1)) {

                    amplify.store("languages", response.ResponseData, window.User.storageExpiration);

                    $("#language-modal div.modal-content").html(langTemplate({
                        data: response.ResponseData,
                        lblTitle: $.i18n("LABEL_MENU_LANGUAGE"),
                        hasCloseButton: hasClose,
                        activeLang: Cookies().getCookie('language')
                    }));
                }
            });
        } else {

            $("#language-modal div.modal-content").html(langTemplate({
                data: lang,
                lblTitle: $.i18n("LABEL_MENU_LANGUAGE"),
                hasCloseButton: hasClose,
                activeLang: Cookies().getCookie('language')
            }));
        }
    };

    languages.select = function (langCode) {
        if (!_.isEmpty(langCode)) {
            Cookies().setCookie('language', langCode);
            window.location.reload();
        }
    };

    languages.show = function () {

        _w88_LanguageSelection.fetch(true);

        $('#language-modal div.modal-header.close').show();
        $('#language-modal').modal('show');

    };

    return languages;
}