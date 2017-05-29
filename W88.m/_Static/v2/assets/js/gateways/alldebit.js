window.w88Mobile.Gateways.AllDebit = AllDebit();
var _w88_alldebit = window.w88Mobile.Gateways.AllDebit;

function AllDebit() {

    var alldebit;

    try {
        alldebit = Object.create(new w88Mobile.Gateway(_w88_paymentSvcV2));
    } catch (err) {
        alldebit = {};
    }

    alldebit.init = function () {
        $('label[id$="lblCardType"]').text(_w88_contents.translate("LABEL_CARD_TYPE"));
        $('label[id$="lblCardName"]').text(_w88_contents.translate("LABEL_CARD_NAME"));
        $('label[id$="lblCardNo"]').text(_w88_contents.translate("LABEL_CARD_NUMBER"));
        $('label[id$="lblExpiry"]').text(_w88_contents.translate("LABEL_CARD_EXPIRY"));
        $('label[id$="lblSecurityCode"]').text(_w88_contents.translate("LABEL_CARD_CCV"));
        $('a[id$="ccvHelp"]').text(_w88_contents.translate("LABEL_CARD_CCV_HELP"));
        $('select[id$="ddlExpiryMonth"]').text(_w88_contents.translate("LABEL_MONTH"));
        $('select[id$="ddlExpiryYear"]').text(_w88_contents.translate("LABEL_YEAR"));

        var defaultSelect = _w88_contents.translate("LABEL_SELECT_DEFAULT");
        var monthText = _w88_contents.translate("LABEL_MONTH");
        var yearText = _w88_contents.translate("LABEL_YEAR");

        alldebit.getCardType();
        alldebit.getExpiryMonthYear();
        alldebit.getCreditCardLastTransaction();
    };

    alldebit.getCardType = function () {
        var _self = this;

        _self.send("/CardType", "GET", "", function (response) {
            $('select[id$="ddlCardType"]').append($('<option>').text(defaultSelect).attr('value', '-1'));

            _.forOwn(response.ResponseData, function (data) {
                $('select[id$="ddlCardType"]').append($('<option>').text(data.Text).attr('value', data.Value));
            });
        });
    };

    alldebit.getExpiryMonthYear = function () {

        $('select[id$="ddlExpiryMonth"]').append($("<option />").val('-1').text(monthText));
        var month = ['01', '02', '03', '04', '05', '06', '07', '08', '09', '10', '11', '12'];
        _.forOwn(month, function (data) {
            $('select[id$="ddlExpiryMonth"]').append($('<option>').text(data).attr('value', data));
        });

        $('select[id$="ddlExpiryYear"]').append($("<option />").val('-1').text(yearText));
        var yr = new Date().getFullYear();
        for (i = yr; i <= yr + 10; i++) {
            $('select[id$="ddlExpiryYear"]').append($("<option />").val(i).text(i));
        }

    };

    alldebit.getCreditCardLastTransaction = function () {
        var _self = this;

        _self.send("/payments/creditcard/lasttrans", "GET", "", function (response) {

            if (response.ResponseCode == 1 && response.ResponseData != null) {

                $('input[id$="txtCardName"]').val(response.ResponseData.CardName);
                $('input[id$="txtCardNo"]').val(response.ResponseData.CardNumber);

                //Selection based on API result
                $('select[id$="ddlCardType"]').val(response.ResponseData.CardType.Value).change();
                $('select[id$="ddlExpiryMonth"]').val(response.ResponseData.CardExpiryMonth).change();
                $('select[id$="ddlExpiryYear"]').val(response.ResponseData.CardExpiryYear).change();

                $('input[id$="txtCardNo"]').mask('9999-9999-9999-9999');
                $('input[id$="txtSecurityCode"]').mask('999');
            }
        });
    };

    alldebit.createDeposit = function () {
        var _self = this;
        var params = _self.getUrlVars();
        var data = {
            Amount: params.Amount,
            CardType: { Text: params.CardTypeText, Value: params.CardTypeValue },
            AccountName: params.AccountName,
            CardNumber: params.CardNumber,
            CardExpiryMonth: params.CardExpiryMonth,
            CardExpiryYear: params.CardExpiryYear,
            CCV: params.CCV
        };

        _self.methodId = params.MethodId;
        _self.changeRoute();
        _self.deposit(data, function (response) {
            switch (response.ResponseCode) {
                case 1:
                    w88Mobile.PostPaymentForm.create(response.ResponseData.FormData, response.ResponseData.PostUrl, "body");
                    w88Mobile.PostPaymentForm.submit();

                    break;
                default:
                    if (_.isArray(response.ResponseMessage))
                        w88Mobile.Growl.shout(w88Mobile.Growl.bulletedList(response.ResponseMessage), _self.shoutCallback);
                    else
                        w88Mobile.Growl.shout(response.ResponseMessage, _self.shoutCallback);

                    break;
            }
        },
            function () {
                pubsub.publish('stopLoadItem', { selector: "" });
            });
    }

    return alldebit;
}