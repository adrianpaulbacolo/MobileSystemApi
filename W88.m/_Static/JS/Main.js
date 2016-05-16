$(window).load(function () {
    GPINTMOBILE.HideSplash();
    if(typeof window.User != "undefined" && window.User.hasSession) checkSession();
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

if ($("#divBalance").hasClass("open")) { $("#divBalance").addClass("close"); } else { if ($("#divBalance").hasClass("open")) { $("#divBalance").addClass("close"); } }

// mozfullscreenerror event handler
function errorHandler() { /*alert('mozfullscreenerror');*/ }
//document.documentElement.addEventListener('mozfullscreenerror', errorHandler, false);

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

function Cookies() {
    var setCookie = function (cname, cvalue, expiryDays) {
        var date = new Date();
        date.setTime(date.getTime() + (expiryDays * 24 * 60 * 60 * 1000));
        var expires = "expires=" + date.toUTCString();
        document.cookie = cname + "=" + cvalue + ";" + expires;
    };

    var getCookie = function (cname) {
        var name = cname + "=";
        var cookies = document.cookie.split(';');
        for (var i = 0; i < cookies.length; i++) {
            var cookie = cookies[i];
            while (cookie.charAt(0) == ' ')
                cookie = cookie.substring(1);

            if (cookie.indexOf(name) == 0) {
                return cookie.substring(name.length, cookie.length);
            }
        }
        return "";
    };

    return {
        setCookie: setCookie,
        getCookie: getCookie
    };

}

function redirectToHttps() {
    var protocol = window.location.protocol;
    var httpURL = window.location.hostname + window.location.pathname;

    if (protocol == "http:") {
        var httpsURL = "https://" + httpURL;

        window.location = httpsURL;
    }
}

