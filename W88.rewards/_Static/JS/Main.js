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
                        if (!_.isEmpty(data.message)) w88Mobile.Growl.shout(data.message);
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

    $('.navbar-toggle').on('click', function () {
        $(this).toggleClass('active');
    });
});

(function (send) {
    XMLHttpRequest.prototype.send = function (data) {
        if (window.user && window.user.Token)
            this.setRequestHeader('token', window.user.Token);
        send.call(this, data);
    };
})(XMLHttpRequest.prototype.send);

$(document).on('pagecontainerbeforeshow', function (event, ui) {
    toggleLoginButton();
    var baseUri = [event.target.baseURI];
    if (_.some(baseUri, _.method('match', /Login/i))) {
        if (window.user && window.user.Token) {
            loadPage('/Index.aspx', null, 'slide');
        }
    }

    if ($.mobile) {
        if ($.mobile.navigate.history.stack.length === 0)
            $('#backButton').hide();
        else
            $('#backButton').show();
    }

    if (GPINTMOBILE)
        GPINTMOBILE.HideSplash();
});

$(document).on('pagecontainershow', function (event, ui) {

});

$(document).on('pagecontainerbeforechange', function (event, ui) {

});

$(document).on('pagecontainerbeforeload', function (event, ui) {

});

function loadPage(uri, params, transition) {
    $(':mobile-pagecontainer').pagecontainer('change', uri, { data: params, reload: true, transition: transition });
}

function logout() {
    setUser();
    if (_.isEmpty(window.user)) return;
    $.ajax({
        url: '/api/user/logout',
        contentType: 'text/html',
        async: true,
        data: 'MemberId=' + window.user.MemberId,
        beforeSend: function() {
            $.mobile.loading('show');
        },
        success: function (response) {
            switch (response.ResponseCode) {
                case 1:
                    clear();
                    break;
                default:
                    $.mobile.loading('hide');
                    if (_.isEmpty(response.ResponseMessage)) return;
                    window.w88Mobile.Growl.shout(response.ResponseMessage);
                    break;
            }
        },
        error: function (response) {
            $.mobile.loading('hide');
            window.w88Mobile.Growl.shout(response.ResponseMessage);
        }
    });
}

function clear() {
    try {
        window.localStorage.clear();
        Cookies().setCookie('user', null, -1);
        Cookies().setCookie('product', null, -1);
    } catch (e) {
        Cookies().setCookie('user', null, -1);
        Cookies().setCookie('product', null, -1);
    }
    window.user = null;
    $.mobile.loading('hide');
    window.location.href = '/Logout';
}

function Cookies() {
    var setCookie = function (cname, cvalue, expiryDays) {
        var date = new Date();
        date.setTime(date.getTime() + (expiryDays * 24 * 60 * 60 * 1000));
        var expires = 'expires=' + date.toUTCString(),
            splitHost = window.location.host.split('.'),
            domain = splitHost.length > 2 ? (splitHost[1] + '.' + splitHost[2]) : (splitHost[0] + '.' + splitHost[1]);
        document.cookie = cname + '=' + cvalue + ';expires=' + expires + ';domain=' + domain + ';path=/';
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

function setUser() {
    try {
        var storedObject = window.localStorage.getItem('user');
        window.user = _.isEmpty(storedObject) ? new User() : (new User()).createUser(storedObject);
    } catch (e) {
        var cookieObject = Cookies().getCookie('user');
        window.user = _.isEmpty(cookieObject) ? new User() : (new User()).createUser(cookieObject);
    }
}

function toggleLoginButton() {
    var headerLoginButton = $('div.dropdown ul>li#headerLoginButton'),
        headerLogoutButton = $('div.dropdown ul>li#headerLogoutButton'),
        loginFooterButton = $('div.btn-group a#loginFooterButton'),
        logoutFooterButton = $('div.btn-group a#logoutFooterButton'),
        submitButton = $('#btnSubmit');

    if (headerLoginButton && headerLogoutButton) {
        if (window.user && window.user.hasSession()) {
            headerLoginButton.hide();
            headerLogoutButton.show();
        } else {
            headerLogoutButton.hide();
            headerLoginButton.show();
        }
    }

    if (loginFooterButton && logoutFooterButton) {
        if (window.user && window.user.hasSession()) {
            loginFooterButton.hide();
            logoutFooterButton.show();
        } else {
            logoutFooterButton.hide();
            loginFooterButton.show();
        }
    }

    if (submitButton) {
        if (window.user && window.user.hasSession()) {
            $('#btnSubmit').hide();
        } else {
            $('#btnSubmit').show();
        }
    }
}

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

if ($('#divBalance').hasClass('open')) { $('#divBalance').addClass('close'); } else { if ($('#divBalance').hasClass('open')) { $('#divBalance').addClass('close'); } }