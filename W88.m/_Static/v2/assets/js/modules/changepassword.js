window.w88Mobile.ChangePassword = ChangePassword();
var _w88_changepassword = window.w88Mobile.ChangePassword;

function ChangePassword() {

    var changepassword = {};

    changepassword.init = function (gatewayId) {

        _w88_validator.initiateValidator();

        $('header .header-title').append(_w88_contents.translate("LABEL_CHANGEPASSWORD"));

        $('[id$="lblPassword"]').text(_w88_contents.translate("LABEL_CHANGEPASSWORD_CURRENT"));
        $('[id$="lblPasswordNew"]').text(_w88_contents.translate("LABEL_CHANGEPASSWORD_NEW"));
        $('[id$="lblPasswordConfirm"]').text(_w88_contents.translate("LABEL_CHANGEPASSWORD_CONFIRM"));

        $('#btnSubmit').text(_w88_contents.translate("BUTTON_SUBMIT"));
    };

    changepassword.send = function (data) {
        send("/user/changepassword", "POST", data, function (response) {
            if (_.isArray(response.ResponseMessage))
                w88Mobile.Growl.shout(w88Mobile.Growl.bulletedList(response.ResponseMessage));
            else
                w88Mobile.Growl.shout(response.ResponseMessage);
        });
    };

    return changepassword;
}