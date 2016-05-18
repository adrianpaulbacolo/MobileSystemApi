function FormValidator() {
    var FormValidator = {
        form: {},
        init: function (pageForm) {
            var self = this;
            self.form = pageForm;
        },
        disableSubmitButton: function (selector) {
            setTimeout(function () {
                $(selector).prop("disabled", true);
            }, 0);
        }
    }

    return FormValidator;
}

window.w88Mobile.FormValidator = FormValidator();