_.templateSettings = {
    interpolate: /\{\{(.+?)\}\}/g, // print value: {{ value_name }}
    evaluate: /\{%([\s\S]+?)%\}/g, // excute code: {% code_to_execute %}
    escape: /\{%-([\s\S]+?)%\}/g
};

var Catalogue = function (data) {
    this.elem = data.elem;
    this.isSearching = true;
    this.params = data.params;
    this.template = null;
    this.translations = data.translations;
    this.storageKey = window.location.host + '_' + data.language + '_catalogue';
    this.token = data.token;
    this.uri = '/api/rewards/search/';
};

Catalogue.prototype.getProducts = function (params, isProductCache) {
    var self = this;
    if (!isProductCache) {
        self.isSearching = true;
        $.mobile.loading('show');
    }
    $.ajax({
        type: 'GET',
        url: self.uri,
        headers: {
            'token': self.token
        },
        dataType: 'json',
        data: !_.isEmpty(params) ? params : self.params,
        success: function(response) {
            if (response.ResponseCode != 1 || _.isEmpty(response.ResponseData)) {
                if (!isProductCache) self.reset();
                self.manageCache();
                return;
            }
            if (!isProductCache) {
                self.searchCallback(response.ResponseData);
            } else {
                params.Index += 1;
                self.manageCache(response.ResponseData);
                self.cacheProducts(params, true);
            }
        },
        error: function () {
            if (!isProductCache) self.reset();
            self.manageCache();
        }
    });
};

Catalogue.prototype.searchCallback = function (data) {
    var self = this;
    $.get('/_Static/catalogue/template.html', function (html) {
        self.template = _.template(html);
        self.elem.append(self.template({
            products: data,
            translations: self.translations
        }));
    });
    self.params.Index += 1;
    self.reset();
};

Catalogue.prototype.cacheProducts = function (params) {
    var self = this;
    params = _.isEmpty(params) ? _.clone(self.params) : params;
    self.getProducts(params, true);
};

Catalogue.prototype.manageCache = function (data) {
    var self = this,
        cachedProducts = amplify.store(self.storageKey) || [],
        filtered = _.filter(data, function (item) {
            var duplicate = _.find(cachedProducts, { ProductId: item.ProductId });
            if (_.isEmpty(duplicate)) return item;

            if (!_.isEqual(duplicate, item)) {
                var index = _.findIndex(cachedProducts, { ProductId: item.ProductId });
                if (index > -1)
                    cachedProducts[index] = item;
            }
        });
    if (!_.isEmpty(filtered)) {
        _.each(filtered, function (item) {
            cachedProducts.push(item);
        });
    }
    if (_.isEmpty(cachedProducts)) return;
    amplify.store(self.storageKey, cachedProducts);
};

Catalogue.prototype.reset = function () {
    var self = this;
    $.mobile.loading('hide');
    self.isSearching = false;
};