<%@ Page Language="C#" MasterPageFile="~/v2/MasterPages/Payment.master" AutoEventWireup="true" CodeFile="Funds.aspx.cs" Inherits="v2_Funds" %>

<asp:Content ID="PaymentContent" ContentPlaceHolderID="PaymentMainContent" runat="Server">
    <div class="dashboard dashboard-funds">
        <div class="dashboard-row">
            <div class="dashboard-col">
                <a href="/Deposit/Default.aspx">
                    <span class="icon icon-deposit"></span> <%=commonCulture.ElementValues.getResourceString("deposit", commonVariables.LeftMenuXML)%>
                </a>
            </div>
            <div class="dashboard-col">
                <a href="/FundTransfer">
                    <span class="icon icon-transfer"></span> <%=commonCulture.ElementValues.getResourceString("transfer", commonVariables.LeftMenuXML)%>
                </a>
            </div>
        </div>
        <div class="dashboard-row">
            <div class="dashboard-col">
                    <a href="/Withdrawal/">
                        <span class="icon icon-withdraw"></span> <%=commonCulture.ElementValues.getResourceString("withdrawal", commonVariables.LeftMenuXML)%>
                    </a>
            </div>
            <div class="dashboard-col">
                    <a href="/History">
                        <span class="icon icon-history"></span> <%=commonCulture.ElementValues.getResourceString("history", commonVariables.HistoryXML)%>
                    </a>
            </div>
        </div>
    </div>
</asp:Content>