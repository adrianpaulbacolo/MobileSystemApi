<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Pay.aspx.cs" Inherits="v2_Deposit_Pay" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/modules/cookie.min.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
    <!--#include virtual="~/_static/v2/head.inc" -->
    <script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/gateways/defaultpayments.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
    <script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/gateways/gateway.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
    <script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/gateways/<%=base.GatewayFile %>.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
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
