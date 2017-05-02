document.onkeypress = function(event) {
    event = (event || window.event);
    if (event.keyCode == 123 || event.keyCode == 73) {
        return false;
    }
};

document.onmousedown = function(event) {
    event = (event || window.event);
    if (event.keyCode == 123 || event.keyCode == 73) {
        return false;
    }
};

document.onkeydown = function(event) {
    event = (event || window.event);
    if (event.keyCode == 123 || event.keyCode == 73) {
        return false;
    }
};

function clickIE() { if (document.all) { return false; } }
function clickNS(e) {
    if(document.layers || (document.getElementById && !document.all)) {
        if (e.which == 2 || e.which == 3) { return false; }
    }
}

if (document.layers)
{ document.captureEvents(Event.MOUSEDOWN); document.onmousedown = clickNS; }
else { document.onmouseup = clickNS; document.oncontextmenu = clickIE; }
document.oncontextmenu = new Function("return false");

var _sid = 0;
	
var SW = function(user, translations, coordinates, swc, pic, isMobile, isLoggedIn) {
    this.u = user;
    this.t = translations;
    this.ca = coordinates;
    this.im = isMobile;
    this.swp = [];
    this.hl = false;
    this.is = false;
    this.swr = {};
    this.wp = {};
    this.la = 0;
    this.swc = swc;
    this.pic = pic;
    this.siid = 0;
    this.ciid = 0;
    this.isLoggedIn = isLoggedIn;
};

window.onload = function () {
    if (!sw.swc) 
        sw.swc = {h: $('#roulette').height(), w: $('#roulette').width()};    
    sw._db_();
    sw._isw_();
    document.getElementById('spinWheel').addEventListener('wheel', function () { return false; });
    sw._obsM_();
};

SW.prototype._obsM_ = function() {
    try {
        var _mo = (function() {
            var prefixes = ['WebKit', 'Moz', 'O', 'Ms', ''];
            for (var i = 0; i < prefixes.length; i++) {
                if (prefixes[i] + 'MutationObserver' in window) {
                    return window[prefixes[i] + 'MutationObserver'];
                }
            }
            return false;
        }());

        if (!_mo) {
            return;
        }

        var self = this,
            _t1 = document.getElementById('spinWheelContainer'),
            _obs1 = new MutationObserver(function(mutations) {
                mutations.forEach(function(mutation) {
                    if (!mutation || !mutation.target || !mutation.target.attributes || !mutation.target.attributes.id) return;
                    var id = mutation.target.attributes.id.nodeValue;
                    if (self.hl && !self.is && id === 'spinWheel') {
                        var _tr = mutation.target.style.transform || mutation.target.style.WebkitTransform || mutation.target.style.MozTransform || mutation.target.style.msTransform;
                        if (_.isEmpty(_tr)) return;
                        if (self.la !== parseInt((_tr.split('rotate(')[1]).split('deg)')[0])) {
                            document.getElementById('spinWheel').style.visibility = 'hidden';
                            window.location.reload();
                        }
                    }
                });
            }),
            _t2 = document.getElementById('prizeModal'),
            _obs2 = new MutationObserver(function(mutations) {
                mutations.forEach(function(mutation) {
                    if (!mutation || !mutation.target || !mutation.target.attributes || !mutation.target.attributes.id) return;
                    var id = mutation.target.attributes.id.nodeValue;
                    if (self.hl && !self.is && id === 'prizeContainer') {
                        document.getElementById('spinWheel').style.visibility = 'hidden';
                        window.location.reload();
                    }
                });
            }),
            _cfg = {
                attributes: true,
                childList: true,
                characterData: true,
                subtree: true,
                attributeFilter: ['style']
            };

        // pass in the target node, as well as the observer options
        _obs1.observe(_t1, _cfg);
        _obs2.observe(_t2, _cfg);
    } catch (e) {}
};

function _dg_(obj, prop, fn) {

    if (Object.defineProperty)
        return Object.defineProperty(obj, prop, fn);
    if (Object.prototype.__defineGetter__)
        return obj.__defineGetter__(prop, fn.get);

    throw new Error("no support");
}

function _ds_(obj, prop, fn) {

    if (Object.defineProperty)
        return Object.defineProperty(obj, prop, fn);
    if (Object.prototype.__defineSetter__)
        return obj.__defineSetter__(prop, fn.set);

    throw new Error("no support");
}

SW.prototype._dsw_ = function() {
    var c = document.getElementById('swc'),
        ctx = c.getContext('2d'),
        imgs = {},
        ri = new Image(),
        self = this;
    ctx.canvas.height = self.swc.h;
    ctx.canvas.width = self.swc.w;
    ctx.save();
    for (var x = 0; x < 8; x++) {
        imgs[x] = new Image();
    }

    ri.src = '/_static/Images/Spinwheel/spinwheel2.png';
    ri.onload = function () {
        ctx.drawImage(ri, 0, 0, self.swc.w, self.swc.h);
        imgs[0].src = self.swp[0].is;
        imgs[0].onload = function() {
            // Draw prize 1
            ctx.drawImage(imgs[0], self.ca[0].a, self.ca[0].b, self.swc.w, self.swc.h);
            imgs[1].src = self.swp[1].is;
            imgs[1].onload = function() {
                // Draw prize 2
                self._rpi_(ctx, imgs[1], 45, self.ca[1].a, self.ca[1].b);
                imgs[2].src = self.swp[2].is;
                imgs[2].onload = function() {
                    // Draw prize 3
                    self._rpi_(ctx, imgs[2], 90, self.ca[2].a, self.ca[2].b);
                    imgs[3].src = self.swp[3].is;
                    imgs[3].onload = function() {
                        // Draw prize 4
                        self._rpi_(ctx, imgs[3], 135, self.ca[3].a, self.ca[3].b);
                        imgs[4].src = self.swp[4].is;
                        imgs[4].onload = function() {
                            // Draw prize 5
                            self._rpi_(ctx, imgs[4], 180, self.ca[4].a, self.ca[4].b);
                            imgs[5].src = self.swp[5].is;
                            imgs[5].onload = function() {
                                // Draw image 6
                                self._rpi_(ctx, imgs[5], 225, self.ca[5].a, self.ca[5].b);
                                imgs[6].src = self.swp[6].is;
                                imgs[6].onload = function() {
                                    // Draw image 7
                                    self._rpi_(ctx, imgs[6], 270, self.ca[6].a, self.ca[6].b);
                                    imgs[7].src = self.swp[7].is;
                                    imgs[7].onload = function() {
                                        // Draw image 8
                                        self._rpi_(ctx, imgs[7], 315, self.ca[7].a, self.ca[7].b);
                                    };
                                };
                            };
                        };
                    };
                };
            };
        };
    };
};

SW.prototype._rpi_ = function (ctx, img, deg, x, y) {
    var self = this;
    ctx.rotate(deg * Math.PI / 180);
    ctx.drawImage(img, x, y, self.swc.w, self.swc.h);
    ctx.restore();
    ctx.save();
};

SW.prototype._isw_ = function() {
    var self = this;
    if (!self.isLoggedIn) {
        $('#loginFrame').modal('show');
        return;
    }

    if (typeof String.prototype.trim !== 'function') {
        String.prototype.trim = function() {
            return this.replace(/^\s+|\s+$/g, '');
        };
    }
    self._t_(0);
    $.ajax({
        type: 'GET',
        async: true,
        url: '/api/rewards/spinwheel/initialize',
        contentType: 'application/json',
        data: self.u,
        success: function(response) {
            try {
                if (!response || !response.ResponseData) {
                    self._sem_(self.t.message5, true);
                    $('#spinWheelContent').show();
                    return;
                }
                self.swr = response;
                self._iv_();
            } catch (e) {
                self._sem_(self.t.message5, true);
            }
            $('#spinWheelContent').show();
        },
        error: function() {
            self._sem_(self.t.message5, true);
            $('#spinWheelContent').show();
        }
    });
};

SW.prototype._iv_ = function () {
    var self = this,
        data = self.swr.ResponseData;
    if (_.isEmpty(data) || _.isEmpty(data.PrizeItems)) {
        self._sem_(self.swr.ResponseMessage, true);
        return;
    }

    var self = this;
    _.each(data.PrizeItems, function(p, i) {
        var index = i + 1,
            r = 0,
            o = 0;
        switch (index) {
            case 1:
                r = 0;
                o = 360;
                break;
            case 2:
                r = 45;
                o = 315;
                break;
            case 3:
                r = 90;
                o = 270;
                break;
            case 4:
                r = 135;
                o = 225;
                break;
            case 5:
                r = 180;
                o = 180;
                break;
            case 6:
                r = 225;
                o = 135;
                break;
            case 7:
                r = 270;
                o = 90;
                break;
            case 8:
                r = 315;
                o = 45;
                break;
        }

        if (p) {
            document.getElementById('prize' + index).innerHTML = p.PrizeName;
            self.swp.push({
                n: 'prize' + index,
                r: r,
                o: o,
                i: i,
                is: p.ImagePath,
                pn: p.PrizeName,
                c: p.ProductCode
            });
        } 
    });
    self._dsw_();
    self._ssw_(true);
};

SW.prototype._sr_ = function() {
    var self = this,
        degrees = 0;
    self.siid = setInterval(function() {
        if (degrees < 360) {
            degrees += 15;
        } else {
            degrees = 0;
        }
        self._t_(degrees);
    }, 50);
};

SW.prototype._is_ = function () {
    var self = this;
    self._ar_(self.la, self.la - 20, 600, false, true);
};

SW.prototype._ar_ = function(fromDegrees, toDegrees, duration, hasError, isInit) {
    var self = this;
    $({ deg: fromDegrees }).animate({ deg: toDegrees }, {
        duration: duration,
        easing: 'easeOutQuint',
        step: function(now) {
            self._t_(now);
        },
        complete: function() {
            self.la = toDegrees;
            if (!self.wp || self.wp.c == 0) {
                self._sem_(self.t.wonNothing);
                self._isw_();
                return;
            }
            if (hasError) {
                self._sem_(self.t.message5);
                return;
            }
            if (isInit) {
                self._sr_();
            } else {
                self._sp_(self.t.claimMessage);
            }
        }
    });
};

SW.prototype._ss_ = function(hasError) {
    var self = this;
    clearInterval(self.siid);
    self._ar_(self._gar_(), 720 + (self.wp === null || self.wp.c === undefined ? 0 : self.wp.o), 6000, hasError);
};

function _gp_() {
    sw.is = true;
    sw._rs_();
    sw._is_();
    $.ajax({
        type: 'POST',
        async: true,
        url: '/api/rewards/spinwheel/spin',
        contentType: 'application/json',
        data: JSON.stringify(sw.u),
        success: function (response) {
            try {
                if (!response || !response.ResponseData) {
                    sw._ss_(true);
                    return;
                }
                setTimeout(function () {
                    sw.swr = response;
                    sw._gr_(sw.swr);
                }, 700);           
            } catch (e) {
                sw._ss_(true);
            }
        },
        error: function () {
            sw._ss_(true);
        }
    });
}

SW.prototype._gr_ = function (response) {
    var self = this;
    self.wp = {};
    self.wp = _.find(self.swp, { c: response.ResponseData.ProductCode });
    if (!self.wp) {
        self._ss_(true);
    } else {
        self._ss_();
    }
};

function _rp_() {
    $('#claimButton').hide();
    sw._ssr_(sw.t.successfulClaim);
}

SW.prototype._eb_ = function() {
    $('#spinButton').show();
    $('#spinButton').css('background', '#2a8fbd');
    $('#spinButton').attr('onclick', 'javascript: _gp_();');
    $('#spinButton').attr('href', '#');
    $('#spinButton').prop('disabled', false);
};

SW.prototype._db_ = function() {
    $('#spinButton').css('background', '#808080');
    $('#spinButton').attr('onclick', null);
    $('#spinButton').attr('href', null);
    $('#spinButton').prop('disabled', true);
};

SW.prototype._sp_ = function (message) {
    var self = this;
    message = message.replace('[prize]', self.wp.pn);
    var splitMessage = message.split('<br />');
    $('#spinMessage').html('<span>' + splitMessage[0] + '</span><span>' + splitMessage[1] + '</span>');
    self._dp_();
    $('#claimButton').show();
    $('#prizeModal').modal('show');
    self.is = false;
};

SW.prototype._ssr_ = function(message) {
    if (message.indexOf('html') !== -1) return;
    $('#spinMessage').html(message);
    $('#okButton').attr('onclick', 'javascript: _tp_(true);');
    $('#claimButton').hide();
    $('#okButton').show();
};

SW.prototype._sem_ = function(message, isInit) {
    $('#claimButton').hide();
    $('#okButton').show();
    $('#spinsLeft').html(message);
    $('#okButton').attr('onclick', 'javascript: _tp_();');
    self.is = false;
    if (!isInit) {
        $('#spinMessage').html(message);
        $('#prizeModal').modal('show');
    } else {
        $('#spinsLeft').show();
        $('#spinWheelContent').show();
    }
};

function _tp_(isRedemption) {
    $('#prizeModal').modal('hide');
    $('#okButton').hide();
    if (isRedemption) {
        sw._ssw_(true);
    }
}

SW.prototype._esw_ = function(isInit) {
    var self = this;
    $('#spinsLeft').html(self.t.spinsLeftLabel1 + '<span>' + self.swr.ResponseData.Spins + '</span>' + self.t.spinsLeftLabel2);
    self._eb_();
    if (isInit) {
        document.getElementById('spinWheel').style.display = 'inline-block';
    }
};

SW.prototype._ssw_ = function (isInit) {
    var self = this;
    self.hl = true;
    if (isInit) {
        $('#spinsLeft').show();
    }
    if (self.swr.ResponseCode != 0) {
        self._db_();
        $('#spinButton').hide();
        self._sem_(self.swr.ResponseMessage, isInit);
        if (self.swr.ResponseCode == 1) {
            self._scd_(self.swr.ResponseData.Frequency);
        }
        return;
    }
    self._esw_(isInit);
};

SW.prototype._scd_ = function(frequency) {
    var self = this;
    $('#spinsLeft').html(self.swr.ResponseMessage);
    try {
        var currentDate = new Date(self.swr.ResponseData.ServerTime),
            aDayInMillis = 1000 * 60 * 60 * 24,
            anHourInMillis = 1000 * 60 * 60,
            aMinuteInMillis = 1000 * 60;
        self.ciid = setInterval(function() {
            var millisec,
                now = date.getTime();
            switch (frequency) {
                case 2:
                    millisec = (new Date(currentDate.getFullYear(), currentDate.getMonth(), currentDate.getDate(), 23, 59, 59)).getTime();
                    break;
                case 3:
                    millisec = (new Date(currentDate.getFullYear(), currentDate.getMonth(), currentDate.getDate() + (7 - currentDate.getDay()), 23, 59, 59)).getTime();
                    break;
                case 4:
                    millisec = (new Date(currentDate.getFullYear(), currentDate.getMonth() + 1, 0, 23, 59, 59)).getTime();
                    break;
            }
            var diff = millisec - now;
            if (diff <= 0) {
                window.location.reload();
                clearInterval(self.ciid);
                return;
            }
            var days = Math.floor(diff / aDayInMillis),
                hours = Math.floor((diff / anHourInMillis) % 24),
                minutes = Math.floor((diff / aMinuteInMillis) % 60),
                secs = Math.floor((diff / 1000) % 60),
                cells = document.getElementById('countdownContainer').getElementsByTagName('div');
            for (var i = 0; i < cells.length - 1; i++) {
                switch (i) {
                    case 0:
                        cells[i].innerHTML = '<p>' + days + '</p><p>' + self.t.countdownDay + '</p>';
                        break;
                    case 1:
                        cells[i].innerHTML = '<p>' + hours + '</p><p>' + self.t.countdownHour + '</p>';
                        break;
                    case 2:
                        cells[i].innerHTML = '<p>' + minutes + '</p><p>' + self.t.countdownMin + '</p>';
                        break;
                    case 3:
                        cells[i].innerHTML = '<p>' + secs + '</p><p>' + self.t.countdownSec + '</p>';
                        break;
                }
            }
            if ($('#countdownContainer').css('display') === 'none') {
                $('#countdownContainer').show();
            }
        }, 1000);
    } catch (e) {}
};

SW.prototype._rs_ = function () {
    var self = this;
    self._db_();
    self.wp = {};
    self._cc_('pic');
    $('#spinMessage').html('');
    $('#okButton').hide();
    $('#claimButton').hide();
};

function getRandomInt(min, max) {
    return Math.floor(Math.random() * (max - min)) + min;
}

SW.prototype._t_ = function(degrees) {
    var st = document.getElementById('spinWheel').style;
    if (st.hasOwnProperty('WebkitTransform') || st.constructor.prototype.hasOwnProperty('WebkitTransform')) {
        st['WebkitTransform'] = 'rotate(' + degrees + 'deg)';
    }
    if (st.hasOwnProperty('MozTransform') || st.constructor.prototype.hasOwnProperty('MozTransform')) {
        st['MozTransform'] = 'rotate(' + degrees + 'deg)';
    }
    if (!_.isEmpty(_.pick(st, 'msTransform'))) {
        st['msTransform'] = 'rotate(' + degrees + 'deg)';
    }
    if (st.hasOwnProperty('transform') || st.constructor.prototype.hasOwnProperty('transform')) {
        st['transform'] = 'rotate(' + degrees + 'deg)';
    }
};

SW.prototype._dp_ = function() {
    var self = this,
        c = document.getElementById('pic'),
        ctx = c.getContext('2d'),
        pi = new Image();
    ctx.canvas.width = self.pic.w;
    ctx.canvas.height = self.pic.h;
    ctx.save();
    pi.src = self.wp.is;
    pi.onload = function() {
        ctx.drawImage(pi, self.pic.a, self.pic.b);
        ctx.restore();
        ctx.save();
    };
};

SW.prototype._cc_ = function(id) {
    var c = document.getElementById(id),
        ctx = c.getContext('2d');
    ctx.clearRect(0, 0, c.width, c.height);
};

SW.prototype._gar_ = function() {
    var el = document.getElementById('spinWheel');
    var st = window.getComputedStyle(el, null);
    var tr = st.getPropertyValue('-webkit-transform') ||
        st.getPropertyValue('-moz-transform') ||
        st.getPropertyValue('-ms-transform') ||
        st.getPropertyValue('-o-transform') ||
        st.getPropertyValue('transform') ||
        st.getPropertyValue('msTransform') ||
        'FAIL';
    if (tr === 'FAIL') return 0;
    var values = tr.split('(')[1].split(')')[0].split(','),
        a = values[0],
        b = values[1],
        radians = Math.atan2(b, a);
    if (radians < 0) {
        radians += (2 * Math.PI);
    }
    var angle = Math.round(radians * (180 / Math.PI));
    return angle;
};