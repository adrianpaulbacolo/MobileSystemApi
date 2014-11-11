<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Points.aspx.cs" Inherits="Points" %>

<!DOCTYPE html>
<html>
<head>
    <title>Account Summary</title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/Js/Main.js"></script>
</head>
<body>
    <!--#include virtual="~/_static/splash.shtml" -->
    <div data-role="page" data-theme="b" data-ajax="false">
        <!--#include virtual="~/_static/header.shtml" -->
        <div class="ui-content" role="main">
            <div class="div-page-header"><span><%= title %></span></div>
            <div class="page-content">
                <style>
                    table td div.pointDetailMainHeader {
                        background: none repeat scroll 0 0 #404040;
                        display: block;
                        font-weight: bold !important;
                        line-height: 40px;
                        position: relative;
                        text-align: left;
                        padding: 0 5pt;
                        font-size: 10pt;
                    }

                    table td div.pointDetailMainHeaderHor {
                        background: none repeat scroll 0 0 #404040;
                        display: block;
                        font-size: 10pt;
                        font-weight: bold !important;
                        min-height: 35px;
                        padding: 4pt 5pt;
                        position: relative;
                        text-align: left;
                    }

                    table td div.pointsLink {
                        background: none repeat scroll 0 0 #333;
                        display: block;
                        line-height: 40px;
                        padding: 0 5pt;
                        font-size: 9pt;
                    }

                    table td div.points {
                        background: none repeat scroll 0 0 #333;
                        display: block;
                        font-size: 8pt;
                        height: 30pt;
                        line-height: 20px;
                        padding: 0 3pt;
                    }


                    .button-blue {
                        background-color: #2a8fbd !important;
                        border: medium none;
                        color: #fff;
                        font-family: "Open Sans",sans-serif,helvetica,Tahoma,Arial,Verdana,"Comic Sans MS";
                        font-size: 1em !important;
                        font-weight: 700;
                        opacity: 1 !important;
                        text-indent: 0 !important;
                        text-shadow: none !important;
                    }

                    .ui-field-contain {
                        border-bottom: 0px none rgba(0, 0, 0, 0.15);
                        font-size: 9pt;
                        color: #808080;
                    }

                    .ui-mini .ui-input-text input, .ui-mini .ui-input-search input, .ui-input-text.ui-mini input, .ui-input-search.ui-mini input, .ui-mini textarea.ui-input-text, textarea.ui-mini {
                        font-size: 9pt;
                        color: white;
                    }

                    .ui-input-text, .ui-input-search {
                        border-style: solid;
                        border-width: 1px;
                        float: left;
                        margin: 0 1px 2px;
                        width: 96%;
                    }

                    .validator {
                        color: red;
                        text-align: left;
                    }

                    .errormessage {
                        color: red;
                    }

                    .ui-field-contain textarea {
                        background: none repeat scroll 0 0 transparent;
                        border: 0 none;
                        border-radius: inherit;
                        margin: 0;
                        min-height: 2.2em;
                        text-align: left;
                    }

                    .ui-mini textarea.ui-input-text, textarea.ui-mini {
                        margin: 0px;
                    }

                    .ui-select {
                        float: left;
                        margin-bottom: 0.5em;
                        margin-top: 0.5em;
                        position: relative;
                    }

                    .ui-grid-a > .ui-block-a {
                        width: 15%;
                    }

                    .ui-grid-a > .ui-block-b {
                        width: 85%;
                    }

                    .labelleft {
                        font-size: 11pt;
                        margin-top: 10px;
                        position: relative;
                        top: 5pt;
                    }

                    .ui-select .ui-btn > span:not(.ui-li-count) {
                        display: block;
                        font-size: 9pt;
                        overflow: hidden !important;
                        text-overflow: ellipsis;
                        white-space: nowrap;
                    }

                    #resultpanel {
                        margin-top: 10px;
                    }
                </style>
                <form id="form1" runat="server">
                    <div id="">
                        <div class="ui-field-contain ui-hide-label">
                            <div class="ui-grid-a">
                                <div class="ui-block-a">
                                    <asp:Label ID="lbl1" CssClass="labelleft" runat="server" Text="Date From: " data-mini="true" />
                                </div>
                                <div class="ui-block-b">
                                    <select name="selectdayfrom" id="selectdayfrom" runat="server">
                                    </select>
                                    <select name="selectmonthfrom" id="selectmonthfrom" runat="server">
                                    </select>
                                    <select name="selectyearfrom" id="selectyearfrom" runat="server">
                                    </select>
                                </div>
                            </div>

                        </div>
                        <div class="ui-field-contain ui-hide-label">
                            <div class="ui-grid-a">
                                <div class="ui-block-a">
                                    <asp:Label ID="Label1" CssClass="labelleft" runat="server" Text="Date To: " data-mini="true" />
                                </div>
                                <div class="ui-block-b">
                                    <select name="selectdayto" id="selectdayto" runat="server">
                                    </select>
                                    <select name="selectmonthto" id="selectmonthto" runat="server">
                                    </select>
                                    <select name="selectyearto" id="selectyearto" runat="server">
                                    </select>
                                </div>
                            </div>
                        </div>
                        <div class="ui-field-contain ui-hide-label">
                            <asp:Button ID="btnSubmit" runat="server" Text="Search" CssClass="button-blue" data-corners="false" OnClick="btnSubmit_Click" />
                        </div>


                        <div id="resultpanel" runat="server">
                        </div>
                     
                    </div>


                </form>

            </div>
        </div>
        <!-- /content -->
        <!--#include virtual="~/_static/footer.shtml" -->
        <!--#include virtual="~/_static/navMenu.shtml" -->


        <script type="text/javascript">
            $(function () {
                if ('<%=strAlertCode%>'.length > 0) {
                    switch ('<%=strAlertCode%>') {
                        case 'wrongfromdate':
                            alert('Invalid Date From. Please correct and search again.');
                           // $('#btnSubmit').attr("disabled", true);
                            break;
                        case 'wrongtodate':
                            alert('Invalid Date To. Please correct and search again.');
                            // $('#btnSubmit').attr("disabled", true);
                            break;
                        case 'wrongdaterange':
                            alert('Invalid date range. Date To cannot be earlier than Date From.');
                         //   $('#btnSubmit').attr("disabled", true);                          
                            break;
                        case 'nodata':
                            alert('No record found.');
                          //  $('#btnSubmit').attr("disabled", true);                          
                            break;
                        case 'FAIL':
                            alert("Unexpected system error. Please contact customer service.");
                          //  $('#btnSubmit').attr("disabled", true);
                            break;
                        default:
                          //  $('#btnSubmit').attr("disabled", true);
                            break;
                    }
                }
            });

           
            function SetHdnField(clientid, val) {
                var x = document.getElementById(clientid);
                x.value = val;
                return false;
            }

           
        </script>

    </div>
    <!-- /page -->
</body>
</html>
