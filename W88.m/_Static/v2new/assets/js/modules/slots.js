function Slots() {
    var slots = [];
    var filteredSlots = [];
    var clubLimit = 9;
    var section = "New";

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
        , itemsBySection: itemsBySection
        , clubLimit: clubLimit
        , clubs: clubs
        , club: {}
        , providers: providers
        , section: section
        , toggleSection: toggleSection
        , selectClub: selectClub
    }


    function send(url, method, data, success, error, complete) {

        var headers = {};

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

    function addItems(games) {
        w88Mobile.v2.Slots.items = _.union(w88Mobile.v2.Slots.items, games);
    }

    function itemsByClub(providers) {
        return _.filter(w88Mobile.v2.Slots.items, function (item) {
            var itemProviders = _.join(item.OtherProvider, ",").toLowerCase().split(",");
            return !_.isEmpty(_.intersection(itemProviders, providers));
        });
    }

    function itemsBySection(section) {
        return _.filter(w88Mobile.v2.Slots.items, function (item) {
            return _.includes(item.Section, section)
        });
    }

    function toggleSection(self, section) {
        if (!$(self).hasClass('active')) {
            $('#sectionTab').find('li').removeClass('active')
            $(self).addClass('active');
        }

        w88Mobile.v2.Slots.section = section;

        pubsub.publish("fetchSlots", w88Mobile.v2.Slots.club);
    }


    function selectClub(clubName) {
        var club = _.find(w88Mobile.v2.Slots.clubs, function (data) {
            return _.isEqual(data.name.toLowerCase(), clubName);
        });

        w88Mobile.v2.Slots.club = club;
    }
}

w88Mobile.v2.Slots = Slots();