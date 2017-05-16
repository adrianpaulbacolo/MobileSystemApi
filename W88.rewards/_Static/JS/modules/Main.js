var sessionPoll;
// Set window.user property
setUser();
$(window).load(function () {
    GPINTMOBILE.HideSplash();
    function checkSession() {
        var intervalMin = 3000,
            interval = (window.user && parseInt(sessionInterval) > 0) 
            ? parseInt(sessionInterval) : intervalMin;
            
        sessionPoll = window.setInterval(function () {
            setUser();
            $.ajax({
                contentType: 'text/html',
                url: '/_Secure/AjaxHandlers/MemberSessionCheck.ashx',
                type: 'POST',
                data: window.user.Token,
                success: function (data) {
                    if (!data) return;
                    if (data.Code === 1) {
                        return;
                    }

                    if (!_.isEmpty(data.Message)) w88Mobile.Growl.shout(data.Message);
                    clearInterval(sessionPoll);
                    setTimeout(function() {
                        clear();
                    }, 2000);                    
                },
                error: function (err) {
                }
            });
        }, interval);
    }

    if (window.user && window.user.hasSession())
        checkSession();

    $('.navbar-toggle').on('click', function () {
        $(this).toggleClass('active');
    });
});

$(document).on('pagecontainerbeforeshow', function (event, ui) {
    toggleButtons();

    var baseUri = [event.target.baseURI];
    if (_.some(baseUri, _.method('match', /Login/i))) {
        if (window.user && window.user.Token) {
            window.location.href = '/Index.aspx';
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
            if (sessionPoll) clearInterval(sessionPoll);
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
        if (!_.isEmpty(Cookies().getCookie('user')))
            Cookies().setCookie('user', null, -1);
        if (!_.isEmpty(Cookies().getCookie('isvip')))
            Cookies().setCookie('isvip', null, -1);
        if (!_.isEmpty(Cookies().getCookie('token')))
            Cookies().setCookie('token', null, -1);
    } catch (e) {
        if (!_.isEmpty(Cookies().getCookie('user')))
            Cookies().setCookie('user', null, -1);
        if (!_.isEmpty(Cookies().getCookie('isvip')))
            Cookies().setCookie('isvip', null, -1);
        if (!_.isEmpty(Cookies().getCookie('token')))
            Cookies().setCookie('token', null, -1);
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
    var storedObject;
    try {
        storedObject = window.localStorage.getItem('user');
        if (_.isEmpty(storedObject)) {
            storedObject = Cookies().getCookie('user');
        }
    } catch (e) {
        storedObject = Cookies().getCookie('user');
    } 

    window.user = _.isEmpty(storedObject) ? new User() : (new User()).createUser(storedObject);
}

function toggleButtons() {
    var headerLoginButton = $('div.dropdown ul>li#headerLoginButton'),
        headerLogoutButton = $('div.dropdown ul>li#headerLogoutButton'),
        loginFooterButton = $('div.btn-group a#loginFooterButton'),
        logoutFooterButton = $('div.btn-group a#logoutFooterButton'),
        spinWheelLinkButton = $('div.dropdown ul>li#spinWheelLink'),
        submitButton = $('#btnSubmit');

    setUser();
    if (headerLoginButton && headerLogoutButton)
        window.user && window.user.hasSession() ? (headerLoginButton.hide(), headerLogoutButton.show())
        : (headerLogoutButton.hide(), headerLoginButton.show());
    if (loginFooterButton && logoutFooterButton)
        window.user && window.user.hasSession() ? (loginFooterButton.hide(), logoutFooterButton.show())
        : (logoutFooterButton.hide(), loginFooterButton.show());
    if (submitButton)
        window.user && window.user.hasSession() ? $("#btnSubmit").hide() : $("#btnSubmit").show();
        }

        if (spinWheelLinkButton) 
            spinWheelLinkButton.show();
        
        if (submitButton) 
            $('#btnSubmit').hide();      
        if (headerLoginButton && headerLogoutButton) {
            headerLogoutButton.hide();
            headerLoginButton.show();
        }
        if (loginFooterButton && logoutFooterButton) {
            spinWheelLinkButton.hide();
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