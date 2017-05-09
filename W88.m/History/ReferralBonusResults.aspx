<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReferralBonusResults.aspx.cs" Inherits="History_ReferralBonusResults" %>

<%@ Register TagPrefix="uc" TagName="Wallet" Src="~/UserControls/MainWalletBalance.ascx" %>
<%@ Register TagPrefix="uc" TagName="AppFooterMenu" Src="~/UserControls/AppFooterMenu.ascx" %>

<!DOCTYPE html>

<html>
<head>
    <title><%=string.Format("{0} {1}", commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML), commonCulture.ElementValues.getResourceString("referralbonus", commonVariables.HistoryXML))%></title>
    <!--#include virtual="~/_static/head.inc" -->
</head>
<body>
    <div data-role="page" data-theme="b">
        <header data-role="header" data-theme="b" data-position="fixed" id="header">
            <a class="btn-clear ui-btn-left ui-btn" href="#divPanel" data-role="none" id="aMenu" data-load-ignore-splash="true">
                <% if (commonFunctions.isExternalPlatform())
                   { %>
                <i class="icon icon-back"></i>
                <% }
                   else
                   { %>
                <i class="icon icon-navicon"></i>
                <% } %>
            </a>
            <h1 class="title"><%=string.Format("{0} - {1}", commonCulture.ElementValues.getResourceString("history", commonVariables.HistoryXML), commonCulture.ElementValues.getResourceString("referralbonus", commonVariables.HistoryXML))%></h1>
        </header>

        <div class="ui-content" role="main">

            <div class="wallet main-wallet">
                <uc:Wallet id="uMainWallet" runat="server" />
            </div>

            <form class="form" id="form1" runat="server" data-ajax="false">
                <br />
                <ul class="list fixed-tablet-size">
                    <li class="row">
                        <div class="col">
                            <span class="label"><%=commonCulture.ElementValues.getResourceString("totalInvitees", commonVariables.HistoryXML)%></span>
                        </div>
                        <div class="col">
                            <asp:Label ID="lblInvitees" runat="server"></asp:Label>
                        </div>
                    </li>
                    <li class="row">
                        <div class="col">
                            <span class="label"><%=commonCulture.ElementValues.getResourceString("totalRegistered", commonVariables.HistoryXML)%></span>
                        </div>
                        <div class="col">
                            <asp:Label ID="lblRegistered" runat="server"></asp:Label>
                        </div>
                    </li>
                    <li class="row">
                        <div class="col">
                            <span class="label"><%=commonCulture.ElementValues.getResourceString("totalSuccessReferral", commonVariables.HistoryXML)%></span>
                        </div>
                        <div class="col">
                            <asp:Label ID="lblSuccessfulReferrals" runat="server"></asp:Label>
                        </div>
                    </li>
                    <li class="row">
                        <div class="col">
                            <span class="label"><%=commonCulture.ElementValues.getResourceString("totalReferralBonus", commonVariables.HistoryXML).Replace("[cur]", commonVariables.GetSessionVariable("CurrencyCode"))%></span>
                        </div>
                        <div class="col">
                            <asp:Label ID="lblTotalReferralBonus" runat="server"></asp:Label>
                        </div>
                    </li>
                </ul>
                <br />
                <div style="overflow: scroll;">
                    <asp:GridView ID="GridView1" runat="server" CssClass="gridHistory table table-striped"
                        AutoGenerateColumns="False"
                        AllowSorting="false"
                        GridLines="None"
                        CellSpacing="1"
                        AllowPaging="True" PageSize="10"
                        OnPageIndexChanging="GridView1_PageIndexChanging"
                        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-HorizontalAlign="Center" EmptyDataRowStyle-ForeColor="#dab867"
                        OnRowDataBound="GridView1_RowDataBound"
                        DataKeyNames="status">
                        <FooterStyle ForeColor="#dab867"></FooterStyle>
                        <PagerStyle ForeColor="#dab867" HorizontalAlign="Right"></PagerStyle>
                        <HeaderStyle ForeColor="#dab867" Font-Bold="True"></HeaderStyle>
                        <Columns>
                            <asp:TemplateField HeaderText="#">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="requestDate" SortExpression="requestDate">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField
                                DataField="invId" SortExpression="invId">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField
                                DataField="requestAmount" SortExpression="requestAmount">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField
                                DataField="status" SortExpression="status">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                        </Columns>
                        <SelectedRowStyle BackColor="#9471DE"></SelectedRowStyle>
                    </asp:GridView>
                </div>
                
                <uc:AppFooterMenu runat="server" ID="AppFooterMenu" />
                
                <div class="item row" id="NonAppMenu">
                    <div class="col">
                        <a href="/History" role="button" class="ui-btn btn-bordered" data-ajax="false"><%=commonCulture.ElementValues.getResourceString("cancel", commonVariables.LeftMenuXML)%></a>
                    </div>
                </div>

            </form>
            
        </div>
        <!-- /content -->
        <!--#include virtual="~/_static/navMenu.shtml" -->

    </div>
</body>
</html>


