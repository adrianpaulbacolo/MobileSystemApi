w88Mobile.v2.Routes = Routes();

function Routes() {

    function initPage(route) {

        if (routeStack.length > 0) {
            w88Mobile.v2.Routes.currentPage().css("display","none");
        }

        return $.when($.get('assets/templates/page.html', function (template) {

            var mainClass = [];

            if (route != "index") mainClass.push("has-tab");

            var content = _.template(template);
            var page = content({
                page: route,
                classes: mainClass.join(" ")
            });
            $(".header").after(page);
        }));
    }

    var routes = {};

    routes["index"] = {
        init: function (params) {
            var pageRoute = "index";
            initPage(pageRoute).then(function (page) {
                var indexpage = new w88Mobile.v2.SlotsCtrl(w88Mobile.v2.Routes);
                indexpage.route = pageRoute;
                indexpage.page = w88Mobile.v2.Routes.currentPage();
                indexpage.init();
            });
        }
    }


    routes["club"] = {
        parent: "index"
        , init: function (params) {
            initPage("club").then(function () {
                pubsub.publish("loadClubPage");

                var selectedClub = _.find(w88Mobile.v2.Slots.clubs, function (club) {
                    return club.name == params.club;
                });
                pubsub.publish("filterSlot", {club: selectedClub});
            });
        }
        , onOpen: function (){

        }
        , onClose: function (){
            $("." + _.last(routeStack) + "-page").remove();
            routeStack = _.dropRight(routeStack);
        }
    }

    var routeStack = ["index"];

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