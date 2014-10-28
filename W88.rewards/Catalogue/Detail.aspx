<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Detail.aspx.cs" Inherits="Catalogue_Detail" %>

<!DOCTYPE html>

<html>

<head runat="server">
    <title>Product Detail</title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/Js/PreLoad.js"></script>
</head>
<body>
    <div data-role="page" data-close-btn="right" data-corners="false">
        <div data-role="header" data-theme="b" class="div-header-logoonly">
            <div class="text-center"></div>
        </div>


        <style type="text/css">
            .div-nav-header {
                background-position: center center;
            }

            .div-content-wrapper {
                background-color: #252525;
                margin: 0.5em;
            }

            #leftdiv {
                float: left;
                height: 130px;
                width: 100%;
                text-align: center;
            }


            #rightdiv {
                float: left;
                width: 100%;
                padding: 3%;
                text-align: left;
            }

            #bottomdiv {
                clear: both;
            }

            .imgProduct {
                height: 105px;
                margin: 10px 0px 0px 0px;
            }


            #lblPointCenter {
                color: #2a8fbd;
                float: left;
                font-size: 10pt;
                font-weight: bold;
                text-align: right;
                width: 95%;
            }

            #DescHeader {
                color: white;
                font-size: 13pt;
                position: relative;
                top: 8px;
            }

            #lblDescription {
                color: #808080;
                font-size: 8pt;
            }

            #lblDelivery {
                color: #808080;
                font-size: 8pt;
            }

            .button-blue {
                background-color: #2a8fbd !important;
                border: medium none;
                color: #fff;
                font-family: "Open Sans",sans-serif,helvetica,Tahoma,Arial,Verdana,"Comic Sans MS";
                font-size: 1em !important;
                font-weight: 700;
                opacity: 1 !important;
                text-indent: 0 !important;
                text-shadow: none !important;
            }
        </style>
        <div class="ui-content" role="main">
            <form id="form2" runat="server" data-ajax="false">
                <div class="div-content-wrapper">
                    <div id="leftdiv">
                        <asp:Image ID="imgPic" runat="server" CssClass="imgProduct" />
                        <asp:Label ID="lblPointCenter" runat="server" />
                        <asp:HiddenField ID="hiddenproductitd" runat="server" />
                    </div>
                    <div id="rightdiv">
                        <asp:Label ID="lblName" runat="server" Visible="false" />
                        <span id="DescHeader">Product Description</span>

                        <asp:Label ID="lblDescription" runat="server" />
                         <asp:Label ID="lblCurrency" runat="server" />

                        <asp:Label ID="lblDelivery" runat="server" />
                    </div>

                    <div id="bottomdiv">
                        <% if (!string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId))
                           { %>
                        <a data-role="button" class="button-blue" style="color: #fff" href='<%= strRedirect %>'>Redeem</a>
                        <% }
                           else
                           {%>
                        <a data-role="button" class="button-blue" style="color: #fff" data-rel="dialog" data-transition="slidedown" href='<%= strRedirect %>'>Redeem</a>
                        <% }%>
                    </div>

                </div>
            </form>
        </div>
        <script type="text/javascript">
           
        </script>


    </div>
</body>

</html>
