<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Site.master" AutoEventWireup="true" CodeFile="ChangePassword.aspx.cs" Inherits="Settings.SettingsChangePassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="ui-content" role="main">
        <form id="form1" class="form" runat="server" data-ajax="false">
            <p>&nbsp;</p>
            <ul class="list fixed-tablet-size">
                <li class="item item-input">

                    <div>
                        <asp:Label ID="lblPassword" runat="server" AssociatedControlID="txtPassword" />
                    </div>
                    <asp:TextBox name="txtPassword" runat="server" type="password" MaxLength="20" ID="txtPassword" data-mini="true" data-clear-btn="true" />
                </li>
                <li class="item item-input">

                    <div>
                        <asp:Label ID="lblPasswordNew" runat="server" AssociatedControlID="txtPasswordNew" />
                    </div>
                    <asp:TextBox name="txtPasswordNew" runat="server" type="password" MaxLength="40" ID="txtPasswordNew" data-mini="true" data-clear-btn="true" />
                </li>
                <li class="item item-input">

                    <div>
                        <asp:Label ID="lblPasswordConfirm" runat="server" AssociatedControlID="txtPasswordConfirm" />
                    </div>
                    <asp:TextBox name="txtPasswordConfirm" runat="server" type="password" MaxLength="40" ID="txtPasswordConfirm" data-mini="true" data-clear-btn="true" />
                </li>

                <li class="item row">
                    <div class="col">
                        <a href="/Index" id="btnCancel" class="ui-btn btn-bordered" text="cancel" data-corners="false" data-ajax="false"><%=commonCulture.ElementValues.getResourceString("cancel", commonVariables.LeftMenuXML)%></a>
                    </div>
                    <div class="col">
                        <asp:Button ID="btnSubmit" name="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
                    </div>
                </li>

            </ul>
        </form>
    </div>

    <script type="text/javascript">
        $(function () {
            if ('<%=strAlertCode%>'.length > 0) {
                switch ('<%=strAlertCode%>') {
                    case '-1':
                        alert('<%=strAlertMessage%>');
                        break;
                    case '0':
                        alert('<%=strAlertMessage%>');
                        break;
                    case '1':
                        alert('<%=strAlertMessage%>');
                        window.location.replace('/Index.aspx');
                        break;
                    default:
                        break;
                }
            }
        });

        $('#form1').submit(function (e) {
            $('#btnSubmit').attr("disabled", true);
            if ($('#txtPassword').val().trim().length == 0) {
                alert('<%=commonCulture.ElementValues.getResourceXPathString("UpdatePassword/MissingPassword", xeErrors)%>');
                    $('#btnSubmit').attr("disabled", false);
                    e.preventDefault();
                    return;
                } else if ($('#txtPasswordNew').val().trim().length == 0) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("UpdatePassword/MissingPasswordNew", xeErrors)%>');
                    $('#btnSubmit').attr("disabled", false);
                    e.preventDefault();
                    return;
                } else if ($('#txtPasswordConfirm').val().trim().length == 0) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("UpdatePassword/MissingPasswordNew", xeErrors)%>');
                $('#btnSubmit').attr("disabled", false);
                e.preventDefault();
                return;
            } else if ($('#txtPasswordNew').val().trim() != $('#txtPasswordConfirm').val().trim()) {
                alert('<%=commonCulture.ElementValues.getResourceXPathString("UpdatePassword/UnmatchedPassword", xeErrors)%>');
                    $('#btnSubmit').attr("disabled", false);
                    e.preventDefault();
                    return;
                } else {
                    $('#btnSubmit').attr("disabled", false);
                }
            });
    </script>

</asp:Content>

