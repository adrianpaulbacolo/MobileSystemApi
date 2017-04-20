window.w88Mobile.Gateways.AutoRoute = AutoRoute();

function AutoRoute() {

    var autoroute = {
        Deposit: deposit,
        Initialize: init
    };

    return autoroute;

    function send(resource, method, data, beforeSend, success, complete) {
        var url = w88Mobile.APIUrl + resource;

        var headers = {
            'Token': window.User.token,
            'LanguageCode': window.User.lang
        };

        $.ajax({
            type: method,
            url: url,
            data: data,
            beforeSend: beforeSend,
            headers: headers,
            success: success,
            error: function () {
                console.log("Error connecting to api");
            },
            complete: complete
        });
    }

    // deposit
    function deposit(methodId, data, successCallback, completeCallback) {
        send("/payments/" + methodId, "POST", data, function () { GPInt.prototype.ShowSplash() }, successCallback, completeCallback);
    }

    function init() {
        getGatewayBanks();

        setTranslations();
        function setTranslations() {
            if (_w88_contents.translate("LABEL_SWITCH_LINE") != "LABEL_SWITCH_LINE") {
                $('label[id$="lblSwitchLine"]').text(_w88_contents.translate("LABEL_SWITCH_LINE"));
                $("#paymentNote").text(_w88_contents.translate("LABEL_PAYMENT_NOTE"));
                $("#paymentNoteContent").text(_w88_contents.translate("LABEL_MSG_BANK_NOT_SUPPORTED"));
            } else {
                window.setInterval(function () {
                    setTranslations();
                }, 500);
            }
        }
    }

    function getGatewayBanks() {
        _w88_paymentSvc.SendDeposit("/Banks/gateway", "GET", "", function (response) {
            switch (response.ResponseCode) {
                case 1:
                    $('select[id$="drpBank"]').append($("<option></option>").attr("value", "-1").text(_w88_contents.translate("LABEL_SELECT_DEFAULT")));
                    $('select[id$="drpBank"]').val("-1").selectmenu("refresh");

                    _.forEach(response.ResponseData, function (data) {

                        if (_.isEqual(data.Value, "ICBC") || _.isEqual(data.Value, "ECITIC"))
                            data.Text = data.Text + " (*)";

                        $('select[id$="drpBank"]').append($("<option></option>").attr("value", data.Value).text(data.Text))
                    })

                    break;
                default:
                    if (_.isArray(response.ResponseMessage))
                        w88Mobile.Growl.shout(w88Mobile.Growl.bulletedList(response.ResponseMessage));
                    else
                        w88Mobile.Growl.shout(response.ResponseMessage);

                    break;
            }
        },
      function () {
          GPInt.prototype.HideSplash();
      });
    }
}

window.w88Mobile.Gateways.AutoRoute = AutoRoute();