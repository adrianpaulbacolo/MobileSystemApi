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
        <div class="main-content has-footer" role="main">
            <div id="divLevel" class="wallet-box" runat="server" visible="False">
                <h4 id="usernameLabel" runat="server"></h4>
                <p id="pointsLabel" runat="server"></p>                
                <span id="pointLevelLabel" runat="server"></span>
            </div>    
            <form id="form2" runat="server" data-ajax="false">
                <div class="container">
                    <div class="catalog-detail-box">
                        <div class="catalog-detail-image">
                            <asp:Image ID="imgPic" runat="server" />
                        </div>
                        <div class="catalog-details">
                            <asp:Label ID="lblPointCenter" runat="server" />
                            <asp:Label ID="lblName" runat="server" Visible="false" />                         
                        </div>
                    </div>     
                    <div class="catalog-information">
                        <strong><%=HttpContext.GetLocalResourceObject(LocalResx, "lbl_product_desc").ToString() %></strong>
                        <p><asp:Label ID="lblDescription" runat="server" /></p>
                        <p><asp:Label ID="lblCurrency" runat="server" /></p>
                        <p><asp:Label ID="lblDelivery" runat="server" /></p>
                    </div>                  
                </div>
                <asp:HiddenField ID="hiddenproductitd" runat="server" />
            </form>
        </div>
        <div class="footer">
            <% if (HasSession)
                {
                    if (IsValidRedemption)
                    {  %>
                        <a data-role="button" class="btn btn-block btn-primary" href='<%= RedirectUri %>'><%=HttpContext.GetLocalResourceObject(LocalResx, "lbl_redeem").ToString() %></a>
                <%  }
                    else if (IsRedemptionLimitReached)
                    { %>
                        <a data-role="button" class="btn btn-block btn-primary" href='#' onclick="Error();"><%=HttpContext.GetLocalResourceObject(LocalResx, "lbl_redeem").ToString() %></a>
                <%  }
                    else if (IsProcessingLimitReached)
                    { %>
                        <a data-role="button" class="btn btn-block btn-primary" href='#' onclick="Error();"><%=HttpContext.GetLocalResourceObject(LocalResx, "lbl_redeem").ToString() %></a>
                <%  }
                    else
                    { %>
                        <a data-role="button" class="btn btn-block btn-primary" href='#' onclick="VIPOnly();"><%=HttpContext.GetLocalResourceObject(LocalResx, "lbl_redeem").ToString() %></a>
                <%  }
                }
                else
                {%>
                    <a data-role="button" data-ajax="false" class="btn btn-block btn-primary" href='<%= RedirectUri %>'>
                        <%=HttpContext.GetLocalResourceObject(LocalResx, "lbl_redeem").ToString() %>
                    </a>
            <%  }%>
        </div>
        <script type="text/javascript">
            function VIPOnly() {
                window.w88Mobile.Growl.shout('<%= VipOnly %>');
            }
            function Error() {
                window.w88Mobile.Growl.shout('<%= Errormsg %>');
            }
        </script>
    </div>
</body>
</html>
