<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title></title>
    <script type="text/javascript" src="/_Static/JS/jquery-1.10.2.min.js"></script>
    <script type="text/javascript">
        var strRedirectPathUrl = '<%=strRedirectPathUrl%>';
        var strQS = window.location.href.indexOf('?') > -1 ? window.location.href.substring(window.location.href.indexOf('?')) : '';

        $.ajax({
            contentType: "application/json; charset=utf-8",
            url: "https://ip2loc.w2script.com/IP2LOC?v=" + new Date().getTime(),
            dataType: "jsonp",
            success: function (data) {
                var strURL = '//m' + data.domainName + '/' + strRedirectPathUrl + strQS;
                window.location.assign(strURL);
            },
            error: function (err) { }
        });
    </script>
</head>
<body></body></html>