<!DOCTYPE html>
<html lang="en">
<head>
<meta>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title><%=commonCulture.ElementValues.getResourceXPathString("HttpError/e404title", commonVariables.ErrorsXML)%></title>
    <link rel="shortcut icon" href="/favicon.ico" type="image/x-icon">
    <link href="/_Static/Css/errors/styles.css" rel="stylesheet" type="text/css">

</head>
<body>

    <header>
    	<a href="/"><img class="logo" src="/_Static/Images/errors/w88-logo.png"></a>
    </header>

    <section>
        <div class="error-container">
            <div class="error-content">
                <h1><%=commonCulture.ElementValues.getResourceXPathString("StatusCode/e404", commonVariables.ErrorsXML)%></h1>
                <h3><%=commonCulture.ElementValues.getResourceXPathString("HttpError/e404title", commonVariables.ErrorsXML)%></h3>
                <p><%=commonCulture.ElementValues.getResourceXPathString("HttpError/e404", commonVariables.ErrorsXML)%></p>
                <ul class="links links-error">
                    <li>
                        <a href="/Index.aspx">
                            <span class="icon icon-home"></span><br>
                            <span class="link-text"><%=commonCulture.ElementValues.getResourceString("home", commonVariables.LeftMenuXML)%></span> 
                             
                        </a>
                    </li>
                    <li>
                        <a href="https://server.iad.liveperson.net/hc/88942816/?cmd=file&amp;file=visitorWantsToChat&amp;site=88942816&amp;SV!skill=English&amp;leInsId=88942816527642465&amp;skId=1&amp;leEngId=88942816_29aeab82-a5fc-4de7-b801-c6a87c638106&amp;leEngTypeId=8&amp;leEngName=LiveHelp_default&amp;leRepAvState=3&amp;SESSIONVAR!visitor_profile=English" onclick="window.open(&#39;https://server.iad.liveperson.net/hc/88942816/?cmd=file&amp;file=visitorWantsToChat&amp;site=88942816&amp;SV!skill=English&amp;leInsId=88942816527642465&amp;skId=1&amp;leEngId=88942816_29aeab82-a5fc-4de7-b801-c6a87c638106&amp;leEngTypeId=8&amp;leEngName=LiveHelp_default&amp;leRepAvState=3&amp;SESSIONVAR!visitor_profile=English&#39;, &#39;popup&#39;, &#39;width=517,height=465&#39;); return false">
                            <span class="icon icon-chat"></span><br>
                            <span class="link-text">
                            <%=commonCulture.ElementValues.getResourceString("helpCenter", commonVariables.LeftMenuXML)%></span>
                        </a>
                    </li>
                </ul>
            </div>
        </div>
    </section>

    <footer>
        <img src="/_Static/Images/errors/gpi-logo.png" alt="">
        <p><small>&copy; 2017 <a href="/">W88</a>. All rights reserved.</small></p>
    </footer>

</body>
</html>
