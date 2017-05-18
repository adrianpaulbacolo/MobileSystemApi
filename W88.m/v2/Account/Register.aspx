<%@ Page Language="C#" MasterPageFile="~/v2/MasterPages/Main.master" AutoEventWireup="true" CodeFile="Register.aspx.cs" Inherits="v2_Account_Register" %>

<asp:Content ID="PaymentContent" ContentPlaceHolderID="MainContentHolder" runat="Server">
    <div class="form-container">
        <div class="container">
            <form class="form" id="form1" runat="server">
                <div class="form-group">
                    <span class="icon icon-profile"></span>
                    <asp:Label ID="lblUsername" runat="server" AssociatedControlID="txtUsername" data-i18n="LABEL_USERNAME" />
                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" MaxLength="16" required data-require="" />
                </div>
                <div class="form-group">
                    <span class="icon icon-password"></span>
                    <asp:Label ID="lblPassword" runat="server" AssociatedControlID="txtPassword" data-i18n="LABEL_PASSWORD" />
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" required data-require="" />
                </div>
                <div class="form-group">
                    <span class="icon icon-password"></span>
                    <asp:Label ID="lblConfirmPassword" runat="server" AssociatedControlID="txtConfirmPassword" data-i18n="LABEL_PASSWORD_CONFIRM" />
                    <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" CssClass="form-control" required data-confirmvalue="txtPassword" />
                </div>
                <div class="form-group">
                    <span class="icon icon-mail"></span>
                    <asp:Label ID="lblEmail" runat="server" AssociatedControlID="txtEmail" data-i18n="LABEL_EMAIL" />
                    <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" CssClass="form-control" required data-require="" />
                </div>
                <div class="form-group">
                    <i class="icon icon-phone"></i>
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
                <div class="form-group">
                    <span class="icon icon-currency"></span>
                    <asp:Label ID="lblCurrency" runat="server" AssociatedControlID="drpCurrency" data-i18n="LABEL_CURRENCY" />
                    <asp:DropDownList ID="drpCurrency" runat="server" CssClass="form-control" required data-selectequals="-1" />
                </div>
                <div class="form-group">
                    <span class="icon icon-profile"></span>
                    <asp:Label ID="lblFullName" runat="server" AssociatedControlID="txtFullName" data-i18n="LABEL_FULLNAME" />
                    <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" required data-require="" />
                </div>
                <div class="form-group">
                    <p class="text-center small"><span data-i18n="LABEL_REGISTER_NOTE"></span></p>
                </div>
                <div class="form-group">
                    <span class="icon icon-event"></span>
                    <asp:Label ID="lblDateOfBirth" runat="server" AssociatedControlID="txtDateOfBirth" data-i18n="LABEL_DOB" />
                    <asp:TextBox ID="txtDateOfBirth" runat="server" CssClass="form-control" data-date-box="dob" />
                </div>
                <div class="form-group lineid">
                    <span class="icon icon-info"></span>
                    <asp:Label ID="lblLineId" runat="server" AssociatedControlID="txtLineId" Text="Line ID" />
                    <asp:TextBox ID="txtLineId" runat="server" CssClass="form-control" />
                </div>
                <div class="form-group">
                    <span class="icon icon-referral-bonus"></span>
                    <asp:Label ID="lblAffiliateID" runat="server" AssociatedControlID="txtAffiliateID" data-i18n="LABEL_AFFILIATE" />
                    <asp:TextBox ID="txtAffiliateID" runat="server" TextMode="Number" CssClass="form-control" />
                </div>
                <div class="form-group">
                    <p class="text-center small" data-i18n="LABEL_REGISTER_DISCLAIMER"></p>
                    <p class="text-center small"><a href="https://info.w88live.com/termofuse_en.shtml" target="_blank" data-i18n="LABEL_TERMS"></a></p>
                </div>
                <button type="submit" id="btnSubmit" class="btn btn-block btn-primary" data-i18n="BUTTON_SUBMIT"></button>
            </form>
        </div>
    </div>
</asp:Content>

<asp:Content ID="ScriptHolder" ContentPlaceHolderID="InnerScriptPlaceHolder" runat="server">
    <script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/modules/bootstrapvalidator.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
    <script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/modules/register.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>

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
                        Phone: $('[id$="txtConfirmPassword"]').val(),
                        CountryCode: $('[id$="drpCountryCode"]').val(),
                        ContactNumber: $('[id$="txtPhoneNumber"]').val(),
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
