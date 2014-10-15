<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Index.aspx.cs" Inherits="_Index" %>

<!DOCTYPE html>
<html>
<head>
    <title><%=commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML).Replace(" -", "")/* + commonCulture.ElementValues.getResourceString("home", commonVariables.LeftMenuXML)*/%></title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="~/_Static/Js/Main.js"></script>

    <!--[if IE]><link type="text/css" href="/_Static/Css/Index.css" rel="stylesheet"><![endif]-->
    <!--[if !IE]><!-->
    <link type="text/css" href="/_Static/Css/IndexScroll.css" rel="stylesheet">
    <!--<![endif]-->

    <%--  <script type="text/javascript">
        $(document).ready(function () {
            ProductScroll();
            document.addEventListener('touchmove', function (e) { e.preventDefault(); }, false);
        });


        var VerScroll;
        function ProductScroll() {
            VerScroll = new IScroll('#divContent', { bounceEasing: 'elastic', bounceTime: 1200 });
        }

    </script>--%>
</head>
<body>
    <!--#include virtual="~/_static/splash.shtml" -->
    <div id="divMain" data-role="page" data-theme="b" data-ajax="false">
        <!--#include virtual="~/_static/header.shtml" -->
        <div class="ui-content" role="main">
            <div id="divLoginMessage" runat="server"><span id="lblLogin" runat="server">please login to start redemption</span></div>
            <div id="divLevel" runat="server" visible="False">
                <span id="lblPoint" runat="server"></span>
            </div>
            <div class="page-content">
                <div id="divContent">
                    <div class="div-product-scroll">
                        <asp:ListView ID="Listview1" runat="server" GroupItemCount="3">
                            <LayoutTemplate>
                              <ul id="CatalogueUL">
                                    <div runat="server" id="groupPlaceholder">
                                    </div>
                                </ul>
                            </LayoutTemplate>
                            <GroupTemplate>
                                <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                            </GroupTemplate>
                            <ItemTemplate>
                                <li>
                                    <% if (string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId))
                                       { %>
                                    <a data-ajax="false" href="/_Secure/Login.aspx?redirect=/Catalogue&categoryId=<%#DataBinder.Eval(Container.DataItem,"categoryId")%>&sortBy=2" data-rel="dialog" data-transition="slidedown"><% }
                                       else
                                       {%>
                                        <a data-ajax="false" href="/Catalogue/?categoryId=<%#DataBinder.Eval(Container.DataItem,"categoryId")%>&sortBy=2"><% }%>
                                            <img src="<%#DataBinder.Eval(Container.DataItem,"imagePathOff")%>" data-imageover="<%#DataBinder.Eval(Container.DataItem,"imagePathOn")%>" />
                                            <br />
                                            <div class="catName"><%#DataBinder.Eval(Container.DataItem,"categoryName")%></div>
                                        </a></li>
                            </ItemTemplate>
                        </asp:ListView>
                    </div>

                </div>
            </div>
        </div>
        <!-- /content -->
        <!--#include virtual="~/_static/footer.shtml" -->
        <!--#include virtual="~/_static/navMenu.shtml" -->
    </div>
    <!-- /page -->
</body>
</html>
