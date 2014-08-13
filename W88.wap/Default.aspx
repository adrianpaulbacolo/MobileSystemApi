<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML Basic 1.1//EN" "http://www.w3.org/TR/xhtml-basic/xhtml-basic11.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en">
<head runat="server">
    <title>W88.com</title>
    <script type="text/javascript" src="/_Static/JS/jquery-1.10.2.min.js"></script>
    <script type="text/javascript">
        $.ajax({
            contentType: "application/json; charset=utf-8",
            url: "https://ip2loc.w2script.com/IP2LOC?v=" + new Date().getTime(),
            dataType: "jsonp",
            success: function (data) {
                var strURL = '//wap' + data.domainName;
                //window.location.assign(strURL);
                $(function () {
                    $('#tblLanguage a').each(function () {
                        $(this).attr('href', $(this).attr('href') + '&Code=' + data.country + '&IP=' + data.ip);
                    });
                });
            },
            error: function (err) { }
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <span id="txtMessage" runat="server"></span>
            <table id="tblLanguage" runat="server">
            </table>
        </div>
    </form>
</body>
</html>
