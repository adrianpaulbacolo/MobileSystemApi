<%@ Page Language="C#" MasterPageFile="~/v2/MasterPages/Main.master" AutoEventWireup="true" CodeFile="ChangePassword.aspx.cs" Inherits="v2_ChangePassword" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContentHolder" runat="Server">
    <div class="form-container">
        <div class="container">
            <form class="form" id="form1" runat="server">
                <div class="form-group">
                    <asp:Label ID="lblPassword" runat="server" AssociatedControlID="txtPassword" />
                    <asp:TextBox ID="txtPassword" runat="server" type="password" CssClass="form-control" required data-require="" />
                </div>
                <div class="form-group">
                    <asp:Label ID="lblPasswordNew" runat="server" AssociatedControlID="txtPasswordNew" />
                    <asp:TextBox ID="txtPasswordNew" runat="server" type="password" MinLength="8" MaxLength="10" CssClass="form-control" required data-require="" />
                </div>
                <div class="form-group">
                    <asp:Label ID="lblPasswordConfirm" runat="server" AssociatedControlID="txtPasswordConfirm" />
                    <asp:TextBox ID="txtPasswordConfirm" runat="server" type="password" MinLength="8" MaxLength="10" CssClass="form-control" required data-confirmvalue="txtPasswordNew" />
                </div>
                <button type="submit" id="btnSubmit" class="btn btn-block btn-primary"></button>
            </form>
        </div>
    </div>
</asp:Content>

<asp:Content ID="InnerScriptBottom" ContentPlaceHolderID="InnerScriptPlaceHolder" runat="server">
    <script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/modules/bootstrapvalidator.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
    <script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/modules/changepassword.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>

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
