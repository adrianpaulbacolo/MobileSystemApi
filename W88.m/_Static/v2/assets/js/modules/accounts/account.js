var _w88_account = w88Mobile.Account = Account();

function Account() {
    var account = {};

    account.logout = function () {
        var lang = siteCookie.getCookie("language");
        amplify.clearStore();
        siteCookie.clearCookies();
        window.location.href = _constants.DASHBOARD_URL + "?lang=" + lang;
    };

    return account;
}