var _w88_login = w88Mobile.Login = Login();

function Login() {
    var counter = 0;
    var login = {};

    login.init = function (options, isSelection) {

        login.getRedirectUrl();

        _w88_validator.initiateValidator("#form1");

        $('header .header-title').append($.i18n("LABEL_MENU_LOGIN"));
        $('input[id$="txtUsername"]').attr("placeholder", $.i18n("LABEL_USERNAME"));
        $('input[id$="txtPassword"]').attr("placeholder", $.i18n("LABEL_PASSWORD"));
        $('input[id$="txtCaptcha"]').attr("placeholder", $.i18n("LABEL_CAPTCHA"));
        $("#loginNote").html($.i18n("LABEL_LOGIN_NOTE_CONTACT_US"));

        $('#imgCaptcha').click(function () {
            $(this).attr('src', '/v2/Account/Captcha.ashx?t=' + new Date().getTime());
        });

        login.hideCaptcha();
    };

    login.getRedirectUrl = function () {
        var redirect = getQueryStringValue("redirect");
        var token = getQueryStringValue("token");

        if (!_.isEmpty(token)) {
            data = { encryptedToken: token };
            login.autologin(data);
        }
        else {
            if (_.isEmpty(redirect)) {
                login.getGPIUrl();
            }
        }
    };

    login.getGPIUrl = function () {
        var url = getQueryStringValue("url");
        var sessionid = siteCookie.getCookie("s");
        if (!_.isEmpty(url)) {
            var newUrl = new URL(decodeURIComponent(url));
            if (!_.isEmpty(sessionid)) {
                var params = "&s=" + sessionid + "&token=" + sessionid + "&domainlink=" + getDomainName() + "&domain=" + getDomainName();

                if (newUrl.href.indexOf("?") > 0)
                    newUrl = new URL(newUrl.href.substring(0, newUrl.href.indexOf("?")));

                var redirect = newUrl.href + "?" + params;

                window.location = redirect;
            }
        }
    };

    login.showCaptcha = function () {
        $('.captcha').show();
        $('input[id$="txtCaptcha"]').attr({ required: '', 'data-require': '' });
        $('#form1').validator('update')
    };

    login.hideCaptcha = function () {
        $('.captcha').hide();
        $('input[id$="txtCaptcha"]').removeAttr('required data-require');
        $('#form1').validator('update')
    };

    login.login = function (data) {
        data.HasVCode = counter >= 3;
        _w88_send("/user/login", "POST", data, function (response) {
            switch (response.ResponseCode) {
                case 1:
                    var userData = response.ResponseData
                    siteCookie.setCookie("s", userData.Token, 1);
                    siteCookie.setCookie("palazzo", userData.Palazzo, 1);

                    var user = w88Mobile.Keys.userSettings;
                    amplify.store(user, userData, User.storageExpiration);

                    pubsub.subscribe('checkFreeRounds', onCheckFreeRounds);
                    _w88_products.checkFreeRounds();

                    break;
                default:
                    counter += 1;

                    if (_.isArray(response.ResponseMessage))
                        w88Mobile.Growl.shout(w88Mobile.Growl.bulletedList(response.ResponseMessage));
                    else
                        w88Mobile.Growl.shout(response.ResponseMessage);

                    if (counter >= 3) {
                        login.showCaptcha();
                    }

                    break;
            }
        });
    };

    login.changePassword = function (data) {
        var redirect = getQueryStringValue("redirect");
        var changePassword = "/v2/Account/ChangePassword.aspx";

        if (_.isEmpty(redirect)) {
            redirect = changePassword;
        } else {
            redirect = changePassword + "?" + redirect;
        }

        window.location = redirect;
    };

    function onCheckFreeRounds(topic, data) {
        if (!_.isUndefined(data)) {

            $('#btnClaimNow').attr('href', data);

            $("#freerounds-modal").on('hidden.bs.modal', function () {
                window.location = "/v2/Dashboard.aspx";
            });

            $('#freerounds-modal').modal('show');

        } else {
            var userData = amplify.store(w88Mobile.Keys.userSettings);

            if (userData.ResetPassword == true) {
                login.changePassword();
            } else {
                login.getGPIUrl();
            }
        }
    }

    login.autologin = function (data) {
        _w88_send("/user/autologin", "Get", data, function (response) {
            switch (response.ResponseCode) {
                case 1:
                    var userData = response.ResponseData
                    var user = w88Mobile.Keys.userSettings;
                    amplify.store(user, userData, User.storageExpiration);

                    var redirect = getQueryStringValue("redirect");
                    if (_.isEmpty(redirect))
                        window.location = "/v2/Funds.aspx";
                    else
                        window.location = redirect;

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

    return login;
}