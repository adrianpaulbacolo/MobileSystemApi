<!DOCTYPE html>
<%@ Import Namespace="W88.BusinessLogic.Shared.Helpers" %>
<html lang="en">
<head>
<meta>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>Enhancement</title>
    <link rel="shortcut icon" href="/favicon.ico" type="image/x-icon">
    <link href="/_Static/Css/errors/styles.css" rel="stylesheet" type="text/css">

</head>
<body class="enhancement">

    <header>
        <a href="/"><img class="logo" src="/_Static/Images/logo-<%=LanguageHelpers.SelectedLanguageShort%>.png"></a>
    </header>
    
    <section>
        <div class="enhancement-container">
            <div class="enhancement-content">
                <div class="enhancement-header">
                    <span class="icon icon-settings"></span>
                    <h1>Enhancement</h1>
                    <p>We will be back shortly after this scheduled enhancement. If you need assistance, please contact our Customer Service via <a href="/LiveChat/Default.aspx" target="_blank" class="text-primary">Live Chat</a> or <span class="text-primary"><a href="mailto:info@w88.com" class="text-primary">info@w88.com</a></span>.</p>
                </div>
                <ul class="links">
                    <li style="width: 100%;">
                        <a href="/">
                            <span class="icon icon-home"></span><br>
                            <span class="link-text">Home</span>
                        </a>
                    </li>
                </ul>
            </div>
        </div>
    </section>

    <footer>
        <img src="/_Static/Images/errors/gpi-logo.png" alt="">
        <p><small><%=DateTime.Now.Year%> <a href="/">W88</a>. All rights reserved.</small></p>
    </footer>

</body>
</html>
