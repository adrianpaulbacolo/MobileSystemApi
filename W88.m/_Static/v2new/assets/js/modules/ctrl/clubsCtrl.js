w88Mobile.v2.ClubsCtrl = clubsCtrl;

function clubsCtrl(routeObj, slotSvc, templateSvc) {

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
                _self.setActiveSection(_self.club.section);
                _self.filterClubSlots({
                    section: _self.club.section
                });
                break;

            case "club_search":

                break;

            case "club_filter":
                break;
        }
    }

    this.filterClubSlots = function (filter) {

        var _self = this;

        if (!_.isUndefined(filter.section) && filter.section.toLowerCase() == 'all') {

            _self.page.find(".main-content").children().remove();
            pubsub.publish('searchSlots', {
                club: _self.club
                , games: _self.games
            });

        } else {

            _self.page.find(".main-content").children().remove();
            var games = slotSvc.filterSlots(filter, _.clone(_self.games));
            pubsub.publish("displaySlotList", _self.setPushData({
                games: games
                , page: _self.page
                , selector: "." + _self.club.name + "-main"
                , club: _self.club
            }));

        }
    }

    this.filterSearchSlots = function (filter) {

        var _self = this;
        var games = slotSvc.filterSlots(filter, _.clone(_self.games));

        pubsub.publish('searchSlots', {
            club: _self.club
            , games: games
        });
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

        if (!slotSvc.clubFilter.club || (slotSvc.clubFilter.club && !_.isEqual(slotSvc.clubFilter.club, club))) {
            var defaultAll = { Text: slotSvc.translations.LABEL_ALL_DEFAULT, Value: "All" };

            slotSvc.clubFilter = {
                club: club,
                category: [defaultAll],
                minbet: [defaultAll],
                playlines: [defaultAll],
            };

            _.forEach(club.providers, function (provider) {
                var url = "/api/games/" + provider;

                slotSvc.send(url + "/category", "GET", {}, function (response) {

                    slotSvc.clubFilter.category = _.concat(slotSvc.clubFilter.category, response.ResponseData);

                    loadClubCategory();

                }, function () { });

                slotSvc.send(url + "/minbet", "GET", {}, function (response) {

                    var minBet = _.map(response.ResponseData, function (data) {
                        return { Text: data, Value: data };
                    });

                    slotSvc.clubFilter.minbet = _.concat(slotSvc.clubFilter.minbet, minBet);

                    loadClubMinBet();

                }, function () { });

                slotSvc.send(url + "/playlines", "GET", {}, function (response) {

                    var playlines = _.map(response.ResponseData, function (data) {
                        return { Text: data, Value: data };
                    });

                    slotSvc.clubFilter.playlines = _.concat(slotSvc.clubFilter.playlines, playlines);

                    loadClubPlayLines();
                }, function () { });

            })
        }
        else {
            loadClubCategory();
            loadClubMinBet();
            loadClubPlayLines();
        }
        loadClubProviders(club);
    }

    function loadClubCategory() {
        $('#clubCategory').empty();
        _.forEach(slotSvc.clubFilter.category, function (data) {
            if ($('#clubCategory').find('option[value="' + data.Value + '"]').length == 0) {
                $('#clubCategory').append($("<option></option>").attr("value", data.Value).text(data.Text))
            }
        })
    }

    function loadClubMinBet() {
        $('#clubMinBet').empty();
        _.forEach(slotSvc.clubFilter.minbet, function (data) {
            if ($('#clubMinBet').find('option[value="' + data.Value + '"]').length == 0) {
                $('#clubMinBet').append($("<option></option>").attr("value", data.Value).text(data.Text))
            }
        })
    }

    function loadClubPlayLines() {
        $('#clubPlayLines').empty();
        _.forEach(slotSvc.clubFilter.playlines, function (data) {
            if ($('#clubPlayLines').find('option[value="' + data.Value + '"]').length == 0) {
                $('#clubPlayLines').append($("<option></option>").attr("value", data.Value).text(data.Text))
            }
        })
    }

    function loadClubProviders(club) {
        $('#clubProviders').empty();
        if (_.indexOf(["divino", "apollo", "gallardo"], club.name) != -1) {
            $('#clubProviders').parent().show();
            _.forEach(club.providers, function (data) {
                if ($('#clubProviders').find('option[value="' + data.toUpperCase() + '"]').length == 0) {
                    $('#clubProviders').append($("<option></option>").attr("value", data.toUpperCase()).text(data.toUpperCase()))
                }
            })
        } else {
            $('#clubProviders').parent().hide();
            $('#clubProviders').append($("<option></option>").attr("value", '').text("All"));
        }
    }
}