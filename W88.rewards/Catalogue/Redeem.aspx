<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Redeem.aspx.cs" Inherits="Catalogue_Redeem" Async="true"%>
<%@ Import Namespace="W88.BusinessLogic.Rewards.Helpers" %>
<%@ Import Namespace="W88.BusinessLogic.Rewards.Models" %>

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
                    <a id="pointsLabel" runat="server" data-ajax="false" href="/Account"></a>                
                    <span id="pointLevelLabel" runat="server"></span>
                </div>  
                <div class="container">
                    <div class="catalog-detail-box">
                        <a href="#" class="catalog-detail-image">
                            <asp:Image ID="imgPic" runat="server" />
                        </a>
                    </div>
                    <div class="catalog-information">
                        <h4><%=RewardsHelper.GetTranslation(TranslationKeys.Redemption.RedeemInfo)%></h4>
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
                            <h4><%=RewardsHelper.GetTranslation(TranslationKeys.Redemption.Recipient)%>: </h4>
                            <div class="ui-field-contain ui-hide-label">
                                <asp:Label ID="nameLabel" CssClass="validator" runat="server" Text="*" data-mini="true" />
                                <asp:TextBox ID="tbRName" runat="server" MaxLength="50" type="Text" data-mini="true" placeholder="Recipient Name" />                              
                            </div>
                            <div class="ui-field-contain ui-hide-label">
                                <asp:Label ID="addressLabel" CssClass="validator" runat="server" Text="*" data-mini="true" />
                                <div class="ui-input-text ui-body-inherit ui-corner-all ui-mini ui-shadow-inset">
                                    <textarea cols="40" rows="3" runat="server" id="tbAddress" style="height: auto;" placeholder="Mailing Address"></textarea>
                                </div>
                            </div>
                            <div class="ui-field-contain ui-hide-label">
                                <asp:Label ID="postalLabel" CssClass="validator" runat="server" Text="*" data-mini="true" />
                                <asp:TextBox ID="tbPostal" runat="server" MaxLength="10" type="Text" data-mini="true" placeholder="Postal Code" />
                            </div>
                            <div class="ui-field-contain ui-hide-label">
                                <asp:Label ID="cityLabel" CssClass="validator" runat="server" Text="*" data-mini="true" />
                                <asp:TextBox ID="tbCity" runat="server" MaxLength="50" type="Text" data-mini="true" placeholder="City" />
                            </div>
                            <div class="ui-field-contain ui-hide-label">
                                <asp:Label ID="countryLabel" CssClass="validator" runat="server" Text="*" data-mini="true" />
                                <asp:TextBox ID="tbCountry" runat="server" MaxLength="50" type="Text" data-mini="true" placeholder="Country" />
                            </div>
                            <div class="ui-field-contain ui-hide-label">
                                <asp:Label ID="contactLabel" CssClass="validator" runat="server" Text="*" data-mini="true" />
                                <asp:TextBox ID="tbContact" runat="server" MaxLength="50" type="tel" data-mini="true" placeholder="Contact Number" />
                            </div>
                            <div class="ui-field-contain ui-hide-label">
                                <asp:Label ID="remarksLabel" CssClass="validator" runat="server" Text="*" data-mini="true" Visible="false" />
                                <textarea cols="40" rows="3" runat="server" id="txtBoxRemarks" style="height: auto;" placeholder="" Visible="false"></textarea>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <asp:HiddenField ID="ProductDetailsField" Value="" runat="server"/>
            <asp:HiddenField ID="ProductIdField" runat="server"/>
            <div class="footer">
                <asp:Button ID="redeemButton" runat="server" Text="" CssClass="btn btn-block btn-primary" OnClick="RedeemButtonOnClick" />
            </div>
        </form>
    </div>
    <script type="text/javascript">
        $('#form1').submit(function (e) {
            $('#btnSubmit').attr("disabled", true);

            if (!isValid('<%=ProductType%>')) {
                $('#btnSubmit').attr("disabled", false);
                return false;
            }
            return true;
        });

        function showMessage(status, message) {
            if (_.isEmpty(status) || _.isEmpty(message)) {
                return;
            }
            switch (status) {
                case '3':
                    $('div.footer').removeClass('footer');
                    break;
                case '-1':
                    $('#redeemButton').attr('disabled', false);
                    break;
            }

            window.w88Mobile.Growl.shout(message);

            var closeButtons = $('#PopUpModal a');
            if (!closeButtons || closeButtons.length === 0) {
                return;
            }

            if (status === '1') {
                closeButtons.each(function() {
                    $(this).attr('data-rel', null);
                    $(this).on('click', function() {
                        window.location.href = '/Catalogue?categoryId=0&sortBy=2';
                    });
                });
            } else {
                closeButtons.each(function() {
                    $(this).attr('data-rel', 'back');
                    $(this).off('click');
                });
            }
        }

        function isValid(type) {
            const NORMAL = '2';
            const WISHLIST = '3';
            const ONLINE = '4';
            var isInvalid = false,
                messages = [];

            if ($('#tbQuantity').val() && $('#tbQuantity').val().trim().length == 0) {
                messages.push('<%=RewardsHelper.GetTranslation(TranslationKeys.Redemption.InvalidQuantity)%>');
                isInvalid = true;
            }

            if (type == ONLINE) {
                if ($('#tbAccount').val() && $('#tbAccount').val().trim().length == 0) {
                    messages.push('<%=RewardsHelper.GetTranslation(TranslationKeys.Redemption.EnterAccount)%>');
                    isInvalid = true;
                }
            } else if (type == NORMAL || type == WISHLIST) {
                if ($('#tbRName').val() && $('#tbRName').val().trim().length == 0) {
                    messages.push('<%=RewardsHelper.GetTranslation(TranslationKeys.Redemption.EnterName)%>');
                    isInvalid = true;
                }
                if ($('#tbAddress').val() && $('#tbAddress').val().trim().length == 0) {
                    messages.push('<%=RewardsHelper.GetTranslation(TranslationKeys.Redemption.EnterAddress)%>');
                    isInvalid = true;
                }
                if ($('#tbPostal').val() && $('#tbPostal').val().trim().length == 0) {
                    messages.push('<%=RewardsHelper.GetTranslation(TranslationKeys.Redemption.EnterPostal)%>');
                    isInvalid = true;
                }
                if ($('#tbCity').val() && $('#tbCity').val().trim().length == 0) {
                    messages.push('<%=RewardsHelper.GetTranslation(TranslationKeys.Redemption.EnterCity)%>');
                    isInvalid = true;
                }
                if ($('#tbCountry').val() && $('#tbCountry').val().trim().length == 0) {
                    messages.push('<%=RewardsHelper.GetTranslation(TranslationKeys.Redemption.EnterCountry)%>');
                    isInvalid = true;
                }
                if ($('#tbContact').val() && $('#tbContact').val().trim().length == 0) {
                    messages.push('<%=RewardsHelper.GetTranslation(TranslationKeys.Redemption.EnterContactNumber)%>');
                    isInvalid = true;
                }
                if (type == WISHLIST) {
                    if ($('#txtBoxRemarks').val() && $('#txtBoxRemarks').val().trim().length == 0) {
                        messages.push('<%=RewardsHelper.GetTranslation(TranslationKeys.Redemption.EnterRemarks)%>');
                        isInvalid = true;
                    }
                }
            }

            if (isInvalid) {
                w88Mobile.Growl.shout(messages.join('; '));
                return false;
            }
            return true;
        }
    </script>
    <!-- /page -->
</body>
</html>
