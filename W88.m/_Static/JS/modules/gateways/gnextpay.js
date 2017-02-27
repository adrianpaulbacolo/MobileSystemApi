function GNextPay() {

    var gatewayId = "";

    var nextpay = {
        init: init
    };

    return nextpay;

    function init(gateway) {
        gatewayId = gateway;
        getBanks();
    }

    function getBanks() {
        _w88_paymentSvc.SendDeposit("/Banks/vendor/" + gatewayId, "GET", "", function (response) {
            var banks = response.ResponseData;
            var defaultSelect = _w88_contents.translate("LABEL_SELECT_DEFAULT");
            $('select[id$="drpBank"]').append($('<option>').text(defaultSelect).attr('value', '-1'));
            $('select[id$="drpBank"]').val("-1").selectmenu("refresh");

            _.forOwn(banks, function (data) {
                $('select[id$="drpBank"]').append($('<option>').text(data.Text).attr('value', data.Value));
            });
        });
    }
}

window.w88Mobile.Gateways.GNextPay = GNextPay();