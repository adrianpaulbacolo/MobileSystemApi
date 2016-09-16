<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Site.master" AutoEventWireup="true" CodeFile="ForgotPassword.aspx.cs" Inherits="_Secure_ForgotPassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
     <style>
        .step2 {
            display: none;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
     <div class="ui-content" role="main">          
            <form class="form" id="form1" runat="server" data-ajax="false">
                <br/><br/>
                <ul class="list fixed-tablet-size">
                    <li class="item item-icon-left item-input">
                        <i class="icon icon-profile"></i>
                        <asp:Label ID="lblUsername" runat="server" AssociatedControlID="txtUsername" Text="Username" />
                        <asp:TextBox ID="txtUsername" runat="server" data-corners="false" autofocus="on" MaxLength="16" data-clear-btn="true" required />
                    </li>
                    <li class="item item-icon-left item-input">
                        <i class="icon icon-mail"></i>
                        <asp:Label ID="lblEmail" runat="server" AssociatedControlID="txtEmail" Text="Email" />
                        <asp:TextBox ID="txtEmail" runat="server" data-corners="false" data-clear-btn="true" required />
                    </li>
                    <li class="item item-icon-left item-select step2">
                        <i class="icon icon-security"></i>
                        <div>
                            <asp:Label ID="lblSecurityQuestion" runat="server" AssociatedControlID="drpSecurityQuestion" />
                        </div>
                        <div>
                            <asp:DropDownList ID="drpSecurityQuestion" runat="server" CssClass="ui-mini" />
                        </div>
                    </li>
                    <li class="item item-icon-left item-select step2">
                        <i class="icon icon-security"></i>
                        <div>
                            <asp:Label ID="lblSecurityAnswer" runat="server" AssociatedControlID="txtSecurityAnswer" Text="Security Answer" />
                        </div>
                        <div>
                            <asp:TextBox ID="txtSecurityAnswer" runat="server" />
                        </div>
                    </li>
                    <li class="item row">
                        <div class="col">
                            <a href="" role="button" data-rel="back" class="ui-btn btn-bordered"><%=commonCulture.ElementValues.getResourceString("cancel", commonVariables.LeftMenuXML)%></a>
                        </div>
                        <div class="col step1">
                            <asp:Button ID="btnStep1" runat="server" Text="submit" data-corners="false" CausesValidation="False" />
                        </div>
                        <div class="col step2">
                            <asp:Button ID="btnSubmit" runat="server" Text="submit" data-corners="false" OnClick="btnSubmit_Click"  />
                        </div>
                    </li>
                </ul>
            </form>
        </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsPlaceHolder" Runat="Server">
    
    <script>
        var firstStep = false;

        $(function() {
            if ('<%=AlertCode%>'.length > 0) {
                switch ('<%=AlertCode%>') {
                    case '1':
                        w88Mobile.Growl.shout('<%=AlertMessage%>');
                        window.location.replace('/Index.aspx?lang=<%=commonVariables.SelectedLanguage.ToLower()%>');
                        break;
                    default:
                        w88Mobile.Growl.shout('<%=AlertMessage%>');
                        break;
                }
            }

        });
     

        $(document).ready(function () {

            $('#<%=btnStep1.ClientID%>').click(function (e) {
                e.preventDefault();
                if ($("#<%=txtUsername.ClientID%>").val().length > 0 && $("#<%=txtEmail.ClientID%>").val().length > 0) {
                    check();
                } else {
                    ValidateStep1();
                }
            });

            $('#<%=btnSubmit.ClientID%>').click(function (e) {

                var s1, s2;
                s1 = ValidateStep1();

                if (firstStep == false) {

                    if (s1) {
                        return true;
                    } else return false;
                } else {
                    s1 = ValidateStep1();
                    s2 = ValidateStep2();

                    if (s1 && s2) {
                        return true;
                    } else {
                        e.preventDefault();
                        return false;
                    }
                }
            });

        });

        function check() {
            //show loading
            $.ajax({
                type: "POST",
                url: "ForgotPassword.aspx/CheckIfMemberPartial",
                dataType: "json",
                data: JSON.stringify({ username: $("#<%=txtUsername.ClientID%>").val(), email: $("#<%=txtEmail.ClientID%>").val() }),
                contentType: "application/json; charset=utf-8",
                success: function (msg) {
                    console.log(msg);

                    switch (msg.d) {
                    case 2:
                        $('.step1').hide();
                        $('.step2').show();
                        firstStep = true;
                        break;
                    case 1:
                        firstStep = false;
                        $('#<%=btnStep1.ClientID%>').click();
                        break;
                    case 10:
                    case 11:
                        w88Mobile.Growl.shout('<%=commonCulture.ElementValues.getResourceXPathString("ForgotPassword/NotExist", XeErrors)%>');
                        break;

                    default:
                        w88Mobile.Growl.shout('<%=commonCulture.ElementValues.getResourceXPathString("ForgotPassword/Other", XeErrors)%>');
                        break;
                    }
                }
            });
        }

        function ValidateStep1() {
            if ($("#<%=txtUsername.ClientID%>").val().trim().length == 0) {
                w88Mobile.Growl.shout('<%=commonCulture.ElementValues.getResourceXPathString("Register/MissingUsername", XeErrors)%>');
                return false;
            } else if ($("#<%=txtEmail.ClientID%>").val().trim().length == 0) {
                w88Mobile.Growl.shout('<%=commonCulture.ElementValues.getResourceXPathString("Register/MissingEmail", XeErrors)%>');
                return false;
            }

            return true;
        }

        function ValidateStep2() {
            if ($('#<%=drpSecurityQuestion.ClientID%>').val() == '0') {
                w88Mobile.Growl.shout('<%=commonCulture.ElementValues.getResourceXPathString("UpdateProfile/MissingSecurityQuestion", XeErrors)%>');
                return false;
            } else if ($("#<%=txtSecurityAnswer.ClientID%>").val().trim().length == 0) {
                w88Mobile.Growl.shout('<%=commonCulture.ElementValues.getResourceXPathString("UpdateProfile/MissingSecurityAnswer", XeErrors)%>');
                return false;
            }

            return true;
        }

    </script>
</asp:Content>

