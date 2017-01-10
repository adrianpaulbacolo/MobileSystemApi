w88Mobile.v2.Loader = loader();

function loader() {
    var items = [];
    return {
        items: items
        , init: init
        , onStartLoadItem: onStartLoadItem
        , onStopLoadItem: onStopLoadItem
    }

    function init() {
        pubsub.subscribe('startLoadItem', onStartLoadItem);
        pubsub.subscribe('stopLoadItem', onStopLoadItem);
    }

    function onStartLoadItem(topic, data) {

        w88Mobile.v2.Loader.items.push(data);

        if (_.isEmpty(w88Mobile.v2.Routes.currentPage().find('div#divSplashContainer'))) {
            w88Mobile.v2.Routes.currentPage().find('.main-content').append(
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
    }

    function onStopLoadItem(topic, data) {
        w88Mobile.v2.Loader.items.pop();
        if (w88Mobile.v2.Loader.items.length == 0) {
            w88Mobile.v2.Routes.currentPage().find('div#divSplashContainer').remove();
        }
    }
}