<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DepositWithdrawalResults.aspx.cs" Inherits="History_DepositWithdrawalResults" %>

<!DOCTYPE html>

<html>
<head>
    <title><%=commonCulture.ElementValues.getResourceString("depositwithdrawal", commonVariables.LeftMenuXML)%></title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/Js/Main.js"></script>
    
    <style>

     .gridHistory {
        width: 100%; 
        word-wrap:break-word;
        table-layout: fixed;
    }
    </style>

</head>
<body>
    <!--#include virtual="~/_static/splash.shtml" -->
    <div data-role="page" data-theme="b">
        <header data-role="header" data-theme="b" data-position="fixed" id="header">
            <a class="btn-clear ui-btn-left ui-btn" href="#divPanel" data-role="none" id="aMenu" data-load-ignore-splash="true">
                <i class="icon-navicon"></i>
            </a>
            <h1 class="title"><%=commonCulture.ElementValues.getResourceString("depositwithdrawal", commonVariables.LeftMenuXML)%></h1>
        </header>

        <div class="ui-content" role="main">
            
            <div class="wallet main-wallet">
                <label class="label">Main Wallet</label>
                <h2 class="value"><%=Session["Main"].ToString()%></h2>
                <small class="currency"><%=commonVariables.GetSessionVariable("CurrencyCode")%></small>
            </div>
            
            <div class="row row-no-padding">

                <form id="form1" runat="server" >
                    <asp:GridView ID="GridView1" Runat="server" CssClass="gridHistory"
                          AutoGenerateColumns="False"
                            AllowSorting="false"
                            GridLines="None" 
                            CellSpacing="1"  
                            AllowPaging="True" PageSize="10" OnPageIndexChanging="GridView1_PageIndexChanging">
                            <FooterStyle ForeColor="#dab867"></FooterStyle>
                            <PagerStyle ForeColor="#dab867" HorizontalAlign="Right" ></PagerStyle>
                            <HeaderStyle ForeColor="#dab867" Font-Bold="True"></HeaderStyle>
                            <Columns>
                                <%--<asp:BoundField HeaderText="NO" 
                                  DataField="paymentType" SortExpression="paymentType">
                                </asp:BoundField>--%>
                                <asp:TemplateField ItemStyle-Width="10%" HeaderText="#">
                                    <ItemStyle HorizontalAlign="Center" />
                                  <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                  </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="DATE/TIME" DataField="requestDate" SortExpression="requestDate" ItemStyle-Width="15%">
                                     <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="REF. NO." 
                                  DataField="invId" SortExpression="invId" ItemStyle-Width="13%" >
                                     <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="PAYMENT METHOD" 
                                  DataField="methodCode" SortExpression="methodCode" ItemStyle-Width="13%">
                                     <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="TYPE" 
                                  DataField="paymentType" SortExpression="paymentType" ItemStyle-Width="10%">
                                     <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="SUBMITTED AMT" 
                                  DataField="requestAmount" SortExpression="requestAmount" ItemStyle-Width="13%">
                                     <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="RECEIVED AMT" 
                                  DataField="transAmount" SortExpression="transAmount" ItemStyle-Width="13%">
                                     <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="STATUS" 
                                  DataField="status" SortExpression="status" ItemStyle-Width="13%">
                                     <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                            </Columns>
                            <SelectedRowStyle ForeColor="White" Font-Bold="True" 
                                 BackColor="#9471DE"></SelectedRowStyle>
                            <RowStyle ForeColor="White" ></RowStyle>
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


