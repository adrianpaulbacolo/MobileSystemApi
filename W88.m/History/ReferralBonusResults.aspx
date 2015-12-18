<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReferralBonusResults.aspx.cs" Inherits="History_ReferralBonusResults" %>

<!DOCTYPE html>

<html>
<head>
    <title><%=commonCulture.ElementValues.getResourceString("referralbonus", commonVariables.LeftMenuXML)%></title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/Js/Main.js"></script>
</head>
<body>
    <!--#include virtual="~/_static/splash.shtml" -->
    <div data-role="page" data-theme="b">
        <header data-role="header" data-theme="b" data-position="fixed" id="header">
            <a class="btn-clear ui-btn-left ui-btn" href="#divPanel" data-role="none" id="aMenu" data-load-ignore-splash="true">
                <i class="icon-navicon"></i>
            </a>
            <h1 class="title"><%=commonCulture.ElementValues.getResourceString("referralbonus", commonVariables.LeftMenuXML)%></h1>
        </header>

        <div class="ui-content" role="main">

            <div class="wallet main-wallet">
                <label class="label">Main Wallet</label>
                <h2 class="value"><%=Session["Main"].ToString()%></h2>
                <small class="currency"><%=commonVariables.GetSessionVariable("CurrencyCode")%></small>
            </div>

            <div class="padding">
                <p><span class="label">Total Invitees: </span><asp:Label ID="lblInvitees" runat="server"></asp:Label></p>
                <p><span class="label">Total Registered: </span><asp:Label ID="lblRegistered" runat="server"></asp:Label></p>
                <p><span class="label">Total Successful Referrals: </span><asp:Label ID="lblSuccessfulReferrals" runat="server"></asp:Label></p>
                <p><span class="label">Total Referral Bonus (RMB)</span><asp:Label ID="lblTotalReferralBonus" runat="server"></asp:Label></p>
            </div>

            <div class="row row-no-padding">
                <form id="form1" runat="server" class="table-responsive">
                    <asp:GridView ID="GridView1" Runat="server" CssClass="gridHistory table table-striped"
                          AutoGenerateColumns="False"
                            AllowSorting="false"
                            GridLines="None"
                            CellSpacing="1"
                            AllowPaging="True" PageSize="10" OnPageIndexChanging="GridView1_PageIndexChanging">
                            <FooterStyle ForeColor="#dab867"></FooterStyle>
                            <PagerStyle ForeColor="#dab867" HorizontalAlign="Right" ></PagerStyle>
                            <HeaderStyle ForeColor="#dab867" Font-Bold="True"></HeaderStyle>
                            <Columns>
                                <asp:TemplateField HeaderText="#">
                                    <ItemStyle HorizontalAlign="Center" />
                                  <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                  </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="DATE/TIME" DataField="requestDate" SortExpression="requestDate">
                                     <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="REF. NO."
                                  DataField="invId" SortExpression="invId">
                                     <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="AMOUNT"
                                  DataField="requestAmount" SortExpression="requestAmount">
                                     <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="STATUS"
                                  DataField="status" SortExpression="status">
                                     <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                            </Columns>
                            <SelectedRowStyle BackColor="#9471DE"></SelectedRowStyle>
                        </asp:GridView>
                    </form>
            </div>

        </div>
        <!-- /content -->
        <!--#include virtual="~/_static/footer.shtml" -->
        <!--#include virtual="~/_static/navMenu.shtml" -->

    </div>
</body>
</html>


