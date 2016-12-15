function Slots() {
    var slots = [];
    var filteredSlots = [];
    var clubLimit = 9;

    var providers = ["qt", "gpi", "mgs", "pt", "ctxm", "isb"];
    var clubs = [
        { name: "apollo", label: "Club Apollo", providers: ["qt", "pp", "gpi"] }
        , { name: "bravado", label: "Club Bravado", providers: ["gpi"] }
        , { name: "massimo", label: "Club Massimo", providers: ["mgs", "gpi"] }
        , { name: "palazzo", label: "Club Palazzo", providers: ["pt", "gpi"] }
        , { name: "divino", label: "Club Divino", providers: ["ctxm", "bs", "gpi"] }
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

}

w88Mobile.v2.Slots = Slots();