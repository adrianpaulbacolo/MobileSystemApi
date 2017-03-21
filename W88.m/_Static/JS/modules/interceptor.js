function isAPIRequest(url) {
    var apiPaths = [w88Mobile.APIUrl];
    for (var i in apiPaths) {
        if (_.includes(url, apiPaths[i])) {
            return true;
        }
    }
    return false;
};

// interceptor: check api calls if user status has changed
$(document).ajaxComplete(function (event, request, settings) {

    if (isAPIRequest(settings.url)) {
        if (!_.isUndefined(request.responseJSON.ResponseCode)) {
            switch (request.responseJSON.ResponseCode) {
                case -7: //session expired
                    //alert(request.responseJSON.ResponseMessage);
                    //window.location.href = "/logout";
                    // use below if growl is available
                    w88Mobile.Growl.shout(request.responseJSON.ResponseMessage, function () {
                        window.location.href = "/logout";
                    });
                    break;

                case -6: //multiple login
                    //alert(request.responseJSON.ResponseMessage);
                    //window.location.href = "/logout";
                    // use below if growl is available
                    w88Mobile.Growl.shout(request.responseJSON.ResponseMessage, function () {
                        window.location.href = "/logout";
                    });
                    break;

                case -2: // not logged in
                    //alert(request.responseJSON.ResponseMessage);
                    //window.location.href = "/_secure/login.aspx";
                    // use below if growl is available
                    w88Mobile.Growl.shout(request.responseJSON.ResponseMessage, function () {
                        window.location.href = "/_secure/login.aspx";
                    });
                    break;
            }
        }
    }
});