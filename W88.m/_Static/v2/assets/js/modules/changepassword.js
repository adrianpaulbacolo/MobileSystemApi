window.w88Mobile.ChangePassword = ChangePassword();
var _w88_changepassword = window.w88Mobile.ChangePassword;

function ChangePassword() {

    var changepassword = {};

    changepassword.init = function (gatewayId) {

        _w88_validator.initiateValidator("#form1");

        $('header .header-title').append(_w88_contents.translate("LABEL_CHANGEPASSWORD"));
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