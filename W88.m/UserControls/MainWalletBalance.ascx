<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MainWalletBalance.ascx.cs" Inherits="UserControls.UserControlsMainWalletBalance" %>


<label class="label"><%=commonCulture.ElementValues.getResourceString("mainWallet", commonVariables.LeftMenuXML)%></label>
<h2 class="value" id="mainwallet"></h2>
<small class="currency"><%=commonVariables.GetSessionVariable("CurrencyCode")%></small>


<script type="text/javascript">
    var loader = GPInt.prototype.GetLoaderScafold();

    $(function () {
        $("#mainwallet").html(loader);
        GetMainBalance();
    });

    function GetMainBalance() {
        $("#mainwallet").html(loader);
        var fetch = w88Mobile.Wallets.getMain().done();

        fetch.done(function (data) {
            $("#mainwallet").html(data);
        });
    }
</script>
