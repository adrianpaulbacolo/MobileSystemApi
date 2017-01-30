function JutaPay() {

    var gatewayId = "120280";

    var jutapay = {
        gatewayId: gatewayId,
        Deposit: deposit,
        Withdraw: withdraw,
    };

    return jutapay;

    // deposit
    function deposit(data, successCallback, completeCallback) {
        validate(data, "deposit");
        window.w88Mobile.Gateways.DefaultPayments.Send("/payments/" + window.w88Mobile.Gateways.JutaPay.gatewayId, "POST", successCallback, data, completeCallback);
    }

    // withdraw
    function withdraw(data, successCallback, completeCallback) {
    }

    function validate(data, method) {
        // @todo add validation here
        return;
    }

}

window.w88Mobile.Gateways.JutaPay = JutaPay();