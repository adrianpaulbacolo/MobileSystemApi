<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChangePassword.aspx.cs" Inherits="_Change_Password" %>

<!DOCTYPE html>
<html>
<head>
    <title>Change Password</title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/Js/Main.js"></script>
</head>
<body>
    <!--#include virtual="~/_static/splash.shtml" -->
    <div data-role="page" data-theme="b">

        <!--#include virtual="~/_static/header.shtml" -->

        <div class="ui-content" role="main">
            <form class="form">
                <p>&nbsp;</p>
                <ul class="list fixed-tablet-size">
                    <li class="item item-input">
                        <label for="" id="">Current Password</label>
                        <input name="txtPassword" type="password" maxlength="10" id="txtPassword" data-mini="true" data-clear-btn="true">
                    </li>
                    <li class="item item-input">
                        <label for="" id="">New Password</label>
                        <input name="txtPassword" type="password" maxlength="10" id="txtPassword" data-mini="true" data-clear-btn="true">
                    </li>
                    <li class="item item-input">
                        <label for="" id="">Confirm Password</label>
                        <input name="txtPassword" type="password" maxlength="10" id="txtPassword" data-mini="true" data-clear-btn="true">
                    </li>

                    <li class="item row">
                        <div class="col">
                            <a href="/Index" id="btnCancel" class="ui-btn btn-bordered" text="cancel" data-corners="false" data-ajax="false">Cancel</a>
                        </div>
                        <div class="col">
                            <input type="submit" name="btnSubmit" value="Submit" id="btnSubmit">
                        </div>
                    </li>

                </ul>
            </form>
        </div>

        <!--#include virtual="~/_static/footer.shtml" -->
        <!--#include virtual="~/_static/navMenu.shtml" -->
    </div>
</body>
</html>
