<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Site.master" AutoEventWireup="true" CodeFile="Slots.aspx.cs" Inherits="Slots" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <% var deviceId = commonFunctions.getMobileDevice(Request); %>

    <div class="ui-content" role="main" id="sports">

        <ul class="row row-uc row-wrap bg-gradient">
            <% try
               { %>
            <%if (deviceId == 1 || deviceId == 3)
              {%>

            <li class="col col-33">
                <a class="card" href="/_Static/Downloads/w88.aspx" data-ajax="false">
                    <img src="/_Static/Images/sports/bnr-clubW88-iOS.jpg" class="img-responsive">
                    <div class="title"><%=commonCulture.ElementValues.getResourceXPathString("Products/ClubW/Label", commonVariables.ProductsXML)%></div>
                </a>
            </li>
            <%}%>
            <%if (deviceId == 2 || deviceId == 3)
              {%>
            <li class="col col-33">
                <a class="card" href="/_Static/ClubW/casino.aspx" data-ajax="false">
                    <img src="/_Static/Images/sports/bnr-clubW88-Android.jpg" class="img-responsive">
                    <div class="title"><%=commonCulture.ElementValues.getResourceXPathString("Products/ClubW/Label", commonVariables.ProductsXML)%></div>
                </a>
                <%}%>

                <%}
               catch (Exception) { }%>

            <li class="col col-33">
                <a href="/ClubBravado" class="card" data-ajax="false">
                    <img src="/_Static/Images/bnr-clubbravado.jpg" class="img-responsive">
                    <div class="title" id="ClubBravadoTitle"></div>
                </a>
            </li>

            <li class="col col-33">
                <a href="/ClubMassimo" class="card" data-ajax="false">
                    <img src="/_Static/Images/bnr-clubmassimo-slots.jpg" class="img-responsive">
                    <div class="title" id="ClubMassimoTitle"></div>
                </a>
            </li>

            <%
                try
                {
            %>
            <%if (deviceId == 2 || deviceId == 3)
              {%>
            <li class="col col-33">
                <a href="<%=commonClubMassimo.getDownloadUrl%>" class="card" data-ajax="false">
                    <img src="/_Static/Images/bnr-clubmassimo-slots2.jpg" class="img-responsive">
                    <div class="title"><%=commonCulture.ElementValues.getResourceXPathString("Products/ClubMassimoSlots2/Label", commonVariables.ProductsXML).Replace("<br />", "")%></div>
                </a>
            </li>
            <%}%>
            <%}
                catch (Exception) { }%>

            <li class="col col-33">
                <a href="/ClubPalazzo" class="card" data-ajax="false">
                    <img src="/_Static/Images/bnr-clubpalazzo-slots.jpg?" class="img-responsive">
                    <div class="title" id="ClubPalazzoTitle"></div>
                </a>
            </li>
            <%
                try
                {
            %>
            <%if (deviceId == 2 || deviceId == 3)
              {%>
            <li class="col col-33">
                <a href="/_static/palazzo/slots.aspx" class="card" data-ajax="false">
                    <img src="/_Static/Images/bnr-clubpalazzo-slots2.jpg" class="img-responsive">
                    <div class="title"><%=commonCulture.ElementValues.getResourceXPathString("Products/ClubPalazzoSlots2/Label", commonVariables.ProductsXML).Replace("<br />", "")%></div>
                </a>
            </li>
            <%}%>
            <%}
                catch (Exception) { }%>

            <li class="col col-33">
                <a href="/ClubGallardo" class="card" data-ajax="false">
                    <img src="/_Static/Images/bnr-clubgallardo.jpg" class="img-responsive">
                    <div class="title" id="ClubGallardoTitle"></div>
                </a>
            </li>
            <% if (!string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId))
               {
                   if (commonCookie.CookieCurrency.ToLower() != "idr" && commonCookie.CookieCurrency.ToLower() != "vnd")
                   { %>
                <li class="col col-33">
                    <a href="/ClubNuovo" class="card" data-ajax="false">
                        <img src="/_Static/Images/bnr-clubnuovo.jpg" class="img-responsive">
                        <div class="title ClubNuovoTitle" ></div>
                    </a>
                </li>
                <% }
               }
               else
               { %>
            <li class="col col-33">
                <a href="/ClubNuovo" class="card" data-ajax="false">
                    <img src="/_Static/Images/bnr-clubnuovo.jpg" class="img-responsive">
                    <div class="title ClubNuovoTitle"></div>
                </a>
            </li>
            <% } %>

            <li class="col col-33">
                <a href="/ClubApollo" class="card" data-ajax="false">
                    <img src="/_Static/Images/bnr-clubapollo.jpg" class="img-responsive">
                    <div class="title" id="ClubApolloTitle"></div>
                </a>
            </li>
            <li class="col col-33">
                <a href="/ClubDivino" class="card" data-ajax="false">
                    <img src="/_Static/Images/bnr-clubdivino.jpg" class="img-responsive">
                    <div class="title" id="ClubDivinoTitle"></div>
                </a>
            </li>
        </ul>
    </div>

    <script type="text/javascript">
        $(document).ready(function () {

            setTranslations();
            function setTranslations() {
                if (_w88_contents.translate("LABEL_PRODUCTS_CLUB_NUOVO") != "LABEL_PRODUCTS_CLUB_NUOVO") {
                    $("div.ClubNuovoTitle").text(_w88_contents.translate("LABEL_PRODUCTS_CLUB_NUOVO"));
                    $("#ClubBravadoTitle").text(_w88_contents.translate("LABEL_PRODUCTS_BRAVADO"));
                    $("#ClubMassimoTitle").text(_w88_contents.translate("LABEL_PRODUCTS_MASSIMO"));
                    $("#ClubPalazzoTitle").text(_w88_contents.translate("LABEL_PRODUCTS_PALAZZO"));
                    $("#ClubGallardoTitle").text(_w88_contents.translate("LABEL_PRODUCTS_GALLARDO"));
                    $("#ClubApolloTitle").text(_w88_contents.translate("LABEL_PRODUCTS_APOLLO"));
                    $("#ClubDivinoTitle").text(_w88_contents.translate("LABEL_PRODUCTS_DIVINO"));
                } else {
                    window.setInterval(function () {
                        setTranslations();
                    }, 500);
                }
            }
        });
    </script>

</asp:Content>

