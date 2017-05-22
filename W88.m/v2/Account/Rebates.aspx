<%@ Page Title="" Language="C#" MasterPageFile="~/v2/MasterPages/Main.master" AutoEventWireup="true" CodeFile="Rebates.aspx.cs" Inherits="v2_Account_Rebates" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContentHolder" runat="Server">
    <div class="form-container">
        <div class="container">
            <div class="form-group">
                <label id="labelPeriod"></label>
                <select id="weeks" class="form-control"></select>
                <p class="text-center"><small><span id="monday"></span>(<span id="startdate"></span>) - <span id="sunday"></span>(<span id="endDate"></span>)</small> </p>
            </div>
        </div>
    </div>

    <div id="group"></div>

    <div class="rebates-disclaimer">

        <div><small id="rebateDisclaimer"></small></div>
        <p><small id="rebateDisclaimerMin"></small></p>

        <div class="curr_week">
            <small id="rebateDisclaimerNoteCurrent"></small>
        </div>

        <div class="prev_week">
            <small id="rebateDisclaimerNote1"></small>
            <small id="rebateDisclaimerNote2"></small>
        </div>

    </div>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/modules/account/rebates.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
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
        <p><small><i>{%- tplData.Note %}</i></small></p>
        {% } %}

        {% _.forEach( op.Collection, function( col, index ){ %}
        <div class="rebate-box">
            <div class="rebate-title">
                <div class="row thin-gutter">
                    <div class="col-xs-6">
                        <h3 title="{%- col.Name %}">{%- col.Name %}</h3>
                    </div>
                    <div class="col-xs-6">
                        {% if(op.NativeGroupKey != "SB") { %}
                        {% if(col.AllowClaim) { %}
                        <input type="button" name="btnSubmit" value="{%- tplData.BtnClaim %}" id="rebatesClaim" class="btn btn-block btn-primary" onclick="javascript: _w88_Rebates.ClaimQuery('{%- col.ProductCode %}', '{%- col.AllowClaim %}');">
                        {% } else { %}
                        <input type="button" name="" value="{%- tplData.BtnClaim %}" id="" class="btn btn-block btn-primary" disabled="disabled">
                        {% } %}
                    {% } %}
                    </div>
                </div>
            </div>
            <div class="rebate-table">
                <table>
                    <tbody>
                        <tr>
                            <td>{%- tplData.LabelRebateAmount %}</td>
                            <td>{%- col.RebateAmount %}</td>
                        </tr>
                    </tbody>
                </table>
                <div class="collapsible-table">
                    <table>
                        <tbody>
                            <tr>
                                <td>{%- tplData.LabelRebatePercent %}</td>
                                <td>{%- col.RebatePercent %}</td>
                            </tr>
                            <tr>
                                <td>{%- tplData.LabelRebateBets %}</td>
                                <td>{%- col.TotalEligibleBet %}</td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
            <a href="#" class="collapsible-btn"><span>{%- tplData.LabelSeeMore %}</span></a>
        </div>
        {% if((op.NativeGroupKey == "SB") && ((index + 1) == op.Collection.length) && (tplData.ShowWeekClaim))  { %}
            <button type="button" value="" id="weeklyBtn" class="btn btn-block btn-primary" onclick="javascript: weeklyClaim();"></button>
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
            <h4>{%- tplData.data.congrats %}!</h4>
                {% } %}

        <p>{%- tplData.data.msg %}</p>
            </div>
        </div>
    </script>

    <script type="text/template" id='ClaimModal'>
        <div class="padding rebates-modal-content">
            <div class="text-center">
                <h4>{%- tplData.data.Summary %}</h4>
                <p>{%- tplData.data.Monday %} ({%- tplData.data.StartDate %}) - {%- tplData.data.Sunday %} ({%- tplData.data.EndDate %}) </p>
            </div>
            <div class="rebate-table rebate-table-modal">
                <table>
                    <tbody>
                        <tr>
                            <td>{%- tplData.data.LabelRebateBets %}</td>
                            <td id="betModal">{%- tplData.data.EligibleBets %}</td>
                        </tr>
                        <tr>
                            <td>{%- tplData.data.LabelRebatePercent %}</td>
                            <td id="percentModal">{%- tplData.data.rebatePercent %}</td>
                        </tr>
                        <tr>
                            <td>{%- tplData.data.LabelRebateAmount %}</td>
                            <td id="amountModal">{%- tplData.data.Amount %}</td>
                        </tr>
                        <tr>
                            <td>{%- tplData.data.LabelClaimedAmount %}</td>
                            <td id="claimModal">{%- tplData.data.ClaimedAmount %}</td>
                        </tr>
                        <tr>
                            <td>{%- tplData.data.LabelBalanceAmount %}</td>
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

