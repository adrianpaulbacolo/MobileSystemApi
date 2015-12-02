<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Report.aspx.cs" Inherits="_Report" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
<script src="/_static/_script/jquery.js"></script>

    <%--<script type="text/javascript" >
        $(document).ready(function () {
          
            $.ajax({
                contentType: "application/json; charset=utf-8",
                url: "https://ip2loc.w2script.com/ip2loc",
                dataType: "jsonp",
                success: function (data) {
                  
                    $("#TextBox1").val(data.country);
 
                    document.getElementById("form1").submit();
                },
                error: function (err) {
                    $("div.progressBar").hide();
                    alert("error");
                }
            });
      

        });

    </script>--%>

        <style>
        .htmlOverflow{
            overflow:auto;
            scrollbar-base-color:black;
            scrollbar-track-color:black;
            scrollbar-face-color: #FF0000;
            scrollbar-shadow-color: #0000FF;
            scrollbar-highlight-color: #00FF00;
            scrollbar-3dlight-color: #FF00FF;
            scrollbar-darkshadow-color: #00FFFF;
            scrollbar-track-color: #FFFF00;
            scrollbar-arrow-color: #000000;
        }
        	.table_form {
			border-collapse: separate;
			border-spacing: 10px;
			vertical-align: top;
		}
		.table_form td {
			vertical-align: top;
		}
		span {
			line-height:15px !important;
		}

    </style>
</head>
<body>
    <%--<form id="form1" runat="server">--%>
     <div id="divMain" data-role="page">
        <!--#include virtual="~/_static/header.shtml" -->
        <div class="ui-content" role="main">

            <div class="div-page-header"><span><%=commonCulture.ElementValues.getResourceString("Report", commonVariables.LeftMenuXML)%></span></div>
                <div class="page-content">
                   <%-- <form id="form1" runat="server" data-ajax="false">--%>
                        <div class="div-content-wrapper">
            <%--<div class="div-page-header" id="divDefaultHeader" runat="server"><span id="lblDefaultHeader" runat="server"></span></div>--%>
            <%--<div id="divContent">--%>
                    <div>       
           <div class="mainContent clear ">
            <div >
                <table class="table_form">
                    <tr>
                        <td style="text-align:left; padding-right:38px">
                            <% =commonCulture.ElementValues.getResourceString("lbl_tracking_code", xeResources).ToString() %>&nbsp;
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlTrackingCode" runat="server" width="300"></asp:DropDownList>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td style="text-align:left; vertical-align:top;padding-right:38px">
                          <% =commonCulture.ElementValues.getResourceString("lbl_period", xeResources).ToString() %>&nbsp;
                        </td>
                        <td colspan="2">
                           <%-- <input type="radio" name="Period" value="1" <% =rb1 %>>&nbsp;<% =commonCulture.ElementValues.getResourceString("lbl_option_current_day", xeResources).ToString() %><br />
                            <input type="radio" name="Period" value="2" <% =rb2 %>>&nbsp;<% =commonCulture.ElementValues.getResourceString("lbl_option_current_week", xeResources).ToString() %><br />
                            <input type="radio" name="Period" value="3" <% =rb3 %>>&nbsp;<% =commonCulture.ElementValues.getResourceString("lbl_option_current_month", xeResources).ToString() %><br />
                            <input type="radio" name="Period" value="4" <% =rb4 %>>&nbsp;<% =commonCulture.ElementValues.getResourceString("lbl_option_from", xeResources).ToString() %>--%>
                            &nbsp;
                            <asp:TextBox ID="txtFrom" runat="server" Width="100px" CssClass="txtDate">&nbsp;</asp:TextBox><% =commonCulture.ElementValues.getResourceString("lbl_option_to", xeResources).ToString() %>&nbsp;<asp:TextBox ID="txtTo" runat="server" Width="100px" CssClass="txtDate"></asp:TextBox>
                        </td>      
                    </tr>
                    <tr>
                        <td style="text-align:left; vertical-align:top;padding-right:38px">
                          <% =commonCulture.ElementValues.getResourceString("lbl_sort_by", xeResources).ToString() %>&nbsp;
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlSortBy" runat="server" Width="100px">
                            </asp:DropDownList>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td><br />  <input type="submit" style="position: absolute; left: -9999px; width: 1px; height: 1px;" />
								<a href="#" class="button_submit center" onclick="$('#form1').submit(); return false;"><% =commonCulture.ElementValues.getResourceString("lbl_btn_submit", xeResources).ToString() %></a>
                        <td></td>
                    </tr>
                </table>
            </div>
            <br /><hr />
            <div class="content_normal">
                <div class="htmlOverflow">
                    <div class="CSSTableGeneratorBlackTrack" >    
                        <asp:Label ID="lblTable" runat="server" Text=""></asp:Label>                      
                    </div>
                </div>
            </div>
                            </div>
         <%-- </form>--%>
         </div>
              </div>
        <!-- /content -->
        <!--#include virtual="~/_static/footer.shtml" -->
        <!--#include virtual="~/_static/navMenu.shtml" -->
    </div>
               </div>
            </div>
    <!-- /page -->
</body>
</html>
