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
                    margin-top:10px;
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
                                    <select name="selectdayfrom" id="selectdayfrom">
                                        <%
                                            for (int i = 1; i <= 31; i++)
                                            {%>
                                        <option <%= (i== 1)?"selected='selected'":"" %> value="<%=i %>"><%=i %></option>
                                        <% } %>
                                    </select>
                                    <select name="selectmonthfrom" id="selectmonthfrom">
                                        <% int month = DateTime.Now.Month;
                                           for (int i = 1; i <= 12; i++)
                                           {%>
                                        <option <%= (i== month)?"selected='selected'":"" %> value="<%=i %>"><%=i %></option>
                                        <% } %>
                                    </select>
                                    <select name="selectyearfrom" id="selectyearfrom">
                                        <% int year = DateTime.Now.Year;
                                           for (int i = year; i >= 2013; i--)
                                           {%>
                                        <option <%= (i== year)?"selected='selected'":"" %> value="<%=i %>"><%=i %></option>
                                        <% } %>
                                    </select>
                                </div>
                            </div>

                        </div>
                        <div class="ui-field-contain ui-hide-label">
                            <div class="ui-grid-a">
                                <div class="ui-block-a">
                                    <asp:Label ID="Label1" CssClass="labelleft" runat="server" Text="Date From: " data-mini="true" />
                                </div>
                                <div class="ui-block-b">
                                    <select name="selectdayto" id="selectdayto">
                                        <% DateTime today = DateTime.Now;
                                           int day = DateTime.DaysInMonth(today.Year, today.Month);
                                           for (int i = 1; i <= 31; i++)
                                           {%>
                                        <option <%= (i== day)?"selected='selected'":"" %> value="<%=i %>"><%=i %></option>
                                        <% } %>
                                    </select>
                                    <select name="selectmonthto" id="selectmonthto">
                                        <% 
                                            for (int i = 1; i <= 12; i++)
                                            {%>
                                        <option <%= (i== month)?"selected='selected'":"" %> value="<%=i %>"><%=i %></option>
                                        <% } %>
                                    </select>
                                    <select name="selectyearto" id="selectyearto">
                                        <%
                                            for (int i = year; i >= 2013; i--)
                                            {%>
                                        <option <%= (i== year)?"selected='selected'":"" %> value="<%=i %>"><%=i %></option>
                                        <% } %>
                                    </select>
                                </div>
                            </div>
                        </div>
                        <div class="ui-field-contain ui-hide-label">
                            <asp:Button ID="btnSubmit" runat="server" Text="Search" CssClass="button-blue" data-corners="false" OnClientClick="validate();" OnClick="btnSubmit_Click" />
                        </div>

                        <div id="resultpanel" runat="server">
                    </div>
                        <asp:HiddenField ID="hidDateFrom" runat="server" />
                        <asp:HiddenField ID="hidDateTo" runat="server" />
                        <asp:HiddenField ID="hidError" runat="server" />
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
                        case 'wronginput':
                            alert('Invalid Date Format. Please correct and search again.');
                            $('#btnSubmit').attr("disabled", true);
                            break;
                        case 'wrongdate':
                            alert('Invalid Day, Month, or Year range detected. Please correct and search again.');
                            $('#btnSubmit').attr("disabled", true);
                            break;
                        case 'nodata':
                            alert('No record found.');
                            $('#btnSubmit').attr("disabled", true);
                            break;
                        default:
                            break;
                    }
                }
            });

            $('#form1').submit(function (e) {
                validate();
            });


            function validate() {
                $('#btnSubmit').attr("disabled", true);
                var df = document.getElementById("selectdayfrom");
                var dayfrom = df.options[df.selectedIndex].value;
                var mf = document.getElementById("selectmonthfrom");
                var monthfrom = mf.options[mf.selectedIndex].value;
                var yf = document.getElementById("selectyearfrom");
                var yearfrom = yf.options[yf.selectedIndex].value;
                var datefrom = ('0' + dayfrom).slice(-2) + '/' + ('0' + monthfrom).slice(-2) + '/' + yearfrom;

                var dt = document.getElementById("selectdayto");
                var dayto = dt.options[dt.selectedIndex].value;
                var mt = document.getElementById("selectmonthto");
                var monthto = mt.options[mt.selectedIndex].value;
                var yt = document.getElementById("selectyearto");
                var yearto = yt.options[yt.selectedIndex].value;
                var dateto = ('0' + dayto).slice(-2) + '/' + ('0' + monthto).slice(-2) + '/' + yearto;

                if (checkdate(datefrom) == false) {
                    $('#btnSubmit').attr("disabled", false);
                    SetHdnField('<%= hidDateFrom.ClientID %>', '');
                    SetHdnField('<%= hidDateTo.ClientID %>', '');
                    return false;
                } else {
                    if (checkdate(dateto) == false) {
                        $('#btnSubmit').attr("disabled", false);
                        SetHdnField('<%= hidDateFrom.ClientID %>', '');
                        SetHdnField('<%= hidDateTo.ClientID %>', '');
                        return false;
                    } else {
                        SetHdnField('<%= hidDateFrom.ClientID %>', datefrom);
                        SetHdnField('<%= hidDateTo.ClientID %>', dateto);
                        return true;
                    }
                }
            }

            function SetHdnField(clientid, val) {
                var x = document.getElementById(clientid);
                x.value = val;
                return false;
            }

            function checkdate(input) {
                var validformat = /^\d{2}\/\d{2}\/\d{4}$/ //Basic check for format validity

                if (!validformat.test(input)) {
                    SetHdnField('<%= hidError.ClientID %>', 'wronginput');
                    return false;
                }
                else { //Detailed check for valid date ranges
                    var dayfield = input.split("/")[0]
                    var monthfield = input.split("/")[1]
                    var yearfield = input.split("/")[2]

                    var dayobj = new Date(yearfield, monthfield - 1, dayfield)
                    if ((dayobj.getMonth() + 1 != monthfield) || (dayobj.getDate() != dayfield) || (dayobj.getFullYear() != yearfield)) {
                        SetHdnField('<%= hidError.ClientID %>', 'wrongdate');
                        return false;
                    }
                    else {
                        SetHdnField('<%= hidError.ClientID %>', 'valid');
                        return true;
                    }
                }
            }

        </script>

    </div>
    <!-- /page -->
</body>
</html>
