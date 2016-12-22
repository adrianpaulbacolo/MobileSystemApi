<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RebateWeeklyClaim.aspx.cs" Inherits="Profile_RebateWeeklyClaim" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <!--#include virtual="~/_static/head.inc" -->
  
    <script type="text/javascript" src="/_Static/JS/modules/rebates.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table>
                <tr>
                    <td></td>
                    <td colspan="4">
                        <div class="title white">
                            <% = Server.HtmlDecode((Request.QueryString["code"].Split(',').Length!=Request.QueryString["product"].Split(',').Length ||(Request.QueryString["code"].Split(',').Distinct().Count()==1))?Request.QueryString["code"].Split(',')[0]:"") %>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td colspan="4">
                        <asp:Label ID="lblMemberCode" runat="server" Text="" />
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td colspan="4">
                        <asp:Label ID="lblOption" runat="server" Text="" />
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td class="left" colspan="3">
                        <%
                            if (!string.IsNullOrEmpty(Request.QueryString["product"]))
                            {
                                var code = check_promo_code(Request.QueryString["code"]); //window.w88Mobile.Rebates.CheckCode(Request.QueryString["code"]);
                                code = code + ",";
                                for (int i = 0; i < Request.QueryString["product"].Split(',').Count(); i++)
                                {%>
                        <input type="<%=(Request.QueryString["code"].Split(',').Distinct().Count()==1?"radio":"checkbox") %>" 
                            name="comment" 
                            class="required" <%=(code.IndexOf(Request.QueryString["code"].Split(',')[i])>=0)?"disabled":"" %>
                            value="<%=(Request.QueryString["code"].Split(',').Length!=Request.QueryString["product"].Split(',').Length)?Request.QueryString["product"].Split(',')[i]:Request.QueryString["code"].Split(',')[i]+"|"+Request.QueryString["product"].Split(',')[i] %>" 
                            id="Radio1<%=i %>" />
                        <label for="Radio1<%=i %>">
                            <% = 
                            ((Request.QueryString["code"].Split(',').Length==Request.QueryString["product"].Split(',').Length && !(Request.QueryString["code"].Split(',').Distinct().Count()==1))?(" - " + Request.QueryString["code"].Split(',')[i]):"") %>
                        </label><br />
                        <%}
}
                        %>
                    </td>
                    <td style="width: 20px;">&nbsp;</td>
                </tr>
                <tr>
                    <td></td>
                    <td colspan="3">
                        <input type="submit" style="position: absolute; left: -9999px; width: 1px; height: 1px;" />
                        <a id="claim" href="#" onclick="$('#form1').submit(); return false;" class="button"></a>
                        <%--<a href="javascript:window.parent.closePromoModal();"></a>--%>
                    </td>
                    <td>&nbsp;</td>
                </tr>
            </table>
        </div>
    </form>

    <script>
        $(document).ready(function () {
            $("#claim").html(sessionStorage.getItem("weeklyClaim"));
            $("#lblOption").html(sessionStorage.getItem("promoOption"));
        });
    </script>


</body>
</html>
