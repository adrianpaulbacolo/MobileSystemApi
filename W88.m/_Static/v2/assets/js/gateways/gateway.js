window.w88Mobile.Gateway = Gateway;

function Gateway(paymentSvc) {
    this.switchLineExpiration = { expires: 50000 };
    this.methodId;

    this.send = function (resource, method, data, success, complete) {
        paymentSvc.Send(resource, method, data, success, complete);
    }

    this.deposit = function (data, successCallback, completeCallback) {
        var _self = this;

        if (_.includes(_w88_paymentSvcV2.AutoRouteIds, _self.methodId)) {

            var prevMethodIds = amplify.store(w88Mobile.Keys.switchLineSettings);
            data.Id = prevMethodIds;

            _self.send("/payments/autoroute/" + _self.methodId, "POST", data, function (response) {
                switch (response.ResponseCode) {
                    case 1:
                        var methodId = response.ResponseData.MethodId;

                        if (prevMethodIds && !response.ResponseData.ResetPreviousList)
                            amplify.store(w88Mobile.Keys.switchLineSettings, prevMethodIds + "," + methodId, _self.switchLineExpiration);
                        else
                            amplify.store(w88Mobile.Keys.switchLineSettings, methodId, _self.switchLineExpiration);
                        data.Bank = response.ResponseData.Bank;
                        _self.send("/payments/" + methodId, "POST", data, successCallback, completeCallback);
                        break;
                    default:
                        if (_.isArray(response.ResponseMessage))
                            w88Mobile.Growl.shout(w88Mobile.Growl.bulletedList(response.ResponseMessage), _self.shoutCallback);
                        else
                            w88Mobile.Growl.shout(response.ResponseMessage, _self.shoutCallback);
                        break;
                }
            }, function () { });
        }
        else {
            _self.send("/payments/" + _self.methodId, "POST", data, successCallback, completeCallback);
        }
    }

    this.offlineDeposit = function (form, data, successCallback, completeCallback) {
        var _self = this;

        if (_.isUndefined(successCallback)) {
            _self.send("/payments/" + _self.methodId, "POST", data, function (response) {
                switch (response.ResponseCode) {
                    case 1:
                        w88Mobile.Growl.shout("<p>" + response.ResponseMessage + "</p> <p>" + _w88_contents.translate("LABEL_TRANSACTION_ID") + ": " + response.ResponseData.TransactionId + "</p>", function () {
                            _self.formReset(form);
                        });

                        break;
                    default:
                        if (_.isArray(response.ResponseMessage))
                            w88Mobile.Growl.shout(w88Mobile.Growl.bulletedList(response.ResponseMessage));
                        else
                            w88Mobile.Growl.shout(response.ResponseMessage);
                        break;
                }
            }, function () { });
        }
        else {
            _self.send("/payments/" + _self.methodId, "POST", data, successCallback, completeCallback);
        }
    }

    this.withdraw = function (data, successCallback, completeCallback) {
        var _self = this;

        if (_.isUndefined(successCallback)) {
            _self.send("/payments/" + _self.methodId, "POST", data, function (response) {
                switch (response.ResponseCode) {
                    case 1:
                        w88Mobile.Growl.shout("<p>" + response.ResponseMessage + "</p> <p>" + _w88_contents.translate("LABEL_TRANSACTION_ID") + ": " + response.ResponseData.TransactionId + "</p>", function () {
                            window.location = "/v2/Withdrawal/";
                        });

                        break;
                    default:
                        if (_.isArray(response.ResponseMessage))
                            w88Mobile.Growl.shout(w88Mobile.Growl.bulletedList(response.ResponseMessage));
                        else
                            w88Mobile.Growl.shout(response.ResponseMessage);
                        break;
                }
            }, function () { });
        }
        else {
            _self.send("/payments/" + _self.methodId, "POST", data, successCallback, completeCallback);
        }
    }

    this.getUrlVars = function () {
        var vars = [], hash;
        var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < hashes.length; i++) {
            hash = hashes[i].split('=');
            vars.push(hash[0]);
            vars[hash[0]] = decodeURIComponent(hash[1].replace(/\+/g, '%20'));
        }
        return vars;
    }

    this.shoutCallback = function () {
        window.close();
    }

    this.changeRoute = function () {
        if (history.pushState) {
            var newurl = window.location.protocol + "//" + window.location.host + window.location.pathname;
            window.history.pushState({ path: newurl }, '', newurl);
        }
    }

    this.formReset = function (form) {
        if (!_.isUndefined(form)) _.first(form).reset();

        _.forEach($('[data-date-box]'), function (item, index) {
            $(item).datebox('setTheDate', new Date());
        });
    }
}