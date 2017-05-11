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
                    <label for="">Reference ID</label>
                    <div class="input-group">
                        <input type="text" class="form-control input-upload" placeholder="Upload File" readonly>
                        <input type="file" class="hidden" />
                        <span class="input-group-btn">
                            <button class="btn btn-primary btn-upload" type="button"><span class="icon-submit"></span></button>
                        </span>
                    </div>
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


            // Script for Upload field
            $(".btn-upload").click(function() {
                var upload = $(this).parent().siblings('input[type="file"]');
                upload.click();
               
               $(document).on('change',upload, function(){
                    var filename = upload.val().replace(/C:\\fakepath\\/i, '')
                    upload.siblings('.input-upload').val(filename);
               });
            });
        });
    </script>
</asp:Content>
