<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ClubPalazzoLauncher.aspx.cs" Inherits="Slots_ClubPalazzo" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title><%=commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML) + commonCulture.ElementValues.getResourceXPathString("Products/ClubPalazzoSlots/Label", commonVariables.ProductsXML)%></title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="https://login.goldenphoenix88.com/jswrapper/integration.js.php?casino=blacktiger88"></script>

    <script type="text/javascript">     

        function launchMobileClient(temptoken) {

            if (gametype = "ngm") {
                var clientUrl = <%=link%> + '&lobby=' + location.href.substring(0, location.href.lastIndexOf('.com') + 4) + '/ClubPalazzo' + '&support=' + location.href.substring(0, location.href.lastIndexOf('.com') + 4) + '/LiveChat/Default.aspx' + '&logout=' + location.href.substring(0, location.href.lastIndexOf('.com') + 4) + '/Logout';
                console.log(clientUrl);
                document.location = clientUrl;
            }

        }


        //CALLOUT----------------------------------------------

        function calloutLogin(response) {
            if (response.errorCode) {
                alert("Login failed. " + response.playerMessage + " Error code: " + response.errorCode);
            }
            else {
                <%=javascriptToken%>
                iapiSetCallout('GetTemporaryAuthenticationToken', calloutGetTemporaryAuthenticationToken);
            }
        }

        function calloutGetTemporaryAuthenticationToken(response) {
            if (response.errorCode) {
                alert("Token failed. " + response.playerMessage + " Error code: " + response.errorCode);
            }
            else {
                launchMobileClient(response.sessionToken.sessionToken);
            }
        }

        $(function () {
            iapiSetClientPlatform("mobile&deliveryPlatform=HTML5");
            <%=javascriptLogin%>
            iapiSetCallout('Login',calloutLogin);
            


        });
    </script>
</head>
<body>
</body>
</html>

