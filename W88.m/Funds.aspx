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
            <h1 class="title"><%=commonCulture.ElementValues.getResourceString("funds", commonVariables.LeftMenuXML)%></h1>
        </header>

        <div class="ui-content" role="main" id="funds">

            <div class="wallet main-wallet">
                <label class="label"><%=commonCulture.ElementValues.getResourceString("mainWallet", commonVariables.LeftMenuXML)%></label>
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
                        <asp:LinkButton ID="texasmahjongBtn" runat="server" OnClick="texasmahjongBtn_Click">
                        <div class="wallet">
                            <label class="label"><%=commonCulture.ElementValues.getResourceXPathString("Wallets/pmahjong", commonVariables.ProductsXML)%></label>
                            <h4 class="value">
                                <%if (Session["PMAHJONG"].ToString() != "-")
                                  {%> 
                                   <%=Session["PMAHJONG"].ToString()%>
                                <%}%>
                                <%else{%>
                                      0.00
                                <%}%>

                            </h4>
                            <small class="currency"><%=commonVariables.GetSessionVariable("CurrencyCode")%></small>
                        </div>
                        </asp:LinkButton>
                    </li>
                    <li class="col col-50">
                        <asp:LinkButton ID="aSportsBtn" runat="server" OnClick="aSportsBtn_Click">
                        <div class="wallet">
                            <label class="label"><%=commonCulture.ElementValues.getResourceXPathString("Wallets/asports", commonVariables.ProductsXML)%></label>
                            <h4 class="value">
                                <%if(Session["ASPORTS"].ToString() != "-"){%> 
                                   <%=Session["ASPORTS"].ToString()%>
                                <%}%>
                                <%else{%>
                                      0.00
                                <%}%>

                            </h4>
                            <small class="currency"><%=commonVariables.GetSessionVariable("CurrencyCode")%></small>
                        </div>
                        </asp:LinkButton>
                    </li>
                    <li class="col col-50">
                        <asp:LinkButton ID="eSportsBtn" runat="server" OnClick="eSportsBtn_Click">
                        <div class="wallet">
                            <label class="label"><%=commonCulture.ElementValues.getResourceXPathString("Wallets/esports", commonVariables.ProductsXML)%></label>
                            <h4 class="value">
                                <%if(Session["SBTECH"].ToString() != "-"){%> 
                                   <%=Session["SBTECH"].ToString()%>
                                <%}%>
                                <%else{%>
                                      0.00
                                <%}%>
                            </h4>
                            <small class="currency"><%=commonVariables.GetSessionVariable("CurrencyCode")%></small>
                        </div>
                        </asp:LinkButton>
                    </li>
                    <li class="col col-50">
                        <asp:LinkButton ID="lotteryBtn" runat="server" OnClick="lotteryBtn_Click">
                        <div class="wallet">
                            <label class="label"><%=commonCulture.ElementValues.getResourceXPathString("Wallets/lottery", commonVariables.ProductsXML)%></label>
                            <h4 class="value">
                                <%if(Session["LOTTERY"].ToString() != "-"){%> 
                                   <%=Session["LOTTERY"].ToString()%>
                                <%}%>
                                <%else{%>
                                      0.00
                                <%}%>
                            </h4>
                            <small class="currency"><%=commonVariables.GetSessionVariable("CurrencyCode")%></small>
                        </div>
                        </asp:LinkButton>
                    </li>
                    <li class="col col-50">
                        <asp:LinkButton ID="casinoBtn" runat="server" OnClick="casinoBtn_Click">
                        <div class="wallet">
                            <label class="label"><%=commonCulture.ElementValues.getResourceXPathString("Wallets/liveCasino", commonVariables.ProductsXML)%></label>
                            <h4 class="value">
                                <%if(Session["CASINO"].ToString() != "-"){%> 
                                   <%=Session["CASINO"].ToString()%>
                                <%}%>
                                <%else{%>
                                      0.00
                                <%}%>                           
                            </h4>
                            <small class="currency"><%=commonVariables.GetSessionVariable("CurrencyCode")%></small>
                        </div>
                    </asp:LinkButton>
                    </li>
                    <li class="col col-50">
                        <asp:LinkButton ID="nuovoBtn" runat="server" OnClick="nuovoBtn_Click">
                        <div class="wallet">
                            <label class="label"><%=commonCulture.ElementValues.getResourceXPathString("Wallets/netent", commonVariables.ProductsXML)%></label>
                            <h4 class="value">
                                <%if(Session["NETENT"].ToString() != "-"){%> 
                                   <%=Session["NETENT"].ToString()%>
                                <%}%>
                                <%else{%>
                                      0.00
                                <%}%>                                  
                            </h4>
                            <small class="currency"><%=commonVariables.GetSessionVariable("CurrencyCode")%></small>
                        </div>
                        </asp:LinkButton>
                    </li>
                    <li class="col col-50">
                        <asp:LinkButton ID="clubPalazzoBtn" runat="server" OnClick="clubPalazzoBtn_Click">
                        <div class="wallet">
                            <label class="label"><%=commonCulture.ElementValues.getResourceXPathString("Wallets/playtech", commonVariables.ProductsXML)%></label>
                            <h4 class="value">
                                <%if(Session["PLAYTECH"].ToString() != "-"){%> 
                                   <%=Session["PLAYTECH"].ToString()%>
                                <%}%>
                                <%else{%>
                                      0.00
                                <%}%>  
                                </h4>
                            <small class="currency"><%=commonVariables.GetSessionVariable("CurrencyCode")%></small>
                        </div>
                        </asp:LinkButton>
                    </li>
                    <li class="col col-50">
                        <asp:LinkButton ID="pokerBtn" runat="server" OnClick="pokerBtn_Click">
                        <div class="wallet">
                            <label class="label"><%=commonCulture.ElementValues.getResourceXPathString("Wallets/poker", commonVariables.ProductsXML)%></label>
                            <h4 class="value">
                                <%if(Session["POKER"].ToString() != "-"){%> 
                                   <%=Session["POKER"].ToString()%>
                                <%}%>
                                <%else{%>
                                      0.00
                                <%}%> 
                                </h4>
                            <small class="currency"><%=commonVariables.GetSessionVariable("CurrencyCode")%></small>
                        </div>
                      </asp:LinkButton>
                    </li>
                </ul>
            </form>
            <br>
            <p class="note text-center">
                <small>*<%=commonCulture.ElementValues.getResourceXPathString("Wallets/casino", commonVariables.ProductsXML)%></small>
            </p>
        </div>

        <!--#include virtual="~/_static/navMenu.shtml" -->
    </div>
</body>
</html>

