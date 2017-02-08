w88Mobile.Loader = loader();

function loader() {
    var items = [];
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

        w88Mobile.Loader.items.push(data);
        var elem = $('body');

        if (_.isEmpty(elem.find('div#divSplashContainer'))) {
            w88Mobile.Loader.attachLoader(elem);
        }
    }

    function attachLoader(elem) {
        elem.append(
            $('<div />', {
                style: '',
                class: 'loader',
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
        w88Mobile.Loader.items.pop();
        if (w88Mobile.Loader.items.length == 0) {
            $('div#divSplashContainer').remove();
        }
    }
}