window.w88Mobile.BankDetails = BankDetails();
var _w88_bankdetails = window.w88Mobile.BankDetails;

function BankDetails() {

    var bankdetails = {
        init: init,
        toogleBank: toogleBank,
        createBankDetails: createBankDetails,
    };

    return bankdetails;

    function init() {
        _w88_validator.initiateValidator("#form1");

        $('header .header-title').html(_w88_contents.translate("LABEL_MENU_BANK_DETAILS"));

        getBanks();

        $('select[id$="drpBank"]').change(function () {
            toogleBank(this.value);
        });
    }

    function toogleBank(id) {
        if (!_.isEmpty(id)) {
            if (_.isEqual(id.toUpperCase(), "OTHER")) {
                showBankName();
            } else {
                hideBankName();
            }
        }
    }

    function showBankName() {
        $('.bankname').show();
        $('input[id$="txtBankName"]').attr({ required: '', 'data-require': '' });
        $('#form1').validator('update')
    };

    function hideBankName() {
        $('.bankname').hide();
        $('input[id$="txtBankName"]').removeAttr('required data-require');
        $('#form1').validator('update')
    };

    function createBankDetails(data) {
        send("/user/banks", "POST", data, function (response) {
            if (_.isArray(response.ResponseMessage))
                w88Mobile.Growl.shout(w88Mobile.Growl.bulletedList(response.ResponseMessage));
            else
                w88Mobile.Growl.shout(response.ResponseMessage);

        });
    }

    function getBanks() {
        send("/banks/member", "GET", "", function (response) {
            $('select[id$="drpBank"]').append($("<option></option>").attr("value", "-1").text(_w88_contents.translate("LABEL_SELECT_DEFAULT")));

            _.forEach(response.ResponseData, function (data) {
                $('select[id$="drpBank"]').append($("<option></option>").attr("value", data.Value).text(data.Text))
            })

            send("/user/banks", "GET", "", function (response) {
                if (_.isEqual(response.ResponseCode, 1)) {
                    $('select[id$="drpBank"]').val(response.ResponseData.Bank.Value).change();
                    $('input[id$="txtBankName"]').val(response.ResponseData.BankName);
                    $('input[id$="txtBankBranch"]').val(response.ResponseData.BankBranch);
                    $('input[id$="txtBankAddress"]').val(response.ResponseData.BankAddress);
                    $('input[id$="txtAccountName"]').val(response.ResponseData.AccountName);
                    $('input[id$="txtAccountNumber"]').val(response.ResponseData.AccountNumber);
                    $('[id$="isPreferred"]').prop("checked", response.ResponseData.IsPreferred);
                }
            });
        });
    }

    function send(resource, method, data, success, complete) {
        var selector = "";
        if (!_.isEmpty(data.selector)) {
            selector = _.clone(data.selector);
            delete data["selector"];
        }

        var url = w88Mobile.APIUrl + resource;

        var headers = {
            'Token': window.User.token,
            'LanguageCode': window.User.lang
        };

        $.ajax({
            type: method,
            url: url,
            data: data,
            beforeSend: function () {
                pubsub.publish('startLoadItem', { selector: "" });
            },
            headers: headers,
            success: success,
            error: function () {
                console.log("Error connecting to api");
            },
            complete: function () {
                if (_.isFunction(complete)) complete();
                pubsub.publish('stopLoadItem', { selector: "" });
            }
        });
    }
}