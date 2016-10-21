<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChangePassword.aspx.cs" Inherits="_Secure_ChangePassword" Async="true"%>
<%@ Import Namespace="W88.BusinessLogic.Rewards.Helpers" %>
<%@ Import Namespace="W88.BusinessLogic.Rewards.Models" %>
<%@ Import Namespace="W88.BusinessLogic.Shared.Helpers" %>

<!DOCTYPE html>
<html>
<head>
    <title><%=RewardsHelper.GetTranslation(TranslationKeys.Label.Brand)%></title>
    <!--#include virtual="~/_static/head.inc" -->
    <style>  
        h6 {
            text-align: center;
            font-size: 15px;
        }     
    </style>
</head>
<body>
    <div data-role="page" data-theme="b">
        <!--#include virtual="~/_static/header.shtml" -->
        <form id="form1" runat="server">
            <div role="main" class="main-content">               
                <div class="container">
                    <h6><%=RewardsHelper.GetTranslation("LABEL_CHANGEPASSWORD_TITLE", Language, TranslationsPath)%></h6>
                    <div class="form-container login">
                        <div class="form-group form-group-line">
                            <asp:Label ID="lblCurrentPassword" runat="server" AssociatedControlID="txtCurrentPassword" Text="" />
                            <asp:TextBox ID="txtCurrentPassword" runat="server" TextMode="Password" data-corners="false" autofocus="on" MaxLength="10" CssClass="form-control" />
                        </div>
                        <div class="form-group form-group-line">
                            <asp:Label ID="lblNewPassword" runat="server" AssociatedControlID="txtNewPassword" Text="" />
                            <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" data-corners="false" MaxLength="10" CssClass="form-control" />
                        </div>
                        <div class="form-group form-group-line">
                            <asp:Label ID="lblConfirmPassword" runat="server" AssociatedControlID="txtConfirmPassword" Text="" />
                            <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" data-corners="false" MaxLength="10" CssClass="form-control" />
                        </div>
                    </div>
                </div>
            </div>
            <div>
                <section class="footer footer-public">
                    <div class="btn-group btn-group-justified" role="group">
                        <div class="btn-group" role="group">
                            <a data-ajax="false" class="btn btn-generic" href="<%="/Index.aspx?lang=" + LanguageHelpers.SelectedLanguage %>">
                                <span class="icon"></span><%=RewardsHelper.GetTranslation("BUTTON_CANCEL", Language, TranslationsPath)%>
                            </a>
                        </div>
                        <div class="btn-group" role="group">                                                   
                            <a id="changePasswordBtn" href="#" class="btn btn-generic">
                                <span class="icon"></span><%=RewardsHelper.GetTranslation("BUTTON_SUBMIT", Language, TranslationsPath)%>
                            </a>
                        </div>
                    </div>
                </section>
            </div>
        </form>
    </div>

    <script type="text/javascript">
        $(function () {           
            $('#changePasswordBtn').click(function (e) {
                $('#changePasswordBtn').attr('disabled', true);

                if ($('#txtCurrentPassword').val().trim().length == 0) {
                    showMessage('<%=RewardsHelper.GetTranslation("LABEL_CHANGEPASSWORD_ENTER_CURRENT", Language, TranslationsPath)%>');
                    $('#changePasswordBtn').attr('disabled', false);
                    e.preventDefault();
                    return;
                }
                if ($('#txtNewPassword').val().trim().length == 0) {
                    showMessage('<%=RewardsHelper.GetTranslation("LABEL_CHANGEPASSWORD_ENTER_NEW", Language, TranslationsPath)%>');
                    $('#changePasswordBtn').attr('disabled', false);
                    e.preventDefault();
                    return;
                }
                if ($('#txtConfirmPassword').val().trim().length == 0) {
                    showMessage('<%=RewardsHelper.GetTranslation("LABEL_CHANGEPASSWORD_ENTER_CONFIRM", Language, TranslationsPath)%>');
                    $('#changePasswordBtn').attr('disabled', false);
                    e.preventDefault();
                    return;
                }
                if ($('#txtNewPassword').val().trim() !== $('#txtConfirmPassword').val().trim()) {
                    showMessage('<%=RewardsHelper.GetTranslation("LABEL_CHANGEPASSWORD_MISMATCH", Language, TranslationsPath)%>');
                    $('#changePasswordBtn').attr('disabled', false);
                    e.preventDefault();
                    return;
                }
                changePassword();
                e.preventDefault();
            });
        });

        function changePassword() {
            $.ajax({
                type: 'POST',
                contentType: 'application/json',
                url: '/api/user/changepassword',
                beforeSend: function () {
                    GPINTMOBILE.ShowSplash();
                },
                timeout: function () {
                    $('#changePasswordBtn').prop('disabled', false);
                    showMessage('<%=RewardsHelper.GetTranslation(TranslationKeys.Errors.Exception)%>');
                },
                data: JSON.stringify({                     
                    MemberId: !_.isEmpty(window.user) && window.user.hasSession() ? window.user.MemberId : '',
                    Password: $('#txtCurrentPassword').val(),
                    NewPassword: $('#txtNewPassword').val(),
                    ConfirmPassword: $('#txtConfirmPassword').val(),
                    Language: '<%=Language%>'
                }),
                success: function (response) {
                    if (!response || response.ResponseCode == undefined) {
                        showMessage('<%=RewardsHelper.GetTranslation(TranslationKeys.Errors.Exception)%>');
                        return;
                    }

                    var message = response.ResponseMessage;
                    switch (response.ResponseCode) {
                        case 1:
                            GPINTMOBILE.HideSplash();
                            showMessage(message);

                            var closeButtons = $('#PopUpModal a');
                            if (!closeButtons || closeButtons.length === 0) {                                
                                setTimeout(function() {
                                    loadPage('<%=string.Format("/Index.aspx?lang={0}", Language)%>');
                                }, 3000);
                                return;
                            }
                            closeButtons.each(function () {
                                $(this).attr('data-rel', null);
                                $(this).on('click', function() {
                                    loadPage('<%=string.Format("/Index.aspx?lang={0}", Language)%>');
                                });
                            });
                            break;
                        default:
                            $('#changePasswordBtn').attr('disabled', false);
                            GPINTMOBILE.HideSplash();
                            showMessage(message);
                            break;
                    }
                },
                error: function () {
                    GPINTMOBILE.HideSplash();
                    showMessage('<%=RewardsHelper.GetTranslation(TranslationKeys.Errors.Exception)%>');
                    $('#changePasswordBtn').attr('disabled', false);
                }
            });
        }

        function showMessage(message) {
            if (_.isEmpty(message)) return;
            window.w88Mobile.Growl.shout('<div>' + message + '</div>');
        }
    </script>
</body>
</html>