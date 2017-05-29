<%@ Page Language="C#" MasterPageFile="~/v2/MasterPages/Main.master" AutoEventWireup="true" CodeFile="ChangePassword.aspx.cs" Inherits="v2_Account_ChangePassword" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContentHolder" runat="Server">
    <div class="form-container">
        <div class="container">
            <form class="form" id="form1" runat="server">
                <div class="form-group">
                    <asp:Label ID="lblPassword" runat="server" AssociatedControlID="txtPassword" data-i18n="LABEL_PASSWORD_CURRENT" />
                    <asp:TextBox ID="txtPassword" runat="server" type="password" CssClass="form-control" required data-require="" />
                </div>
                <div class="form-group">
                    <asp:Label ID="lblPasswordNew" runat="server" AssociatedControlID="txtPasswordNew" data-i18n="LABEL_PASSWORD_NEW" />
                    <asp:TextBox ID="txtPasswordNew" runat="server" type="password" MinLength="8" MaxLength="10" CssClass="form-control" required data-require="" />
                </div>
                <div class="form-group">
                    <asp:Label ID="lblPasswordConfirm" runat="server" AssociatedControlID="txtPasswordConfirm" data-i18n="LABEL_PASSWORD_CONFIRM" />
                    <asp:TextBox ID="txtPasswordConfirm" runat="server" type="password" MinLength="8" MaxLength="10" CssClass="form-control" required data-confirmvalue="txtPasswordNew" />
                </div>
                <button type="submit" id="btnSubmit" class="btn btn-block btn-primary"  data-i18n="BUTTON_SUBMIT"></button>
            </form>
        </div>
    </div>
</asp:Content>

<asp:Content ID="InnerScriptBottom" ContentPlaceHolderID="InnerScriptPlaceHolder" runat="server">
    <script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/modules/bootstrapvalidator.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
    <script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/modules/account/changepassword.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>

    <script>
        $(document).ready(function () {
            _w88_changepassword.init();

            $('#form1').validator().on('submit', function (e) {
                if (!e.isDefaultPrevented()) {
                    e.preventDefault();
                    var data = {
                        Password: $('input[id$="txtPassword"]').val(),
                        NewPassword: $('input[id$="txtPasswordNew"]').val(),
                        ConfirmPassword: $('input[id$="txtPasswordConfirm"]').val(),
                    };

                    _w88_changepassword.send(data);
                }
            });
        });
    </script>
</asp:Content>
