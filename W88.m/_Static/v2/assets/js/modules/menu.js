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
            $('.menu_home').text(_w88_contents.translate("LABEL_MENU_HOME"));
            $('.menu_login').text(_w88_contents.translate("LABEL_MENU_LOGIN"));
            $('.menu_logout').text(_w88_contents.translate("LABEL_MENU_LOGOUT"));
            $('.menu_desktop').text(_w88_contents.translate("LABEL_MENU_DESKTOP"));
            $('.menu_promo').text(_w88_contents.translate("LABEL_MENU_PROMOTIONS"));
            $('.menu_rewards').text(_w88_contents.translate("LABEL_MENU_REWARDS"));
            $('.menu_live_chat').text(_w88_contents.translate("LABEL_MENU_LIVE_CHAT"));
            $('.menu_language').text(_w88_contents.translate("LABEL_MENU_LANGUAGE"));

            // Funds
            $('.menu_funds').text(_w88_contents.translate("LABEL_MENU_FUNDS"));
            $('.menu_deposit').text(_w88_contents.translate("LABEL_FUNDS_DEPOSIT"));
            $('.menu_transfer').text(_w88_contents.translate("LABEL_FUNDS_TRANSFER"));
            $('.menu_withdraw').text(_w88_contents.translate("LABEL_FUNDS_WIDRAW"));
            $('.menu_history').text(_w88_contents.translate("LABEL_FUNDS_HISTORY"));

            // Sports
            $('.menu_sports').text(_w88_contents.translate("LABEL_MENU_SPORTS"));
            $('.menu_clubw').text(_w88_contents.translate("LABEL_PRODUCTS_CLUB_W"));
            $('.menu_asport').text(_w88_contents.translate("LABEL_PRODUCTS_ASPORTS"));
            $('.menu_esport').text(_w88_contents.translate("LABEL_PRODUCTS_ESPORTS"));
            $('.menu_vsport').text(_w88_contents.translate("LABEL_PRODUCTS_VSPORTS"));
            $('.menu_xsport').text(_w88_contents.translate("LABEL_PRODUCTS_XSPORTS"));

            // Casino
            $('.menu_live_casino').text(_w88_contents.translate("LABEL_MENU_LIVE_CASINO"));
            $('.menu_clubwpremier').text(_w88_contents.translate("LABEL_PRODUCTS_CLUB_W_PREMIER"));

            // Slots
            $('.menu_slots').text(_w88_contents.translate("LABEL_MENU_SLOTS"));
            $('.menu_bravado').text(_w88_contents.translate("LABEL_PRODUCTS_BRAVADO"));
            $('.menu_massimo').text(_w88_contents.translate("LABEL_PRODUCTS_MASSIMO"));
            $('.menu_massimo_download').text(_w88_contents.translate("LABEL_PRODUCTS_MASSIMO_DOWNLOAD"));
            $('.menu_palazzo').text(_w88_contents.translate("LABEL_PRODUCTS_PALAZZO"));
            $('.menu_palazzo_download').text(_w88_contents.translate("LABEL_PRODUCTS_PALAZZO_DOWNLOAD"));
            $('.menu_gallardo').text(_w88_contents.translate("LABEL_PRODUCTS_GALLARDO"));
            $('.menu_apollo').text(_w88_contents.translate("LABEL_PRODUCTS_APOLLO"));
            $('.menu_divino').text(_w88_contents.translate("LABEL_PRODUCTS_DIVINO"));

            // Fishing Master
            $('.menu_fishing_master').text(_w88_contents.translate("LABEL_PRODUCTS_FISHING_MASTER"));

            // Lottery
            $('.menu_lottery').text(_w88_contents.translate("LABEL_MENU_LOTTERY"));
            $('.menu_keno').text(_w88_contents.translate("LABEL_PRODUCTS_KENO"));

            // Poker
            $('.menu_poker').text(_w88_contents.translate("LABEL_MENU_POKER"));

            // Texas Mahjong
            $('.menu_texas_mahjong').text(_w88_contents.translate("LABEL_MENU_TEXAS_MAHJONG"));
            $('.menu_android').text(_w88_contents.translate("LABEL_ANDROID_DOWNLOAD"));
            $('.menu_ios').text(_w88_contents.translate("LABEL_IOS_DOWNLOAD"));

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