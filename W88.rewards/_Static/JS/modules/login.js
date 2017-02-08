var Login = function(translations, elems, redirectUri, isVipLogin) {
    this.elems = elems;
    this.translations = translations;
    this.count = 0;
    this.redirectUri = redirectUri;
    this.elems.captchaDiv.hide();
    this.isVipLogin = isVipLogin;
};

Login.prototype.initializeButtons = function() {
    var self = this,
        submitButton = self.elems.submitButton;
    submitButton.click(function(e) {
        submitButton.attr('disabled', true);
        var username = self.elems.username.val().trim(),
            password = self.elems.password.val().trim(),
            captcha = self.elems.captcha.val().trim();

        if (username.length == 0) {
            self.showMessage(self.translations.MissingUsername);
            submitButton.attr('disabled', false);
            e.preventDefault();
            return;
        }
        if (!/^[a-zA-Z0-9]+$/.test(username)) {
            self.showMessage(self.translations.InvalidUsernamePassword);
            submitButton.attr('disabled', false);
            e.preventDefault();
            return;
        }
        if (password.length == 0) {
            self.showMessage(self.translations.MissingPassword);
            submitButton.attr('disabled', false);
            e.preventDefault();
            return;
        }
        if (captcha.length == 0 && self.counter >= 3) {
            self.showMessage(self.translations.IncorrectVCode);
            submitButton.attr('disabled', false);
            e.preventDefault();
            return;
        }

        e.preventDefault();
        self.initiateLogin();
    });
};


Login.prototype.initiateLogin = function () {
    var self = this;
    $.ajax({
        type: 'POST',
        contentType: 'application/json',
        url: '/api/user/login',
        beforeSend: function() {
            GPINTMOBILE.ShowSplash();
        },
        timeout: function() {
            self.elems.submitButton.attr('disabled', false);
            self.showMessage(self.translations.Exception);
            window.location.href = '/Default.aspx?lang=<%=Language%>';
        },
        data: JSON.stringify({
            UserInfo: {
                Username: self.elems.username.val(),
                Password: self.elems.password.val()
            },
            Captcha: self.elems.captcha.val()
        }),
        success: function(response) {
            if (!response || response.ResponseCode == undefined) {
                self.initiateLogin();
                return;
            }

            var message = response.ResponseMessage;
            switch (response.ResponseCode) {
                case 1:
                    window.user = (new User()).setProperties(response.ResponseData);
                    window.user.save();

                    if (response.ResponseData.ResetPassword) {
                        window.location.href = '/_Secure/ChangePassword.aspx';
                        return;
                    }
                    if (!_.isEmpty(self.redirectUri)) {
                        frsm_code = window.user.MemberId;
                        window.location.href = self.redirectUri;
                        return;
                    }

                    window.location.reload();
                    break;
                case 22:
                    GPINTMOBILE.HideSplash();
                    self.elems.submitButton.attr('disabled', false);
                    self.showMessage(message);
                    break;
                default:
                    self.counter += 1;

                    if (counter >= 3) {
                        self.elems.captchaDiv.show();
                        self.elems.captchaImg.attr('src', '/_Secure/Captcha.aspx?t=' + new Date().getTime());
                        self.elems.captcha.val('');
                        self.elems.password.val('');
                    }

                    self.elems.submitButton.attr('disabled', false);
                    GPINTMOBILE.HideSplash();
                    self.showMessage(message);
                    break;
            }
        },
        error: function(err) {
            GPINTMOBILE.HideSplash();
            self.showMessage(self.translations.Exception);
            self.elems.submitButton.attr('disabled', false);
        }
    });
};

Login.prototype.notAllow = function () {
    var self = this;
    self.showMessage(self.translations.VipOnly);
};

Login.prototype.showMessage = function(message) {
    if (_.isEmpty(message)) return;
    window.w88Mobile.Growl.shout('<div>' + message + '</div>');
};