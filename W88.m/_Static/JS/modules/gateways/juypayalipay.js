function JuyPayAlipay() {

    var juypayalipay = {
        Initialize: init,
        Deposit: deposit,
        Withdraw: withdraw,
    };

    return juypayalipay;

    // deposit
    function deposit(data, successCallback, completeCallback) {
        validate(data, "deposit");
        window.w88Mobile.Gateways.DefaultPayments.Send("/payments/1202113", "POST", successCallback, data, completeCallback);
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

window.w88Mobile.Gateways.JuyPayAlipay = JuyPayAlipay();