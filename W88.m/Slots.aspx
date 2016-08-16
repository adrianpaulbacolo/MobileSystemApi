<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Site.master" AutoEventWireup="true" CodeFile="Slots.aspx.cs" Inherits="Slots" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="ui-content" role="main" id="sports">

        <ul class="row row-uc row-wrap bg-gradient">
            <li class="col col-33">
                <a href="/ClubBravado" class="card" data-ajax="false">
                    <img src="/_Static/Images/bnr-clubbravado.jpg" class="img-responsive">
                    <div class="title"><%=commonCulture.ElementValues.getResourceXPathString("Products/ClubBravado/Label", commonVariables.ProductsXML)%></div>
                </a>
            </li>
            <li class="col col-33">
                <a href="/ClubPalazzo" class="card" data-ajax="false">
                    <img src="/_Static/Images/bnr-clubpalazzo-slots.jpg?" class="img-responsive">
                    <div class="title"><%=commonCulture.ElementValues.getResourceXPathString("Products/ClubPalazzoSlots/Label", commonVariables.ProductsXML).Replace("<br />", "")%></div>
                </a>
            </li>
            <li class="col col-33">
                <a href="/_static/palazzo/slots.aspx" class="card" data-ajax="false">
                    <img src="/_Static/Images/bnr-clubpalazzo-slots2.jpg" class="img-responsive">
                    <div class="title"><%=commonCulture.ElementValues.getResourceXPathString("Products/ClubPalazzoSlots2/Label", commonVariables.ProductsXML).Replace("<br />", "")%></div>
                </a>
            </li>
            <li class="col col-33">
                <a href="/ClubMassimo" class="card" data-ajax="false">
                    <img src="/_Static/Images/bnr-clubmassimo-slots.jpg" class="img-responsive">
                    <div class="title"><%=commonCulture.ElementValues.getResourceXPathString("Products/ClubMassimoSlots/Label", commonVariables.ProductsXML).Replace("<br />", "")%></div>
                </a>
            </li>
            <li class="col col-33">
                <a href="<%=commonClubMassimo.getDownloadUrl%>" class="card" data-ajax="false">
                    <img src="/_Static/Images/bnr-clubmassimo-slots2.jpg" class="img-responsive">
                    <div class="title"><%=commonCulture.ElementValues.getResourceXPathString("Products/ClubMassimoSlots2/Label", commonVariables.ProductsXML).Replace("<br />", "")%></div>
                </a>
            </li>
            <li class="col col-33">
                <a href="/ClubGallardo" class="card" data-ajax="false">
                    <img src="/_Static/Images/bnr-clubgallardo.jpg" class="img-responsive">
                    <div class="title"><%=commonCulture.ElementValues.getResourceXPathString("Products/ClubGallardo/Label", commonVariables.ProductsXML)%></div>
                </a>
            </li>

            <asp:Literal ID="ltlApollo" runat="server"></asp:Literal>

            <li class="col col-33">
                <a href="/ClubDivino" class="card" data-ajax="false">
                    <img src="/_Static/Images/bnr-clubdivino.jpg" class="img-responsive">
                    <div class="title"><%=commonCulture.ElementValues.getResourceXPathString("Products/ClubDivino/Label", commonVariables.ProductsXML)%></div>
                </a>
            </li>
            <li class="col col-33">
                <a class="card" href="/_Static/Downloads/w88.aspx" data-ajax="false">
                    <img src="/_Static/Images/sports/bnr-clubW88-iOS.jpg" class="img-responsive">
                    <div class="title"><%=commonCulture.ElementValues.getResourceXPathString("Products/iOSSports/Label", commonVariables.ProductsXML)%></div>
                </a>
            </li>
        </ul>
    </div>
</asp:Content>

