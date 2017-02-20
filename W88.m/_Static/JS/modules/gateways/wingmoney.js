window.w88Mobile.Gateways.WingMoney = WingMoney();
var _w88_wingmoney = window.w88Mobile.Gateways.WingMoney;

function WingMoney() {

    var wingmoney = Object.create(new w88Mobile.Gateway(_w88_paymentSvc));

    wingmoney.createDeposit = function () {
        var _self = this;
        var params = _self.getUrlVars();
        var data = {
            Amount: params.Amount,
            AccountName: params.AccountName,
            AccountNumber: params.AccountNumber,
            ReferenceId: params.ReferenceId,
            DepositDateTime: params.DepositDateTime,
        };

        _self.methodId = params.MethodId;
        _self.changeRoute();
        _self.deposit(data, function (response) {
            switch (response.ResponseCode) {
                case 1:
                    window.close();
                    break;
                default:
                    if (_.isArray(response.ResponseMessage))
                        w88Mobile.Growl.shout(w88Mobile.Growl.bulletedList(response.ResponseMessage));
                    else
                        w88Mobile.Growl.shout(response.ResponseMessage);

                    break;
            }
        },
        function () {
            GPInt.prototype.HideSplash();
        });
    }

    wingmoney.createWithdrawal = function () {
        var _self = this;
        var params = _self.getUrlVars();
        var data = {
            Amount: params.Amount,
            AccountName: params.AccountName,
            AccountNumber: params.AccountNumber,
        };

        _self.methodId = params.MethodId;
        _self.changeRoute();
        _self.withdraw(data, function (response) {
            switch (response.ResponseCode) {
                case 1:
                    window.close();
                    break;
                default:
                    if (_.isArray(response.ResponseMessage))
                        w88Mobile.Growl.shout(w88Mobile.Growl.bulletedList(response.ResponseMessage));
                    else
                        w88Mobile.Growl.shout(response.ResponseMessage);

                    break;
            }
        },
        function () {
            GPInt.prototype.HideSplash();
        });
    }

    return wingmoney;
}

