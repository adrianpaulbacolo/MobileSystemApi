w88Mobile.v2.Routes = Routes();

function Routes() {

    function initPage(route) {

        if (routeStack.length > 0) {
            w88Mobile.v2.Routes.currentPage().css("display", "none");
        }

        return $.when($.get('assets/templates/page.html', function (template) {

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
            var pageRoute = "index";
            initPage(pageRoute).then(function (page) {
                var indexpage = new w88Mobile.v2.SlotsCtrl(w88Mobile.v2.Routes
                    , w88Mobile.v2.Slots
                    , w88Mobile.v2._templates);
                indexpage.route = pageRoute;
                indexpage.page = w88Mobile.v2.Routes.currentPage();
                indexpage.init();
                routeCtrl.push(indexpage);
            });
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
    }

    routes["index_search"] = {
        init: function (params) {
            var pageRoute = "index_search";
            initPage(pageRoute).then(function (page) {
                var indexpage = new w88Mobile.v2.SlotsCtrl(w88Mobile.v2.Routes
                    , w88Mobile.v2.Slots
                    , w88Mobile.v2._templates);
                indexpage.route = pageRoute;
                indexpage.page = w88Mobile.v2.Routes.currentPage();
                indexpage.init();
                routeCtrl.push(indexpage);
            });
        }
       , onOpen: function () {
           pubsub.publish("changeHeader");
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
                clubpage.route = pageRoute;
                clubpage.page = w88Mobile.v2.Routes.currentPage();
                clubpage.page = _.extend(clubpage.page, params);
                clubpage.club = _.find(w88Mobile.v2.Slots.clubs, function (club) {
                    return club.name == clubpage.page.club;
                });
                clubpage.init();
                routeCtrl.push(clubpage);
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

    routes["club_filter"] = {
        parent: "club"
        , init: function (params) {
            var pageRoute = "club_filter";
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
                if (!_.isUndefined(params.form)) {
                    clubpage.filterClubSlots({
                        form: params.form
                    });
                }
            });
        }
        , onOpen: function () {
            pubsub.publish("changeHeader");
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

           });
       }
       , onOpen: function () {
           pubsub.publish("changeHeader", { club: w88Mobile.v2.Slots.club });
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

    var routeStack = ["index"];
    var routeCtrl = [];

    return {
        init: function () {
            _.forEach(routeStack, function (route) {
                routes[route].init();
            });
        },
        changeRoute: function (route, params) {
            routes[route].onOpen();
            routes[route].init(params);
            routeStack.push(route);
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
            w88Mobile.v2.Routes.currentPage().css("display", "inline");
        },
        previous: function () {
            if (routeStack.length > 1) {
                w88Mobile.v2.Routes.popPage()
            } else {

            }
        },

    }
}