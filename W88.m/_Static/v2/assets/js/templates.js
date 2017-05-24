var _w88_templates = w88Mobile.Templates = Templates();

_w88_templates.init();

function Templates() {

    //templates
    var templateSettings = [
          { id: 'History_DepositWithdraw', url: 'templates/depositWithdraw.html' }
        , { id: 'History_FundTransfer', url: 'templates/fundTransfer.html' }
        , { id: 'History_Adjustment', url: 'templates/adjustment.html' }
        , { id: 'History_PromoClaim', url: 'templates/promoClaim.html' }

        , { id: 'History_AdjustmentModal', url: 'templates/adjustmentModal.html' }
        , { id: 'History_DepositWithdrawModal', url: 'templates/depositWithdrawModal.html' }
        , { id: 'History_FundTransferModal', url: 'templates/fundTransferModal.html' }

        , { id: 'Free_Rounds', url: 'templates/freerounds.html' }
    ];

    var items = {};

    function init() {
        var _self = this;

        _.templateSettings = {
            interpolate: /\{\{(.+?)\}\}/g,      // print value: {{ value_name }}
            evaluate: /\{%([\s\S]+?)%\}/g,   // excute code: {% code_to_execute %}
            escape: /\{%-([\s\S]+?)%\}/g
        };
        _.templateSettings.variable = "tplData";

        _.forEach(templateSettings, function (template) {
            $.get(template.url, function (tmpl) {
                if (_.isUndefined(w88Mobile.Templates.items[template.id])) {
                    _self[template.id] = tmpl;
                }
            }, 'html');
        });
    }

    return {
        init: init,
        items: items
    };
}