w88Mobile.v2.SlotsCtrl = slotsCtrl;

function slotsCtrl(routeObj, slotSvc, templateSvc) {

    this.games = [];
    this.page = {};
    this.route = "";
    this.routeObj = routeObj;

    this.init = function () {
        var _self = this;
        pubsub.subscribe("fetchSlotsByClub", onFetchSlotsByClub);
        pubsub.subscribe("slotItemsChanged", onSlotItemsChanged);
        pubsub.subscribe("displaySlotList", onDisplaySlotList);

        switch (_self.route) {
            case "index":

                _.forEach(w88Mobile.v2.Slots.clubs, function (club) {
                    pubsub.publish("fetchSlotsByClub", _self.setPushData({
                        club: club
                    }));
                });

                break;

            case "index_search":
                _self.games = w88Mobile.v2.Slots.items;
                break;
        }
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

    function onSlotItemsChanged(topic, data) {
        var _self = data._self;
        items = w88Mobile.v2.Slots.items;

        switch (_self.route) {
            case "index":
                var self = this;
                var clubs = _.filter(w88Mobile.v2.Slots.clubs, function (club) {
                    return _.indexOf(club.providers, data.provider.toLowerCase()) !== -1;
                });

                if (_.isEmpty(clubs)) return;

                _.forEach(clubs, function (club) {
                    var items = w88Mobile.v2.Slots.itemsByClub(club.providers, "Home");
                    games = _.slice(items, 0, w88Mobile.v2.Slots.clubLimit);
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

    function onFetchSlotsByClub(topic, data) {
        _.forEach(data.club.providers, function (provider) {
            w88Mobile.v2.Slots.get(provider, function (response) {
                w88Mobile.v2.Slots.addItems(response.ResponseData.Games, response.ResponseData.Provider);
                pubsub.publish("slotItemsChanged", data._self.setPushData({
                    provider: response.ResponseData.Provider
                }));
            }, function () { })
        });
        data._self.games = w88Mobile.v2.Slots.items;

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
            data._self.page.find(".main-content").append(innerHtml);
        }
    }

}