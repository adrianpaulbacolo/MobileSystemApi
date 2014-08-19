// PUSH - Adds new elements to the end of an array, and returns the new length
// POP - Removes the last element of an array, and returns that element
// UNSHIFT - Adds new elements to the beginning of an array, and returns the new length
// SHIFT - Removes the first element of an array, and returns that element

//http://localhost:53698/Slots/default.html
var SLOT_SPEED = 60;
var SLOT_COLUMNS = 5;

var SLOTS_ITEMS_ARRAY = [{ id: '1' }, { id: '2' }, { id: '3' }, { id: '4' }, { id: '5' }, { id: '6' }, { id: '7' }, { id: '8' }, { id: '9' }, { id: '10' }, { id: '11' }];
var SLOTS_BG_ARRAY = [{ id: 'up', type: '.jpg' }, { id: 'right', type: '.jpg' }, { id: 'below', type: '.jpg' }, { id: 'left', type: '.jpg' }, { id: 'screen', type: '.jpg' }, { id: 'winframe', type: '.png' }];
var SLOTS_BONUS_TMU_ARRAY = [{ id: 1 }, { id: 2 }, { id: 3 }, { id: 4 }, { id: 5 }, { id: 6 }];	//tan me up
var SLOTS_BONUS_SW_ARRAY = [{ id: 1 }, { id: 2 }, { id: 3 }, { id: 0 }, { id: 4 }, { id: 5 }];	//swimwear
var SLOTS_BONUS_FS_ARRAY = [{ id: 1 }];	//freespin

var SLOTS_ARRANGED_ASSETS = [];
var SLOTS_ITEMS_ASSETS = [];
var SPIN_COLUMNS = [];
var SLOTS_RESULTS_ASSETS = [];

var SLOT_IMG_WIDTH = 199; //282
var SLOT_IMG_HEIGHT = 191; //270

var SLOT_X_OFFSET = 5;
var SLOT_Y_OFFSET = 1;

var CVSMAIN = null;
var CTXMAIN = null;

var REELS_START_X = 0;
var REELS_START_Y = 0;

var MAX_SCREEN_WIDTH = 1136;
var MAX_SCREEN_HEIGHT = 625;

var SCREEN_WIDTH = 1136;
var SCREEN_HEIGHT = 600;

var STARTTIMER, STOPTIMER, BONUSSTARTTIMER, BONUSSTOPTIMER;
var SPIN_STARTED = false;
var SPIN_STOPPED = false;
var SPIN_PAUSED = false;
var SPIN_RESULTS = null;
var AUTO_SPIN_MAX = 3;
var AUTO_SPIN_CURR = 0;
var AUTO_SPIN = false;

var ISFULLSCREEN = false;

var FPS = 1000 / 30; //30FPS
var BALANCE_XML = null;
var NEW_BALANCE_XML = null;
var MEMBER_CURR = null;

var HIGHLIGHT = false;
var HIGHLIGHT_OBJ = [];

var BKEY = null;
var BSTEPS = 0;
var LOADBONUS = false;
var BONUSTYPE = null;
var BONUSWIN = 0;
var BONUSWIN_SINGLE = 0;
var MEMBERCODE = '-';
var BONUSLOADED = false;
var BONUS_FREESPIN_REMAINING = 0;

var GAMENAME = '';

var PLAY_BETLINES = '30';
var PLAY_MULIPLY = '20';
var PLAY_AMOUNT = '0.20';

var URL_LOBBY = '//m.w88uat.com/ClubBravado';
var URL_INDEX = '//m.w88uat.com/Index';

var ORIENTATION_CHANGED = false;

var XML_MESSAGES = null;

$.get('/_Secure/AjaxHandlers/XmlMessagesGet.ashx', function (data) { XML_MESSAGES = data; });
window.addEventListener("orientationchange", function () { ORIENTATION_CHANGED = true; });
$(window).resize(function () { Canvas_Resize(); window.scrollTo(0, 0); });
$(document).bind('touchmove', function (e) { e.preventDefault(); });

/* #region Controls */
function Orientation_Update(e) {
    CVSMAIN = $('.cvs-main')[0];
    CTXMAIN = CVSMAIN.getContext('2d');

    var height = window.innerHeight;
    var ratio = CVSMAIN.width / CVSMAIN.height;

    SCREEN_WIDTH = window.innerWidth > 1136 ? 1136 - (104 * 2) : window.innerWidth - (0.12 * window.innerWidth);
    SCREEN_HEIGHT = window.innerHeight > 640 ? 640 - 18 - 192 : window.innerHeight - 18 - (0.12 * window.innerHeight);

    CVSMAIN.width = SCREEN_WIDTH;
    CVSMAIN.height = SCREEN_HEIGHT;

    switch (window.orientation) {
        case 0: //portrait
            // Do your thing
            $('.div-main').addClass('rotate-landscape');
            break;

        case -90: //landscape clockwise
            // Do your thing
            window.scrollTo(0, 0);
            break;

        case 90: //landscape counter clockwise
            // Do your thing
            window.scrollTo(0, 0);
            break;

        default:
            $('.div-main').addClass('rotate-landscape');
            break;
    }

    SLOT_IMG_WIDTH = (SCREEN_WIDTH - (SLOT_X_OFFSET * 4)) / 5;
    SLOT_IMG_HEIGHT = (SCREEN_HEIGHT - (SLOT_Y_OFFSET * 2)) / 3;

    var OverlayBottomWidth = window.innerWidth > MAX_SCREEN_WIDTH ? MAX_SCREEN_WIDTH : window.innerWidth;
    var OverlayBottomHeight = window.innerHeight > MAX_SCREEN_HEIGHT ? 114 : window.innerHeight - SCREEN_HEIGHT;
    var OverlayBottomTop = $('.cvs-main').offset().top + SCREEN_HEIGHT;
    var OverlayBottomLeft = window.innerWidth > MAX_SCREEN_WIDTH ? $('.cvs-main').offset().left - ((MAX_SCREEN_WIDTH - SCREEN_WIDTH) / 2) : 0;
    $('.div-overlay-bottom').css({ width: OverlayBottomWidth + 'px', height: OverlayBottomHeight + 'px', top: OverlayBottomTop + 'px', left: OverlayBottomLeft + 'px' });

    var OverlayLeftRight = $('.cvs-main').offset().left + SCREEN_WIDTH;
    var OverlayLeftWidth = window.innerWidth > MAX_SCREEN_WIDTH ? ($('.cvs-main').offset().left - OverlayBottomLeft) : $('.cvs-main').offset().left;
    var OverlayLeftHeight = window.innerHeight > MAX_SCREEN_HEIGHT ? $('.cvs-main').offset().top + SCREEN_HEIGHT + OverlayBottomHeight : window.innerHeight;
    $('.div-overlay-left').css({ right: OverlayLeftRight + 'px', height: OverlayLeftHeight + 'px', width: OverlayLeftWidth + 'px' });

    var OverlayRightWidth = window.innerWidth > MAX_SCREEN_WIDTH ? $('.cvs-main').offset().left - OverlayBottomLeft + 'px' : '100%';
    var OverlayRightHeight = window.innerHeight > MAX_SCREEN_HEIGHT ? $('.cvs-main').offset().top + SCREEN_HEIGHT + OverlayBottomHeight : window.innerHeight;
    $('.div-overlay-right').css({ left: ($('.cvs-main').offset().left + SCREEN_WIDTH) + 'px', height: OverlayRightHeight + 'px', width: OverlayLeftWidth });

    $('.div-overlay-top').css({ left: OverlayBottomLeft + 'px', width: OverlayBottomWidth + 'px' })

    var ButtonSpinWidth = window.innerWidth > MAX_SCREEN_WIDTH ? SLOT_IMG_WIDTH : SLOT_IMG_WIDTH * 0.8;
    var ButtonSpinLeft = window.innerWidth > MAX_SCREEN_WIDTH ? ($('.cvs-main').offset().left + SCREEN_WIDTH) - ButtonSpinWidth + ($('.cvs-main').offset().left - OverlayBottomLeft) + 'px' : window.innerWidth - ButtonSpinWidth + 'px';
    var ButtonSpinTop = OverlayBottomTop - ButtonSpinWidth + 'px';
    $('.div-button-spin').css({ width: ButtonSpinWidth + 'px', left: ButtonSpinLeft, top: ButtonSpinTop });

    var ButtonHomeWidth = window.innerWidth > MAX_SCREEN_WIDTH ? 80 : (SLOT_IMG_WIDTH * 0.5 > 80 ? 80 : SLOT_IMG_WIDTH * 0.5);
    var ButtonHomeLeft = window.innerWidth > MAX_SCREEN_WIDTH ? $('.div-overlay-left').offset().left : 0;
    $('.div-button-home').css({ left: ButtonHomeLeft + 'px', width: ButtonHomeWidth + 'px' });

    var ButtonLobbyWidth = window.innerWidth > MAX_SCREEN_WIDTH ? 80 : (SLOT_IMG_WIDTH * 0.5 > 80 ? 80 : SLOT_IMG_WIDTH * 0.5);
    //var ButtonLobbyLeft = $('.div-button-spin').offset().left + $('.div-button-spin').width() - ButtonLobbyWidth;
    var ButtonLobbyLeft = window.innerWidth > MAX_SCREEN_WIDTH ? ($('.cvs-main').offset().left + SCREEN_WIDTH) - ButtonLobbyWidth + ($('.cvs-main').offset().left - OverlayBottomLeft) : window.innerWidth - ButtonLobbyWidth;

    $('.div-button-lobby').css({ left: ButtonLobbyLeft + 'px', width: ButtonLobbyWidth + 'px' });

    var GameLogoHeight = window.innerHeight > MAX_SCREEN_HEIGHT ? OverlayBottomHeight : window.innerHeight - OverlayBottomTop;
    var GameLogoLeft = window.innerWidth > MAX_SCREEN_WIDTH ? $('.div-overlay-left').offset().left + $('.div-overlay-left').width() / 10 : $('.cvs-main').offset().left / 4;
    var GameLogoTop = window.innerHeight > MAX_SCREEN_HEIGHT ? OverlayBottomTop + 5 : OverlayBottomTop;
    $('.div-game-logo').css({ top: GameLogoTop + 'px', left: GameLogoLeft + 'px', height: GameLogoHeight + 'px', width: '100%' });

    var LabelBackgroundDefaultSize = '65%';

    var LabelTotalBetsSize = window.innerHeight > MAX_SCREEN_HEIGHT ? '' : LabelBackgroundDefaultSize;
    var LabelWinningsSize = window.innerHeight > MAX_SCREEN_HEIGHT ? '' : LabelBackgroundDefaultSize;
    var LabelCreditsSize = window.innerHeight > MAX_SCREEN_HEIGHT ? '' : LabelBackgroundDefaultSize;

    var LabelCreditsLeft = $('.cvs-main').offset().left + SLOT_IMG_WIDTH * 0.5;
    var LabelCreditsTop = window.innerHeight > 480 ? OverlayBottomTop + GameLogoHeight / 3.5 : OverlayBottomTop;
    $('.div-game-credits-label').css({ top: LabelCreditsTop + 'px', left: LabelCreditsLeft + 'px', height: GameLogoHeight + 'px', backgroundSize: LabelTotalBetsSize });
    $('.div-game-credits').css({ top: LabelCreditsTop + 'px', left: LabelCreditsLeft + SLOT_IMG_WIDTH + 'px', height: GameLogoHeight + 'px', lineHeight: GameLogoHeight + 'px' });


    var LabelTotalBetsTop = window.innerHeight > 480 ? OverlayBottomTop - GameLogoHeight / 5.5 : OverlayBottomTop;
    var LabelTotalBetsLeft = window.innerHeight > 480 ? $('.cvs-main').offset().left + SLOT_IMG_WIDTH * 3 : $('.cvs-main').offset().left + SLOT_IMG_WIDTH * 2.8;
    $('.div-game-totalbets-label').css({ top: LabelTotalBetsTop + 'px', left: LabelTotalBetsLeft + 'px', height: GameLogoHeight + 'px', backgroundSize: LabelTotalBetsSize });
    $('.div-game-totalbets').css({ top: LabelTotalBetsTop + 'px', left: LabelTotalBetsLeft + SLOT_IMG_WIDTH / 2 + 'px', height: GameLogoHeight + 'px', lineHeight: GameLogoHeight + 'px' });

    var LabelWinningsTop = window.innerHeight > 480 ? OverlayBottomTop + GameLogoHeight / 3.5 : OverlayBottomTop;
    var LabelWinningsLeft = window.innerHeight > 480 ? $('.cvs-main').offset().left + SLOT_IMG_WIDTH * 3 : $('.cvs-main').offset().left + SLOT_IMG_WIDTH * 4.2;
    $('.div-game-winnings-label').css({ top: LabelWinningsTop + 'px', left: LabelWinningsLeft + 'px', height: GameLogoHeight + 'px', backgroundSize: LabelTotalBetsSize });
    $('.div-game-winnings').css({ top: LabelWinningsTop + 'px', left: LabelWinningsLeft + SLOT_IMG_WIDTH / 2 + 'px', height: GameLogoHeight + 'px', lineHeight: GameLogoHeight + 'px' });

    if (ORIENTATION_CHANGED) { Reels_Draw(); ORIENTATION_CHANGED = false; }
}
function Spin_Start() {
    console.log('spin_start');
    toggleFullScreen();
    HIGHLIGHT = false;
    HIGHLIGHT_OBJ = null;
    BONUSWIN = 0;
    
    $('.div-game-winnings').text(parseFloat('0').toFixed(2));
    $('.div-button-spin').unbind('touch click');

    $.ajax({
        url: "/_Secure/AjaxHandlers/WheelSpin.ashx",
        dataType: "xml",
        type: "POST"
    })
    .done(function (data) {
        if ($(data).find("error").length < 1) {
            SLOTS_RESULTS_ASSETS = [];

            $(data).find("wheels").children().each(function (uid) {
                SPIN_COLUMNS[uid].StopSpeed = SLOT_SPEED;
                SPIN_COLUMNS[uid].lastUpdated = new Date();
                SPIN_COLUMNS[uid].StartSpeed = SLOT_SPEED - SLOT_SPEED;
                SLOTS_RESULTS_ASSETS.push([]);

                var col_results = $(this).text().split(',');

                for (var row = 0; row < col_results.length; row++) {
                    var item = new Object();
                    var index = col_results[row] - 1;
                    item.img = SLOTS_ITEMS_ASSETS[index].img;
                    item.result = true;
                    item.position = col_results.length - row - 1;
                    item.id = SLOTS_ITEMS_ASSETS[index].id;
                    SLOTS_RESULTS_ASSETS[uid].push(item);
                }
            });
            var wins = 0;
            if ($(data).find("winposition").children().length > 0) {
                HIGHLIGHT = true;
                HIGHLIGHT_OBJ = [];

                $(data).find("winposition").children().each(function (uid) {
                    wins += parseFloat($(this).attr("win"));
                    var item = new Object();
                    item.lines = $(this).attr("line");
                    item.array = $(this).text().split(',');

                    HIGHLIGHT_OBJ.push(item);
                });
            } else { HIGHLIGHT = false; }

            if ($(data).find("bonus").text().length != 0) {
                if (parseInt($(data).find("bonus").attr("id")) == 2) {
                    BKEY = $(data).find("bonus").text();
                    LOADBONUS = true;
                    BONUSTYPE = 'fs';
                } else if (parseInt($(data).find("bonus").attr("id")) == 3) {
                    BKEY = $(data).find("bonus").text();
                    LOADBONUS = true;
                    BONUSTYPE = 'sw';
                } else if (parseInt($(data).find("bonus").attr("id")) == 4) {
                    BKEY = $(data).find("bonus").text();
                    LOADBONUS = true;
                    BONUSTYPE = 'tmu';
                }
            }

            NEW_BALANCE_XML = $.parseXML('<BALANCE CREDIT="' + $(data).find("balance").attr('credit') + '" COIN="' + $(data).find("balance").text() + '" CUR="' + MEMBER_CURR + '" CONV="' + $(data).find("balance").attr("conv") + '" WINS="' + wins + '"/>');
        }
    })
    .fail(function () { })
    .always(function (data) {
        if ($(data).find("error").length > 0) {
            var error_code = $(data).find("error").attr("code");
            switch (error_code) {
                case "10":
                    alert($(XML_MESSAGES).find('Error10').text());
                    $('.div-button-spin').unbind('touch click').hide();
                    break;

                default:
                    alert($(XML_MESSAGES).find('SessionExpired').text());
                    $('.div-button-spin').unbind('touch click').hide();
                    break;
            }
            return;
        }
        else {
            if ($(data).find("spin").length > 0) {
                $('.div-game-credits').text(($('.div-game-credits').text() - (PLAY_AMOUNT * PLAY_BETLINES * PLAY_MULIPLY)).toFixed(2));
                SPIN_COLUMNS[0].state = 1;
                SPIN_STARTED = true;
                SPIN_STOPPED = false;
                Reels_Spin();
                ButtonSwitch(true);
                $('.div-button-spin').bind('touch click', function (e) { Spin_Stop(); });
                window.clearTimeout(STOPTIMER);
            }
            else {
                alert($(XML_MESSAGES).find('SessionExpired').text());
                $('.div-button-spin').unbind('touch click').hide();
            }
        }
    });
}
function Spin_Stop() {
    console.log('spin_stop');
    $('.div-button-spin').unbind('touch click');
    SPIN_COLUMNS[0].state = 2;
    SPIN_COLUMNS[0].lastUpdated = new Date();
    SPIN_STARTED = false;
    SPIN_STOPPED = true;
    Reels_Stop();
    window.clearTimeout(STARTTIMER);
}
function ButtonSwitch(bool)
{
    if (bool) {
        $('.div-button-spin > img').attr('src', $('.div-button-spin > img').attr('src').replace('spin.png', 'stop.png'));
    }
    else {
        $('.div-button-spin > img').attr('src', $('.div-button-spin > img').attr('src').replace('stop.png', 'spin.png'));
    }
}
/* #endregion */

/* #region Reels */
function Reels_Draw() {
    var drawX, drawY;

    REELS_START_X += SLOT_X_OFFSET;
    REELS_START_Y = CVSMAIN.height - REELS_START_Y;

    for (var col = 0; col < SLOT_COLUMNS; col++) {
        SLOTS_ARRANGED_ASSETS.push([]);
        SPIN_COLUMNS.push(new Object());
        SPIN_COLUMNS[col].state = 0;
        y = REELS_START_Y - SLOT_IMG_HEIGHT - SLOT_Y_OFFSET; // starting point

        drawX = col == 0 ? 0 : (SLOT_IMG_WIDTH + SLOT_X_OFFSET) * col;
        var column_assets = _GPIntMobileSlots.copyArray(SLOTS_ITEMS_ASSETS);
        _GPIntMobileSlots.shuffleArray(column_assets);

        for (var row = 0; row < SLOTS_ITEMS_ARRAY.length; row++) {
            drawY = y = row == 0 ? CVSMAIN.height - SLOT_IMG_HEIGHT : y;
            var col_obj = column_assets[row];
            var item = new Object();
            item.img = col_obj.img;
            item.id = col_obj.id;
            item.y = drawY;
            if (item.result == null) { item.result = false; }
            if (row == 0) { item.state == 0; }
            if (row < 4) { CTXMAIN.drawImage(item.img, drawX, drawY, SLOT_IMG_WIDTH, SLOT_IMG_HEIGHT); }
            SLOTS_ARRANGED_ASSETS[col].push(item);
            y -= SLOT_IMG_HEIGHT + SLOT_Y_OFFSET;
        }
    }
}
function Reels_Spin() {
    var drawX, drawY;
    if (SPIN_STARTED) {
        Canvas_Clear();
        for (var col = 0; col < SLOT_COLUMNS; col++) {
            drawX = col == 0 ? 0 : ((SLOT_IMG_WIDTH + SLOT_X_OFFSET) * col);

            for (var row = 0; row < 6; row++) {
                var item = SLOTS_ARRANGED_ASSETS[col][row];
                if (SPIN_COLUMNS[col].state == 1) { item.y += SPIN_COLUMNS[col].StartSpeed; }
                drawY = item.y;
                CTXMAIN.drawImage(item.img, drawX, drawY, SLOT_IMG_WIDTH, SLOT_IMG_HEIGHT);

                if (Math.floor(drawY) >= Math.floor(SCREEN_HEIGHT - SLOT_IMG_HEIGHT)) { SLOTS_ARRANGED_ASSETS[col][5].y = SLOTS_ARRANGED_ASSETS[col][4].y - SLOT_IMG_HEIGHT - SLOT_Y_OFFSET; }
                if (Math.floor(drawY) > Math.floor(SCREEN_HEIGHT + SLOT_IMG_HEIGHT)) { var item = SLOTS_ARRANGED_ASSETS[col].shift(); if (!item.result) { SLOTS_ARRANGED_ASSETS[col].push(item); } }
            }
            if (Math.floor((new Date().getTime() - SPIN_COLUMNS[col].lastUpdated) / 50) >= col && SPIN_STARTED) { SPIN_COLUMNS[col].state = 1; }
            if (SPIN_COLUMNS[col].StartSpeed < SLOT_SPEED && SPIN_COLUMNS[col].state == 1) { SPIN_COLUMNS[col].StartSpeed += 1; }
        }
        RequestAnimFrame(function () { Reels_Spin(); });
        if (Math.floor((new Date().getTime() - SPIN_COLUMNS[0].lastUpdated) / 1000) > 1 && SPIN_STARTED && !SPIN_STOPPED) { Spin_Stop(); return; }
        Balance_Draw(false);
    }
}
function Reels_Stop() {
    var drawX, drawY;
    if (SPIN_STOPPED) {
        Canvas_Clear();
        for (var col = 0; col < SLOT_COLUMNS; col++) {
            drawX = col == 0 ? 0 : ((SLOT_IMG_WIDTH + SLOT_X_OFFSET) * col);

            if (SPIN_COLUMNS[col].state == 1) {
                for (var row = 0; row < 6; row++) {
                    var item = SLOTS_ARRANGED_ASSETS[col][row];
                    drawY = item.y += SLOT_SPEED;

                    if (col > 0 && SPIN_COLUMNS[col - 1].state > 2) {
                        if (Math.floor((new Date().getTime() - SPIN_COLUMNS[col - 1].lastUpdated) / 500) > 0)
                        { SPIN_COLUMNS[col].state = 2; SPIN_COLUMNS[col].lastUpdated = new Date(); }
                    }
                    CTXMAIN.drawImage(item.img, drawX, drawY, SLOT_IMG_WIDTH, SLOT_IMG_HEIGHT);

                    if (Math.floor(drawY) >= Math.floor(SCREEN_HEIGHT - SLOT_IMG_HEIGHT)) { SLOTS_ARRANGED_ASSETS[col][5].y = SLOTS_ARRANGED_ASSETS[col][4].y - SLOT_IMG_HEIGHT - SLOT_Y_OFFSET; }
                    if (Math.floor(drawY) > Math.floor(SCREEN_HEIGHT + SLOT_IMG_HEIGHT)) { var item = SLOTS_ARRANGED_ASSETS[col].shift(); SLOTS_ARRANGED_ASSETS[col].push(item); }
                }
            }
            else if (SPIN_COLUMNS[col].state == 2) {
                for (var row = 0; row < SLOTS_RESULTS_ASSETS[col].length; row++) {
                    var item = SLOTS_RESULTS_ASSETS[col][row];
                    var item_position = item.position;

                    drawY = item.y = item_position == 0 ? CVSMAIN.height - SLOT_IMG_HEIGHT : (SLOT_IMG_HEIGHT + SLOT_Y_OFFSET) * (2 - item_position);
                    CTXMAIN.drawImage(item.img, drawX, drawY, SLOT_IMG_WIDTH, SLOT_IMG_HEIGHT);
                    SLOTS_ARRANGED_ASSETS[col].unshift(item);
                }

                SPIN_COLUMNS[col].state = 3;
            }
            else if (SPIN_COLUMNS[col].state == 3) {
                for (var row = 0; row < SLOTS_ARRANGED_ASSETS[col].length; row++) {
                    var item = SLOTS_ARRANGED_ASSETS[col][row];
                    if (row > 2) { item.y = SLOTS_ARRANGED_ASSETS[col][row - 1].y - (SLOT_IMG_HEIGHT + SLOT_Y_OFFSET); }

                    drawY = item.y;
                    CTXMAIN.drawImage(item.img, drawX, drawY, SLOT_IMG_WIDTH, SLOT_IMG_HEIGHT);
                }

                SPIN_COLUMNS[col].state = 4;
            }
            else if (SPIN_COLUMNS[col].state == 4) {
                for (var row = 0; row < 6; row++) {
                    var item = SLOTS_ARRANGED_ASSETS[col][row];
                    drawY = item.y = row == 0 ? SCREEN_HEIGHT - SLOT_IMG_HEIGHT : (SCREEN_HEIGHT - SLOT_IMG_HEIGHT) - ((SLOT_IMG_HEIGHT + SLOT_Y_OFFSET) * row);
                    CTXMAIN.drawImage(item.img, drawX, drawY, SLOT_IMG_WIDTH, SLOT_IMG_HEIGHT);
                }
            }

            if (SPIN_COLUMNS[SLOT_COLUMNS - 1].state == 4) {
                Reels_Redraw();
                window.clearTimeout(STOPTIMER);
                Balance_Draw(true);
                Reels_Highlight();
                ButtonSwitch(false);
                
                if (LOADBONUS) {
                    $('.div-button-spin').hide(); SPIN_PAUSED = true;
                    if (!HIGHLIGHT) { window.setTimeout(function () { Bonus_Load(); }, 1000); }
                }
                else { $('.div-button-spin').bind('touch click', function (e) { Spin_Start(); }); }

                /*
                if (AUTO_SPIN && AUTO_SPIN_CURR < AUTO_SPIN_MAX - 1 && !SPIN_PAUSED) {
                    AUTO_SPIN_CURR++;
                    Spin_Start();
                    return;
                }
                else {
                    //if (!LOADBONUS) { $('.div-main > div:not(:last-child) ').show(); }
                    //$('.div-button-auto > div > span').hide();
                    //AUTO_SPIN = false;
                    if (!HIGHLIGHT && LOADBONUS) { SPIN_PAUSED = true; window.setTimeout(function () { Bonus_Load(); console.log('load bonus highlight && loadbonus'); }, 1000); }
                    return;
                }
                */
            }
        }
        StopAnimFrame(function () { if (SPIN_COLUMNS[SLOT_COLUMNS - 1].state != 4) { Reels_Stop(); } });
    }
}
function Reels_Redraw() {
    var drawX, drawY;
    Canvas_Clear();
    for (var col = 0; col < SLOT_COLUMNS; col++) {
        drawX = col == 0 ? 0 : ((SLOT_IMG_WIDTH + SLOT_X_OFFSET) * col);
        if (SPIN_COLUMNS[col]) {
            if (SPIN_COLUMNS[col].state == 0 || SPIN_COLUMNS[col].state == 4) {
                for (var row = 0; row < 6; row++) {
                    var item = SLOTS_ARRANGED_ASSETS[col][row];
                    drawY = item.y = row == 0 ? SCREEN_HEIGHT - SLOT_IMG_HEIGHT : (SCREEN_HEIGHT - SLOT_IMG_HEIGHT) - ((SLOT_IMG_HEIGHT + SLOT_Y_OFFSET) * row);
                    CTXMAIN.drawImage(item.img, drawX, drawY, SLOT_IMG_WIDTH, SLOT_IMG_HEIGHT);
                }
            }
            else {
                for (var row = 0; row < 6; row++) {
                    var item = SLOTS_ARRANGED_ASSETS[col][row];
                    drawY = item.y = row == 0 ? SCREEN_HEIGHT - SLOT_IMG_HEIGHT : (SCREEN_HEIGHT - SLOT_IMG_HEIGHT) - ((SLOT_IMG_HEIGHT + SLOT_Y_OFFSET) * row);
                    CTXMAIN.drawImage(item.img, drawX, drawY, SLOT_IMG_WIDTH, SLOT_IMG_HEIGHT);
                }
            }
        }
    }
}
/* #endregion */

/* #region Canvas */
function Canvas_Init(real, game) {
    /* uncomment this chunk */
    GAMENAME = game;

    $.ajax({
        url: "/_Secure/AjaxHandlers/KeyBalanceGet.ashx",
        dataType: "xml",
        type: "POST",
        data: { type: real }
    })
    .done(function (data) {
        if ($(data).find("STATUSCODE").text() == "1") {
            alert($(XML_MESSAGES).find('SessionInvalid').text());
            window.location.assign("/" + GAMENAME + "/fun.html");
            return;
        }

        if ($(data).find("REALMODE").text() == "1") {
            MEMBERCODE = $(data).find("NAME").text();
        }

        BALANCE_XML = data;
        _GPIntMobileSlots.preloadImages('reels', GAMENAME, '/_Static/Images/Slots/{GAMENAME}/Reels', SLOTS_ITEMS_ARRAY, null, function () {
            _GPIntMobileSlots.preloadImages('background', GAMENAME, '/_Static/Images/Slots/{GAMENAME}/Background', SLOTS_BG_ARRAY, null, function () {
                Orientation_Update();
                Reels_Draw();
                Balance_Draw(false);
                $('.div-content-wrapper').parent().find('div.loader').remove();
                $('.div-button-spin').bind('touch click', function (e) { Spin_Start(); });
                $('.div-button-home').bind('touch click', function (e) { window.location.assign(URL_INDEX); });
                $('.div-button-lobby').bind('touch click', function (e) { window.location.assign(URL_LOBBY); });
            });
        });       
    })
    .fail(function () { })
    .always(function () { });
}
function Canvas_Resize() {
    Orientation_Update();
    Canvas_Clear();
    var id = 0;
    if (BONUSLOADED && BONUSTYPE == 'fs') { id = 1; }
    Reels_Redraw();
    if (BONUSLOADED && BONUSTYPE == 'fs') { BonusFSGirl_Draw(BSTEPS); }
    if (HIGHLIGHT) { Reels_Highlight(); }

    if ($('.div-bonus'))
    {
        $('.div-bonus').css({ left: $('.cvs-main').offset().left + 'px', width: $('.cvs-main').width() + 'px', height: $('.cvs-main').height() + 'px', lineHeight: $('.cvs-main').height() + 'px' });
    }
}
function Canvas_Clear() {
    CTXMAIN.clearRect(0, 0, CVSMAIN.width, CVSMAIN.height);
    CTXMAIN.restore();
}
/* #endregion */

/* #region Bonus */
function Bonus_Bind(bonus_type) {
    if (bonus_type == 'tanmeup') {
        $('div[class^="div-tmu-"]').each(function () {
            $(this).bind('touch click', function (e) {
                $('div[class^="div-tmu-"]').unbind('touch click');
                var that = $(this);
                $.ajax({
                    url: "/_Secure/AjaxHandlers/BonusPost.ashx",
                    data: {
                        choice: $(this).attr('rel'),
                        id: 4,
                        key: BKEY,
                        step: BSTEPS
                    },
                    dataType: "xml",
                    type: "POST"
                })
                .done(function (data) {                    
                    var winning_amount = $(data).find("wheels > item[id='" + BSTEPS + "']").text();
                    var current_credit = $(data).find("balance").attr("credit");
                    $(that).append($('<span />', { 'class': 'span-bonus-win' }).text(parseFloat(winning_amount).toFixed(2)));
                    $($(that).children()[0]).css({ height: $(that).height() + 'px', lineHeight: $(that).height() + 'px' });
                    BSTEPS++;
                    $('.div-game-winnings').text((parseFloat($('.div-game-winnings').text()) + parseFloat(winning_amount)).toFixed(2));
                    $('.div-game-credits').text(current_credit);                    
                })
                .always(function (data) {
                    if ($(data).find('error').length > 0) {
                        var error_code = $(data).find('error').attr('code');
                        if (error_code == '7') { $(XML_MESSAGES).find('Error7').text() }
                    }
                    else if ($(data).find('bonus').length > 0) {
                        if (BSTEPS < 4) { Bonus_Bind(bonus_type); } else {
                            BKEY = null;
                            LOADBONUS = false;
                            BONUSLOADED = false;
                            BSTEPS = 0;
                            window.setTimeout(function () {
                                $('.div-button-spin').show();
                                $('.div-button-spin').bind('touch click', function (e) { Spin_Start(); });
                                $('.div-bonus > div').remove();
                            }, 2000);
                        }
                    }
                });
            });
        });
    }
    else if (bonus_type == 'swimsuit') {
        $('div[class^="div-sw-"]').each(function () {
            $(this).bind('touch click', function (e) {
                $('div[class^="div-sw-"]').unbind('touch click');
                var that = $(this);
                $.ajax({
                    url: "/_Secure/AjaxHandlers/BonusPost.ashx",
                    data: {
                        choice: $(this).attr('rel'),
                        id: 3,
                        key: BKEY,
                        step: BSTEPS
                    },
                    dataType: "xml",
                    type: "POST"
                })
                .done(function (data) {                    
                    var winning_amount = $(data).find("wheels > item[id='" + BSTEPS + "']").text();
                    var current_credit = $(data).find("balance").attr("credit");
                    $(that).append($('<span />', { 'class': 'span-bonus-win' }).text(parseFloat(winning_amount).toFixed(2)));
                    $($(that).children()[0]).css({ height: $(that).height() + 'px', lineHeight: $(that).height() + 'px' });
                    $('.div-game-winnings').text((parseFloat($('.div-game-winnings').text()) + parseFloat(winning_amount)).toFixed(2));
                    $('.div-game-credits').text(current_credit);
                })
                .always(function (data) {
                    console.log(data);
                    console.log($(data).find('error').length);
                    if ($(data).find('error').length > 0) {
                        var error_code = $(data).find('error').attr('code');
                        if (error_code == '7') { $(XML_MESSAGES).find('Error7').text() }
                    }
                    else if ($(data).find('bonus').length > 0) {
                        BKEY = null;
                        LOADBONUS = false;
                        BONUSLOADED = false;
                        BSTEPS = 0;
                        window.setTimeout(function () {
                            $('.div-button-spin').show();
                            $('.div-button-spin').bind('touch click', function (e) { Spin_Start(); });
                            $('.div-bonus > div').remove();
                        }, 2000);
                    }
                });
            });
        });
    }
}
function Bonus_Load() {
    if (BONUSLOADED == true) { return; }
    BSTEPS = 0;
    BONUSLOADED = true;
    if (BKEY != null && BKEY.length > 0) {
        if (BONUSTYPE == 'tmu') {
            $('.div-bonus').append($('<div />', { 'class' : 'div-bonus-row-one' })).append($('<div />', { 'class' : 'div-bonus-row-two' }));
            $.each(SLOTS_BONUS_TMU_ARRAY, function (index, value) {
                if (index < 3) { $('.div-bonus-row-one').append($('<div/>', { rel: value.id, 'class' : 'div-tmu-' + value.id })); }
                else { $('.div-bonus-row-two').append($('<div/>', { rel: value.id, 'class' : 'div-tmu-' + value.id })); }
            });
            $.ajax({
                url: "/_Secure/AjaxHandlers/BonusPost.ashx",
                data: {
                    choice: 0,
                    id: 4,
                    key: BKEY,
                    step: BSTEPS
                },
                dataType: "xml",
                type: "POST"
            }).done(function () { BSTEPS++; Bonus_Bind('tanmeup'); });
            
        }
        else if (BONUSTYPE == 'sw') {
            $('.div-bonus').append($('<div />', { 'class' : 'div-bonus-row-one' })).append($('<div />', { 'class' : 'div-bonus-row-two' }));
            $.each(SLOTS_BONUS_SW_ARRAY, function (index, value) {
                if (index < 3) { $('.div-bonus-row-one').append($('<div/>', { rel: value.id, 'class' : 'div-sw-' + value.id })); }
                else {
                    if (index == 3) { $('.div-bonus-row-two').append($('<div/>', { rel: value.id, 'class' : 'div-sw-' + value.id })); }
                    else { $('.div-bonus-row-two').append($('<div/>', { rel: value.id, 'class' : 'div-sw-' + value.id })); }
                }
            });
            $.ajax({
                url: "/_Secure/AjaxHandlers/BonusPost.ashx",
                data: {
                    choice: 0,
                    id: 3,
                    key: BKEY,
                    step: BSTEPS
                },
                dataType: "xml",
                type: "POST"
            }).done(function () { BSTEPS++; Bonus_Bind('swimsuit'); });
        }
        else if (BONUSTYPE == 'fs') {
            BSTEPS++;
            $.ajax({
                url: "/_Secure/AjaxHandlers/BonusPost.ashx",
                data: {
                    choice: 0,
                    id: 2,
                    key: BKEY,
                    step: BSTEPS
                },
                dataType: "xml",
                type: "POST"
            }).done(function (data) { BONUS_FREESPIN_REMAINING = parseInt($(data).find("bonus").attr("counter")); BONUSWIN = $(data).find("data").attr("summ"); BONUSWIN_SINGLE = parseInt($(data).find("win").text()); BonusFS_Init(); BonusReel_Spin(); });
        }
        $('.div-bonus').css({ left: $('.cvs-main').offset().left + 'px', top: $('.cvs-main').offset().top + 'px', width: SCREEN_WIDTH + 'px', height: SCREEN_HEIGHT + 'px', lineHeight: SCREEN_HEIGHT + 'px' });
    }
}
/* #region FreeSpin */
function BonusFS_Init() {
    SLOTS_RESULTS_ASSETS = [];
    for (var uid = 0; uid < 5; uid++) {
        SPIN_COLUMNS[uid].StopSpeed = SLOT_SPEED;
        SPIN_COLUMNS[uid].lastUpdated = new Date();
        SPIN_COLUMNS[uid].StartSpeed = SLOT_SPEED - SLOT_SPEED;
        SLOTS_RESULTS_ASSETS.push([]);
        for (var row = 0; row < 4; row++) {
            var item = new Object();
            var rand = Math.floor(Math.random() * SLOTS_ITEMS_ASSETS.length - 1) + 1;
            item.img = SLOTS_ITEMS_ASSETS[rand].img;
            item.result = true;
            item.position = 4 - row - 1;
            item.id = SLOTS_ITEMS_ASSETS[rand].id;
            SLOTS_RESULTS_ASSETS[uid].push(item);
        }
    }

    SPIN_COLUMNS[0].state = 1;
    SPIN_STARTED = true;
    SPIN_STOPPED = false;
    window.clearTimeout(BONUSSTOPTIMER);
}
function BonusFSGirl_Draw(index) {
    //index--;
    var ImageWidth = SLOT_IMG_WIDTH * 1.25;
    var ImageHeight = SLOT_IMG_HEIGHT * 3;

    if (index > 0 && index < 5) {
        if (index == null) { index = 1; }
        if (index == 4) { index = 1; }
        var drawX = (SLOT_IMG_WIDTH + SLOT_X_OFFSET) * index - (ImageWidth * 0.07);
        var drawY = 47;

        if (FSIMG == null) {
            FSIMG = new Image();
            FSIMG.src = '/_Static/Images/Slots/' + GAMENAME + '/fs01.png';
            FSIMG.onload = function () { CTXMAIN.drawImage(FSIMG, drawX, 0, ImageWidth, ImageHeight); }
        }
        else { CTXMAIN.drawImage(FSIMG, drawX, 0, ImageWidth, ImageHeight); }
    }
}
function BonusReel_Spin() {
    var drawX, drawY;
    if (SPIN_STARTED) {
        Canvas_Clear();
        for (var col = 0; col < SLOT_COLUMNS; col++) {
            drawX = col == 0 ? 0 : ((SLOT_IMG_WIDTH + SLOT_X_OFFSET) * col);

            for (var row = 0; row < 6; row++) {
                var item = SLOTS_ARRANGED_ASSETS[col][row];
                if (SPIN_COLUMNS[col].state == 1) { item.y += SPIN_COLUMNS[col].StartSpeed; }
                drawY = item.y;
                if (BSTEPS == 4 && col == 1) { }
                else if (BSTEPS < 4 && BSTEPS == col) { }
                else { CTXMAIN.drawImage(item.img, drawX, drawY, SLOT_IMG_WIDTH, SLOT_IMG_HEIGHT); }
                BonusFSGirl_Draw(BSTEPS);
                if (Math.floor(drawY) >= Math.floor(SCREEN_HEIGHT - SLOT_IMG_HEIGHT)) { SLOTS_ARRANGED_ASSETS[col][5].y = SLOTS_ARRANGED_ASSETS[col][4].y - SLOT_IMG_HEIGHT - SLOT_Y_OFFSET; }
                if (Math.floor(drawY) > Math.floor(SCREEN_HEIGHT + SLOT_IMG_HEIGHT)) { var item = SLOTS_ARRANGED_ASSETS[col].shift(); if (!item.result) { SLOTS_ARRANGED_ASSETS[col].push(item); } }
            }
            if (Math.floor((new Date().getTime() - SPIN_COLUMNS[col].lastUpdated) / 50) >= col && SPIN_STARTED) { SPIN_COLUMNS[col].state = 1; }
            if (SPIN_COLUMNS[col].StartSpeed < SLOT_SPEED && SPIN_COLUMNS[col].state == 1) { SPIN_COLUMNS[col].StartSpeed += 1; }
        }
        RequestBonusAnimFrame(function () { BonusReel_Spin(); });
        if (Math.floor((new Date().getTime() - SPIN_COLUMNS[0].lastUpdated) / 1000) > 1 && SPIN_STARTED) {
            SPIN_COLUMNS[0].state = 2;
            SPIN_COLUMNS[0].lastUpdated = new Date();
            SPIN_STARTED = false;
            SPIN_STOPPED = true;
            BonusReel_Stop();
            window.clearTimeout(BONUSSTARTTIMER);
            return;
        }
    }
}
function BonusReel_Stop() {
    var drawX, drawY;
    if (SPIN_STOPPED) {
        Canvas_Clear();
        for (var col = 0; col < SLOT_COLUMNS; col++) {
            drawX = col == 0 ? 0 : ((SLOT_IMG_WIDTH + SLOT_X_OFFSET) * col);
            if (SPIN_COLUMNS[col].state == 1) {
                for (var row = 0; row < 6; row++) {
                    var item = SLOTS_ARRANGED_ASSETS[col][row];
                    drawY = item.y += SLOT_SPEED;

                    if (col > 0 && SPIN_COLUMNS[col - 1].state > 2) {
                        if (Math.floor((new Date().getTime() - SPIN_COLUMNS[col - 1].lastUpdated) / 500) > 0)
                        { SPIN_COLUMNS[col].state = 2; SPIN_COLUMNS[col].lastUpdated = new Date(); }
                    }
                    if (BSTEPS == 4 && col == 1) { }
                    else if (BSTEPS < 4 && BSTEPS == col) { }
                    else { CTXMAIN.drawImage(item.img, drawX, drawY, SLOT_IMG_WIDTH, SLOT_IMG_HEIGHT); }

                    if (Math.floor(drawY) >= Math.floor(CVSMAIN.height - SLOT_IMG_HEIGHT)) { SLOTS_ARRANGED_ASSETS[col][5].y = SLOTS_ARRANGED_ASSETS[col][4].y - SLOT_IMG_HEIGHT - SLOT_Y_OFFSET; }
                    if (Math.floor(drawY) > Math.floor(CVSMAIN.height + SLOT_IMG_HEIGHT)) { var item = SLOTS_ARRANGED_ASSETS[col].shift(); SLOTS_ARRANGED_ASSETS[col].push(item); }
                }
            }
            else if (SPIN_COLUMNS[col].state == 2) {
                for (var row = 0; row < SLOTS_RESULTS_ASSETS[col].length; row++) {
                    var item = SLOTS_RESULTS_ASSETS[col][row];
                    var item_position = item.position;

                    drawY = item.y = item_position == 0 ? CVSMAIN.height - SLOT_IMG_HEIGHT : (SLOT_IMG_HEIGHT + SLOT_Y_OFFSET) * (2 - item_position);
                    if (BSTEPS == 4 && col == 1) { }
                    else if (BSTEPS < 4 && BSTEPS == col) { }
                    else { CTXMAIN.drawImage(item.img, drawX, drawY, SLOT_IMG_WIDTH, SLOT_IMG_HEIGHT); }

                    SLOTS_ARRANGED_ASSETS[col].unshift(item);
                }

                SPIN_COLUMNS[col].state = 3;
            }
            else if (SPIN_COLUMNS[col].state == 3) {
                for (var row = 0; row < SLOTS_ARRANGED_ASSETS[col].length; row++) {
                    var item = SLOTS_ARRANGED_ASSETS[col][row];
                    if (row > 2) { item.y = SLOTS_ARRANGED_ASSETS[col][row - 1].y - (SLOT_IMG_HEIGHT + SLOT_Y_OFFSET); }

                    drawY = item.y;
                    if (BSTEPS == 4 && col == 1) { }
                    else if (BSTEPS < 4 && BSTEPS == col) { }
                    else { CTXMAIN.drawImage(item.img, drawX, drawY, SLOT_IMG_WIDTH, SLOT_IMG_HEIGHT); }
                }

                SPIN_COLUMNS[col].state = 4;
            }
            else if (SPIN_COLUMNS[col].state == 4) {
                for (var row = 0; row < 6; row++) {
                    var item = SLOTS_ARRANGED_ASSETS[col][row];
                    drawY = item.y = row == 0 ? CVSMAIN.height - SLOT_IMG_HEIGHT : (CVSMAIN.height - SLOT_IMG_HEIGHT) - ((SLOT_IMG_HEIGHT + SLOT_Y_OFFSET) * row);
                    if (BSTEPS == 4 && col == 1) { }
                    else if (BSTEPS < 4 && BSTEPS == col) { }
                    else { CTXMAIN.drawImage(item.img, drawX, drawY, SLOT_IMG_WIDTH, SLOT_IMG_HEIGHT); }
                }
            }

            BonusFSGirl_Draw(BSTEPS);

            if (SPIN_COLUMNS[SLOT_COLUMNS - 1].state == 4) {
                window.clearTimeout(BONUSSTOPTIMER);
                if ($('.div-bonus > span').length > 0) { $('.div-bonus > span').text($(XML_MESSAGES).find('YouHaveWon').text() + parseFloat(BONUSWIN_SINGLE).toFixed(2)); }
                else { $('.div-bonus').append($('<span />', { 'class': 'span-bonus-win' }).text($(XML_MESSAGES).find('YouHaveWon').text() + parseFloat(BONUSWIN_SINGLE).toFixed(2))); }
                $('.div-game-winnings').text(parseFloat(BONUSWIN).toFixed(2));

                if (BONUS_FREESPIN_REMAINING > 0) {
                    BONUS_FREESPIN_REMAINING--;
                    BSTEPS++;
                    $.ajax({
                        url: "/_Secure/AjaxHandlers/BonusPost.ashx",
                        data: {
                            choice: 0,
                            id: 2,
                            key: BKEY,
                            step: BSTEPS
                        },
                        dataType: "xml",
                        type: "POST"
                    }).done(function (data) {
                        BONUSWIN = $(data).find("data").attr("summ");
                        BONUSWIN_SINGLE = $(data).find("win").length > 1 ? $($(data).find("win")[0]).text() : $(data).find("win").text();
                        window.setTimeout(function () { BonusFS_Init(); BonusReel_Spin(); }, 1000);
                        //$('.div-game-winnings').text(parseFloat(BONUSWIN).toFixed(2));
                    });
                }
                else {
                    //alert($(XML_MESSAGES).find('YouHaveWonTotal').text() + BONUSWIN);
                    $('.div-bonus > span').text($(XML_MESSAGES).find('YouHaveWon').text() + parseFloat(BONUSWIN_SINGLE).toFixed(2) + '. ' + $(XML_MESSAGES).find('ATotalOf').text() + parseFloat(BONUSWIN).toFixed(2));
                    Canvas_Clear();
                    Reels_Redraw();
                    $('.div-game-winnings').text(parseFloat(BONUSWIN).toFixed(2));
                    BKEY = null;
                    LOADBONUS = false;
                    BONUSLOADED = false;
                    BSTEPS = 0;
                    window.setTimeout(function () {
                        $('.div-button-spin').show();
                        $('.div-button-spin').bind('touch click', function (e) { Spin_Start(); });
                        $('.div-bonus > span').remove();
                    }, 2000);
                }
                return;
            }
        }
        StopBonusAnimFrame(function () { BonusReel_Stop(); });
    }
}
/* #endregion */
/* #endregion */

var FSIMG = null;
/* #region Highlight Reels */
var POPUP_SCALE = 0;
var POPUP_SCALE_DIRECTION = 0.02;
var POPUP_SCALE_MAX = 1.06;
function Lines_Draw_DEBUG(lines) {
    var line_coords = [];

    switch (lines) {
        case "1":
            line_coords = [{ x: 1, y: 2 }, { x: 2, y: 2 }, { x: 3, y: 2 }, { x: 4, y: 2 }, { x: 5, y: 2 }];
            break;
        case "2":
            line_coords = [{ x: 1, y: 1 }, { x: 2, y: 1 }, { x: 3, y: 1 }, { x: 4, y: 1 }, { x: 5, y: 1 }];
            break;
        case "3":
            line_coords = [{ x: 1, y: 3 }, { x: 2, y: 3 }, { x: 3, y: 3 }, { x: 4, y: 3 }, { x: 5, y: 3 }];
            break;
        case "4":
            line_coords = [{ x: 1, y: 1 }, { x: 2, y: 2 }, { x: 3, y: 3 }, { x: 4, y: 2 }, { x: 5, y: 1 }];
            break;
        case "5":
            line_coords = [{ x: 1, y: 3 }, { x: 2, y: 2 }, { x: 3, y: 1 }, { x: 4, y: 2 }, { x: 5, y: 3 }];
            break;
        case "6":
            line_coords = [{ x: 1, y: 1 }, { x: 2, y: 1 }, { x: 3, y: 2 }, { x: 4, y: 1 }, { x: 5, y: 1 }];
            break;
        case "7":
            line_coords = [{ x: 1, y: 3 }, { x: 2, y: 3 }, { x: 3, y: 2 }, { x: 4, y: 3 }, { x: 5, y: 3 }];
            break;
        case "8":
            line_coords = [{ x: 1, y: 2 }, { x: 2, y: 3 }, { x: 3, y: 3 }, { x: 4, y: 3 }, { x: 5, y: 2 }];
            break;
        case "9":
            line_coords = [{ x: 1, y: 2 }, { x: 2, y: 1 }, { x: 3, y: 1 }, { x: 4, y: 1 }, { x: 5, y: 2 }];
            break;
        case "10":
            line_coords = [{ x: 1, y: 2 }, { x: 2, y: 1 }, { x: 3, y: 2 }, { x: 4, y: 1 }, { x: 5, y: 2 }];
            break;
        case "11":
            line_coords = [{ x: 1, y: 2 }, { x: 2, y: 3 }, { x: 3, y: 2 }, { x: 4, y: 3 }, { x: 5, y: 2 }];
            break;
        case "12":
            line_coords = [{ x: 1, y: 1 }, { x: 2, y: 2 }, { x: 3, y: 1 }, { x: 4, y: 2 }, { x: 5, y: 1 }];
            break;
        case "13":
            line_coords = [{ x: 1, y: 3 }, { x: 2, y: 2 }, { x: 3, y: 3 }, { x: 4, y: 2 }, { x: 5, y: 3 }];
            break;
        case "14":
            line_coords = [{ x: 1, y: 2 }, { x: 2, y: 2 }, { x: 3, y: 1 }, { x: 4, y: 2 }, { x: 5, y: 2 }];
            break;
        case "15":
            line_coords = [{ x: 1, y: 2 }, { x: 2, y: 2 }, { x: 3, y: 3 }, { x: 4, y: 2 }, { x: 5, y: 2 }];
            break;
        case "16":
            line_coords = [{ x: 1, y: 1 }, { x: 2, y: 2 }, { x: 3, y: 2 }, { x: 4, y: 2 }, { x: 5, y: 1 }];
            break;
        case "17":
            line_coords = [{ x: 1, y: 3 }, { x: 2, y: 2 }, { x: 3, y: 2 }, { x: 4, y: 2 }, { x: 5, y: 3 }];
            break;
        case "18":
            line_coords = [{ x: 1, y: 1 }, { x: 2, y: 2 }, { x: 3, y: 3 }, { x: 4, y: 3 }, { x: 5, y: 3 }];
            break;
        case "19":
            line_coords = [{ x: 1, y: 3 }, { x: 2, y: 2 }, { x: 3, y: 1 }, { x: 4, y: 1 }, { x: 5, y: 1 }];
            break;
        case "20":
            line_coords = [{ x: 1, y: 1 }, { x: 2, y: 3 }, { x: 3, y: 1 }, { x: 4, y: 3 }, { x: 5, y: 1 }];
            break;
        case "21":
            line_coords = [{ x: 1, y: 3 }, { x: 2, y: 1 }, { x: 3, y: 3 }, { x: 4, y: 1 }, { x: 5, y: 3 }];
            break;
        case "22":
            line_coords = [{ x: 1, y: 1 }, { x: 2, y: 3 }, { x: 3, y: 3 }, { x: 4, y: 3 }, { x: 5, y: 1 }];
            break;
        case "23":
            line_coords = [{ x: 1, y: 3 }, { x: 2, y: 1 }, { x: 3, y: 1 }, { x: 4, y: 1 }, { x: 5, y: 3 }];
            break;
        case "24":
            line_coords = [{ x: 1, y: 1 }, { x: 2, y: 1 }, { x: 3, y: 3 }, { x: 4, y: 1 }, { x: 5, y: 1 }];
            break;
        case "25":
            line_coords = [{ x: 1, y: 3 }, { x: 2, y: 3 }, { x: 3, y: 1 }, { x: 4, y: 3 }, { x: 5, y: 3 }];
            break;
        case "26":
            line_coords = [{ x: 1, y: 1 }, { x: 2, y: 3 }, { x: 3, y: 2 }, { x: 4, y: 1 }, { x: 5, y: 3 }];
            break;
        case "27":
            line_coords = [{ x: 1, y: 3 }, { x: 2, y: 1 }, { x: 3, y: 2 }, { x: 4, y: 3 }, { x: 5, y: 1 }];
            break;
        case "28":
            line_coords = [{ x: 1, y: 2 }, { x: 2, y: 1 }, { x: 3, y: 3 }, { x: 4, y: 2 }, { x: 5, y: 3 }];
            break;
        case "29":
            line_coords = [{ x: 1, y: 1 }, { x: 2, y: 3 }, { x: 3, y: 2 }, { x: 4, y: 3 }, { x: 5, y: 2 }];
            break;
        case "30":
            line_coords = [{ x: 1, y: 3 }, { x: 2, y: 2 }, { x: 3, y: 1 }, { x: 4, y: 1 }, { x: 5, y: 2 }];
            break;
    }

    var random_color = randomRGBColor();

    CTXMAIN.lineWidth = "8";
    CTXMAIN.strokeStyle = random_color;
    CTXMAIN.beginPath();

    $.each(line_coords, function (index, value) {
        var x = value.x;
        var y = value.y;

        var originX = (SLOT_IMG_WIDTH * 0.5);
        var originY = ((SLOT_IMG_HEIGHT + SLOT_Y_OFFSET) * y) - (SLOT_IMG_HEIGHT + SLOT_Y_OFFSET) / 2;
        if (index == 0) {
            CTXMAIN.moveTo(originX, originY);
        } else {
            var x_coord = originX + ((SLOT_IMG_WIDTH + SLOT_X_OFFSET) * (x - 1));
            CTXMAIN.lineTo(x_coord, (SLOT_IMG_HEIGHT + SLOT_Y_OFFSET) * y - (SLOT_IMG_HEIGHT + SLOT_Y_OFFSET) / 2);
        }
    });
    CTXMAIN.stroke();
}
function Reels_Highlight() {
    var line_coords = [];
    var item = HIGHLIGHT_OBJ == null ? null : (HIGHLIGHT_OBJ.length > 0 ? HIGHLIGHT_OBJ[0] : null);

    if (item != null) {
        if (HIGHLIGHT) {
            POPUP_SCALE = 0;
            Image_Popup(item);
        }
    }
}
function Lines_Draw(obj) {
    var line_coords = [];
    var item = HIGHLIGHT_OBJ == null ? null : (HIGHLIGHT_OBJ.length > 0 ? HIGHLIGHT_OBJ[0] : null);

    if (item != null) {
        if (HIGHLIGHT) {
            switch (item.lines) {
                case "1":
                    line_coords = [{ x: 1, y: 2 }, { x: 2, y: 2 }, { x: 3, y: 2 }, { x: 4, y: 2 }, { x: 5, y: 2 }];
                    break;
                case "2":
                    line_coords = [{ x: 1, y: 1 }, { x: 2, y: 1 }, { x: 3, y: 1 }, { x: 4, y: 1 }, { x: 5, y: 1 }];
                    break;
                case "3":
                    line_coords = [{ x: 1, y: 3 }, { x: 2, y: 3 }, { x: 3, y: 3 }, { x: 4, y: 3 }, { x: 5, y: 3 }];
                    break;
                case "4":
                    line_coords = [{ x: 1, y: 1 }, { x: 2, y: 2 }, { x: 3, y: 3 }, { x: 4, y: 2 }, { x: 5, y: 1 }];
                    break;
                case "5":
                    line_coords = [{ x: 1, y: 3 }, { x: 2, y: 2 }, { x: 3, y: 1 }, { x: 4, y: 2 }, { x: 5, y: 3 }];
                    break;
                case "6":
                    line_coords = [{ x: 1, y: 1 }, { x: 2, y: 1 }, { x: 3, y: 2 }, { x: 4, y: 1 }, { x: 5, y: 1 }];
                    break;
                case "7":
                    line_coords = [{ x: 1, y: 3 }, { x: 2, y: 3 }, { x: 3, y: 2 }, { x: 4, y: 3 }, { x: 5, y: 3 }];
                    break;
                case "8":
                    line_coords = [{ x: 1, y: 2 }, { x: 2, y: 3 }, { x: 3, y: 3 }, { x: 4, y: 3 }, { x: 5, y: 2 }];
                    break;
                case "9":
                    line_coords = [{ x: 1, y: 2 }, { x: 2, y: 1 }, { x: 3, y: 1 }, { x: 4, y: 1 }, { x: 5, y: 2 }];
                    break;
                case "10":
                    line_coords = [{ x: 1, y: 2 }, { x: 2, y: 1 }, { x: 3, y: 2 }, { x: 4, y: 1 }, { x: 5, y: 2 }];
                    break;
                case "11":
                    line_coords = [{ x: 1, y: 2 }, { x: 2, y: 3 }, { x: 3, y: 2 }, { x: 4, y: 3 }, { x: 5, y: 2 }];
                    break;
                case "12":
                    line_coords = [{ x: 1, y: 1 }, { x: 2, y: 2 }, { x: 3, y: 1 }, { x: 4, y: 2 }, { x: 5, y: 1 }];
                    break;
                case "13":
                    line_coords = [{ x: 1, y: 3 }, { x: 2, y: 2 }, { x: 3, y: 3 }, { x: 4, y: 2 }, { x: 5, y: 3 }];
                    break;
                case "14":
                    line_coords = [{ x: 1, y: 2 }, { x: 2, y: 2 }, { x: 3, y: 1 }, { x: 4, y: 2 }, { x: 5, y: 2 }];
                    break;
                case "15":
                    line_coords = [{ x: 1, y: 2 }, { x: 2, y: 2 }, { x: 3, y: 3 }, { x: 4, y: 2 }, { x: 5, y: 2 }];
                    break;
                case "16":
                    line_coords = [{ x: 1, y: 1 }, { x: 2, y: 2 }, { x: 3, y: 2 }, { x: 4, y: 2 }, { x: 5, y: 1 }];
                    break;
                case "17":
                    line_coords = [{ x: 1, y: 3 }, { x: 2, y: 2 }, { x: 3, y: 2 }, { x: 4, y: 2 }, { x: 5, y: 3 }];
                    break;
                case "18":
                    line_coords = [{ x: 1, y: 1 }, { x: 2, y: 2 }, { x: 3, y: 3 }, { x: 4, y: 3 }, { x: 5, y: 3 }];
                    break;
                case "19":
                    line_coords = [{ x: 1, y: 3 }, { x: 2, y: 2 }, { x: 3, y: 1 }, { x: 4, y: 1 }, { x: 5, y: 1 }];
                    break;
                case "20":
                    line_coords = [{ x: 1, y: 1 }, { x: 2, y: 3 }, { x: 3, y: 1 }, { x: 4, y: 3 }, { x: 5, y: 1 }];
                    break;
                case "21":
                    line_coords = [{ x: 1, y: 3 }, { x: 2, y: 1 }, { x: 3, y: 3 }, { x: 4, y: 1 }, { x: 5, y: 3 }];
                    break;
                case "22":
                    line_coords = [{ x: 1, y: 1 }, { x: 2, y: 3 }, { x: 3, y: 3 }, { x: 4, y: 3 }, { x: 5, y: 1 }];
                    break;
                case "23":
                    line_coords = [{ x: 1, y: 3 }, { x: 2, y: 1 }, { x: 3, y: 1 }, { x: 4, y: 1 }, { x: 5, y: 3 }];
                    break;
                case "24":
                    line_coords = [{ x: 1, y: 1 }, { x: 2, y: 1 }, { x: 3, y: 3 }, { x: 4, y: 1 }, { x: 5, y: 1 }];
                    break;
                case "25":
                    line_coords = [{ x: 1, y: 3 }, { x: 2, y: 3 }, { x: 3, y: 1 }, { x: 4, y: 3 }, { x: 5, y: 3 }];
                    break;
                case "26":
                    line_coords = [{ x: 1, y: 1 }, { x: 2, y: 3 }, { x: 3, y: 2 }, { x: 4, y: 1 }, { x: 5, y: 3 }];
                    break;
                case "27":
                    line_coords = [{ x: 1, y: 3 }, { x: 2, y: 1 }, { x: 3, y: 2 }, { x: 4, y: 3 }, { x: 5, y: 1 }];
                    break;
                case "28":
                    line_coords = [{ x: 1, y: 2 }, { x: 2, y: 1 }, { x: 3, y: 3 }, { x: 4, y: 2 }, { x: 5, y: 3 }];
                    break;
                case "29":
                    line_coords = [{ x: 1, y: 1 }, { x: 2, y: 3 }, { x: 3, y: 2 }, { x: 4, y: 3 }, { x: 5, y: 2 }];
                    break;
                case "30":
                    line_coords = [{ x: 1, y: 3 }, { x: 2, y: 2 }, { x: 3, y: 1 }, { x: 4, y: 1 }, { x: 5, y: 2 }];
                    break;
            }

            var random_color = randomRGBColor();

            CTXMAIN.lineWidth = "8";
            CTXMAIN.strokeStyle = random_color;
            CTXMAIN.beginPath();

            $.each(line_coords, function (index, value) {
                var x = value.x;
                var y = value.y;
                var originX = (SLOT_IMG_WIDTH * 0.5);
                var originY = ((SLOT_IMG_HEIGHT + SLOT_Y_OFFSET) * y) - (SLOT_IMG_HEIGHT + SLOT_Y_OFFSET) / 2;
                if (index == 0) {
                    CTXMAIN.moveTo(originX, originY);
                } else {
                    var x_coord = originX + ((SLOT_IMG_WIDTH + SLOT_X_OFFSET) * (x - 1));
                    CTXMAIN.lineTo(x_coord, (SLOT_IMG_HEIGHT + SLOT_Y_OFFSET) * y - (SLOT_IMG_HEIGHT + SLOT_Y_OFFSET) / 2);
                }
            });
            CTXMAIN.stroke();
        }
    }
}
function Image_Popup(obj) {
    if (obj != null) {
        if (POPUP_SCALE < POPUP_SCALE_MAX) {
            window.setTimeout(function () {
                Canvas_Clear();
                Reels_Redraw();
                Lines_Draw(obj);
                $.each(obj.array, function (index, value) {
                    var x = index;
                    var y = value - 1;
                    if (y > -1) {
                        var item = SLOTS_ARRANGED_ASSETS[x][2 - y];
                        var drawX = x == 0 ? 0 : (SLOT_IMG_WIDTH + SLOT_X_OFFSET) * x;
                        var drawY = y == 0 ? 0 : (SLOT_IMG_HEIGHT + SLOT_Y_OFFSET) * y;

                        CTXMAIN.save(); // Save the current context   
                        CTXMAIN.translate(drawX + SLOT_IMG_WIDTH * 0.5, drawY + SLOT_IMG_HEIGHT * 0.5); // Translate to the center point of our image    
                        CTXMAIN.scale(POPUP_SCALE, POPUP_SCALE); // Perform the scale    
                        CTXMAIN.translate(-SLOT_IMG_WIDTH * 0.5, -SLOT_IMG_HEIGHT * 0.5); // Translate back to the top left of our image    
                        CTXMAIN.drawImage(SLOTS_BG_ARRAY[SLOTS_BG_ARRAY.length - 1].img, -1, -0.5, SLOT_IMG_WIDTH * (POPUP_SCALE_MAX - 0.04), SLOT_IMG_HEIGHT * (POPUP_SCALE_MAX - 0.04));// Finally we draw the image    
                        CTXMAIN.restore(); // And restore the context ready for the next loop    
                    }
                });
                POPUP_SCALE += POPUP_SCALE_DIRECTION; // Animate our scale value
                Image_Popup(obj);
            }, 1);
        } else {
            if (HIGHLIGHT_OBJ.length > 1) {
                window.setTimeout(function () {
                    POPUP_SCALE = 0;
                    HIGHLIGHT_OBJ.shift();
                    Image_Popup(HIGHLIGHT_OBJ[0]);
                }, 3000);
            }
            else if (HIGHLIGHT_OBJ.length <= 1) {
                if (LOADBONUS) { window.setTimeout(function () { Bonus_Load(); }, 1000); }
            }
        }
    }
}
/* #endregion */

function Balance_Draw(type) {
    var data = null;
    if (type) { data = NEW_BALANCE_XML; BALANCE_XML = NEW_BALANCE_XML; Reels_Highlight(); }
    else { data = BALANCE_XML; }
    var MemberBalance = $(data).find("BALANCE").attr('CREDIT');
    var MemberCoin = $(data).find("BALANCE").attr('COIN');
    var MemberConv = $(data).find("BALANCE").attr('CONV');
    var MemberWins = $(data).find("BALANCE").attr('WINS');
    MemberWins = MemberWins == null ? 0 : MemberWins;
    MEMBER_CURR = $(data).find("BALANCE").attr('CUR');
    $('.div-game-totalbets').text((PLAY_BETLINES * PLAY_AMOUNT * PLAY_MULIPLY).toFixed(2));
    if (!SPIN_STARTED) { $('.div-game-credits').text((MemberCoin * 1).toFixed(2)); }
    if (!SPIN_STARTED) { $('.div-game-winnings').text(parseFloat(MemberWins).toFixed(2)); }
}

var _GPIntMobileSlots = {
    preloadImages: function (type, gameName, path, images, extension, callback) {
        function _preload(asset) {
            asset.img = new Image();
            asset.img.src = path.replace('{GAMENAME}', gameName) + '/' + asset.id + (asset.type == null ? '.jpg' : asset.type);
            asset.img.addEventListener("load", function () { _check(); }, false);
            if (type == 'reels') { SLOTS_ITEMS_ASSETS.push(asset); }
        }
        var loadc = 0;
        function _check(err, id) { if (err) { alert('Failed to load ' + id); } loadc++; if (images.length == loadc) { return callback(); } }
        images.forEach(function (asset) { _preload(asset); });
    },
    copyArray: function (array) {
        var copy = [];
        for (var i = 0 ; i < array.length; i++) { copy.push(array[i]); }
        return copy;
    },
    shuffleArray: function (array) {
        for (i = array.length - 1; i > 0; i--) {
            var j = parseInt(Math.random() * i)
            var tmp = array[i];
            array[i] = array[j];
            array[j] = tmp;
        }
    }
}

/* #region animation frames */
window.RequestAnimFrame = (function () {
    return window.requestAnimationFrame ||
        window.webkitRequestAnimationFrame ||
        window.mozRequestAnimationFrame ||
        window.oRequestAnimationFrame ||
        window.msRequestAnimationFrame ||
        function (/* function */ callback, /* DOMElement */ element) {
            STARTTIMER = window.setTimeout(callback, FPS);
        };
})();
window.StopAnimFrame = (function () {
    return window.requestAnimationFrame ||
        window.webkitRequestAnimationFrame ||
        window.mozRequestAnimationFrame ||
        window.oRequestAnimationFrame ||
        window.msRequestAnimationFrame ||
        function (/* function */ callback, /* DOMElement */ element) {
            STOPTIMER = window.setTimeout(callback, FPS);
        };
})();
window.RequestBonusAnimFrame = (function () {
    return window.requestAnimationFrame ||
        window.webkitRequestAnimationFrame ||
        window.mozRequestAnimationFrame ||
        window.oRequestAnimationFrame ||
        window.msRequestAnimationFrame ||
        function (/* function */ callback, /* DOMElement */ element) {
            BONUSSTARTTIMER = window.setTimeout(callback, FPS);
        };
})();
window.StopBonusAnimFrame = (function () {
    return window.requestAnimationFrame ||
        window.webkitRequestAnimationFrame ||
        window.mozRequestAnimationFrame ||
        window.oRequestAnimationFrame ||
        window.msRequestAnimationFrame ||
        function (/* function */ callback, /* DOMElement */ element) {
            BONUSSTOPTIMER = window.setTimeout(callback, FPS);
        };
})();
/* #endregion */

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
    }
}

/* #region color */
function myRandom(n) { return Math.floor(Math.random() * n); }
function rgbColor(r, g, b) { return 'rgb(' + r + ',' + g + ',' + b + ')'; }
function randomRGBColor() { return rgbColor(myRandom(256), myRandom(256), myRandom(256)); }
/* #endregion */