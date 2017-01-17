function NineVPayAlipay() {

    var ninevpayalipay = {
        Initialize: init,
        Deposit: deposit,
        Withdraw: withdraw,
    };

    return ninevpayalipay;

    // deposit
    function deposit(data, successCallback, completeCallback) {
        validate(data, "deposit");
        window.w88Mobile.Gateways.DefaultPayments.Send("/payments/1202105", "POST", successCallback, data, completeCallback);
    }

    // withdraw
    function withdraw(data, successCallback, completeCallback) {
    }

    function validate(data, method) {
        // @todo add validation here
        return;
    }

    function init() {
    }

}

window.w88Mobile.Gateways.NineVPayAlipay = NineVPayAlipay();