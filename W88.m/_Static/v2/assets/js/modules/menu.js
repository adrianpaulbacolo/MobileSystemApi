var _w88_menu = w88Mobile.Menu = Menu();
_w88_menu.init();

function Menu() {
    var menu = {
        init: init,
    }

    function init() {
        setTranslations();
        setImages();
    }

    function setImages() {
        // Sports
        $('a.sports').bind('touch click', function () {
            $('.img_sports_clubW_iOS').attr('src', '/_Static/Images/sports/bnr-clubW88-iOS.jpg');
            $('.img_sports_clubW_Android').attr('src', '/_Static/Images/sports/bnr-clubW88-Android.jpg');
            $('.img_sports_A').attr('src', '/_Static/Images/sports/bnr-asports.jpg');
            $('.img_sports_E').attr('src', '/_Static/Images/sports/bnr-esports.jpg');
            $('.img_sports_V').attr('src', '/_Static/Images/sports/bnr-vSports.jpg');
            $('.img_sports_X').attr('src', '/_Static/Images/sports/bnr-xsports.jpg');
        });

        // Casino
        $('a.casino').bind('touch click', function () {
            $('.img_casino_clubWPremier_iOS').attr('src', '/_Static/Images/casino/bnr-clubwpremier-ios.jpg');
            $('.img_casino_clubW_iOS').attr('src', '/_Static/Images/casino/bnr-ClubW-iOS.jpg');
            $('.img_casino_clubWPremier_Android').attr('src', '/_Static/Images/casino/bnr-clubwpremier-android.jpg');
            $('.img_casino_clubW_Android').attr('src', '/_Static/Images/casino/bnr-clubw-android.jpg');
            $('.img_casino_palazzo').attr('src', '/_Static/Images/casino/bnr-clubpalazzo-casino-android.jpg');
            $('.img_casino_massimo').attr('src', '/_Static/Images/casino/bnr-clubmassimo-casino-android.jpg');

        });

        // Slots
        $('a.slots').bind('touch click', function () {
            $('.img_slots_clubW_iOS').attr('src', '/_Static/Images/sports/bnr-clubW88-iOS.jpg');
            $('.img_slots_clubW_Android').attr('src', '/_Static/Images/sports/bnr-clubW88-Android.jpg');
            $('.img_slots_bravado').attr('src', '/_Static/Images/bnr-clubbravado.jpg');
            $('.img_slots_massimo').attr('src', '/_Static/Images/bnr-clubmassimo-slots.jpg');
            $('.img_slots_massimo-download').attr('src', '/_Static/Images/bnr-clubmassimo-slots2.jpg');
            $('.img_slots_palazzo').attr('src', '/_Static/Images/bnr-clubpalazzo-slots.jpg');
            $('.img_slots_palazzo-download').attr('src', '/_Static/Images/bnr-clubpalazzo-slots2.jpg');
            $('.img_slots_gallardo').attr('src', '/_Static/Images/bnr-clubgallardo.jpg');
            $('.img_slots_apollo').attr('src', '/_Static/Images/bnr-clubapollo.jpg');
            $('.img_slots_divino').attr('src', '/_Static/Images/bnr-clubdivino.jpg');

        });

        // Fishing
        $('a.fishing').bind('touch click', function () {
            $('.img_fishing_master_iOS').attr('src', '/_Static/Images/bnr-fishingmaster-iOs.jpg');
            $('.img_fishing_master_Android').attr('src', '/_Static/Images/bnr-fishingmaster-android.jpg');
            $('.img_fishing_world').attr('src', '/_Static/Images/thumbnail-FishingWorld.jpg');
        });

        // Lottery
        $('a.lottery').bind('touch click', function () {
            $('.img_lottery_keno').attr('src', '/_Static/Images/bnr_lottery.jpg');
        });

        // Poker
        $('a.poker').bind('touch click', function () {
            $('.img_poker_iOS').attr('src', '/_Static/Images/bnr-fishingmaster-iOs.jpg');
            $('.img_poker_Android').attr('src', '/_Static/Images/bnr-fishingmaster-android.jpg');
        });

        // Texas Mahjong
        $('a.texas_mahjong').bind('touch click', function () {
            $('.img_texas_mahjong_iOS').attr('src', '/_Static/Images/Download/TexasMahjong-iOS.jpg');
            $('.img_texas_mahjong_Android').attr('src', '/_Static/Images/Download/TexasMahjong-Android.jpg');
        });
    }

    function setTranslations() {
        if (_w88_contents.translate("LABEL_MENU_HOME") != "LABEL_MENU_HOME") {
            // Menu
            $('.title_home').html(_w88_contents.translate("LABEL_MENU_HOME"));
            $('.title_login').html(_w88_contents.translate("LABEL_MENU_LOGIN"));
            $('.title_logout').html(_w88_contents.translate("LABEL_MENU_LOGOUT"));
            $('.title_desktop').html(_w88_contents.translate("LABEL_MENU_DESKTOP"));
            $('.title_promo').html(_w88_contents.translate("LABEL_MENU_PROMOTIONS"));
            $('.title_rewards').html(_w88_contents.translate("LABEL_MENU_REWARDS"));
            $('.title_live_chat').html(_w88_contents.translate("LABEL_MENU_LIVE_CHAT"));
            $('.title_language').html(_w88_contents.translate("LABEL_MENU_LANGUAGE"));

            // Funds
            $('.title_funds').html(_w88_contents.translate("LABEL_MENU_FUNDS"));
            $('.title_deposit').html(_w88_contents.translate("LABEL_FUNDS_DEPOSIT"));
            $('.title_transfer').html(_w88_contents.translate("LABEL_FUNDS_TRANSFER"));
            $('.title_withdraw').html(_w88_contents.translate("LABEL_FUNDS_WIDRAW"));
            $('.title_history').html(_w88_contents.translate("LABEL_FUNDS_HISTORY"));

            // Sports
            $('.title_sports').html(_w88_contents.translate("LABEL_MENU_SPORTS"));
            $('.title_clubw').html(_w88_contents.translate("LABEL_PRODUCTS_CLUB_W"));
            $('.title_asport').html(_w88_contents.translate("LABEL_PRODUCTS_ASPORTS"));
            $('.title_esport').html(_w88_contents.translate("LABEL_PRODUCTS_ESPORTS"));
            $('.title_vsport').html(_w88_contents.translate("LABEL_PRODUCTS_VSPORTS"));
            $('.title_xsport').html(_w88_contents.translate("LABEL_PRODUCTS_XSPORTS"));

            // Casino
            $('.title_live_casino').html(_w88_contents.translate("LABEL_MENU_LIVE_CASINO"));
            $('.title_clubwpremier').html(_w88_contents.translate("LABEL_PRODUCTS_CLUB_W_PREMIER"));

            // Slots
            $('.title_slots').html(_w88_contents.translate("LABEL_MENU_SLOTS"));
            $('.title_bravado').html(_w88_contents.translate("LABEL_PRODUCTS_BRAVADO"));
            $('.title_massimo').html(_w88_contents.translate("LABEL_PRODUCTS_MASSIMO"));
            $('.title_massimo_download').html(_w88_contents.translate("LABEL_PRODUCTS_MASSIMO_DOWNLOAD"));
            $('.title_palazzo').html(_w88_contents.translate("LABEL_PRODUCTS_PALAZZO"));
            $('.title_palazzo_download').html(_w88_contents.translate("LABEL_PRODUCTS_PALAZZO_DOWNLOAD"));
            $('.title_gallardo').html(_w88_contents.translate("LABEL_PRODUCTS_GALLARDO"));
            $('.title_apollo').html(_w88_contents.translate("LABEL_PRODUCTS_APOLLO"));
            $('.title_divino').html(_w88_contents.translate("LABEL_PRODUCTS_DIVINO"));

            // Fishing Master
            $('.title_fishing').html(_w88_contents.translate("LABEL_PRODUCTS_FISHING"));
            $('.title_fishing_master').html(_w88_contents.translate("LABEL_PRODUCTS_FISHING_MASTER"));
            $('.title_fishing_world').html(_w88_contents.translate("LABEL_PRODUCTS_FISHING_WORLD"));

            // Lottery
            $('.title_lottery').html(_w88_contents.translate("LABEL_MENU_LOTTERY"));
            $('.title_keno').html(_w88_contents.translate("LABEL_PRODUCTS_KENO"));

            // Poker
            $('.title_poker').html(_w88_contents.translate("LABEL_MENU_POKER"));

            // Texas Mahjong
            $('.title_texas_mahjong').html(_w88_contents.translate("LABEL_MENU_TEXAS_MAHJONG"));
            $('.title_android').html(_w88_contents.translate("LABEL_ANDROID_DOWNLOAD"));
            $('.title_ios').html(_w88_contents.translate("LABEL_IOS_DOWNLOAD"));

        } else {
            window.setInterval(function () {
                setTranslations();
            }, 500);
        }
    }

    function send(data, resource, method, success, complete) {
        var url = w88Mobile.APIUrl + resource;

        var headers = {
            'Token': window.User.token,
            'LanguageCode': window.User.lang
        };

        $.ajax({
            type: method,
            url: url,
            data: data,
            beforeSend: function () {
                pubsub.publish('startLoadItem', { selector: "" });
            },
            headers: headers,
            success: success,
            error: function (resp) {
                console.log("Error connecting to api");
            },
            complete: function () {
                if (!_.isUndefined(complete)) complete();
                pubsub.publish('stopLoadItem', { selector: "" });
            }
        });
    };

    return menu;

}