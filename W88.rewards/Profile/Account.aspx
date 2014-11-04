<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Account.aspx.cs" Inherits="Account" %>

<!DOCTYPE html>
<html>
<head>
    <title>Account Summary</title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/Js/Main.js"></script>


   
</head>
<body>
    <!--#include virtual="~/_static/splash.shtml" -->
             <style>
        table td div.pointDetailMainHeader {
            background: none repeat scroll 0 0 #404040;
            display: block;
            font-weight: bold !important;
            line-height: 30px;
            position: relative;
            text-align: left;
                padding: 0 5pt;
        }

        table td div.pointsLink {
            background: none repeat scroll 0 0 #333;
            display: block;
         line-height: 30px;
             padding: 0 5pt;
        }

        table td div.points {
            background: none repeat scroll 0 0 #333;
            display: block;
             line-height: 30px;
                 padding: 0 5pt;
        }

    </style>

    <div data-role="page" data-theme="b">

        <!--#include virtual="~/_static/header.shtml" -->
        
        <div class="ui-content" role="main">
            <div class="div-page-header"><span>Account Summary</span></div>

            <div class="page-content">
                <div class="history_menu">
                    <asp:ListView ID="ListviewHistory" runat="server">
                        <LayoutTemplate>
                            <table id="tblHistory" cellpadding="1" cellspacing="0" style="width: 100%">
                                <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <div class="pointDetailMainHeader">
                                        <asp:Label ID="txtStake" runat="server" Text="Total Stake" />
                                    </div>
                                </td>
                                <td>
                                    <% 
                                        if ((int)HttpContext.Current.Session["pointsAwarded"] > 0)
                                        {%>
                                    <a href='javascript:showProduct();' id='pointsProduct' data-toggle='popover' data-html='true' data-placement='bottom' data-content='' runat="server">
                                        <div class="pointsLink" style="text-align: right;"><span><%#DataBinder.Eval(Container.DataItem,"stake")%></span></div>
                                    </a>
                                    <% }
                                        else
                                        {%>
                                    <div class="points" style="text-align: right;"><span><%#DataBinder.Eval(Container.DataItem,"stake")%></span></div>
                                    <% } %> 
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="pointDetailMainHeader">
                                        <asp:Label ID="txtEarn" runat="server" Text="Total Points Earning" />
                                    </div>
                                </td>
                                <td>
                                    <% 
                                        if ((int)HttpContext.Current.Session["pointsAwarded"] > 0)
                                        {%>
                                    <a href='javascript:showProduct();' id='pointsEarn' data-toggle='popover' data-html='true' data-placement='bottom' data-content='' runat="server">
                                        <div class="pointsLink" style="text-align: right;"><span><%#DataBinder.Eval(Container.DataItem,"earning")%></span></div>
                                    </a>
                                    <% }
                                        else
                                        {%>
                                    <div class="points" style="text-align: right;"><span><%#DataBinder.Eval(Container.DataItem,"stake")%></span></div>
                                    <% } %>
                                </td>

                            </tr>
                            <tr>
                                <td>
                                    <div class="pointDetailMainHeader spinWheelIcon">
                                        <asp:Label ID="txtRedeem" runat="server" Text="Total Points Redeemed" />
                                    </div>
                                </td>
                                <td>
                                    <a href='javascript:showRedeem();' id='pointsRedeem' data-toggle='popover' data-html='true' data-placement='bottom' data-content='' runat="server">
                                        <div class="pointsLink" style="text-align: right;"><span><%#DataBinder.Eval(Container.DataItem,"redemption")%></span></div>
                                    </a>
                                </td>

                            </tr>
                            <tr>
                                <td>
                                    <div class="pointDetailMainHeader">
                                        <asp:Label ID="txtExpired" runat="server" Text="Total Points Expired" />
                                    </div>
                                </td>
                                <td>
                                    <% 
                                        if ((int)HttpContext.Current.Session["pointsExpired"] > 0)
                                        {%>

                                    <a href='javascript:showExpired();' id='pointsExpired' data-toggle='popover' data-html='true' data-placement='bottom' data-content='' runat="server">
                                        <div class="pointsLink" style="text-align: right;"><span><%#DataBinder.Eval(Container.DataItem,"expired")%></span></div>
                                    </a>
                                    <% }
                                        else
                                        {%>
                                    <div class="points" style="text-align: right;"><span><%#DataBinder.Eval(Container.DataItem,"expired")%></span></div>
                                    <% } %>                    
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="pointDetailMainHeader">
                                        <asp:Label ID="txtAdjusted" runat="server" Text="Net Points Adjusted" />
                                    </div>
                                </td>
                                <td>
                                    <% 
                                        if ((int)HttpContext.Current.Session["pointsAdjusted"] > 0)
                                        {%>
                                    <a href='javascript:showAdjusted();' id='pointsAdjusted' data-toggle='popover' data-html='true' data-placement='bottom' data-content='' runat="server">
                                        <div class="pointsLink" style="text-align: right;"><span><%#DataBinder.Eval(Container.DataItem,"adjusted")%></span></div>
                                    </a>
                                    <% }
                                        else
                                        {%>
                                    <div class="points" style="text-align: right;"><span><%#DataBinder.Eval(Container.DataItem,"adjusted")%></span></div>
                                    <% } %>               
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="pointDetailMainHeader">
                                        <asp:Label ID="txtCart" runat="server" Text="Total Points in Cart" />
                                    </div>
                                </td>
                                <td>
                                    <% 
                                        if ((int)HttpContext.Current.Session["pointsCart"] > 0)
                                        {%>
                                    <a href='/Cart' id='pointsCart' data-toggle='popover' data-html='true' data-placement='bottom' data-content='' runat="server">
                                        <div class="pointsLink" style="text-align: right;"><span><%#DataBinder.Eval(Container.DataItem,"cart")%></span></div>
                                    </a>
                                    <% }
                                        else
                                        {%>
                                    <div class="points" style="text-align: right;"><span><%#DataBinder.Eval(Container.DataItem,"cart")%></span></div>
                                    <% } %>               
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="pointDetailMainHeader">
                                        <asp:Label ID="txtBalance" runat="server" Text="    Current Points Balance" />
                                    </div>
                                </td>
                                <td>
                                    <div class="points" style="text-align: right;"><span><%#DataBinder.Eval(Container.DataItem,"cart")%></span></div>
                                </td>
                            </tr>


                        </ItemTemplate>
                    </asp:ListView>
                </div>

                <div id="lblNoRecord" runat="server" style="text-align: center; padding-right: 35px;">No record found</div>
            </div>
        </div>
        <!-- /content -->
        <!--#include virtual="~/_static/footer.shtml" -->
        <!--#include virtual="~/_static/navMenu.shtml" -->
    </div>
    <!-- /page -->
</body>
</html>
