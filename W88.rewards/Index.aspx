<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Index.aspx.cs" Inherits="_Index" %>




<!DOCTYPE html>
<html>
<head>
    <title><%=commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML).Replace(" -", "")/* + commonCulture.ElementValues.getResourceString("home", commonVariables.LeftMenuXML)*/%></title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/Js/Main.js"></script>
    <link rel="stylesheet" href="/_Static/Css/add2home.css">
    <script type="application/javascript" src="/_Static/Js/add2home.js"></script>
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
            <div id="divContent">
                <div class="div-product-scroll">
                  
                        <asp:ListView ID="Listview1" runat="server" GroupItemCount="3">
                            <LayoutTemplate>
                           <%--     <table id="tblCatalogue">
                                    <div runat="server" id="groupPlaceholder">
                                    </div>
                                </table>--%>
                                
                                <ul id="CatalogueUL" >
                                     <div runat="server" id="groupPlaceholder">
                                    </div>
                                </ul>
                            </LayoutTemplate>
                            <GroupTemplate>
                               <%-- <tr>--%>
                                    <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                                <%--</tr>--%>
                            </GroupTemplate>
                            <ItemTemplate>
                               <%-- <td>--%>
                                <li><a  href="/Product?categoryId=<%#DataBinder.Eval(Container.DataItem,"categoryId")%>&sortBy=2">
                                        <img src="<%#DataBinder.Eval(Container.DataItem,"imagePathOff")%>" data-imageover="<%#DataBinder.Eval(Container.DataItem,"imagePathOn")%>" />
                                        <br />
                                        <div class="catName"><%#DataBinder.Eval(Container.DataItem,"categoryName")%></div>
                                     
                                    </a></li>
                                    
                               <%-- </td>--%>

                               <%-- <td style="width: 5%; text-align: center;"></td>--%>
                            </ItemTemplate>
                        </asp:ListView>

                    
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
