w88Mobile.v2.Templates = Templates;

function Templates() {

    //templates
    var templates = [
        { id: 'SearchList', url: 'assets/templates/search.html' }
        , { id: 'SearchList', url: 'assets/templates/search.html' }
    ];

    this.init = function () {
        var _self = this;
        _.forEach(templates, function (template) {
            $.get(template.url, function (tmpl) {
                if (_.isUndefined(w88Mobile.v2.Templates[template.id])) {
                    _self[template.id] = tmpl;
                }
            }, 'html');
        });
    }

}