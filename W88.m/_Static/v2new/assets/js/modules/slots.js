w88Mobile.v2.Slots = Slots();

function Slots() {
    var slots = [];
    var filteredSlots = [];
    var clubLimit = 9;
    var sections = ["New", "Top", "All"];
    var sectionKeys = {
        "new": "LABEL_SLOTS_NEW"
        , "top": "LABEL_SLOTS_TOP"
        , "all": "LABEL_SLOTS_ALL"
    };

    clubFilterOptions = {};

    var providers = ["qt", "gpi", "mgs", "pt", "ctxm", "isb"];
    var clubs = [
        { name: "bravado", key: "LABEL_PRODUCTS_BRAVADO", label: "Club Bravado", providers: ["gpi"] }
        , { name: "massimo", key: "LABEL_PRODUCTS_MASSIMO", label: "Club Massimo", providers: ["mgs"] }
        , { name: "palazzo", key: "LABEL_PRODUCTS_PALAZZO", label: "Club Palazzo", providers: ["pt"] }
        , { name: "gallardo", key: "LABEL_PRODUCTS_GALLARDO", label: "Club Gallardo", providers: ["isb", "png"] }
        , { name: "apollo", key: "LABEL_PRODUCTS_APOLLO", label: "Club Apollo", providers: ["qt", "pp"] }
        , { name: "nuovo", key: "LABEL_PRODUCTS_NUOVO", label: "Club Nuovo", providers: ["gns", "pls"] }
        , { name: "divino", key: "LABEL_PRODUCTS_DIVINO", label: "Club Divino", providers: ["bs", "ctxm", "uc8"] }
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
        , getFilterOptions: getFilterOptions
        , showGameModal: showGameModal
        , sections: sections
        , clubFilterOptions: clubFilterOptions
        , fetchClubFilter: fetchClubFilter
        , getClubFilterOptions: getClubFilterOptions
        , translations: {}
        , sectionKeys: sectionKeys
        , send: send
        , publishedItems: publishedItems
        , sortGames: sortGames
        , formatReleaseDate: formatReleaseDate
    }


    function send(url, method, data, success, error, complete) {

        var headers = {
            'Token': w88Mobile.User.token,
            'LanguageCode': w88Mobile.User.lang
        };

        $.ajax({
            type: method,
            beforeSend: function () {
                pubsub.publish('startLoadItem', {});
            },
            url: url,
            data: data,
            headers: headers,
            success: success,
            error: error,
            complete: function () {
                pubsub.publish('stopLoadItem', {});
            }
        });

    }

    function getSlots(provider, success, error) {
        var data = {
            cashier: "v2/Funds.aspx",
            lobby: "_static/v2new/slots.html"
        }
        var url = "/api/games/" + provider;
        send(url, "GET", data, success, error);
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
                //var sections = _.join(item.Section, ",").toLowerCase().split(",");
                //return _.includes(sections, filter.section.toCamelCase());
                return !_.isUndefined(item.Section[filter.section.toCapitalize()]);
            });
        }

        if (!_.isUndefined(filter.form)) {
            items = _.filter(items, function (item) {

                var categories = _.join(item.Category, ",").toLowerCase().split(",");
                var hasCategory = (!_.isEqual(filter.form.category.toLowerCase(), "all")) ? _.includes(categories, filter.form.category.toLowerCase()) : true;
                var hasMinBet = (!_.isEmpty(filter.form.minbet) && !_.isEqual(filter.form.minbet.toLowerCase(), "all")) ? _.isEqual(filter.form.minbet.toLowerCase(), item.MinBet) : true;
                var hasPL = (!_.isEqual(filter.form.playlines.toLowerCase(), "all")) ? _.isEqual(filter.form.playlines.toLowerCase(), item.Lines) : true;
                var hasProvider = true;
                if (!_.isEmpty(filter.form.provider.toLowerCase())) {
                    var itemProviders = _.join(item.providers, ",").toLowerCase().split(",");
                    hasProvider = _.indexOf(itemProviders, filter.form.provider.toLowerCase()) != -1;
                }

                return hasCategory && hasMinBet && hasPL && hasProvider;

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
            else return !_.isUndefined(item.Section[section.toCapitalize()]) && hasClub;
        });
    }

    function publishedItems(club) {
        var items = _.filter(w88Mobile.v2.Slots.items, function (item) {
            hasClub = (!_.isEmpty(item.PublishedTo)) && (!_.isUndefined(item.PublishedTo[club.name.toCapitalize()]));
            return hasClub;
        });

        var publishedItems = [];
        _.forEach(items, function (item) {

            var topItem = !_.isEmpty(item.PublishedTo[club.name.toCapitalize()]["Top"]) ? item.PublishedTo[club.name.toCapitalize()]["Top"] : "";
            var homeItem = !_.isEmpty(item.PublishedTo[club.name.toCapitalize()]["Home"]) ? item.PublishedTo[club.name.toCapitalize()]["Home"] : "";
            var newItem = !_.isEmpty(item.PublishedTo[club.name.toCapitalize()]["New"]) ? item.PublishedTo[club.name.toCapitalize()]["New"] : "";

            var cloneItem = _.cloneDeep(item);
            var hasSection = !_.isUndefined(cloneItem.Section);

            if (_.isEmpty(topItem) && hasSection) {
                if (!_.isUndefined(cloneItem.Section["Top"])) delete cloneItem.Section["Top"];
            } else {
                cloneItem.Section.Top = topItem;
            }

            if (_.isEmpty(homeItem) && hasSection) {
                if (!_.isUndefined(cloneItem.Section["Home"])) delete cloneItem.Section["Home"];
            } else {
                cloneItem.Section.Home = homeItem;
            }

            if (_.isEmpty(newItem) && hasSection) {
                if (!_.isUndefined(cloneItem.Section["New"])) delete cloneItem.Section["New"];
            } else {
                cloneItem.Section.New = newItem;
            }

            publishedItems.push(cloneItem);
        });

        return publishedItems;
    }

    function getFilterOptions(club) {

        if (!w88Mobile.v2.Slots.clubFilter.club || (w88Mobile.v2.Slots.clubFilter.club && !_.isEqual(w88Mobile.v2.Slots.clubFilter.club, club.name))) {
            var defaultAll = { Text: _contents.translate("LABEL_ALL_DEFAULT") };

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

    function launchGame(id, club) {
        viewGame(id, club);
        $('#gameModal').modal('toggle');
    }

    function showGameModal(id, club) {

        var game = _.find(w88Mobile.v2.Slots.items, function (data) {
            return _.isEqual(data.Id, id);
        });

        $('#gameImage').attr('src', game.ImagePath);

        $('#gameTitle').text(game.TranslatedTitle.toUpperCase());

        // remove click action then re-attach
        $('#gameFunUrl').off("click");
        $('#gameFunUrl').html(_contents.translate("LABEL_TRY")).on("click", function (e) {
            e.preventDefault();
            launchGame(id, club);
        });
        $('#gameRegisterUrl').html(_contents.translate("BUTTON_REGISTER"));
        $('#gameLoginUrl').html(_contents.translate("BUTTON_LOGIN"));
        $('#gameRealUrl').html(_contents.translate("LABEL_PLAY"));

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

            $('#gameRealUrl').off("click");
            $('#gameRealUrl').on("click", function (e) {
                e.preventDefault();
                launchGame(id, club);
            });
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

    function fetchClubFilter(option, provider) {

        var url = "/api/games/" + provider + "/" + option;
        send(url, "GET", {}, function (response) {
            if (_.isUndefined(clubFilterOptions[provider])) clubFilterOptions[provider] = {};
            clubFilterOptions[provider][option] = response.ResponseData;
            pubsub.publish("updateFilterOptions", { option: option });

        }, function () {
        });
    }

    function getClubFilterOptions(option, providers) {
        var options = [];
        options.push({ Text: _contents.translate("LABEL_ALL_DEFAULT"), Value: "All" });
        _.forEach(providers, function (provider) {
            if (!_.isUndefined(clubFilterOptions[provider]) && !_.isUndefined(clubFilterOptions[provider][option])) {
                var data = [];
                switch (option) {
                    case 'minbet':
                        data = _.map(clubFilterOptions[provider][option], function (data) {
                            return { Text: data, Value: data };
                        });
                        break;
                    case 'playlines':
                        data = _.map(clubFilterOptions[provider][option], function (data) {
                            return { Text: data, Value: data };
                        });
                        break;
                    default:
                        data = clubFilterOptions[provider][option]
                        break;
                }
                options = _.concat(options, data);
            }
        });

        return options;
    }

    function sortGames(games, section) {
        // filter
        switch (section) {
            case "Home":
                games = _.orderBy(games, [function (game) {
                    var max = games.length;
                    try {
                        return (game.Section.Home > 0) ? parseInt(game.Section.Home) : max;
                    } catch (e) {
                        return max;
                    }
                }, getSortedReleaseDate], ["asc", "asc"]);
                break;
            case "Top":
                games = _.orderBy(games, [function (game) {
                    var max = games.length;
                    try {
                        return (game.Section.Top > 0) ? parseInt(game.Section.Top) : max;
                    } catch (e) {
                        return max;
                    }
                }, getSortedReleaseDate], ["asc", "asc"]);
                break;
            case "New":
                games = _.orderBy(games, [function (game) {
                    var max = games.length;
                    try {
                        return (game.Section.New > 0) ? parseInt(game.Section.New) : max;
                    } catch (e) {
                        return max;
                    }
                }, getSortedReleaseDate], ["asc", "asc"]);
                break;
        }

        return games;
    }

    // private function used for sorting date, should return integer
    function getSortedReleaseDate(game) {
        return (game.release instanceof Date) ? parseInt(game.release.getTime() / 1000) : parseInt(new Date().getTime() / 1000);
    }

    function formatReleaseDate(game) {
        try {
            if (!_.isEmpty(game.ReleaseDate)) {
                game.release = new Date([game.ReleaseDate.substr(0, 4), game.ReleaseDate.substr(4, 2), game.ReleaseDate.substr(6, 2)].join("-"));
            }
        } catch (e) {
            console.log("invalid date");
        }
    }
}