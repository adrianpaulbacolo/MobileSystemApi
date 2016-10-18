<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegisterSuccess.aspx.cs" Inherits="_Secure_RegisterSuccess" %>

<!DOCTYPE html>
<html>
<head>
    <title><%=commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML) + commonCulture.ElementValues.getResourceString("register", commonVariables.LeftMenuXML)%></title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/Js/PreLoad.js"></script>
</head>
<body>
    <div data-role="page" data-theme="b" id="register">

        <header id="header" data-role="header" data-position="fixed" data-theme="b" data-tap-toggle="false">
            <a href="" role="button" data-rel="back" class="btn-clear ui-btn-left ui-btn ion-ios-arrow-back" id="aMenu" data-load-ignore-splash="true">
                <%=commonCulture.ElementValues.getResourceString("back", commonVariables.LeftMenuXML)%>
            </a>
            <h1 class="title"><%=commonCulture.ElementValues.getResourceString("register", commonVariables.LeftMenuXML)%></h1>
        </header>

        <div class="ui-content" role="main">
            <div class="register-success-message">
                <div class="register-success-icon"><span class="icon icon-check"></span></div>
                <h4>Welcome to W88, and thank you for opening an account.</h4>
                <p>Your account is now ready for you to login and play on W88.com and mobile <a href="#">m.w88.com</a>.</p>
                <p>Depositing is Quick and Easy. We have a huge range of deposit options available.</p>
            </div>

            <div class="ui-navbar ui-navbar-justified"  role="navigation">
                <ul>
                    <li>
                        <a href="#" class="btn-primary ui-link ui-btn">Bank Transfer</a>
                    </li>
                    <li>
                        <a href="#" class="ui-link ui-btn">Credit Card</a>
                    </li>
                </ul>
            </div>

            <div class="register-success-content">
                <p>Deposit name must be the same as the registered name on your W88 account. Please refer to Terms & Condition.</p>
                <div class="table-container">
                    <table>
                        <tbody>
                            <tr>
                                <td>Method:</td>
                                <td>
                                    <ul>
                                        <li>Automated Teller Machine (ATM)</li>
                                        <li>Online Banking</li>
                                        <li>Over The Counter Service</li>
                                    </ul>
                                </td>
                            </tr>
                            <tr>
                                <td>Transaction Limit (USD) :</td>
                                <td>Min. 10.00<br>
                                Max. 5,000.00</td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    If you have any queries or problems when making a deposit,
                                    please <a href="https://www.w88bet.com/ContactUs.aspx"
                                    target="_blank">Contact Us</a>.
                                </td>
                            </tr>
                        </tbody>
                    </table> 
                </div>               
            </div>

            <div class="bank_logo"> 
                <i class="logo_10"></i>
                <i class="logo_11"></i>
                <i class="logo_12"></i>
                <i class="logo_13"></i>
                <i class="logo_14"></i>
                <i class="logo_15"></i>
            </div>
        </div>

    </div>
    <!-- /page -->
</body>
</html>
