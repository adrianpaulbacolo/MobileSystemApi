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
<body class="enhancement-all">

    <header>
        <a href="/"><img class="logo" src="/_Static/Images/errors/w88-logo.png"></a> 
        <!--<a href="/"><img class="logo" src="/_Static/Images/errors/w88-logo-light.png"></a> -->
    </header>
    
    <section>
        <div class="enhancement-container">
            <div class="enhancement-content-all">
                <div id="msg-en" class="enhancement-lang">
                    <h3></h3>
                    <p></p>
                </div>
                <div id="msg-cn" class="enhancement-lang">
                    <h3></h3> 
                    <p></p>
                </div>
                <div id="msg-id" class="enhancement-lang">
                    <h3></h3>
                    <p></p>
                </div>
                <div id="msg-jp" class="enhancement-lang">
                    <h3></h3>
                    <p></p>
                </div>
                <div id="msg-kh" class="enhancement-lang">
                    <h3></h3>
                    <p></p>
                </div>
                <div id="msg-kr" class="enhancement-lang">
                    <h3></h3>
                    <p></p>
                </div>
                <div id="msg-th" class="enhancement-lang">
                    <h3></h3>
                    <p></p>
                </div>
                <div id="msg-vn" class="enhancement-lang">
                    <h3></h3>
                    <p></p>
                </div>
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

        function loadTranslation()
        {
            $("#msg-en h3").html(_w88_contents.translate("Enhancement_en"));
            $("#msg-cn h3").html(_w88_contents.translate("Enhancement_cn"));
            $("#msg-id h3").html(_w88_contents.translate("Enhancement_id"));
            $("#msg-kh h3").html(_w88_contents.translate("Enhancement_kh"));
            $("#msg-kr h3").html(_w88_contents.translate("Enhancement_kr"));
            $("#msg-jp h3").html(_w88_contents.translate("Enhancement_jp"));
            $("#msg-th h3").html(_w88_contents.translate("Enhancement_th"));
            $("#msg-vn h3").html(_w88_contents.translate("Enhancement_vn"));

            $("#msg-en p").html(_w88_contents.translate("Enhancement_Message_en"));
            $("#msg-cn p").html(_w88_contents.translate("Enhancement_Message_cn"));
            $("#msg-id p").html(_w88_contents.translate("Enhancement_Message_id"));
            $("#msg-kh p").html(_w88_contents.translate("Enhancement_Message_kh"));
            $("#msg-kr p").html(_w88_contents.translate("Enhancement_Message_kr"));
            $("#msg-jp p").html(_w88_contents.translate("Enhancement_Message_jp"));
            $("#msg-th p").html(_w88_contents.translate("Enhancement_Message_th"));
            $("#msg-vn p").html(_w88_contents.translate("Enhancement_Message_vn"));
        }
    </script>
</body>
</html>
