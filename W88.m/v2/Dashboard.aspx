<%@ Page Language="C#" MasterPageFile="~/v2/MasterPages/Main.master" AutoEventWireup="true" CodeFile="Dashboard.aspx.cs" Inherits="v2_Dashboard" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContentHolder" runat="Server">
    <div class="home-banner slick-slider">
    </div>

    <div class="dashboard dashboard-home">
        <div class="dashboard-row">
            <div class="dashboard-col">
                <a href="/Sports.aspx?lang=<%=commonVariables.SelectedLanguage.ToLower() %>"><span class="icon icon-soccer"></span>
                    <%=commonCulture.ElementValues.getResourceString("sports", commonVariables.LeftMenuXML)%>
                </a>
            </div>
            <div class="dashboard-col">
                <a href="#"><span class="icon icon-casino"></span>
                    <%=commonCulture.ElementValues.getResourceString("livecasino", commonVariables.LeftMenuXML)%></a>
            </div>
        </div>
        <div class="dashboard-row">
            <div class="dashboard-col">
                <a href="/Slots.aspx"><span class="icon icon-slots"></span>
                    <%=commonCulture.ElementValues.getResourceString("slots", commonVariables.LeftMenuXML)%>
                </a>
            </div>
            <div class="dashboard-col">
                <a href="<%=FishingLink %>" target="_blank"><span class="icon icon-games"></span>
                    <%=commonCulture.ElementValues.getResourceString("LuckyFishing", commonVariables.LeftMenuXML)%>
                </a>
            </div>
        </div>
        <div class="dashboard-row">
            <div class="dashboard-col">
                <a href="/Lottery.aspx?lang=<%=commonVariables.SelectedLanguage%>"><span class="icon icon-keno"></span>
                    <%=commonCulture.ElementValues.getResourceString("lottery", commonVariables.LeftMenuXML)%>
                </a>
            </div>
            <div class="dashboard-col">
                <a href="/v2/Downloads"><span class="icon icon-download"></span>
                    <%=commonCulture.ElementValues.getResourceString("download", commonVariables.LeftMenuXML)%>
                </a>
            </div>
        </div>
    </div>

    <div class="home-footer">
        <img src="/_Static/v2/assets/images/GPI-logo2.png" alt="">
        <p><%=commonCulture.ElementValues.getResourceString("gpiFooter", commonVariables.LeftMenuXML)%></p>
        <p><%=commonCulture.ElementValues.getResourceString("copyright2016", commonVariables.LeftMenuXML)%></p>
    </div>

</asp:Content>

<asp:Content ID="ScriptBottom" ContentPlaceHolderID="InnerScriptPlaceHolder" runat="Server">
    <script>
        $(".dashboard-home a > span.icon-casino").click(function (e) {
            e.preventDefault();
            try {
                Native.openLiveCasino()
            } catch (e) {
                console.log("Native does not exist!");
            }
        });

        $(document).ready(function () {
            $(".home-banner.slick-slider").append('<%=base.BannerDiv %>').slick({
                arrows: false,
                dots: true,
                autoplay: true
            });

        });
    </script>
</asp:Content>
