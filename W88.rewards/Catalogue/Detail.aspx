<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Detail.aspx.cs" Inherits="Catalogue_Detail" Async="true"%>
<%@ Import Namespace="W88.BusinessLogic.Rewards.Helpers" %>
<%@ Import Namespace="W88.BusinessLogic.Rewards.Models" %>

<!DOCTYPE html>
<html>
<head>
    <title>Product Detail</title>
    <!--#include virtual="~/_static/head.inc" -->
</head>
<body>
    <div data-role="page" data-theme="b">
        <!--#include virtual="~/_static/header.shtml" -->
        <form id="form1" runat="server">
            <div class="main-content has-footer" role="main">
                <div id="divLevel" class="wallet-box" runat="server" visible="False">
                    <h4 id="usernameLabel" runat="server"></h4>
                    <a id="pointsLabel" runat="server" data-ajax="false" href="/Account"></a>                
                    <span id="pointLevelLabel" runat="server"></span>
                </div>    
                <div class="container">
                    <div class="catalog-detail-box">
                        <a href="#" class="catalog-detail-image">
                            <asp:Image ID="imgPic" runat="server" />
                        </a>
                        <div class="catalog-details">
                            <h4>
                                <asp:Label ID="lblBeforeDiscount" runat="server" Style="text-decoration: line-through;" Visible="true" data-mini="true" Text="39" />
                                <asp:Label ID="lblPointCenter" runat="server" />
                            </h4>
                            <p><asp:Label ID="lblName" runat="server" Visible="false" /></p>                       
                        </div>
                    </div>     
                    <div class="catalog-information">
                        <strong><%=RewardsHelper.GetTranslation(TranslationKeys.Label.ProductDescription) %></strong>
                        <p><asp:Label ID="lblDescription" runat="server" /></p>
                        <div id="CurrencyDiv" runat="server" class="ui-field-contain ui-hide-label" visible="false">
                            <div>
                                <div>
                                    <asp:Label ID="lbcurr" runat="server" Text="" data-mini="true" />
                                </div>
                                <div>
                                    <asp:Label ID="lblCurrency" runat="server" data-mini="true" />
                                </div>
                            </div>
                        </div>
                        <div id="DeliveryDiv" class="ui-field-contain ui-hide-label" runat="server" visible="false">
                            <div>
                                <div>
                                    <asp:Label ID="lbperiod" runat="server" Text="" data-mini="true" />
                                </div>
                                <div>
                                    <asp:Label ID="lblDelivery" runat="server" data-mini="true" />
                                </div>
                            </div>
                        </div>
                    </div>                  
                </div>
            </div>
            <div class="footer">
                <% if (HasSession)
                    {
                        if (IsValidRedemption && !IsVipOnly)
                        {  %>
                            <input type="button" id="validButton" class="btn btn-block btn-primary" value="<%=RewardsHelper.GetTranslation(TranslationKeys.Redemption.Redeem)%>" />
                    <%  }
                        else if ((IsVipOnly && !IsValidRedemption) || (IsValidRedemption && (IsLimitReached || IsPending)))
                        { %>
                            <input type="button" id="vipOnlyButton" class="btn btn-block btn-primary" value="<%=RewardsHelper.GetTranslation(TranslationKeys.Redemption.Redeem)%>" />
                    <%  }
                          %>                          
                    <%  
                    }
                    else
                    {%>
                        <input type="button" id="noSessionButton" class="btn btn-block btn-primary" value="<%=RewardsHelper.GetTranslation(TranslationKeys.Redemption.Redeem)%>" />
                <%  }%>
            </div>
            <asp:HiddenField ID="hiddenproductitd" runat="server" />
        </form>
        <script type="text/javascript">
            $('#validButton').on('click', function () {
                window.location.href = '<%= RedirectUri %>';
                return false;
            });
            $('#vipOnlyButton').on('click', function () {
                var message = '<%= VipOnlyMessage %>';
                if (_.isEmpty(message)) return false;
                window.w88Mobile.Growl.shout(message);
                return false;
            });
            $('#noSessionButton').on('click', function () {
                window.location.href = '<%= RedirectUri %>';
                return false;
            });
        </script>
    </div>
</body>
</html>
