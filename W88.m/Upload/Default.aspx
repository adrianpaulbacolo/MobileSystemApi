<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Upload_Default" %>

<!DOCTYPE html>
<html>
<head>
    <title><%=string.Format("{0} {1}", commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML), commonCulture.ElementValues.getResourceString("submitUpload", commonVariables.LeftMenuXML))%></title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/Js/Main.js"></script>
</head>
<body>
    <!--#include virtual="~/_static/splash.shtml" -->
    <div data-role="page" data-theme="b">
        <header data-role="header" data-theme="b" data-position="fixed" id="header">
            <a href="" role="button" data-rel="back" class="btn-clear ui-btn-left ui-btn"> < </a>
            <h1 class="title"><%=commonCulture.ElementValues.getResourceString("submitUpload", commonVariables.LeftMenuXML)%></h1>
        </header>
        <div class="ui-content" role="main">
            <form class="form" id="form1" runat="server" data-ajax="false">
                <ul class="list fixed-tablet-size">
                    <li class="item item-text-wrap">
                        <p><asp:Literal ID="lblSuccess" runat="server"></asp:Literal></p>
                        <p>
                            <strong><asp:Label ID="lblUsername" runat="server" /></strong>
                            <asp:Literal ID="txtUsername" runat="server" />
                        </p>
                        <p>
                            <strong><asp:Label ID="lblCurrency" runat="server" /></strong>
                            <asp:Literal ID="txtCurrency" runat="server" />
                        </p>
                    </li>
                    <li class="item item-input">
                        <asp:Label ID="lblFileUpload" runat="server" />
                        <asp:FileUpload ID="fuFileUpload" runat="server" AllowMultiple="false" />
                    </li>
                    <li class="item item-textarea">
                        <asp:Label ID="lblRemarks" runat="server" />
                        <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" />
                    </li>
                    <li class="item row">
                        <div class="col">
                            <a href="" role="button" data-rel="back" class="ui-btn btn-bordered">Cancel</a>
                        </div>
                        <div class="col">
                            <asp:Button data-theme="b" ID="btnSubmit" runat="server" Text="upload" data-corners="false" OnClick="btnSubmit_Click" />
                        </div>
                    </li>
                    <asp:HiddenField runat="server" ID="_repostcheckcode" />
                </ul>
            </form>
        </div>
        <!-- /content -->
        <!--#include virtual="~/_static/footer.shtml" -->
        <!--#include virtual="~/_static/navMenu.shtml" -->

        <script type="text/javascript">
            $(function () {
                $('.div-content-wrapper > div:first-child').hide();
                var code = '<%=strAlertCode%>';
                var message = '<%=strAlertMessage%>';

                switch (code) {
                    case "00":
                        $('.div-content-wrapper > div:nth-child(n+1)').hide();
                        $('.div-content-wrapper > div:first-child').show();
                        break;

                    case "01":
                        alert(message);
                        break;
                }
            });
        </script>
    </div>
</body>
</html>
