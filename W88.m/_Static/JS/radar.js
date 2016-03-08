(function (e, c, a, g, f) {
    function d() {
        var b = c.createElement("script");
        b.async = !0;
        b.src = "//radar.cedexis.com/1/14375/radar.js";
        c.body.appendChild(b)
    }
    (function () {
        for (var b = [/\bMSIE (5|6)/i], a = b.length; a--;)
            if (b[a].test(navigator.userAgent))
                return !1;
        return !0
    })()
    && ("complete" !== c.readyState ? (a = e[a]) ? a(f, d, !1) : (a = e[g]) && a("on" + f, d) : d())
})(window, document, "addEventListener", "attachEvent", "load");