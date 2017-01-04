w88Mobile.v2.ClubsCtrl = clubsCtrl;

function clubsCtrl(routeObj, slotSvc) {

    this.games = [];
    this.page = {};
    this.route = "";
    this.routeObj = routeObj;
    this.club = {};

    this.init = function () {
        var _self = this;
        pubsub.subscribe("filterSlotByClub", onFilterSlotByClub);
        _self.games = slotSvc.itemsByClub(_self.club.providers);
        getFilterOptions(_self.club);

        switch (_self.route) {
            case "club":
                _self.club.section = _.first(slotSvc.sections);
                pubsub.publish("loadClubTabBar", _self.setPushData({
                    section: _self.club.section
                    , ctrl: _self
                }));

                _self.filterClubSlots({
                    section: _self.club.section
                });
                break;

            case "club_search":

                break;
        }
    }

    this.filterClubSlots = function (filter) {

        var _self = this;
        var games = _.clone(_self.games);

        // filter for section
        if (!_.isUndefined(filter.section)) {
            games = _.filter(games, function (item) {
                var sections = _.join(item.Section, ",").toLowerCase().split(",");
                return _.includes(sections, filter.section.toLowerCase());
            });
        }

        if (!_.isUndefined(filter.form)) {
            games = _.filter(games, function (item) {

                var categories = _.join(item.Category, ",").toLowerCase().split(",");
                var hasCategory = (!_.isEqual(filter.form.category.toLowerCase(), "all")) ? _.includes(categories, filter.form.category.toLowerCase()) : true;
                var hasMinBet = (!_.isEqual(filter.form.minbet.toLowerCase(), "all")) ? _.isEqual(filter.form.minbet.toLowerCase(), item.MinBet) : true;
                var hasPL = (!_.isEqual(filter.form.playlines.toLowerCase(), "all")) ? _.isEqual(filter.form.playlines.toLowerCase(), item.Lines) : true;

                return hasCategory && hasMinBet && hasPL;

            });
        }


        pubsub.publish("displaySlotList", _self.setPushData({
            games: games
            , page: _self.page
            , selector: "." + _self.club.name + "-main"
            , club: _self.club
        }));
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

    /**
    * List of listeners below
    *
    **/

    function onFilterSlotByClub(topic, data) {
        var _self = data._self;

        if (_.isUndefined(data.club.section)) {
            data.club.section = _.first(slotSvc.sections);
        }

        var games = _.filter(_self.games, function (item) {
            var sections = _.join(item.Section, ",").toLowerCase().split(",");
            return _.includes(sections, data.club.section.toLowerCase());
        });

        pubsub.publish("displaySlotList", _self.setPushData({
            games: games
            , page: _self.page
            , selector: "." + _self.club.name + "-main"
            , club: _self.club
        }));

        getFilterOptions(data.club);
    }

    function getFilterOptions(club) {

        if (!w88Mobile.v2.Slots.clubFilter.club || (w88Mobile.v2.Slots.clubFilter.club && !_.isEqual(w88Mobile.v2.Slots.clubFilter.club, club))) {
            var defaultAll = { Text: w88Mobile.v2.Slots.translations.LABEL_ALL_DEFAULT, Value: "All" };

            w88Mobile.v2.Slots.clubFilter = {
                club: club,
                category: [defaultAll],
                minbet: [defaultAll],
                playlines: [defaultAll],
            };

            _.forEach(club.providers, function (provider) {
                var url = "/api/games/" + provider;

                w88Mobile.v2.Slots.send(url + "/category", "GET", {}, function (response) {

                    w88Mobile.v2.Slots.clubFilter.category = _.concat(w88Mobile.v2.Slots.clubFilter.category, response.ResponseData);

                    loadClubCategory();

                }, function () { });

                w88Mobile.v2.Slots.send(url + "/minbet", "GET", {}, function (response) {

                    var minBet = _.map(response.ResponseData, function (data) {
                        return { Text: data, Value: data };
                    });

                    w88Mobile.v2.Slots.clubFilter.minbet = _.concat(w88Mobile.v2.Slots.clubFilter.minbet, minBet);

                    loadClubMinBet();

                }, function () { });

                w88Mobile.v2.Slots.send(url + "/playlines", "GET", {}, function (response) {

                    var playlines = _.map(response.ResponseData, function (data) {
                        return { Text: data, Value: data };
                    });

                    w88Mobile.v2.Slots.clubFilter.playlines = _.concat(w88Mobile.v2.Slots.clubFilter.playlines, playlines);

                    loadClubPlayLines();
                }, function () { });

            })
        }
        else {
            loadClubCategory();
            loadClubMinBet();
            loadClubPlayLines();
        }
    }

    function loadClubCategory() {
        $('#clubCategory').empty();
        _.forEach(w88Mobile.v2.Slots.clubFilter.category, function (data) {
            if ($('#clubCategory').find('option[value="' + data.Value + '"]').length == 0) {
                $('#clubCategory').append($("<option></option>").attr("value", data.Value).text(data.Text))
            }
        })
    }

    function loadClubMinBet() {
        $('#clubMinBet').empty();
        _.forEach(w88Mobile.v2.Slots.clubFilter.minbet, function (data) {
            if ($('#clubMinBet').find('option[value="' + data.Value + '"]').length == 0) {
                $('#clubMinBet').append($("<option></option>").attr("value", data.Value).text(data.Text))
            }
        })
    }

    function loadClubPlayLines() {
        $('#clubPlayLines').empty();
        _.forEach(w88Mobile.v2.Slots.clubFilter.playlines, function (data) {
            if ($('#clubPlayLines').find('option[value="' + data.Value + '"]').length == 0) {
                $('#clubPlayLines').append($("<option></option>").attr("value", data.Value).text(data.Text))
            }
        })
    }
}