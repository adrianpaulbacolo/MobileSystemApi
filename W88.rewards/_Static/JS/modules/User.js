var User = function User() {
    this.Balance = null;
    this.CountryCode = null;
    this.CurrencyCode = null;
    this.FirstName = null;
    this.LanguageCode = null;
    this.MemberId = null;
    this.ResetPassword = false;
    this.PartialSignup = null;
    this.Token = null;
};

User.prototype.convertToJsonString = function () {
    return JSON.stringify(this);
};

User.prototype.createUser = function (user) {
    var obj = JSON.parse(user);
    this.CurrencyCode = _.isEmpty(obj.CurrencyCode) ? null : obj.CurrencyCode;
    this.LanguageCode = _.isEmpty(obj.LanguageCode) ? null : obj.LanguageCode;
    this.MemberId = _.isEmpty(obj.MemberId) ? null : obj.MemberId;
    this.ResetPassword = _.isEmpty(obj.ResetPassword) ? false : obj.ResetPassword;
    this.Token = _.isEmpty(obj.Token) ? null : obj.Token;
    return this;
};

User.prototype.save = function () {
    if (_.isEmpty(this)) return;
    var user = this.convertToJsonString();
    try {
        window.localStorage.setItem('user', user);
        Cookies().setCookie('user', user, 30);
    } catch (e) {
        Cookies().setCookie('user', user, 30);
    }
};

User.prototype.hasSession = function () {
    return !_.isEmpty(this.Token);
};

User.prototype.setProperties = function (data) {
    if (_.isEmpty(data)) return;
    this.CurrencyCode = data.CurrencyCode || null;
    this.LanguageCode = data.LanguageCode || null;
    this.MemberId = data.MemberId || null;
    this.ResetPassword = data.ResetPassword || false;
    this.Token = data.Token || null;
    return this;
};