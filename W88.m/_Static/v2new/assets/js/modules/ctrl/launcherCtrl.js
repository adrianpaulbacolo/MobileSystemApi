w88Mobile.v2.LauncherCtrl = launcherCtrl;

function launcherCtrl(routeObj, slotSvc, templateSvc) {

    this.title = "";
    this.game = [];
    this.page = {};
    this.route = "";
    this.routeObj = routeObj;
    this.mode = "fun";

    this.club = {};

    this.init = function () {
        var _self = this;
        if (!_.isUndefined(_self.club) && !_.isEmpty(_self.club.key)) _self.title = _contents.translate(_self.club.key);

        var headerHeight = _self.page.find("header.header").height();
        var launcherHeight = _self.page.height() - headerHeight;
        game = {
            height: launcherHeight + "px",
            top: headerHeight + "px"
        };


        switch (_self.mode) {
            default:
            case "fun":
                game.url = _self.game.FunUrl;
                break;
            case "real":
                game.url = _self.game.RealUrl;
                break;
        }
        var club = (!_.isUndefined(_self.club)) ? _self.club.name : "";
        switch (club) {
            case "palazzo":
                initPalazzo(
                    (_self.mode == "fun") ? 0 : 1
                    , _self.game.ProviderSettings.username
                    , _self.game.ProviderSettings.gamepass
                    , _self.game.ProviderSettings.gamelang
                    , game
                    , _self
                    );
                break
            default:
                _self.attachGame(game, true);
                break;
        }
    }

    this.resize = function () {
        var _self = this;
        _self.init();
    }

    this.attachGame = function (game, newWindow)
    {
        if (newWindow == true) {
            try {
                Native.onSlotGameOpened();
            } catch (e) {
                console.log(e.message)
            }
            window.open(game.url, "_blank");
            routeObj.previous();
            return;
        }
        var _self = this;
        var content = _.template(_templates.GameLauncher);
        var gameDiv = content({
            game: game
        });

        _self.page.find(".main-content").append(gameDiv);
        _self.page.find(".iframe-launcher").on("load", function (e) {
            try {
                Native.onSlotGameOpened();
            } catch (e) {
                console.log(e.message)
            }
        });
        var launcherOrigin = window.location.origin;
        window.launcherWindow = _.first(_self.page.find(".iframe-launcher")).contentWindow;


    }


    // hijack pushed data to include instance
    this.setPushData = function (data) {
        data._self = this;
        return data;
    }

    this.setActiveSection = function (section) {
        var _self = this;
        if (!_self.page.find('#sectionTab > li.' + section).hasClass('active')) {
            _self.page.find('#sectionTab > li').removeClass('active')
            _self.page.find('#sectionTab > li.' + section).addClass('active');
        }
    }

    /**
    * List of listeners below
    *
    **/

    /**
    * List of custom private functions
    *
    **/
    function initPalazzo(isReal, userName, password, langCode, game, _self) {

        link = game.url
        iapiSetClientPlatform("mobile&deliveryPlatform=HTML5");
        var result = iapiLogin(userName, password, isReal, langCode);
        iapiSetCallout('Login', calloutLogin);

        //CALLOUT----------------------------------------------
        function calloutLogin(response) {
            if (response.errorCode) {
                w88Mobile.Growl.shout("Login failed. " + response.playerMessage + " Error code: " + response.errorCode, function () {
                    _routes.previous();
                });
            }
            else {
                iapiRequestTemporaryToken(isReal, '427', 'GamePlay');
                iapiSetCallout('GetTemporaryAuthenticationToken', calloutGetTemporaryAuthenticationToken);
            }
        }

        function calloutGetTemporaryAuthenticationToken(response) {
            if (response.errorCode) {
                w88Mobile.Growl.shout("Token failed. " + response.playerMessage + " Error code: " + response.errorCode, function () {
                    _routes.previous();
                });
            }
            else {
                game.url = link.replace("{TOKEN}", response.sessionToken.sessionToken);
                _self.attachGame(game, true);
            }
        }
    }

}