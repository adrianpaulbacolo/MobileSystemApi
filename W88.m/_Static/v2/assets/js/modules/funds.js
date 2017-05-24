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
        _w88_send(resource, "GET", data, function (response) {
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
        _w88_send(resource, "GET", selector, function (response) {
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
}