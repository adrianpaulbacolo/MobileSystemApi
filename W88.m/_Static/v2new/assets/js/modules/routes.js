w88Mobile.v2.Routes = Routes();

function Routes() {

    function initPage(route, tpl) {

        if (routeStack.length > 0) {
            w88Mobile.v2.Routes.currentPage().css("display", "none");
        }

        var pageTpl = (!_.isEmpty(_templates.Mainpage)) ? _templates.Mainpage : '/_Static/v2new/assets/templates/page.html';

        return $.when($.get(pageTpl, function (template) {

            var mainClass = [];
            var sections = [];

            if (_.isEqual(route, "club")) {
                sections = w88Mobile.v2.Slots.sections;
            }

            var content = _.template(template);
            var page = content({
                page: route,
                classes: mainClass.join(" "),
                sections: sections
            });
            $("#page-stack").after(page);

        }));
    }

    var routes = {};

    routes["index"] = {
        init: function (params) {
            var initD = $.Deferred();
            var pageRoute = "index";
            initPage(pageRoute).then(function (page) {
                var indexpage = new w88Mobile.v2.SlotsCtrl(w88Mobile.v2.Routes
                    , w88Mobile.v2.Slots
                    , w88Mobile.v2._templates);
                routeCtrl.push(indexpage);

                indexpage.route = pageRoute;
                indexpage.page = w88Mobile.v2.Routes.currentPage();
                indexpage.init();
                pubsub.publish("changeHeader");
                initD.resolve();
            });

            return initD;
        }
        , onOpen: function () {
            pubsub.publish("changeHeader", {
                route: "index"
            });
        }
        , onClose: function () {
            pubsub.publish("changeHeader", {
                route: "index"
            });
        }
        , pushState: function () {
            History.pushState(null, null, '/v2/Slots');
        }
    }

    routes["index_search"] = {
        init: function (params) {
            var pageRoute = "index_search";
            initPage(pageRoute).then(function (page) {
                var indexpage = new w88Mobile.v2.SlotsCtrl(w88Mobile.v2.Routes
                    , w88Mobile.v2.Slots
                    , w88Mobile.v2._templates);
                routeCtrl.push(indexpage);

                indexpage.route = pageRoute;
                indexpage.page = w88Mobile.v2.Routes.currentPage();
                indexpage.init();
                pubsub.publish("changeHeader");
            });
        }
       , onOpen: function () {
           $('.filter-bar').hide();
       }
       , onClose: function () {
           $("." + _.last(routeStack) + "-page").remove();
           $('.filter-bar').hide();

           pubsub.publish("changeHeader");

           routeStack = _.dropRight(routeStack);
           routeCtrl = _.dropRight(routeCtrl);
       }
    }

    routes["club"] = {
        parent: "index"
        , init: function (params) {
            var pageRoute = "club";
            initPage(pageRoute).then(function () {
                var clubpage = new w88Mobile.v2.ClubsCtrl(w88Mobile.v2.Routes
                    , w88Mobile.v2.Slots
                    , w88Mobile.v2._templates);
                routeCtrl.push(clubpage);

                clubpage.route = pageRoute;
                clubpage.page = w88Mobile.v2.Routes.currentPage();
                clubpage.page = _.extend(clubpage.page, params);
                clubpage.club = _.find(w88Mobile.v2.Slots.clubs, function (club) {
                    return club.name == clubpage.page.club;
                });
                clubpage.init();
                pubsub.publish("changeHeader");
            });
        }
        , onOpen: function () {
            pubsub.publish("changeHeader");
            $('.filter-bar').show();
        }
        , onClose: function () {
            $("." + _.last(routeStack) + "-page").remove();
            $('.filter-bar').hide();

            pubsub.publish("changeHeader");

            routeStack = _.dropRight(routeStack);
            routeCtrl = _.dropRight(routeCtrl);
        }
        , pushState: function () {
            History.pushState(null, null, '/v2/Slots/' + _.last(routeCtrl).club.name.toCapitalize());
        }
    }

    routes["club_filter"] = {
        parent: "club"
        , init: function (params) {
            var pageRoute = "club_filter";
            initPage(pageRoute).then(function () {
                var clubpage = new w88Mobile.v2.ClubsCtrl(w88Mobile.v2.Routes
                    , w88Mobile.v2.Slots
                    , w88Mobile.v2._templates);
                routeCtrl.push(clubpage);

                clubpage.route = pageRoute;
                clubpage.page = w88Mobile.v2.Routes.currentPage();
                clubpage.page = _.extend(clubpage.page, params);
                clubpage.club = _.find(w88Mobile.v2.Slots.clubs, function (club) {
                    return club.name == clubpage.page.club;
                });
                clubpage.init();
                pubsub.publish("changeHeader");
                if (!_.isUndefined(params.form)) {
                    clubpage.filterClubSlots({
                        form: params.form
                    });
                }
            });
        }
        , onOpen: function () {
            // find suitable place to do this
            if (!_.isEmpty($('.filter-bar'))) {
                $('.filter-bar').remove();
                $('.club-page').removeClass('has-tab');
            }

            $('.filter-bar').show();
        }
        , onClose: function () {
            $("." + _.last(routeStack) + "-page").remove();
            $('.filter-bar').hide();

            pubsub.publish("changeHeader");

            routeStack = _.dropRight(routeStack);
            routeCtrl = _.dropRight(routeCtrl);
        }
    }


    routes["club_search"] = {
        parent: "index"
       , init: function (params) {
           var pageRoute = "club_search";
           initPage(pageRoute).then(function () {
               var clubpage = new w88Mobile.v2.ClubsCtrl(w88Mobile.v2.Routes
                   , w88Mobile.v2.Slots
                   , w88Mobile.v2._templates);
               clubpage.route = pageRoute;
               clubpage.page = w88Mobile.v2.Routes.currentPage();
               clubpage.page = _.extend(clubpage.page, params);
               clubpage.club = _.find(w88Mobile.v2.Slots.clubs, function (club) {
                   return club.name == clubpage.page.club;
               });
               clubpage.init();
               routeCtrl.push(clubpage);
               pubsub.publish("changeHeader", { club: w88Mobile.v2.Slots.club });

           });
       }
       , onOpen: function () {
           $('.filter-bar').hide();
       }
       , onClose: function () {
           $("." + _.last(routeStack) + "-page").remove();
           $('.filter-bar').show();
           pubsub.publish("changeHeader");

           routeStack = _.dropRight(routeStack);
           routeCtrl = _.dropRight(routeCtrl);
       }
    }

    routes["launcher"] = {
        parent: "index"
        , init: function (params) {
            var pageRoute = "launcher";
            var pageTemplate = 'assets/templates/page-full.html';
            initPage(pageRoute, pageTemplate).then(function () {
                var launcherPage = new w88Mobile.v2.LauncherCtrl(w88Mobile.v2.Routes
                    , w88Mobile.v2.Slots
                    , w88Mobile.v2._templates);
                routeCtrl.push(launcherPage);

                launcherPage.route = pageRoute;
                launcherPage.page = w88Mobile.v2.Routes.currentPage();
                launcherPage.page = _.extend(launcherPage.page, params);
                launcherPage.mode = (!_.isEmpty(params.mode)) ? params.mode : "fun";
                launcherPage.game = _.find(w88Mobile.v2.Slots.items, function (data) {
                    return _.isEqual(data.Id, launcherPage.page.gameId);
                });
                launcherPage.club = _.find(w88Mobile.v2.Slots.clubs, function (club) {
                    return club.name == launcherPage.page.club;
                });
                launcherPage.init();
                pubsub.publish("changeHeader");
            });
        }
        , onOpen: function () {
            pubsub.publish("changeHeader");
            $('.filter-bar').show();
        }
        , onClose: function () {
            $("." + _.last(routeStack) + "-page").remove();
            $('.filter-bar').hide();

            pubsub.publish("changeHeader");

            routeStack = _.dropRight(routeStack);
            routeCtrl = _.dropRight(routeCtrl);
        }
    }

    var routeStack = [];
    var routeCtrl = [];

    return {
        init: function () {
            _.forEach(routeStack, function (route) {
                routes[route].init();
            });
        },
        changeRoute: function (route, params, callback) {
            routes[route].onOpen();
            // assign deferred
            var initD = routes[route].init(params);
            routeStack.push(route);
            return initD;
        },
        currentPage: function () {
            var topPage = _.last(routeStack);
            return $("." + topPage + "-page");
        },
        currentCtrl: function () {
            return _.last(routeCtrl);
        },
        current: function () {
            return _.last(routeStack);
        },
        popPage: function () {
            var latestRoute = _.last(routeStack);
            routes[latestRoute].onClose();
            w88Mobile.v2.Routes.currentPage().css("display", "block");
        },
        previous: function () {
            if (routeStack.length > 1) {
                w88Mobile.v2.Routes.popPage();
                _.last(routeCtrl).init();
                if (typeof routes[_.last(routeStack)].pushState != "undefined") {
                    routes[_.last(routeStack)].pushState();
                }
            } else {
                window.location.href = "/v2/Dashboard.aspx"
            }
        },
        click_handler: function (e) {
            var url = $(this).attr('href'),
                title = $(this).attr('title');

            if (url.match(/:\/\//)) {
                return true;
            }

            if (url === '#') {
                return false;
            }

            e.preventDefault();
            History.pushState({}, title, url);
        },
        stack: function(){
            return routeStack;
        },
        clearStack: function (offset) {
            while (routeStack.length > offset && routeStack.length > 0) {
                w88Mobile.v2.Routes.popPage();
            }
        },
        isSameRoute: function (route) {
            return _.last(routeStack) == route;
        },
        slotRoutes: function () {

            return slotRoutes = {
                '/v2/slots': function () {
                    if (w88Mobile.v2.Routes.isSameRoute('index')) return;
                    if (w88Mobile.v2.Routes.stack().length == 0) w88Mobile.v2.Routes.changeRoute('index');
                    else w88Mobile.v2.Routes.clearStack(1);
                },
                '/v2/slots/:club': function (club) {
                    if (w88Mobile.v2.Routes.isSameRoute('club')) return;
                    if (w88Mobile.v2.Routes.stack().length == 0) {
                        $.when(w88Mobile.v2.Routes.changeRoute('index')).then(function () {
                            w88Mobile.v2.Routes.changeRoute('club', {
                                club: club
                            });
                        });
                    } else {
                        w88Mobile.v2.Routes.clearStack(1);
                        w88Mobile.v2.Routes.changeRoute('club', {
                            club: club
                        });

                    }
                }
            }
        }

    }
}