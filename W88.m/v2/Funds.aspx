<%@ Page Language="C#" MasterPageFile="~/v2/MasterPages/Payment.master" AutoEventWireup="true" CodeFile="Funds.aspx.cs" Inherits="v2_Funds" %>

<asp:Content ID="PaymentContent" ContentPlaceHolderID="PaymentMainContent" runat="Server">
    <div class="dashboard">
        <div class="dashboard-row">
            <div class="row">
                <div class="col-xs-6">
                    <a href="#"><span class="icon icon-deposit"></span>Deposit</a>
                </div>
                <div class="col-xs-6">
                    <a href="#"><span class="icon icon-transfer"></span>Transfer</a>
                </div>
            </div>
        </div>
        <div class="dashboard-row">
            <div class="row">
                <div class="col-xs-6">
                    <a href="#"><span class="icon icon-withdraw"></span>Withdraw</a>
                </div>
                <div class="col-xs-6">
                    <a href="#"><span class="icon icon-history"></span>History</a>
                </div>
            </div>
        </div>
    </div>
</asp:Content>