<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Pay.aspx.cs" Inherits="Deposit_Pay" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <!--#include virtual="~/_static/head.inc" -->
    <%--<script type="text/javascript" src="/_Static/JS/PreLoad.js"></script>--%>
    <script type="text/javascript" src="/_Static/JS/modules/gateways/gateway.js"></script>
    <script type="text/javascript" src="/_Static/JS/modules/gateways/ecpsspay.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
</body>
</html>
<script type="text/javascript">
    $(document).ready(function () {
        var ecpss = w88Mobile.Gateways.ECPSSPay;
        ecpss.createDeposit();
    });
</script>
