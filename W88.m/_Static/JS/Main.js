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
                        if (typeof data.message != "undefined") window.w88Mobile.Growl.shout(data.message, function () {
                            window.location.replace("/Logout");
                        });
                        clearInterval(sessionPoll);
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

function CheckWholeNumber(element) {
    if (element.val().length > 0) {
        if (element.val().indexOf('.') >= 0) {
            element.parent("div.ui-input-text").attr("style", "border-bottom: 2px solid red !important");
            $('#amtErr').show();
            return false;
        } else {
            element.parent("div.ui-input-text").removeAttr("style");
            $('#amtErr').hide();
            return true;
        }
    } else {
        return false;
    }
}

function ValidatePositiveDecimal(ctrl, e, cur) {
    var allowDecimal;
    if (cur === undefined) cur = "";
    switch (cur) {
        case "JPY":
            allowDecimal = false;
            break;
        default:
            allowDecimal = true;
            break;
    }

    var key = e.keyCode;
    if ($.browser.mozilla) {
        key = e.which;
    }
    if (key != 0 && key != 8) {
        if (key == 46) {
            if (!allowDecimal) return false;

            var code = String.fromCharCode(key);
            if (ctrl.value.indexOf(code) >= 0)
                return false;
        }
        else if (key < 48 || key > 57)
            return false;
        else {
            var num = parseFloat($(ctrl).val() + String.fromCharCode(key));
            var cleanNum = num.toFixed(2);
            if (num / cleanNum != 1)
                return false;
        }
    }
}

function PositiveDecimal(value, cur) {
    var allowDecimal;
    if (cur === undefined) cur = "";
    switch (cur) {
        case "JPY":
            allowDecimal = false;
            break;
        default:
            allowDecimal = true;
            break;
    }

    if (!allowDecimal) {
        return value % 1 === 0;
    }

    var num = parseFloat(value);
    var cleanNum = num.toFixed(2);
    if (num / cleanNum != 1)
        return false;

    return true;
}

function PositiveOneDecimalValidation(value, element) {
    if (isNaN(value) || value <= 0)
        return false;
    var numArr = value.split(".");
    if (numArr.length > 1 && numArr[1].length > 1)
        return false;
    return true;
}

function TwoDecimalAndroid(ctrl, event) {
    var $this = ctrl;
    if ((event.which != 46 || $this.val().indexOf('.') != -1) &&
       ((event.which < 48 || event.which > 57) &&
       (event.which != 0 && event.which != 8))) {
        event.preventDefault();
    }

    var text = $this.val();
    if ((event.which == 46) && (text.indexOf('.') == -1)) {
        setTimeout(function () {
            if ($this.val().substring($this.val().indexOf('.')).length > 3) {
                $this.val($this.val().substring(0, $this.val().indexOf('.') + 3));
            }
        }, 1);
    }

    if ((text.indexOf('.') != -1) && (text.substring(text.indexOf('.')).length > 2) && (event.which != 0 && event.which != 8) && ($this[0].selectionStart >= text.length - 2)) {
        event.preventDefault();
    }
}