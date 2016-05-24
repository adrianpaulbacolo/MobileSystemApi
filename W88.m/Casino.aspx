<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Site.master" AutoEventWireup="true" CodeFile="Casino.aspx.cs" Inherits="Casino" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    <div class="ui-content" role="main">
        <ul class="row banner-lists banner-odd-even">
            <li class="col">
                <figure class="banner">
                    <img src="/_Static/Images/casino/clubwpremierbanner.jpg" class="img-responsive img-bg">
                    <figcaption class="banner-caption">
                        <h3 class="title"><%=commonCulture.ElementValues.getResourceString("clubwpremier", commonVariables.LeftMenuXML)%></h3>
                        <p><%=commonCulture.ElementValues.getResourceString("clubwpremierMessage", commonVariables.LeftMenuXML)%></p>
                        <a href="/_Static/ClubW/casino.aspx" data-ajax="false" class="ui-btn btn-primary"><%=commonCulture.ElementValues.getResourceString("playNow", commonVariables.LeftMenuXML)%></a>
                    </figcaption>
                </figure>
            </li>
            <li class="col">
                <figure class="banner">
                    <img src="/_Static/Images/casino/clubwbanner.jpg" class="img-responsive img-bg">
                    <figcaption class="banner-caption">
                        <h3 class="title"><%=commonCulture.ElementValues.getResourceString("liveCasino", commonVariables.LeftMenuXML)%></h3>
                        <p><%=commonCulture.ElementValues.getResourceString("liveCasinoMessage", commonVariables.LeftMenuXML)%></p>
                        <a href="/_Static/ClubW/casino.aspx" data-ajax="false" class="ui-btn btn-primary"><%=commonCulture.ElementValues.getResourceString("playNow", commonVariables.LeftMenuXML)%></a>
                    </figcaption>
                </figure>
            </li>
            <li class="col">
                <figure class="banner">
                    <img src="/_Static/Images/casino/clubpalazzobanner.jpg" class="img-responsive img-bg">
                    <figcaption class="banner-caption">
                        <h3 class="title"><%=commonCulture.ElementValues.getResourceString("clubPalazo", commonVariables.LeftMenuXML)%></h3>
                        <p><%=commonCulture.ElementValues.getResourceString("clubPalazoMessage", commonVariables.LeftMenuXML)%></p>
                        <a href="/_Static/Palazzo/casino.aspx" data-ajax="false" class="ui-btn btn-primary"><%=commonCulture.ElementValues.getResourceString("playNow", commonVariables.LeftMenuXML)%></a>
                    </figcaption>
                </figure>
            </li>
            <li class="col">
                <figure class="banner">
                    <img src="/_Static/Images/casino/clubmassimobanner.jpg" class="img-responsive img-bg">
                    <figcaption class="banner-caption">
                        <h3 class="title"><%=commonCulture.ElementValues.getResourceString("clubMassimo", commonVariables.LeftMenuXML)%></h3>
                        <p><%=commonCulture.ElementValues.getResourceString("clubMassimoMessage", commonVariables.LeftMenuXML)%></p>
                        <a href="https://livegames.cdn.gameassists.co.uk/AIR/Poria/Installer/V20021/w88/Download.html" data-ajax="false" class="ui-btn btn-primary" target="_blank"><%=commonCulture.ElementValues.getResourceString("playNow", commonVariables.LeftMenuXML)%></a>
                        <p>&nbsp;</p>
                    </figcaption>
                </figure>
            </li>
        </ul>
    </div>
</asp:Content>

