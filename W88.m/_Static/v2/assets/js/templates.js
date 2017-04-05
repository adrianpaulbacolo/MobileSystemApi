w88Mobile.Templates = Templates;

function Templates() {

    //templates
    var templates = [
        { id: 'SearchList', url: 'assets/templates/search.html' }
        , { id: 'SlotList', url: 'assets/templates/slotCategory.html' }
        , { id: 'TopBar', url: 'assets/templates/header.html' }
        , { id: 'MainPage', url: 'assets/templates/page.html' }
    ];

    this.init = function () {
        var _self = this;

        _.templateSettings = {
            interpolate: /\{\{(.+?)\}\}/g,      // print value: {{ value_name }}
            evaluate: /\{%([\s\S]+?)%\}/g,   // excute code: {% code_to_execute %}
            escape: /\{%-([\s\S]+?)%\}/g
        };
        _.templateSettings.variable = "tplData";

        _.forEach(templates, function (template) {
            $.get(template.url, function (tmpl) {
                if (_.isUndefined(w88Mobile.v2.Templates[template.id])) {
                    _self[template.id] = tmpl;
                }
            }, 'html');
        });
    }

}