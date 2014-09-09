<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Upload_Default" %>

<!DOCTYPE html>
<html>
<head>
    <title><%=string.Format("{0} {1}", commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML), commonCulture.ElementValues.getResourceString("submitUpload", commonVariables.LeftMenuXML))%></title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/Js/Main.js"></script>
    <style type="text/css">
        .page-content {
            max-width: 750px;
            margin-left: auto;
            margin-right: auto;
        }

        .div-content-wrapper > div > div > div {
            display:table-cell;
            vertical-align:middle;
        }
    </style>
</head>
<body>
    <!--#include virtual="~/_static/splash.shtml" -->
    <div data-role="page" data-theme="a">
        <!--#include virtual="~/_static/header.shtml" -->
        <div class="ui-content" role="main">
            <div class="div-page-header"><span><%=commonCulture.ElementValues.getResourceString("submitUpload", commonVariables.LeftMenuXML)%></span></div>
            <div class="page-content">
                <form id="form1" runat="server" data-ajax="false">
                    <div class="div-content-wrapper">
                        <div>
                            <div>
                                <asp:Literal ID="lblSuccess" runat="server"></asp:Literal></div>
                        </div>
                        <div>
                            <div>
                                <div>
                                    <asp:Label ID="lblUsername" runat="server" /></div>
                                <div>
                                    <asp:Literal ID="txtUsername" runat="server" /></div>
                            </div>
                        </div>
                        <div>
                            <div>
                                <div>
                                    <asp:Label ID="lblCurrency" runat="server" /></div>
                                <div>
                                    <asp:Literal ID="txtCurrency" runat="server" /></div>
                            </div>
                        </div>
                        <div>
                            <div>
                                <asp:Label ID="lblFileUpload" runat="server" />
                                <asp:FileUpload ID="fuFileUpload" runat="server" AllowMultiple="false" />
                            </div>
                        </div>
                        <div>
                            <div><asp:Label ID="lblRemarks" runat="server" /><asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" />                          
                            </div>
                        </div>
                        <asp:Button data-theme="b" ID="btnSubmit" runat="server" Text="upload" CssClass="button-blue" data-corners="false" OnClick="btnSubmit_Click" />
                        <asp:HiddenField runat="server" ID="_repostcheckcode" />
                    </div>
                </form>
            </div>
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
                        $('.div-content-wrapper > div:first-child').show();
                        $('.div-content-wrapper > div:nth-child(n+1)').hide();
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
