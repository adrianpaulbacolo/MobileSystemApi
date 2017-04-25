var siteCookie = new Cookies();

function Cookies() {
    var setCookie = function (cname, cvalue, expiryDays, domain) {
        var site = "";
        if (domain != undefined) {
            site = "domain=" + domain + ";";
        }
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

    return {
        setCookie: setCookie,
        getCookie: getCookie
    };

}