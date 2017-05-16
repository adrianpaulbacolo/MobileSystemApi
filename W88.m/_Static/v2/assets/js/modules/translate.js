w88Mobile.Translate = translate;

function translate() {

    this.items = {};
    this.expiry = 1200000; // cache expiry
    this.hasi18n = !_.isUndefined($.i18n);

    this.init = function () {
        var _self = this;
        var contents = amplify.store(location.hostname + "_translations");
        if (_.isEmpty(contents) || contents.language != window.User.lang) {
            _self.fetch(window.User.lang);
            _self.fetch(window.User.lang, "/messages");
        } else {
            _self.items = contents;
            if (_self.hasi18n) {
                $.i18n().load(contents, window.User.lang);
                $("body").i18n();
            }
        }

    }

    this.fetch = function (lang, endpoint) {
        var _self = this;
        endpoint = (_.isUndefined(endpoint)) ? "/contents" : endpoint;
        var url = w88Mobile.APIUrl + endpoint;
        var headers = {
            'Token': window.User.token,
            'LanguageCode': lang
        };

        $.ajax({
            type: "GET",
            beforeSend: function () {
            },
            url: url,
            data: {},
            headers: headers,
            success: function (response) {
                if (_.isUndefined(response.ResponseData)) return;
                response.ResponseData.language = window.User.lang;

                var settings = _.clone(window.User.storageExpiration);
                settings.expires = _self.expiry;
                var contents = amplify.store(location.hostname + "_translations");
                if (!_.isEmpty(contents)) {
                    contents = _.assign(contents, response.ResponseData);
                } else {
                    contents = response.ResponseData;
                }
                _self.items = contents;
                if (_self.hasi18n) {
                    $.i18n().load(contents, lang);
                    $("body").i18n();
                }
                amplify.store(location.hostname + "_translations", contents, settings);
            },
            error: function () {
                console.log("unable to load contents");
            },
            complete: function () {
            }
        });
    }

    this.translate = function (key) {
        var _self = this;
        if (_.isEmpty(_self.items[key])) {
            return (_.isUndefined(_i18n_contents) || _.isUndefined(_i18n_contents[key])) ? key : _i18n_contents[key];
        }
        else return _self.items[key];
    }

}