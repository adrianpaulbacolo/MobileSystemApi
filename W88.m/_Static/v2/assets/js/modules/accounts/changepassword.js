window.w88Mobile.ChangePassword = ChangePassword();
var _w88_changepassword = window.w88Mobile.ChangePassword;

function ChangePassword() {

    var changepassword = {};

    changepassword.init = function (gatewayId) {

        _w88_validator.initiateValidator("#form1");

        $('header .header-title').append(_w88_contents.translate("LABEL_PASSWORD_CHANGE"));
    };

    changepassword.send = function (data) {
        _w88_send("/user/changepassword", "POST", data, function (response) {
            switch (response.ResponseCode) {
                case 1:
                    pubsub.subscribe('checkFreeRounds', onCheckFreeRounds);
                    _w88_products.checkFreeRounds();
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

    function onCheckFreeRounds(topic, data) {
        var redirect = getQueryStringValue("redirect");

        if (_.isEmpty(data)) {
            if (_.isEmpty(redirect)) {
                window.location.href = _constants.DASHBOARD_URL;
            } else {
                location.href = redirect;
            }

        } else {
            $('#btnClaimNow').attr('href', data);

            $("#freerounds-modal").on('hidden.bs.modal', function () {
                if (_.isEmpty(redirect)) {
                    window.location.href = _constants.DASHBOARD_URL;
                } else {
                    location.href = redirect;
                }
            });

            $('#freerounds-modal').modal('show');
        }
    }

    return changepassword;
}