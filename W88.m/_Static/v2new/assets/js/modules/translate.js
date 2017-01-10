w88Mobile.v2.Translate = translate;

function translate() {

    this.items = {};

    this.init = function () {
        var _self = this;

        var url = "/api/contents";
        
        var headers = {
            'Token': w88Mobile.User.token,
            'LanguageCode': w88Mobile.User.lang
        };

        $.ajax({
            type: "GET",
            beforeSend: function () {
                pubsub.publish('startLoadItem', {});
            },
            url: url,
            data: {},
            headers: headers,
            success: function (response) {
                _self.items = response.ResponseData;
                pubsub.publish("contentsLoaded", {});
            },
            error: function () {
                console.log("unable to load contents");
            },
            complete: function () {
                pubsub.publish('stopLoadItem', {});
            }
        });
    }

    this.translate = function (key) {
        var _self = this; console.log(key);
        if (_.isEmpty(_self.items[key])) return key;
        else return _self.items[key];
    }

}