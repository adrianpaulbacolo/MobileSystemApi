<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Index" %>

<!DOCTYPE html>
<html>
<head>
    <title><%=commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML).Replace(" -", "")/* + commonCulture.ElementValues.getResourceString("home", commonVariables.LeftMenuXML)*/%></title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/Js/Main.js"></script>
    <link href="/_Static/Css/Catalogue.css" rel="stylesheet" />

    <script>
    </script>

</head>
<body>

    <div id="divMain" data-role="page" data-theme="b" data-ajax="false">
        <!--#include virtual="~/_static/header.shtml" -->

        <div class="ui-content" role="main">
            <div id="divLoginMessage" runat="server"><span id="lblLogin" runat="server">please login to start redemption</span></div>
            <div id="divLevel" runat="server" visible="False">
                <a id="lblPoint" runat="server" href="/Account" class="pointslink"></a>
            </div>

            <div class="page-content">
                <div id="categorywrapper" style="overflow: hidden;">
                    <div id="category-nav">
                        <asp:ListView ID="CategoryListView" runat="server">
                            <LayoutTemplate>
                                <ul id="categorylist" class="">
                                    <div runat="server" id="groupPlaceholder">
                                    </div>
                                </ul>
                            </LayoutTemplate>
                            <GroupTemplate>
                                <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                            </GroupTemplate>
                            <ItemTemplate>
                                <li>
                                    <a data-ajax="false" href="/Catalogue?categoryId=<%#DataBinder.Eval(Container.DataItem,"categoryId")%>&sortBy=2">
                                        <span class="image">
                                            <img src="<%#DataBinder.Eval(Container.DataItem,"imagePathOff")%>" data-imageover="<%#DataBinder.Eval(Container.DataItem,"imagePathOn")%>" /></span>
                                        <span class="label"><%#DataBinder.Eval(Container.DataItem,"categoryName")%></span>
                                    </a></li>

                            </ItemTemplate>
                        </asp:ListView>

                    </div>
                </div>
                <div id="productwrapper">
                    <div id="resultwrapper">
                        <div id="resultscroller">
                            <asp:Label ID="lblnodata" runat="server" CssClass="nodata" Text="Label" Visible="false"></asp:Label>
                            <asp:ListView ID="ListviewProduct" runat="server" GroupItemCount="5">
                                <LayoutTemplate>
                                    <ul>
                                        <div runat="server" id="groupPlaceholder">
                                        </div>
                                    </ul>

                                    <%-- <table id="tblProduct" style="align-content: center;" cellpadding="0" cellspacing="0">
                                        <div runat="server" id="groupPlaceholder">
                                        </div>
                                    </table>--%>
                                </LayoutTemplate>
                                <GroupTemplate>
                                    <tr>
                                        <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                                    </tr>
                                </GroupTemplate>
                                <ItemTemplate>
                                    <li class="hotitem">
                                        <div class="small-label">
                                            <%#(DataBinder.Eval(Container.DataItem, "productIcon").ToString()=="2") ? "<span class='productIcon'>"+  HttpContext.GetLocalResourceObject(localResx, "lbl_hot").ToString() +"</span>":"<span></span>"%>
                                            <%#(DataBinder.Eval(Container.DataItem, "productIcon").ToString()=="3") ? "<span class='productIcon'>"+HttpContext.GetLocalResourceObject(localResx, "lbl_new").ToString() +"</span>":"<span></span>"%>
                                        </div>


                                        <a data-role="button" data-rel="dialog" data-transition="slidedown" href="/Catalogue/Detail.aspx?id=<%#DataBinder.Eval(Container.DataItem,"productId")%>">
                                            <span class="image">
                                                <img src="<%#DataBinder.Eval(Container.DataItem,"imagePath")%>" data-imageover="" /></span>
                                            <span class="description">
                                                <%#DataBinder.Eval(Container.DataItem,"productName")%> 
                                            </span>

                                            <span class="points" style="<%#(DataBinder.Eval(Container, "DataItem.discountPoints").ToString() != "") ? "text-decoration:line-through;": "text-decoration:none;" %>"><%# String.Format("{0:#,###,##0.##}",DataBinder.Eval(Container.DataItem,"pointsRequired"))%> <%=HttpContext.GetLocalResourceObject(localResx, "lbl_points").ToString()%></span>
                                            <span class="newpoints" style="<%#(DataBinder.Eval(Container, "DataItem.discountPoints").ToString() != "") ? "visibility:visible;": "visibility:hidden;" %>"><%# String.Format("{0:#,###,##0.##}",DataBinder.Eval(Container.DataItem,"discountPoints"))%> <%=HttpContext.GetLocalResourceObject(localResx, "lbl_points").ToString()%></span>
                                        </a>
                                    </li>

                                </ItemTemplate>
                            </asp:ListView>

                        </div>
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
<script type="text/javascript">
    $(function () {
        CatScroll();
        //  ProductScroll();
        // document.addEventListener('touchmove', function (e) { e.preventDefault(); }, false);
    });

    function CatScroll() {
        var HorScroll = new IScroll('#categorywrapper', { scrollX: true, scrollY: false, mouseWheel: true, click: true });
    }

    function ProductScroll() {
        var VerScroll = new IScroll('#resultwrapper', { bounceEasing: 'elastic', bounceTime: 1200 });
    }

</script>

</html>
