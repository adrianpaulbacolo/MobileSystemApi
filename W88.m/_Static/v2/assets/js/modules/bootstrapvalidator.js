window.w88Mobile.BootstrapValidator = validator();

function validator() {
    return {
        initiateValidator: initiateValidator
    };

    function initiateValidator($formEl) {
        $($formEl).validator({
            custom: {
                selectequals: function ($el) {
                    $el.parent("div.form-group").removeClass('has-error');
                    $el.parent("div.form-group").children("span.help-block").remove();
                    var matchValue = $el.data("bankequals");
                    if ($el.val() == matchValue) {
                        $el.parent("div.form-group").addClass('has-error');
                        return true;
                    }
                    return false;
                },
                confirmvalue: function ($el) {
                    $el.parent("div.form-group").removeClass('has-error');
                    $el.parent("div.form-group").children("span.help-block").remove();

                    if (!_.isEmpty($el.val())) {
                        var matchValue = $("#" + $el.data("confirmvalue")).val();
                        if ($el.val() != matchValue) {
                            $el.parent("div.form-group").addClass('has-error');
                            return true;
                        }
                    }
                    return false;
                },
                confirmrange: function ($el) {
                    $el.parent("div.form-group").removeClass('has-error');
                    $el.parent("div.form-group").children("span.help-block").remove();
                    var matchValue = $el.data("confirmrange").split("|");

                    if (!_.isEmpty($el.val())) {
                        if (!_.inRange($el.val(), matchValue[0], matchValue[1])) {
                            $el.parent("div.form-group").addClass('has-error');
                            return true;
                        }
                    }
                    return false;
                }
            }
        });

    }
};

