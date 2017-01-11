w88Mobile.v2.Templates = Templates;

function Templates() {

    //templates
    var templates = [
        { id: 'SearchList', url: 'assets/templates/search.html' }
        , { id: 'SlotList', url: 'assets/templates/slotCategory.html' }
        , { id: 'TopBar', url: 'assets/templates/header.html' }
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