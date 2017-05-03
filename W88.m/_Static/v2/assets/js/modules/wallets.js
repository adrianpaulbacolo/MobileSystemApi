var _w88_wallets = w88Mobile.Wallets = Wallets();

function Wallets() {

    var wallet = [];
    var task = "";

    return {
        mainWalletInit: mainWalletInit
    }

    function mainWalletInit() {
        pubsub.subscribe('mainWalletLoaded', onMainWalletLoaded);

        getMainWallet();
    }

    function getMainWallet() {
        var resource = "/user/wallet/0";
        send(resource, "GET", {}, function (response) {
            if (_.isUndefined(response.ResponseData)) {
                console.log('Unable to fetch wallet.');
                return;
            }
            wallet = response.ResponseData;
            pubsub.publish("mainWalletLoaded");
        });
    }

    function onMainWalletLoaded() {

        var mainWalletTpl = _.template(
            $("script#mainWallet").html()
        );

        $("div.wallets").append(
            mainWalletTpl(wallet)
        );
    }

    function send(resource, method, data, success, error, complete) {

        if (_.isUndefined(error)) {
            error = function () {
                console.log("Error connecting to api");
            }
        }
        var selector = "";
        if (!_.isUndefined(data.selector)) {
            selector = _.clone(data.selector);
            delete data["selector"];
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
                pubsub.publish('startLoadItem', { selector: selector });
            },
            headers: headers,
            success: success,
            error: error,
            complete: function () {
                if (!_.isUndefined(complete)) complete();
                pubsub.publish('stopLoadItem', { selector: selector });
            }
        });
    }
}