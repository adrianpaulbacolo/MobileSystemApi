<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Site.master" AutoEventWireup="true" CodeFile="Sports.aspx.cs" Inherits="Sports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="ui-content" role="main" >
            <ul class="row banner-lists banner-odd-even row-uc">
                <li class="col">
                    <figure class="banner">
                        <img src="_Static/Images/sports/aSports-Rio16.jpg" class="img-responsive img-bg">
                        <figcaption class="banner-caption">
                            <h3 class="title"><%=commonCulture.ElementValues.getResourceXPathString("Products/ASports/Label", commonVariables.ProductsXML)%></h3>
                            <p><%=commonCulture.ElementValues.getResourceXPathString("Products/ASports/Description", commonVariables.ProductsXML)%></p>
                            <a href="<%=(string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId) ? "_Secure/Login.aspx" : commonASports.getSportsbookUrl)%>" data-ajax="false" class="ui-btn btn-primary"><%=commonCulture.ElementValues.getResourceString("playNow", commonVariables.LeftMenuXML)%></a>
                        </figcaption>
                    </figure>
                </li>
                <li class="col">
                    <figure class="banner">
                        <img src="_Static/Images/sports/eSports-Rio16.jpg" class="img-responsive img-bg">
                        <figcaption class="banner-caption">
                            <h3 class="title"><%=commonCulture.ElementValues.getResourceXPathString("Products/ESports/Label", commonVariables.ProductsXML)%></h3>
                            <p><%=commonCulture.ElementValues.getResourceXPathString("Products/ESports/Description", commonVariables.ProductsXML)%></p>
                            <a href="<%=(string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId) ? "_Secure/Login.aspx" : commonESports.getSportsbookUrl)%>" data-ajax="false" class="ui-btn btn-primary"><%=commonCulture.ElementValues.getResourceString("playNow", commonVariables.LeftMenuXML)%></a>
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
                 <li class="col">
                     <figure class="banner">
                        <img src="_Static/Images/sports/ClubW88-iOS-banner.jpg" class="img-responsive img-bg">
                        <figcaption class="banner-caption">
                            <h3 class="title"><%=commonCulture.ElementValues.getResourceXPathString("Products/iOSSports/Label", commonVariables.ProductsXML)%></h3>
                            <p><%=commonCulture.ElementValues.getResourceXPathString("Products/iOSSports/Description", commonVariables.ProductsXML)%></p>
                            <a href="/_Static/Downloads/w88.aspx" data-ajax="false" class="ui-btn btn-primary"><%=commonCulture.ElementValues.getResourceString("playNow", commonVariables.LeftMenuXML)%></a>
                        </figcaption>
                    </figure>
                </li>
            </ul>
        </div>
</asp:Content>

