w88Mobile.Templates = Templates;

function Templates() {

    //templates
    var templates = [
        { id: 'History_Report_Title', url: 'templates/reportTitle.html' }
        , { id: 'History_DepositWithdraw', url: 'templates/depositWithdraw.html' }
        , { id: 'History_FundTransfer', url: 'templates/fundTransfer.html' }
        , { id: 'History_Adjustment', url: 'templates/adjustment.html' }
        , { id: 'History_RefferalBonus', url: 'templates/referralBonus.html' }
        , { id: 'History_PromoClaim', url: 'templates/promoClaim.html' }
    ];

    this.init = function () {
        var _self = this;
        _.forEach(templates, function (template) {
            $.get(template.url, function (tmpl) {
                if (_.isUndefined(w88Mobile.Templates[template.id])) {
                    _self[template.id] = tmpl;
                }
            }, 'html');
        });
    }
}