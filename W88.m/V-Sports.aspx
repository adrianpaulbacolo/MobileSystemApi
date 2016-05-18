<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Site.master" AutoEventWireup="true" CodeFile="V-Sports.aspx.cs" Inherits="VSports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="ui-content" role="main" id="sports">

        <ul class="row row-wrap bg-gradient">
            <li class="col col-33">
                <a href="/_Secure/Login.aspx" class="card" data-rel="sports" rel="vsportsBasketball" target="_blank">
                    <img src="/_Static/Images/sports/bnr-vSports-Basketball.jpg" class="img-responsive">
                    <div class="title"><%=commonCulture.ElementValues.getResourceXPathString("Products/VSports-basketball/Label", commonVariables.ProductsXML)%></div>
                </a>
            </li>
            <%--<li class="col col-33">
                    <a href="/_Secure/Login.aspx" class="card" data-rel="sports" rel="vsportsDogRacing">
                        <img src="_Static/Images/bnr-vSPORTS-dogracing.jpg" alt="a-Sports" class="img-responsive">
                       <div class="title"><%=commonCulture.ElementValues.getResourceXPathString("Products/VSports-dogracing/Label", commonVariables.ProductsXML)%></div>
                    </a>
                </li>--%>
            <li class="col col-33">
                <a href="/_Secure/Login.aspx" class="card" data-rel="sports" rel="vsportsFootBall" target="_blank">
                    <img src="_Static/Images/sports/bnr-vSports-soccer.jpg" class="img-responsive">
                    <div class="title"><%=commonCulture.ElementValues.getResourceXPathString("Products/VSports-football/Label", commonVariables.ProductsXML)%></div>
                </a>
            </li>
            <%--<li class="col col-33">
                    <a href="/_Secure/Login.aspx" class="card" data-rel="sports" rel="vsportsHorseRacing">
                        <img src="_Static/Images/bnr-vSPORTS-horseracing.jpg" alt="a-Sports" class="img-responsive">
                        <div class="title"><%=commonCulture.ElementValues.getResourceXPathString("Products/VSports-horseracing/Label", commonVariables.ProductsXML)%></div>
                    </a>
                </li>--%>
            <%--<li class="col col-33">
                    <a href="/_Secure/Login.aspx" class="card" data-rel="sports" rel="vsportsTennis">
                        <img src="_Static/Images/bnr-vSPORTS-tennis.jpg" alt="a-Sports" class="img-responsive">
                        <div class="title"><%=commonCulture.ElementValues.getResourceXPathString("Products/VSports-tennis/Label", commonVariables.ProductsXML)%></div>
                    </a>
                </li>--%>
        </ul>
        <div class="note text-center" hidden>
            <%=commonCulture.ElementValues.getResourceXPathString("Products/VSports/Notice", commonVariables.ProductsXML)%>
        </div>
    </div>

    <script type="text/javascript">
        $(function () {

            (function (a) { (jQuery.browser = jQuery.browser || {}).android = /android|(android|bb\d+|meego).+mobile/i.test(a) })(navigator.userAgent || navigator.vendor || window.opera);

            if ($.browser.android) {
                $('.note').show();
            } else {
                $('.note').hide();
            }
        });
        </script>
</asp:Content>

