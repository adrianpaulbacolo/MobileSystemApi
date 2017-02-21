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
        var url = w88Mobile.APIUrl + "/banks/vendor/" + gatewayId ;
        $.ajax({
            type: "GET",
            url: url,
            success: function (d) {
                banks = d.ResponseData;
                var defaultSelect = _w88_contents.translate("LABEL_SELECT_DEFAULT");
                $('select[id$="drpBanks"]').append($('<option>').text(defaultSelect).attr('value', '-1'));
                $('select[id$="drpBanks"]').val("-1").selectmenu("refresh");

                _.forOwn(banks, function (data) {
                    $('select[id$="drpBanks"]').append($('<option>').text(data.Text).attr('value', data.Value));
                });
            }
        });
    }
}

window.w88Mobile.Gateways.GNextPay = GNextPay();