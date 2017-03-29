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

        _self.attachGame(game);

    }

    this.attachGame = function (game)
    {
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

}