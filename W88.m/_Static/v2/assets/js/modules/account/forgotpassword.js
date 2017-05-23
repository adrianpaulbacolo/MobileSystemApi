window.w88Mobile.ForgotPassword = ForgotPassword();
var _w88_ForgotPassword = window.w88Mobile.ForgotPassword;

function ForgotPassword() {

    var forgot = {};
    var key = "forgot_login";
    var forgotData = {
        Username: {},
        Email: {},
        QuestionId: 0,
        Answer: '',
        LastRequested: {},
        isPartial: true
    };

    forgot.init = function () {

        $('.header-title').first().text($.i18n("LABEL_PASSWORD_FORGOT"));

        amplify.store(key, forgotData, window.User.storageExpiration);
        var step1Template = _.template($("script#Step1Template").html());

        $("#forgot").html(step1Template({
            lblUsername: $.i18n("LABEL_USERNAME"),
            lblEmail: $.i18n("LABEL_EMAIL"),
            lblSubmit: $.i18n("BUTTON_SUBMIT")
        }));

        _w88_validator.initiateValidator($("#form1"), {});
    };

    forgot.fetchQuestions = function () {

        _w88_ForgotPassword.send("", "/SecurityQuestions", "GET", function (response) {
            if (_.isEqual(response.ResponseCode, 1)) {
                _.forOwn(response.ResponseData, function (data) {
                    $('#questions').append($('<option>').text(data.Text).attr('value', data.Value));
                });
                $("#questions").val($("#questions option:first").val()).change();
                $('#form1').validator('update').validator();
            }
        });
    };

    forgot.checkPartial = function () {

        var hasErrors = $('#form1').validator('validate').has('.has-error').length;

        if (!hasErrors) {

            forgotData.Username = $("#txtUsername").val();
            forgotData.Email = $("#txtEmail").val();

            amplify.store(key, forgotData, window.User.storageExpiration);

            var d = {
                username: forgotData.Username,
                email: forgotData.Email
            };

            _w88_ForgotPassword.send(d, "/user/CheckPartialRegistration", "GET", function (response) {
                if (_.isEqual(response.ResponseCode, 1)) {

                    switch (response.ResponseData) {
                        case 1:
                            forgotData.isPartial = true;
                            amplify.store(key, forgotData, window.User.storageExpiration);
                            _w88_ForgotPassword.submit();
                            break;

                        case 2:
                            forgotData.isPartial = false;
                            amplify.store(key, forgotData, window.User.storageExpiration);

                            var step2Template = _.template($("script#Step2Template").html());

                            $("#forgot").html(step2Template({
                                lblQuestion: $.i18n("LABEL_SECURITY_QUESTION"),
                                lblAnswer: $.i18n("LABEL_SECURITY_ANSWER"),
                                lblSubmit: $.i18n("BUTTON_SUBMIT")
                            }));

                            _w88_ForgotPassword.fetchQuestions();

                            break;
                        default:
                            window.w88Mobile.Growl.shout(response.ResponseMessage);
                            break;
                    }
                }
            });
        }
    };

    forgot.submit = function () {
        forgotData = amplify.store(key);

        if (forgotData.isPartial) {
            forgotData.QuestionId = 0;
            forgotData.Answer = '';
        } else {

            var hasErrors = $('#form1').validator('validate').has('.has-error').length;

            if (!hasErrors) {
                forgotData.QuestionId = $("#questions").val();
                forgotData.Answer = $("#txtSecurityAnswer").val();
            } else return false;
        }

        forgotData.LastRequested = Cookies().getCookie(key);

        _w88_ForgotPassword.send(forgotData, "/user/ForgotPassword", "POST", function (response) {
            if (_.isEqual(response.ResponseCode, 1)) {

                Cookies().setCookie(key, response.ResponseData);
                window.w88Mobile.Growl.shout(response.ResponseMessage, function () {
                    window.location.reload();
                });

            } else {
                window.w88Mobile.Growl.shout(response.ResponseMessage);
            }
        });
    };

    forgot.send = function (data, resource, method, success, complete) {

        var url = w88Mobile.APIUrl + resource;

        var headers = {
            'Token': window.User.token,
            'LanguageCode': window.User.lang
        };

        $.ajax({
            type: method,
            url: url,
            data: data,
            beforeSend: function () {
                pubsub.publish('startLoadItem', { selector: "" });
            },
            headers: headers,
            success: success,
            error: function () {
                console.log("Error connecting to api");
            },
            complete: function () {
                if (!_.isUndefined(complete)) complete();
                pubsub.publish('stopLoadItem', { selector: "" });
            }
        });
    };

    return forgot;
}