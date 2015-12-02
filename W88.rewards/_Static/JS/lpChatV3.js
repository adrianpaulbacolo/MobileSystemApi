function CookieMethods(a) {
    if (a) {
        this.separator = a;
    }
}
CookieMethods.prototype.separator = "::";
CookieMethods.prototype.readCookie = function (b) {
    if (!b) {
        return "";
    }
    b = encodeURIComponent(b);
    var a = document.cookie.split(b + "=");
    if (a.length == 1) {
        return "";
    }
    return decodeURIComponent(a[1].split(";")[0]);
};
CookieMethods.prototype.writePersistentCookie = function (a, b, c) {
    c = typeof c == "number" ? (c) : (3600 * 24 * 30);
    this.writeSessionCookie(a, b, c);
};
CookieMethods.prototype.addToCookie = function (a, b, d) {
    var c = this.readCookie(a);
    c += this.separator + b;
    return typeof d === "number" ? this.writeSessionCookie(a, c, d) : this.writePersistentCookie(a, c, d);
};
CookieMethods.prototype.writeSessionCookie = function (b, c, d) {
    if (!b) {
        return false;
    }
    if (c === null || c === undefined) {
        d = -1;
    }
    if (typeof d === "number") {
        var a = new Date();
        a.setDate(a.getTime() + (d * 1000));
    }
    c = c ? encodeURIComponent(c) : "";
    document.cookie = [encodeURIComponent(b), "=", c, (a ? "; expires=" + a.toUTCString() : ""), "; path=/"].join("");
    return true;
};
CookieMethods.prototype.clearCookie = function (a) {
    return this.writeSessionCookie(a);
};

function StorageMethods() {
    var b = false,
        c = "lpTestCase";
    try {
        b = !!(typeof Storage) && this.setSessionData(c, "1");
        this.removeSessionData(c);
    } catch (a) { }
    this.isStorageEnabled = function () {
        return b;
    };
}
StorageMethods.prototype.setSessionData = function (a, b) {
    sessionStorage.setItem(a, b);
    return true;
};
StorageMethods.prototype.getSessionData = function (a) {
    return sessionStorage.getItem(a) || "";
};
StorageMethods.prototype.removeSessionData = function (a) {
    if (this.getSessionData(a)) {
        sessionStorage.removeItem(a);
        return true;
    }
    return false;
};
StorageMethods.prototype.setPersistentData = function (a, b) {
    localStorage.setItem(a, b);
};
StorageMethods.prototype.getPersistentSessionData = function (a) {
    return localStorage.getItem(a) || "";
};
StorageMethods.prototype.removePersistentData = function (a) {
    if (this.getPersistentSessionData(a)) {
        localStorage.removeItem(a);
        return true;
    }
    return false;
};
window.lpTag = window.lpTag || {};
lpTag.SessionDataManager = function (h) {
    var b = "::",
        g = {}, f = {};
    var i, a, e = this,
        d = false;
    if (this === window) {
        return;
    }
    i = new StorageMethods();
    a = new CookieMethods();
    d = i.isStorageEnabled();

    function c(o) {
        if (lpTag && lpTag.utils && lpTag.utils.log) {
            lpTag.utils.log(o, "ERROR", "SessionData");
        } else {
            if (window.console) {
                console.error(o);
            }
        }
    }
    if (h) {
        for (var l in h.sessionData) {
            try {
                if (h.sessionData.hasOwnProperty(l) && typeof h.sessionData[l] === "string") {
                    g[l] = h.sessionData[l];
                }
            } catch (m) {
                c("Error in init of SessionData when setting up sessionData key: " + l);
                if (window.lpAgentConfig && window.lpAgentConfig.throwExceptions) {
                    throw m;
                }
            }
        }
        for (var j in h.persistentData) {
            try {
                if (h.persistentData.hasOwnProperty(j) && typeof h.persistentData[j] === "string") {
                    f[j] = h.persistentData[j];
                }
            } catch (m) {
                c("Error in init of SessionData when setting up persistentData key: " + j);
                if (window.lpAgentConfig && window.lpAgentConfig.throwExceptions) {
                    throw m;
                }
            }
        }
    }

    function n(q, t, s, u) {
        try {
            var p = u ? e.getPersistentSessionData(q) : e.getSessionData(q);
            var r = p !== "" ? p.split(b) : [];
            r.push(t);
            p = r.join("::");
            u ? e.setPersistentData(q, p) : e.setSessionData(q, p, s);
        } catch (o) {
            c((u ? "appendToPersistentData " : "appendToSessionData ") + " failed, key: " + q);
            if (window.lpAgentConfig && window.lpAgentConfig.throwExceptions) {
                throw o;
            }
        }
    }

    function k(t, s, q, p) {
        try {
            var u = p ? e.getPersistentSessionData(t) : e.getSessionData(t);
            var r = u !== "" ? u.split(b) : [];
            var w = [];
            for (var o = 0; o < r.length; o++) {
                if (r[o] !== s) {
                    w.push(r[o]);
                }
            }
            u = w.join("::");
            if (u !== "") {
                p ? e.setPersistentData(t, u) : e.setSessionData(t, u, q);
            } else {
                p ? e.removePersistentData(t) : e.removeSessionData(t);
            }
        } catch (v) {
            c((p ? "removePartialPersistentData " : "removePartialSessionData ") + " failed, key: " + t);
            if (window.lpAgentConfig && window.lpAgentConfig.throwExceptions) {
                throw v;
            }
        }
    }
    this.readCookie = function (o) {
        return a.readCookie(o);
    };
    this.setSessionData = function (p, q) {
        try {
            if (!g[p]) {
                g[p] = p;
            }
            if (d) {
                i.setSessionData(p, q);
            } else {
                a.writeSessionCookie(p, q);
            }
        } catch (o) {
            c("Error in setSessionData, data: " + p);
            if (window.lpAgentConfig && window.lpAgentConfig.throwExceptions) {
                throw o;
            }
        }
    };
    this.getSessionData = function (p) {
        try {
            return d ? i.getSessionData(p) : a.readCookie(p);
        } catch (o) {
            c("Error in getSessionData, data: " + p);
            if (window.lpAgentConfig && window.lpAgentConfig.throwExceptions) {
                throw o;
            }
        }
    };
    this.getPersistentSessionData = function (p) {
        try {
            return d ? i.getPersistentSessionData(p) : a.readCookie(p);
        } catch (o) {
            c("Error in getPersistentSessionData, data: " + p);
            if (window.lpAgentConfig && window.lpAgentConfig.throwExceptions) {
                throw o;
            }
        }
    };
    this.appendToSessionData = function (o, q, p) {
        n(o, q, p, false);
    };
    this.appendToPersistentData = function (o, p) {
        n(o, p, null, true);
    };
    this.removePartialPersistentData = function (o, p) {
        k(o, p, null, true);
    };
    this.removeSessionData = function (o) {
        d ? i.removeSessionData(o) : a.clearCookie(o);
    };
    this.removePersistentData = function (o) {
        d ? i.removePersistentData(o) : a.clearCookie(o);
    };
    this.removePartialSessionData = function (o, q, p) {
        k(o, q, p, false);
    };
    this.setPersistentData = function (p, q) {
        try {
            if (!f[p]) {
                f[p] = p;
            }
            if (d) {
                i.setPersistentData(p, q);
            } else {
                a.writePersistentCookie(p, q);
            }
        } catch (o) {
            c("Error in setPersistentData, data: " + p);
            if (window.lpAgentConfig && window.lpAgentConfig.throwExceptions) {
                throw o;
            }
        }
    };
    this.clearPersistentData = function (p) {
        p = p || f;
        for (var q in p) {
            try {
                if (d) {
                    i.removePersistentData(p[q]);
                } else {
                    a.clearCookie(p[q]);
                }
            } catch (o) {
                c("Error in clearPersistentData, key: " + p[q]);
                if (window.lpAgentConfig && window.lpAgentConfig.throwExceptions) {
                    throw o;
                }
            }
        }
    };
    this.clearSessionData = function (p) {
        p = p || g;
        for (var q in p) {
            try {
                if (d) {
                    i.removeSessionData(p[q]);
                } else {
                    a.clearCookie(p[q]);
                }
            } catch (o) {
                c("Error in SessionData in clearSessionData, key:" + p[q]);
                if (window.lpAgentConfig && window.lpAgentConfig.throwExceptions) {
                    throw o;
                }
            }
        }
    };
};
window.lpTag = window.lpTag || {};
lpTag.utils = lpTag.utils || {};
lpTag.utils.Events = lpTag.utils.Events || function (f) {
    if (this === window) {
        return false;
    }
    var b = 0;
    var k = {};
    var d = {};
    var a = [];
    var i = "evId_";
    var j = 0;
    var g = (f && typeof f.cloneEventData === "boolean" ? f.cloneEventData : false);
    var l = (f && !isNaN(f.eventBufferLimit) ? f.eventBufferLimit : -1);

    function h(o, n, m) {
        if (lpTag && lpTag.utils && lpTag.utils.log) {
            lpTag.utils.log(o, n, m);
        }
    }
    k.once = function (m) {
        m.triggerOnce = true;
        this.register(m);
    };
    k.register = function (q) {
        if (!q.eventName || !q.func || (typeof q.func !== "function" && q.func.constructor !== Array)) {
            h("Ev listen has invalid params: evName=[" + q.eventName + "]  fn=[" + q.func + "]", "ERROR", "Events");
            return null;
        }
        if (q.func.constructor === Array) {
            var m = [],
                n, s;
            for (var p = 0; p < q.func.length; p++) {
                n = c(q);
                n.func = q.func[p];
                s = this.register(n);
                m.push(s);
            }
            return m;
        }
        var r = i + (b++);
        var o = {
            id: r,
            func: q.func,
            context: q.context || null,
            aSync: q.aSync ? true : false,
            appName: q.appName || "*",
            triggerOnce: q.triggerOnce || false
        };
        d[q.eventName] = d[q.eventName] || [];
        d[q.eventName].push(o);
        h("Ev listen rgstr: evName=[" + q.eventName + "]  fn=[" + q.func + "] aSync=" + o.aSync + " appName=" + o.name, "DEBUG", "Events");
        q = null;
        return r;
    };
    k.bind = k.register;
    k.unbind = function (r) {
        var m = false;
        var q;
        if (r && typeof r === "string") {
            return k.unregister(r);
        } else {
            if (!r || (!r.func && !r.context && !r.appName)) {
                return m;
            }
        }
        var p = d;
        if (r.eventName) {
            p = {};
            p[r.eventName] = d[r.eventName];
        }
        for (var n in p) {
            if (p.hasOwnProperty(n)) {
                q = o(p[n], r.func, r.context, r.appName);
                if (q.length !== p[n].length) {
                    d[n] = q;
                    m = true;
                }
            }
        }
        return m;

        function o(A, v, t, y) {
            var z = [];
            for (var x = 0; x < A.length; x++) {
                try {
                    var D = (!t && A[x].func === v);
                    var C = (!v && t && A[x].context === t);
                    var s = (v && t && A[x].func === v && A[x].context === t);
                    var u = (!t && !v && y && y == A[x].appName);
                    var B = (A[x].appName == "*");
                    if ((D || C || s)) {
                        if (u || B) {
                            continue;
                        }
                    } else {
                        if (u) {
                            continue;
                        }
                    }
                    z.push(A[x]);
                } catch (w) {
                    h("Error in unbind", "ERROR", "Events.unbind");
                }
            }
            return z;
        }
    };
    k.unregister = function (p) {
        var n = false;
        if (!p) {
            h("Ev listen id not spec for unregister", "ERROR", "Events");
            return null;
        }
        for (var m in d) {
            if (d.hasOwnProperty(m)) {
                for (var o = 0; o < d[m].length; o++) {
                    if (d[m][o].id == p) {
                        d[m].splice(o, 1);
                        h("Ev listen=" + p + " and name=" + m + " unregister", "DEBUG", "Events");
                        n = true;
                        break;
                    }
                }
            }
        }
        if (!n) {
            h("Ev listen not found " + p + " unregister", "DEBUG", "Events");
        }
        return n;
    };
    k.hasFired = function (p) {
        if (typeof (p.eventName) == "undefined" || p.eventName == "*") {
            return a;
        }
        var m = [];
        for (var o = 0; o < a.length; o++) {
            if (a[o].eventName == p.eventName) {
                if ((p.appName && p.appName == a[o].appName) || (!a[o].appName || a[o].appName == "*")) {
                    m.push(a[o]);
                }
            }
        }
        return m;
    };
    k.publish = function (m) {
        if (!m || typeof (m.eventName) == "undefined") {
            h("Ev name not spec for publish", "ERROR", "Events");
            m = null;
            return null;
        }
        m.data = m.data || {};
        m.passDataByRef = m.passDataByRef || !g;
        e(m);
        var t = false;
        if (!d[m.eventName]) {
            return false;
        }
        var r = [];
        for (var s = 0; s < d[m.eventName].length; s++) {
            if ((!m.appName || d[m.eventName][s].appName == "*") || d[m.eventName][s].appName == m.appName) {
                r.push(d[m.eventName][s]);
                t = true;
            }
        }
        if (t && r.length > 0) {
            for (var q = 0; q < r.length; q++) {
                var u = m.passDataByRef ? m.data : c(m.data);
                try {
                    var n = r[q];
                    if (n.aSync) {
                        setTimeout(p(n, u), 0);
                    } else {
                        n.func.call(n.context, u);
                        u = null;
                        if (n.triggerOnce) {
                            k.unbind(n);
                        }
                        n = null;
                    }
                } catch (o) {
                    h("Error executing " + m.eventName + " eventId: " + n.id, "ERROR", "Events");
                }
                h("Ev listen ev=" + m.eventName + " exec listenersFound=" + t, "DEBUG", "Events");
            }
        }
        m = null;
        return t;

        function p(w, v) {
            return function () {
                w.func.call(w.context, v);
                v = null;
                if (w.triggerOnce) {
                    k.unbind(w);
                }
                w = null;
            };
        }
    };
    k.trigger = k.publish;
    return k;

    function e(m) {
        if (l === 0 || (m.data && !!m.data.doNotStore)) {
            m = null;
            return;
        }
        var n = {
            eventName: m.eventName,
            appName: m.appName
        };
        n.data = m.passDataByRef ? m.data : c(m.data);
        if (l > 0) {
            if (j >= l) {
                j = 0;
            }
            a[j] = n;
            j++;
        } else {
            a.push(n);
        }
        m = null;
    }

    function c(n) {
        var m = {};
        if (n.constructor === Object) {
            for (var o in n) {
                if (n.hasOwnProperty(o) && n[o] !== null && n[o] !== undefined) {
                    if (typeof n[o] === "object" && n[o].constructor !== Array) {
                        m[o] = c(n[o]);
                    } else {
                        if (n[o].constructor === Array) {
                            m[o] = n[o].slice(0) || [];
                        } else {
                            if (typeof n[o] !== "function") {
                                m[o] = n[o] !== null && n[o] !== undefined ? n[o] : "";
                            }
                        }
                    }
                }
            }
        } else {
            if (n.constructor === Array) {
                m = n.slice(0) || [];
            } else {
                if (typeof n !== "function") {
                    m = n;
                }
            }
        }
        return m;
    }
};
window.lpTag = window.lpTag || {};
lpTag.RelManager = lpTag.RelManager || function () {
    if (this === window) {
        d("RelManager called without new", "ERROR", "lpTag.RelManager");
        return null;
    }
    var j = true;
    var g = this;
    var l = null;
    var h = "";
    var c = {
        domain: "",
        lpNumber: "",
        appKey: "",
        accessToken: ""
    };
    var b = ["xhr", "postmessage", "rest2jsonp"];

    function d(o, n, m) {
        if (lpTag && lpTag.utils && lpTag.utils.log) {
            lpTag.utils.log(o, n, m);
        }
    }
    this.clearData = function () {
        for (var m in c) {
            c[m] = "";
        }
        l = null;
        h = "";
    };
    this.setData = function (m) {
        l = {};
        h = "";
        for (var n in m) {
            if (m.hasOwnProperty(n)) {
                c[n] = m[n];
            }
        }
        if (m && typeof m.useJSON === "boolean") {
            j = m.useJSON;
        }
        if (m.transportOrder) {
            b = m.transportOrder;
        }
        m = null;
    };
    this.addRels = function (n, q) {
        n = e(n);
        if (!q) {
            n = null;
            return null;
        }
        l[q.type] = l[q.type] || {};
        var p = l[q.type];
        if (q.id) {
            p[q.id] = p[q.id] || {};
            p = p[q.id];
        }
        for (var m in n) {
            if (n.hasOwnProperty(m)) {
                if (n[m]["rel"]) {
                    p[n[m]["rel"]] = n[m]["href"];
                } else {
                    if (n[m]["@rel"]) {
                        p[n[m]["@rel"]] = n[m]["@href"];
                    }
                }
            }
        }
        if (q.data) {
            p.data = p.data || {};
            for (var o in q.data) {
                if (q.data.hasOwnProperty(o)) {
                    p.data[o] = q.data[o];
                }
            }
        }
        n = null;
        q = null;
        p = null;
    };
    this.removeRels = function (n) {
        var m = false;
        if (!n) {
            return null;
        }
        if (n.id && l[n.type] && l[n.type][n.id]) {
            l[n.type][n.id] = null;
            delete l[n.type][n.id];
            m = true;
        } else {
            if (l[n.type]) {
                l[n.type] = null;
                delete l[n.type];
                m = true;
            }
        }
        n = null;
        return m;
    };
    this.hasRel = function (n, o) {
        var m = this.getURI(n, o);
        o = null;
        return (m !== null && m !== "");
    };
    this.getURI = function (m, q) {
        if (!q || !l) {
            return null;
        }
        var o = null;
        var p;
        if (l[q.type]) {
            p = l[q.type];
            if (q.id && p[q.id]) {
                p = p[q.id];
            }
        }
        if (p) {
            o = p[m] || null;
            if (!o && p.data) {
                for (var n in p.data) {
                    if (p.data.hasOwnProperty(n)) {
                        o = this.getURI(m, {
                            type: n,
                            id: p.data[n],
                            needAuth: q.needAuth
                        });
                        if (o) {
                            break;
                        }
                    }
                }
            }
            if (!q.ignoreParameters) {
                o = o ? f(o, q.needAuth || false) : "";
            }
        }
        q = null;
        p = null;
        return o;
    };

    function f(n, m) {
        if (n.indexOf("v=1") < 0) {
            n += n.indexOf("?") > -1 ? "&" : "?";
            n += "v=1";
        }
        if (m && n.indexOf("&NC=true") < 0) {
            n += "&NC=true";
        }
        return n;
    }

    function i(m) {
        if (j && m.toLowerCase().indexOf(".json") < 0) {
            var n = m.indexOf("?");
            if (n > 0) {
                m = m.replace("?", ".json?");
            } else {
                m += ".json";
            }
        }
        return m;
    }
    this.buildRequestObj = function (m) {
        var n = a(m);
        if (!n) {
            m = null;
            n = null;
            return null;
        }
        if (m.queryParams && n.url) {
            n.url = m.queryParams ? n.url + k(m.queryParams) : n.url;
        }
        n.transportOrder = b.slice(0);
        m = null;
        return n;
    };

    function k(o) {
        var m = "";
        for (var p in o) {
            if (o.hasOwnProperty(p)) {
                if (o[p].constructor === Array) {
                    for (var n = 0; n < o[p].length; n++) {
                        m += "&" + p + "=" + o[p][n];
                    }
                } else {
                    m += "&" + p + "=" + o[p];
                }
            }
        }
        o = null;
        return m;
    }

    function e(o) {
        var m = [];
        for (var n in o) {
            if (o.link) {
                m = o.link.constructor === Array ? o.link : [o.link];
                o = null;
                return m;
            }
            if (o.hasOwnProperty(n) && m.length === 0) {
                if (n === "link") {
                    m = o[n].constructor === Array ? o[n] : [o[n]];
                    o = null;
                    return m;
                } else {
                    if (o[n] !== null && o[n] !== undefined && typeof o[n] === "object" && o[n].constructor !== Array) {
                        m = e(o[n]);
                    }
                }
            }
        }
        o = null;
        return m;
    }
    this.extractRels = e;

    function a(o) {
        var m = {
            AUTHORIZATION: "LivePerson appKey=" + c.appKey
        };
        if (c.accessToken) {
            m = {
                AUTHORIZATION: "Bearer " + c.accessToken
            };
        }
        var n = {
            url: "",
            method: "",
            headers: "",
            data: ""
        };
        if (!o.url) {
            n.url = (o.rel === "") ? "https://" + c.domain + "/api/account/" + c.lpNumber + "?v=1" : g.getURI(o.rel, {
                type: o.type,
                id: o.id,
                needAuth: o.needAuth
            });
        } else {
            n.url = o.url;
        } if (n.url) {
            n.url = i(n.url);
        }
        n.method = o.requestType || "GET";
        n.headers = m;
        m = null;
        if (n.method === "PUT") {
            n.headers["X-HTTP-Method-Override"] = "PUT";
            n.method = "POST";
        }
        if (n.method === "DELETE") {
            n.headers["X-HTTP-Method-Override"] = "DELETE";
            n.method = "POST";
        }
        if (o.data) {
            n.data = o.data;
        }
        o = null;
        if (n.url) {
            return n;
        } else {
            return null;
        }
    }
    this.setBaseUrl = function (m) {
        var n = (m.indexOf("http://") > -1) || (m.indexOf("https://") > -1);
        h = n ? m : "https://" + m;
    };
};
window.lpTag = window.lpTag || {};
lpTag.taglets = lpTag.taglets || {};
lpTag.taglets.lpAjax = (function (f) {
    var h = "1.0";
    var a = "lpAjax";
    var c = {};
    var e = false;

    function l() {
        e = true;
    }
    var b = function (n, o) {
        c[n] = o;
    };

    function g(o, n) {
        if (f.lpTag && lpTag.log) {
            lpTag.log(o, n, a);
        }
    }

    function j(o) {
        if (!e) {
            l();
        }
        try {
            var q = m(o);
            if (q) {
                q.issueCall(o);
                return true;
            } else {
                o.error.call(o.context || null, {
                    responseCode: 602,
                    error: "Transport - " + n + " - unable to issueCall request: " + o.url + " e=" + p,
                    body: "ERROR"
                });
            }
        } catch (p) {
            g("general exception in calling transport: " + p, "ERROR");
            if (typeof o.error == "function") {
                var n = "unknown";
                if (q && q.getName) {
                    n = q.getName();
                }
                try {
                    o.error.call(o.context || null, {
                        responseCode: 602,
                        error: "Transport - " + n + " - unable to issueCall request: " + o.url + " e=" + p,
                        body: "ERROR"
                    });
                } catch (p) {
                    g("Exception in execution of ERROR callback, type :" + n + " e=[" + p.message + "]", "ERROR");
                }
            }
        }
    }

    function d(p) {
        if (!e) {
            l();
        }
        for (var n in p) {
            var o = c[n];
            if (o) {
                o.configure(p[n]);
            }
        }
    }

    function m(r) {
        var n = false,
            o = -1;
        for (var q = 0; q < r.transportOrder.length; q++) {
            if (!n) {
                var s = i({}, r);
                var p = c[s.transportOrder[q]];
                if (p && p.isValidRequest && p.isValidRequest(s)) {
                    n = true;
                    o = q;
                }
            }
        }
        if (n) {
            return c[s.transportOrder[o]];
        } else {
            return null;
        }
    }

    function k(t, s, r) {
        if (t == null) {
            return;
        }
        var q = Array.prototype.forEach;
        if (q && t.forEach === q) {
            t.forEach(s, r);
        } else {
            if (t.length === +t.length) {
                for (var p = 0, n = t.length; p < n; p++) {
                    if (p in t && s.call(r, t[p], p, t) === {}) {
                        return;
                    }
                }
            } else {
                for (var o in t) {
                    if (Object.prototype.hasOwnProperty.call(t, o)) {
                        if (s.call(r, t[o], o, t) === {}) {
                            return;
                        }
                    }
                }
            }
        }
    }

    function i(n) {
        k(Array.prototype.slice.call(arguments, 1), function (o) {
            for (var p in o) {
                n[p] = o[p];
            }
        });
        return n;
    }
    return {
        getVersion: function () {
            return h;
        },
        getName: function () {
            return a;
        },
        init: l,
        issueCall: j,
        configureTransports: d,
        addTransport: b
    };
})(window);
window.lpTag = window.lpTag || {};
lpTag.taglets = lpTag.taglets || {};
lpTag.taglets.postmessage = lpTag.taglets.postmessage || (function (aa) {
    var ai = "1.0.5";
    var al = "postmessage";
    var g = true;
    var C = {};
    var s = {};
    var M = {};
    var A = 0;
    var p = 0;
    var n = 0;
    var ad = 0;
    var V = {};
    var S;
    var v = {
        DEBUG: "DEBUG",
        INFO: "INFO",
        ERROR: "ERROR"
    };
    var H = w(document.location.href);
    var Q = {
        progress: "progressLoad",
        completed: "completeLoad",
        error: "errorLoad",
        reloading: "reloading"
    };
    var af = {
        responseType: Q.error,
        responseCode: 404,
        message: "Request timed out on parent postMessage layer",
        name: "TIMEOUT"
    };
    var an = {
        responseType: Q.success,
        responseCode: 200,
        message: "iFrame has successfully loaded",
        name: "OK"
    };
    var f = {
        responseType: Q.error,
        responseCode: 418,
        message: "This iFrame is a teapot, not very useful for communication but lovely for earl grey",
        name: "TEAPOT"
    };
    var X = {
        timeout: 60000
    };
    O(aa, "message", c);
    var F = {
        VALIDATED: "valid",
        PENDING: "pending",
        FAILED: "failed"
    };

    function O(ap, ao, aq) {
        if (ap.addEventListener) {
            ap.addEventListener(ao, aq, false);
        } else {
            ap.attachEvent("on" + ao, aq);
        }
    }

    function q(ap, ao) {
        return {
            callId: ap,
            responseType: ao.responseType,
            responseCode: ao.responseCode,
            error: {
                message: ao.message,
                id: ao.responseCode,
                name: ao.name
            }
        };
    }

    function K(ap, ao, aq) {
        if (ap.removeEventListener) {
            ap.removeEventListener(ao, aq, false);
        } else {
            if (ap.detachEvent) {
                ap.detachEvent("on" + ao, aq);
            }
        }
    }

    function E(ao) {
        return ao + "_" + Math.floor(Math.random() * 100000) + "_" + Math.floor(Math.random() * 100000);
    }

    function w(ao) {
        var ar = new RegExp(/(http{1}s{0,1}?:\/\/)([^\/\?]+)(\/?)/ig);
        var aq, ap = null;
        if (ao.indexOf("http") === 0) {
            aq = ar.exec(ao);
        } else {
            aq = ar.exec(aa.location.href);
        } if (aq && aq.length >= 3 && aq[2] !== "") {
            ap = aq[1].toLowerCase() + aq[2].toLowerCase();
        }
        return ap;
    }

    function R(ap) {
        var ao = false,
            aq;
        if (!ap || !ap.url) {
            r("iFrame configuration empty or missing url parameter", v.ERROR, "addiFrameLocation");
            return ao;
        }
        aq = w(ap.url);
        if (C[aq] || V[aq]) {
            ao = false;
        } else {
            V[aq] = ap;
            ao = true;
        }
        return ao;
    }

    function t(ao) {
        var ap = w(ao.url);
        if (C[ap]) {
            return ac(ap, success, context);
        }
        var aq = E("fr");
        C[ap] = {
            elem: W(aq),
            url: ao.url,
            validated: F.PENDING,
            defaults: ao.defaults || {},
            delayLoad: isNaN(ao.delayLoad) ? 0 : ao.delayLoad,
            requestCount: 0,
            success: ao.callback || ao.success,
            error: ao.error,
            maxReloadRetries: ao.maxReloadRetries || 3,
            reloadInterval: ao.reloadInterval * 1000 || 30000
        };
        setTimeout(function () {
            u(ao.url, ap);
        }, C[ap].delayLoad);
        r("iFrame Queued to load " + ap, v.INFO, "addFrame");
        return F.PENDING;
    }

    function u(ap, ao) {
        C[ao].loadCallback = function (aq) {
            if (C[ao].iFrameOnloadTimeout) {
                clearTimeout(C[ao].iFrameOnloadTimeout);
                delete C[ao].iFrameOnloadTimeout;
            }
            y(ao, aq);
        };
        x(C[ao].elem, ap);
        O(C[ao].elem, "load", C[ao].loadCallback);
        C[ao].iFrameOnloadTimeout = setTimeout(C[ao].loadCallback, 5000);
        document.body.appendChild(C[ao].elem);
    }

    function J() {
        A = A + 1;
        n = n + 1;
    }

    function b(ap, ao) {
        M[ap] = M[ap] || [];
        M[ap].push(ao);
        ad = ad + 1;
        return true;
    }

    function ac(aq, ar, ap) {
        var ao = i(aq);
        d(ar, ap, ao);
        return C[aq].validated;
    }

    function W(ap) {
        var ao = document.createElement("IFRAME");
        ao.setAttribute("id", ap);
        ao.setAttribute("name", ap);
        ao.style.width = "0px";
        ao.style.height = "0px";
        ao.style.position = "absolute";
        ao.style.top = "-1000px";
        ao.style.left = "-1000px";
        return ao;
    }

    function aj(at, av, ap, ao, aq, au) {
        var ar = false;
        if (at && av && typeof av === "function") {
            s[at] = {
                success: av,
                error: ap,
                progress: ao,
                ctx: aq,
                launchTime: new Date(),
                timeout: (au + 1000) || X.timeout
            };
            ar = true;
        }
        return ar;
    }

    function z(ao) {
        if (s[ao]) {
            s[ao] = null;
            delete s[ao];
            return true;
        }
        return false;
    }

    function y(ap, ao) {
        r("onload validation called " + ap, v.INFO, "validateFrame");
        var aq = function (ar) {
            ag(ar, ap);
        };
        if (ao && ao.error) {
            ag(ao, ap);
        } else {
            setTimeout(function () {
                T({
                    domain: ap,
                    success: aq,
                    error: aq,
                    validation: true,
                    timeout: 100,
                    retries: -1,
                    defaults: C[ap].defaults
                });
            }, 500);
        }
        return true;
    }

    function ag(aq, ap) {
        var ao;
        var ar = C[ap];
        r("running validation of domain " + ap, v.INFO, "validateFrameCallback");
        if (ar) {
            C[ap].validated = aq && aq.error ? F.FAILED : F.VALIDATED;
            ao = (C[ap].validated === F.VALIDATED);
            if (ao) {
                h(ap, aq);
            } else {
                if (C[ap].reloadObj && C[ap].reloadObj.retriesLeft > 0) {
                    am(ap);
                } else {
                    a(ap);
                }
            }
        }
        ar = null;
        return ao;
    }

    function h(ap, ao) {
        r("FrameLoaded " + ap, v.INFO, "runFrameValidated");
        if (ao && ao.info && ao.info.defaults) {
            C[ap].defaults = ao.info.defaults;
        }
        an.domain = ap;
        d(C[ap].success, C[ap].context, an);
        m(ap);
        ab(ap, true);
    }

    function a(ap) {
        r("iFrame is a teapot " + ap, v.ERROR, "runFrameFailedToLoad");
        if (C[ap].error) {
            var ao = q(0, f);
            ao.domain = ap;
            d(C[ap].error, C[ap].context, ao);
        }
        N(ap);
        ab(ap, false);
    }

    function am(ao) {
        r("Retry " + ao, "info", "runReloadAttempt");
        ab(ao, false);
        P({
            origin: ao
        });
    }

    function ab(ap, ao) {
        r("Running buffer queue : " + ap + " loaded: " + ao, v.INFO, "runQueuedRequests");
        if (M[ap] && M[ap].length > 0) {
            do {
                var aq = M[ap].shift();
                if (ao) {
                    T(aq);
                } else {
                    d(aq.error, aq.context, {
                        responseCode: 600,
                        error: "Transport - postmessage - unable to run request: " + ap,
                        body: "ERROR"
                    });
                }
            } while (M[ap].length > 0);
            M[ap] = null;
            delete M[ap];
        }
    }

    function N(ao) {
        r("Cleaning up failed iFrame: " + ao, v.INFO, "cleanupIFrame");
        if (C[ao]) {
            K(C[ao].elem, "load", C[ao].loadCallback);
            C[ao].elem.parentNode.removeChild(C[ao].elem);
            C[ao] = null;
            delete C[ao];
        }
    }

    function Z(ap, aq, ao) {
        r("Frame not found for domain: " + ap, v.ERROR, "noFrameFound");
        d(aq, {
            responseCode: 600,
            error: "Transport - postmessage - unable to run request: " + ap,
            body: "ERROR"
        }, ao);
        return false;
    }

    function ae(ap) {
        var ao = false;
        if (aa.postMessage && aa.JSON) {
            if (ap && ap.success && ((ap.domain && ap.validation) || ap.url)) {
                ap.domain = ap.domain || w(ap.url);
                if (C[ap.domain] || V[ap.domain]) {
                    ao = true;
                }
            }
        }
        return ao;
    }

    function T(ap) {
        var ao = false;
        if (g && ae(ap)) {
            if (C[ap.domain]) {
                if (C[ap.domain].validated === F.PENDING && !ap.validation) {
                    ao = b(ap.domain, ap);
                    ad = ad + 1;
                } else {
                    ao = ah(ap);
                    if (ao) {
                        J();
                    } else {
                        s[ap.callId].timeout = 0;
                    }
                }
            } else {
                r("Adding iFrame to DOM - first request: " + ap.domain, v.INFO, "issueCall");
                ao = b(ap.domain, ap);
                t(V[ap.domain]);
                delete V[ap.domain];
            }
        } else {
            ao = Z(ap.domain, ap.error, ap.context);
        }
        return ao;
    }

    function ah(ap) {
        var ao, at = false;
        ap = D(ap);
        ao = e(ap);
        ao = JSON.stringify(ao);
        r("sending msg to domain " + ap.domain, v.DEBUG, "sendRequest");
        var aq = (ap.timeout * (ap.retries + 1)) + 2000;
        aj(ap.callId, ap.success, ap.error, ap.progress, ap.context, aq);
        try {
            at = ak(ap.domain, ao);
            S = setTimeout(Y, 1000);
        } catch (ar) {
            r("Error trying to send message", v.ERROR, "sendMessageToFrame");
            at = false;
        }
        return at;
    }

    function D(ao) {
        ao.callId = E("call");
        ao.returnDomain = H;
        if (ao.progress) {
            ao.fireProgress = true;
        }
        ao.headers = ao.headers || {};
        ao.headers["LP-URL"] = aa.location.href;
        return ao;
    }

    function ak(ar, ap) {
        var ao = false;
        try {
            C[ar].elem.contentWindow.postMessage(ap, ar);
            C[ar].requestCount = C[ar].requestCount + 1;
            ao = true;
        } catch (aq) {
            r("Error trying to send message", v.ERROR, "sendMessageToFrame");
        }
        return ao;
    }

    function Y() {
        if (S) {
            clearTimeout(S);
        }
        S = null;
        var ao = new Date();
        var at = 0;
        var au = [];
        for (var aq in s) {
            if (s.hasOwnProperty(aq) && s[aq].launchTime) {
                var ar = ao - s[aq].launchTime;
                if (ar > s[aq].timeout) {
                    au.push(aq);
                } else {
                    at = at + 1;
                }
            }
        }
        if (au.length) {
            r("Checking errors found " + au.length + " timeout callbacks to call", v.DEBUG, "checkForErrors");
            for (var ap = 0; ap < au.length; ap++) {
                G(q(au[ap], af));
            }
        }
        if (at > 0) {
            S = setTimeout(Y, 1000);
        }
        return true;
    }

    function e(aq) {
        var ar = {};
        if (aq.constructor === Object) {
            for (var ap in aq) {
                try {
                    if (aq.hasOwnProperty(ap) && typeof aq[ap] !== "function") {
                        ar[ap] = aq[ap];
                    }
                } catch (ao) {
                    r("Error creating request object data clone", v.ERROR, "cloneSimpleObj");
                }
            }
        } else {
            if (aq.constructor === Array) {
                ar = aq.slice(0) || [];
            } else {
                if (typeof aq !== "function") {
                    ar = aq;
                }
            }
        }
        return ar;
    }

    function G(au, aq) {
        if ((au.callId && s[au.callId]) || au.responseType === Q.reloading) {
            var ap = s[au.callId];
            var at, ao = false;
            try {
                switch (au.responseType) {
                    case Q.completed:
                        at = ap.success;
                        ao = true;
                        break;
                    case Q.error:
                        at = ap.error;
                        ao = true;
                        p = p + 1;
                        break;
                    case Q.progress:
                        at = ap.progress;
                        break;
                    case Q.reloading:
                        au.origin = aq;
                        at = P;
                        break;
                }
                if (ao) {
                    z(au.callId);
                    o(au);
                    n = n > 0 ? 0 : n - 1;
                }
                if (at && typeof at === "function") {
                    d(at, (ap && ap.ctx) || null, au);
                }
                at = null;
                ap = null;
            } catch (ar) {
                r("Error in executing callback", "ERROR", "runCallback");
                return false;
            }
        }
        return true;
    }

    function P(ao) {
        r("Got reload request from " + ao.origin, v.DEBUG, "handleReloadState");
        C[ao.origin].validated = F.PENDING;
        if (!C[ao.origin].reloadObj) {
            r("Creating reloadObj" + ao.origin, v.DEBUG, "handleReloadState");
            C[ao.origin].reloadObj = l(ao.origin);
        }
        L(ao.origin);
    }

    function L(ao) {
        r("Reload try for domain " + ao + " ,retries left " + C[ao].reloadObj.retriesLeft, v.DEBUG, "reloadIFrame");
        C[ao].reloadObj.retriesLeft = C[ao].reloadObj.retriesLeft - 1;
        if (C[ao].reloadObj.setLocationTimeout) {
            clearTimeout(C[ao].reloadObj.setLocationTimeout);
        }
        if (C[ao].reloadObj.retry) {
            C[ao].reloadObj.setLocationTimeout = setTimeout(j(ao), C[ao].reloadInterval);
        } else {
            C[ao].reloadObj.retry = true;
            j(ao)();
        }
    }

    function j(ao) {
        return function () {
            C[ao].iFrameOnloadTimeout = setTimeout(function () {
                y(ao, {
                    error: {
                        code: 404,
                        message: "Frame did not trigger load"
                    }
                });
            }, 5000);
            x(C[ao].elem, C[ao].url);
        };
    }

    function x(ao, ap) {
        ap += (ap.indexOf("?") > 0 ? "&bust=" : "?bust=");
        ap += new Date().getTime();
        r("Setting iFrame to URL: " + ap, v.INFO, "setIFrameLocation");
        ao.setAttribute("src", ap);
    }

    function l(ao) {
        r("Creating reload object " + ao, v.INFO, "createReloadObject");
        var ap = C[ao].maxReloadRetries;
        return {
            retriesLeft: ap
        };
    }

    function m(ao) {
        r("Cleaning up reload object for this instance" + ao, v.INFO, "cleanUpReloadObject");
        if (C[ao].reloadObj) {
            if (C[ao].reloadObj.setLocationTimeout) {
                clearTimeout(C[ao].reloadObj.setLocationTimeout);
            }
            C[ao].reloadObj = null;
            delete C[ao].reloadObj;
        }
    }

    function o(aq) {
        var ap = ["callId", "responseType"];
        for (var ao = 0; ao < ap.length; ao++) {
            aq[ap[ao]] = null;
            delete aq[ap[ao]];
        }
    }

    function i(ap) {
        var ao;
        if (C[ap]) {
            ao = {
                domain: ap,
                url: C[ap].url,
                validated: C[ap].validated,
                requestCount: C[ap].requestCount,
                defaults: C[ap].defaults,
                started: (C[ap].validated === F.VALIDATED)
            };
        }
        return ao;
    }

    function U() {
        var ao = {};
        for (var ap in C) {
            if (C.hasOwnProperty(ap)) {
                ao[C[ap].domain] = i(C[ap].domain);
            }
        }
        return ao;
    }

    function d(aq, ao, ar) {
        if (aq && typeof aq === "function") {
            try {
                aq.call(ao || null, ar);
            } catch (ap) {
                r("Error in executing callback", v.ERROR, "runCallback");
            }
        }
    }

    function c(aq) {
        var ap, ar;
        try {
            ar = aq.origin;
            if (!C[ar]) {
                return;
            }
            ap = JSON.parse(aq.data);
            if (ap.body && typeof ap.body === "string") {
                try {
                    ap.body = JSON.parse(ap.body);
                } catch (ao) {
                    r("Error in parsing message body from frame, origin: " + ar, v.DEBUG, "handleMessageFromFrame");
                }
            }
        } catch (ao) {
            ap = null;
            r("Error in handeling message from frame, origin: " + ar, v.ERROR, "handleMessageFromFrame");
        }
        if (ap && (ap.callId || ap.responseType === Q.reloading)) {
            G(ap, ar);
        }
    }

    function r(aq, ap, ao) {
        if (aa.lpTag && lpTag.log) {
            lpTag.log(aq, ap, ao);
        }
    }

    function I(ao) {
        if (ao) {
            if (ao.frames) {
                ao.frames = ao.frames.constructor === Array ? ao.frames : [ao.frames];
                for (var aq = 0; aq < ao.frames.length; aq++) {
                    R(ao.frames[aq]);
                }
            }
            if (ao.defaults) {
                for (var ap in ao.defaults) {
                    if (X.hasOwnProperty(ap) && ao.defaults.hasOwnProperty(ap)) {
                        X[ap] = ao.defaults[ap];
                    }
                }
            }
        }
        g = true;
    }
    var B = {
        init: function () {
            if (lpTag && lpTag.taglets && lpTag.taglets.lpAjax) {
                try {
                    lpTag.taglets.lpAjax.addTransport(al, B);
                } catch (ao) { }
            }
        },
        issueCall: T,
        isValidRequest: ae,
        getVersion: function () {
            return ai;
        },
        getName: function () {
            return al;
        },
        configure: I,
        getFrameData: i,
        inspect: function () {
            return {
                name: al,
                version: ai,
                callsMade: A,
                errorsFound: p,
                pending: n,
                defaults: X,
                iFrameList: e(V),
                activeFrames: U()
            };
        }
    };
    if (lpTag && lpTag.taglets && lpTag.taglets.lpAjax) {
        try {
            lpTag.taglets.lpAjax.addTransport(al, B);
        } catch (k) { }
    }
    return B;
})(window);
window.lpTag = window.lpTag || {};
lpTag.taglets = lpTag.taglets || {};
lpTag.taglets.jsonp = lpTag.taglets.jsonp || (function (E) {
    var x = {
        callback: "cb",
        encoding: "UTF-8",
        timeout: 10000,
        retries: 2
    };
    var p = {
        ERROR: "ERROR",
        DEBUG: "DEBUG",
        INFO: "INFO"
    };
    var b = true;
    var d = false;
    var k = 2083;
    var i = "lpCb";
    var M = {};
    var m = 0;
    var e = 0;
    var q = 0;
    var v = 0;
    var K;
    var y = {};
    var f = C().length;
    var J = "1.0.5";
    var L = "jsonp";

    function a(P) {
        if (typeof P === "string") {
            var O = P;
            P = {
                url: O
            };
        }
        if (!P.url) {
            return false;
        }
        P.encoding = P.encoding || x.encoding;
        P.callback = P.callback || x.callback;
        P.retries = typeof P.retries === "number" ? P.retries : x.retries;
        P.timeout = P.timeout ? P.timeout : x.timeout;
        return P;
    }

    function C() {
        return Math.round(Math.random() * 99999) + "x" + Math.round(Math.random() * 99999);
    }

    function s() {
        return "scr" + Math.round(Math.random() * 999999999) + "_" + Math.round(Math.random() * 999999999);
    }

    function I(P) {
        var O = false;
        if (b && P && P.url) {
            var Q = 2 + (P.callback || x.callback).length + P.url.length + f + G(P.data).length;
            if (Q < k) {
                O = true;
            }
        }
        return O;
    }

    function G(Q) {
        var S = "";
        if (typeof Q === "string") {
            S += Q;
        } else {
            var R = true;
            for (var O in Q) {
                var P;
                if (typeof Q[O] == "object") {
                    P = JSON.stringify(Q[O]);
                } else {
                    if (typeof Q[O] !== "function") {
                        P = Q[O];
                    }
                } if (P) {
                    if (!R) {
                        S += "&";
                    }
                    S += encodeURIComponent(O) + "=" + encodeURIComponent(P);
                    R = false;
                }
            }
        }
        return S;
    }

    function z(P) {
        var O;
        if (I(P)) {
            P = a(P);
            P.callbackName = i + C();
            O = P.url + (P.url.indexOf("?") > -1 ? "&" : "?") + P.callback + "=" + P.callbackName + "&" + G(P.data);
            P.callUrl = O;
            if (o(P)) {
                j(P);
                u();
            } else {
                h("URL request was too long and was not sent, url: " + O, p.ERROR, "issueCall");
            }
        } else {
            h("URL request was too long and was not sent, url: " + O, p.ERROR, "issueCall");
            if (P && P.error) {
                c("ERROR", P.error, {
                    responseCode: 600,
                    error: "Transport - JSONP - unable to run request: " + P.url,
                    body: "ERROR"
                }, P.context);
            }
            return false;
        }
        return true;
    }

    function o(P) {
        var O = false;
        var S = new RegExp(/(http{1}s{0,1}?:\/\/)([^\/\?]+)(\/?)/ig);
        var R;
        if (P.callUrl.indexOf("http") === 0) {
            R = S.exec(P.callUrl);
        } else {
            R = S.exec(E.location.href);
        } if (R && R.length >= 3 && R[2] !== "") {
            var Q = R[2].toLowerCase();
            P.domainMatch = Q;
            M[Q] = M[Q] || [];
            M[Q].inFlight = M[Q].inFlight || 0;
            M[Q].push(P);
            O = true;
            e = e + 1;
            h("buffered URL: " + P.callUrl, p.DEBUG, "lpTag.taglets.jsonp.bufferRequest");
        } else {
            h("NO MATCH for URL: " + P.callUrl, p.ERROR, "lpTag.taglets.jsonp.bufferRequest");
        }
        return O;
    }

    function u() {
        var R;
        for (var O in M) {
            if (M.hasOwnProperty(O)) {
                R = M[O];
                var Q = false;
                while (!Q && R.inFlight < 6 && R.length > 0) {
                    var P = R.shift();
                    if (P) {
                        h("Sent URL: " + P.callUrl, p.DEBUG, "lpTag.taglets.jsonp.sendRequests");
                        P.scriptId = F(P.callUrl, P.encoding, P.callbackName);
                        B(O, P.callbackName, P.timeout);
                        e = e - 1;
                    } else {
                        Q = true;
                    }
                }
            }
        }
        R = null;
    }

    function A() {
        clearTimeout(K);
        K = null;
        var O = new Date();
        for (var P in y) {
            if (y.hasOwnProperty(P) && y[P].launchTime) {
                var Q = O - y[P].launchTime;
                if (y[P].loadTime || Q > y[P].timeout) {
                    E[P].apply(null, [{
                        responseCode: 404,
                        error: {
                            message: "Request timed out",
                            id: 404,
                            name: "timeout"
                        }
                    },
                        true
                    ]);
                }
            }
        }
        if (q > 0) {
            K = setTimeout(A, 1000);
        }
    }

    function F(O, P, Q) {
        var S = s();
        var R = document.createElement("script");
        R.setAttribute("type", "text/javascript");
        R.setAttribute("charset", P);
        R.onload = function () {
            if (y[Q]) {
                y[Q].loadTime = new Date();
            }
            this.onload = this.onerror = this.onreadystatechange = null;
        };
        if (!E.addEventListener) {
            R.onreadystatechange = function () {
                if (this.readyState) {
                    if (this.readyState === "loaded" || this.readyState === "complete") {
                        if (y[Q]) {
                            y[Q].loadTime = new Date();
                        }
                        this.onload = this.onerror = this.onreadystatechange = null;
                    }
                }
            };
        } else {
            R.onerror = function () {
                if (y[Q]) {
                    y[Q].loadTime = new Date();
                }
                this.onload = this.onerror = this.onreadystatechange = null;
            };
        }
        R.setAttribute("src", O);
        R.setAttribute("id", S);
        document.getElementsByTagName("head")[0].appendChild(R);
        if (!K) {
            K = setTimeout(A, 1000);
        }
        R = null;
        return S;
    }

    function B(O, Q, P) {
        M[O].inFlight = M[O].inFlight + 1;
        y[Q] = {
            launchTime: new Date(),
            timeout: P
        };
        q = q + 1;
        m = m + 1;
    }

    function t(R) {
        var Q;
        while (Q = document.getElementById(R)) {
            try {
                Q.parentNode.removeChild(Q);
                for (var P in Q) {
                    if (Q.hasOwnProperty(P)) {
                        delete Q[P];
                    }
                }
            } catch (O) {
                h("error when removing script", p.ERROR, "removeScript");
            }
        }
    }

    function D(O) {
        M[O].inFlight = M[O].inFlight - 1;
        q = q - 1;
    }

    function w(Q, P, O) {
        t(P.scriptId);
        D(P.domainMatch);
        N(P.callbackName, O);
        if (O) {
            H(Q, P);
        } else {
            l(P);
            c("callback", P.success, Q, P.context);
            P = null;
            u();
        }
    }

    function H(P, O) {
        v = v + 1;
        if (O.retries > 0) {
            O.retries = O.retries - 1;
            z(O);
        } else {
            l(O);
            c("ERROR", O.error, P || {
                responseCode: 404,
                error: {
                    id: 404,
                    name: "timeout",
                    message: "Request has timed out on all retries"
                }
            }, O.context);
            O = null;
            u();
        }
    }

    function l(Q) {
        var P = ["callUrl", "retries", "id", "requestTimeout", "type", "encoding", "launchTime", "callbackName", "domainMatch"];
        for (var O = 0; O < P.length; O++) {
            if (Q.hasOwnProperty(P[O])) {
                Q[P[O]] = null;
                delete Q[P[O]];
            }
        }
    }

    function c(P, S, Q, O) {
        if (typeof S === "function") {
            try {
                S.call(O || null, Q);
                S = null;
            } catch (R) {
                h("Exception in execution of callback, type :" + P + " e=[" + R.message + "]", p.ERROR, "runCallback");
            }
        } else {
            h("No callback, of type :" + P, p.INFO, "runCallback");
        }
    }

    function N(Q, O) {
        y[Q] = null;
        delete y[Q];
        if (O === true) {
            E[Q] = function () {
                E[Q] = null;
                try {
                    delete E[Q];
                } catch (R) { }
            };
        } else {
            E[Q] = null;
            try {
                delete E[Q];
            } catch (P) { }
        }
    }

    function j(O) {
        E[O.callbackName] = function (Q, P) {
            w(Q, O, P);
        };
    }

    function h(Q, P, O) {
        if (d === true || P === "ERROR") {
            if (E.lpTag && lpTag.log) {
                lpTag.log(Q, P, O);
            }
        }
    }

    function r(O) {
        if (O) {
            for (var P in O) {
                if (x.hasOwnProperty(P) && O.hasOwnProperty(P)) {
                    x[P] = O[P];
                }
            }
            d = typeof O.debugMode === "boolean" ? O.debugMode : d;
        }
    }
    var n = {
        init: function () {
            if (lpTag && lpTag.taglets && lpTag.taglets.lpAjax) {
                try {
                    lpTag.taglets.lpAjax.addTransport(L, n);
                } catch (O) { }
            }
        },
        configure: r,
        issueCall: z,
        isValidRequest: I,
        getVersion: function () {
            return J;
        },
        getName: function () {
            return L;
        },
        inspect: function () {
            return {
                name: L,
                version: J,
                callsMade: m,
                errorsFound: v,
                pending: q,
                buffered: e,
                defaults: (function () {
                    var P = {};
                    for (var O in x) {
                        if (x.hasOwnProperty(O)) {
                            P[O] = x[O];
                        }
                    }
                    return P;
                })()
            };
        }
    };
    if (lpTag && lpTag.taglets && lpTag.taglets.lpAjax) {
        try {
            lpTag.taglets.lpAjax.addTransport(L, n);
        } catch (g) { }
    }
    return n;
})(window);
window.lpTag = window.lpTag || {};
lpTag.taglets = lpTag.taglets || {};
lpTag.taglets.xhr = lpTag.taglets.xhr || (function lpXHR(L) {
    var I = {
        JSON: "application/json",
        JAVASCRIPT: "text/javascript",
        HTML: "text/html",
        XMLAPP: "application/xml",
        XMLTEXT: "text/xml",
        FORM: "application/x-www-form-urlencoded;"
    };
    var x = {
        GET: "GET",
        POST: "POST",
        PUT: "PUT",
        DELETE: "DELETE"
    };
    var B = {
        PROGRESS: "progress",
        LOAD: "load",
        ERROR: "error",
        ABORT: "abort",
        READYSTATE: "readystatechange"
    };
    var w = {
        ERROR: "ERROR",
        DEBUG: "DEBUG",
        INFO: "INFO"
    };
    var G = {
        encoding: "UTF-8",
        method: x.GET,
        asynch: true,
        timeout: 30000,
        mimeType: I.JSON,
        cache: false,
        acceptHeader: "*/*",
        XMLHTTPOverride: true,
        retries: 2
    };
    var m = {
        UNSENT: 0,
        OPENED: 1,
        HEADERS_RECIEVED: 2,
        LOADING: 3,
        COMPLETE: 4
    };
    var aa = "xhr";
    var Z = "1.0.2";
    var k = o(document.location.href);
    var g = "X-HTTP-Method-Override";
    var T = {
        responseCode: 404,
        HTTPStatus: "bad request",
        body: {
            error: "Request timed out"
        },
        headers: ""
    };
    var F = {
        responseCode: 600,
        HTTPStatus: "unable to service request",
        body: {
            error: "Transport - " + aa + " - unable to run request"
        },
        headers: ""
    };
    var p = !L.addEventListener;
    var J = 2083;
    var s = 0,
        f = 0;
    var y = [];
    var P = [];
    var Y;
    var c = true;

    function A(ae) {
        if (ae && ae.defaults) {
            for (var ad in ae.defaults) {
                if (ae.defaults.hasOwnProperty(ad) && G.hasOwnProperty(ad)) {
                    G[ad] = ae.defaults[ad];
                }
            }
        }
        c = true;
    }

    function b(ad) {
        ad.method = ad.method || G.method;
        ad.encoding = ad.encoding || G.encoding;
        ad.mimeType = ad.mimeType || G.mimeType;
        ad.retries = !isNaN(ad.retries) ? ad.retries : G.retries;
        ad.timeout = ad.timeout || G.timeout;
        ad.XMLHTTPOverride = typeof ad.XMLHTTPOverride == "boolean" ? ad.XMLHTTPOverride : G.XMLHTTPOverride;
        ad.cache = typeof ad.cache === "boolean" ? ad.cache : G.cache;
        ad.asynch = typeof ad.asynch === "boolean" ? ad.asynch : G.asynch;
        if ((ad.method.toLowerCase() === x.PUT.toLocaleLowerCase() || ad.method.toLowerCase() === x.DELETE.toLocaleLowerCase()) && ad.XMLHTTPOverride) {
            ad.headers[g] = ad.method;
            ad.method = x.POST;
        }
        return ad;
    }

    function u(ae) {
        var ad = ae.url.indexOf("__d=");
        if (ad > -1) {
            ae.url = ae.url.substr(0, ad - 1);
        }
        if (!ae.cache && (ae.method.toLowerCase() !== x.GET.toLowerCase() || ae.url.length <= (J - 10))) {
            ae.url += ae.url.indexOf("?") > 0 ? "&__d=" : "?__d=";
            ae.url += Math.floor(Math.random() * 100000);
        }
    }

    function V(af) {
        af = b(af);
        var ae = false;
        if (af && af.url && typeof af.url === "string") {
            if (k == o(af.url)) {
                if (af.method.toLowerCase() === x.GET.toLowerCase()) {
                    var ad = J,
                        ag = "";
                    if (af.url.indexOf("http") !== 0) {
                        af.url = K(af.url);
                    }
                    if (af.data) {
                        ag = Q(af.data);
                    }
                    if (!af.cache) {
                        ad = ad - 10;
                    }
                    if ((af.url.length + 1 + ag.length) <= ad) {
                        ae = true;
                    }
                } else {
                    ae = true;
                }
            }
        }
        return (c && ae);
    }

    function K(af) {
        var ae = document.location.href.lastIndexOf("/") || document.location.href.lastIndexOf("?");
        var ad = document.location.href.substr(0, ae);
        af = ad + "/" + af;
        return af;
    }

    function o(ad) {
        var ag = new RegExp(/(http{1}s{0,1}?:\/\/)([^\/\?]+)(\/?)/ig);
        var af, ae = null;
        if (ad.indexOf("http") === 0) {
            af = ag.exec(ad);
        } else {
            af = ag.exec(L.location.href);
        } if (af && af.length >= 3 && af[2] !== "") {
            ae = af[1].toLowerCase() + af[2].toLowerCase();
        }
        return ae;
    }

    function q(ah, ag, ae) {
        ag = ag || {};
        ag["Content-Type"] = z(ag["Content-Type"] || G.mimeType, ae.encoding);
        ag.Accept = ag.Accept || G.acceptHeader;
        ag["X-Requested-With"] = "XMLHttpRequest";
        if (ag) {
            for (var ad in ag) {
                if (ag.hasOwnProperty(ad)) {
                    try {
                        ah.setRequestHeader(ad, ag[ad]);
                    } catch (af) { }
                }
            }
        }
        return ah;
    }

    function z(ae, af) {
        var ad = ae;
        if (!ad || ad == "" || ad.indexOf("charset") < 0) {
            ad = (ad || G.MIMETYPE) + "; charset=" + (af || G.encoding);
        }
        return ad;
    }

    function H(ad) {
        if (V(ad)) {
            C(ad);
        } else {
            if (ad && ad.error) {
                j(ad.error, F, ad.context);
            }
        }
    }

    function D(af) {
        var ag = p ? O(af) : E(af);
        if (ag.overrideMimeType) {
            ag.overrideMimeType(af.mimeType);
        }
        if (af.method === x.GET && af.data) {
            var ae = Q(af.data);
            if (ae) {
                af.url += af.url.indexOf("?") < 0 ? "?" : "&";
                af.url += ae;
            }
            af.data = null;
        }
        u(af);
        try {
            if (typeof af.data !== "undefined") {
                af.data = l(af.data);
            }
        } catch (ad) {
            j(af.error, {
                responseCode: 601,
                HTTPStatus: "Unable to service request",
                headers: "",
                body: {
                    error: "unable to stringify data for requests"
                }
            }, af.context);
            U(ag);
            return false;
        }
        af.xhr = ag;
        ag.open(af.method, af.url, af.asynch);
        ag = q(ag, af.headers, af);
        N(af);
        ag.send(af.data || null);
        if (!Y) {
            setTimeout(S, 1000);
        }
        return true;
    }

    function N(ad) {
        ad.launchTime = new Date();
        P.push(ad);
        M();
    }

    function C(ad) {
        y.push(ad);
        R();
    }

    function S() {
        if (Y) {
            clearTimeout(Y);
            Y = null;
        }
        var ae = new Date();
        var ai = [];
        for (var af = 0; af < P.length; af++) {
            var ah = P[af];
            var ag = ae.valueOf() - ah.launchTime.valueOf();
            if (ag > ah.timeout) {
                ab(ah, ah.xhr, true);
                ai.push(ah);
            }
        }
        for (var ad = 0; ad < ai.length; ad++) {
            W(ai[ad]);
            ai[ad] = null;
        }
        if (y.length > 0 || P.length > 0) {
            Y = setTimeout(S, 1000);
        }
    }

    function ab(ag, ah, af, ae) {
        X();
        if (!af) {
            W(ag);
        }
        e(ag);
        if (af && ah) {
            try {
                ah.abort();
            } catch (ad) {
                i("Error when trying to abort request " + ag.url, w.ERROR);
            }
        }
        if (ah) {
            U(ah);
        }
        if (ag.retries > 0 && !ae) {
            ag.retries = ag.retries - 1;
            C(ag);
        } else {
            j(ag.error, ae || T, ag.context);
        }
    }

    function Q(af) {
        var ah = "";
        if (typeof af === "string") {
            ah += af;
        } else {
            var ag = true;
            for (var ad in af) {
                var ae;
                if (typeof af[ad] == "object") {
                    ae = encodeURIComponent(JSON.stringify(af[ad]));
                } else {
                    if (typeof af[ad] !== "function") {
                        ae = encodeURIComponent(af[ad]);
                    }
                } if (ae) {
                    if (!ag) {
                        ah += "&";
                    }
                    ah += encodeURIComponent(ad) + "=" + encodeURIComponent(ae);
                    ag = false;
                }
            }
        }
        return ah;
    }

    function e(af) {
        var ae = ["xhr"];
        for (var ad = 0; ad < ae.length; ad++) {
            af[ae[ad]] = null;
            delete af[ae[ad]];
        }
    }

    function l(ad) {
        if (typeof ad === "object") {
            return JSON.stringify(ad);
        } else {
            if (typeof ad === "string") {
                return ad;
            } else {
                return "";
            }
        }
    }

    function R() {
        while (P.length < 6 && y.length > 0) {
            D(y.shift());
        }
    }

    function j(ag, ae, ad) {
        if (ag && typeof ag == "function") {
            try {
                ag.call(ad, ae);
            } catch (af) {
                i(af.message);
            }
        }
    }

    function i(ae, ad) {
        if (L.lpTag && lpTag.log) {
            lpTag.log(ae, ad, name);
        }
    }

    function E(ad) {
        var ae = new L.XMLHttpRequest();
        ae.addEventListener(B.LOAD, function () {
            if (ad && !ad.completed) {
                n(this, ad);
                ad.completed = true;
            } else {
                ad = null;
            }
        });
        ae.addEventListener(B.PROGRESS, function () {
            var af = v(this);
            if (af && ad.progress) {
                j(ad.progress, af, ad.context);
            }
        });
        ae.addEventListener(B.ERROR, function () {
            if (ad && !ad.completed) {
                h(this, ad);
                ad.completed = true;
            } else {
                ad = null;
            }
        });
        ae.addEventListener(B.READYSTATE, function () {
            if (this.readyState === m.COMPLETE) {
                if (ad && !ad.completed) {
                    n(this, ad);
                    ad.completed = true;
                } else {
                    ad = null;
                }
            }
        });
        return ae;
    }

    function O(ad) {
        var ae = new L.XMLHttpRequest();
        ae[B.READYSTATE] = function () {
            if (!ad.completed) {
                if (this.readyState === m.COMPLETE) {
                    n(this, ad);
                    ad.completed = true;
                }
            }
        };
        ae[B.ERROR] = function () {
            h(this, ad);
        };
        return ae;
    }

    function U(ad) {
        ad[B.READYSTATE] = null;
        ad[B.ERROR] = null;
        if (!p) {
            ad[B.PROGRESS] = null;
            ad[B.LOAD] = null;
        }
        ad = null;
    }

    function v(ag) {
        var ae, ad;
        if (ag) {
            try {
                ad = ac(ag);
                ae = {
                    responseCode: r(ag, "status") || 404,
                    HTTPStatus: r(ag, "statusText"),
                    body: r(ag, "responseText", "string") || r(ag, "responseXML"),
                    headers: ad.headers,
                    originalHeader: ad.originalHeader
                };
            } catch (af) {
                i(af.message, w.ERROR);
                return {};
            }
        } else {
            i("No data from request", w.INFO);
            return {};
        } if (ae && ae.headers && ae.headers["Content-Type"] && ae.headers["Content-Type"] === I.JSON) {
            try {
                ae.body = JSON.parse(ae.body);
            } catch (af) { }
        }
        ae.responseCode = ae.responseCode == 1223 ? 204 : ae.responseCode;
        if (ae.responseCode >= 12000) {
            ae.internalCode = ae.responseCode;
            ae.responseCode = 500;
        }
        return ae;
    }

    function r(ag, ad, ae) {
        try {
            if (ae) {
                if (typeof ag[ad] === ae) {
                    return ag[ad];
                } else {
                    return "";
                }
            } else {
                return ag[ad];
            }
        } catch (af) {
            return "";
        }
    }

    function ac(ak) {
        var ah, ag = {};
        try {
            ah = ak.getAllResponseHeaders();
        } catch (ai) {
            ah = "";
        }
        if (ah) {
            var af = ah.split("\n");
            for (var ae = 0; ae < af.length; ae++) {
                try {
                    var ad = af[ae].split(":");
                    if (ad[0]) {
                        var aj = ad.length > 2 ? ad.slice(1).join(":") : ad[1];
                        ag[a(ad[0])] = a(aj);
                    }
                } catch (ai) { }
            }
        }
        return {
            originalHeader: ah,
            headers: ag
        };
    }

    function a(ad) {
        ad = ad.replace(/^[\s\r]*/, "");
        ad = ad.replace(/[\s\r]*$/g, "");
        return ad;
    }

    function n(ag, af) {
        e(af);
        var ae = v(ag);
        if (ae) {
            var ad;
            if (ae.responseCode > 399) {
                ab(af, ag, false, ae);
            } else {
                W(af);
                ad = af.success;
                j(ad, ae, af.context);
                U(ag);
            }
        }
        R();
    }

    function h(ae, ad) {
        ab(ad, ae);
        R();
    }

    function X() {
        f = f + 1;
    }

    function M() {
        s = s + 1;
    }

    function W(ae) {
        for (var ad = 0; ad < P.length; ad++) {
            if (P[ad] === ae) {
                P.splice(ad);
                break;
            }
        }
    }
    if (p) {
        B.READYSTATE = "on" + B.READYSTATE;
        B.ERROR = "on" + B.ERROR;
    }
    var t = {
        init: function () {
            if (lpTag && lpTag.taglets && lpTag.taglets.lpAjax) {
                try {
                    lpTag.taglets.lpAjax.addTransport(aa, t);
                } catch (ad) { }
            }
        },
        configure: A,
        issueCall: H,
        isValidRequest: V,
        getVersion: function () {
            return Z;
        },
        getName: function () {
            return aa;
        },
        inspect: function () {
            return {
                name: aa,
                version: Z,
                callsMade: s,
                errorsFound: f,
                pending: P.length,
                defaults: (function () {
                    var ae = {};
                    for (var ad in G) {
                        if (G.hasOwnProperty(ad)) {
                            ae[ad] = G[ad];
                        }
                    }
                    return ae;
                })()
            };
        }
    };
    if (lpTag && lpTag.taglets && lpTag.taglets.lpAjax) {
        try {
            lpTag.taglets.lpAjax.addTransport(aa, t);
        } catch (d) { }
    }
    return t;
})(window);
window.lpTag = window.lpTag || {};
lpTag.taglets = lpTag.taglets || {};
lpTag.taglets.rest2jsonp = lpTag.taglets.rest2jsonp || (function () {
    var Q = "1.0";
    var C = "1";
    var R = "rest2jsonp";
    var n = 0,
        i = 0,
        h = 0;
    var A = {
        retries: 3,
        method: "GET",
        timeout: 30000,
        encoding: "UTF-8",
        callback: "cb"
    };
    var q = {
        prefix: "https://",
        middle: "/api/account/",
        suffix: "/js2rest"
    };
    var f = "";
    var E = {
        site: "",
        domain: ""
    };
    var k = "1";
    var m = 32;
    var M = {
        PART_PARAM: "lpP",
        OUTOF_PARAM: "lpO",
        SECURE_IDENTIFIER: "lpS"
    };
    var j = {
        VERSION_PARAM: "lpV",
        BODY_PARAM: "lpB",
        JSON: "lpjson",
        CALLID: "lpCallId"
    };
    var S = F().length;
    var D = 2083;
    var d = y();
    var I = u();
    var l = {};

    function x(V) {
        for (var U in A) {
            if (A.hasOwnProperty(U)) {
                V[U] = V[U] || A[U];
            }
        }
        V.data = w(V);
        V.url = f + a(V.data.u);
        return V;
    }

    function w(U) {
        return {
            u: J(U.url),
            m: U.method || A.method,
            b: U.data || "",
            h: U.headers || ""
        };
    }

    function J(U) {
        if (U && U.indexOf(".json?")) {
            U = U.replace(".json?", "?");
        }
        return U;
    }

    function N(U) {
        if (U && typeof U.url === "string" && typeof U.success === "function" && E.site && E.domain && f) {
            return true;
        } else {
            return false;
        }
    }

    function F() {
        return Math.round(Math.random() * 9999999999) + "-" + Math.round(Math.random() * 9999999999);
    }

    function z(U) {
        return U ? encodeURIComponent(JSON.stringify(U)) : "";
    }

    function y() {
        var V = 0;
        for (var U in j) {
            if (j.hasOwnProperty(U)) {
                V += ("&" + j[U] + "=99").length;
            }
        }
        V += Q.length + A.callback.length + k.length + S;
        return V;
    }

    function u() {
        var V = 0;
        for (var U in M) {
            if (M.hasOwnProperty(U)) {
                V += ("&" + M[U] + "=99").length;
            }
        }
        V += 3;
        V += m;
        return V;
    }

    function p(X, V) {
        var U = X.url + (X.url.indexOf("?") > -1 ? "&" : "?") + j.VERSION_PARAM + "=" + C + "&" + j.JSON + "=" + k;
        if (V) {
            var W = z(X.data);
            if (W) {
                U += "&" + j.BODY_PARAM + "=" + W;
            }
        }
        return U;
    }

    function K(U) {
        return (U.url.length + d + z(U.data).length > D);
    }

    function c(V, Y, W, U) {
        if (typeof Y === "function") {
            try {
                Y.call(U || null, W);
            } catch (X) {
                lpTag.log("Exception in execution of callback, type :" + V + " e=[" + X.message + "]", "ERROR", "lpTag.JSONP.runCallback");
                if (window.lpAgentConfig && window.lpAgentConfig.throwExceptions) {
                    throw X;
                }
            }
        } else {
            lpTag.log("No callback, of type :" + V, "INFO", "lpTag.JSONP.runCallback");
        }
    }

    function H(W, U) {
        var V = {};
        V.encoding = W.encoding;
        V.timeout = W.timeout || A.timeout;
        V.callback = W.callback || A.callback;
        V.retries = typeof U === "number" ? U : 0;
        return V;
    }

    function g(U) {
        U.id = F();
        l[U.id] = {
            originalRequest: U,
            index: 0,
            baseUrl: p(U, false),
            urls: P(U.url, z(U.data), U.id),
            interimObj: H(U)
        };
        s(l[U.id].interimObj, l[U.id]);
    }

    function s(V, U) {
        V.url = G(U);
        V.id = U.id + "!" + U.index;
        V.success = function (W) {
            if (W && (W.responseCode === 200 || W.responseCode === 201)) {
                if (W && W.lpMeta && W.lpMeta.lpS) {
                    U.secureId = W.lpMeta.lpS;
                }
                if ((U.index + 1) < U.urls.length) {
                    U.index++;
                    s(V, U);
                } else {
                    U.originalRequest.success.apply(null, [W]);
                    v(U.id);
                }
            } else {
                U.originalRequest.error.apply(null, [W]);
                v(U.id);
                O();
            }
            b();
        };
        V.error = V.success;
        lpTag.taglets.jsonp.issueCall(V);
        L();
    }

    function T(V) {
        var U = H(V, V.retries || A.retries);
        U.id = F();
        U.url = p(V, true);
        U.success = function (W) {
            if (W.responseCode && W.responseCode === 200 || W.responseCode === 201) {
                c("CALLBACK", V.success, W, V.context);
            } else {
                c("ERROR", V.error, W, V.context);
                O();
            }
            U = null;
            V = null;
            b();
        };
        U.error = function (W) {
            c("ERROR", V.error, W, V.context);
            U = null;
            V = null;
            b();
            O();
        };
        lpTag.taglets.jsonp.issueCall(U);
        L();
    }

    function v(U) {
        l[U] = null;
        delete l[U];
    }

    function G(U) {
        return U.baseUrl + "&" + M.PART_PARAM + "=" + (U.index + 1) + "&" + M.OUTOF_PARAM + "=" + U.urls.length + (U.secureId ? "&" + M.SECURE_IDENTIFIER + "=" + U.secureId : "") + "&" + j.BODY_PARAM + "=" + U.urls[U.index];
    }

    function P(U, V, X) {
        U += "&" + j.CALLID + "=" + X;
        var W = D - U.length - I - d;
        return r(W, V);
    }

    function r(U, W) {
        var Y = "",
            ac = [],
            X;
        var V = W.split("%");
        for (X = 0; X < V.length; X++) {
            if (V[X].length > U) {
                V[X] = Z(Y, V[X], ac, X, U);
                Y = "";
                Y = aa(V[X], U, ac);
            } else {
                Y = ab(Y, V[X], U, ac, X);
            }
        }
        if (Y !== "") {
            ac.push(Y);
        }

        function Z(ag, ai, ae, af, ad) {
            ag += af > 0 ? "%" : "";
            var ah = ad - ag.length;
            ag += ai.substring(0, ah);
            ae.push(ag);
            ai = ai.substring(ah, ai.length);
            return ai;
        }

        function aa(ai, ad, af) {
            var ah = Math.ceil(ai.length / ad);
            for (var ag = 0; ag < ah; ag++) {
                var ae = ai.substring(ag * ad, (ag + 1) * ad);
                if (ag + 1 < ah) {
                    af.push(ae);
                }
            }
            return ae;
        }

        function ab(ah, ai, ad, ae, af) {
            var ag = (ah + ai).length;
            ag += (af > 0) ? 1 : 0;
            if (ag > ad) {
                ae.push(ah);
                ah = "";
            }
            ah += af > 0 ? "%" : "";
            ah += ai;
            return ah;
        }
        return ac;
    }

    function L() {
        n = n + 1;
        h = h + 1;
    }

    function b() {
        h = h > 0 ? h - 1 : 0;
    }

    function O() {
        i = i + 1;
    }

    function B(U) {
        if (N(U)) {
            U = x(U);
            if (K(U)) {
                return g(U);
            } else {
                return T(U);
            }
        } else {
            return false;
        }
    }

    function a(U) {
        var V = "";
        if (U.indexOf("/agentSession") != -1) {
            V = "/agentSession";
        } else {
            if (U.indexOf("/chat") != -1) {
                V = "/chat";
            } else {
                if (U.indexOf("/visit") != -1) {
                    V = "/visit";
                    if (U.indexOf("/keepAlive") != -1) {
                        V += "/keepAlive";
                    }
                } else {
                    if (U.indexOf("/configuration") != -1) {
                        V = "/configuration";
                    }
                }
            }
        }
        return V;
    }

    function t(V) {
        for (var U in V) {
            if (V.hasOwnProperty(U)) {
                if (A.hasOwnProperty(U)) {
                    A[U] = V[U];
                } else {
                    if (E.hasOwnProperty(U)) {
                        E[U] = V[U];
                    }
                }
            }
        }
        if (E.domain && E.site) {
            f = q.prefix + E.domain + q.middle + E.site + q.suffix;
        }
    }
    var o = {
        init: function () {
            if (lpTag && lpTag.taglets && lpTag.taglets.lpAjax) {
                try {
                    lpTag.taglets.lpAjax.addTransport(R, o);
                } catch (U) { }
            }
        },
        configure: t,
        issueCall: B,
        isValidRequest: N,
        getVersion: function () {
            return Q;
        },
        getName: function () {
            return R;
        },
        inspect: function () {
            return {
                name: R,
                version: Q,
                callsMade: n,
                errorsFound: i,
                pending: h,
                baseUrl: f,
                defaults: (function () {
                    var V = {};
                    for (var U in A) {
                        if (A.hasOwnProperty(U)) {
                            V[U] = A[U];
                        }
                    }
                    return V;
                })()
            };
        }
    };
    if (lpTag && lpTag.taglets && lpTag.taglets.lpAjax) {
        try {
            lpTag.taglets.lpAjax.addTransport(R, o);
        } catch (e) { }
    }
    return o;
})();
window.lpTag = window.lpTag || {};
lpTag.taglets = lpTag.taglets || {};
lpTag.utils = lpTag.utils || {};
lpTag.utils.log = lpTag.utils.log || function () { };
lpTag.utils.log.debug = false;
lpTag.taglets.ChatOverRestAPI = lpTag.taglets.ChatOverRestAPI || function ChatOverRestAPI(ae) {
    if (this === window) {
        return false;
    }
    var K = this;
    var V = {}, ad = 0,
        L = (ae && ae.failureTolerance) || 9;
    var d = {};
    var g;
    var P;
    var U;
    var w = false;
    var c = false,
        h = false,
        G = 0,
        ac = "1.0.0";
    var s = {
        resumeMode: "lpVisitorResumeMode",
        sessionVars: "lpVisitorSessionVars",
        chat: "lpVisitorChatRels"
    };
    var ah = "::";
    var F = {
        QA: "QA",
        DEV: "DEV",
        PRODUCTION: "PRODUCTION"
    };
    var S = F.PRODUCTION;
    var af = function () {
        return (S != F.PRODUCTION) ? "/hcp/html/lpsunco/postmessage.html" : "/hcp/html/postmessage.min.html";
    };
    var v = {
        ERROR: "ERROR",
        INFO: "INFO"
    };
    var E = false;
    var j;
    var n = lpTag && lpTag.taglets && lpTag.taglets.jsonp;
    var B = lpTag && lpTag.taglets && lpTag.taglets.lpAjax;
    var Z = {
        typing: false,
        visitorName: "",
        agentName: "",
        rtSessionId: "",
        initialised: false,
        agentTyping: false,
        chatInProgress: false,
        visitorId: "",
        agentId: "",
        timeout: "",
        chatSessionKey: "",
        chatState: "",
        exitSurveyOn: false
    };

    function x() {
        if (lpTag.utils.Events && lpTag.RelManager && lpTag.SessionDataManager) {
            g = lpTag.utils.Events({
                cloneEventData: true,
                eventBufferLimit: 50
            });
            P = new R();
            U = new lpTag.RelManager();
            if (!lpTag.utils.sessionDataManager) {
                lpTag.utils.sessionDataManager = new lpTag.SessionDataManager();
            }
            j = lpTag.utils.sessionDataManager;
            C();
        } else {
            if (G < 20) {
                setTimeout(x, 500);
            } else { }
        }
        G++;
    }

    function Q() {
        return !!(d.lpNumber && d.appKey && d.domain);
    }

    function p(ai) {
        if (ai) {
            d.domain = a(ai.domain) || a(d.domain) || "";
            d.lpNumber = a(ai.lpNumber) || a(d.lpNumber) || "";
            d.appKey = a(ai.appKey) || a(d.appKey) || "";
            if (d.lpNumber && d.appKey && d.domain) {
                U.setData(d);
                j.setSessionData(M(s.sessionVars), d.lpNumber + ah + d.appKey + ah + d.domain);
            }
            S = ai && ai.environment && F[ai.environment] ? ai.environment : S;
        }
    }

    function a(ai) {
        if (typeof ai === "string" && ai !== "") {
            ai = ai.replace(/[\s*\n*\r*\t\*]/g, "");
        }
        return ai;
    }

    function C() {
        K.onLoad(function () {
            if (d.lpNumber && !Q()) {
                K.getSiteDomain({
                    site: d.lpNumber,
                    success: function (ak) {
                        p({
                            lpNumber: ak.site,
                            domain: ak.domain
                        });
                        X();
                    },
                    error: function (ak) {
                        y({}, {}, ak, "onInit", "unable to initiate session");
                    }
                });
            }
        });
        K.onInit(function (ak) {
            if (!ak.error) {
                Z.initialised = true;
            }
        });
        K.onStart(function (ak) {
            if (!ak.error) {
                Z.chatInProgress = true;
            }
        });
        K.onState(function (ak) {
            if (!ak.error) {
                Z.state = ak.state;
                switch (Z.state) {
                    case K.chatStates.CHATTING:
                    case K.chatStates.WAITING:
                    case K.chatStates.RESUMING:
                        if (Z.state !== K.chatStates.RESUMING) {
                            j.setSessionData(M(s.resumeMode), true);
                        }
                        Z.chatInProgress = true;
                        P.changeKeepAliveState();
                        break;
                    case K.chatStates.INITIALISED:
                        if (h) {
                            Z.chatInProgress = true;
                        } else {
                            Z.chatInProgress = false;
                        }
                        break;
                    case K.chatStates.ENDED:
                    case K.chatStates.UNINITIALISED:
                        Z.chatInProgress = false;
                        P.changeKeepAliveState();
                        break;
                    case K.chatStates.NOTFOUND:
                        Z.chatInProgress = false;
                        Z.exitSurveyOn = false;
                        P.changeKeepAliveState();
                        break;
                    default:
                        break;
                }
            }
        });
        K.onVisitorName(function (ak) {
            if (!ak.error) {
                Z.visitorName = ak.visitorName;
            }
        });
        K.onSetVisitorTyping(function (ak) {
            if (!ak.error) {
                Z.typing = ak.typing;
            }
        });
        V = aj(ae);
        p(V);
        ai(V);
        if (typeof V.debug === "boolean") {
            K.setDebugMode(V.debug);
        }
        if (document.readyState === "complete") {
            w = true;
            z();
        } else {
            if (window.addEventListener) {
                window.addEventListener("load", z, false);
            } else {
                window.attachEvent("onload", z);
            }
        }

        function ai(am) {
            for (var al in am) {
                if (am.hasOwnProperty(al)) {
                    if (String(al).indexOf("on") == 0) {
                        if (K.hasOwnProperty(al)) {
                            am[al] = am[al].constructor === Array ? am[al] : [am[al]];
                            for (var ak = 0; ak < am[al].length; ak++) {
                                if (typeof am[al][ak] === "function") {
                                    K[al](am[al][ak]);
                                } else {
                                    if (typeof am[al][ak] === "object" && am[al][ak].callback) {
                                        if (typeof am[al][ak].callback === "function") {
                                            K[al](am[al][ak].callback, (am[al][ak].appName || ""), (am[al][ak].context || null));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        function aj(an) {
            if (an) {
                var ao = {};
                for (var al in an) {
                    if (an.hasOwnProperty(al)) {
                        var am = typeof (ao[al]) !== "undefined";
                        var ak = al.indexOf("on") == 0;
                        if (!am) {
                            ao[al] = an[al];
                        } else {
                            if (ao[al] !== an[al] && !ak) {
                                ao[al] = an[al];
                            } else {
                                ao[al] = ao[al].constructor === Array ? ao[al] : [ao[al]];
                                ao[al].push(an[al]);
                            }
                        }
                    }
                }
            }
            return ao;
        }
    }

    function z() {
        if (j) {
            h = j.getSessionData(M(s.resumeMode));
            if (h === "true") {
                h = I();
            }
        }
        if (!h && Q()) {
            X();
        }
        g.publish({
            eventName: "onLoad",
            data: {
                API: "Chat API SDK Loaded",
                version: ac,
                state: (h ? K.chatStates.RESUMING : K.chatStates.UNINITIALISED)
            }
        });
        Z.chatInProgress = h;
        if (h) {
            Z.chatState = K.chatStates.RESUMING;
        }
        if (!w) {
            if (window.removeEventListener) {
                window.removeEventListener("load", z, false);
            } else {
                window.detachEvent("onload", z);
            }
        }
        w = true;
        return true;
    }

    function l(aj, am, ai, ak) {
        if (!g) {
            return false;
        }
        var al = {
            eventName: aj,
            appName: ai || "",
            aSync: false,
            func: am,
            context: (ak || null)
        };
        return g.register(al);
    }

    function N(aj, ai, am, ak) {
        if (!g) {
            return false;
        }
        if (typeof aj === "function") {
            am = aj;
            aj = "";
        } else {
            if (typeof aj === "object") {
                ak = aj;
                aj = "";
            }
        }
        var al = {
            eventName: aj,
            appName: ai || "",
            func: am,
            context: (ak || null)
        };
        return g.unbind(al);
    }

    function M(ak) {
        if (!d.lpNumber) {
            return "";
        }
        if (typeof ak === "object") {
            var aj = [];
            for (var ai in ak) {
                if (ak.hasOwnProperty(ai)) {
                    aj.push(ak[ai] + d.lpNumber);
                }
            }
            return aj;
        } else {
            return ak + d.lpNumber;
        }
    }

    function R() {
        var aq, an = false,
            ao, ai;
        var am = {
            inChat: 2000,
            exitSurvey: 15000
        };
        var al = am.inChat;
        var ak = this;

        function aj() {
            if (aq) {
                clearTimeout(aq);
                aq = null;
            }
        }

        function ar() {
            b({
                success: ap,
                error: ap
            });
            ao = new Date().valueOf();
        }

        function ap() {
            if (ai) {
                aj();
                var at = (new Date().valueOf()) - ao;
                at = al - at;
                if (at > 0) {
                    aq = setTimeout(ar, at);
                } else {
                    ar();
                }
            }
        }
        ak.changeKeepAliveState = function () {
            aj();
            ai = (Z.chatInProgress || Z.exitSurveyOn);
            if (ai) {
                if (Z.chatInProgress) {
                    al = am.inChat;
                } else {
                    al = am.exitSurvey;
                }
                ar();
            }
        };
        ak.storeChatLocationURI = function (at) {
            j.setSessionData(M(s.chat), at);
        };
        ak.sessionEnded = function () {
            j.clearSessionData(M(s.chat));
            j.clearSessionData(M(s.resumeMode));
        };
    }

    function aa(ak, aj, al, ai) {
        f(v.ERROR, {
            error: ak,
            originalRequest: aj,
            sentRequest: al,
            response: ai
        });
        aj = null;
        al = null;
        ai = null;
        return ak;
    }

    function f(aj, ak) {
        var ai = {
            eventName: "onLog"
        };
        if (aj && typeof aj === "string") {
            ai.appName = aj;
        } else {
            if (typeof aj === "object") {
                ak = aj;
            }
        }
        ai.data = ak;
        if (ai.appName && ai.appName !== v.ERROR && !c) {
            ai = null;
            return;
        }
        g.publish(ai);
        ai = null;
    }

    function O(aj, ai) {
        if (aj && aj.error) {
            e(aj.error, aj, ai);
        }
        ai = null;
        aj = null;
    }

    function ab(aj, ai) {
        if (aj && aj.success) {
            e(aj.success, aj, ai);
        }
    }

    function e(al, ak, aj) {
        if (al && typeof al === "function") {
            try {
                al.call(ak.context || ak, aj);
            } catch (ai) {
                f(v.ERROR, {
                    originalRequest: ak,
                    response: aj,
                    error: "Failed to run callback",
                    thrownError: ai
                });
            }
        }
    }
    K.setDebugMode = function (ai) {
        if (typeof ai === "boolean") {
            c = ai;
            lpTag.utils.log.debug = ai;
        }
    };

    function i(al, ak, aj, am) {
        al = al && al.body ? al.body : {
            error: {
                message: ak,
                time: new Date(),
                originalRequest: aj
            }
        };
        for (var ai in am) {
            if (am.hasOwnProperty(ai) && !al[ai]) {
                al[ai] = am[ai];
            }
        }
        am = null;
        return al;
    }

    function r(am, aq, ai, ap) {
        var ao = am && am.body ? am.body : am;
        ao = ao || {};
        var aj, an, at;
        if (aq) {
            at = aq.split(".");
            an = at.length;
            for (var al = 0; al < an; al++) {
                aj = at[al];
                if (typeof ao[aj] !== "undefined") {
                    ao = ao[aj];
                } else {
                    break;
                }
            }
            if (ai === true) {
                var ak = {};
                ak[at[an - 1]] = ao;
                ao = ak;
            }
        }
        ao = o(ao);
        for (var ar in ap) {
            if (ap.hasOwnProperty(ar) && typeof ao[ar] === "undefined") {
                ao[ar] = ap[ar];
            }
        }
        return ao;
    }

    function A() {
        if (!E) {
            B.configureTransports({
                rest2jsonp: {
                    domain: d.domain,
                    site: d.lpNumber
                },
                postmessage: {
                    frames: [{
                        url: "https://" + d.domain + af(),
                        delayLoad: 0
                    }]
                }
            });
            E = true;
        }
    }

    function I(am) {
        var al = true;
        var aj;
        if (!Q()) {
            if (am && am.lpNumber && am.domain && am.appKey) {
                aj = [am.lpNumber, am.appKey, am.domain];
            } else {
                var an = j.getSessionData(M(s.sessionVars));
                aj = an.split(ah);
            } if (aj.length == 3) {
                var ak = {
                    lpNumber: aj[0],
                    appKey: aj[1],
                    domain: aj[2]
                };
                p(ak);
                A();
            }
        }
        if (Q()) {
            var ai = am && am.URI || j.getSessionData(M(s.chat));
            if (ai) {
                X({
                    success: function () {
                        U.addRels({
                            link: [{
                                rel: "location",
                                href: ai
                            }]
                        }, {
                            type: "chat"
                        });
                        H(t(K.chatStates.RESUMING, new Date()));
                        W(am);
                    }
                });
            } else {
                al = false;
            }
            ak = null;
        } else {
            j.clearSessionData(M(s));
            al = false;
        }
        return al;
    }

    function y(am, ao, an, aj, ak, ai) {
        f(v.ERROR, {
            originalRequest: am,
            sentRequest: ao,
            response: an
        });
        an = i(an, ak, am);
        aj = aj.constructor === Array ? aj : [aj];
        for (var al = 0; al < aj.length; al++) {
            g.publish({
                eventName: aj[al],
                appName: ai || "",
                data: an
            });
        }
        O(am, an);
        ao = null;
    }

    function o(aj) {
        for (var ai in aj) {
            if (aj.hasOwnProperty(ai)) {
                if (ai == "link") {
                    aj[ai] = null;
                    delete aj[ai];
                } else {
                    if (typeof aj[ai] === "object") {
                        aj[ai] = o(aj[ai]);
                    }
                }
            }
        }
        return aj;
    }

    function Y(aj) {
        var ai = {
            site: aj.lpNumber
        };
        ai.success = function (ak) {
            aj.domain = ak.domain;
            X(aj);
        };
        ai.error = function (ak) {
            aa({
                error: "unable to resolve site domain"
            }, aj, "", ak);
            y({}, {}, {}, "onInit", "unable to initiate session");
            O(aj);
        };
        K.getSiteDomain(ai);
    }
    K.getSiteDomain = function (al) {
        var ak = "https://api.liveperson.net/csdr/account/";
        var ai = "/service/adminArea/baseURI.lpCsds?version=1.0";
        if (typeof al.success !== "function" || !al.site) {
            return aa({
                error: "missing callback or site - cannot lookup domain"
            }, "", "", "");
        }
        var aj = {
            url: ak + al.site + ai
        };
        aj.success = function (ap) {
            var ao;
            f(v.INFO, {
                originalRequest: {
                    site: al.site
                },
                sentRequest: aj,
                response: ap
            });
            if (ap && ap.ResultSet && ap.ResultSet.lpData) {
                var an = ap.ResultSet.lpData;
                for (var am = 0; am < an.length; am++) {
                    if (!ao && an[am]["lpServer"]) {
                        ao = an[am]["lpServer"];
                    }
                }
            }
            aj = null;
            if (ao) {
                ab(al, {
                    domain: ao,
                    site: al.site
                });
            } else {
                O(al, {
                    error: "unable to resolve site domain",
                    response: ap,
                    site: al.site
                });
            }
        };
        aj.error = aj.success;
        f(v.INFO, {
            originalRequest: {
                site: al.site
            },
            sentRequest: aj,
            response: "SENDING REQUEST"
        });
        return n.issueCall(aj);
    };

    function X(ai) {
        ai = ai || {};
        p(ai);
        ai.lpNumber = d.lpNumber;
        ai.appKey = d.appKey;
        ai.domain = d.domain;
        if (!ai.lpNumber) {
            return aa({
                error: "missing siteconfiguration data, cannot init"
            }, ai, "", "");
        }
        if (!ai.domain) {
            Y(ai);
            return false;
        }
        if (!Q()) {
            return false;
        }
        A();
        U.setData(ai);
        var aj = U.buildRequestObj({
            rel: "",
            data: {
                lpNumber: d.lpNumber,
                appKey: d.appKey,
                domain: d.domain
            }
        });
        aj.success = function (ak) {
            f(v.INFO, {
                originalRequest: ai,
                sentRequest: aj,
                response: ak
            });
            if (ak) {
                U.addRels(ak, {
                    type: "chat"
                });
                g.publish({
                    eventName: "onInit",
                    data: {
                        account: ai.lpNumber,
                        domain: ai.domain,
                        init: true
                    }
                });
                H(t(K.chatStates.INITIALISED));
            }
            if (ai.requestChat) {
                K.requestChat(ai);
            } else {
                ab(ai, {});
            }
            aj = null;
        };
        aj.error = function (al) {
            var ak = ["onInit"];
            if (ai.requestChat) {
                ak.push("onRequestChat");
            }
            y(ai, aj, al, ak, "unable to initiate session");
            U.clearData();
        };
        f(v.INFO, {
            originalRequest: ai,
            sentRequest: aj,
            response: "SENDING REQUEST"
        });
        return B.issueCall(aj);
    }
    K.requestChat = function (ai) {
        if (Z.initialised) {
            k(ai);
        } else {
            ai.requestChat = true;
            X(ai);
        }
    };
    K.startChat = K.requestChat;

    function q(aj) {
        var al = {}, ak = false;
        if (aj.agent) {
            m(aj, ["skill", "serviceQueue", "maxWaitTime"]);
            al.agent = aj.agent;
            ak = true;
        } else {
            if (aj.skill) {
                al.skill = aj.skill;
                m(aj, ["serviceQueue", "maxWaitTime"]);
                ak = true;
            } else {
                if (aj.serviceQueue && !isNaN(aj.maxWaitTime)) {
                    al.serviceQueue = aj.serviceQueue;
                    al.maxWaitTime = aj.maxWaitTime;
                    ak = true;
                }
            }
        } if (aj.chatReferrer || document.referrer) {
            al.chatReferrer = aj.chatReferrer || document.referrer;
            ak = true;
        }
        if (navigator && navigator.userAgent) {
            al.userAgent = navigator.userAgent;
            ak = true;
        }
        if (typeof aj.ssoKey === "string") {
            al.ssoKey = aj.ssoKey;
            ak = true;
        }
        if (typeof aj.survey === "object") {
            al.survey = aj.survey;
            ak = true;
        }
        if (typeof aj.LETagVisitorId === "string") {
            al.LETagVisitorId = aj.LETagVisitorId;
            ak = true;
        }
        if (typeof aj.LETagSessionId === "string") {
            al.LETagSessionId = aj.LETagSessionId;
            ak = true;
        }
        if (typeof aj.LETagContextId === "string") {
            al.LETagContextId = aj.LETagContextId;
            ak = true;
        }
        if (aj.runWithRules === true) {
            al.runWithRules = true;
            ak = true;
        }
        var an = aj.visitorId || j.readCookie(V.lpNumber + "-VID");
        if (an) {
            al.visitorId = an;
            ak = true;
        }
        var am = aj.visitorSessionId || j.readCookie(V.lpNumber + "-SKEY");
        if (am) {
            al.visitorSessionId = am;
            ak = true;
        }
        if (typeof aj.customVariables === "object") {
            if (aj.customVariables.customVariable && aj.customVariables.customVariable.constructor === Array) {
                al.customVariables = aj.customVariables;
                ak = true;
            } else {
                var ai = ag(aj.customVariables);
                if (ai) {
                    al.customVariables = ai;
                    ak = true;
                }
            }
        }
        if (aj.preChatLines && aj.preChatLines.constructor === Array) {
            al.preChatLines = {
                lines: aj.preChatLines
            };
            ak = true;
        }
        return ak ? {
            request: al
        } : "";
    }

    function ag(ai) {
        var ak = [];
        for (var aj in ai) {
            if (ai.hasOwnProperty(aj)) {
                ak.push({
                    name: aj,
                    value: ai[aj]
                });
            }
        }
        if (ak.length > 0) {
            return {
                customVariable: ak
            };
        } else {
            return null;
        }
    }

    function m(al, ak) {
        for (var aj = 0; aj < ak.length; aj++) {
            if (al.hasOwnProperty(ak[aj])) {
                try {
                    al[ak[aj]] = null;
                    delete al[ak[aj]];
                } catch (ai) { }
            }
        }
    }

    function k(ai) {
        if (Z.chatInProgress) {
            return false;
        }
        var aj = U.buildRequestObj({
            rel: "chat-request",
            type: "chat",
            needAuth: true,
            requestType: "POST",
            data: q(ai || {}, {})
        });
        if (!aj) {
            return aa({
                error: "requestChat - unable to find rel for request"
            }, "", "", "");
        }
        aj.success = function (ak) {
            f(v.INFO, {
                originalRequest: (ai || ""),
                sentRequest: aj,
                response: ak
            });
            if (ak && ak.headers && ak.headers.Location) {
                U.addRels({
                    link: [{
                        "@rel": "location",
                        "@href": ak.headers.Location
                    }]
                }, {
                    type: "chat"
                });
                P.storeChatLocationURI(ak.headers.Location);
                W(ai);
            }
            aj = null;
        };
        aj.error = function (ak) {
            y(ai, aj, ak, ["onRequestChat"], "requestChat  - server error");
        };
        f(v.INFO, {
            originalRequest: (ai || ""),
            sentRequest: aj,
            response: "SENDING REQUEST"
        });
        return B.issueCall(aj);
    }

    function W(ai) {
        var aj = U.buildRequestObj({
            rel: "location",
            type: "chat",
            needAuth: true,
            requestType: "GET"
        });
        if (!aj) {
            return aa({
                error: "getSessionData - unable to find rel for request"
            }, "", "", "");
        }
        aj.success = function (am) {
            f(v.INFO, {
                originalRequest: (ai || ""),
                sentRequest: aj,
                response: am
            });
            if (am) {
                U.addRels(am.body.chat, {
                    type: "chat"
                });
                U.addRels(am.body.chat.info, {
                    type: "visitor"
                });
                var ak = r(am, "chat", false);
                g.publish({
                    eventName: "onRequestChat",
                    data: ak
                });
                D(ak.info);
                var al = u(ak.events);
                if (al) {
                    J(al);
                }
                ab(ai, ak);
            }
            aj = null;
        };
        aj.error = function (ak) {
            if (Z.chatState === K.chatStates.RESUMING) {
                H(t(K.chatStates.NOTFOUND));
                P.sessionEnded();
            }
            y(ai, aj, ak, "onRequestChat", "getSessionData  - server error");
        };
        f(v.INFO, {
            originalRequest: "",
            sentRequest: aj,
            response: "SENDING REQUEST"
        });
        return B.issueCall(aj);
    }
    K.getAvailableSlots = function (aj) {
        var ai = {};
        if (aj) {
            if (aj.agent && typeof aj.agent === "string") {
                ai.agent = aj.agent;
            } else {
                if (aj.skill && typeof aj.skill === "string") {
                    ai.skill = aj.skill;
                }
                if (aj.maxWaitTime && typeof aj.maxWaitTime === "number") {
                    ai.maxWaitTime = aj.maxWaitTime;
                }
            }
        }
        var ak = U.buildRequestObj({
            rel: "chat-available-slots",
            type: "chat",
            needAuth: true,
            requestType: "GET",
            queryParams: ai
        });
        if (!ak) {
            return aa({
                error: "getAvailableSlots - unable to find rel for request"
            }, "", "", "");
        }
        ak.success = function (am) {
            f(v.INFO, {
                originalRequest: (aj || ""),
                sentRequest: ak,
                response: am
            });
            if (am) {
                var al = r(am, "availableSlots", true);
                g.publish({
                    eventName: "onAvailableSlots",
                    data: al
                });
                ab(aj, al);
            }
            ak = null;
        };
        ak.error = function (al) {
            y(aj, ak, al, "onAvailableSlots", "getAvailableSlots - server error");
        };
        f(v.INFO, {
            originalRequest: (aj || ""),
            sentRequest: ak,
            response: "SENDING REQUEST"
        });
        return B.issueCall(ak);
    };
    K.getEstimatedWaitTime = function (aj) {
        var ai = {};
        if (aj) {
            if (aj.skill && typeof aj.skill === "string") {
                ai.skill = aj.skill;
            }
            if (aj.serviceQueue && typeof aj.serviceQueue === "string") {
                ai.serviceQueue = aj.serviceQueue;
            }
        }
        var ak = U.buildRequestObj({
            rel: "chat-estimatedWaitTime",
            type: "chat",
            needAuth: true,
            requestType: "GET"
        });
        if (!ak) {
            return aa({
                error: "getEstimatedWaitTime - unable to find rel for request"
            }, "", "", "");
        }
        ak.success = function (am) {
            f(v.INFO, {
                originalRequest: "",
                sentRequest: ak,
                response: am
            });
            if (am) {
                var al = r(am, "estimatedWaitTime", true);
                g.publish({
                    eventName: "onEstimatedWaitTime",
                    data: al
                });
                ab(aj, al);
            }
            ak = null;
        };
        ak.error = function (al) {
            y(aj, ak, al, "onEstimatedWaitTime", "getEstimatedWaitTime - server error");
        };
        f(v.INFO, {
            originalRequest: "",
            sentRequest: ak,
            response: "SENDING REQUEST"
        });
        return B.issueCall(ak);
    };
    K.getAvailabilty = function (aj) {
        var ai = {};
        if (aj) {
            if (aj.agent && typeof aj.agent === "string") {
                ai.agent = aj.agent;
            } else {
                if (aj.skill && (typeof aj.skill === "string" || aj.skill.constructor === Array)) {
                    ai.skill = aj.skill;
                }
                if (aj.maxWaitTime && typeof aj.maxWaitTime === "number") {
                    ai.maxWaitTime = aj.maxWaitTime;
                }
            }
        }
        var ak = U.buildRequestObj({
            rel: "chat-availability",
            type: "chat",
            needAuth: true,
            requestType: "GET",
            queryParams: ai
        });
        if (!ak) {
            return aa({
                error: "getAvailability - unable to find rel for request"
            }, "", "", "");
        }
        ak.success = function (am) {
            f(v.INFO, {
                originalRequest: (aj || ""),
                sentRequest: ak,
                response: am
            });
            if (am) {
                var al = r(am, "availability", true);
                if (al.availability && al.availability.AvailabilityForSkills && al.availability.AvailabilityForSkills.skillAvailability) {
                    al = T(al.availability.AvailabilityForSkills.skillAvailability);
                }
                g.publish({
                    eventName: "onAvailability",
                    data: al
                });
                ab(aj, al);
            }
            ak = null;
        };
        ak.error = function (al) {
            y(aj, ak, al, "onAvailability", "getAvailabilty - server error");
        };
        f(v.INFO, {
            originalRequest: (aj || ""),
            sentRequest: ak,
            response: "SENDING REQUEST"
        });
        return B.issueCall(ak);
    };

    function T(ak) {
        var aj = {};
        for (var ai = 0; ai < ak.length; ai++) {
            aj[ak[ai].skill] = ak[ai].isAvailable;
        }
        return aj;
    }
    K.getPreChatSurvey = function (aj) {
        var ai = {};
        if (aj) {
            if (aj.visitorProfile && typeof aj.visitorProfile === "string") {
                ai.visitorProfile = aj.visitorProfile;
            } else {
                if (aj.skill && typeof aj.skill === "string") {
                    ai.skill = aj.skill;
                }
            } if (aj.visitorIp && typeof aj.visitorIp === "string") {
                ai.visitorIp = aj.visitorIp;
            }
            if (aj.surveyName && typeof aj.surveyName === "string") {
                ai.surveyName = aj.surveyName;
            } else {
                if (aj.surveyApiId) {
                    ai.surveyApiId = aj.surveyApiId;
                }
            }
        }
        var ak = U.buildRequestObj({
            rel: "prechat-survey",
            type: "chat",
            needAuth: true,
            requestType: "GET",
            queryParams: ai
        });
        if (!ak) {
            return aa({
                error: "getPreChatSurvey - unable to find rel for request"
            }, "", "", "");
        }
        ak.success = function (am) {
            f(v.INFO, {
                originalRequest: (aj || ""),
                sentRequest: ak,
                response: am
            });
            if (am) {
                var al = r(am, "survey", true);
                g.publish({
                    eventName: "onPreChatSurvey",
                    data: al
                });
                ab(aj, al);
            }
            ak = null;
        };
        ak.error = function (al) {
            y(aj, ak, al, "onPreChatSurvey", "getPreChatSurvey - server error");
        };
        f(v.INFO, {
            originalRequest: (aj || ""),
            sentRequest: ak,
            response: "SENDING REQUEST"
        });
        return B.issueCall(ak);
    };
    K.getOfflineSurvey = function (aj) {
        var ai = {};
        if (aj) {
            if (aj.skill && typeof aj.skill === "string") {
                ai.skill = aj.skill;
            } else {
                if (aj.visitorProfile && typeof aj.visitorProfile === "string") {
                    ai.visitorProfile = aj.visitorProfile;
                } else {
                    if (aj.visitorId && typeof aj.visitorId === "string") {
                        ai.visitorId = aj.visitorId;
                    } else {
                        if (aj.surveyName && typeof aj.surveyName === "string") {
                            ai.surveyName = aj.surveyName;
                        } else {
                            if (aj.surveyApiId) {
                                ai.surveyApiId = aj.surveyApiId;
                            }
                        }
                    }
                }
            }
        }
        var ak = U.buildRequestObj({
            rel: "chat-offline-survey",
            type: "chat",
            needAuth: true,
            requestType: "GET",
            queryParams: ai
        });
        if (!ak) {
            return aa({
                error: "getOfflineSurvey - unable to find rel for request"
            }, "", "", "");
        }
        ak.success = function (am) {
            f(v.INFO, {
                originalRequest: (aj || ""),
                sentRequest: ak,
                response: am
            });
            if (am) {
                var al = r(am, "survey", true);
                g.publish({
                    eventName: "onOfflineSurvey",
                    data: al
                });
                ab(aj, al);
            }
            ak = null;
        };
        ak.error = function (al) {
            y(aj, ak, al, "onOfflineSurvey", "getOfflineSurvey - server error");
        };
        f(v.INFO, {
            originalRequest: (aj || ""),
            sentRequest: ak,
            response: "SENDING REQUEST"
        });
        return B.issueCall(ak);
    };
    K.submitOfflineSurvey = function (aj) {
        var ai = {}, ak = {}, al;
        if (!aj.survey) {
            return aa({
                error: "Survey must be present in order to submit"
            }, "", "", "");
        }
        al = aj.visitorSessionId || j.readCookie(V.lpNumber + "-SKEY");
        if (al) {
            ak.visitorSessionId = al;
        }
        ak.survey = aj.survey;
        if (typeof aj.LETagVisitorId === "string") {
            ak.LETagVisitorId = aj.LETagVisitorId;
        }
        if (typeof aj.LETagSessionId === "string") {
            ak.LETagSessionId = aj.LETagSessionId;
        }
        if (typeof aj.LETagContextId === "string") {
            ak.LETagContextId = aj.LETagContextId;
        }
        var am = U.buildRequestObj({
            rel: "chat-offline-survey",
            type: "chat",
            needAuth: true,
            requestType: "PUT",
            data: ak,
            queryParams: ai
        });
        if (!am) {
            return aa({
                error: "submitOfflineSurvey - unable to find rel for request"
            }, "", "", "");
        }
        am.success = function (ao) {
            f(v.INFO, {
                originalRequest: (aj || ""),
                sentRequest: am,
                response: ao
            });
            if (ao) {
                var an = r(ao);
                g.publish({
                    eventName: "onSubmitOfflineSurvey",
                    data: an
                });
                ab(aj, an);
            }
            am = null;
        };
        am.error = function (an) {
            y(aj, am, an, "onSubmitOfflineSurvey", "submitOfflineSurvey - server error");
        };
        f(v.INFO, {
            originalRequest: (aj || ""),
            sentRequest: am,
            response: "SENDING REQUEST"
        });
        return B.issueCall(am);
    };

    function b(ai) {
        var aj = U.buildRequestObj({
            rel: "next",
            type: "chat",
            needAuth: true,
            requestType: "GET"
        });
        if (!aj) {
            return aa({
                error: "getEvents - unable to find rel for request"
            }, "", "", "");
        }
        aj.timeout = 10000;
        aj.retries = 0;
        aj.success = function (al) {
            ad = 0;
            var am, ak;
            f(v.INFO, {
                originalRequest: "",
                sentRequest: aj,
                response: al
            });
            if (al) {
                U.addRels(al.body.chat, {
                    type: "chat"
                });
                U.addRels(al.body.chat.info, {
                    type: "visitor"
                });
                am = r(al, "chat");
                ak = u(am.events);
                if (ak) {
                    J(ak);
                }
                if (am.info) {
                    D(am.info);
                }
            }
            ab(ai, ak);
            aj = null;
        };
        aj.error = function (ak) {
            ad = ad + 1;
            y(ai, aj, ak, ["onLine", "onAgentTyping", "onInfo", "onEvents"], "getEvents - server error");
            if (ad > L) {
                H(t(K.chatStates.NOTFOUND));
            }
        };
        f(v.INFO, {
            originalRequest: "",
            sentRequest: aj,
            response: "SENDING REQUEST"
        });
        return B.issueCall(aj);
    }

    function J(ai) {
        if (ai.line) {
            g.publish({
                eventName: "onLine",
                data: {
                    lines: ai.line
                }
            });
        }
        if (ai.collaboration) {
            g.publish({
                eventName: "onCollaboration",
                data: {
                    collaboration: ai.collaboration
                }
            });
        }
        if (ai.state) {
            ai.state = ai.state.constructor === Array ? ai.state.pop() : ai.state;
            H(t(ai.state.state, ai.state.time));
        }
        if (ai["a2a-transfer"]) {
            g.publish({
                eventName: "onA2ATransfer",
                data: {
                    transferData: ai["a2a-transfer"]
                }
            });
        }
        g.publish({
            eventName: "onEvents",
            data: ai
        });
    }

    function t(ai, aj) {
        return {
            state: ai,
            time: aj || new Date().toTimeString()
        };
    }

    function D(ak) {
        if ((ak.agentTyping === "typing") != Z.agentTyping) {
            Z.agentTyping = (ak.agentTyping === "typing");
            g.publish({
                eventName: "onAgentTyping",
                data: {
                    agentTyping: Z.agentTyping
                }
            });
            ak.agentTyping = null;
            delete ak.agentTyping;
        }
        var aj = false;
        for (var ai in ak) {
            if (ak.hasOwnProperty(ai) && Z.hasOwnProperty(ai) && (ak[ai] != Z[ai])) {
                if (ai != "lastUpdate" && ai != "agentTyping" && ai != "visitorTyping" && ai != "state") {
                    Z[ai] = ak[ai];
                    aj = true;
                }
            }
        }
        Z.lastUpdate = ak.lastUpdate;
        if (aj) {
            g.publish({
                eventName: "onInfo",
                data: Z,
                cloneEventData: true
            });
        }
    }

    function H(ai) {
        if (Z.chatState === ai.state) {
            return;
        } else {
            Z.chatState = ai.state;
        }
        switch (ai.state) {
            case K.chatStates.ENDED:
                g.publish({
                    eventName: "onStop",
                    data: ai
                });
                break;
            case K.chatStates.CHATTING:
                g.publish({
                    eventName: "onStart",
                    data: ai
                });
                break;
        }
        g.publish({
            eventName: "onState",
            data: ai
        });
    }

    function u(al) {
        if (!al.event) {
            return null;
        }
        al = al.event;
        al = al.constructor === Array ? al : [al];
        var an = {};
        var am = false;
        for (var ai = 0; ai < al.length; ai++) {
            var aj = al[ai].type || al[ai]["@type"];
            if (al[ai]["channelID"]) {
                var ak = al[ai]["channelID"];
                an[aj] = an[aj] || {};
                an[aj][ak] = an[aj][ak] || [];
                an[aj][ak].push(al[ai]);
            } else {
                an[aj] = an[aj] || [];
                an[aj].push(al[ai]);
            }
            am = true;
        }
        return (!am ? null : an);
    }
    K.setCustomVariable = function (ai) {
        if (!ai.customVariables && !ai.customVariable) {
            return aa({
                error: "setCustomVariable - no variables passed in"
            }, "", "", "");
        } else {
            var ak = ag(ai.customVariable);
            if (ak && ak.customVariable) {
                ai.customVariable = ak.customVariable;
            } else {
                return aa({
                    error: "setCustomVariable - unable to find any custom variables to set"
                }, "", "", "");
            }
        }
        var aj = U.buildRequestObj({
            rel: "custom-variables",
            type: "chat",
            needAuth: true,
            requestType: "PUT",
            data: {
                customVariables: {
                    customVariable: ai.customVariable
                }
            }
        });
        if (!aj) {
            return aa({
                error: "setCustomVariable - unable to find rel for request"
            }, "", "", "");
        }
        aj.success = function (am) {
            f(v.INFO, {
                originalRequest: (ai || ""),
                sentRequest: aj,
                response: am
            });
            if (am) {
                var al = {
                    customVariable: ai.customVariable
                };
                g.publish({
                    eventName: "onSetCustomVariable",
                    data: al
                });
                ab(ai, al);
            }
            aj = null;
        };
        aj.error = function (al) {
            y(ai, aj, al, "onSetCustomVariable", "setCustomVariable  - server error");
        };
        f(v.INFO, {
            originalRequest: (ai || ""),
            sentRequest: aj,
            response: "SENDING REQUEST"
        });
        return B.issueCall(aj);
    };
    K.requestTranscript = function (aj) {
        var ai = new RegExp(/[a-zA-Z0-9.-_]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/i);
        if (typeof aj === "string") {
            aj = {
                email: aj
            };
        }
        if (!aj.email || !ai.test(aj.email)) {
            return aa({
                error: "requestTranscriptEmail - no variables passed in or invalid email"
            }, "", "", "");
        }
        var ak = U.buildRequestObj({
            rel: "transcript-request",
            type: "chat",
            needAuth: true,
            requestType: "PUT",
            data: {
                email: aj.email
            }
        });
        if (!ak) {
            return aa({
                error: "requestTranscript - unable to find rel for request"
            }, "", "", "");
        }
        ak.success = function (am) {
            f(v.INFO, {
                originalRequest: (aj || ""),
                sentRequest: ak,
                response: am
            });
            if (am) {
                var al = {
                    email: aj.email
                };
                g.publish({
                    eventName: "onTranscript",
                    data: al
                });
                ab(aj, al);
            }
            ak = null;
        };
        ak.error = function (al) {
            y(aj, ak, al, "onTranscript", "requestTranscriptEmail  - server error");
        };
        f(v.INFO, {
            originalRequest: (aj || ""),
            sentRequest: ak,
            response: "SENDING REQUEST"
        });
        return B.issueCall(ak);
    };
    K.getExitSurvey = function (aj) {
        var ai = {};
        if (aj && aj.surveyName) {
            ai.surveyName = aj.surveyName;
        } else {
            if (aj && aj.surveyApiId) {
                ai.surveyApiId = aj.surveyApiId;
            }
        }
        var ak = U.buildRequestObj({
            rel: "exit-survey",
            type: "chat",
            needAuth: true,
            requestType: "GET",
            queryParams: ai
        });
        if (!ak) {
            return aa({
                error: "getExitSurvey - unable to find rel for request"
            }, "", "", "");
        }
        ak.success = function (am) {
            f(v.INFO, {
                originalRequest: (aj || ""),
                sentRequest: ak,
                response: am
            });
            var al = {};
            if (am && am.body && am.body.survey) {
                Z.exitSurveyOn = true;
                P.changeKeepAliveState();
                al = r(am, "survey", true);
                g.publish({
                    eventName: "onExitSurvey",
                    data: al
                });
                ab(aj, al);
            }
            ak = null;
        };
        ak.error = function (al) {
            Z.exitSurveyOn = false;
            P.changeKeepAliveState();
            y(aj, ak, al, "onExitSurvey", "getExitSurvey  - server error");
        };
        f(v.INFO, {
            originalRequest: (aj || ""),
            sentRequest: ak,
            response: "SENDING REQUEST"
        });
        return B.issueCall(ak);
    };
    K.submitExitSurvey = function (ai) {
        if (!ai.survey) {
            return aa({
                error: "submitExitSurvey - no survey passed in"
            }, "", "", "");
        }
        if (ai.survey.survey) {
            ai.survey = ai.survey.survey;
        }
        if (document.referrer) {
            ai.chatReferrer = document.referrer;
        }
        if (navigator && navigator.userAgent) {
            ai.userAgent = navigator.userAgent;
        }
        var aj = U.buildRequestObj({
            rel: "exit-survey",
            type: "chat",
            needAuth: true,
            requestType: "PUT",
            data: ai
        });
        if (!aj) {
            return aa({
                error: "submitExitSurvey - unable to find rel for request"
            }, "", "", "");
        }
        aj.success = function (al) {
            f(v.INFO, {
                originalRequest: (ai || ""),
                sentRequest: aj,
                response: al
            });
            Z.exitSurveyOn = false;
            P.changeKeepAliveState();
            if (al) {
                var ak = r(al);
                g.publish({
                    eventName: "onSubmitExitSurvey",
                    data: ak
                });
                ab(ai, ak);
            }
            aj = null;
        };
        aj.error = function (ak) {
            y(ai, aj, ak, "onSubmitExitSurvey", "submitExitSurvey  - server error");
        };
        f(v.INFO, {
            originalRequest: (ai || ""),
            sentRequest: aj,
            response: "SENDING REQUEST"
        });
        return B.issueCall(aj);
    };
    K.addLine = function (ai) {
        if (!ai) {
            return aa({
                error: "No object data passed"
            }, "", "", "");
        }
        if (!ai.text) {
            return aa({
                error: "No text to send"
            }, ai, "", "");
        }
        var aj = U.buildRequestObj({
            rel: "events",
            needAuth: true,
            type: "chat",
            requestType: "POST",
            data: {
                event: {
                    "@type": "line",
                    text: ai.text
                }
            }
        });
        if (!aj) {
            return aa({
                error: "addLine - unable to find rel for request"
            }, ai, "", "");
        }
        aj.success = function (al) {
            f(v.INFO, {
                originalRequest: ai,
                sentRequest: aj,
                response: al
            });
            if (al) {
                var ak = {
                    text: ai.text
                };
                g.publish({
                    eventName: "onAddLine",
                    data: ak
                });
                ab(ai, ak);
            }
            aj = null;
        };
        aj.error = function (ak) {
            y(ai, aj, ak, "onAddLine", "addLine  - server error");
        };
        f(v.INFO, {
            originalRequest: ai,
            sentRequest: aj,
            response: "SENDING REQUEST"
        });
        return B.issueCall(aj);
    };
    K.setVisitorTyping = function (ai) {
        if (!ai) {
            return aa({
                error: "No object data passed"
            }, "", "", "");
        }
        if (typeof ai.typing !== "boolean" || ai.typing == Z.typing) {
            return aa({
                error: "setVisitorTyping typing status missing, not a boolean or unchanged from previous update"
            }, ai, "", "");
        } else {
            Z.typing = ai.typing;
        }
        var aj = U.buildRequestObj({
            rel: "visitor-typing",
            needAuth: true,
            type: "visitor",
            requestType: "PUT",
            data: {
                visitorTyping: (ai.typing ? "typing" : "not-typing")
            }
        });
        if (!aj) {
            return aa({
                error: "setVisitorTyping - unable to find rel for request"
            }, ai, "", "");
        }
        aj.success = function (al) {
            f(v.INFO, {
                originalRequest: ai,
                sentRequest: aj,
                response: al
            });
            if (al) {
                var ak = {
                    typing: ai.typing
                };
                g.publish({
                    eventName: "onSetVisitorTyping",
                    data: ak
                });
                ab(ai, ak);
            }
            aj = null;
        };
        aj.error = function (ak) {
            y(ai, aj, ak, "onSetVisitorTyping", "setVisitorTyping  - server error");
        };
        f(v.INFO, {
            originalRequest: ai,
            sentRequest: aj,
            response: "SENDING REQUEST"
        });
        return B.issueCall(aj);
    };
    K.endChat = function (ai) {
        var aj = U.buildRequestObj({
            rel: "events",
            type: "chat",
            needAuth: true,
            requestType: "POST",
            data: {
                event: {
                    "@type": "state",
                    state: K.chatStates.ENDED
                }
            }
        });
        if (!aj) {
            return aa({
                error: "endChat - unable to find rel for request"
            }, ai, "", "");
        }
        aj.success = function (al) {
            f(v.INFO, {
                originalRequest: ai,
                sentRequest: aj,
                response: al
            });
            if (al) {
                var ak = t(K.chatStates.ENDED);
                H(ak);
                ab(ai, ak);
            }
            aj = null;
        };
        aj.error = function (ak) {
            y(ai, aj, ak, ["onState"], "endChat - server error");
        };
        f(v.INFO, {
            originalRequest: ai,
            sentRequest: aj,
            response: "SENDING REQUEST"
        });
        return B.issueCall(aj);
    };
    K.setVisitorName = function (aj) {
        if (!aj) {
            return aa({
                error: "No object data passed"
            }, "", "", "");
        }
        var ai = "";
        if (typeof aj.visitorName !== "string" || aj.visitorName == Z.visitorName) {
            return aa({
                error: "setVisitorName missing visitorName or unchanged from previous update"
            }, aj, "", "");
        } else {
            ai = Z.visitorName;
            Z.visitorName = aj.visitorName;
        }
        var ak = U.buildRequestObj({
            rel: "visitor-name",
            needAuth: true,
            type: "visitor",
            requestType: "PUT",
            data: {
                visitorName: aj.visitorName
            }
        });
        if (!ak) {
            return aa({
                error: "setVisitorName - unable to find rel for request"
            }, aj, "", "");
        }
        ak.success = function (am) {
            f(v.INFO, {
                originalRequest: aj,
                sentRequest: ak,
                response: am
            });
            if (am) {
                var al = {
                    visitorName: aj.visitorName
                };
                g.publish({
                    eventName: "onVisitorName",
                    data: al
                });
                ab(aj, al);
            }
            ak = null;
        };
        ak.error = function (al) {
            Z.visitorName = ai;
            y(aj, ak, al, "onVisitorName", "setVisitorName  - server error");
        };
        f(v.INFO, {
            originalRequest: aj,
            sentRequest: ak,
            response: "SENDING REQUEST"
        });
        return B.issueCall(ak);
    };
    K.getAgentTyping = function () {
        return Z.agentTyping;
    };
    K.getVisitorName = function () {
        return Z.visitorName;
    };
    K.getState = function () {
        return Z.state;
    };
    K.getAgentLoginName = function () {
        return Z.agentName;
    };
    K.getAgentId = function () {
        return Z.agentId;
    };
    K.getRtSessionId = function () {
        return Z.rtSessionId;
    };
    K.getConfig = function () {
        var aj = {};
        for (var ai in V) {
            if (V.hasOwnProperty(ai)) {
                aj[ai] = V[ai];
            }
        }
        return aj;
    };
    K.getSessionKey = function () {
        return Z.chatSessionKey;
    };
    K.resumeChat = function (ai) {
        if (ai && ai.URI) {
            if ((ai.lpNumber && ai.domain && ai.appKey) || Q()) {
                return I(ai);
            } else {
                return aa({
                    error: "missing parameters to resume session"
                }, ai, null, null);
            }
        } else {
            return aa({
                error: "missing parameters to resume session"
            }, ai, null, null);
        }
    };
    K.onLoad = function (ak, ai, aj) {
        return l("onLoad", ak, ai || "", aj);
    };
    K.onInit = function (ak, ai, aj) {
        return l("onInit", ak, ai || "", aj);
    };
    K.onStart = function (ak, ai, aj) {
        return l("onStart", ak, ai || "", aj);
    };
    K.onRequestChat = function (ak, ai, aj) {
        return l("onRequestChat", ak, ai || "", aj);
    };
    K.onTransferChat = function (ak, ai, aj) {
        return l("onTransferChat", ak, ai || "", aj);
    };
    K.onAvailableSkills = function (ak, ai, aj) {
        return l("onAvailableSkills", ak, ai || "", aj);
    };
    K.onAvailability = function (ak, ai, aj) {
        return l("onAvailability", ak, ai || "", aj);
    };
    K.onEstimatedWaitTime = function (ak, ai, aj) {
        return l("onEstimatedWaitTime", ak, ai || "", aj);
    };
    K.onAgentTyping = function (ak, ai, aj) {
        return l("onAgentTyping", ak, ai || "", aj);
    };
    K.onUrlPush = function (ak, ai, aj) {
        return l("onUrlPush", ak, ai || "", aj);
    };
    K.onLine = function (ak, ai, aj) {
        return l("onLine", ak, ai || "", aj);
    };
    K.onInfo = function (ak, ai, aj) {
        return l("onInfo", ak, ai || "", aj);
    };
    K.onEvents = function (ak, ai, aj) {
        return l("onEvents", ak, ai || "", aj);
    };
    K.onState = function (ak, ai, aj) {
        return l("onState", ak, ai || "", aj);
    };
    K.onStop = function (ak, ai, aj) {
        return l("onStop", ak, ai || "", aj);
    };
    K.onResume = function (ak, ai, aj) {
        return l("onResume", ak, ai || "", aj);
    };
    K.onAccountToAccountTransfer = function (ak, ai, aj) {
        return l("onAccountToAccountTransfer", ak, ai || "", aj);
    };
    K.onAddLine = function (ak, ai, aj) {
        return l("onAddLine", ak, ai || "", aj);
    };
    K.onVisitorName = function (ak, ai, aj) {
        return l("onVisitorName", ak, ai || "", aj);
    };
    K.onSetVisitorTyping = function (ak, ai, aj) {
        return l("onSetVisitorTyping", ak, ai || "", aj);
    };
    K.onEstimatedChatWaitTime = function (ak, ai, aj) {
        return l("onEstimatedChatWaitTime", ak, ai || "", aj);
    };
    K.onAvailableSlots = function (ak, ai, aj) {
        return l("onAvailableSlots", ak, ai || "", aj);
    };
    K.onTranscript = function (ak, ai, aj) {
        return l("onTranscript", ak, ai || "", aj);
    };
    K.onPreChatSurvey = function (ak, ai, aj) {
        return l("onPreChatSurvey", ak, ai || "", aj);
    };
    K.onOfflineSurvey = function (ak, ai, aj) {
        return l("onOfflineSurvey", ak, ai || "", aj);
    };
    K.onSubmitOfflineSurvey = function (ak, ai, aj) {
        return l("onSubmitOfflineSurvey", ak, ai || "", aj);
    };
    K.onExitSurvey = function (ak, ai, aj) {
        return l("onExitSurvey", ak, ai || "", aj);
    };
    K.onLog = function (ak, ai, aj) {
        return l("onLog", ak, ai || "", aj);
    };
    K.onError = function (ak, ai, aj) {
        return l("onError", ak, ai || "", aj);
    };
    K.unbind = function (aj, ai, al, ak) {
        return N(aj, ai, al, ak);
    };
    x(ae);
};
lpTag.taglets.ChatOverRestAPI.prototype.chatStates = {
    CHATTING: "chatting",
    WAITING: "waiting",
    ENDED: "ended",
    RESUMING: "resume",
    UNINITIALISED: "uninitialised",
    INITIALISED: "initialised",
    NOTFOUND: "notfound"
};
lpTag.taglets.ChatOverRestAPI.prototype.connectionStates = {
    ACTIVE: "active",
    INACTIVE: "inactive"
};