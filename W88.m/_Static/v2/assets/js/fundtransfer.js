var _w88_fundtransfer = w88Mobile.FundTransfer = FundTransfer();

function FundTransfer() {

    var wallets = [];

    return {
        init: init,
        changeWalletTo: changeWalletTo,
        swap: swap,
        create : create
};

    function init() {

        setTranslations();

        function setTranslations() {
            if (_w88_contents.translate("LABEL_FUNDS_TRANSFER") != "LABEL_FUNDS_TRANSFER") {
                $('label[id$="lblTransferFrom"]').text(_w88_contents.translate("LABEL_FUNDS_TRANSFER") + " " + _w88_contents.translate("LABEL_FROM"));
                $('label[id$="lblTransferTo"]').text(_w88_contents.translate("LABEL_FUNDS_TRANSFER") + " " + _w88_contents.translate("LABEL_TO"));
                $('label[id$="lblTransferAmount"]').text(_w88_contents.translate("LABEL_FUNDS_TRANSFER") + " " + _w88_contents.translate("LABEL_AMOUNT"));
                $('label[id$="lblPromoCode"]').text(_w88_contents.translate("LABEL_PROMO_CODE"));
                $("header .header-title").text(_w88_contents.translate("LABEL_HISTORY_FUND_TRANSFER"));
                $('#btnSubmit').text(_w88_contents.translate("BUTTON_SUBMIT"));
                $('#showBalance').text(_w88_contents.translate("BUTTON_SHOW_BALANCE"));
            } else {
                window.setInterval(function () {
                    setTranslations();
                }, 500);
            }
        }

        $('select[id$="drpTransferFrom"]').attr('disabled', 'disabled');
        $('select[id$="drpTransferTo"]').attr('disabled', 'disabled');

        pubsub.subscribe('fundsLoaded', onFundsLoaded);
        pubsub.subscribe('mainWalletLoaded', onMainWalletLoaded);

        _w88_funds.init({ selector: "walletFrom" }, true);

        _w88_funds.mainWalletInit();
    }

    function onMainWalletLoaded() {

        var wallet = _w88_funds.wallet();

        $(".wallet-title").html(wallet.Name);
        $(".wallet-value").html(wallet.Balance);
        $(".wallet-currency").html(wallet.CurrencyLabel);
        $(".wallets").addClass('wallet-auto');
    }

    function onFundsLoaded() {

        wallets = _w88_funds.wallets();

        _.forOwn(wallets, function (data) {
            $('select[id$="drpTransferFrom"]').append($('<option>').text(data.Name).attr('value', data.Id).attr('balance', data.Balance));
        });

        var option = $('option:selected', $('select[id$="drpTransferFrom"]')).attr('balance');
        $('select[id$="drpTransferFrom"]').attr('balance', option);

        changeWalletTo($('select[id$="drpTransferFrom"]').val());

        $('select[id$="drpTransferFrom"]').removeAttr('disabled');
        $('select[id$="drpTransferTo"]').removeAttr('disabled');

        var walletList = _.template(
                 $("script#walletBalance").html()
             );

        $("#walletBalances").append(
            walletList({ wallets: wallets })
        );
    }

    function changeWalletTo(selectedValue) {

        $('select[id$="drpTransferTo"]').empty();

        _.forOwn(wallets, function (data) {
            if (data.Id != selectedValue) {
                $('select[id$="drpTransferTo"]').append($('<option>').text(data.Name).attr('value', data.Id).attr('balance', data.Balance));
            }
        });

        $('select[id$="drpTransferTo"]').val($('select[id$="drpTransferTo"] option:first').val());
    }

    function swap($FromEl, $ToEl) {
        var from = $FromEl.val();
        var to = $ToEl.val();
        var fBalance = $FromEl.attr('balance');
        var tBalance = $ToEl.attr('balance');

        if (!_.isUndefined(from) && !_.isUndefined(to)) {

            $FromEl.empty();
            $ToEl.empty();

            _.forOwn(wallets, function (data) {
                $('select[id$="drpTransferFrom"]').append($('<option>').text(data.Name).attr('value', data.Id));
            });
            $FromEl.val(to).attr('balance', tBalance);

            _.forOwn(wallets, function (data) {
                if (data.Id != to) {
                    $('select[id$="drpTransferTo"]').append($('<option>').text(data.Name).attr('value', data.Id));
                }
            });
            $ToEl.val(from).attr('balance', fBalance);
        }

    }

    function create(data) {

        var headers = {
            'Token': window.User.token,
            'LanguageCode': window.User.lang
        };

        $.ajax({
            type: "POST",
            url: w88Mobile.APIUrl + "/payments/transfer",
            data: data,
            beforeSend: function () {
                pubsub.publish('startLoadItem', { selector: '' });
            },
            headers: headers,
            success: function (response) {
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

                        init();

                        break;

                    default:
                        if (_.isArray(response.ResponseMessage))
                            w88Mobile.Growl.shout(w88Mobile.Growl.bulletedList(response.ResponseMessage));
                        else
                            w88Mobile.Growl.shout(response.ResponseMessage);
                        break;
                }
            },
            error: function(response) {
                if (_.isUndefined(response.ResponseData)) {
                    console.log('Unable to fetch wallets.');
                    return;
                }
            },
            complete: function () {
                pubsub.publish('stopLoadItem', { selector: '' });
            }
        });
    }
}