<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Site.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="History_Default" %>

<%@ Register TagPrefix="mainWallet" TagName="Wallet" Src="~/UserControls/MainWalletBalance.ascx" %>

<asp:Content ID="Content4" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="/_Static/history/assets/js/vendor/slick.min.js"></script>
    <script type="text/javascript" src="/_Static/history/assets/js/vendor/pubsub.js"></script>
    <script type="text/javascript" src="/_Static/history/assets/js/modules/templates.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
    <script type="text/javascript" src="/_Static/history/assets/js/modules/history.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion")%>"></script>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="ui-content" role="main">
        <div class="wallet main-wallet">
            <mainWallet:Wallet ID="uMainWallet" runat="server" />
        </div>

        <div class="history-nav-container">
            <div class="history-nav">
                <div id="adj-btn">
                    <span class="initial" id="adj-lbl"></span>
                </div>
                <div id="dep-btn">
                    <span id="dep-lbl"></span>
                </div>
                <div id="fund-btn">
                    <span id="fund-lbl"></span>
                </div>
                <div id="promo-btn">
                    <span id="promo-lbl"></span>
                </div>
            </div>
        </div>

        <div class="history-result bg-gradient">
            <div id="adjustment">
            </div>
            <div id="depositwidraw">
            </div>
            <div id="fundtransfer">
            </div>
            <div id="promoclaim">
            </div>
        </div>

        <div class="history-modal"></div>

        <div id="filterModal" data-role="popup" data-overlay-theme="b" data-theme="b" data-history="false">
            <a href="#" data-rel="back" class="close close-enhanced">&times;</a>
            <div class="padding">
                <div class="download-app padding">
                    <form class="form" id="form1" runat="server" data-ajax="false">
                        <br>
                        <ul class="list fixed-tablet-size">
                            <li class="item item-select">
                                <asp:Label ID="lblTransactionType" runat="server" AssociatedControlID="ddlTransactionType" />
                                <asp:DropDownList ID="ddlTransactionType" runat="server" data-corners="false" />
                            </li>
                            <li class="item item-input">
                                <asp:Label ID="lblDateFrom" runat="server" AssociatedControlID="txtDateFrom" />
                                <asp:TextBox ID="txtDateFrom" type="date" runat="server"></asp:TextBox>
                            </li>
                            <li class="item item-input">
                                <asp:Label ID="lblDateTo" runat="server" AssociatedControlID="txtDateTo" />
                                <asp:TextBox ID="txtDateTo" type="date" runat="server"></asp:TextBox>
                            </li>
                            <li class="item item-select" id="type">
                                <asp:Label ID="lblType" runat="server" AssociatedControlID="ddlType" />
                                <asp:DropDownList ID="ddlType" runat="server" data-corners="false" />
                            </li>
                            <li class="item item-select" id="status">
                                <asp:Label ID="lblStatus" runat="server" AssociatedControlID="ddlStatus" />
                                <asp:DropDownList ID="ddlStatus" runat="server" data-corners="false" />
                            </li>
                            <li class="item row">
                                <div class="col">
                                    <asp:Button data-theme="b" data-rel="back" ID="btnSubmit" runat="server" CssClass="button-blue" />
                                </div>
                            </li>
                        </ul>
                    </form>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ScriptsPlaceHolder" runat="Server">
    <script type="text/javascript">
        $(document).ready(function () {
            _.templateSettings = {
                interpolate: /\{\{(.+?)\}\}/g,      // print value: {{ value_name }}
                evaluate: /\{%([\s\S]+?)%\}/g,   // excute code: {% code_to_execute %}
                escape: /\{%-([\s\S]+?)%\}/g
            };
            _.templateSettings.variable = "history";

            var _w88_history = window.w88Mobile.History();
            _w88_history.init();

            $('#filterHistory').click(function () {
                $('#filterModal').popup();
                $('#filterModal').popup('open');
            });

            $('select[id$="ddlTransactionType"]').change(function () {
                _w88_history.toggleType(this.value);
            });

            $('input[id$="txtDateFrom"]').on('focusout', function () {

                $('input[id$="txtDateTo"]').val("");
                $('input[id$="txtDateTo"]').attr("min", $('input[id$="txtDateFrom"]').val());

                var date = new Date($('input[id$="txtDateFrom"]').val());
                var maxDays = parseInt(90);
                date.setDate(date.getDate() + maxDays);

                var month = date.getMonth() + 1;
                var day = date.getDate();

                var maxDate = date.getFullYear() + '-' +
                    (month < 10 ? '0' : '') + month + '-' +
                    (day < 10 ? '0' : '') + day;

                $('input[id$="txtDateTo"]').attr("max", maxDate);
            });

            $('#form1').submit(function (e) {
                e.preventDefault();

                $('#filterModal').popup();
                $('#filterModal').popup('close');

                var dateFrom = new Date($('input[id$="txtDateFrom"]').val());
                var dateTo = new Date($('input[id$="txtDateTo"]').val());

                var data = {
                    DateFrom: _w88_history.formatDateTime(dateFrom),
                    DateTo: _w88_history.formatDateTime(dateTo),
                    Status: $('select[id$="ddlStatus"] option:selected').val(),
                    Type: $('select[id$="ddlType"] option:selected').val(),
                    ReportType: $('select[id$="ddlTransactionType"] option:selected').val(),
                }

                _w88_history.setReportStatus(data.ReportType);

                _w88_history.getReport(data.ReportType, data);
            });
        });
    </script>
</asp:Content>
