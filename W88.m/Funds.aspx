<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Funds.aspx.cs" Inherits="Funds_Main" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <title>Funds</title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/Js/Main.js"></script>
</head>
<body>
    <!--#include virtual="~/_static/splash.shtml" -->
    <div data-role="page" data-theme="b" data-ajax="false">
        <header data-role="header" data-theme="b" data-position="fixed" id="header">
            <a class="btn-clear ui-btn-left ui-btn" href="#divPanel" data-role="none" id="aMenu" data-load-ignore-splash="true">
                <i class="icon-navicon"></i>
            </a>
            <h1 class="title">Funds</h1>
        </header>

        <div class="ui-content" role="main" id="funds">

            <div class="wallet main-wallet">
                <label class="label">Main Wallet</label>
                <h2 class="value"><%=Session["Main"].ToString()%></h2>
                <small class="currency"><%=commonVariables.GetSessionVariable("CurrencyCode")%></small>
                <a
                    href="Funds.aspx"
                    role="button"
                    runat="server"
                    data-ajax="false"
                    class="reload ion-ios-refresh-empty"
                    data-transition="fade"
                ></a>
            </div>

            <div  id="tabs">
                <div data-role="navbar">
                    <ul>
                        <li><a href="/Deposit/Default.aspx" data-ajax="false"><%=commonCulture.ElementValues.getResourceString("deposit", commonVariables.LeftMenuXML)%></a></li>
                        <li><a href="/FundTransfer/Default.aspx" data-ajax="false"><%=commonCulture.ElementValues.getResourceString("transfer", commonVariables.LeftMenuXML)%></a></li>
                        <li><a href="/Withdrawal/Default.aspx" data-ajax="false"><%=commonCulture.ElementValues.getResourceString("withdrawal", commonVariables.LeftMenuXML)%></a></li>
                        <li><a href="/History" data-ajax="false"><%=commonCulture.ElementValues.getResourceString("history", commonVariables.LeftMenuXML)%></a></li>
                    </ul>
                </div>
                <!-- <div id="deposit">
                    <%=commonCulture.ElementValues.getResourceString("deposit", commonVariables.LeftMenuXML)%>
                </div>
                <div id="transfer">
                    <%=commonCulture.ElementValues.getResourceString("transfer", commonVariables.LeftMenuXML)%>
                </div>
                <div id="withdrawal">
                    <%=commonCulture.ElementValues.getResourceString("withdrawal", commonVariables.LeftMenuXML)%>
                </div> -->
            </div>
            <form runat="server">
                <ul class="row row-bordered bg-gradient">
                    <li class="col col-50">
                        <asp:LinkButton ID="aSportsBtn" runat="server" OnClick="aSportsBtn_Click">
                        <div class="wallet">
                            <label class="label">a-Sports</label>
                            <h4 class="value"><%=Session["ASPORTS"].ToString()%></h4>
                            <small class="currency"><%=commonVariables.GetSessionVariable("CurrencyCode")%></small>
                        </div>
                        </asp:LinkButton>
                    </li>
                    <li class="col col-50">
                        <asp:LinkButton ID="eSportsBtn" runat="server" OnClick="eSportsBtn_Click">
                        <div class="wallet">
                            <label class="label">e-Sports</label>
                            <h4 class="value"><%=Session["SBTECH"].ToString()%></h4>
                            <small class="currency"><%=commonVariables.GetSessionVariable("CurrencyCode")%></small>
                        </div>
                        </asp:LinkButton>
                    </li>
                    <li class="col col-50">
                        <asp:LinkButton ID="wSportsBtn" runat="server" OnClick="wSportsBtn_Click">
                        <div class="wallet">
                            <label class="label">w-Sports</label>
                            <h4 class="value"><%=Session["SBO"].ToString()%></h4>
                            <small class="currency"><%=commonVariables.GetSessionVariable("CurrencyCode")%></small>
                        </div>
                        </asp:LinkButton>
                    </li>
                    <li class="col col-50">
                        <asp:LinkButton ID="lotteryBtn" runat="server" OnClick="lotteryBtn_Click">
                        <div class="wallet">
                            <label class="label">Lottery</label>
                            <h4 class="value"><%=Session["LOTTERY"].ToString()%></h4>
                            <small class="currency"><%=commonVariables.GetSessionVariable("CurrencyCode")%></small>
                        </div>
                        </asp:LinkButton>
                    </li>
                    <li class="col col-50">
                        <asp:LinkButton ID="casinoBtn" runat="server" OnClick="casinoBtn_Click">
                        <div class="wallet">
                            <label class="label">*Live Casino</label>
                            <h4 class="value"><%=Session["CASINO"].ToString()%></h4>
                            <small class="currency"><%=commonVariables.GetSessionVariable("CurrencyCode")%></small>
                        </div>
                    </asp:LinkButton>
                    </li>
                    <li class="col col-50">
                        <asp:LinkButton ID="nuovoBtn" runat="server" OnClick="nuovoBtn_Click">
                        <div class="wallet">
                            <label class="label">Nuovo</label>
                            <h4 class="value"><%=Session["NETENT"].ToString()%></h4>
                            <small class="currency"><%=commonVariables.GetSessionVariable("CurrencyCode")%></small>
                        </div>
                        </asp:LinkButton>
                    </li>
                    <li class="col col-50">
                        <asp:LinkButton ID="clubPalazzoBtn" runat="server" OnClick="clubPalazzoBtn_Click">
                        <div class="wallet">
                            <label class="label">Club Palazzo</label>
                            <h4 class="value"><%=Session["CASINO"].ToString()%></h4>
                            <small class="currency"><%=commonVariables.GetSessionVariable("CurrencyCode")%></small>
                        </div>
                        </asp:LinkButton>
                    </li>
                    <li class="col col-50">
                        <asp:LinkButton ID="pokerBtn" runat="server" OnClick="pokerBtn_Click">
                        <div class="wallet">
                            <label class="label">Poker</label>
                            <h4 class="value"><%=Session["POKER"].ToString()%></h4>
                            <small class="currency"><%=commonVariables.GetSessionVariable("CurrencyCode")%></small>
                        </div>
                      </asp:LinkButton>
                    </li>
                </ul>
            </form>
            <br>
            <p class="note text-center">
                <small>*Cub W, Bravado, Apollo, Crescendo, Divino & Massimo</small>
            </p>
        </div>

        <!--#include virtual="~/_static/footer.shtml" -->
        <!--#include virtual="~/_static/navMenu.shtml" -->
    </div>
</body>
</html>

