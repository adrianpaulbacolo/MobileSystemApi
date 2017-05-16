<%@ Page Title="" Language="C#" MasterPageFile="~/v2/History/History.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="History_Default" %>

<asp:Content ID="Content4" ContentPlaceHolderID="head" runat="Server">
    <script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/templates.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"> </script>
    <script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/modules/history.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"> </script>
    <script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/modules/funds.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"> </script>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="body">

        <div class="wallets wallet-auto">
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

        <!-- Results Page -->
        <div class="history-result">
            <div id="adjustment">
            </div>
            <div id="depositwidraw">
            </div>
            <div id="fundtransfer">
            </div>
            <div id="promoclaim">
            </div>
        </div>
    </section>

    <!-- Results Detail Page -->
    <div class="history-full"></div>
</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <!-- Modal -->
    <div class="modal fade" id="history-modal" tabindex="-1" role="dialog" aria-labelledby="history">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header mheader-notitle">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span class="icon icon-close"></span></button>
                </div>
                <div class="modal-body">
                    <div class="form-container">
                        <form class="form" id="form1" runat="server">
                            <div class="form-group">
                                <asp:Label ID="lblTransactionType" runat="server" AssociatedControlID="ddlTransactionType" />
                                <asp:DropDownList ID="ddlTransactionType" runat="server" CssClass="form-control" />
                            </div>
                            <div class="form-group">
                                <asp:Label ID="lblDateFrom" runat="server" AssociatedControlID="txtDateFrom" />
                                <asp:TextBox ID="txtDateFrom" type="text" runat="server" CssClass="form-control" />
                            </div>
                            <div class="form-group">
                                <asp:Label ID="lblDateTo" runat="server" AssociatedControlID="txtDateTo" />
                                <asp:TextBox ID="txtDateTo" type="text" runat="server" CssClass="form-control" />
                            </div>
                            <div class="form-group" id="type">
                                <asp:Label ID="lblType" runat="server" AssociatedControlID="ddlType" />
                                <asp:DropDownList ID="ddlType" runat="server" CssClass="form-control" />
                            </div>
                            <div class="form-group" id="status">
                                <asp:Label ID="lblStatus" runat="server" AssociatedControlID="ddlStatus" />
                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control" />
                            </div>
                            <button type="submit" id="btnSubmit" class="btn btn-block btn-primary"></button>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content7" ContentPlaceHolderID="ScriptsPlaceHolder" runat="Server">
    <script type="text/javascript">
        $(document).ready(function () {
            _w88_funds.mainWalletInit();

            _w88_history.init();

            $('#historyFilter').click(function () {
                $('#history-modal').modal('show');
            });

            $('select[id$="ddlTransactionType"]').change(function () {
                _w88_history.toggleType(this.value);
            });

            var dateOptions = {
                mode: 'calbox',
                showInitialValue: true,
                overrideDateFormat: '%m/%d/%Y',
                beforeToday: true,
            };

            $('input[id$="txtDateFrom"]').datebox(dateOptions);
            $('input[id$="txtDateFrom"]').datebox('setTheDate', _w88_history.minDate);
            $('input[id$="txtDateTo"]').datebox(dateOptions);

            $('#form1').submit(function (e) {
                e.preventDefault();

                $('#history-modal').modal('hide');

                var dateFrom = new Date($('input[id$="txtDateFrom"]').datebox('getTheDate'));
                var dateTo = new Date($('input[id$="txtDateTo"]').datebox('getTheDate'));

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

    <script type="text/template" id='mainWallet'>
        <div class="wallet-main">
            <p class="wallet-title">{%-tplData.Name%}</p>
            <h4 class="wallet-value">{%-tplData.Balance%}</h4>
            <p class="wallet-currency">{%-tplData.CurrencyLabel%}</p>
        </div>
    </script>
</asp:Content>
