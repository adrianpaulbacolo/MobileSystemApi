<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Redeem.aspx.cs" Inherits="Catalogue_Redeem" %>

<!DOCTYPE html>
<html>
<head>
    <title>Redemption</title>
    <!--#include virtual="~/_static/head.inc" -->
</head>
<body>
    <div data-role="page" data-theme="b">
        <!--#include virtual="~/_static/header.shtml" -->
        <form id="form1" runat="server">
            <div class="main-content has-footer" role="main">
                <div id="divLevel" class="wallet-box" runat="server" visible="False">
                    <h4 id="usernameLabel" runat="server"></h4>
                    <p id="pointsLabel" runat="server"></p>                
                    <span id="pointLevelLabel" runat="server"></span>
                </div>  
                <div class="container">
                    <div class="catalog-detail-box">
                        <a href="#" class="catalog-detail-image">
                            <asp:Image ID="imgPic" runat="server" />
                        </a>
                    </div>
                    <div class="catalog-information">
                        <h4><%=HttpContext.GetLocalResourceObject(LocalResx, "lbl_redeem_info").ToString()%></h4>
                        <asp:HiddenField ID="lblproductid" runat="server" />
                        <div class="ui-field-contain ui-hide-label">
                            <div>
                                <div>
                                    <asp:Label ID="lbcat" runat="server" Text="" data-mini="true" />                              
                                </div>
                                <div>
                                    <asp:Label ID="lblCategory" runat="server" Text="" data-mini="true" />
                                </div>
                            </div>
                        </div>
                        <div class="ui-field-contain ui-hide-label">
                            <div>
                                <div>
                                    <asp:Label ID="lbproduct" runat="server" Text="" data-mini="true" />
                                </div>
                                <div>
                                    <asp:Label ID="lblName" runat="server" data-mini="true" />
                                </div>
                            </div>
                        </div>
                        <div id="CurrencyDiv" runat="server" class="ui-field-contain ui-hide-label" visible="false">
                            <div>
                                <div>
                                    <asp:Label ID="lbcurr" runat="server" Text="" data-mini="true" />
                                </div>
                                <div>
                                    <asp:Label ID="lblCurrency" runat="server" data-mini="true" />
                                </div>
                            </div>
                        </div>
                        <div class="ui-field-contain ui-hide-label">
                            <div>
                                <div>
                                    <asp:Label ID="lbpoint" runat="server" Text="" data-mini="true" />
                                </div>
                                <div>
                                    <asp:Label ID="lblBeforeDiscount" runat="server" Style="text-decoration: line-through;" Visible="true" data-mini="true" Text="39" />
                                    <asp:Label ID="lblPointCenter" runat="server" data-mini="true" Text="39" />
                                </div>
                            </div>
                        </div>
                        <div id="DeliveryDiv" class="ui-field-contain ui-hide-label" runat="server" visible="false">
                            <div>
                                <div>
                                    <asp:Label ID="lbperiod" runat="server" Text="" data-mini="true" />
                                </div>
                                <div>
                                    <asp:Label ID="lblDelivery" runat="server" data-mini="true" />
                                </div>
                            </div>
                        </div>
                        <div id="QuantityDiv" class="ui-field-contain ui-hide-label">
                            <div>
                                <div>
                                    <asp:Label ID="lbqty" runat="server" Text="" data-mini="true" />
                                </div>
                                <div>
                                    <asp:TextBox ID="tbQuantity" runat="server" MaxLength="2" type="number" data-mini="true" Text="1" />
                                </div>
                            </div>
                        </div>
                        <div id="AccountDiv" runat="server" class="ui-field-contain ui-hide-label" visible="true">
                            <div>
                                <div>
                                    <asp:Label ID="lbaccount" runat="server" Text="" data-mini="true" />
                                </div>
                                <div>
                                    <asp:TextBox ID="tbAccount" runat="server" MaxLength="100" type="text" data-mini="true" Text="" />
                                </div>
                            </div>
                        </div>
                        <br />
                        <div id="RecipientDiv" runat="server">
                            <h4><%=HttpContext.GetLocalResourceObject(LocalResx, "lbl_receipient").ToString() %>: </h4>
                            <div class="ui-field-contain ui-hide-label">
                                <asp:TextBox ID="tbRName" runat="server" MaxLength="50" type="Text" data-mini="true" placeholder="Recipient Name" />
                                <asp:Label ID="Label4" CssClass="validator" runat="server" Text="*" data-mini="true" />
                            </div>
                            <div class="ui-field-contain ui-hide-label">
                                <div class="ui-input-text ui-body-inherit ui-corner-all ui-mini ui-shadow-inset">
                                    <textarea cols="40" rows="3" runat="server" id="tbAddress" style="height: auto;" placeholder="Mailing Address"></textarea>
                                </div>
                                <asp:Label ID="Label6" CssClass="validator" runat="server" Text="*" data-mini="true" />
                            </div>
                            <div class="ui-field-contain ui-hide-label">
                                <asp:TextBox ID="tbPostal" runat="server" MaxLength="10" type="Text" data-mini="true" placeholder="Postal Code" />
                                <asp:Label ID="Label1" CssClass="validator" runat="server" Text="*" data-mini="true" />
                            </div>
                            <div class="ui-field-contain ui-hide-label">
                                <asp:TextBox ID="tbCity" runat="server" MaxLength="50" type="Text" data-mini="true" placeholder="City" />
                                <asp:Label ID="Label8" CssClass="validator" runat="server" Text="*" data-mini="true" />
                            </div>
                            <div class="ui-field-contain ui-hide-label">
                                <asp:TextBox ID="tbCountry" runat="server" MaxLength="50" type="Text" data-mini="true" placeholder="Country" />
                                <asp:Label ID="Label9" CssClass="validator" runat="server" Text="*" data-mini="true" />
                            </div>
                            <div class="ui-field-contain ui-hide-label">
                                <asp:TextBox ID="tbContact" runat="server" MaxLength="50" type="tel" data-mini="true" placeholder="Contact Number" />
                                <asp:Label ID="Label10" CssClass="validator" runat="server" Text="*" data-mini="true" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="footer">
                <asp:Button ID="redeemButton" runat="server" Text="" CssClass="btn btn-block btn-primary" OnClick="RedeemButtonOnClick" />
            </div>
        </form>
    </div>
    <script type="text/javascript">
        $('#form1').submit(function (e) {
            $('#btnSubmit').attr("disabled", true);

            switch ('<%=ProductType%>') {
                case '1': //freebet
                    if (!validateNormal()) {
                        $('#btnSubmit').attr("disabled", false);
                        return false;
                    } 
                    return true;                   
                case '2': //normal
                    break;
                case '3': //wishlist same as normal
                    if (!validateNormal()) {
                        $('#btnSubmit').attr("disabled", false);
                        return false;
                    } 
                    return true;
                case '4': //online
                    if (!validateOnlineAccount()) {
                        $('#btnSubmit').attr("disabled", false);
                        return false;
                    } 
                    return true;                
                default:
                    break;
            }
        });

        function showMessage(alertCode, message) {
            if (_.isEmpty(alertCode)) {
                return;
            }

            switch (alertCode) {
                case 'VIP':
                    window.w88Mobile.Growl.shout(message);
                    $('#redeemButton').attr("disabled", true);
                    window.location.replace('/Catalogue');
                    break;
                case 'SUCCESS':
                    window.w88Mobile.Growl.shout(message);
                    window.location.replace('/Catalogue');
                    break;
                case 'FAIL':
                    window.w88Mobile.Growl.shout(message);
                    $('#redeemButton').attr("disabled", false);
                    break;
                case 'CURR':
                    window.location.replace('/Catalogue?categoryId=0&sortBy=2');
                    break;
                default:
                    break;
            }
        }

        function validateFreebet() {
            if ($('#tbQuantity').val() && $('#tbQuantity').val().trim().length == 0) {
                window.w88Mobile.Growl.shout('<%=HttpContext.GetLocalResourceObject(LocalResx, "lbl_invalid_number").ToString()%>');
                return false;
            }
            return true;       
        }

        function validateOnlineAccount() {
            if ($('#tbQuantity').val() && $('#tbQuantity').val().trim().length == 0) {
                window.w88Mobile.Growl.shout('<%=HttpContext.GetLocalResourceObject(LocalResx, "lbl_invalid_number").ToString()%>');
                return false;
            }
            if ($('#tbAccount').val() && $('#tbAccount').val().trim().length == 0) {
                window.w88Mobile.Growl.shout('<%=HttpContext.GetLocalResourceObject(LocalResx, "lbl_enter_account").ToString()%>');
                return false;
            } 
            return true;        
        }

        function validateNormal() {
            if ($('#tbQuantity').val() && $('#tbQuantity').val().trim().length == 0) {
                window.w88Mobile.Growl.shout('<%=HttpContext.GetLocalResourceObject(LocalResx, "lbl_invalid_number").ToString()%>');
                return false;
            }
            if ($('#tbRName').val() && $('#tbRName').val().trim().length == 0) {
                window.w88Mobile.Growl.shout('<%=HttpContext.GetLocalResourceObject(LocalResx, "lbl_enter_name").ToString()%>');
                return false;
            }
            if ($('#tbAddress').val() && $('#tbAddress').val().trim().length == 0) {
                window.w88Mobile.Growl.shout('<%=HttpContext.GetLocalResourceObject(LocalResx, "lbl_enter_address").ToString()%>');
                return false;
            }
            if ($('#tbPostal').val() && $('#tbPostal').val().trim().length == 0) {
                window.w88Mobile.Growl.shout('<%=HttpContext.GetLocalResourceObject(LocalResx, "lbl_enter_postal").ToString()%>');
                return false;
            }
            if ($('#tbCity').val() && $('#tbCity').val().trim().length == 0) {
                window.w88Mobile.Growl.shout('<%=HttpContext.GetLocalResourceObject(LocalResx, "lbl_enter_city").ToString()%>');
                return false;
            }
            if ($('#tbCountry').val() && $('#tbCountry').val().trim().length == 0) {
                window.w88Mobile.Growl.shout('<%=HttpContext.GetLocalResourceObject(LocalResx, "lbl_enter_country").ToString()%>');
                return false;
            }
            if ($('#tbContact').val() && $('#tbContact').val().trim().length == 0) {
                window.w88Mobile.Growl.shout('<%=HttpContext.GetLocalResourceObject(LocalResx, "lbl_enter_contact").ToString()%>');
                return false;
            } 
            return true;            
        }
    </script>
    <!-- /page -->
</body>
</html>
