var _w88_loader = w88Mobile.Loader = loader();

_w88_loader.init();

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

        if (_.isEmpty(elem.find('div.css-loader'))) {
            w88Mobile.Loader.attachLoader(elem, key);
        }

        if (_.isUndefined(w88Mobile.Loader.items[key])) w88Mobile.Loader.items[key] = [];

        w88Mobile.Loader.items[key].push(data);
    }

    function attachLoader(elem, key) {
        if (key == "main") {
            elem.append(
                $('<section />', { class: 'css-loader-full' })
                .append($('<div />', { class: 'css-loader-container' })
                    .append($('<img />', { class: 'css-loader-logo', alt: "W88 Logo", src: '/_Static/images/v2/logo/logo-en.png' })).append("&nbsp;")
                    .append($('<div />', { class: 'css-loader' })
                        .append($('<div />')).append("&nbsp;")
                        .append($('<div />')).append("&nbsp;")
                        .append($('<div />')).append("&nbsp;")
                        )
                    )
                );
        } else {
            if (elem.width() < 100) {
                elem.append($('<div />', { class: 'css-loader  css-loader-small' })
                        .append($('<div />')).append("&nbsp;")
                        .append($('<div />')).append("&nbsp;")
                        .append($('<div />')).append("&nbsp;")
                        );
            } else {
                if (!elem.hasClass("css-loader-ready")) elem.addClass("css-loader-ready");

                elem.append($('<div />', { class: 'css-loader-center' })
                    .append($('<div />', { class: 'css-loader-container' })
                        .append($('<div />', { class: 'css-loader' })
                            .append($('<div />')).append("&nbsp;")
                            .append($('<div />')).append("&nbsp;")
                            .append($('<div />')).append("&nbsp;")
                            )
                        )
                );
            }
        }
    }

    function onStopLoadItem(topic, data) {
        var key = "main";
        if (!_.isUndefined(data.selector) && !_.isEmpty(data.selector)) {
            key = data.selector;
        }

        if (_.isUndefined(w88Mobile.Loader.items[key])) return;
        w88Mobile.Loader.items[key].pop();
        if (w88Mobile.Loader.items[key].length == 0) {
            if (key != "main") {
                $("." + key).find("[class^='css-loader']").remove();
            } else {
                $('section.css-loader-full').remove();
            }
        }
    }
}