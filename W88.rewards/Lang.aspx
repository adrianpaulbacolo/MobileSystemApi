<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Lang.aspx.cs" Inherits="_Lang" Async="true"%>
<%@ Import Namespace="W88.BusinessLogic.Rewards.Helpers" %>
<%@ Import Namespace="W88.BusinessLogic.Rewards.Models" %>

<!DOCTYPE html>
<html>
<head>
    <title><%=RewardsHelper.GetTranslation(TranslationKeys.Label.Brand)%></title>
    <!--#include virtual="~/_static/head.inc" -->
</head>
<body>
    <div data-role="page" data-theme="b">
        <!--#include virtual="~/_static/header.shtml" -->
        <form id="form1" runat="server">
            <section class="main-content">
                <div class="container">
                    <div class="page-title">
                        <h3 class="text-center"><%=RewardsHelper.GetTranslation(TranslationKeys.Label.SelectLanguage)%></h3>
                    </div>
                    <div class="language-box">
                        <div runat="server" ID="divLanguageContainer" class="row"></div>
                    </div>
                </div>
            </section>
        </form>
    </div>
    <script>
        $(function () {
            $('#divLanguageContainer div.col-xs-6 span').each(function () {
                $(this).on('click', function () {
                    var id = $(this).attr('id'),
                        langCode = 'en-us';
                    switch (id) {
                        case 'en-us':
                            langCode = 'en-us';
                            break;
                        case 'en-my':
                            langCode = 'en-us';
                            break;
                        case 'id-id':
                            langCode = 'id-id';
                            break;
                        case 'ja-jp':
                            langCode = 'ja-jp';
                            break;
                        case 'km-kh':
                            langCode = 'km-kh';
                            break;
                        case 'ko-kr':
                            langCode = 'ko-kr';
                            break;
                        case 'th-th':
                            langCode = 'th-th';
                            break;
                        case 'vi-vn':
                            langCode = 'vi-vn';
                            break;
                        case 'zh-cn':
                            langCode = 'zh-cn';
                            break;
                        case 'zh-my':
                            langCode = 'zh-cn';
                            break;
                    }
                    Cookies().setCookie('language', langCode);
                    window.location.href = (<%=Convert.ToString(HasSession).ToLower()%> ? '/Index.aspx?lang=' : '/Default.aspx?lang=') + langCode;
                });
            });
        });
    </script>
</body>
</html>