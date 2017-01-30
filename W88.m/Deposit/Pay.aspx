<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Pay.aspx.cs" Inherits="Deposit_Pay" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <!--#include virtual="~/_static/head.inc" -->
    <%--<script type="text/javascript" src="/_Static/JS/PreLoad.js"></script>--%>
    <script type="text/javascript" src="/_Static/JS/modules/gateways/defaultpayments.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
    <script type="text/javascript" src="/_Static/JS/modules/gateways/gateway.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
    <script type="text/javascript" src="/_Static/JS/modules/gateways/<%=base.GatewayFile %>.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
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
        var gateway = _w88_<%=base.GatewayFile %>;
        gateway.createDeposit();
    });
</script>
