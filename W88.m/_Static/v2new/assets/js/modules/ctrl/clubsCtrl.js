w88Mobile.v2.ClubsCtrl = clubsCtrl;

function clubsCtrl(routeObj) {

    this.items = [];
    this.page = {};
    this.route = "";
    this.routeObj = routeObj;
    this.club = {};

    this.init = function () {
        var _self = this;
        pubsub.subscribe("filterSlotByClub", onFilterSlotByClub);
        pubsub.subscribe("loadClubTabBar", onLoadClubTabBar);

        switch (_self.route) {
            case "club":
                pubsub.publish("loadClubTabBar");

                pubsub.publish("filterSlotByClub", _self.setPushData({
                    club: this.club
                }));

                break;

            case "club_search":

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

    /**
    * List of listeners below
    *
    **/

    function onLoadClubTabBar(topic, data) {
        $.get('assets/templates/tabs.html', function (template) {
            var content = _.template(template);
            var innerHtml = content({
                sections: w88Mobile.v2.Slots.sections,
                section: _.first(w88Mobile.v2.Slots.sections)
            });
            if (!_.isEmpty($('.filter-bar'))) {
                //w88Mobile.v2.Routes.currentPage().find('.' + club.name + '-main').html($(innerHtml).html());
            } else {
                $(".header").after(innerHtml);
            }
        }, 'html');
    }

    function onFilterSlotByClub(topic, data) {
        var _self = data._self;

        if (!_.isUndefined(data.club)) {
            _self.items = w88Mobile.v2.Slots.itemsByClub(data.club.providers);
        }

        if (_.isUndefined(data.club.section)) {
            data.club.section = _.first(w88Mobile.v2.Slots.sections);
        }

        _self.items = _.filter(_self.items, function (item) {
            var sections = _.join(item.Section, ",").toLowerCase().split(",");
            return _.includes(sections, data.club.section.toLowerCase());
        });

        pubsub.publish("displaySlotList", _self.setPushData({
            games: _self.items
            , page: _self.page
            , selector: "." + data.club.name + "-main"
            , club: data.club
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