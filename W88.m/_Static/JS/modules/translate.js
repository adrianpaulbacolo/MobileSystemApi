w88Mobile.Translate = translate;

function translate() {

    this.items = {};
    this.expiry = 1200000; // cache expiry

    this.init = function () {
        var _self = this;
        var contents = amplify.store(location.hostname + "_translations");
        if (_.isEmpty(contents) || contents.language != window.User.lang) {
            _self.fetch(window.User.lang);
        } else {
            _self.items = contents;
        }

    }
    
    this.fetch = function (lang) {
        var _self = this;
        var url = w88Mobile.APIUrl  + "/contents";
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
                response.ResponseData.language = window.User.lang;
                _self.items = response.ResponseData;

                var settings = _.clone(window.User.storageExpiration);
                settings.expires = _self.expiry;
                amplify.store(location.hostname + "_translations", response.ResponseData, settings);
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