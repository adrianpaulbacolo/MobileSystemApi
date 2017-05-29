<%@ Page Title="" Language="C#" MasterPageFile="~/v2/MasterPages/Main.master" AutoEventWireup="true" CodeFile="Rebates.aspx.cs" Inherits="v2_Account_Rebates" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContentHolder" runat="Server">
    <div class="form-container">
        <div class="container">
            <div class="form-group">
                <label id="labelPeriod" data-i18n="LABEL_REBATE_PERIOD"></label>
                <select id="weeks" class="form-control"></select>
                <p class="text-center"><small><span id="monday" data-i18n="LABEL_MONDAY"></span>(<span id="startdate"></span>) - <span id="sunday" data-i18n="LABEL_SUNDAY"></span>(<span id="endDate"></span>)</small> </p>
            </div>
        </div>
    </div>

    <div id="group"></div>

    <div class="rebates-disclaimer">

        <p><small id="rebateDisclaimer" data-i18n="LABEL_REBATE_DISCLAIMER"></small><br><small id="rebateDisclaimerMin" data-i18n="LABEL_REBATE_NOTE1"></small></p>

        <p class="curr_week">
            <small id="rebateDisclaimerNoteCurrent" data-i18n="LABEL_REBATE_DISCLAIMER_CONTENT_CURRENT"></small>
        </p>

        <p class="prev_week">
            <small id="rebateDisclaimerNote1" data-i18n="LABEL_REBATE_DISCLAIMER_CONTENT1"></small>
            <small id="rebateDisclaimerNote2" data-i18n="LABEL_REBATE_DISCLAIMER_CONTENT2"></small>
        </p>

    </div>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/modules/accounts/rebates.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="InnerScriptPlaceHolder" runat="Server">

    <script>

        function weeklyClaim() {
            window.w88Mobile.Rebates.GetWeeklySettings('<%=Username%>');
        }

        $(document).ready(function () {

            _w88_Rebates.init();

            $("#weeks").change(function () {

                _w88_Rebates.Statement();

            });

        });
    </script>

    <script type="text/template" id='ClaimGroup'>
        <div class="rebates-group">
            {% _.forEach( tplData.data, function( op ){ %}
    {% if((op.NativeGroupKey == "SB")) { %}
    <div class="rebates-row rebates-row-boxed">
        {% } else { %}
    <div class="rebates-row ">
        {% } %}
        
        <h2>{%- op.GroupNameKey %}</h2>
        {% if(op.NativeGroupKey == "SB") { %} 
        <p><small><i data-i18n="LABEL_REBATE_NOTE"></i></small></p>
        {% } %}

        {% _.forEach( op.Collection, function( col, index ){ %}
        <div class="rebate-box">
            <div class="rebate-title">
                <div class="row thin-gutter">
                    <div class="col-xs-6">
                        <h3 title="{%- col.Name %}">{%- col.Name %}</h3>
                    </div>
                    <div class="col-xs-6 text-right">
                        {% if(op.NativeGroupKey != "SB") { %}
                        {% if(col.AllowClaim) { %}
                        <input type="button" name="btnSubmit" value="{%- tplData.BtnClaim %}" id="rebatesClaim" class="btn btn-xs btn-primary" onclick="javascript: _w88_Rebates.ClaimQuery('{%- col.ProductCode %}', '{%- col.AllowClaim %}');">
                        {% } else { %}
                        <input type="button" name="" value="{%- tplData.BtnClaim %}" id="" class="btn btn-xs btn-primary" disabled="disabled">
                        {% } %}
                    {% } %}
                    </div>
                </div>
            </div>
            <div class="rebate-table">
                <table>
                    <tbody>
                        <tr>
                            <td data-i18n="LABEL_REBATE_AMOUNT"></td>
                            <td>{%- col.RebateAmount %}</td>
                        </tr>
                    </tbody>
                </table>
                <div class="collapsible-table">
                    <table>
                        <tbody>
                            <tr>
                                <td data-i18n="LABEL_REBATE_PERCENT"></td>
                                <td>{%- col.RebatePercent %}</td>
                            </tr>
                            <tr>
                                <td data-i18n="LABEL_REBATE_BETS"></td>
                                <td>{%- col.TotalEligibleBet %}</td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
            <a href="#" class="collapsible-btn"><span data-i18n="LABEL_MORE"></span></a>
        </div>
        {% if((op.NativeGroupKey == "SB") && ((index + 1) == op.Collection.length) && (tplData.ShowWeekClaim))  { %}
            <button type="button" class="btn btn-block btn-primary" onclick="javascript: weeklyClaim();">{%- tplData.Btnweekly %}</button>
        {% } %}
        
        {% }); %}
    </div>

        {% }); %}
    </div>
    </script>

    <script type="text/template" id='ClaimMessage'>
        <div class="padding rebates-modal-content">
            <div class="text-center">
                {% if(tplData.data.statusCode == 1) { %}
            <h4 data-i18n="LABEL_CONGRATS"></h4>
                {% } %}

        <p>{%- tplData.data.msg %}</p>
            </div>
        </div>
    </script>

    <script type="text/template" id='ClaimModal'>
        <div class="padding rebates-modal-content">
            <div class="text-center">
                <h4 data-i18n="LABEL_REBATE_SUMMARY"></h4>
                <p><small>{%- tplData.data.Monday %} ({%- tplData.data.StartDate %}) - {%- tplData.data.Sunday %} ({%- tplData.data.EndDate %})</small></p>
            </div>
            <div class="rebate-table rebate-table-modal">
                <table>
                    <tbody>
                        <tr>
                            <td data-i18n="LABEL_REBATE_BETS"></td>
                            <td id="betModal">{%- tplData.data.EligibleBets %}</td>
                        </tr>
                        <tr>
                            <td data-i18n="LABEL_REBATE_PERCENT"></td>
                            <td id="percentModal">{%- tplData.data.rebatePercent %}</td>
                        </tr>
                        <tr>
                            <td data-i18n="LABEL_REBATE_AMOUNT"></td>
                            <td id="amountModal">{%- tplData.data.Amount %}</td>
                        </tr>
                        <tr>
                            <td data-i18n="LABEL_REBATE_CLAIMED_AMOUNT"></td>
                            <td id="claimModal">{%- tplData.data.ClaimedAmount %}</td>
                        </tr>
                        <tr>
                            <td data-i18n="LABEL_REBATE_BALANCE_AMOUNT"></td>
                            <td id="balanceModal">{%- tplData.data.BalanceRebateAmount %}</td>
                        </tr>
                        <tr>
                            <td colspan="2">{% if(tplData.data.AllowClaim) { %}
                        <input type="button" name="claimNow" value="{%- tplData.data.btnInstantClaim %}" id="rebateClaim" class="btn btn-block btn-primary" onclick="javascript: $('#rebate-modal').modal('toggle'); _w88_Rebates.ClaimNow('{%- tplData.data.ProductCode %}', '{%- tplData.data.BalanceRebateAmount %}', '{%- tplData.data.AllowClaim %}');">
                                {% } else { %}
                        <input type="button" name="" value="{%- tplData.data.btnInstantClaim %}" id="" class="btn btn-block btn-primary" disabled="disabled">
                                {% } %}
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>

            <p><small>{%- tplData.data.note1 %} {%- tplData.data.CurrencyCode %} {%- tplData.data.MinimumClaim %} {%- tplData.data.note2 %}</small></p>
        </div>
    </script>

    <!-- Modal -->
    <div class="modal modal-fullscreen fade" id="rebate-modal" tabindex="-1" role="dialog" aria-labelledby="">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span class="icon icon-close"></span></button>
                    <h4 class="modal-title"></h4>
                </div>
                <div class="modal-body">
                </div>
            </div>
        </div>
    </div>
</asp:Content>

