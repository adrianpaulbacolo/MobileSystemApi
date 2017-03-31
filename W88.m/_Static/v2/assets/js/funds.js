var _w88_funds = w88Mobile.Funds = Funds();

function Funds() {

    var wallets = [];
    var wallet = [];
    var task = "";

    return {
        init: init,
        wallets: getWallets,
        wallet: getWallet,
        mainWalletInit: mainWalletInit
    }

    function init(options, isSelection) {
        var isSelect = _.isUndefined(isSelection) ? false : true;

        var selector = _.isUndefined(options) ? "wallets" : options.selector;
        fetchWallets({ selector: selector }, isSelect);

        $("div.launch-deposit").on("click", function (e) {
            e.preventDefault();
            try {
                Native.openDeposit();
            } catch (e) {
                console.log("Native is not defined");
            }
        });
    }

    function mainWalletInit(options) {

        var selector = _.isUndefined(options) ? "wallet-value" : options.selector;
        getMainWallet({ selector: selector });
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

        $.ajax({
            type: method,
            url: url,
            data: data,
            beforeSend: function () {
                pubsub.publish('startLoadItem', {selector: selector});
            },
            success: success,
            error: error,
            complete: function () {
                if(!_.isUndefined(complete)) complete();
                pubsub.publish('stopLoadItem', { selector: selector });
            }
        });
    }

    function fetchWallets(data, isSelection) {

        var resource = "/user/wallets?isSelection=" + isSelection;
        send(resource, "GET", data, function (response) {
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

    function getWallet() {
        return wallet;
    }

    function getMainWallet(selector) {
        var resource = "/user/wallet/0";
        send(resource, "GET", selector, function (response) {
            if (_.isUndefined(response.ResponseData)) {
                console.log('Unable to fetch wallet.');
                return;
            }
            wallet = response.ResponseData;
            pubsub.publish("mainWalletLoaded");
        });
    }

}