w88Mobile.v2.Slots = Slots();

function Slots() {
    var slots = [];
    var filteredSlots = [];
    var clubLimit = 9;
    var sections = ["New", "Top", "All"];

    var providers = ["qt", "gpi", "mgs", "pt", "ctxm", "isb"];
    var clubs = [
        { name: "apollo", label: "Club Apollo", providers: ["qt", "pp", "gpi"] }
        , { name: "bravado", label: "Club Bravado", providers: ["gpi"] }
        , { name: "massimo", label: "Club Massimo", providers: ["mgs", "gpi"] }
        , { name: "palazzo", label: "Club Palazzo", providers: ["pt", "gpi"] }
        , { name: "divino", label: "Club Divino", providers: ["bs", "ctxm", "uc8", "gpi"] }
        , { name: "gallardo", label: "Club Gallardo", providers: ["isb", "png", "gpi"] }
    ];

    return {
        get: getSlots
        , items: slots
        , addItems: addItems
        , itemsByClub: itemsByClub
        , clubLimit: clubLimit
        , clubs: clubs
        , providers: providers
        , filterSlots: filterSlots
        , getTranslations: getTranslations
        , getFilterOptions: getFilterOptions
        , showGameModal: showGameModal
        , sections: sections
        , clubFilter: {}
        , translations: {}
        , send: send
    }


    function send(url, method, data, success, error, complete) {

        var headers = {
            'Token': w88Mobile.User.token,
            'LanguageCode': w88Mobile.User.lang
        };

        $.ajax({
            type: method,
            url: url,
            data: data,
            headers: headers,
            success: success,
            error: error,
            complete: complete
        });

    }

    function getSlots(provider, success, error) {
        var url = "/api/games/" + provider;
        send(url, "GET", {}, success, error);
    }

    function addItems(games, provider) {
        _.forEach(games, function (game) {
            var hasItem = _.findIndex(w88Mobile.v2.Slots.items, function (item) {
                return item.Id == game.Id;
            });
            if (hasItem == -1) {
                game.providers = _.clone(game.OtherProvider);
                if (!_.isUndefined(provider)) {
                    game.providers.push(provider);
                }
                w88Mobile.v2.Slots.items.push(game);
            }
        });
    }

    function filterSlots(filter, items) {

        // filter for section
        if (!_.isUndefined(filter.section)) {
            items = _.filter(items, function (item) {
                var sections = _.join(item.Section, ",").toLowerCase().split(",");
                return _.includes(sections, filter.section.toLowerCase());
            });
        }

        if (!_.isUndefined(filter.form)) {
            items = _.filter(items, function (item) {

                var categories = _.join(item.Category, ",").toLowerCase().split(",");
                var hasCategory = (!_.isEqual(filter.form.category.toLowerCase(), "all")) ? _.includes(categories, filter.form.category.toLowerCase()) : true;
                var hasMinBet = (!_.isEqual(filter.form.minbet.toLowerCase(), "all")) ? _.isEqual(filter.form.minbet.toLowerCase(), item.MinBet) : true;
                var hasPL = (!_.isEqual(filter.form.playlines.toLowerCase(), "all")) ? _.isEqual(filter.form.playlines.toLowerCase(), item.Lines) : true;

                return hasCategory && hasMinBet && hasPL;

            });
        }

        if (!_.isUndefined(filter.title)) {
            items = _.filter(items, function (item) {
                if (_.isEmpty(filter.title)) return false;
                return _.includes(item.TranslatedTitle.toLowerCase(), filter.title.toLowerCase());
            });
        }

        return items;
    }


    function itemsByClub(providers, section) {
        return _.filter(w88Mobile.v2.Slots.items, function (item) {
            var itemProviders = _.join(item.providers, ",").toLowerCase().split(",");
            var hasClub = !_.isEmpty(_.intersection(itemProviders, providers));
            if (_.isUndefined(section)) return hasClub;
            else return _.includes(item.Section, section) && hasClub;
        });
    }

    function getTranslations(success, error) {
        var url = "/api/contents";
        send(url, "GET", {}, success, error);
    }

    function getFilterOptions(club) {

        if (!w88Mobile.v2.Slots.clubFilter.club || (w88Mobile.v2.Slots.clubFilter.club && !_.isEqual(w88Mobile.v2.Slots.clubFilter.club, club.name))) {
            var defaultAll = { Text: w88Mobile.v2.Slots.translations.LABEL_ALL_DEFAULT, Value: "All" };

            w88Mobile.v2.Slots.clubFilter = {
                club: club.name,
                category: [defaultAll],
                minbet: [defaultAll],
                playlines: [defaultAll],
            };

            _.forEach(club.providers, function (provider) {
                var url = "/api/games/" + provider;

                send(url + "/category", "GET", {}, function (response) {

                    w88Mobile.v2.Slots.clubFilter.category = _.concat(w88Mobile.v2.Slots.clubFilter.category, response.ResponseData);

                    loadClubCategory();

                }, function () { });

                send(url + "/minbet", "GET", {}, function (response) {

                    var minBet = _.map(response.ResponseData, function (data) {
                        return { Text: data, Value: data };
                    });

                    w88Mobile.v2.Slots.clubFilter.minbet = _.concat(w88Mobile.v2.Slots.clubFilter.minbet, minBet);

                    loadClubMinBet();

                }, function () { });

                send(url + "/playlines", "GET", {}, function (response) {

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

    function showGameModal(id) {

        var game = _.find(w88Mobile.v2.Slots.items, function (data) {
            return _.isEqual(data.Id, id);
        });

        $('#gameImage').attr('src', game.ImagePath);

        $('#gameTitle').text(game.TranslatedTitle.toUpperCase());

        $('#gameFunUrl').attr('href', game.FunUrl);

        var isPT = _.indexOf(game.providers, "PT") != -1;
        if (isPT) {
            $('#gameFunUrl').hide();
        } else {
            $('#gameFunUrl').show();
        }

        if (w88Mobile.User.token) {
            $('#gameRealUrl').show();
            $('#gameRegisterUrl').hide();
            $('#gameLoginUrl').hide();

            $('#gameRealUrl').attr('href', game.RealUrl);
        }
        else {

            var loginUrl = "/_Secure/Login.aspx"
            var registerUrl = "/_Secure/Register.aspx"

            $('#gameRealUrl').hide();
            $('#gameRegisterUrl').show();
            $('#gameLoginUrl').show();

            $('#gameRegisterUrl').attr('href', loginUrl);
            $('#gameLoginUrl').attr('href', registerUrl);
        }

        $('#gameModal').modal('toggle');
    }
}