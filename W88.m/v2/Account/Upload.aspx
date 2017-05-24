<%@ Page Language="C#" MasterPageFile="~/v2/MasterPages/Main.master" AutoEventWireup="true" CodeFile="Upload.aspx.cs" Inherits="v2_Account_Upload" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContentHolder" runat="Server">
    <div class="form-container">
        <div class="container">
            <form class="form" id="form1" runat="server">
                <div class="form-group">
                    <label id="lblFileUpload" for="" data-i18n="LABEL_FILE" ></label>
                    <div class="input-group">
                        <input type="text" class="form-control input-upload" placeholder="Upload Image" readonly>
                        <input type="file" class="hidden" id="fileUpload" accept="image/*" required data-require="" />
                        <span class="input-group-btn">
                            <button class="btn btn-primary btn-upload" type="button"><span class="icon-submit"></span></button>
                        </span>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label ID="lblRemarks" runat="server" AssociatedControlID="txtRemarks" data-i18n="LABEL_REMARKS" />
                    <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" />
                </div>
                <button type="submit" id="btnSubmit" class="btn btn-block btn-primary" data-i18n="BUTTON_SUBMIT"></button>
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
        });
    </script>
</asp:Content>