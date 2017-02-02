<%@ Page Language="C#" MasterPageFile="~/v2/MasterPages/Payment.master" AutoEventWireup="true" CodeFile="Funds.aspx.cs" Inherits="v2_Funds" %>

<asp:Content ID="PaymentContent" ContentPlaceHolderID="PaymentMainContent" runat="Server">
    <div class="dashboard dashboard-funds">
        <div class="dashboard-row">
            <div class="dashboard-col">
                <a href="/Deposit/Default.aspx"><span class="icon icon-deposit"></span> Deposit</a>
            </div>
            <div class="dashboard-col">
                <a href="/FundTransfer"><span class="icon icon-transfer"></span> Transfer</a>
            </div>
        </div>
        <div class="dashboard-row">
            <div class="dashboard-col">
                    <a href="/Withdrawal/"><span class="icon icon-withdraw"></span> Withdraw</a>
            </div>
            <div class="dashboard-col">
                    <a href="/History"><span class="icon icon-history"></span> History</a>
            </div>
        </div>
    </div>
</asp:Content>