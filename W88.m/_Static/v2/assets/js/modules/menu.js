var _w88_menu = w88Mobile.Menu = Menu();
_w88_menu.init();

function Menu() {
    var menu = {
        init: init,
    }

    function init() {
        setTranslations();
    }

    function setTranslations() {
        if (_w88_contents.translate("LABEL_MENU_HOME") != "LABEL_MENU_HOME") {
            // Menu
            $('.menu_home').html(_w88_contents.translate("LABEL_MENU_HOME"));
            $('.menu_login').html(_w88_contents.translate("LABEL_MENU_LOGIN"));
            $('.menu_logout').html(_w88_contents.translate("LABEL_MENU_LOGOUT"));
            $('.menu_desktop').html(_w88_contents.translate("LABEL_MENU_DESKTOP"));
            $('.menu_promo').html(_w88_contents.translate("LABEL_MENU_PROMOTIONS"));
            $('.menu_rewards').html(_w88_contents.translate("LABEL_MENU_REWARDS"));
            $('.menu_live_chat').html(_w88_contents.translate("LABEL_MENU_LIVE_CHAT"));
            $('.menu_language').html(_w88_contents.translate("LABEL_MENU_LANGUAGE"));

            // Funds
            $('.menu_funds').html(_w88_contents.translate("LABEL_MENU_FUNDS"));
            $('.menu_deposit').html(_w88_contents.translate("LABEL_FUNDS_DEPOSIT"));
            $('.menu_transfer').html(_w88_contents.translate("LABEL_FUNDS_TRANSFER"));
            $('.menu_withdraw').html(_w88_contents.translate("LABEL_FUNDS_WIDRAW"));
            $('.menu_history').html(_w88_contents.translate("LABEL_FUNDS_HISTORY"));

            // Sports
            $('.menu_sports').html(_w88_contents.translate("LABEL_MENU_SPORTS"));
            $('.menu_clubw').html(_w88_contents.translate("LABEL_PRODUCTS_CLUB_W"));
            $('.menu_asport').html(_w88_contents.translate("LABEL_PRODUCTS_ASPORTS"));
            $('.menu_esport').html(_w88_contents.translate("LABEL_PRODUCTS_ESPORTS"));
            $('.menu_vsport').html(_w88_contents.translate("LABEL_PRODUCTS_VSPORTS"));
            $('.menu_xsport').html(_w88_contents.translate("LABEL_PRODUCTS_XSPORTS"));

            // Casino
            $('.menu_live_casino').html(_w88_contents.translate("LABEL_MENU_LIVE_CASINO"));
            $('.menu_clubwpremier').html(_w88_contents.translate("LABEL_PRODUCTS_CLUB_W_PREMIER"));

            // Slots
            $('.menu_slots').html(_w88_contents.translate("LABEL_MENU_SLOTS"));
            $('.menu_bravado').html(_w88_contents.translate("LABEL_PRODUCTS_BRAVADO"));
            $('.menu_massimo').html(_w88_contents.translate("LABEL_PRODUCTS_MASSIMO"));
            $('.menu_massimo_download').html(_w88_contents.translate("LABEL_PRODUCTS_MASSIMO_DOWNLOAD"));
            $('.menu_palazzo').html(_w88_contents.translate("LABEL_PRODUCTS_PALAZZO"));
            $('.menu_palazzo_download').html(_w88_contents.translate("LABEL_PRODUCTS_PALAZZO_DOWNLOAD"));
            $('.menu_gallardo').html(_w88_contents.translate("LABEL_PRODUCTS_GALLARDO"));
            $('.menu_apollo').html(_w88_contents.translate("LABEL_PRODUCTS_APOLLO"));
            $('.menu_divino').html(_w88_contents.translate("LABEL_PRODUCTS_DIVINO"));

            // Fishing Master
            $('.menu_fishing_master').html(_w88_contents.translate("LABEL_PRODUCTS_FISHING_MASTER"));

            // Lottery
            $('.menu_lottery').html(_w88_contents.translate("LABEL_MENU_LOTTERY"));
            $('.menu_keno').html(_w88_contents.translate("LABEL_PRODUCTS_KENO"));

            // Poker
            $('.menu_poker').html(_w88_contents.translate("LABEL_MENU_POKER"));

            // Texas Mahjong
            $('.menu_texas_mahjong').html(_w88_contents.translate("LABEL_MENU_TEXAS_MAHJONG"));
            $('.menu_android').html(_w88_contents.translate("LABEL_ANDROID_DOWNLOAD"));
            $('.menu_ios').html(_w88_contents.translate("LABEL_IOS_DOWNLOAD"));

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