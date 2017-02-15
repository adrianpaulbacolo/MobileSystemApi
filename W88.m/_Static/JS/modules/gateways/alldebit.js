window.w88Mobile.Gateways.AllDebit = AllDebit();
var _w88_alldebit = window.w88Mobile.Gateways.AllDebit;

function AllDebit() {

    var alldebit = Object.create(new w88Mobile.Gateway(_w88_paymentSvcV2));

    alldebit.init = function () {
        setTranslations();
        function setTranslations() {
            if (_w88_contents.translate("LABEL_PAYMENT_NOTE") != "LABEL_PAYMENT_NOTE") {

                $('label[id$="lblDepositAmount"]').text(_w88_contents.translate("LABEL_AMOUNT"));
                $('label[id$="lblCardType"]').text(_w88_contents.translate("LABEL_CARD_TYPE"));
                $('label[id$="lblCardName"]').text(_w88_contents.translate("LABEL_CARD_NAME"));
                $('label[id$="lblCardNo"]').text(_w88_contents.translate("LABEL_CARD_NUMBER"));
                $('label[id$="lblExpiry"]').text(_w88_contents.translate("LABEL_CARD_EXPIRY"));
                $('label[id$="lblSecurityCode"]').text(_w88_contents.translate("LABEL_CARD_CCV"));
                $('a[id$="ccvHelp"]').text(_w88_contents.translate("LABEL_CARD_CCV_HELP"));
                $('select[id$="ddlExpiryMonth"]').text(_w88_contents.translate("LABEL_MONTH"));
                $('select[id$="ddlExpiryYear"]').text(_w88_contents.translate("LABEL_YEAR"));

                var ddlCardTypeText = _w88_contents.translate("LABEL_SELECT_CARD_TYPE");
                var monthText = _w88_contents.translate("LABEL_MONTH");
                var yearText = _w88_contents.translate("LABEL_YEAR");

                _w88_paymentSvc.SendDeposit("/CardType", "GET", "", function (response) {

                    $('select[id$="ddlCardType"]').append($("<option />").val('-1').text(ddlCardTypeText));

                    $.each(response.ResponseData, function (m, val) {
                        $('select[id$="ddlCardType"]').append($("<option />").val(val.Value).text(val.Text));
                    });
                });

                $('select[id$="ddlExpiryMonth"]').append($("<option />").val('-1').text(monthText));
                var month = ['01', '02', '03', '04', '05', '06', '07', '08', '09', '10', '11', '12'];
                $.each(month, function (m, val) {
                    $('select[id$="ddlExpiryMonth"]').append($("<option />").val(val).text(val));
                });
                $('select[id$="ddlExpiryMonth"]').val('-1').change();

                $('select[id$="ddlExpiryYear"]').append($("<option />").val('-1').text(yearText));
                var yr = new Date().getFullYear();
                for (i = yr; i <= yr + 10; i++) {
                    $('select[id$="ddlExpiryYear"]').append($("<option />").val(i).text(i));
                }
                $('select[id$="ddlExpiryYear"]').val('-1').change();

                _w88_paymentSvc.SendDeposit("/user/lastccdetails", "GET", "", function (response) {

                    if (response.ResponseCode == 1 && response.ResponseData != null) {

                        $('input[id$="txtCardName"]').val(response.ResponseData.CardName);
                        $('input[id$="txtCardNo"]').val(response.ResponseData.CardNumber);

                        //Selection based on API result
                        var ctype = $('select[id$="ddlCardType"] option').filter(function () {
                            return $(this).html() == response.ResponseData.CardType;
                        }).val();
                        $('select[id$="ddlCardType"]').val(ctype).change();

                        var cMonth = $('select[id$="ddlExpiryMonth"] option').filter(function () {
                            return $(this).html() == response.ResponseData.CardExpiryMonth;
                        }).val();
                        $('select[id$="ddlExpiryMonth"]').val(cMonth).change();

                        var cYear = $('select[id$="ddlExpiryYear"] option').filter(function () {
                            return $(this).html() == response.ResponseData.CardExpiryYear;
                        }).val();
                        $('select[id$="ddlExpiryYear"]').val(cYear).change();

                        $('input[id$="txtCardNo"]').mask('9999-9999-9999-9999');
                        $('input[id$="txtSecurityCode"]').mask('999');
                    }
                });


            } else {
                window.setInterval(function () {
                    setTranslations();
                }, 500);
            }
        }
    };

    alldebit.createDeposit = function () {
        var _self = this;
        var params = _self.getUrlVars();
        var data = {
            Amount: params.Amount,
            CardType: { Text: params.CardTypeTextText, Value: params.CardTypeTextValue },
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
                    w88Mobile.PostPaymentForm.createv2(response.ResponseData.FormData, response.ResponseData.PostUrl, "body");
                    w88Mobile.PostPaymentForm.submit();

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

    return alldebit;
}