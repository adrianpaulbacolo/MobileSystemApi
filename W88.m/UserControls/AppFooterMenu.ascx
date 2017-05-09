<%@ Control Language="C#" ClassName="AppFooterMenu" %>

<script type="text/javascript" src="/_Static/JS/modules/hideMenu.js"></script>

<script runat="server">
   
</script>

<%--<div id="appMenu">
    <div class="row">
        <div class="col" id="DepositFooterLink">
            <input type="button" data-theme="b" onclick="location.href = '/Deposit/Default_app.aspx';" value="<%=commonCulture.ElementValues.getResourceString("deposit", commonVariables.LeftMenuXML)%>" class="button-blue" data-corners="false" />
        </div>
        <div class="col" id="WidrawFooterLink">
            <input type="button" data-theme="b" onclick="location.href = '/Withdrawal/Default_app.aspx';" value="<%=commonCulture.ElementValues.getResourceString("withrawal", commonVariables.LeftMenuXML)%>" class="button-blue" data-corners="false" />
        </div>
        <div class="col" id="FundsFooterLink">
            <input type="button" data-theme="b" onclick="location.href = '/FundTransfer/Default_app.aspx';" value="<%=commonCulture.ElementValues.getResourceString("fundTransfer", commonVariables.LeftMenuXML)%>" class="button-blue" data-corners="false" />
        </div>
        <div class="col" id="HistoryFooterLink">
            <input type="button" data-theme="b" onclick="location.href = '/History/Default.aspx';" value="<%=commonCulture.ElementValues.getResourceString("history", commonVariables.HistoryXML)%>" class="button-blue" data-corners="false" />
        </div>
    </div>
</div>--%>

<script type="text/javascript">
    <% if (!commonFunctions.isExternalPlatform())
       { %>
        $(document).ready(function () {
            window.w88Mobile.HideMenu.CheckIfApp();
        });
    <% } %>
</script>
