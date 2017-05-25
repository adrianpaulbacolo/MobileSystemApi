var _w88_menu = w88Mobile.Menu = Menu();
_w88_menu.init();

function Menu() {
    var menu = {
        init: init,
    }

    function init() {
        setImages();
        getUrl();
    }

    function setImages() {
        // Sports
        $('a.sports').bind('touch click', function () {
            $('.img_sports_clubW_iOS').attr('src', _constants.IMAGES_URL + '/sports/bnr-clubW88-iOS.jpg');
            $('.img_sports_clubW_Android').attr('src', _constants.IMAGES_URL + '/sports/bnr-clubW88-Android.jpg');
            $('.img_sports_A').attr('src', _constants.IMAGES_URL + '/sports/bnr-asports.jpg');
            $('.img_sports_E').attr('src', _constants.IMAGES_URL + '/sports/bnr-esports.jpg');
            $('.img_sports_V').attr('src', _constants.IMAGES_URL + '/sports/bnr-vSports.jpg');
            $('.img_sports_X').attr('src', _constants.IMAGES_URL + '/sports/bnr-xsports.jpg');
        });

        // Casino
        $('a.casino').bind('touch click', function () {
            $('.img_casino_clubWPremier_iOS').attr('src', _constants.IMAGES_URL + '/casino/bnr-clubwpremier-ios.jpg');
            $('.img_casino_clubW_iOS').attr('src', _constants.IMAGES_URL + '/casino/bnr-ClubW-iOS.jpg');
            $('.img_casino_clubWPremier_Android').attr('src', _constants.IMAGES_URL + '/casino/bnr-clubwpremier-android.jpg');
            $('.img_casino_clubW_Android').attr('src', _constants.IMAGES_URL + '/casino/bnr-clubw-android.jpg');
            $('.img_casino_palazzo').attr('src', _constants.IMAGES_URL + '/casino/bnr-clubpalazzo-casino-android.jpg');
            $('.img_casino_massimo').attr('src', _constants.IMAGES_URL + '/casino/bnr-clubmassimo-casino-android.jpg');

        });

        // Slots
        $('a.slots').bind('touch click', function () {
            $('.img_slots_clubW_iOS').attr('src', _constants.IMAGES_URL + '/sports/bnr-clubW88-iOS.jpg');
            $('.img_slots_clubW_Android').attr('src', _constants.IMAGES_URL + '/sports/bnr-clubW88-Android.jpg');
            $('.img_slots_bravado').attr('src', _constants.IMAGES_URL + '/bnr-clubbravado.jpg');
            $('.img_slots_massimo').attr('src', _constants.IMAGES_URL + '/bnr-clubmassimo-slots.jpg');
            $('.img_slots_massimo-download').attr('src', _constants.IMAGES_URL + '/bnr-clubmassimo-slots2.jpg');
            $('.img_slots_palazzo').attr('src', _constants.IMAGES_URL + '/bnr-clubpalazzo-slots.jpg');
            $('.img_slots_palazzo-download').attr('src', _constants.IMAGES_URL + '/bnr-clubpalazzo-slots2.jpg');
            $('.img_slots_gallardo').attr('src', _constants.IMAGES_URL + '/bnr-clubgallardo.jpg');
            $('.img_slots_apollo').attr('src', _constants.IMAGES_URL + '/bnr-clubapollo.jpg');
            $('.img_slots_divino').attr('src', _constants.IMAGES_URL + '/bnr-clubdivino.jpg');

        });

        // Fishing
        $('a.fishing').bind('touch click', function () {
            $('.img_fishing_master_iOS').attr('src', _constants.IMAGES_URL + '/bnr-fishingmaster-iOs.jpg');
            $('.img_fishing_master_Android').attr('src', _constants.IMAGES_URL + '/bnr-fishingmaster-android.jpg');
            $('.img_fishing_world').attr('src', _constants.IMAGES_URL + '/thumbnail-FishingWorld.jpg');
        });

        // Lottery
        $('a.lottery').bind('touch click', function () {
            $('.img_lottery_keno').attr('src', _constants.IMAGES_URL + '/bnr_lottery.jpg');
            $('.img_lottery_pk10').attr('src', _constants.IMAGES_URL + '/lottery/bnr_lottery_pk10.jpg');
        });

        // Poker
        $('a.poker').bind('touch click', function () {
            $('.img_poker_iOS').attr('src', _constants.IMAGES_URL + '/bnr-fishingmaster-iOs.jpg');
            $('.img_poker_Android').attr('src', _constants.IMAGES_URL + '/bnr-fishingmaster-android.jpg');
        });

        // Texas Mahjong
        $('a.texas_mahjong').bind('touch click', function () {
            $('.img_texas_mahjong_iOS').attr('src', _constants.IMAGES_URL + '/Download/TexasMahjong-iOS.jpg');
            $('.img_texas_mahjong_Android').attr('src', _constants.IMAGES_URL + '/Download/TexasMahjong-Android.jpg');
            $('.img_super_bull').attr('src', _constants.IMAGES_URL + '/Download/bnr-superbull-android.jpg');
        });
    }

    function getUrl() {

        // Login
        $('.url_login').attr('href', _constants.LOGIN_URL);

        // Funds
        $('.url_deposit').attr('href', _constants.DEPOSIT_URL);
        $('.url_transfer').attr('href', _constants.TRANSFER_URL);
        $('.url_withdraw').attr('href', _constants.WITHRAW_URL);
        $('.url_history').attr('href', _constants.HISTORY_URL);

        // Sports
        getSportsUrl();

        // Slot
        $('.url_slot').attr('href', _constants.SLOT_URL);

        // Downloads
        $('.url_download').attr('href', _constants.DOWNLOAD_URL);

        // Club W Premier
        $('.url_download_clubW_ios').attr('href', _constants.DOWNLOAD_CLUBW_IOS_URL);

        // Palazzo
        $('.url_palazzo_casino').attr('href', _constants.DOWNLOAD_PALAZZO_CASINO_URL);
        $('.url_palazzo_slot').attr('href', _constants.DOWNLOAD_PALAZZO_SLOT_URL);

        // Massimo
        $('.url_massimo').attr('href', _constants.MASSIMO_URL);
        $('.url_massimo_slot').attr('href', _constants.MASSIMO_SLOTS_URL);

        // Lottery
        $('.url_massimo').attr('href', _constants.MASSIMO_URL);

        // Promotions
        $('.url_promo').attr('href', _constants.PROMO_URL);


        // Texas Mahjong
        $('.url_texas_mahjong').attr('href', _constants.DOWNLOAD_TM_IOS_URL);
        $('.url_super_bull').attr('href', _constants.DOWNLOAD_SB_IOS_URL);
    }

    function getSportsUrl() {
        var aSportsItem = {
            "Link": _constants.A_SPORTS_URL,
            "Lang": {
                "en-us": "en",
                "id-id": "id",
                "ja-jp": "jp",
                "km-kh": "en",
                "ko-kr": "ko",
                "th-th": "th",
                "vi-vn": "vn",
                "zh-cn": "cs"
            }
        };

        var eSportsItem = {
            "Link": _constants.E_SPORTS_URL,
            "Lang": {
                "en-us": "236",
                "id-id": "242",
                "ja-jp": "269",
                "km-kh": "236",
                "ko-kr": "270",
                "th-th": "244",
                "vi-vn": "241",
                "zh-cn": "240"
            }
        };

        var xSportsItem = {
            "Link": {
                "Fun": _constants.X_SPORTS_FUN_URL,
                "Real": _constants.X_SPORTS_REAL_URL
            },
            "Lang": {
                "en-us": "en-US",
                "id-id": "id-ID",
                "ja-jp": "ja-JP",
                "km-kh": "en-US",
                "ko-kr": "ko-KR",
                "th-th": "th-TH",
                "vi-vn": "vi-VN",
                "zh-cn": "zh-CN"
            },
            "Currency": {
                "RMB": "CNY",
                "VND": "VD",
            }
        };

        // TODO: get values from localstorage
        var selectedLang = siteCookie.getCookie("language");
        var memberID = window.User.memberId;
        var currency = siteCookie.getCookie('currencyCode');

        if (_.isEmpty(siteCookie.getCookie("s"))) {
            $('.url_sports_A').attr('href', _constants.LOGIN_URL);

            $('.url_sports_E').attr('href', _constants.LOGIN_URL);

            var xsportsLink = xSportsItem.Link.Fun.replace('{LANG}', xSportsItem.Lang[selectedLang]);
            $('.url_sports_X').attr('href', xsportsLink);

        } else {

            var asportsLink = aSportsItem.Link.replace('{LANG}', aSportsItem.Lang[selectedLang]);
            $('.url_sports_A').attr('href', asportsLink);

            var esportsLink = eSportsItem.Link.replace('{LANG}', eSportsItem.Lang[selectedLang]);
            $('.url_sports_E').attr('href', esportsLink);

            var xsportsLink = xSportsItem.Link.Real.replace('{LANG}', xSportsItem.Lang[selectedLang]).replace('{USER}', memberID).replace('{CURR}', xSportsItem.Currency[currency] || currency);
            $('.url_sports_X').attr('href', xsportsLink);
        }

        $('.url_sports_V').attr('href', _constants.V_SPORTS_URL);

    }

    return menu;
}