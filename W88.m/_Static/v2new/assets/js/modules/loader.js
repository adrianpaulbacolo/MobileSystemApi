w88Mobile.v2.Loader = loader();

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

        w88Mobile.v2.Loader.items.push(data);
        var elem = w88Mobile.v2.Routes.currentPage();
        if (_.isEmpty(elem)) {
            elem = $('body');
        } else {
            elem = elem.find('.main-content');
        }
        if (_.isEmpty(elem.find('div#divSplashContainer'))) {
            w88Mobile.v2.Loader.attachLoader(elem);
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
        w88Mobile.v2.Loader.items.pop();
        if (w88Mobile.v2.Loader.items.length == 0) {
            $('div#divSplashContainer').remove();
        }
    }
}