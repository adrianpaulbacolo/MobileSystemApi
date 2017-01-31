<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Payments.master" AutoEventWireup="true" CodeFile="Baokim.aspx.cs" Inherits="Deposit_Baokim" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
    <ul class="list fixed-tablet-size">
        <li class="item row selection">
            <div class="col col-50">
                <label>
                    <input type="radio" name="depOption" value="EWALLET" />Ví Điện Tử Nội Địa</label>
            </div>
            <div class="col col-50">
                <label>
                    <input type="radio" name="depOption" value="ATM" />Thẻ ATM Nội Địa</label>
            </div>
        </li>
        <li class="item item-input atm ewallet">
            <asp:Label ID="lblDepositAmount" runat="server" AssociatedControlID="txtDepositAmount" />
            <asp:TextBox ID="txtDepositAmount" runat="server" type="number" step="any" min="1" data-clear-btn="true" />
        </li>
        <li class="item item-select atm" runat="server">
            <asp:Label ID="lblBanks" runat="server" AssociatedControlID="drpBanks" />
            <asp:DropDownList ID="drpBanks" runat="server" data-corners="false">
            </asp:DropDownList>
        </li>
        <li class="item item-input atm ewallet">
            <asp:Label ID="lblEmail" runat="server" AssociatedControlID="txtEmail" />
            <asp:TextBox ID="txtEmail" runat="server" data-mini="true" type="email" data-clear-btn="true" />
        </li>
        <li class="item item-select atm">
            <asp:Label ID="lblContact" runat="server" AssociatedControlID="txtContact" />
            <asp:TextBox ID="txtContact" runat="server" type="tel" data-mini="true" data-clear-btn="true" />
        </li>
        <li class="item item-select">
            <p id="notice" style="color: #ff0000"></p>
        </li>
    </ul>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsPlaceHolder1" runat="Server">
    <script type="text/javascript" src="/_Static/JS/modules/gateways/baokim.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            var selectName = '<%=strdrpBank%>';

            var payments = new w88Mobile.Gateways.Payments("<%=base.PaymentMethodId %>");
            payments.init();

            window.w88Mobile.Gateways.DefaultPayments.Deposit("<%=base.strCountryCode %>", "<%=base.strMemberID %>", '<%= commonCulture.ElementValues.getResourceString("paymentNotice", commonVariables.PaymentMethodsXML)%>', "<%=base.PaymentMethodId %>");
         
            window.w88Mobile.Gateways.Baokim.getBanks(selectName);
            window.w88Mobile.Gateways.Baokim.Initialize();

            $('#btnSubmitPlacement').hide();

            $('.ewallet').hide();
            $('.atm').hide();

            $("input[name='depOption']").change(function (e) {
                e.preventDefault();
                var value = $(this).val();

                $('#btnSubmitPlacement').show();

                if (value == "EWALLET") {
                    $('.atm').hide();
                    $('.ewallet').show();
                    $('#btnEwallet').hide();
                    window.w88Mobile.Gateways.Baokim.method = "EWALLET";
                    $("#notice").html(sessionStorage.getItem("noticeWallet"));
                } else {
                    $('.ewallet').hide();
                    $('.atm').show();
                    $(this).hide();
                    window.w88Mobile.Gateways.Baokim.method = "ATM";
                    $("#notice").html(sessionStorage.getItem("noticeAtm"));
                }
            });

            $('#form1').submit(function (e) {
                e.preventDefault();
                var data;

                if (window.w88Mobile.Gateways.Baokim.method == "EWALLET") {
                    data = {
                        Method: window.w88Mobile.Gateways.Baokim.method,
                        Amount: $('#ContentPlaceHolder1_ContentPlaceHolder2_txtDepositAmount').val(),
                        Email: $('#ContentPlaceHolder1_ContentPlaceHolder2_txtEmail').val(),
                        ThankYouPage: location.protocol + "//" + location.host + "/Deposit/BaokimWallet.aspx?requestAmount=" + $('#ContentPlaceHolder1_ContentPlaceHolder2_txtDepositAmount').val()
                    };
                } else {
                    data = {
                        Method: window.w88Mobile.Gateways.Baokim.method,
                        Amount: $('#ContentPlaceHolder1_ContentPlaceHolder2_txtDepositAmount').val(),
                        Email: $('#ContentPlaceHolder1_ContentPlaceHolder2_txtEmail').val(),
                        Phone: $('#txtContact').val(),
                        Bank: {
                            Text: $('#ContentPlaceHolder1_ContentPlaceHolder2_drpBanks option:selected').text(),
                            Value: $('#ContentPlaceHolder1_ContentPlaceHolder2_drpBanks').val()
                        },
                        ThankYouPage: location.protocol + "//" + location.host + "/Deposit/Thankyou.aspx"
                    };
                }

                window.w88Mobile.Gateways.Baokim.deposit(data, function (response) {
                    switch (response.ResponseCode) {
                        case 1:
                            window.w88Mobile.FormValidator.enableSubmitButton('#ContentPlaceHolder1_btnSubmit');

                            window.location.replace(response.ResponseData.PostUrl);
                            break;
                        default:
                            w88Mobile.Growl.shout(response.ResponseMessage);
                            break;
                    }
                },
                    function () {
                        w88Mobile.FormValidator.enableSubmitButton('#ContentPlaceHolder1_btnSubmit');
                        GPINTMOBILE.HideSplash();
                    });
            });
        });
    </script>
</asp:Content>
