$(window).load(function () {

    if (typeof window.User != "undefined" && window.User.hasSession) checkSession();
    var sessionPoll;

    function checkSession() {
        var intervalMin = 10000; // 10 secs
        var sessionInterval = (typeof window.User != "undefined" && parseInt(window.User.sessionInterval) > intervalMin) ? parseInt(window.User.sessionInterval) : intervalMin;

        sessionPoll = window.setInterval(function () {
            $.ajax({
                contentType: "application/json; charset=utf-8",
                url: "/_secure/AjaxHandlers/MemberSessionCheck.ashx",
                responseType: "json",
                success: function (data) {
                    if (data.code != "1") {
                        if (typeof data.message != "undefined") {
                            w88Mobile.Growl.shout(data.message, function () {
                                window.location.replace("/Logout");
                            });
                        } else window.location.replace("/Logout");

                        clearInterval(sessionPoll);
                    }
                },
                error: function (err) {
                }
            });
        }, sessionInterval);
    }
});