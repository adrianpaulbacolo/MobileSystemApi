<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Lang.aspx.cs" Inherits="_Lang" %>
<%@ Import Namespace="W88.BusinessLogic.Shared.Helpers" %>

<!DOCTYPE html>
<html>
<head>
    <title><%=CultureHelpers.ElementValues.GetResourceString("brand", LeftMenu) + CultureHelpers.ElementValues.GetResourceString("language", LeftMenu)%></title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/Js/PreLoad.js"></script>
    <link type="text/css" rel="stylesheet" href="/_Static/Css/Language.css" />
    <link rel="stylesheet" href="/_Static/Css/add2home.css">
    <script type="application/javascript" src="/_Static/Js/add2home.js"></script>
    <script type="application/javascript" src="/_Static/Js/checkManifest.js"></script>
</head>
<body>
    <div data-role="page" data-theme="b">
        <!--#include virtual="~/_static/header.shtml" -->
        <section class="main-content">
            <div class="container">
                <div class="page-title">
                    <h3 class="text-center"><%=CultureHelpers.ElementValues.GetResourceString("selectLanguage", LeftMenu)%></h3>
                </div>
                <div class="language-box">
                    <div runat="server" ID="divLanguageContainer" class="row">
                        <div class="col-xs-6">
                            <a href="/Index.aspx?lang=en-us">
                                <img src="/_Static/Css/images/flags/en.svg" alt="">
                                <span>English</span>
                            </a>
                        </div>
                        <div class="col-xs-6">
                            <a href="/Index.aspx?lang=zh-cn">
                                <img src="/_Static/Css/images/flags/cn.svg" alt="">
                                <span>简体中文</span>
                            </a>
                        </div>
                        <div class="col-xs-6">
                            <a href="/Index.aspx?lang=vi-vn">
                                <img src="/_Static/Css/images/flags/vn.svg" alt="">
                                <span>Tiếng Việt</span>
                            </a>
                        </div>
                        <div class="col-xs-6">
                            <a href="/Index.aspx?lang=th-th">
                                <img src="/_Static/Css/images/flags/th.svg" alt="">
                                <span>ไทย</span>
                            </a>
                        </div>
                        <div class="col-xs-6">
                            <a href="/Index.aspx?lang=km-kh">
                                <img src="/_Static/Css/images/flags/kh.svg" alt="">
                                <span>ខ្មែរ</span>
                            </a>
                        </div>
                        <div class="col-xs-6">
                            <a href="/Index.aspx?lang=id-id">
                                <img src="/_Static/Css/images/flags/id.svg" alt="">
                                <span>Bhs Indonesia</span>
                            </a>
                        </div>
                        <div class="col-xs-6">
                            <a href="/Index.aspx?lang=ko-kr">
                                <img src="/_Static/Css/images/flags/kr.svg" alt="">
                                <span>한국어</span>
                            </a>
                        </div>
                        <div class="col-xs-6">
                            <a href="/Index.aspx?lang=ja-jp">
                                <img src="/_Static/Css/images/flags/jp.svg" alt="">
                                <span>日本語</span>
                            </a>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>
</body>
</html>