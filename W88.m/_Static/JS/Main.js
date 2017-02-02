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
                        if (typeof data.message != "undefined") alert(data.message);
                        clearInterval(sessionPoll);
                        window.location.replace("/Logout");
                    }
                },
                error: function (err) {
                }
            });
        }, sessionInterval);
    }
});

// toggle full screen
function toggleFullScreen() {
    if (!document.fullscreenElement &&    // alternative standard method
        !document.mozFullScreenElement && !document.webkitFullscreenElement) {  // current working methods
        if (document.documentElement.requestFullscreen) {
            document.documentElement.requestFullscreen();
        } else if (document.documentElement.mozRequestFullScreen) {
            document.documentElement.mozRequestFullScreen();
        } else if (document.documentElement.webkitRequestFullscreen) {
            document.documentElement.webkitRequestFullscreen(Element.ALLOW_KEYBOARD_INPUT);
        }
    } else {
        if (document.cancelFullScreen) {
            document.cancelFullScreen();
        } else if (document.mozCancelFullScreen) {
            document.mozCancelFullScreen();
        } else if (document.webkitCancelFullScreen) {
            document.webkitCancelFullScreen();
        }
    }
}

function redirectToHttps() {
    var protocol = window.location.protocol;
    var httpURL = window.location.hostname + window.location.pathname;

    if (protocol == "http:") {
        var httpsURL = "https://" + httpURL;

        window.location = httpsURL;
    }
}

function NotAllowDecimal(e) {
    var key = e.keyCode;
    if ($.browser.mozilla) {
        key = e.which;
    }
    if (key != 0 && key != 8) {
        var regex = new RegExp("^[0-9]+$");
        var code = String.fromCharCode(key);
        if (!regex.test(code))
            return false;
    }
}

