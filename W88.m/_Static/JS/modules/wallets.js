function Wallets() {

    var Wallets = {
        balance: 0,
        getMain: function() {
            return GetData();
        },
        getWallets: function() {
            return GetWallets();
        }
    };

    return Wallets;
}

function GetData() {
    var udata = { id: 0 };

    return $.ajax({
        type: 'POST',
        contentType: "application/json",
        url: '/AjaxHandlers/GetWalletBalance.ashx',
        data: JSON.stringify(udata),
        success: function (d) {
            self.balance = d;
        },
        error: function (err) {
            self.balance = 0;
        }
    });
}

var loader = GPInt.prototype.GetLoaderScafold();

function GetWallets() {
    $.ajax({
        type: 'POST',
        contentType: "application/json",
        url: '/AjaxHandlers/GetWalletsWithBalance.ashx',
        beforeSend: function () {
            $(".fundsType h4").html(loader);
            $("#refesh").css("display", "none");
        },
        success: function (xml) {
            $.each(xml, function (i, val) {
                var id = val.Key;
                $("#" + id).html(val.Value);
            });

            $("#refesh").css("display", "block");
        },
        error: function (err) {
        }
    });
}


window.w88Mobile.Wallets = Wallets();

  