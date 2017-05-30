w88Mobile.v2.Templates = Templates;

function Templates() {

    //templates
    var templates = [
        { id: 'SearchList', url: '/_Static/v2/assets/js/modules/slots/templates/search.html' }
        , { id: 'SlotList', url: '/_Static/v2/assets/js/modules/slots/templates/slotCategory.html' }
        , { id: 'TopBar', url: '/_Static/v2/assets/js/modules/slots/templates/header.html' }
        , { id: 'MainPage', url: '/_Static/v2/assets/js/modules/slots/templates/page.html' }
        , { id: 'GameLauncher', url: '/_Static/v2/assets/js/modules/slots/templates/launcher.html' }
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