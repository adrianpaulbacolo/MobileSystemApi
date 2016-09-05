function BankTransfer() {

    var BankTransfer = {
        LoadBankLocation: function (selectName, bankLocationId) {
            return loadBankLocation(selectName, bankLocationId);
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
        },
        ToogleBankBranch: function (bankId, selectName, bankLocationId, bankBranchId) {
            return toogleBankBranch(bankId, selectName, bankLocationId, bankBranchId);
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

                if ($('#drpSecondaryBank').val() != '-1') {
                    $('#drpSecondaryBank').change();
                }
            } else {
                $('#divBankName').show();
            }
        } else {
            $('#divBankBranch').show();
            $('#divAddress').show();
            $('#divOtherBank').hide();
            $('#divBankLocation').hide();
            $('#divBankNameSelection').hide();
        }
    }

    function toogleSecondary(bankId, selectName, bankLocationId) {
        if (bankId == "OTHER") {
            $('#divBankBranch').show();
            $('#divAddress').show();
            $('#divBankName').show();
            $('#divOtherBank').hide();
            $('#divBankLocation').hide();
            $('#divBankNameSelection').hide();
        } else {
            $('#divBankName').hide();
            $('#divBankLocation').show();
            $('#divBankNameSelection').hide();

            loadBankLocation(selectName, bankLocationId);
        }
    }

    function toogleBankBranch(bankId, selectName, bankLocationId, bankBranchId) {
        $('#divBankNameSelection').show();
        loadBankBranch(bankId, selectName, bankLocationId, bankBranchId);
    }

    function setOriginState() {
        $('#divBankName').hide();
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

        var loader = GPInt.prototype.GetLoaderScafold();
        var bankId = $("#drpSecondaryBank").val();

        if (bankId != '-1') {
            $.ajax({
                type: "POST",
                url: "BankTransfer.aspx/GetBankLocation",
                data: JSON.stringify({ bankId: bankId }),
                contentType: "application/json; charset=utf-8",
                beforeSend: function () {
                    $('#loader1').html(loader);
                },
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

                    $('#loader1').html('');
                }
            });
        }
    }

    function loadBankBranch(bankId, selectName, bankLocationId, bankBranchId) {

        var loader = GPInt.prototype.GetLoaderScafold();

        if (bankId != '-1') {
            $.ajax({
                type: "POST",
                url: "BankTransfer.aspx/GetBankBranch",
                data: JSON.stringify({ bankId: bankId, bankLocationId: bankLocationId }),
                contentType: "application/json; charset=utf-8",
                beforeSend: function () {
                    $('#loader2').html(loader);
                },
                dataType: "json",
                success: function (response) {
                    var result = $.parseJSON(response.d);

                    if (result.length > 0) {

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

                    $('#loader2').html('');
                }
            });
        }
    }

    return BankTransfer;
}

window.w88Mobile.BankTransfer = BankTransfer();

