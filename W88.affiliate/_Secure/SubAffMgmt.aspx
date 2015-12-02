<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SubAffMgmt.aspx.cs" Inherits="_Secure_SubAffMgmt" %>

<!DOCTYPE html>
<html>
<head>
    <title><%=commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML) + commonCulture.ElementValues.getResourceString("myAccount", commonVariables.LeftMenuXML)%></title>
    <!--#include virtual="~/_static/head.inc" -->
   <%-- <script type="text/javascript" src="/_Static/Js/PreLoad.js"></script>--%>
    <link type="text/css" rel="stylesheet" href="/_Static/Css/MyAccount.css" />
</head>
<body>
    <!--#include virtual="~/_static/splash.shtml" -->
    <div id="divMain" data-role="page">
        <!--#include virtual="~/_static/header.shtml" -->
        <div class="ui-content" role="main">
            <div class="div-page-header"><span><%=commonCulture.ElementValues.getResourceString("myAccount", commonVariables.LeftMenuXML)%></span></div>
            <div class="page-content">
                <form id="form1" runat="server" data-ajax="false">
                <div class="div-content-wrapper">
                                        
                        <div style="text-align:center;">
                            <span><%=commonCulture.ElementValues.getResourceString("subAffmgmt", commonVariables.LeftMenuXML)%></span>
                        </div>
                  
                   <%-- <div class="div-account-subAff">--%>
                       <div>
                            <asp:Label ID="lblEmailAddress" runat="server" />
                        </div>                        
                       <%-- <div>--%>
                            <asp:TextBox ID="txtEmail1" runat="server" CssClass="ui-mini" data-clear-btn="true" />
                            <asp:TextBox ID="txtEmail2" runat="server" CssClass="ui-mini" data-clear-btn="true" />
                            <asp:TextBox ID="txtEmail3" runat="server" CssClass="ui-mini" data-clear-btn="true" />
                       <%-- </div>
                        <div>--%>
                            <asp:TextBox ID="txtEmail4" runat="server" CssClass="ui-mini" data-clear-btn="true" />
                            <asp:TextBox ID="txtEmail5" runat="server" CssClass="ui-mini" data-clear-btn="true" />
                            <%--<asp:TextBox ID="TextBox6" runat="server" placeholder=" " CssClass="ui-mini" data-clear-btn="true" />
                            <asp:TextBox ID="TextBox7" runat="server" placeholder=" " CssClass="ui-mini" data-clear-btn="true" />--%>
                        <%--</div>--%>
                    <%--</div>--%>

                    <%--<div class="div-account-message">--%>
                                            
                        <div>
                            <asp:Label ID="lblMessage" runat="server" />
                        </div>
                            
                        <div>
                            <asp:Literal ID="mainContent" runat="server"></asp:Literal>
                        </div>
                    <%--</div>--%>


                    <div>
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="button-blue" data-corners="false" OnClick="btnSubmit_Click" />
                    </div>
                    <div>
                        <a data-theme="c" ID="btnCancel" runat="server" Text="cancel" class="ui-btn" data-corners="false" data-ajax="false" href="/Index" />
                    </div>
                  <%--  <asp:HiddenField id="hidValues" runat="server" />
                	<asp:HiddenField runat="server" ID="ioBlackBox" Value="" />--%>
                </div>
                </form>
            </div>
        </div>
        <!-- /content -->
        <!--#include virtual="~/_static/footer.shtml" -->
        <!--#include virtual="~/_static/navMenu.shtml" -->
        <script type="text/javascript">

    
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
                            //window.location.replace('/Index.aspx');
                            break;
                        default:
                            break;
                    }
                }

            $('#form1').submit(function (e) {
                $('#btnSubmit').attr("disabled", true);
                if ($('#txtEmail1').val().trim().length == 0 && $('#txtEmail2').val().trim().length == 0 && $('#txtEmail3').val().trim().length == 0 && $('#txtEmail4').val().trim().length == 0 && $('#txtEmail5').val().trim().length == 0) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("SubAffiliate/MissingValidEmail", xeErrors)%>');
                    $('#btnSubmit').attr("disabled", false);
                    e.preventDefault();
                    return;
                }
                else { $('#btnSubmit').attr("disabled", false); }
            });
                      

        </script>
               
    </div>
    <!-- /page -->
</body>
</html>