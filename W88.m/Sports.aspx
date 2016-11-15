<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Site.master" AutoEventWireup="true" CodeFile="Sports.aspx.cs" Inherits="Sports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <% var deviceId = commonFunctions.getMobileDevice(Request);
       var bannerPosition = false; 
       %>

    <div class="ui-content" role="main">
        <ul class="row banner-lists banner-odd-even row-uc">
            <li class="col">
                <figure class="banner">
                    <img src="_Static/Images/sports/a-sports-banner.jpg" class="img-responsive img-bg">
                    <figcaption class="banner-caption">
                        <h3 class="title"><%=commonCulture.ElementValues.getResourceXPathString("Products/ASports/Label", commonVariables.ProductsXML)%></h3>
                        <p><%=commonCulture.ElementValues.getResourceXPathString("Products/ASports/Description", commonVariables.ProductsXML)%></p>

                        <%if (string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId)) {%>

                        <a href="/_Secure/Login.aspx" data-ajax="false" class="ui-btn btn-primary"><%=commonCulture.ElementValues.getResourceString("playNow", commonVariables.LeftMenuXML)%></a>

                        <%} else { %>

                        <a href="<%=commonASports.getSportsbookUrl%>" target="_blank" data-ajax="false" class="ui-btn btn-primary"><%=commonCulture.ElementValues.getResourceString("playNow", commonVariables.LeftMenuXML)%></a>

                        <%} %>
                    </figcaption>
                </figure>
            </li>
            <li class="col">
                <figure class="banner">
                    <img src="_Static/Images/sports/e-sports-banner.jpg" class="img-responsive img-bg">
                    <figcaption class="banner-caption">
                        <h3 class="title"><%=commonCulture.ElementValues.getResourceXPathString("Products/ESports/Label", commonVariables.ProductsXML)%></h3>
                        <p><%=commonCulture.ElementValues.getResourceXPathString("Products/ESports/Description", commonVariables.ProductsXML)%></p>

                        <%if (string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId)) {%>

                        <a href="/_Secure/Login.aspx" data-ajax="false" class="ui-btn btn-primary"><%=commonCulture.ElementValues.getResourceString("playNow", commonVariables.LeftMenuXML)%></a>

                        <%} else { %>

                        <a href="<%=commonESports.getSportsbookUrl%>" target="_blank" data-ajax="false" class="ui-btn btn-primary"><%=commonCulture.ElementValues.getResourceString("playNow", commonVariables.LeftMenuXML)%></a>

                        <%} %>
                    </figcaption>
                </figure>
            </li>
            <li class="col">
                <figure class="banner">
                    <img src="_Static/Images/sports/v-sports-banner.jpg" class="img-responsive img-bg">
                    <figcaption class="banner-caption">
                        <h3 class="title"><%=commonCulture.ElementValues.getResourceXPathString("Products/VSports/Label", commonVariables.ProductsXML)%></h3>
                        <p><%=commonCulture.ElementValues.getResourceXPathString("Products/VSports/Description", commonVariables.ProductsXML)%></p>
                        <a href="/V-Sports.aspx" data-ajax="false" class="ui-btn btn-primary"><%=commonCulture.ElementValues.getResourceString("playNow", commonVariables.LeftMenuXML)%></a>
                    </figcaption>
                </figure>
            </li>
            <% if (!string.IsNullOrWhiteSpace(commonCookie.CookieCurrency))
               {
                   if (!commonCookie.CookieCurrency.Equals("rmb", StringComparison.OrdinalIgnoreCase))
                   {
                       bannerPosition = true;
                       %>
            
            <li class="col">
                <figure class="banner">
                    <img src="_Static/Images/sports/x-sports-banner.jpg" class="img-responsive img-bg">
                    <figcaption class="banner-caption">
                        <h3 class="title"><%= commonCulture.ElementValues.getResourceXPathString("Products/XSports/Label", commonVariables.ProductsXML) %></h3>
                        <p><%= commonCulture.ElementValues.getResourceXPathString("Products/XSports/Description", commonVariables.ProductsXML) %></p>
                        <a href="<%= commonXSports.SportsBookUrl %>" data-ajax="false" class="ui-btn btn-primary" target="_blank"><%= commonCulture.ElementValues.getResourceString("playNow", commonVariables.LeftMenuXML) %></a>
                    </figcaption>
                </figure>
            </li>
            <% }
               }
               else if (!commonVariables.SelectedLanguage.Equals("zh-cn", StringComparison.OrdinalIgnoreCase))
               {
                   bannerPosition = true;
                   %>

            <li class="col">
                <figure class="banner">
                    <img src="_Static/Images/sports/x-sports-banner.jpg" class="img-responsive img-bg">
                    <figcaption class="banner-caption">
                        <h3 class="title"><%= commonCulture.ElementValues.getResourceXPathString("Products/XSports/Label", commonVariables.ProductsXML) %></h3>
                        <p><%= commonCulture.ElementValues.getResourceXPathString("Products/XSports/Description", commonVariables.ProductsXML) %></p>
                        <a href="<%= commonXSports.SportsBookUrl %>" data-ajax="false" class="ui-btn btn-primary" target="_blank"><%= commonCulture.ElementValues.getResourceString("playNow", commonVariables.LeftMenuXML) %></a>
                    </figcaption>
                </figure>
            </li>
            <% } %>


            <%
                try
                {
            %>
            <%if (deviceId == 1 || deviceId == 3)
              {%>
            <li class="col">
                <figure class="banner">
                    <img src='<%=bannerPosition ? "_Static/Images/sports/ClubW88-iOS-banner-l.jpg" : "_Static/Images/sports/ClubW88-iOS-banner-r.jpg" %>' class="img-responsive img-bg">
                    <figcaption class="banner-caption">
                        <h3 class="title"><%=commonCulture.ElementValues.getResourceXPathString("Products/ClubW/Label", commonVariables.ProductsXML)%></h3>
                        <p><%=commonCulture.ElementValues.getResourceXPathString("Products/iOSSports/Description", commonVariables.ProductsXML)%></p>
                        <a href="/_Static/Downloads/w88.aspx" data-ajax="false" class="ui-btn btn-primary"><%=commonCulture.ElementValues.getResourceString("playNow", commonVariables.LeftMenuXML)%></a>
                    </figcaption>
                </figure>
            </li>
            <%}%>
            <%if (deviceId == 2 || deviceId == 3)
              {%>
            <li class="col">
                <figure class="banner">
                    <img src='<%=bannerPosition ? "_Static/Images/sports/ClubW88-Android-banner-l.jpg" : "_Static/Images/sports/ClubW88-Android-banner-r.jpg" %>' class="img-responsive img-bg">
                    <figcaption class="banner-caption">
                        <h3 class="title"><%=commonCulture.ElementValues.getResourceXPathString("Products/ClubW/Label", commonVariables.ProductsXML)%></h3>
                        <p><%=commonCulture.ElementValues.getResourceXPathString("Products/iOSSports/Description", commonVariables.ProductsXML)%></p>
                        <a href="/_Static/ClubW/casino.aspx" data-ajax="false" class="ui-btn btn-primary"><%=commonCulture.ElementValues.getResourceString("playNow", commonVariables.LeftMenuXML)%></a>
                    </figcaption>
                </figure>
            </li>
            <%}%>

            <%}
                catch (Exception) { }%>
        </ul>
    </div>
</asp:Content>

