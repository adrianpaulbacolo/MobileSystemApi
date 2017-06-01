<%@ Page Language="C#" MasterPageFile="~/v2/MasterPages/Main.master" AutoEventWireup="true" CodeFile="Register.aspx.cs" Inherits="v2_Account_Register" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContentHolder" runat="Server">
    <div class="form-container">
        <div class="container">
            <form class="form" id="form1" runat="server">
                <div class="form-group">
                    <div class="form-group-icon">
                        <div class="form-icon">
                            <span class="icon-profile"></span>
                        </div>
                        <div class="form-input">
                            <asp:Label ID="lblUsername" runat="server" AssociatedControlID="txtUsername" data-i18n="LABEL_USERNAME" />
                            <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" MaxLength="16" required data-require="" />
                        </div>
                    </div>
                </div>

                <div class="form-group">
                    <div class="form-group-icon">
                        <div class="form-icon">
                            <span class="icon-password"></span>
                        </div>
                        <div class="form-input">
                            <asp:Label ID="lblPassword" runat="server" AssociatedControlID="txtPassword" data-i18n="LABEL_PASSWORD" />
                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" required data-require="" />
                        </div>
                    </div>
                </div>

                <div class="form-group">
                    <div class="form-group-icon">
                        <div class="form-icon">
                            <span class="icon-password"></span>
                        </div>
                        <div class="form-input">
                            <asp:Label ID="lblConfirmPassword" runat="server" AssociatedControlID="txtConfirmPassword" data-i18n="LABEL_PASSWORD_CONFIRM" />
                            <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" CssClass="form-control" required data-confirmvalue="txtPassword" />
                        </div>
                    </div>
                </div>

                <div class="form-group">
                    <div class="form-group-icon">
                        <div class="form-icon">
                            <span class="icon-mail"></span>
                        </div>
                        <div class="form-input">
                            <asp:Label ID="lblEmail" runat="server" AssociatedControlID="txtEmail" data-i18n="LABEL_EMAIL" />
                            <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" CssClass="form-control" required data-require="" />
                        </div>
                    </div>
                </div>

                <div class="form-group">
                    <div class="form-group-icon">
                        <div class="form-icon">
                            <span class="icon-phone"></span>
                        </div>
                        <div class="form-input">
                            <asp:Label ID="lblContact" runat="server" AssociatedControlID="drpCountryCode" data-i18n="LABEL_MOBILE_NUMBER" />
                            <div class="row thin-gutter">
                                <div class="col-xs-6 col-sm-6">
                                    <asp:DropDownList ID="drpCountryCode" runat="server" CssClass="form-control" required data-selectequals="-1" />
                                </div>
                                <div class="col-xs-6 col-sm-6">
                                    <asp:TextBox ID="txtPhoneNumber" runat="server" CssClass="form-control" required data-require="" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="form-group">
                    <div class="form-group-icon">
                        <div class="form-icon">
                            <span class="icon-currency"></span>
                        </div>
                        <div class="form-input">
                            <asp:Label ID="lblCurrency" runat="server" AssociatedControlID="drpCurrency" data-i18n="LABEL_CURRENCY" />
                            <asp:DropDownList ID="drpCurrency" runat="server" CssClass="form-control" required data-selectequals="-1" />
                        </div>
                    </div>
                </div>

                <div class="form-group">
                    <div class="form-group-icon">
                        <div class="form-icon">
                            <span class="icon-profile"></span>
                        </div>
                        <div class="form-input">
                            <asp:Label ID="lblFullName" runat="server" AssociatedControlID="txtFullName" data-i18n="LABEL_FULLNAME" />
                            <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" required data-require="" />
                        </div>
                    </div>
                    <span class="form-help-text" data-i18n="LABEL_REGISTER_NOTE"></span>
                </div>


                <div class="form-group">
                    <div class="form-group-icon">
                        <div class="form-icon">
                            <span class="icon-event"></span>
                        </div>
                        <div class="form-input">
                            <asp:Label ID="lblDateOfBirth" runat="server" AssociatedControlID="txtDateOfBirth" data-i18n="LABEL_DOB" />
                            <asp:TextBox ID="txtDateOfBirth" runat="server" CssClass="form-control" data-date-box="dob" />
                        </div>
                    </div>
                </div>

                <div class="form-group lineid">
                    <div class="form-group-icon">
                        <div class="form-icon">
                            <span class="icon-info"></span>
                        </div>
                        <div class="form-input">
                            <asp:Label ID="lblLineId" runat="server" AssociatedControlID="txtLineId" Text="Line ID" />
                            <asp:TextBox ID="txtLineId" runat="server" CssClass="form-control" />
                        </div>
                    </div>
                </div>

                <div class="form-group">
                    <div class="form-group-icon">
                        <div class="form-icon">
                            <span class="icon-referral-bonus"></span>
                        </div>
                        <div class="form-input">
                            <asp:Label ID="lblAffiliateID" runat="server" AssociatedControlID="txtAffiliateID" data-i18n="LABEL_AFFILIATE" />
                            <asp:TextBox ID="txtAffiliateID" runat="server" TextMode="Number" CssClass="form-control" />
                        </div>
                    </div>
                </div>

                <div class="form-group text-center ">
                    <p class="small text-alt" data-i18n="LABEL_REGISTER_DISCLAIMER"></p>
                    <p class="small"><a href="#" class="url_terms" target="_blank" data-i18n="LABEL_REGISTER_TERMS"></a></p>
                </div>
                <button type="submit" id="btnSubmit" class="btn btn-block btn-primary" data-i18n="BUTTON_SUBMIT"></button>
            </form>
        </div>
    </div>
</asp:Content>

<asp:Content ID="ScriptHolder" ContentPlaceHolderID="InnerScriptPlaceHolder" runat="server">
    <script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/modules/bootstrapvalidator.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
    <script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/modules/accounts/register.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>

    <script>
        $(document).ready(function () {
            _w88_register.init();

            $('#form1').validator().on('submit', function (e) {
                if (!e.isDefaultPrevented()) {
                    e.preventDefault();

                    var data = {
                        Username: $('[id$="txtUsername"]').val(),
                        Password: $('[id$="txtConfirmPassword"]').val(),
                        Firstname: $('[id$="txtFullName"]').val(),
                        Email: $('[id$="txtEmail"]').val(),
                        DateOfBirth: formatDateTime($('[id$="txtDateOfBirth"]').datebox('getTheDate')),
                        CountryCode: $('[id$="drpCountryCode"]').val(),
                        Phone: $('[id$="txtPhoneNumber"]').val(),
                        CurrencyCode: $('[id$="drpCurrency"]').val(),
                        AffiliateId: $('[id$="txtAffiliateID"]').val(),
                        LineId: $('[id$="txtLineId"]').val(),
                        ReferralId: getQueryStringValue('Referid')
                    };

                    _w88_register.createAccount(data);
                }
            });
        });
    </script>
</asp:Content>
