window.w88Mobile.Gateway = Gateway;

function Gateway() {

    this.methodId;

    this.send = function(resource, method, data, beforeSend, success, complete) {
        var url = w88Mobile.APIUrl + resource;

        var headers = {
            'Token': window.User.token,
            'LanguageCode': window.User.lang
        };

        $.ajax({
            type: method,
            url: url,
            data: data,
            beforeSend: beforeSend,
            headers: headers,
            success: success,
            error: function () {
                console.log("Error connecting to api");
            },
            complete: complete
        });
    }

    this.deposit = function(data, successCallback, completeCallback) {
        var _self = this;
        _self.send("/payments/" + _self.methodId, "POST", data, function () { GPInt.prototype.ShowSplash() }, successCallback, completeCallback);
    }

    this.withdraw = function(data, successCallback, completeCallback) {
        _self.send("/payments/" + _self.methodId, "POST", data, function () { GPInt.prototype.ShowSplash() }, successCallback, completeCallback);
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