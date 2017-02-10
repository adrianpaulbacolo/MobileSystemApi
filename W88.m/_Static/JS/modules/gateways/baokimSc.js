function BaokimScratchCard() {

    var defaultSelect = "";
    var telcos = "";

    var baokim = {
        SetFee: setFee,
        SetDenom: setDenom,
        Initialize: init,
    };

    return baokim;

    function init() {
        setTranslations();
        function setTranslations() {
            if (_w88_contents.translate("LABEL_PAYMENT_NOTE") != "LABEL_PAYMENT_NOTE") {
                $('label[id$="lblDepositAmount"]').text(_w88_contents.translate("LABEL_CARD_AMOUNT"));
                $('label[id$="lblBanks"]').text(_w88_contents.translate("LABEL_TELCO_NAME"));
                $('label[id$="lblPin"]').text(_w88_contents.translate("LABEL_CARD_PIN"));
                $('label[id$="lblCardSerialNo"]').text(_w88_contents.translate("LABEL_CARD_SERIAL"));

                sessionStorage.setItem("indicator", _w88_contents.translate("LABEL_INDICATOR_MSG"));
                $('p[id$="IndicatorMsg"]').html(sessionStorage.getItem("indicator"));

                defaultSelect = _w88_contents.translate("LABEL_SELECT_DEFAULT");

                getBanks();

            } else {
                window.setInterval(function () {
                    setTranslations();
                }, 500);
            }
        }
    }

    function getBanks() {
        var url = w88Mobile.APIUrl + "/payments/baokindenom/";

        $.ajax({
            type: "GET",
            url: url,
            success: function (d) {
                telcos = d.ResponseData.Telcos;

                $('select[id$="ContentPlaceHolder1_ContentPlaceHolder2_drpBanks"]').append($('<option>').text(defaultSelect).attr('value', '-1'));
                $('select[id$="ContentPlaceHolder1_ContentPlaceHolder2_drpBanks"]').val("-1").selectmenu("refresh");

                _.forOwn(d.ResponseData.Telcos, function (data) {
                    $('select[id$="ContentPlaceHolder1_ContentPlaceHolder2_drpBanks"]').append($('<option>').text(data.Name).attr('value', data.Id));
                });


                $('select[id$="ContentPlaceHolder1_ContentPlaceHolder2_drpAmount"]').append($('<option>').text(defaultSelect).attr('value', '-1'));
                $('select[id$="ContentPlaceHolder1_ContentPlaceHolder2_drpAmount"]').val("-1").selectmenu("refresh");
            }
        });
    }

    function setDenom(selectedValue) {
        var telco = _.find(telcos, function (data) {
            return data.Id == selectedValue;
        });

        $('select[id$="ContentPlaceHolder1_ContentPlaceHolder2_drpAmount"]').empty();

        $('select[id$="ContentPlaceHolder1_ContentPlaceHolder2_drpAmount"]').append($('<option>').text(defaultSelect).attr('value', '-1'));
        $('select[id$="ContentPlaceHolder1_ContentPlaceHolder2_drpAmount"]').val("-1").selectmenu("refresh");

        _.forOwn(telco.Denominations, function (data) {
            $('select[id$="ContentPlaceHolder1_ContentPlaceHolder2_drpAmount"]').append($('<option>').text(data.Text).attr('value', data.Value));
        });
    }

    function setFee(selectedValue) {

        var fee = "";

        _.forEach(telcos, function (i) {
            if (i.Id == selectedValue) {
                fee = i.Fee;
            }
        });

        $('p[id$="IndicatorMsg"]').html(sessionStorage.getItem("indicator") + fee);
    }

}

window.w88Mobile.Gateways.BaokimScratchCard = BaokimScratchCard();