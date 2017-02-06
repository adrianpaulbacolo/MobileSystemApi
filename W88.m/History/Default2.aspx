<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Wallets.master" AutoEventWireup="true" CodeFile="Default2.aspx.cs" Inherits="History_Default2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head1" runat="Server">
    <script type="text/javascript" src="/_Static/JS/vendor/slick.min.js"></script>
    <script type="text/javascript" src="/_Static/JS/modules/templates.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
    <script type="text/javascript" src="/_Static/JS/modules/history.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion")%>"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
    <div class="history-result">
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
                                <asp:Button data-theme="b" ID="btnSubmit" runat="server" CssClass="button-blue" />
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

            $('.history-result').on('beforeChange', function (event, slick, currentSlide, nextSlide) {
                _w88_history.getReport($(slick.$slides.get(nextSlide)).attr('id'));
            });

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
                window.w88Mobile.FormValidator.disableSubmitButton('input[id$="btnSubmit"]');

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
