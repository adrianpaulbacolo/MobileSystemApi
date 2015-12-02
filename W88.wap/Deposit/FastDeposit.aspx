<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FastDeposit.aspx.cs" Inherits="Deposit_FastDeposit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML Basic 1.1//EN" "http://www.w3.org/TR/xhtml-basic/xhtml-basic11.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table>
                <tr>
                    <td><span id="lblAmount" runat="server"></span></td>
                    <td>
                        <input id="txtAmount" runat="server" type="text" /></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td><span id="lblDailyLimit" runat="server"></span></td>
                </tr>    
                <tr>
                    <td>&nbsp;</td>
                    <td><span id="lblTotalAllowed" runat="server"></span></td>
                </tr>            
                <tr>
                    <td><span id="lblReferenceId" runat="server"></span></td>
                    <td>
                        <input id="txtReferenceId" runat="server" type="text" /></td>
                </tr>
                <tr>
                    <td><span id="lblSystemAccount" runat="server"></span></td>
                    <td>
                        <select id="drpSystemAccount" runat="server"></select></td>
                </tr>
                <tr>
                    <td><span id="lblDepositDateTime" runat="server"></span></td>
                    <td><select id="drpDepositDate" runat="server"></select>
                        <select id="drpHour" runat="server"></select>
                        <select id="drpMinute" runat="server"></select>
                    </td>
                </tr>
                <tr>
                    <td><span id="lblDepositChannel" runat="server"></span></td>
                    <td><select id="drpDepositChannel" runat="server"></select>
                    </td>
                </tr>
                <tr>
                    <td><span id="lblBank" runat="server"></span></td>
                    <td><select id="drpBank" runat="server"></select>
                    </td>
                </tr>
                <tr>
                    <td><span id="lblBankName" runat="server"></span></td>
                    <td>
                        <input id="txtBankName" runat="server" type="text" /></td>
                </tr>
                <tr>
                    <td><span id="lblAccountName" runat="server"></span></td>
                    <td>
                        <input id="txtAccountName" runat="server" type="text" /></td>
                </tr>
                <tr>
                    <td><span id="lblAccountNumber" runat="server"></span></td>
                    <td>
                        <input id="txtAccountNumber" runat="server" type="text" /></td>
                </tr>
                <tr id="trMessage">
                    <td>&nbsp;</td>
                    <td><span id="txtMessage" runat="server"></span></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <input id="btnSubmit" runat="server" type="submit" onserverclick="btnSubmit_Click" />&nbsp;<input id="btnBack" runat="server" type="button" onclick="javascript: window.location.href = '/index';" /></td>
                </tr>
            </table>
        </div>
    </form>
    <script type="text/javascript">
    </script>
</body>
</html>
