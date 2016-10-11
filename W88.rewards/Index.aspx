<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Index.aspx.cs" Inherits="_Index" Async="true"%>
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
            <div class="wallet-box" id="divLvl" runat="server" visible="False">
                <h4 id="usernameLabel" runat="server"></h4>
                <a id="pointsLabel" runat="server" data-ajax="false" href="/Account"></a>                
                <span id="pointLevelLabel" runat="server"></span>
            </div>
            <div class="container">
                <div class="row">
                    <asp:ListView ID="Listview1" runat="server">
                        <ItemTemplate>
                            <div class="col-xs-6 col-sm-3">
                                <div class="catalog-category-box">                                                                             
                                    <div class="catalog-category-image">                                               
                                        <a data-ajax="false" href="/Catalogue/?categoryId=<%#DataBinder.Eval(Container.DataItem,"categoryId")%>&sortBy=2">
                                            <img src="<%#DataBinder.Eval(Container.DataItem,"imagePathOff")%>" data-imageover="<%#DataBinder.Eval(Container.DataItem,"imagePathOn")%>" />
                                        </a>
                                    </div>
                                    <div class="catalog-category-details">
                                        <h4><%#DataBinder.Eval(Container.DataItem,"categoryName")%></h4>
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:ListView>
                </div>
            </div>
        </div>
        <!-- /content -->
    </div>
    <!-- /page -->
</body>
</html>
