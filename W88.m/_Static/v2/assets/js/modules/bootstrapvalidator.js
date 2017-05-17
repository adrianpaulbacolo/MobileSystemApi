window.w88Mobile.Validator = validator();
var _w88_validator = window.w88Mobile.Validator;

function validator() {
    var status = {
        NotStarted: "notstarted", Started: "started", Done: "done"
    };

    var validatorStatus = status.NotStarted;

    return {
        initiateValidator: initiateValidator
    };

    function initiateValidator($formEl, setting) {
        pubsub.subscribe('validatorStatus', onValidatorStatus);

        setDateTime();
        setNumericValidator();

        $($formEl).validator({
            custom: {
                require: function ($el) {
                    $el.parent("div.form-group").removeClass('has-error');
                    $el.parent("div.form-group").children("span.help-block").remove();

                    if ($el.parent("div.form-group").length == 0) {
                        $el.parent("div").removeClass('has-error');
                        $el.parent("div").children("span.help-block").remove();
                    }

                    if (_.isEmpty($el.val())) {
                        $el.parent("div").append('<span class="help-block">' + $.i18n('RequiredField') + '</span>');

                        if ($el.parent("div.form-group").length == 0) {
                            $el.parent("div").addClass('has-error');
                        }
                        else {
                            $el.parent("div.form-group").addClass('has-error');
                        }
                        return true;
                    }
                    return false;
                },
                selectequals: function ($el) {
                    $el.parent("div.form-group").removeClass('has-error');
                    $el.parent("div.form-group").children("span.help-block").remove();

                    if ($el.parent("div.form-group").length == 0) {
                        $el.parent("div").removeClass('has-error');
                        $el.parent("div").children("span.help-block").remove();
                    }

                    var matchValue = $el.data("selectequals");
                    if ($el.val() == matchValue) {
                        $el.parent("div").append('<span class="help-block">' + $.i18n('RequiredField') + '</span>');
                        if ($el.parent("div.form-group").length == 0) {
                            $el.parent("div").addClass('has-error');
                        }
                        else {
                            $el.parent("div.form-group").addClass('has-error');
                        }
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
                            $el.parent("div").append('<span class="help-block">' + $.i18n('ConfirmField') + '</span>');
                            $el.parent("div.form-group").addClass('has-error');
                            return true;
                        }
                    } else {
                        if ($el.context.required) {
                            $el.parent("div").append('<span class="help-block">' + $.i18n('RequiredField') + '</span>');
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
                            if (parseFloat($el.val()) < parseFloat(matchValue[0])) {
                                $el.parent("div").append('<span class="help-block">' + $.i18n('Pay_AmountMinLimit') + '</span>');
                                $el.parent("div.form-group").addClass('has-error');
                                return true;
                            } else if (parseFloat($el.val()) > parseFloat(matchValue[1])) {
                                $el.parent("div").append('<span class="help-block">' + $.i18n('Pay_AmountMaxLimit') + '</span>');
                                $el.parent("div.form-group").addClass('has-error');
                                return true;
                            }
                        }
                    }
                    else {
                        if ($el.context.required) {
                            $el.parent("div").append('<span class="help-block">' + $.i18n('RequiredField') + '</span>');
                            $el.parent("div.form-group").addClass('has-error');
                            return true;
                        }
                    }
                    return false;
                },
                paylimit: function ($el) {
                    $el.parent("div.form-group").children("span.help-block").remove();
                    $el.parent(".form-group").removeClass('has-error');

                    if (!_.isEmpty($el)) {
                        var elValue = stringToNumber($el.val());

                        if (_.isNaN(elValue) || elValue < setting.MinAmount) {
                            $el.parent("div").append('<span class="help-block">' + $.i18n('Pay_AmountMinLimit') + '</span>');
                            return true;
                        }
                        else if (elValue > setting.MaxAmount) {
                            $el.parent("div").append('<span class="help-block">' + $.i18n('Pay_AmountMaxLimit') + '</span>');
                            return true;
                        }
                        else if ((setting.TotalAllowed != $.i18n('Pay_Unlimited')) &&
                            elValue > parseFloat(_.replace(setting.TotalAllowed, /,/g, "")) && parseFloat(_.replace(setting.TotalAllowed, /,/g, "")) > 0) {
                            $el.parent("div").append('<span class="help-block">' + $.i18n('Pay_TotalAllowedExceeded') + '</span>');
                            return true;
                        }
                    }
                    return false;
                },
                balance: function ($el) {
                    $el.parent("div.form-group").removeClass('has-error');
                    $el.parent("div.form-group").children("span.help-block").remove();
                    var matchValue = $el.data("balance");
                    if ($('option:selected', $el).attr('balance') <= matchValue) {
                        $el.parent("div").append('<span class="help-block">' + $.i18n('Pay_ErrInsufficientBalance') + '</span>');
                        return true;
                    }
                    return false;
                }
            }
        });
    }

    function setNumericValidator() {
        _.forEach($('[data-numeric]'), function (item, index) {
            var numeric = item.getAttribute('data-numeric');

            if (_.isEmpty(numeric))
                $(item).autoNumeric('init');
            else
                $(item).autoNumeric('init', { mDec: numeric });
        });
    }

    function onValidatorStatus(topic, data) {
        validatorStatus = data;
    }

    function setDateTime() {
        _.forEach($('[data-date-box]'), function (item, index) {
            var numeric = item.getAttribute('data-date-box');

            if (_.isEmpty(numeric)) {
                $(item).datebox({
                    mode: 'calbox',
                    showInitialValue: true,
                    overrideDateFormat: '%m/%d/%Y',
                    minDays: 3,
                    maxDays: 3,
                });
            }
            else {
                $(item).datebox({
                    mode: 'timebox',
                    showInitialValue: true,
                });
            }
        });
    }
};
