w88Mobile.v2.ClubsCtrl = clubsCtrl;

function clubsCtrl(routeObj, slotSvc, templateSvc) {

    this.title = "";
    this.games = [];
    this.page = {};
    this.route = "";
    this.routeObj = routeObj;
    this.clubFilter = {
        category: [],
        minbet: [],
        playlines: [],
    }
    this.club = {};

    this.init = function () {
        var _self = this;
        pubsub.subscribe("filterSlotByClub", onFilterSlotByClub);
        pubsub.subscribe("updateFilterOptions", onUpdateFilterOptions);
        _self.games = slotSvc.itemsByClub(_self.club.providers);
        _self.games = _.concat(_self.games, slotSvc.publishedItems(_self.club));
        getFilterOptions(_self.club);

        if(!_.isEmpty(_self.club.key)) _self.title = _contents.translate(_self.club.key);

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

            games = slotSvc.sortGames(games, filter.section);

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

        _.forEach(club.providers, function (provider) {
            var hasProvider = !_.isUndefined(slotSvc.clubFilterOptions[provider]);
            if (!hasProvider || _.isUndefined(slotSvc.clubFilterOptions[provider]["category"])) {
                slotSvc.fetchClubFilter("category", provider);
            }
            // @todo how to make this configurable
            if (club.name == "bravado") {
                if (!hasProvider || _.isUndefined(slotSvc.clubFilterOptions[provider]["minbet"])) {
                    slotSvc.fetchClubFilter("minbet", provider);
                }
            }
            if (!hasProvider || _.isUndefined(slotSvc.clubFilterOptions[provider]["playlines"])) {
                slotSvc.fetchClubFilter("playlines", provider);
            }
        });
        loadClubCategory();
        // @todo how to make this configurable
        if (club.name == "bravado") {
            $('#clubMinBet').parent().show();
            loadClubMinBet();
        } else {
            $('#clubMinBet').parent().hide();
        }
        loadClubPlayLines();
        loadClubProviders(club);
    }

    function onUpdateFilterOptions(topic, data) {
        switch (data.option) {
            case "category":
                loadClubCategory();
                break;
            case "minbet":
                loadClubMinBet();
                break;
            case "playlines":
                loadClubPlayLines();
                break;
        }
    }

    function loadClubCategory() {
        $('#clubCategory').empty();
        var ctrl = routeObj.currentCtrl();
        var categories = slotSvc.getClubFilterOptions("category", ctrl.club.providers);
        _.forEach(categories, function (data) {
            if ($('#clubCategory').find('option[value="' + data.Value + '"]').length == 0) {
                $('#clubCategory').append($("<option></option>").attr("value", data.Value).text(data.Text))
            }
        })
    }

    function loadClubMinBet() {
        $('#clubMinBet').empty();
        var ctrl = routeObj.currentCtrl();
        var minbets = slotSvc.getClubFilterOptions("minbet", ctrl.club.providers);
        _.forEach(minbets, function (data) {
            if ($('#clubMinBet').find('option[value="' + data.Value + '"]').length == 0) {
                $('#clubMinBet').append($("<option></option>").attr("value", data.Value).text(data.Text))
            }
        })
    }

    function loadClubPlayLines() {
        $('#clubPlayLines').empty();
        var ctrl = routeObj.currentCtrl();
        var playlines = slotSvc.getClubFilterOptions("playlines", ctrl.club.providers);
        _.forEach(playlines, function (data) {
            if ($('#clubPlayLines').find('option[value="' + data.Value + '"]').length == 0) {
                $('#clubPlayLines').append($("<option></option>").attr("value", data.Value).text(data.Text))
            }
        })
    }

    function loadClubProviders(club) {
        $('#clubProviders').empty();
        $('#clubProviders').append($("<option></option>").attr("value", '').text(_contents.translate("LABEL_ALL_DEFAULT")));
        if (_.indexOf(["divino", "apollo", "gallardo"], club.name) != -1) {
            $('#clubProviders').parent().show();
            _.forEach(club.providers, function (data) {
                if ($('#clubProviders').find('option[value="' + data.toUpperCase() + '"]').length == 0) {
                    $('#clubProviders').append($("<option></option>").attr("value", data.toUpperCase()).text(data.toUpperCase()))
                }
            })
        } else {
            $('#clubProviders').parent().hide();
        }
    }
}