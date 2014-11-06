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

    <div data-role="page" data-theme="b" data-ajax="false">

        <!--#include virtual="~/_static/header.shtml" -->

        <div class="ui-content" role="main">
            <div class="div-page-header"><span><%= title %></span></div>

            <div class="page-content">
                <style>
                    table td div.pointDetailMainHeader {
                        background: none repeat scroll 0 0 #404040;
                        display: block;
                        font-weight: bold !important;
                        line-height: 40px;
                        position: relative;
                        text-align: left;
                        padding: 0 5pt;
                        font-size: 10pt;
                    }

                    table td div.pointDetailMainHeaderHor {
                        background: none repeat scroll 0 0 #404040;
                        display: block;
                        font-size: 10pt;
                        font-weight: bold !important;
                      min-height: 35px;
                        padding: 4pt 5pt;
                        position: relative;
                        text-align: left;
                    }

                    table td div.pointsLink {
                        background: none repeat scroll 0 0 #333;
                        display: block;
                        line-height: 40px;
                        padding: 0 5pt;
                        font-size: 9pt;
                    }

                    table td div.points {
                        background: none repeat scroll 0 0 #333;
                        display: block;
                        line-height: 40px;
                        padding: 0 5pt;
                        font-size: 9pt;
                    }
                </style>
                    <asp:ListView ID="ListviewHistory" runat="server" data-ajax="false">
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
                                    <a href='/Account?type=stake' id='pointsProduct' runat="server">
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
                                    <a href='/Account?type=stake' id='pointsEarn' runat="server">
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
                                    <a href='/Account?type=redeemed' id='pointsRedeem' runat="server">
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

                                    <a href='/Account?type=expired' id='pointsExpired' runat="server">
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
                                    <a href='/Account?type=adjusted' id='pointsAdjusted' runat="server">
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
                                    <a href='/Account?type=cart' id='pointsCart' runat="server">
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

                    <div id="resultpanel" runat="server">
                        
                    </div>
                <div id="lblNoRecord" runat="server" Visible="False" style="text-align: center; padding-right: 35px;">No record found</div>
            </div>
        </div>
        <!-- /content -->
        <!--#include virtual="~/_static/footer.shtml" -->
        <!--#include virtual="~/_static/navMenu.shtml" -->
        
        
          <script type="text/javascript">

              </script>

    </div>
    <!-- /page -->
</body>
</html>
