<%@ Page Language="C#" MasterPageFile="~/v2/MasterPages/Main.master" AutoEventWireup="true" CodeFile="Upload.aspx.cs" Inherits="v2_Upload" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContentHolder" runat="Server">
    <div class="form-container">
        <div class="container">
            <form class="form" id="form1" runat="server">
                <div class="form-group">
                    <asp:Label ID="lblFileUpload" runat="server" AssociatedControlID="fileUpload" />
                    <asp:FileUpload ID="fileUpload" runat="server" AllowMultiple="false" accept="image/*" required data-require="" />
                </div>
                <div class="form-group">
                    <asp:Label ID="lblRemarks" runat="server" AssociatedControlID="txtRemarks" />
                    <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" />
                </div>
                <button type="submit" id="btnSubmit" class="btn btn-block btn-primary"></button>
            </form>
        </div>
    </div>
</asp:Content>

<asp:Content ID="ScriptHolder" ContentPlaceHolderID="InnerScriptPlaceHolder" runat="server">
    <script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/modules/bootstrapvalidator.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
    <script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/modules/upload.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            _w88_upload.init();

            $('#form1').validator().on('submit', function (e) {
                if (!e.isDefaultPrevented()) {
                    e.preventDefault();

                    _w88_upload.uploadImage();
                }
            });
        });
    </script>
</asp:Content>
