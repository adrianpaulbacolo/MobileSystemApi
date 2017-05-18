var User = function User() {
    this.CurrencyCode = null;
    this.LanguageCode = null;
    this.MemberId = null;
    this.ResetPassword = false;
    this.Token = null;
};

User.prototype.convertToJsonString = function () {
    var self = this;
    return JSON.stringify(self);
};

User.prototype.createUser = function (user) {
    var self = this,
        obj = JSON.parse(user);
    self.CurrencyCode = _.isEmpty(obj.CurrencyCode) ? null : obj.CurrencyCode;
    self.LanguageCode = _.isEmpty(obj.LanguageCode) ? null : obj.LanguageCode;
    self.MemberId = _.isEmpty(obj.MemberId) ? null : obj.MemberId;
    self.ResetPassword = _.isEmpty(obj.ResetPassword) ? false : obj.ResetPassword;
    self.Token = _.isEmpty(obj.Token) ? null : obj.Token;
    return self;
};

User.prototype.save = function () {
    var self = this,
        user = self.convertToJsonString();
    amplify.store(window.location.host + '_user', user);
};

User.prototype.hasSession = function () {
    var self = this;
    return !_.isEmpty(self.Token);
};

User.prototype.setProperties = function (data) {
    if (_.isEmpty(data)) return;
    var self = this;
    self.CurrencyCode = data.CurrencyCode || null;
    self.LanguageCode = data.LanguageCode || null;
    self.MemberId = data.MemberId || null;
    self.ResetPassword = data.ResetPassword || false;
    self.Token = data.Token || null;
    return self;
};