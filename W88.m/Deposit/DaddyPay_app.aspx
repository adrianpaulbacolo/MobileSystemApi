<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DaddyPay.aspx.cs" Inherits="Deposit_DaddyPay" %>

<!DOCTYPE html>
<html>
<head>
    <title><%=string.Format("{0} {1}", commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML), commonCulture.ElementValues.getResourceString("dDaddyPay", commonVariables.PaymentMethodsXML))%></title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/Js/Main.js"></script>
</head>
<body>
    <!--#include virtual="~/_static/splash.shtml" -->
    <div data-role="page" data-theme="b">
        <header data-role="header" data-theme="b" data-position="fixed" id="header">
            <h1 class="title"><%=string.Format("{0} - {1}", commonCulture.ElementValues.getResourceString("deposit", commonVariables.LeftMenuXML), commonCulture.ElementValues.getResourceString("dDaddyPay", commonVariables.PaymentMethodsXML))%></h1>
        </header>

        <div class="ui-content" role="main">
            <div class="wallet main-wallet">
                <label class="label"><%=commonCulture.ElementValues.getResourceString("mainWallet", commonVariables.LeftMenuXML)%></label>
                <h2 class="value"><%=Session["Main"].ToString()%></h2>
                <small class="currency"><%=commonVariables.GetSessionVariable("CurrencyCode")%></small>
            </div>

            <div data-role="navbar">
                <ul id="depositTabs" runat="server">
                </ul>
            </div>

            <form class="form" id="form1" runat="server" data-ajax="false">
                <br>
                <ul class="list fixed-tablet-size">
                    <li class="row">
                        <div class="col">
                            <asp:Literal ID="lblMode" runat="server" />
                        </div>
                        <div class="col">
                            <asp:Literal ID="txtMode" runat="server" />
                        </div>
                    </li>
                    <li class="row">
                        <div class="col">
                            <asp:Literal ID="lblMinMaxLimit" runat="server" />
                        </div>
                        <div class="col">
                            <asp:Literal ID="txtMinMaxLimit" runat="server" />
                        </div>
                    </li>
                    <li class="row">
                        <div class="col">
                            <asp:Literal ID="lblDailyLimit" runat="server" />
                        </div>
                        <div class="col">
                            <asp:Literal ID="txtDailyLimit" runat="server" />
                        </div>
                    </li>
                    <li class="row">
                        <div class="col">
                            <asp:Literal ID="lblTotalAllowed" runat="server" />
                        </div>
                        <div class="col">
                            <asp:Literal ID="txtTotalAllowed" runat="server" />
                        </div>
                    </li>
                         <li class="item item-select">
                        <asp:DropDownList ID="drpBank" runat="server" />
                    </li>
                    <li class="item item-input" id="txtAmount">
                        <asp:Label ID="lblDepositAmount" runat="server" AssociatedControlID="txtDepositAmount" />
                        <asp:TextBox ID="txtDepositAmount" runat="server" type="number" step="any" min="1" data-clear-btn="true" />
                    </li>
                    <li class="item item-select" id="drpAmount" style="display: none;">
                        <asp:DropDownList ID="drpDepositAmount" runat="server" />
                    </li>
                    <li class="item item-input" id="accountName" runat="server">
                        <asp:Label ID="lblAccountName" runat="server" AssociatedControlID="txtAccountName" />
                        <asp:TextBox ID="txtAccountName" runat="server" data-clear-btn="true" />
                        <asp:HiddenField ID="hfWCNickname"  runat="server" ClientIDMode="Static" />
                    </li>
                    <li class="item item-input" id="accountNo" runat="server">
                        <asp:Label ID="lblAccountNumber" runat="server" AssociatedControlID="txtAccountNo" />
                        <asp:TextBox ID="txtAccountNo" type="number" runat="server" data-clear-btn="true" />
                    </li>
                    <li class="item row">
                        <div class="col">
                            <asp:Button data-theme="b" ID="btnSubmit" runat="server" Text="login" CssClass="button-blue" data-corners="false" OnClick="btnSubmit_Click" />
                        </div>
                    </li>
                    <asp:HiddenField runat="server" ID="_repostcheckcode" />
                </ul>

                <div class="row">
                    <div class="col">
                        <input type="button" data-theme="b" onclick="location.href = '/Withdrawal/Withrawal.aspx?source=app';" value="<%=commonCulture.ElementValues.getResourceString("withrawal", commonVariables.LeftMenuXML)%>" class="button-blue" data-corners="false" />
                    </div>
                    <div class="col">
                        <input type="button" data-theme="b" onclick="location.href = '/FundTransfer/FundTransfer.aspx';" value="<%=commonCulture.ElementValues.getResourceString("fundTransfer", commonVariables.LeftMenuXML)%>" class="button-blue" data-corners="false" />
                    </div>
                </div>

            </form>
        </div>
        <!-- /content -->
        <script type="text/javascript">
            $(function () {
                window.history.forward();

                if ('<%=strAlertCode%>'.length > 0) {
                    switch ('<%=strAlertCode%>') {
                        case '-1':
                            alert('<%=strAlertMessage%>');
                            tooglePaymentMethod($('#drpBank').val());
                            break;
                        case '0':
                            break;
                        default:
                            break;
                    }
                }

                $('#drpBank').change(function () {
                    var bId = this.value;

                    tooglePaymentMethod(bId)
                });

                function tooglePaymentMethod(bId) {
                    $("#txtAccountName").val('');

                    if (bId == "40") { //WeChat
                        $("#txtAmount").hide();
                        $("#drpAmount").show();
                        $("#accountNo").hide();

                        populateWeChatNickName();
                    }
                    else { //QR
                        $("#txtAmount").show();
                        $("#drpAmount").hide();
                        $("#accountNo").show();
                    }
                }

                function populateWeChatNickName() {
                    $.ajax({
                        type: "POST",
                        async: false,
                        url: "DaddyPay.aspx/ProcessWeChatNickname",
                        data: JSON.stringify({ action: "getNickname", nickname: "" }),
                        contentType: "application/json;",
                        dataType: "json",
                        success: function (response) {
                            var result = response.d;
                            $('#txtAccountName').val(result);
                            $('#hfWCNickname').val(result); //store original nickname if any.
                        }
                    })
                }

            });
        </script>
    </div>
</body>
</html>
