<!DOCTYPE html>
<html lang="en">
<head>
    <meta>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>Enhancement</title>
    <link rel="shortcut icon" href="/favicon.ico" type="image/x-icon">
    <!-- <link href="/_Static/Css/errors/styles.css" rel="stylesheet" type="text/css"> -->
    <link href="/_Static/Css/errors/styles.css" rel="stylesheet" type="text/css">
</head>
<body class="enhancement">

    <header>
        <a href="/">
            <img class="logo" src="/_Static/Images/errors/w88-logo.png"></a>
        <!--<a href="/"><img class="logo" src="/_Static/Images/errors/w88-logo-light.png"></a> -->
    </header>

    <section>
        <div class="enhancement-container">
            <div class="enhancement-content">
                <div id="msg" class="enhancement-header">
                    <span class="icon icon-settings"></span>
                    <h1></h1>
                    <p></p>
                </div>
                <ul class="links">
                    <li>
                        <a href="/Index.aspx">
                            <span class="icon icon-home"></span>
                            <br>
                            <span class="link-text">Home</span>
                        </a>
                    </li>
                    <li>
                        <a href="/Sports.aspx">
                            <span class="icon icon-soccer"></span>
                            <br>
                            <span class="link-text">Sports</span>
                        </a>
                    </li>
                    <li>
                        <a href="/Casino.aspx">
                            <span class="icon icon-casino"></span>
                            <br>
                            <span class="link-text">Live Casino</span>
                        </a>
                    </li>
                    <li>
                        <a href="/Slots.aspx">
                            <span class="icon icon-slots"></span>
                            <br>
                            <span class="link-text">Slots</span>
                        </a>
                    </li>
                    <li>
                        <a href="/Index.aspx">
                            <span class="icon icon-games"></span>
                            <br>
                            <span class="link-text">Games</span>
                        </a>
                    </li>
                    <li>
                        <a href="/Lottery.aspx">
                            <span class="icon icon-keno"></span>
                            <br>
                            <span class="link-text">Lottery</span>
                        </a>
                    </li>
                    <li>
                        <a href="/Index.aspx">
                            <span class="icon icon-spade"></span>
                            <br>
                            <span class="link-text">Poker</span>
                        </a>
                    </li>
                    <li>
                        <a href="/Index.aspx">
                            <span class="icon icon-mahjong"></span>
                            <br>
                            <span class="link-text">Texas Mahjong</span>
                        </a>
                    </li>
                </ul>
            </div>
        </div>
    </section>

    <footer>
        <img src="/_Static/Images/errors/gpi-logo.png" alt="">
        <p><small></small></p>
    </footer>


    <script type="text/javascript">
        window.w88Mobile = {};
        w88Mobile.Gateways = {};
        window.User = {};
        window.User.token = '<%= commonVariables.CurrentMemberSessionId %>';
        window.User.lang = '<%=commonVariables.SelectedLanguage%>';
        window.User.storageExpiration = { expires: 300000 };
        w88Mobile.APIUrl = '<%= ConfigurationManager.AppSettings.Get("APIUrl") %>';
    </script>

   <script type="text/javascript" src="/_Static/JS/Mobile/jquery-1.11.3.min.js"></script>
    <script type="text/javascript" src="/_Static/JS/vendor/lodash.min.js"></script>
    <script src="/_Static/JS/vendor/pubsub.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
    <script src="/_Static/JS/vendor/amplify.min.js"></script>
    <script type="text/javascript" src="/_Static/JS/i18n/contents-<%=commonVariables.SelectedLanguageShort%>.js"></script>
    <script src="/_Static/JS/modules/translate.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>

    <script>
       
        $(document).ready(function () {
            loadTranslation();
            pubsub.subscribe('contentsLoaded', onContentsLoaded);

            function onContentsLoaded() {
                loadTranslation();
            }

            var year = new Date().getFullYear();
            $("footer p small").html("&copy; " + year + " <a href='/'>W88</a>. All rights reserved.");
        });

        function loadTranslation() {
            var shortLang = '<%=commonVariables.SelectedLanguageShort%>';
            $("#msg h1").html(_w88_contents.translate("Enhancement"));
            $("#msg p").html(_w88_contents.translate("Enhancement_Message_" + shortLang));
        }
    </script>
</body>
</html>
