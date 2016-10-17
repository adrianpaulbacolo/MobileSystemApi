<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Default" Async="true"%>
<%@ Import Namespace="W88.BusinessLogic.Rewards.Helpers" %>
<%@ Import Namespace="W88.BusinessLogic.Rewards.Models" %>
<%@ Import Namespace="W88.BusinessLogic.Shared.Helpers" %>

<!DOCTYPE html>
<html>
<head>
    <title><%=RewardsHelper.GetTranslation(TranslationKeys.Label.Brand)%></title>
    <!--#include virtual="~/_static/head.inc" -->
</head>
<body>
    <div data-role="page" data-theme="b">
        <!--#include virtual="~/_static/header.shtml" -->
        <div class="main-content has-footer" role="main">
            <div id="divLevel" class="wallet-box" runat="server" visible="False">
                <h4 id="usernameLabel" runat="server"></h4>
                <a id="pointsLabel" runat="server" data-ajax="false" href="/Account"></a>                
                <span id="pointLevelLabel" runat="server"></span>
            </div>        
            <div class="container">
                <div class="row">             
                    <asp:Label ID="lblnodata" runat="server" CssClass="nodata" Text="Label" Visible="false"></asp:Label>
                    <asp:ListView ID="ListviewProduct" runat="server" GroupItemCount="5">
                        <LayoutTemplate>
                            <div runat="server" id="groupPlaceholder"></div>
                        </LayoutTemplate>
                        <GroupTemplate>
                            <tr>
                                <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                            </tr>
                        </GroupTemplate>
                        <ItemTemplate>
                            <div id="detailsButton_<%#DataBinder.Eval(Container.DataItem,"productId")%>" class="col-xs-6 col-sm-3">
                                <div class="catalog-box">
                                    <script>
                                        $(function () {
                                            $('#detailsButton_<%#DataBinder.Eval(Container.DataItem,"productId")%>').on('click', function () {
                                                window.location.href = '/Catalogue/Detail.aspx?id=<%#DataBinder.Eval(Container.DataItem,"productId")%>';
                                            });
                                            var labelTag = $('#labelTag_<%#DataBinder.Eval(Container.DataItem,"productId")%>');
                                            if (_.isEmpty(labelTag.html().trim()))
                                                labelTag.removeClass('tag-label');
                                        });
                                    </script>
                                    <div class="catalog-image">
                                        <img src="<%#DataBinder.Eval(Container.DataItem,"imagePath")%>" data-imageover="" alt=""/>
                                    </div>
                                    <div class="catalog-details">
                                        <h4><%#DataBinder.Eval(Container.DataItem,"productName")%></h4>
                                        <small>
                                            <span class="points" style="<%#(DataBinder.Eval(Container, "DataItem.discountPoints").ToString() != "") ? "text-decoration:line-through;": "text-decoration:none;" %>"><%# String.Format("{0:#,###,##0.##}",DataBinder.Eval(Container.DataItem,"pointsRequired"))%> <%=RewardsHelper.GetTranslation(TranslationKeys.Label.Points).ToLower()%></span>
                                            <span class="newpoints" style="<%#(DataBinder.Eval(Container, "DataItem.discountPoints").ToString() != "") ? "visibility:visible;": "visibility:hidden;" %>"><%# String.Format("{0:#,###,##0.##}",DataBinder.Eval(Container.DataItem,"discountPoints"))%> <%=RewardsHelper.GetTranslation(TranslationKeys.Label.Points)%></span>   
                                            <span id="labelTag_<%#DataBinder.Eval(Container.DataItem,"productId")%>" class="tag-label">
                                                <%#(DataBinder.Eval(Container.DataItem, "productIcon").ToString()=="2") ? RewardsHelper.GetTranslation(TranslationKeys.Label.Hot) : ""%>
                                                <%#(DataBinder.Eval(Container.DataItem, "productIcon").ToString()=="3") ? RewardsHelper.GetTranslation(TranslationKeys.Label.New) : ""%>                                 
                                            </span>                                                 
                                        </small> 
                                    </div>                                        
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:ListView>
                </div>
            </div>
        </div>
        <div class="footer footer-generic">
            <div class="btn-group btn-group-justified btn-group-sliding" role="group">
                <asp:ListView ID="CategoryListView" runat="server">
                    <ItemTemplate>
                        <div class="btn-group" role="group">
                            <script>
                                $(function() {
                                    if (_.endsWith(window.location.href, 'categoryId=<%#DataBinder.Eval(Container.DataItem,"categoryId")%>&sortBy=2')) {
                                        if (!$('#category_<%#DataBinder.Eval(Container.DataItem,"categoryId")%>').hasClass('active')) 
                                            $('#category_<%#DataBinder.Eval(Container.DataItem,"categoryId")%>').addClass('active');                                       
                                    }
                                });
                            </script>
                            <a id="category_<%#DataBinder.Eval(Container.DataItem,"categoryId")%>" class="btn" data-ajax="false" href="/Catalogue?categoryId=<%#DataBinder.Eval(Container.DataItem,"categoryId")%>&sortBy=2">
                                <%#DataBinder.Eval(Container.DataItem,"categoryName")%>
                            </a>
                        </div>
                    </ItemTemplate>
                </asp:ListView>      
            </div>
        </div>
    </div>
    <!-- /page -->
</body>
</html>
