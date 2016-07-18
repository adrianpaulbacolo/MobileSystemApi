<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Site.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Upload_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="ui-content" role="main">
        <form class="form" id="form1" runat="server" data-ajax="false">
            <ul class="list fixed-tablet-size">
                <li class="item-text-wrap">
                    <p>
                        <asp:Literal ID="lblSuccess" runat="server"></asp:Literal></p>
                    <p>
                        <strong>
                            <asp:Label ID="lblUsername" runat="server" /></strong>
                        <asp:Literal ID="txtUsername" runat="server" />
                    </p>
                    <p>
                        <strong>
                            <asp:Label ID="lblCurrency" runat="server" /></strong>
                        <asp:Literal ID="txtCurrency" runat="server" />
                    </p>
                </li>
                <li class="item-text-wrap">
                    <asp:Label ID="lblFileUpload" runat="server" />
                    <div class="file-upload">
                         <span class="file-upload-btn"><span class="icon icon-submit"></span><%=commonCulture.ElementValues.getResourceString("lblFileUploadText", xeResources) %></span>
                         <span class="file-upload-filename"><div  id="txtupl"><%=commonCulture.ElementValues.getResourceString("lblNoFileChosen", xeResources) %></div></span>
                         <asp:FileUpload ID="fuFileUpload" runat="server" AllowMultiple="false" onchange="getUplFile()" />
                    </div>
                </li>
                <li class="item item-textarea">
                    <asp:Label ID="lblRemarks" runat="server" />
                    <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" />
                </li>
                <li class="item row">
                    <div class="col">
                        <a href="" role="button" data-rel="back" class="ui-btn btn-bordered"><%=commonCulture.ElementValues.getResourceString("cancel", commonVariables.LeftMenuXML)%></a>
                    </div>
                    <div class="col">
                        <asp:Button data-theme="b" ID="btnSubmit" runat="server" Text="upload" data-corners="false" OnClick="btnSubmit_Click" />
                    </div>
                </li>
            </ul>
               <asp:HiddenField ID="HfUploadLabel" runat="server" />
        </form>
    </div>

 
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsPlaceHolder" runat="Server">

    <script type="text/javascript">
        function getUplFile() {
            var filename = $('#<%=fuFileUpload.ClientID%>').val();
            $('#txtupl').html(filename.replace("C:\\fakepath\\", ""));
        }

        $(function () {
            $('.div-content-wrapper > div:first-child').hide();
            var code = '<%=strAlertCode%>';
            var message = '<%=strAlertMessage%>';

            switch (code) {
            case "00":
                $('.div-content-wrapper > div:nth-child(n+1)').hide();
                $('.div-content-wrapper > div:first-child').show();
                break;

            case "01":
                alert(message);
                break;
            }
        });
    </script>

</asp:Content>
