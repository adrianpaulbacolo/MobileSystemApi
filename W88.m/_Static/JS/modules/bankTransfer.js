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
        ToogleBank: function (bankId, currency, selectName) {
            return toogleBank(bankId, currency, selectName);
        },
        ToogleSecondaryBank: function (bankId, selectName, bankLocationId) {
            return toogleSecondary(bankId, selectName, bankLocationId);
        },
        ToogleBankBranch: function (selectName, bankLocationId, bankBranchId) {
            return toogleBankBranch(selectName, bankLocationId, bankBranchId);
        }
    };

    function reloadValues(selectName, bankLocationId, bankBranchId) {
        setOriginState();
        setWithTransactState();

        if (bankLocationId.length > 0) {
            loadBankLocation(selectName, bankLocationId);
            enableLocation();
            disableBranch();
        }
        
        if (bankBranchId.length > 0) {
            disableBranch(selectName);
            loadBankBranch(selectName, bankLocationId, bankBranchId);
            enableBranch();
        }
      
    }

    function toogleBank(bankId, currency, selectName) {
        if (bankId == "OTHER") {
            if (currency == "vnd") {

                setWithTransactState();
                disableLocation(selectName);
                disableBranch(selectName);

                if (sessionStorage.getItem("hfBLId") == null) {
                    $('#drpSecondaryBank').val("-1").change();
                }

            } else {
                $('#divBankName').show();
            }
        } else {
            setOriginState();
        }
    }

    function toogleSecondary(bankId, selectName, bankLocationId) {
        setWithTransactState();
        disableLocation(selectName);
        disableBranch();

        if (bankId == "OTHER") {

            setOriginState();
            $('#divOtherBank').show();
            $('#divBankName').show();

        } else if (bankId != "-1") {
            
            loadBankLocation(selectName, bankLocationId);

            disableBranch();

        }

    }

    function toogleBankBranch(selectName, bankLocationId, bankBranchId) {
        loadBankBranch(selectName, bankLocationId, bankBranchId);
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
        $('#divBankName').hide();
        $('#divBankBranch').hide();
        $('#divAddress').hide();
        $('#divOtherBank').show();
        $('#divBankLocation').show();
        $('#divBankNameSelection').show();
    }

    function disableLocation(selectName) {
        $('#drpBankLocation').attr('disabled', 'disabled');
        $('#drpBankLocation-button').addClass('select-disabled');
        $('#drpBankLocation').append($('<option>').text(selectName).attr('value', '-1'));
        $('#drpBankLocation').val("-1").change();
    }

    function enableLocation() {
        $('#drpBankLocation').removeAttr('disabled', 'disabled');
        $('#drpBankLocation-button').removeClass('select-disabled');
    }

    function disableBranch(selectName)
    {
        $('#drpBankBranchList').attr('disabled', 'disabled');
        $('#drpBankBranchList-button').addClass('select-disabled');
        $('#drpBankBranchList').append($('<option>').text(selectName).attr('value', '-1'));
        $('#drpBankBranchList').val("-1").change();
    }

    function enableBranch() {
        $('#drpBankBranchList').removeAttr('disabled', 'disabled');
        $('#drpBankBranchList-button').removeClass('select-disabled');
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
                    disableLocation(selectName);
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

                    enableLocation();
                    $('#loader1').html('');
                }
            });
        }
    }

    function loadBankBranch(selectName, bankLocationId, bankBranchId) {

        var loader = GPInt.prototype.GetLoaderScafold();
        var bankId = $('#drpSecondaryBank').val();

        if (bankId != '-1') {
            $.ajax({
                type: "POST",
                url: "BankTransfer.aspx/GetBankBranch",
                data: JSON.stringify({ bankId: bankId, bankLocationId: bankLocationId }),
                contentType: "application/json; charset=utf-8",
                beforeSend: function () {
                    disableBranch();
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

                    enableBranch();
                    $('#loader2').html('');
                }
            });
        }
    }

    return BankTransfer;
}

window.w88Mobile.BankTransfer = BankTransfer();

