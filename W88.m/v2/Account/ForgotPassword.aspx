<%@ Page Title="" Language="C#" MasterPageFile="~/v2/MasterPages/Main.master" AutoEventWireup="true" CodeFile="ForgotPassword.aspx.cs" Inherits="v2_Account_ForgotPassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContentHolder" runat="Server">
    <div class="form-container">
        <div class="container">
            <form class="form" id="form1" runat="server">
                <div id="forgot"></div>

            </form>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="InnerScriptPlaceHolder" runat="Server">
    <script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/modules/bootstrapvalidator.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
    <script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/modules/accounts/forgotpassword.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>

    <script>
        $(document).ready(function () {
            _w88_ForgotPassword.init();
        });
    </script>

    <script type="text/template" id='Step1Template'>
        <div class="form-group">
            <label data-i18n="LABEL_USERNAME"></label>
            <input id="txtUsername" class="form-control" maxlength="16" required data-require="" />
        </div>
        <div class="form-group ">
            <label data-i18n="LABEL_EMAIL"></label>
            <input id="txtEmail" class="form-control"  required data-require="" />
        </div>
        <div class="form-group">
            <button type="submit" id="btnSubmit" class="btn btn-block btn-primary" data-i18n="BUTTON_SUBMIT" onclick="javascript: _w88_ForgotPassword.checkPartial(); return false;"></button>
        </div>

    </script>

    <script type="text/template" id='Step2Template'>
        <div class="form-group">
            <label data-i18n="LABEL_SECURITY_QUESTION"></label>
            <select id="questions" class="form-control" required data-selectequals="0"></select>
        </div>
        <div class="form-group">
            <label data-i18n="LABEL_SECURITY_ANSWER"></label>
            <input id="txtSecurityAnswer" class="form-control" data-i18n="LABEL_EMAIL" required data-require="" />
        </div>
        <div class="form-group">
            <button type="submit" id="btnSendForgot" data-i18n="BUTTON_SUBMIT" class="btn btn-block btn-primary" onclick="javascript: _w88_ForgotPassword.submit(); return false;"></button>
        </div>
    </script>
</asp:Content>

