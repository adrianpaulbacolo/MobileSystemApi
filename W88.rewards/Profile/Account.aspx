<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Account.aspx.cs" Inherits="Account" Async="true"%>

<!DOCTYPE html>
<html>
<head>
    <title>Account Summary</title>
    <!--#include virtual="~/_static/head.inc" -->
</head>
<body>
    <div data-role="page" data-theme="b">
        <!--#include virtual="~/_static/header.shtml" -->
        <div class="main-content static-content">
            <div class="container">
                <h6><%= Title %></h6>
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
                                    <span><%=RewardsHelper.GetTranslation(TranslationKeys.Redemption.TotalStake)%></span>
                                </div>
                            </td>
                            <td>
                                <% 
                                if ((int)DataSet.Tables[0].Rows[0]["earning"] > 0)
                                { %>
                                <a data-ajax="false" href='/Account?type=stake' id='pointsProduct' runat="server">
                                    <div class="pointsLink" style="text-align: right;"><span><%#DataBinder.Eval(Container.DataItem,"stake")%></span></div>
                                </a>
                             <% }
                                    else
                                { %>
                                <div class="points" style="text-align: right;"><span><%#DataBinder.Eval(Container.DataItem,"stake")%></span></div>
                             <% } %> 
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="pointDetailMainHeader">
                                    <span><%=RewardsHelper.GetTranslation(TranslationKeys.Redemption.TotalEarnings)%></span>
                                </div>
                            </td>
                            <td>
                                <% 
                                if ((int)DataSet.Tables[0].Rows[0]["earning"] > 0)
                                { %>
                                <a data-ajax="false" href='/Account?type=stake' id='pointsEarn' runat="server">
                                    <div class="pointsLink" style="text-align: right;"><span><%#DataBinder.Eval(Container.DataItem,"earning")%></span></div>
                                </a>
                             <% }
                                else
                                { %>
                                <div class="points" style="text-align: right;"><span><%#DataBinder.Eval(Container.DataItem,"stake")%></span></div>
                             <% } %>
                            </td>

                        </tr>
                        <tr>
                            <td>
                                <div class="pointDetailMainHeader spinWheelIcon">
                                    <span><%=RewardsHelper.GetTranslation(TranslationKeys.Redemption.TotalRedemption)%></span>
                                </div>
                            </td>
                            <td>
                                <a data-ajax="false" href='/Points?type=redeemed' id='pointsRedeem' runat="server">
                                    <div class="pointsLink" style="text-align: right;"><span><%#DataBinder.Eval(Container.DataItem,"redemption")%></span></div>
                                </a>
                            </td>

                        </tr>
                        <tr>
                            <td>
                                <div class="pointDetailMainHeader">
                                    <span><%=RewardsHelper.GetTranslation(TranslationKeys.Redemption.TotalExpired)%></span>                                       
                                </div>
                            </td>
                            <td>
                                <% 
                                if ((int)DataSet.Tables[0].Rows[0]["expired"] > 0)
                                {%>
                                <a data-ajax="false" href='/Points?type=expired' id='pointsExpired' runat="server">
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
                                    <span><%=RewardsHelper.GetTranslation(TranslationKeys.Redemption.NetAdjusted)%></span>                                     
                                </div>
                            </td>
                            <td>
                                <% 
                                if ((int)DataSet.Tables[0].Rows[0]["adjusted"] > 0)
                                { %>
                                <a data-ajax="false" href='/Points?type=adjusted' id='pointsAdjusted' runat="server">
                                    <div class="pointsLink" style="text-align: right;"><span><%#DataBinder.Eval(Container.DataItem,"adjusted")%></span></div>
                                </a>
                             <% }
                                else
                                { %>
                                <div class="points" style="text-align: right;"><span><%#DataBinder.Eval(Container.DataItem,"adjusted")%></span></div>
                             <% } %>               
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="pointDetailMainHeader">
                                    <span><%=RewardsHelper.GetTranslation(TranslationKeys.Redemption.TotalCart)%></span>                                       
                                </div>
                            </td>
                            <td>                                  
                                <div class="points" style="text-align: right;"><span><%#DataBinder.Eval(Container.DataItem,"cart")%></span></div>                                            
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="pointDetailMainHeader">
                                    <span><%=RewardsHelper.GetTranslation(TranslationKeys.Redemption.CurrentBalance)%></span>                                       
                                </div>
                            </td>
                            <td>
                                <div class="points" style="text-align: right;"><span><%#DataBinder.Eval(Container.DataItem,"balance")%></span></div>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
                <div id="resultpanel" runat="server"></div>
                <h5 id="lblNoRecord" runat="server" Visible="False" class="nodata"><%=RewardsHelper.GetTranslation(TranslationKeys.Redemption.NoRecordFound)%></h5>
            </div>
        </div>
        <!-- /content -->                
    </div>
    <!-- /page -->
</body>
</html>
