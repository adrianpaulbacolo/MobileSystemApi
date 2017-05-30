window.w88Mobile.Products = Products();
var _w88_products = window.w88Mobile.Products;

function Products() {

    var products = {};

    products.checkFreeRounds = function () {
        var data = { cashier: _constants.FUNDS_URL, Lobby: _constants.SLOTS_BRAVADO_URL };

        _w88_send("/products/freerounds/gpi", "GET", data, function (response) {
            switch (response.ResponseCode) {
                case 1:
                    pubsub.publish('checkFreeRounds', response.ResponseData);
                default:
                    break;
            }
        });
    };

    return products;
}
