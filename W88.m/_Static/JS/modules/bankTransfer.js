function BankTransfer() {

    var BankTransfer = {
        LoadBankLocation: function (selectName, bankLocationId) {
            return loadBankLocation(selectName, bankLocationId);
        },
        LoadBankBranch: function (selectName, bankLocationId, bankBranchId) {
            return loadBankBranch(selectName, bankLocationId, bankBranchId);
        },
        ReloadValues: function (selectName, bankLocationId, bankBranchId) {
            return reloadValues(selectName, bankLocationId, bankBranchId);
        },
        SetOriginState: function() {
            return setOriginState();
        },
        SetWithTransactState: function() {
            return setWithTransactState();
        },
        ToogleBank: function (bankId, currency) {
            return toogleBank(bankId, currency);
        },
        ToogleSecondaryBank: function (bankId, selectName, bankLocationId) {
            return toogleSecondary(bankId, selectName, bankLocationId);
        }
    };

    function reloadValues(selectName, bankLocationId, bankBranchId) {
        setOriginState();
        setWithTransactState();
        loadBankLocation(selectName, bankLocationId);
        loadBankBranch(selectName, bankLocationId, bankBranchId);
    }

    function toogleBank(bankId, currency) {
        if (bankId == "OTHER") {
            if (currency == "vnd") {
                $('#divOtherBank').show();
                $('#divBankBranch').hide();
                $('#divAddress').hide();
            } else {
                $('#divBankName').show();
            }
        } else if (bankId == "-1") {
            $('#divBankBranch').show();
            $('#divOtherBank').hide();
            $('#divBankLocation').hide();
            $('#divBankNameSelection').hide();
        }
    }

    function toogleSecondary(bankId, selectName, bankLocationId) {
        if (bankId == "OTHER") {
            $('#divBankBranch').show();
            $('#divOtherBank').hide();
            $('#divBankLocation').hide();
            $('#divBankNameSelection').hide();
        } else {
            $('#divBankLocation').show();
            $('#divBankNameSelection').hide();

            loadBankLocation(selectName, bankLocationId);
        }
    }


    function setOriginState() {
        $('#divBankBranch').show();
        $('#divAddress').show();
        $('#divOtherBank').hide();
        $('#divBankLocation').hide();
        $('#divBankNameSelection').hide();
    }

    function setWithTransactState() {
        $('#divBankBranch').hide();
        $('#divAddress').hide();
        $('#divOtherBank').show();
        $('#divBankLocation').show();
        $('#divBankNameSelection').show();
    }

    function loadBankLocation(selectName, blId) {

        var bankId = $("#drpSecondaryBank").val();
        if (bankId != '-1') {
            $.ajax({
                type: "POST",
                url: "BankTransfer.aspx/GetBankLocation",
                data: JSON.stringify({ bankId: bankId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var result = $.parseJSON(response.d);

                    if (result.length > 0) {

                        $('#drpBankLocation').append($('<option>').text(selectName).attr('value', '-1'));
                        $('#drpBankLocation').children('option:not(:first)').remove();

                        $.each(result, function (index, v) {
                            $('#drpBankLocation').append($('<option>').text(v.name).attr('value', v.value));
                        });


                        if (blId.length > 0) {
                            $('#drpBankLocation').val(blId).change();
                        } else {
                            $('#drpBankLocation').val('-1').change();
                        }

                    }
                }
            });
        }

    }

    function loadBankBranch(selectName, bankLocationId, bankBranchId) {

        var bankId = $("#drpSecondaryBank").val();
        if (bankId != '-1') {
            $.ajax({
                type: "POST",
                url: "BankTransfer.aspx/GetBankBranch",
                data: JSON.stringify({ bankId: bankId, bankLocationId: bankLocationId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var result = $.parseJSON(response.d);

                    if (result.length > 0) {

                        $('#divBankNameSelection').show();

                        $('#drpBankBranchList').append($('<option>').text(selectName).attr('value', '-1'));
                        $('#drpBankBranchList').children('option:not(:first)').remove();

                        $.each(result, function (index, v) {
                            $('#drpBankBranchList').append($('<option>').text(v.name).attr('value', v.value));
                        });

                        if (bankBranchId.length > 0) {
                            $('#drpBankBranchList').val(bankBranchId).change();
                        } else {
                            $('#drpBankBranchList').val("-1").change();
                        }
                    }
                }
            });
        }
    }

    return BankTransfer;
}

window.w88Mobile.BankTransfer = BankTransfer();

