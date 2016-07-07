<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DepositWithdrawalResults.aspx.cs" Inherits="History_DepositWithdrawalResults" %>

<%@ Register TagPrefix="uc" TagName="Wallet" Src="~/UserControls/MainWalletBalance.ascx" %>

<!DOCTYPE html>

<html>
<head>
    <title><%=string.Format("{0} {1}", commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML), commonCulture.ElementValues.getResourceString("depositwithdrawal", commonVariables.HistoryXML))%></title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/Js/Main.js"></script>
</head>
<body>
    <div data-role="page" data-theme="b">
        <header data-role="header" data-theme="b" data-position="fixed" id="header">
            <a class="btn-clear ui-btn-left ui-btn" href="#divPanel" data-role="none" id="aMenu" data-load-ignore-splash="true">
                <i class="icon-navicon"></i>
            </a>
            <h1 class="title"><%=string.Format("{0} - {1}", commonCulture.ElementValues.getResourceString("history", commonVariables.HistoryXML), commonCulture.ElementValues.getResourceString("depositwithdrawal", commonVariables.HistoryXML))%></h1>
        </header>

        <div class="ui-content" role="main">

            <div class="wallet main-wallet">
                <uc:Wallet id="uMainWallet" runat="server" />
            </div>

            <form class="form" id="form1" runat="server" data-ajax="false">
                <div style="overflow: scroll;">
                    <asp:GridView ID="GridView1" runat="server"
                        CssClass="gridHistory table table-striped"
                        AutoGenerateColumns="False"
                        AllowSorting="false"
                        GridLines="None"
                        CellSpacing="1"
                        AllowPaging="True" PageSize="10"
                        ShowHeaderWhenEmpty="true"
                        EmptyDataRowStyle-HorizontalAlign="Center"
                        EmptyDataRowStyle-ForeColor="#dab867"
                        OnPageIndexChanging="GridView1_PageIndexChanging"
                        OnRowDataBound="GridView1_RowDataBound"
                        DataKeyNames="paymenttype,methodid,status">
                        <FooterStyle ForeColor="#dab867"></FooterStyle>
                        <PagerStyle ForeColor="#dab867" HorizontalAlign="Right"></PagerStyle>
                        <HeaderStyle ForeColor="#dab867"></HeaderStyle>
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
                            <asp:BoundField DataField="requestDate" SortExpression="requestDate">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField
                                DataField="invId" SortExpression="invId">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField
                                DataField="methodCode" SortExpression="methodCode">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField
                                DataField="paymentType" SortExpression="paymentType">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField
                                DataField="requestAmount" SortExpression="requestAmount">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField
                                DataField="transAmount" SortExpression="transAmount">
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
            </form>
            <div class="item row">
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


