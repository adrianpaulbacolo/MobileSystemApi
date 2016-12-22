<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Points.aspx.cs" Inherits="Points" Async="true"%>

<!DOCTYPE html>
<html>
<head>
    <title>Account Summary</title>
    <!--#include virtual="~/_static/head.inc" -->
    <link rel="stylesheet" href="/_Static/Css/Mobile/jquery.mobile.datepicker.css" />
    <script src="/_Static/JS/Mobile/jquery.ui.datepicker.js"></script>
    <script src="/_Static/JS/Mobile/jquery.mobile.datepicker.js"></script>
</head>
<body>
    <div data-role="page" data-theme="b">
        <!--#include virtual="~/_static/header.shtml" -->
        <form id="form1" runat="server">
            <div class="main-content static-content has-footer ui-content" role="main">
                <div class="container">
                    <h6><%= Title %></h6>
                    <div id="resultpanel" runat="server"></div>
                    <div class="datePicker">
                        <h6 id="lbdatefrom" runat="server"></h6>
                        <input id="dateFrom" type="text" data-role="date" data-inline="true" runat="server" />
                    </div>
                    <div class="spacing15"></div> 
                    <div class="datePicker">
                        <h6 id="lbdateto" runat="server"></h6>
                        <input id="dateTo" type="text" data-role="date" data-inline="true" runat="server" />
                    </div>
                </div>
            </div>
            <div class="footer">
                <asp:Button ID="submit" runat="server" Text="" CssClass="btn btn-block btn-primary" data-corners="false" OnClick="btnSubmit_Click" />
            </div>
        </form>
        <!-- /content -->
    </div>
    <script type="text/javascript">
        $(function () {
            $('#ui-datepicker-div').css({ 'background': '#01405a', 'padding': '5px 4px' });
        });

        function showMessage(status) {
            switch (status) {
                case 'wrongfromdate':
                    window.w88Mobile.Growl.shout('Invalid Date From. Please correct and search again.');
                    break;
                case 'wrongtodate':
                    window.w88Mobile.Growl.shout('Invalid Date To. Please correct and search again.');
                    break;
                case 'wrongdaterange':
                    window.w88Mobile.Growl.shout('Invalid date range. Date To cannot be earlier than Date From.');
                    break;
                case 'nodata':
                    window.w88Mobile.Growl.shout('<%=RewardsHelper.GetTranslation(TranslationKeys.Redemption.NoRecordFound)%>');
                    break;
                case 'FAIL':
                    window.w88Mobile.Growl.shout('<%=RewardsHelper.GetTranslation(TranslationKeys.Errors.Exception)%>');
                    break;
            }
        }
    </script>
    <!-- /page -->
</body>
</html>
