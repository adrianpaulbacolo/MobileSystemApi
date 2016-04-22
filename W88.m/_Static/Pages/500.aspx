<!DOCTYPE html>
<html lang="en">
<head>
<meta>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>Page Not Found</title>
    <link rel="shortcut icon" href="/favicon.ico" type="image/x-icon">
    <link href="/_Static/Css/style.css" rel="stylesheet" type="text/css">

    <style type="text/css">
        /* -- Din Pro */
        @font-face {
            font-family: 'dinpro';
            src: url('/_static/fonts/din-pro-reg/din-regular-webfont.eot');
            src: url('/_static/fonts/din-pro-reg/din-regular-webfont.eot?#iefix') format('embedded-opentype'),
                 url('/_static/fonts/din-pro-reg/din-regular-webfont.woff') format('woff'),
                 url('/_static/fonts/din-pro-reg/din-regular-webfont.ttf') format('truetype'),
                 url('/_static/fonts/din-pro-reg/din-regular-webfont.svg#dinregular') format('svg');
            font-weight: normal;
            font-style: normal;
        }
        ::-moz-selection {
            color: #fff;
            text-shadow: none;
            background: #FBC02D;
        }
        ::selection {
            color: #fff;
            text-shadow: none;
            background: #FBC02D;
        }
        :-ms-input-placeholder {color:#666;}
        ::-moz-placeholder {color:#666;opacity:1}
        ::-webkit-input-placeholder {color:#666;}

        * {
            line-height: 1.2;
            margin: 0;
            -webkit-box-sizing: border-box;
            -moz-box-sizing: border-box;
            box-sizing: border-box;
        }
        html {
            font: 16px "Helvetica Neue", Helvetica, Arial, sans-serif;
            text-align: center;
            color: #848080;
            display: table;
            width: 100%;
            height: 100%;
        }
        body {
            display: table-cell;
            vertical-align: middle;
            margin: 2em auto;
            color: #848080;
            background: #000 url(_Static/Images/errors/404-bg.jpg) center 35% no-repeat;
        }
        h1, h2, h3, h4, h5, h6, button {
            font-family: 'dinpro', "Helvetica Neue", Helvetica, Arial, sans-serif;
            font-weight: 200;
            color: #ffffff;
        }
        h1 {
            font-size: 6.25rem; /* 100px */
        }
        h3 {
            font-size: 1.5rem; /* 25px */
            color: rgba(255,255,255,.6);
        }
        a {
            color: #2a8fbd;;
            text-decoration: none;
            -webkit-transition: .3s ease;
            -moz-transition: .3s ease;
            transition: .3s ease;
        }
        p {
            font-size: .9375rem; /* 15px */
            margin: 0 auto;
            width: 300px;
        }
        .logo {
            width: 100%;
            max-width: 398px;
            margin-bottom: 3.125rem; // 50px
        }
        .footer {
            margin: 5em auto 1em;
            padding-top: 1em;
            border-top: 1px solid rgba(255,255,255,.1);
            max-width: 500px;
            font-size: 85%;
            color: rgba(255,255,255,.2)
        }
        .footer ul {
            padding: 0;
            margin: 0;
            list-style: none;
            text-align: center;
        }
        .footer li {
            display: inline-block;
            padding: 0 5px;
        }
        .footer a {
            color: rgba(255,255,255,.2);
        }
        .footer a:hover {
            color: rgba(255,255,255,.5);
        }
        .footer span {
            margin: auto 1rem;
        }

        /* Extra large screen / wide desktop */
        @media (max-width: 1200px) {
            .logo {
                margin-bottom: 1.25rem; // 20px
            }
        }

        /* Large screen / desktop */
        @media (max-width: 992px) {
        }

        /* Medium screen / tablet */
        @media (max-width: 768px) {
            body {
                vertical-align: middle;
                background-size: 200%;
            }
            .footer {
                margin: 1rem auto;
            }
        }

        /* Small screen / phone */
        @media (max-width: 544px) {
            body, p, .footer {
                width: 95%;
            }
            h1 {
                font-size: 4.6875rem; /* 75px */
            }
            .logo {
                width: 50%;
                margin: 0 auto;
                display: block;
            }
            .footer li {
                padding: 5px;
                display: block;
            }
            .footer .divider {
                display: none;
            }
        }
    </style>

</head>
<body>

    <section class="container main">
    	<img class="logo" src="/_Static/Images/errors/w-logo.png">
        <h1><%=commonCulture.ElementValues.getResourceXPathString("StatusCode/e500", commonVariables.ErrorsXML)%></h1>
        <h3><%=commonCulture.ElementValues.getResourceXPathString("HttpError/e500title", commonVariables.ErrorsXML)%></h3>
        <p><%=commonCulture.ElementValues.getResourceXPathString("HttpError/e500", commonVariables.ErrorsXML)%></p>
    </section>

    <footer class="container footer">
        <ul>
            <li><a href="https://server.iad.liveperson.net/hc/88942816/?cmd=file&amp;file=visitorWantsToChat&amp;site=88942816&amp;SV!skill=English&amp;leInsId=88942816527642465&amp;skId=1&amp;leEngId=88942816_29aeab82-a5fc-4de7-b801-c6a87c638106&amp;leEngTypeId=8&amp;leEngName=LiveHelp_default&amp;leRepAvState=3&amp;SESSIONVAR!visitor_profile=English" onclick="window.open(&#39;https://server.iad.liveperson.net/hc/88942816/?cmd=file&amp;file=visitorWantsToChat&amp;site=88942816&amp;SV!skill=English&amp;leInsId=88942816527642465&amp;skId=1&amp;leEngId=88942816_29aeab82-a5fc-4de7-b801-c6a87c638106&amp;leEngTypeId=8&amp;leEngName=LiveHelp_default&amp;leRepAvState=3&amp;SESSIONVAR!visitor_profile=English&#39;, &#39;popup&#39;, &#39;width=517,height=465&#39;); return false">Contact Support</a></li>
            <li class="divider"> | </li>
            <li>© 2015 <a href="https://www.w88live.com/">W88</a>. All rights reserved.</li>
        </ul>
    </footer>

</body>
</html>
