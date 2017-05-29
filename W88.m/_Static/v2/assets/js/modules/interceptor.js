function isAPIRequest(url) {
    var apiPaths = [w88Mobile.APIUrl];
    for (var i in apiPaths) {
        if (_.includes(url, apiPaths[i])) {
            return true;
        }
    }
    return false;
};


// interceptor: update headers
$(document).ajaxSend(function (event, xhr, settings) {
    if (isAPIRequest(settings.url) && _.includes(settings.url.toLowerCase(), "payments")) {
        xhr.setRequestHeader("SubPlatformId", siteCookie.getCookie('spfid_mob'));
    }
    var lang = window.User.lang;
    // @todo remove if zh-my is available
    if (lang == "zh-my") lang = "zh-cn";
    xhr.setRequestHeader('Token', window.User.token);
    xhr.setRequestHeader('LanguageCode', lang);
});

// interceptor: check api calls if user status has changed
$(document).ajaxComplete(function (event, request, settings) {

    if (_.isUndefined(request.responseJSON)) return;

    if (isAPIRequest(settings.url)) {
        if (!_.isUndefined(request.responseJSON.ResponseCode)) {
            switch (request.responseJSON.ResponseCode) {
                case -7: //session expired
                    w88Mobile.Growl.shout(request.responseJSON.ResponseMessage, function () {
                        _w88_account.logout();
                    });
                    break;

                case -6: //multiple login
                    w88Mobile.Growl.shout(request.responseJSON.ResponseMessage, function () {
                        _w88_account.logout();
                    });
                    break;

                case -2: // not logged in
                    w88Mobile.Growl.shout(request.responseJSON.ResponseMessage, function () {
                        window.location.href = "/v2/Account/Login.aspx";
                    });
                    break;
            }
        }
    }
});