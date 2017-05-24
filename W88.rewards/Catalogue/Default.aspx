<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Default" Async="true"%>
<%@ Import Namespace="W88.BusinessLogic.Rewards.Helpers" %>
<%@ Import Namespace="W88.BusinessLogic.Rewards.Models" %>

<!DOCTYPE html>
<html>
<head>
    <title><%=RewardsHelper.GetTranslation(TranslationKeys.Label.Brand)%></title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/catalogue/catalogue.js"></script>
    <script>
        var ids = [];
    </script>
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
                    <asp:Label ID="lblnodata" runat="server" CssClass="nodata" Text="Label" Visible="false"><%=RewardsHelper.GetTranslation(TranslationKeys.Redemption.NoAvailableItems, Language)%></asp:Label>
                    <div id="listContainer"></div>
                </div>
            </div>
        </div>
        <div class="footer footer-generic">
            <div class="btn-group btn-group-justified btn-group-sliding" role="group">
                <asp:ListView ID="CategoryListView" runat="server">
                    <ItemTemplate>
                        <div id="category_<%#DataBinder.Eval(Container.DataItem,"categoryId")%>" class="btn-group" role="group">
                            <script>
                                ids.push('<%#DataBinder.Eval(Container.DataItem,"categoryId")%>');
                            </script>
                            <a class="btn" data-ajax="false" href="/Catalogue?categoryId=<%#DataBinder.Eval(Container.DataItem,"categoryId")%>&sortBy=2">
                                <%#DataBinder.Eval(Container.DataItem,"categoryName")%>
                            </a>
                        </div>
                    </ItemTemplate>
                </asp:ListView>      
            </div>
        </div>
    </div>
    <script>
        var catalogue,
            translations = {
                labelHot: '<%=RewardsHelper.GetTranslation(TranslationKeys.Label.Hot, Language)%>',
                labelNew: '<%=RewardsHelper.GetTranslation(TranslationKeys.Label.New, Language)%>',
                labelPoints: '<%=RewardsHelper.GetTranslation(TranslationKeys.Label.Points, Language).ToLower()%>'
            };
        $(function() {
            catalogue = new Catalogue({
                language: '<%=Language%>', 
                token: '<%=Token%>', 
                params: JSON.parse('<%=Params%>'), 
                translations: translations, 
                elem: $('#listContainer')
            });
            catalogue.cacheProducts();
            catalogue.getProducts();

            $(window).scroll(function () {
                if (($(window).scrollTop() + $(window).height() < $(document).height() - 65)
                    || catalogue.isSearching) 
                    return;
                
                catalogue.isSearching = true;
                catalogue.getProducts();
            });

            var children = $('div.btn-group.btn-group-justified.btn-group-sliding').children();
            _.find(ids, function(id) {
                var selector = '#category_' + id,
                    categoryId = selector.substring(1);

                if (_.endsWith(window.location.href, 'categoryId=' + id + '&sortBy=2')) {
                    if (!$(selector).hasClass('active'))
                        $(selector).addClass('active');

                    var index = _.findIndex(children, { id: categoryId }),
                        width = 0;
                    for (var i = 0; i < index; i++) {
                        width += $($(children[i]).find('a')[0]).width();
                    }
                    $('div.footer.footer-generic').scrollLeft(width);
                    return id;
                } 
            });
        });
    </script>
</body>
</html>
