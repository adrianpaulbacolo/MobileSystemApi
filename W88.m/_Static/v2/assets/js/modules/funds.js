var _w88_funds = w88Mobile.Funds = Funds();

function Funds() {
    return {
        init: init,
    }

    function init() {
        _w88_wallets.init(false);

        $("div.launch-deposit").on("click", function (e) {
            e.preventDefault();
            try {
                Native.openDeposit();
            } catch (e) {
                console.log("Native is not defined");
            }
        });
    }
}