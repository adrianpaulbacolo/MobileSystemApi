var _w88_account = w88Mobile.Account = Account();

function Account() {
    var account = {};

    account.logout = function () {
        amplify.clearStore();
        siteCookie.clearCookies();
        window.location = "/v2/Dashboard.aspx";
        window.reload;
    };

    return account;
}