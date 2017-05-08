var _w88_funds = w88Mobile.Funds = Funds();

function Funds() {
    return {
        init: init,
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

        $('#deposit').text(_w88_contents.translate("LABEL_FUNDS_DEPOSIT"));
        $('#transfer').text(_w88_contents.translate("LABEL_FUNDS_TRANSFER"));
        $('#withdraw').text(_w88_contents.translate("LABEL_FUNDS_WIDRAW"));
        $('#history').text(_w88_contents.translate("LABEL_FUNDS_HISTORY"));
    }

    function fetchWallets(data, isSelection) {
        pubsub.subscribe('fundsLoaded', onFundsLoaded);

        var resource = "/user/wallets?isSelection=" + isSelection;
        send(resource, "GET", data, function (response) {
            if (_.isUndefined(response.ResponseData)) {
                console.log('Unable to fetch wallets.');
                return;
            }
            pubsub.publish("fundsLoaded", response.ResponseData);
        });
    }

    function onFundsLoaded(topic, data) {

        var mainWalletTpl = _.template(
            $("script#mainWallet").html()
        );

        var walletList = _.template(
            $("script#walletMenu").html()
        );

        $("div.wallets").append(
            mainWalletTpl(_.first(data))
        );

        $("div.wallets").append(
            walletList({ wallets: data })
        );
    }

    function mainWalletInit(options) {
        pubsub.subscribe('mainWalletLoaded', onMainWalletLoaded);

        var selector = _.isUndefined(options) ? "wallet-value" : options.selector;
        getMainWallet({ selector: selector });
    }

    function getMainWallet(selector) {
        var resource = "/user/wallet/0";
        send(resource, "GET", selector, function (response) {
            if (_.isUndefined(response.ResponseData)) {
                console.log('Unable to fetch wallet.');
                return;
            }
            pubsub.publish("mainWalletLoaded", response.ResponseData);
        });
    }

    function onMainWalletLoaded(topic, data) {

        var mainWalletTpl = _.template(
            $("script#mainWallet").html()
        );

        $("div.wallets").append(
            mainWalletTpl(data)
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
}