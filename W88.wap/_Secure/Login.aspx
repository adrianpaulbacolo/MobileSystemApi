<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="_Secure_Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML Basic 1.1//EN" "http://www.w3.org/TR/xhtml-basic/xhtml-basic11.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en">
<head runat="server">
    <title>W88.com</title>
    <script type="text/javascript" src="/_Static/JS/jquery-1.10.2.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table>
                <tr>
                    <td><span id="lblUsername" runat="server"></span></td>
                    <td>
                        <input type="text" runat="server" id="txtUsername" maxlength="16" /></td>
                </tr>
                <tr>
                    <td><span id="lblPassword" runat="server"></span></td>
                    <td>
                        <input type="password" runat="server" id="txtPassword" maxlength="10" /></td>
                </tr>
                <tr>
                    <td><span id="lblCaptcha" runat="server"></span></td>
                    <td>
                        <input type="text" runat="server" id="txtCaptcha" maxlength="4" /></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <img src="/Captcha" style="width: 150px;" /></td>
                </tr>
                <tr id="trMessage">
                    <td>&nbsp;</td>
                    <td><span id="txtMessage" runat="server"></span></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <input type="submit" runat="server" id="btnSubmit" onserverclick="btnSubmit_Click" /></td>
                </tr>
            </table>
        </div>
        <input type="hidden" id="ioBlackBox" runat="server" value="" />
        <input type="hidden" id="hidCode" runat="server" value="" />
        <input type="hidden" id="hidIP" runat="server" value="" />
    </form>
    <script type="text/javascript">
        if ('<%=strProcessMessage%>'.length != 0) {
            document.getElementById('txtCaptcha').value = '';
        }

        $(function () {
            if ($('#hidCode').val().length == 0) {
                $.ajax({
                    contentType: "application/json; charset=utf-8",
                    url: "https://ip2loc.w2script.com/IP2LOC?v=" + new Date().getTime(),
                    dataType: "jsonp",
                    success: function (data) {
                        $('#hidCode').val(data.country);
                        $('#hidIP').val(data.ip);
                    },
                    error: function (err) { }
                });
            }
        });
    </script>
    <script type="text/javascript" id="iovs_script">
        var io_operation = 'ioBegin';
        var io_bbout_element_id = 'ioBlackBox';
        //var io_submit_element_id = 'btnSubmit';
        var io_submit_form_id = 'form1';
        var io_max_wait = 5000;
        var io_install_flash = false;
        var io_install_stm = false;
        var io_exclude_stm = 12;
    </script>
    <script type="text/javascript" src="//ci-mpsnare.iovation.com/snare.js"></script>
</body>
</html>
