var _w88_fundtransfer = w88Mobile.FundTransfer = FundTransfer();

function FundTransfer() {

    var wallets = [];

    return {
        init: init,
        swap: swap,
        create: create
    };

    function init() {
        $("header .header-title").text(_w88_contents.translate("LABEL_HISTORY_FUND_TRANSFER"));

        pubsub.subscribe('wallets', onWalletsLoaded);

        _w88_wallets.init(true);

        _w88_validator.initiateValidator('#form1')

        $('.wallet-balance').on('show.bs.collapse', function () {
            $('#showBalance').text($.i18n('BUTTON_HIDE_BALANCE'));
        });

        $('.wallet-balance').on('hide.bs.collapse', function () {
            $('#showBalance').text($.i18n('BUTTON_SHOW_BALANCE'));
        });

        $('select[id$="drpTransferFrom"]').change(function () {
            changeWalletTo(this.value);
        });

        $('select[id$="drpTransferTo"]').change(function () {
            var option = $('option:selected', this).attr('balance');
            $(this).attr('balance', option);
        });

        $('#btnSwap').click(function () {
            swap();
        });
    }

    function swap() {
        var valTransferFrom = $('select[id$="drpTransferFrom"]').val();
        $('select[id$="drpTransferFrom"]').val($('select[id$="drpTransferTo"]').val()).change();
        $('select[id$="drpTransferTo"]').val(valTransferFrom).change();
    }

    function onWalletsLoaded(topic, data) {
        wallets = data;

        $('select[id$="drpTransferFrom"]').append($("<option></option>").attr("value", "-1").text($.i18n('LABEL_SELECT_DEFAULT')));
        _.forOwn(wallets, function (data) {
            $('select[id$="drpTransferFrom"]').append($('<option>').text(data.Name).attr('value', data.Id).attr('balance', stringToNumber(data.Balance)));
        });

        $('select[id$="drpTransferTo"]').append($("<option></option>").attr("value", "-1").text($.i18n('LABEL_SELECT_DEFAULT')));
        _.forOwn(wallets, function (data) {
            $('select[id$="drpTransferTo"]').append($('<option>').text(data.Name).attr('value', data.Id).attr('balance', stringToNumber(data.Balance)));
        });

        var transferto = getQueryStringValue("transferto");
        if (!_.isEmpty(transferto)) {
            $('select[id$="drpTransferFrom"]').val("0").change(); //main wallet
            $('select[id$="drpTransferTo"]').val(transferto).change();
        }
    }

    function changeWalletTo(selectedValue) {
        $('select[id$="drpTransferTo"] option').each(function () {
            if (!_.isEqual($(this).val(), "-1")) {
                if (_.isEqual($(this).val(), selectedValue)) {
                    $(this).hide();
                    $('select[id$="drpTransferTo"]').val('-1').change();
                }
                else {
                    $(this).show();
                }
            }
        });

        if (_.isEqual(selectedValue, '0'))
            $('.promocode').show();
        else
            $('.promocode').hide();
    }

    function create(data) {
        _w88_send("/payments/transfer", "POST", data, function (response) {
            switch (response.ResponseCode) {
                case 1:
                    if (_.isArray(response.ResponseMessage))
                        w88Mobile.Growl.shout(w88Mobile.Growl.bulletedList(response.ResponseMessage), function () {
                            window.location.reload();
                        });
                    else
                        w88Mobile.Growl.shout(response.ResponseMessage, function () {
                            window.location.reload();
                        });

                    break;

                default:
                    if (_.isArray(response.ResponseMessage))
                        w88Mobile.Growl.shout(w88Mobile.Growl.bulletedList(response.ResponseMessage));
                    else
                        w88Mobile.Growl.shout(response.ResponseMessage);
                    break;
            }
        });
    }
}