<%@ Page Language="C#" MasterPageFile="~/v2/MasterPages/Main.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="_v2_Account_Login" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="head" runat="server">
    <!-- Modal -->
    <div class="modal fade" id="freerounds-modal" tabindex="-1" role="dialog" aria-labelledby="freerounds">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-body">
                    <img class="img-responsive" src="/_static/images/v2/freerounds/Popup-free-round-<%=commonVariables.SelectedLanguageShort.ToLower()%>.jpg" />
                    <div class="row extra-thin-gutter">
                        <div class="col-xs-6 col-sm-6">
                            <a id="btnClaimNow" href="#" data-ajax="false"
                                class="btn btn-block btn-default" data-i18n="BUTTON_CLAIM"></a>
                        </div>
                        <div class="col-xs-6 col-sm-6">
                            <a id="btnClaimLater" href="/v2/slots/bravado" data-ajax="false"
                                class="btn btn-block btn-default" data-i18n="BUTTON_CLAIM_LATER"></a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContentHolder" runat="Server">
    <section class="viplogin">
        <div class="viplogin-container">
            <div class="viplogin-box">
                <% if (!commonVariables.isVIPDomain)
                   { %>
                <img src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/images/w88.png" alt="" class="viplogo" />
                <% } %>
                <form class="viplogin-form" id="form1" runat="server">
                    <% if (commonVariables.isVIPDomain)
                       { %>
                    <h3><span data-i18n="LABEL_VIP_LOGIN"></span></h3>
                    <% } %>
                    <div class="form-group">
                        <div class="input-group">
                            <input type="text" class="form-control" placeholder="" id="txtUsername" maxlength="20" required data-require="" />
                            <span class="input-group-icon">
                                <img src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/images/icon-user.png" alt="" />
                            </span>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="input-group">
                            <input type="password" class="form-control" placeholder="" id="txtPassword" maxlength="20" required data-require="">
                            <span class="input-group-icon">
                                <img src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/images/icon-password.png" alt="" />
                            </span>
                        </div>
                    </div>
                    <div class="form-group captcha" hidden>
                        <img id="imgCaptcha" class="imgCaptcha" src="/v2/Account/Captcha.ashx" alt="" />
                        <asp:HiddenField ID="vCode" runat="server" />
                        <div class="input-group">
                             <input type="text" class="form-control" placeholder="" id="txtCaptcha" maxlength="4">
                            <span class="input-group-icon">
                                <img src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/images/icon-password.png" alt="" />
                            </span>
                        </div>
                    </div>
                    <div class="form-group">
                         <input type="submit" id="btnSubmit" class="btn btn-block btn-primary" data-i18n="BUTTON_SUBMIT" />
                    </div>
                    <div class="text-center">
                        <a href="/v2/Account/ForgotPassword.aspx" data-ajax="false"><span data-i18n="LABEL_PASSWORD_FORGOT"></span></a>
                    </div>
                </form>
                <div class="text-center">
                    <div id="loginNote"></div>
                    <div id="sslNote" data-i18n="LABEL_LOGIN_NOTE_SSL"></div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>

<asp:Content ID="ScriptHolder" ContentPlaceHolderID="InnerScriptPlaceHolder" runat="server">
    <script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/modules/bootstrapvalidator.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
    <script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/products.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
    <script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/modules/accounts/login.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"> </script>

    <script type="text/javascript">
        $(document).ready(function () {
            _w88_login.init();

            $('#form1').validator().on('submit', function (e) {
                if (!e.isDefaultPrevented()) {
                    e.preventDefault();

                    var data = {
                        Username: $('[id$="txtUsername"]').val(),
                        Password: $('[id$="txtPassword"]').val(),
                        CaptchCode: $('[id$="txtCaptcha"]').val(),
                        VerificationCode: $('[id$="vCode"]').val(),
                    };

                    _w88_login.login(data);
                }
            });
        });
    </script>
</asp:Content>
