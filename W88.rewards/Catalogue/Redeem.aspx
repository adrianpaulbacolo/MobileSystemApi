<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Redeem.aspx.cs" Inherits="Catalogue_Redeem" %>

<!DOCTYPE html>
<html>
<head>
    <title>Redemption</title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="~/_Static/Js/Main.js"></script>
    <link type="text/css" href="/_Static/Css/IndexScroll.css" rel="stylesheet">
    <!--<![endif]-->
</head>
<body>
    <!--#include virtual="~/_static/splash.shtml" -->
    <div id="divMain" data-role="page" data-theme="b" data-ajax="false">
        <!--#include virtual="~/_static/header.shtml" -->
        <div class="ui-content" role="main">
            <div id="divLevel" runat="server" visible="False">
                <span id="lblPoint" runat="server"></span>
            </div>
            <style type="text/css">
                .div-content-wrapper > div {
                    margin-bottom: 6px;
                }

                #bottomdiv {
                    clear: both;
                }

                .imgProduct {
                    height: 105px;
                    margin: 0 0 5px;
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

                .ui-field-contain {
                    border-bottom: 0px none rgba(0, 0, 0, 0.15);
                    font-size: 9pt;
                    color: #808080;
                }

                .imagediv {
                    text-align: center;
                }


                .ui-mini .ui-input-text input, .ui-mini .ui-input-search input, .ui-input-text.ui-mini input, .ui-input-search.ui-mini input, .ui-mini textarea.ui-input-text, textarea.ui-mini {
                    font-size: 9pt;
                    color: white;
                }

                .ui-input-text, .ui-input-search {
                    border-style: solid;
                    border-width: 1px;
                    float: left;
                    margin: 0 1px 2px;
                    width: 96%;
                }

                .div-content-wrapper h4 {
                    color: white;
                    font-family: "Open Sans",sans-serif,helvetica,Tahoma,Arial,Verdana,"Comic Sans MS";
                    font-size: smaller;
                    font-weight: 700;
                    margin-top: 5px;
                    position: relative;
                }

                .validator {
                    color: red;
                    text-align: left;
                }

                .errormessage {
                    color: red;
                }

                .ui-field-contain textarea {
                    background: none repeat scroll 0 0 transparent;
                    border: 0 none;
                    border-radius: inherit;
                    margin: 0;
                    min-height: 2.2em;
                    text-align: left;
                }

                .ui-mini textarea.ui-input-text, textarea.ui-mini {
                    margin: 0px;
                }
            </style>

            <div class="page-content">
                <form id="form1" runat="server">
                    <div class="div-content-wrapper">

                        <h4>REDEEM INFO</h4>
                        <div class="imagediv">
                            <asp:Image ID="imgPic" runat="server" CssClass="imgProduct" />
                        </div>
                        <asp:HiddenField ID="lblproductid" runat="server" />
                        <div class="ui-field-contain ui-hide-label">
                            <div class="ui-grid-a">
                                <div class="ui-block-a">
                                    <asp:Label ID="lbl1" runat="server" Text="Category: " data-mini="true" />
                                </div>
                                <div class="ui-block-b">
                                    <asp:Label ID="lblCategory" runat="server" Text="Category text here" data-mini="true" />
                                </div>
                            </div>
                        </div>
                        <div class="ui-field-contain ui-hide-label">
                            <div class="ui-grid-a">
                                <div class="ui-block-a">
                                    <asp:Label ID="Label2" runat="server" Text="Product: " data-mini="true" />
                                </div>
                                <div class="ui-block-b">
                                    <asp:Label ID="lblName" runat="server" data-mini="true" />
                                </div>
                            </div>
                        </div>
                        <div id="CurrencyDiv" runat="server" class="ui-field-contain ui-hide-label" visible="false">
                            <div class="ui-grid-a">
                                <div class="ui-block-a">
                                    <asp:Label ID="Label3" runat="server" Text="Currency: " data-mini="true" />
                                </div>
                                <div class="ui-block-b">
                                    <asp:Label ID="lblCurrency" runat="server" data-mini="true" />
                                </div>
                            </div>
                        </div>
                        <div class="ui-field-contain ui-hide-label">
                            <div class="ui-grid-a">
                                <div class="ui-block-a">
                                    <asp:Label ID="Label7" runat="server" Text="Points: " data-mini="true" />
                                </div>
                                <div class="ui-block-b">

                                    <asp:Label ID="lblBeforeDiscount" runat="server" Style="text-decoration: line-through;" Visible="true" data-mini="true" Text="39" />
                                    <asp:Label ID="lblPointCenter" runat="server" data-mini="true" Text="39" />

                                </div>
                            </div>
                        </div>
                        <div id="DeliveryDiv" class="ui-field-contain ui-hide-label" runat="server" visible="false">
                            <div class="ui-grid-a">
                                <div class="ui-block-a">
                                    <asp:Label ID="lbl42" runat="server" Text="Delivery Day(s):" data-mini="true" />
                                </div>
                                <div class="ui-block-b">
                                    <asp:Label ID="lblDelivery" runat="server" data-mini="true" />
                                </div>
                            </div>
                        </div>
                        <div id="QuantityDiv" class="ui-field-contain ui-hide-label">
                            <div class="ui-grid-a">
                                <div class="ui-block-a">
                                    <asp:Label ID="Label5" runat="server" Text="Quantity" data-mini="true" />
                                </div>
                                <div class="ui-block-b">
                                    <asp:TextBox ID="tbQuantity" runat="server" MaxLength="2" type="number" data-mini="true" Text="1" />
                                </div>
                            </div>
                        </div>
                        <div id="AccountDiv" runat="server" class="ui-field-contain ui-hide-label" visible="true">
                            <div class="ui-grid-a">
                                <div class="ui-block-a">
                                    <asp:Label ID="Label11" runat="server" Text="Account: " data-mini="true" />
                                </div>
                                <div class="ui-block-b">
                                    <asp:TextBox ID="tbAccount" runat="server" MaxLength="100" type="text" data-mini="true" Text="" />
                                </div>
                            </div>
                        </div>
                        <br />

                        <div id="RecipientDiv" runat="server">
                            <h4>RECIPIENT</h4>
                            <div class="ui-field-contain ui-hide-label">
                                <asp:TextBox ID="TextBox1" runat="server" MaxLength="50" type="Text" data-mini="true" placeholder="Recipient Name" />
                                <asp:Label ID="Label4" CssClass="validator" runat="server" Text="*" data-mini="true" />
                            </div>
                            <div class="ui-field-contain ui-hide-label">
                                <div class="ui-input-text ui-body-inherit ui-corner-all ui-mini ui-shadow-inset">
                                    <textarea cols="40" rows="3" name="textarea" id="tbAddress" style="height: auto;" placeholder="Mailing Address"></textarea>
                                </div>
                                <asp:Label ID="Label6" CssClass="validator" runat="server" Text="*" data-mini="true" />
                            </div>
                            <div class="ui-field-contain ui-hide-label">
                                <asp:TextBox ID="TextBox3" runat="server" MaxLength="10" type="Text" data-mini="true" placeholder="Postal Code" />
                                <asp:Label ID="Label1" CssClass="validator" runat="server" Text="*" data-mini="true" />
                            </div>
                            <div class="ui-field-contain ui-hide-label">
                                <asp:TextBox ID="TextBox4" runat="server" MaxLength="50" type="Text" data-mini="true" placeholder="City" />
                                <asp:Label ID="Label8" CssClass="validator" runat="server" Text="*" data-mini="true" />
                            </div>
                            <div class="ui-field-contain ui-hide-label">
                                <asp:TextBox ID="TextBox5" runat="server" MaxLength="50" type="Text" data-mini="true" placeholder="Country" />
                                <asp:Label ID="Label9" CssClass="validator" runat="server" Text="*" data-mini="true" />
                            </div>
                            <div class="ui-field-contain ui-hide-label">
                                <asp:TextBox ID="TextBox2" runat="server" MaxLength="50" type="tel" data-mini="true" placeholder="Contact Number" />
                                <asp:Label ID="Label10" CssClass="validator" runat="server" Text="*" data-mini="true" />
                            </div>
                        </div>

                        <div class="ui-field-contain ui-hide-label">
                            <asp:Button ID="btnSubmit" runat="server" Text="Redeem Now" CssClass="button-blue" data-corners="false" />
                        </div>


                        <div>


                            <%-- <asp:TextBox runat="server" ID="HiddenQuanFreebet"></asp:TextBox>--%>

                            <%--Normal--%>
                            <%--  <asp:TextBox runat="server" ID="HiddenName"></asp:TextBox>
                            <asp:TextBox runat="server" ID="HiddenAddress"></asp:TextBox>
                            <asp:TextBox runat="server" ID="HiddenPostal"></asp:TextBox>
                            <asp:TextBox runat="server" ID="HiddenCity"></asp:TextBox>
                            <asp:TextBox runat="server" ID="HiddenCountry"></asp:TextBox>
                            <asp:TextBox runat="server" ID="HiddenContact"></asp:TextBox>
                            <asp:TextBox runat="server" ID="HiddenQuanNormal"></asp:TextBox>--%>

                            <%--WishList--%>
                            <%--    <asp:TextBox runat="server" ID="HiddenNameWL"></asp:TextBox>
                            <asp:TextBox runat="server" ID="HiddenAddressWL"></asp:TextBox>
                            <asp:TextBox runat="server" ID="HiddenPostalWL"></asp:TextBox>
                            <asp:TextBox runat="server" ID="HiddenCityWL"></asp:TextBox>
                            <asp:TextBox runat="server" ID="HiddenCountryWL"></asp:TextBox>
                            <asp:TextBox runat="server" ID="HiddenContactWL"></asp:TextBox>
                            <asp:TextBox runat="server" ID="HiddenWishList"></asp:TextBox>
                            <asp:TextBox runat="server" ID="HiddenQuanWish"></asp:TextBox>--%>

                            <%--Online--%>
                            <%--  <asp:TextBox runat="server" ID="HiddenOnline"></asp:TextBox>
                            <asp:TextBox runat="server" ID="HiddenQuanOnline"></asp:TextBox>--%>


                            <%--Error Message--%>
                            <asp:Label ID="lblerror" CssClass="errormessage" runat="server" Text="error msg" />

                            <%-- Hey there! This rewards redemption is only for VIP-Gold and above HOUSE OF HIGHROLLERS, YOU DESERVED IT!--%>
                        </div>
                    </div>

                </form>
                <div id="divContent">
                </div>
            </div>
    </div>
    <!-- /content -->
    <!--#include virtual="~/_static/footer.shtml" -->
    <!--#include virtual="~/_static/navMenu.shtml" -->
    </div>
    <!-- /page -->
</body>
</html>
