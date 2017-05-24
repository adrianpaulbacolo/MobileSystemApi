window.w88Mobile.Products = Products();
var _w88_products = window.w88Mobile.Products;

function Products() {

    var products = {};

    products.init = function() {

        $(".menuHomeTitle").text(_w88_contents.translate("LABEL_MENU_HOME"));
        $(".menuLoginTitle").text(_w88_contents.translate("LABEL_MENU_LOGIN"));
        $(".menuJoinTitle").text(_w88_contents.translate("LABEL_JOIN"));
        $(".menuFundsTitle").text(_w88_contents.translate("LABEL_MENU_FUNDS"));
        $(".menuDepositTitle").text(_w88_contents.translate("LABEL_FUNDS_DEPOSIT"));
        $(".menuTransferTitle").text(_w88_contents.translate("LABEL_FUNDS_TRANSFER"));
        $(".menuWidrawTitle").text(_w88_contents.translate("LABEL_FUNDS_WIDRAW"));
        $(".menuHistoryTitle").text(_w88_contents.translate("LABEL_FUNDS_HISTORY"));

        $(".menuClubWTitle").text(_w88_contents.translate("LABEL_PRODUCTS_CLUB_W"));
        $(".menuClubWPremierTitle").text(_w88_contents.translate("LABEL_PRODUCTS_CLUB_W_PREMIER"));

        $(".menuASportsTitle").text(_w88_contents.translate("LABEL_PRODUCTS_ASPORTS"));
        $(".menuESportsTitle").text(_w88_contents.translate("LABEL_PRODUCTS_ESPORTS"));
        $(".menuXSportsTitle").text(_w88_contents.translate("LABEL_PRODUCTS_XSPORTS"));
        $(".menuVSportsTitle").text(_w88_contents.translate("LABEL_PRODUCTS_VSPORTS"));

        $(".menuPokerTitle").text(_w88_contents.translate("LABEL_MENU_POKER"));
        $(".menuSportsTitle").text(_w88_contents.translate("LABEL_MENU_SPORTS"));
        $(".menuLotteryTitle").text(_w88_contents.translate("LABEL_MENU_LOTTERY"));
        $(".play-pk").text(_w88_contents.translate("BUTTON_PLAY_NOW"));
        $(".try-pk").text(_w88_contents.translate("BUTTON_TRY_NOW"));
        $(".menuLiveCasinoTitle").text(_w88_contents.translate("LABEL_MENU_LIVE_CASINO"));
        $(".menuSlotsTitle").text(_w88_contents.translate("LABEL_MENU_SLOTS"));
        $(".menuTexasMahjongTitle").text(_w88_contents.translate("LABEL_MENU_TEXAS_MAHJONG"));
        $(".menuP2PTitle").text(_w88_contents.translate("LABEL_MENU_P2P"));
        $(".menuSuperBullTitle").text(_w88_contents.translate("LABEL_MENU_SUPERBULL"));
        $(".menuRewardsTitle").text(_w88_contents.translate("LABEL_MENU_REWARDS"));
        $(".menuPromotionsTitle").text(_w88_contents.translate("LABEL_MENU_PROMOTIONS"));
        $(".menuLiveChatTitle").text(_w88_contents.translate("LABEL_MENU_LIVE_CHAT"));
        $(".menuLanguageTitle").text(_w88_contents.translate("LABEL_MENU_LANGUAGE"));
        $(".menuLogoutTitle").text(_w88_contents.translate("LABEL_MENU_LOGOUT"));
        $(".menuDesktopNavTitle").text(_w88_contents.translate("LABEL_MENU_DESKTOP"));
        $(".menuDesktopTitle").text(_w88_contents.translate("LABEL_MENU_DESKTOP_SITE"));
        $(".menuKenoTitle").text(_w88_contents.translate("LABEL_PRODUCTS_KENO"));
        $(".menuPK10Title").text(_w88_contents.translate("LABEL_PRODUCTS_PK10"));
        
        $(".menuFishingWorldTitle").text(_w88_contents.translate("LABEL_PRODUCTS_FISHING_WORLD"));
        $(".menuLuckyFishingTitle").text(_w88_contents.translate("LABEL_PRODUCTS_FISHING_MASTER"));
        $(".menuFishingTitle").text(_w88_contents.translate("LABEL_PRODUCTS_FISHING"));

        $(".menuAndroidDownloadTitle").html(_w88_contents.translate("LABEL_ANDROID_DOWNLOAD"));
        $(".menuiOSDownloadTitle").html(_w88_contents.translate("LABEL_IOS_DOWNLOAD"));

        $(".menuClubMassimoDownloadTitle").html(_w88_contents.translate("LABEL_PRODUCTS_MASSIMO_DOWNLOAD"));
        $(".menuClubPalazzoDownloadTitle").html(_w88_contents.translate("LABEL_PRODUCTS_PALAZZO_DOWNLOAD"));

        $(".ClubNuovoNavTitle").text(_w88_contents.translate("LABEL_PRODUCTS_CLUB_NUOVO"));
        $(".ClubBravadoNavTitle").text(_w88_contents.translate("LABEL_PRODUCTS_BRAVADO"));
        $(".ClubMassimoNavTitle").text(_w88_contents.translate("LABEL_PRODUCTS_MASSIMO"));
        $(".ClubPalazzoNavTitle").text(_w88_contents.translate("LABEL_PRODUCTS_PALAZZO"));
        $(".ClubGallardoNavTitle").text(_w88_contents.translate("LABEL_PRODUCTS_GALLARDO"));
        $(".ClubApolloNavTitle").text(_w88_contents.translate("LABEL_PRODUCTS_APOLLO"));
        $(".ClubDivinoNavTitle").text(_w88_contents.translate("LABEL_PRODUCTS_DIVINO"));

    };

    products.checkFreeRounds = function () {

        var headers = {
            'Token': window.User.token,
            'LanguageCode': window.User.lang
        };

        var data = { cashier: "Funds.aspx", Lobby: "ClubBravado" };

        $.ajax({
            type: "GET",
            url: w88Mobile.APIUrl + "/products/freerounds/gpi",
            data: data,
            beforeSend: function() {
                pubsub.publish('startLoadItem', { selector: '' });
            },
            headers: headers,
            success: function(response) {
                switch (response.ResponseCode) {
                case 1:
                    _w88_products.FreeRoundsGameUrl = response.ResponseData;
                    break;
                }

                pubsub.publish('stopLoadItem', { selector: '' });
                pubsub.publish('checkFreeRounds', { selector: '' });
            },
            error: function(response) {
                if (_.isUndefined(response.ResponseData)) {
                    pubsub.publish('stopLoadItem', { selector: '' });
                    console.log('Unable to get freerounds.');
                    return;
                }
            },
            complete: function() {
                pubsub.publish('stopLoadItem', { selector: '' });               
            }
        });

    };

    return products;
}
