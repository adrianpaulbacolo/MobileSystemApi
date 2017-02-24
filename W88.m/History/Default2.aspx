<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Wallets.master" AutoEventWireup="true" CodeFile="Default2.aspx.cs" Inherits="History_Default2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head1" runat="Server">
    <script type="text/javascript" src="/_Static/JS/vendor/slick.min.js"></script>
    <script type="text/javascript" src="/_Static/JS/modules/templates.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
    <script type="text/javascript" src="/_Static/JS/modules/history.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion")%>"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
    <div class="history-nav-container">
        <div class="history-nav">
            <div id="adj-btn">
                <span class="initial">Adjustment</span>
            </div>
            <div id="dep-btn">
                <span>Deposit/ Withdrawal</span>
            </div>
            <div id="fund-btn">
                <span>Fund Transfer</span>
            </div>
            <div id="ref-btn">
                <span>Referral Bonus</span>
            </div>
            <div id="promo-btn">
                <span>Promo Claim</span>
            </div>
        </div>
    </div>
    <div class="history-result bg-gradient">
    </div>
    <div class="history-full">
        <div class="history-full-header">
            <h5>Withdrawal 11/17/2016 10:57:29 AM</h5>
            <a href="#"><span class="icon ion-ios-close-empty"></span></a>
        </div>
        <div class="history-full-content">
            <div>
                <p><small>Type</small></p>
                <h5>Deposit</h5>
            </div>
            <div>
                <p><small>Transaction ID</small></p>
                <h5>1034284221716</h5>
            </div>
            <div>
                <p><small>Date</small></p>
                <h5>12/28/2016 4:45:53 PM</h5>
            </div>
            <div>
                <p><small>Payment Method</small></p>
                <h5>Online</h5>
            </div>
        </div>
    </div>

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
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsPlaceHolder1" runat="Server">
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
                    DateFrom: dateFrom.toLocaleDateString(),
                    DateTo: dateTo.toLocaleDateString(),
                    Status: $('select[id$="ddlStatus"]').val(),
                    Type: $('select[id$="ddlType"]').val(),
                    ReportType: $('select[id$="ddlTransactionType"]').val(),
                }

                _w88_history.getHistoryReport(data);
            });
        });
    </script>
</asp:Content>
