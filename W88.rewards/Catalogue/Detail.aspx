<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Detail.aspx.cs" Inherits="Catalogue_Detail" %>

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
                    <p id="pointsLabel" runat="server"></p>                
                    <span id="pointLevelLabel" runat="server"></span>
                </div>    
                <div class="container">
                    <div class="catalog-detail-box">
                        <a href="#" class="catalog-detail-image">
                            <asp:Image ID="imgPic" runat="server" />
                        </a>
                        <div class="catalog-details">
                            <h4><asp:Label ID="lblPointCenter" runat="server" /></h4>
                            <p><asp:Label ID="lblName" runat="server" Visible="false" /></p>                       
                        </div>
                    </div>     
                    <div class="catalog-information">
                        <strong><%=HttpContext.GetLocalResourceObject(LocalResx, "lbl_product_desc").ToString() %></strong>
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
                        if (IsValidRedemption)
                        {  %>
                            <input type="button" id="validButton" class="btn btn-block btn-primary" value="<%=HttpContext.GetLocalResourceObject(LocalResx, "lbl_redeem").ToString() %>" />
                    <%  }
                        else if (IsRedemptionLimitReached)
                        { %>
                            <input type="button" id="limitButton" class="btn btn-block btn-primary" value="<%=HttpContext.GetLocalResourceObject(LocalResx, "lbl_redeem").ToString() %>" />
                    <%  }
                        else if (IsProcessingLimitReached)
                        { %>
                            <input type="button" id="processingButton" class="btn btn-block btn-primary" value="<%=HttpContext.GetLocalResourceObject(LocalResx, "lbl_redeem").ToString() %>" />
                    <%  }
                        else
                        { %>
                            <input type="button" id="vipOnlyButton" class="btn btn-block btn-primary" value="<%=HttpContext.GetLocalResourceObject(LocalResx, "lbl_redeem").ToString() %>" />
                    <%  }
                    }
                    else
                    {%>
                        <input type="button" id="noSessionButton" class="btn btn-block btn-primary" value="<%=HttpContext.GetLocalResourceObject(LocalResx, "lbl_redeem").ToString() %>" />
                <%  }%>
            </div>
            <asp:HiddenField ID="hiddenproductitd" runat="server" />
        </form>
        <script type="text/javascript">
            function VIPOnly() {
                var message = '<%= VipOnly %>';
                if (_.isEmpty(message)) return;
                window.w88Mobile.Growl.shout(message);
            }
            function Error() {
                var message = '<%= Errormsg %>';
                if (_.isEmpty(message)) return;
                window.w88Mobile.Growl.shout(message);
            }
            $('#validButton').on('click', function () {
                window.location.href = '<%= RedirectUri %>';
            });
            $('#limitButton').on('click', function () {
                Error();
            });
            $('#processingButton').on('click', function () {
                Error();
            });
            $('#vipOnlyButton').on('click', function () {
                VIPOnly();
            });
            $('#noSessionButton').on('click', function () {
                window.location.href = '<%= RedirectUri %>';
            });
        </script>
    </div>
</body>
</html>
