<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PromotionClaimResults.aspx.cs" Inherits="History_PromotionClaimResults" %>
<%@ Register TagPrefix="uc" TagName="Wallet" Src="~/UserControls/MainWalletBalance.ascx" %>

<!DOCTYPE html>

<html>
<head>
    <title><%=commonCulture.ElementValues.getResourceString("promotionclaim", commonVariables.LeftMenuXML)%></title>
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
            <h1 class="title"><%=commonCulture.ElementValues.getResourceString("promotionclaim", commonVariables.LeftMenuXML)%></h1>
        </header>

        <div class="ui-content" role="main">

            <div class="wallet main-wallet">
                <uc:Wallet id="uMainWallet" runat="server" />
            </div>

            <div class="row row-no-padding">
                <form id="form1" runat="server" class="table-responsive">
                    <asp:GridView ID="GridView1" runat="server" CssClass="gridHistory table table-striped"
                        AutoGenerateColumns="False"
                        AllowSorting="false"
                        GridLines="None"
                        CellSpacing="1"
                        AllowPaging="True" PageSize="10" OnPageIndexChanging="GridView1_PageIndexChanging"
                        ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found" EmptyDataRowStyle-HorizontalAlign="Center" EmptyDataRowStyle-ForeColor="#dab867">
                        <FooterStyle ForeColor="#dab867"></FooterStyle>
                        <PagerStyle ForeColor="#dab867" HorizontalAlign="Right"></PagerStyle>
                        <HeaderStyle ForeColor="#dab867" Font-Bold="True"></HeaderStyle>
                        <Columns>
                            <%--<asp:BoundField HeaderText="NO"
                                  DataField="paymentType" SortExpression="paymentType">
                                </asp:BoundField>--%>
                            <asp:TemplateField HeaderText="#">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="DATE/TIME (GMT+8)" DataField="submissionDate" SortExpression="submissionDate">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="SUBJECT CODE"
                                DataField="subjectCode" SortExpression="subjectCode">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>

                        </Columns>
                        <SelectedRowStyle BackColor="#9471DE"></SelectedRowStyle>
                    </asp:GridView>
                </form>
            </div>
            <div class="row row-no-padding">
                <div class="col">
                    <a href="/Funds.aspx" role="button" class="ui-btn btn-bordered"><%=commonCulture.ElementValues.getResourceString("cancel", commonVariables.LeftMenuXML)%></a>
                </div>
            </div>
        </div>
        <!-- /content -->
        <!--#include virtual="~/_static/navMenu.shtml" -->

    </div>
</body>
</html>


