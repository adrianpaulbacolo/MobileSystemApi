window.w88Mobile.Gateway = Gateway;

function Gateway(paymentSvc) {

    this.methodId;

    this.send = function (resource, method, data, success, complete) {
        paymentSvc.SendDeposit(resource, method, data, success, complete);
    }

    this.deposit = function(data, successCallback, completeCallback) {
        var _self = this;
        _self.send("/payments/" + _self.methodId, "POST", data, successCallback, completeCallback);
    }

    this.withdraw = function(data, successCallback, completeCallback) {
        _self.send("/payments/" + _self.methodId, "POST", data, successCallback, completeCallback);
    }

    this.getUrlVars = function()
    {
        var vars = [], hash;
        var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < hashes.length; i++) {
            hash = hashes[i].split('=');
            vars.push(hash[0]);
            vars[hash[0]] = hash[1];
        }
        return vars;
    }

    this.shoutCallback = function () {
        window.close();
    }

    this.changeRoute = function(){
        if (history.pushState) {
            var newurl = window.location.protocol + "//" + window.location.host + window.location.pathname;
            window.history.pushState({ path: newurl }, '', newurl);
        }
    }

}