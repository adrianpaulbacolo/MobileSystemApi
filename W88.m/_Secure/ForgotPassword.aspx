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
                            <asp:TextBox ID="txtSecurityAnswer" runat="server" data-corners="false" data-clear-btn="true" />
                        </div>
                    </li>
                    <li class="item row">
                        <div class="col">
                            <a href="/Index.aspx" role="button" class="ui-btn btn-bordered" data-ajax="false"><%= commonCulture.ElementValues.getResourceString("cancel", commonVariables.LeftMenuXML) %></a>
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
        var firstStep = true;
        var reqSeq = false;

        $(function() {
            if ('<%=AlertCode%>'.length > 0) {
                switch ('<%=AlertCode%>') {
                    case '1':
                        sessionStorage.removeItem("mp");
                        alert('<%=AlertMessage%>');
                        window.location.replace('/Index.aspx?lang=<%=commonVariables.SelectedLanguage.ToLower()%>');
                        break;
                    default:
                        w88Mobile.Growl.shout('<%=AlertMessage%>');
                        break;
                }
            }

            if ($("#<%=txtUsername.ClientID%>").val().length <= 0) {
                firstStep = true;
                reqSeq = false;
                $('.step1').show();
                $('.step2').hide();
            }
            else if (sessionStorage.getItem("mp") == 2) {
                $('.step1').hide();
                $('.step2').show();
                firstStep = false;
                reqSeq = true;
            }

        });
     

        $(document).ready(function () {

            $('#<%=btnStep1.ClientID%>').click(function (e) {
                e.preventDefault();
                if ($("#<%=txtUsername.ClientID%>").val().length > 0 && $("#<%=txtEmail.ClientID%>").val().length > 0) {
                    if (firstStep) {
                        check();
                    } else {
                        if (ValidateStep1()) {
                            $('.step1').hide();
                            $('.step2').show();
                        }
                    }
                } else {
                    ValidateStep1();
                }
            });

            $('#<%=btnSubmit.ClientID%>').click(function (e) {

                if (!ValidateStep1()) {
                    return false;
                }

                if (firstStep == false) {
                    if (reqSeq) {
                        if (ValidateStep2()) {
                            return true;
                        } else {
                            e.preventDefault();
                            return false;
                        }
                    }
                }

                return true;
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

                    if (msg.d == 2) {
                        $('.step1').hide();
                        $('.step2').show();
                        firstStep = false;
                        reqSeq = true;
                        $('#<%=drpSecurityQuestion.ClientID%>').val("0").change();
                        $("#<%=txtSecurityAnswer.ClientID%>").text = '';
                        sessionStorage.setItem("mp", msg.d);
                    }
                    else if (msg.d == 1) {
                        firstStep = false;
                        reqSeq = false;
                        $('#<%=drpSecurityQuestion.ClientID%>').val("0").change();
                        $('#<%=btnSubmit.ClientID%>').click();
                    }
                    else if (msg.d == 10 || msg.d == 11) {
                        w88Mobile.Growl.shout('<%=commonCulture.ElementValues.getResourceXPathString("ForgotPassword/NotExist", XeErrors)%>');
                    }
                    else {
                        w88Mobile.Growl.shout('<%=commonCulture.ElementValues.getResourceXPathString("ForgotPassword/Other", XeErrors)%>');
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

