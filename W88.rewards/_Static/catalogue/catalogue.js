_.templateSettings = {
    interpolate: /\{\{(.+?)\}\}/g, // print value: {{ value_name }}
    evaluate: /\{%([\s\S]+?)%\}/g, // excute code: {% code_to_execute %}
    escape: /\{%-([\s\S]+?)%\}/g
};

var Catalogue = function (data) {
    this.cacheQuerySize = data.cacheQuerySize;
    this.elems = data.elems;
    this.hasReloaded = false;
    this.isCachingComplete = false;
    this.isSearching = true;
    this.params = data.params;
    this.storageKey = window.location.host + '_' + data.language + '_catalogue';
    this.template = null;
    this.translations = data.translations;
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
                if (isProductCache) {
                    if (params.Index == 0)
                        self.elems.noDataFoundLabel.show();
                    if (params.Index > 0)
                        self.isCachingComplete = true;
                    return;
                }
                self.reset();
                if(self.params.Index == 0)
                    self.elems.noDataFoundLabel.show();
                return;
            }
            if(self.elems.noDataFoundLabel.css('display') == 'block')
                self.elems.noDataFoundLabel.hide();
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
        }
    });
};

Catalogue.prototype.getProductsFromCache = function (isReload) {
    var self = this,
        cachedItems = amplify.store(self.storageKey);
    self.hasReloaded = isReload;
    if (_.isEmpty(cachedItems)) return [];
    if (self.params.CategoryId == 0) {
        self.renderUi(cachedItems, isReload);
        return cachedItems;
    }
    var filtered = _.filter(cachedItems, { CategoryId: self.params.CategoryId });
    if (_.isEmpty(filtered)) 
        self.elems.noDataFoundLabel.show();
    else 
        self.renderUi(filtered, isReload);
    
    return cachedItems;
};

Catalogue.prototype.searchCallback = function (data) {
    var self = this;
    self.renderUi(data);
    self.params.Index += 1;
    self.reset();
};

Catalogue.prototype.cacheProducts = function (params) {
    var self = this;
    params = _.isEmpty(params) ? _.clone(self.params) : params;
    params.PageSize = self.cacheQuerySize;
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

Catalogue.prototype.renderUi = function (data, isReload) {
    var self = this;
    if (isReload) 
        self.elems.container.html(null);
    
    $.get('/_Static/catalogue/template.html', function (html) {
        self.template = _.template(html);
        self.elems.container.append(self.template({
            products: data,
            translations: self.translations
        }));
    });
};

Catalogue.prototype.reset = function () {
    var self = this;
    $.mobile.loading('hide');
    self.isSearching = false;
};