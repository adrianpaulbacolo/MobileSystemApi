function Wallets() {

    var Wallets = {
        getWallets: function () {
            return GetWallets();
        }
    };

    return Wallets;
}

var loader = GPInt.prototype.GetLoaderScafold();

function GetWallets() {
    $.ajax({
        type: 'POST',
        contentType: "application/json",
        url: '/AjaxHandlers/GetWalletsWithBalance.ashx',
        beforeSend: function () {
            $('span[name="WalletBalance"]').html(loader);
        },
        success: function (xml) {
             if (xml != "-1") {
                 $.each(xml, function (i, val) {
                     var id = val.Key;
                     $("#" + id).html(val.Value);
                 });
             } else {
                 window.location.reload();
             }
        },
        error: function (err) {
        }
    });
}

window.w88Mobile.Wallets = Wallets();