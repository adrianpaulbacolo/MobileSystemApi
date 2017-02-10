var _w88_funds = w88Mobile.Funds = Funds();

function Funds() {

    var wallets = [];
    var task = "";

    return {
        init: init,
        wallets: getWallets
    }

    function init() {
        fetchWallets();
    }

    function send(resource, method, data, success, error, complete) {

        if (_.isUndefined(error)) {
            error = function () {
                console.log("Error connecting to api");
            }
        }

        var url = w88Mobile.APIUrl + resource;

        var headers = {
            'Token': window.User.token,
            'LanguageCode': window.User.lang
        };

        $.ajax({
            type: method,
            url: url,
            data: data,
            beforeSend: function () {
                pubsub.publish('startLoadItem', {});
            },
            headers: headers,
            success: success,
            error: error,
            complete: function () {
                if(!_.isUndefined(complete)) complete();
                pubsub.publish('stopLoadItem', {});
            }
        });
    }

    function fetchWallets() {
        var resource = "/user/wallets?isSelection=true";
        send(resource, "GET", {}, function (response) {
            if (_.isUndefined(response.ResponseData)) {
                console.log('Unable to fetch wallets.');
                return;
            }
            wallets = response.ResponseData;
            pubsub.publish("fundsLoaded");
        });
    }

    function getWallets() {
        return wallets;
    }

}