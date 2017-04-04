<%@ Page Language="C#" AutoEventWireup="true" CodeFile="enhancement-all.aspx.cs" Inherits="enhancement_all" Async="true"%> 
<%@ Import Namespace="W88.BusinessLogic.Shared.Helpers" %>
<!DOCTYPE html>
<html lang="en">
<head>
<meta>
    <meta charset="utf-16">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>Enhancement</title>
    <link rel="shortcut icon" href="/favicon.ico" type="image/x-icon">
    <link href="/_Static/Css/errors/styles.ab50292d.css" rel="stylesheet" type="text/css">
    <script src="/_Static/JS/Mobile/jquery-1.11.3.min.js"></script>
</head>
<body class="enhancement-all">

    <header>
        <a href="/"><img class="logo" src="/_Static/Images/logo-<%=LanguageHelpers.SelectedLanguageShort%>.png"></a>
    </header>
    
    <section>
        <div class="enhancement-container">
            <div class="enhancement-content-all">
                <div id="en" class="enhancement-lang">
                    <h3></h3>
                    <p></p>
                </div>
                <div id="cn" class="enhancement-lang">
                    <h3></h3> 
                    <p></p>
                </div>
                <div id="id" class="enhancement-lang">
                    <h3></h3>
                    <p></p>
                </div>
                <div id="jp" class="enhancement-lang">
                    <h3></h3>
                    <p></p>
                </div>
                <div id="kh" class="enhancement-lang">
                    <h3></h3>
                    <p></p>
                </div>
                <div id="kr" class="enhancement-lang">
                    <h3></h3>
                    <p></p>
                </div>
                <div id="th" class="enhancement-lang">
                    <h3></h3>
                    <p></p>
                </div>
                <div id="vn" class="enhancement-lang">
                    <h3></h3>
                    <p></p>
                </div>
            </div>
        </div>
    </section>

    <footer>
        <img src="/_Static/Images/errors/gpi-logo.png" alt="">
        <p><small>&copy; <%=DateTime.Now.Year%> <a href="/">W88</a>. All rights reserved.</small></p>
    </footer>

    <script>
        var language = '<%=LanguageHelpers.SelectedLanguage%>',
            translations = {
                en: {
                    title: "<%=GetTranslation("Enhancement_en", "messages")%>",
                    message: "<%=GetTranslation("Enhancement_Message_en", "messages")%>"
                },
                cn: {
                    title: "<%=GetTranslation("Enhancement_cn", "messages")%>",
                    message: "<%=GetTranslation("Enhancement_Message_cn", "messages")%>"
                },
                id: {
                    title: "<%=GetTranslation("Enhancement_id", "messages")%>",
                    message: "<%=GetTranslation("Enhancement_Message_id", "messages")%>"
                },
                jp: {
                    title: "<%=GetTranslation("Enhancement_jp", "messages")%>",
                    message: "<%=GetTranslation("Enhancement_Message_jp", "messages")%>"
                },
                kh: {
                    title: "<%=GetTranslation("Enhancement_kh", "messages")%>",
                    message: "<%=GetTranslation("Enhancement_Message_kh", "messages")%>"
                },
                kr: {
                    title: "<%=GetTranslation("Enhancement_kr", "messages")%>",
                    message: "<%=GetTranslation("Enhancement_Message_kr", "messages")%>"
                },
                th: {
                    title: "<%=GetTranslation("Enhancement_th", "messages")%>",
                    message: "<%=GetTranslation("Enhancement_Message_th", "messages")%>"
                },
                vn: {
                    title: "<%=GetTranslation("Enhancement_vn", "messages")%>",
                    message: "<%=GetTranslation("Enhancement_Message_vn", "messages")%>"
                }
            };
        $('div.enhancement-content-all').find($('div.enhancement-lang')).each(function() {
            var id = $(this)[0].id;
            $('#' + id + ' h3').html(translations[id].title);
            $('#' + id + ' p').html(translations[id].message);
        });
    </script>
</body>
</html>
