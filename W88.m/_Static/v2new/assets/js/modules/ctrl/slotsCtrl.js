w88Mobile.v2.SlotsCtrl = slotsCtrl;

function slotsCtrl(routeObj, slotSvc, templateSvc) {

    var _self = this;
    this.games = [];
    this.page = {};
    this.route = "";
    this.routeObj = routeObj;
    this.title = "";

    this.init = function () {
        var _self = this;
        pubsub.subscribe("displaySlotList", onDisplaySlotList);
        _self.games = w88Mobile.v2.Slots.items;

        switch (_self.route) {
            case "index":
                _.forEach(slotSvc.clubs, function (club) {
                    _.forEach(club.providers, function (provider) {
                        _self.onSlotItemsChanged({
                            provider: provider
                        });
                    })
                });
                break;

            case "index_search":
                break;
        }
    }

    this.resize = function () {
        var _self = this;
        if (_self.route != "index") return;
        _.forEach(slotSvc.clubs, function (club) {
            _.forEach(club.providers, function (prov) {
                _self.onSlotItemsChanged({
                    provider: prov
                });
            });
        });

    }

    // hijack pushed data to include instance
    this.setPushData = function (data) {
        data._self = this;
        return data;
    }

    this.viewClub = function (club) {
        routeObj.changeRoute("club", {
            club: club
        });
    }

    this.filterSearchSlots = function (filter) {

        var _self = this;
        var games = slotSvc.filterSlots(filter, _.clone(_self.games));

        pubsub.publish('searchSlots', {
            club: {}
            , games: games
        });
    }


    /**
    * List of listeners below
    *
    **/

    this.onSlotItemsChanged = function(data) {
        var _self = this;

        switch (_self.route) {
            case "index":
                var clubs = _.filter(w88Mobile.v2.Slots.clubs, function (club) {
                    return _.indexOf(club.providers, data.provider.toLowerCase()) !== -1;
                });

                if (_.isEmpty(clubs)) return;

                var section = "Home";

                _.forEach(clubs, function (club) {
                    var items = _.clone(w88Mobile.v2.Slots.itemsByClub(club.providers, section));
                    // add published items
                    var publishedItems = _.filter(slotSvc.publishedItems(club), function (slot) {
                        return !_.isUndefined(slot.Section.Home);
                    });
                    items = _.concat(items, publishedItems);
                    items = w88Mobile.v2.Slots.sortGames(items, section);
                    games = _.slice(items, 0, w88Mobile.v2.Slots.getClubLimit(section));
                    pubsub.publish("displaySlotList", _self.setPushData({
                        games: games
                        , page: _self.page
                        , selector: "." + club.name + "-main"
                        , club: club
                        , showClubLabel: true
                    }));
                });

                break;

            case "index_search":

                break;
        }
    }

    function onDisplaySlotList(topic, data) {
        var club = data.club
        , games = data.games;
        var content = _.template(templateSvc.SlotList);
        var innerHtml = content({
            games: games
            , club: club
            , showClubLabel: (!_.isUndefined(data.showClubLabel) && data.showClubLabel == true)
        });

        if (!_.isEmpty(data.page.find(data.selector))) {
            data.page.find(data.selector).html($(innerHtml).html());
        } else {

            if (data._self.route != "index") {
                data._self.page.find(".main-content").append(innerHtml);
            } else {
                if (data.games.length == 0) return;

                if (!_.isEmpty(club.name)) {
                    var slotIndex = _.findIndex(w88Mobile.v2.Slots.clubs, function (slot) {
                        return slot.name == club.name
                    });

                    try {
                        var selector = "";
                        for (var i = _.clone(slotIndex) - 1 ; i >= 0; i--) {
                            var selectorClub = w88Mobile.v2.Slots.clubs[i].name;
                            if (!_.isEmpty($("div." + selectorClub + "-main"))) {
                                selector = "div." + selectorClub + "-main";
                                break;
                            }
                        }

                        if (!_.isEmpty(selector))
                            data._self.page.find(selector).after(innerHtml);
                        else
                            data._self.page.find(".main-content").prepend(innerHtml);

                    } catch (e) {
                        data._self.page.find(".main-content").append(innerHtml);
                    }
                }

            }
        }
    }

}