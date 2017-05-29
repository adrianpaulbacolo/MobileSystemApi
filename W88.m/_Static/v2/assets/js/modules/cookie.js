var siteCookie = new Cookies();

function Cookies() {
    var setCookie = function (cname, cvalue, expiryDays) {
        var site = "";
        var domain = "." + location.hostname.split('.').slice(-2).join('.');
        site = "domain=" + domain + ";";
        var date = new Date();
        date.setTime(date.getTime() + (expiryDays * 24 * 60 * 60 * 1000));
        var expires = "expires=" + date.toUTCString() + ";";
        document.cookie = cname + "=" + cvalue + ";" + expires + site + "path=/";
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

    var clearCookies = function () {
        var cookies = document.cookie.split(";");

        for (var i = 0; i < cookies.length; i++) {
            var cookie = cookies[i];
            var eqPos = cookie.indexOf("=");
            var name = eqPos > -1 ? cookie.substr(0, eqPos) : cookie;
            setCookie(name, "", -1);
        }
    }

    return {
        setCookie: setCookie,
        getCookie: getCookie,
        clearCookies: clearCookies
    };
}