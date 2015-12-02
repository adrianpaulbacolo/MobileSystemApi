<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="FundTransfer_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML Basic 1.1//EN" "http://www.w3.org/TR/xhtml-basic/xhtml-basic11.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en">
<head runat="server">
    <title>W88.com</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table>
                <tr>
                    <td><span id="lblTransferFrom" runat="server"></span></td>
                    <td>
                        <select id="drpTransferFrom" runat="server"></select></td>
                </tr>
                <tr>
                    <td><span id="lblTransferTo" runat="server"></span></td>
                    <td>
                        <select id="drpTransferTo" runat="server"></select></td>
                </tr>
                <tr>
                    <td><span id="lblTransferAmount" runat="server"></span></td>
                    <td>
                        <input id="txtTransferAmount" runat="server" type="text" /></td>
                </tr>
                <!-- 
                <tr>
                    <td><span id="lblPromoCode" runat="server"></span></td>
                    <td>
                        <input id="txtPromoCode" runat="server" type="text" /></td>
                </tr>
                -->
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
        if ('<%=strAlertMessage%>'.length != 0) { }
    </script>
</body>
</html>
