w88Mobile.Templates = Templates;

function Templates() {

    //templates
    var templates = [
          { id: 'History_DepositWithdraw', url: 'templates/depositWithdraw.html' }
        , { id: 'History_FundTransfer', url: 'templates/fundTransfer.html' }
        , { id: 'History_Adjustment', url: 'templates/adjustment.html' }
        , { id: 'History_PromoClaim', url: 'templates/promoClaim.html' }

        , { id: 'History_AdjustmentModal', url: 'templates/adjustmentModal.html' }
        , { id: 'History_DepositWithdrawModal', url: 'templates/depositWithdrawModal.html' }
        , { id: 'History_FundTransferModal', url: 'templates/fundTransferModal.html' }
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