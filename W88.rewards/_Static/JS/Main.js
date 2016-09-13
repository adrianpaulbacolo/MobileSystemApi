var User = function User() {
    this.Balance = null;
    this.CountryCode = null;
    this.CurrencyCode = null;
    this.FirstName = null;
    this.LanguageCode = null;
    this.MemberId = null;
    this.ResetPassword = false;
    this.PartialSignup = null;
    this.Token = null;
};

User.prototype.convertToJsonString = function() {
    return JSON.stringify(this);
};

User.prototype.createUser = function(user) {
    var obj = JSON.parse(user);
    this.CurrencyCode = _.isEmpty(obj.CurrencyCode) ? null : obj.CurrencyCode;
    this.LanguageCode = _.isEmpty(obj.LanguageCode) ? null : obj.LanguageCode;
    this.MemberId = _.isEmpty(obj.MemberId) ? null : obj.MemberId;
    this.ResetPassword = _.isEmpty(obj.ResetPassword) ? false : obj.ResetPassword;
    this.Token = _.isEmpty(obj.Token) ? null : obj.Token;
    return this;
};

User.prototype.save = function () {
    if (_.isEmpty(this)) return;
    var user = this.convertToJsonString();
    try {
        window.localStorage.setItem('user', user);
    } catch (e) {
        Cookies().setCookie('user', user, 30);
    }
};

User.prototype.hasSession = function() {
    return !_.isEmpty(this.Token);
};

User.prototype.setProperties = function (data) {
    if (_.isEmpty(data)) return;
    this.CurrencyCode = data.CurrencyCode || null;
    this.LanguageCode = data.LanguageCode || null;
    this.MemberId = data.MemberId || null;
    this.ResetPassword = data.ResetPassword || false;
    this.Token = data.Token || null;
    return this;
};

// Set window.user property
setUser();

$(window).load(function () {
    GPINTMOBILE.HideSplash();

    var sessionPoll;
    function checkSession() {
        var intervalMin = 10000, // 10000
            sessionInterval = (window.user && parseInt(sessionInterval) > intervalMin) 
            ? parseInt(sessionInterval) : intervalMin;
            
        sessionPoll = window.setInterval(function () {
            setUser();
            $.ajax({
                contentType: 'application/json; charset=utf-8;',
                url: '/_Secure/AjaxHandlers/MemberSessionCheck.ashx',
                type: 'POST',
                data: JSON.stringify(window.user),
                success: function (data) {
                    if (data.Code !== 1) {
                        if (typeof data.message != "undefined") w88Mobile.Growl.shout(data.message);
                        clearInterval(sessionPoll);
                        setTimeout(function() {
                            logout();
                        }, 2000);
                    }
                },
                error: function (err) {
                }
            });
        }, sessionInterval);
    }

    if (window.user && window.user.hasSession())
        checkSession();

    $(".navbar-toggle").on("click", function () {
        $(this).toggleClass("active");
    });
});

function logout() {
    setUser();
    if (_.isEmpty(window.user)) return;
    $.ajax({
        url: '/api/user/logout',
        type: 'GET',
        contentType: 'text/html',
        async: true,
        data: 'MemberId=' + window.user.MemberId,
        success: function (response) {
            switch (response.ResponseCode) {
                case 1:
                    clear();
                    break;
                default:
                    window.w88Mobile.Growl.shout(response.ResponseMessage);
                    break;
            }
        },
        error: function (response) {
            window.w88Mobile.Growl.shout(response.ResponseMessage);
        }
    });
}

function setUser() {
    try {
        var storedObject = window.localStorage.getItem('user');
        window.user = _.isEmpty(storedObject) ? new User() : (new User()).createUser(storedObject);
    } catch (e) {
        var cookieObject = Cookies().getCookie('user');
        window.user = _.isEmpty(cookieObject) ? new User() : (new User()).createUser(cookieObject);
    }
}

function clear() {
    try {
        window.localStorage.clear();
        Cookies().setCookie('user', null, -1);
    } catch (e) {
        Cookies().setCookie('user', null, -1);
    }
    window.user = null;
    window.location.href = '/Default.aspx';
}

function Cookies() {
    var setCookie = function (cname, cvalue, expiryDays) {
        var date = new Date();
        date.setTime(date.getTime() + (expiryDays * 24 * 60 * 60 * 1000));
        var expires = 'expires=' + date.toUTCString();
        document.cookie = cname + '=' + cvalue + ';' + expires + ';path=/';
    };

    var getCookie = function (cname) {
        var name = cname + '=';
        var cookies = document.cookie.split(';');
        for (var i = 0; i < cookies.length; i++) {
            var cookie = cookies[i];
            while (cookie.charAt(0) == ' ')
                cookie = cookie.substring(1);

            if (cookie.indexOf(name) == 0) {
                return cookie.substring(name.length, cookie.length);
            }
        }
        return '';
    };

    return {
        setCookie: setCookie,
        getCookie: getCookie
    };
}

if ($('#divBalance').hasClass('open')) { $('#divBalance').addClass('close'); } else { if ($('#divBalance').hasClass('open')) { $('#divBalance').addClass('close'); } }

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