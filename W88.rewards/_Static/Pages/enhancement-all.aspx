<%@ Page Language="C#" AutoEventWireup="true" %>
<%@ Import Namespace="W88.BusinessLogic.Shared.Helpers" %>
<script runat="server">
    protected string Language = string.Empty;
    protected const string Path = "contents/messages";
    protected void Page_Load(object obj, EventArgs e)
    {
        Language = LanguageHelpers.SelectedLanguage;
    }
</script>
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
                    title: "<%=CultureHelpers.GetTranslation("Enhancement_en", Language, Path)%>",
                    message: "<%=CultureHelpers.GetTranslation("Enhancement_Message_en", Language, Path)%>"
                },
                cn: {
                    title: "<%=CultureHelpers.GetTranslation("Enhancement_cn", Language, Path)%>",
                    message: "<%=CultureHelpers.GetTranslation("Enhancement_Message_cn", Language, Path)%>"
                },
                id: {
                    title: "<%=CultureHelpers.GetTranslation("Enhancement_id", Language, Path)%>",
                    message: "<%=CultureHelpers.GetTranslation("Enhancement_Message_id", Language, Path)%>"
                },
                jp: {
                    title: "<%=CultureHelpers.GetTranslation("Enhancement_jp", Language, Path)%>",
                    message: "<%=CultureHelpers.GetTranslation("Enhancement_Message_jp", Language, Path)%>"
                },
                kh: {
                    title: "<%=CultureHelpers.GetTranslation("Enhancement_kh", Language, Path)%>",
                    message: "<%=CultureHelpers.GetTranslation("Enhancement_Message_kh", Language, Path)%>"
                },
                kr: {
                    title: "<%=CultureHelpers.GetTranslation("Enhancement_kr", Language, Path)%>",
                    message: "<%=CultureHelpers.GetTranslation("Enhancement_Message_kr", Language, Path)%>"
                },
                th: {
                    title: "<%=CultureHelpers.GetTranslation("Enhancement_th", Language, Path)%>",
                    message: "<%=CultureHelpers.GetTranslation("Enhancement_Message_th", Language, Path)%>"
                },
                vn: {
                    title: "<%=CultureHelpers.GetTranslation("Enhancement_vn", Language, Path)%>",
                    message: "<%=CultureHelpers.GetTranslation("Enhancement_Message_vn", Language, Path)%>"
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
