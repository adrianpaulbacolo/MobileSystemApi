w88Mobile.Loader = loader();

function loader() {
    var items = {};
    return {
        items: items
        , init: init
        , onStartLoadItem: onStartLoadItem
        , onStopLoadItem: onStopLoadItem
        , attachLoader: attachLoader
    }

    function init() {
        pubsub.subscribe('startLoadItem', onStartLoadItem);
        pubsub.subscribe('stopLoadItem', onStopLoadItem);
    }

    function onStartLoadItem(topic, data) {
        var key = "main";
        if (!_.isUndefined(data.selector) && !_.isEmpty(data.selector)) {
            key = data.selector;
        }

        var elem = $('body');

        if (key != "main") {
            elem = elem.find("." + key);
        }

        if (_.isEmpty(elem)) return;

        if (_.isEmpty(elem.find('div#divSplashContainer'))) {
            w88Mobile.Loader.attachLoader(elem, key);
        }

        if (_.isUndefined(w88Mobile.Loader.items[key])) w88Mobile.Loader.items[key] = [];

        w88Mobile.Loader.items[key].push(data);
    }

    function attachLoader(elem, key) {
        var mainClassKey = (key != "main") ? "loader inner" : "loader";
        elem.append(
            $('<div />', {
                style: '',
                class: mainClassKey,
                id: 'divSplashContainer'
            })
            .append(
                $('<div />', { style: '' })
                .append($('<div />', { class: 'spinner' })
                    .append($('<div />', { class: 'rect1' })).append("&nbsp;")
                    .append($('<div />', { class: 'rect2' })).append("&nbsp;")
                    .append($('<div />', { class: 'rect3' })).append("&nbsp;")
                    .append($('<div />', { class: 'rect4' })).append("&nbsp;")
                    )
                )
            );

    }

    function onStopLoadItem(topic, data) {
        var key = "main";
        if (!_.isUndefined(data.selector) && !_.isEmpty(data.selector)) {
            key = data.selector;
        }

        if (_.isUndefined(w88Mobile.Loader.items[key])) return;
        w88Mobile.Loader.items[key].pop();
        if (w88Mobile.Loader.items[key].length == 0) {
            $('div#divSplashContainer').remove();
        }
    }
}