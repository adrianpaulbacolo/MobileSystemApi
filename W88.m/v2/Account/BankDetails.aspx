<%@ Page Language="C#" MasterPageFile="~/v2/MasterPages/Main.master" AutoEventWireup="true" CodeFile="BankDetails.aspx.cs" Inherits="v2_Account_BankDetails" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContentHolder" runat="Server">
    <div class="form-container">
        <div class="container">
            <form class="form" id="form1" runat="server">
                <div class="form-group">
                    <asp:Label ID="lblBank" runat="server" AssociatedControlID="drpBank" data-i18n="LABEL_BANK" />
                    <asp:DropDownList ID="drpBank" runat="server" CssClass="form-control" required data-selectequals="-1" />
                </div>
                <div class="form-group bankname" hidden>
                    <asp:Label ID="lblBankName" runat="server" AssociatedControlID="txtBankName" data-i18n="LABEL_BANK_NAME" />
                    <asp:TextBox ID="txtBankName" runat="server" CssClass="form-control" />
                </div>
                <div class="form-group">
                    <asp:Label ID="lblBankBranch" runat="server" AssociatedControlID="txtBankBranch" data-i18n="LABEL_BANK_BRANCH" />
                    <asp:TextBox ID="txtBankBranch" runat="server" CssClass="form-control" required data-require="" />
                </div>
                <div class="form-group">
                    <asp:Label ID="lblBankAddress" runat="server" AssociatedControlID="txtBankAddress" data-i18n="LABEL_BANK_ADDRESS" />
                    <asp:TextBox ID="txtBankAddress" runat="server" CssClass="form-control" required data-require="" />
                </div>
                <div class="form-group">
                    <asp:Label ID="lblAccountName" runat="server" AssociatedControlID="txtAccountName" data-i18n="LABEL_ACCOUNT_NAME" />
                    <asp:TextBox ID="txtAccountName" runat="server" CssClass="form-control" required data-require="" />
                </div>
                <div class="form-group">
                    <asp:Label ID="lblAccountNumber" runat="server" AssociatedControlID="txtAccountNumber" data-i18n="LABEL_ACCOUNT_NUMBER" />
                    <asp:TextBox ID="txtAccountNumber" runat="server" CssClass="form-control" required data-require="" />
                </div>
                <div class="form-group">
                    <div class="checkbox checkbox-custom">
                        <label>
                            <input type="checkbox" id="isPreferred" />
                            <span id="lblIsPreferred" data-i18n="LABEL_IS_PREFERRED"></span>
                        </label>
                    </div>
                </div>
                <button type="submit" id="btnSubmit" class="btn btn-block btn-primary" data-i18n="BUTTON_SUBMIT"></button>
            </form>
        </div>
    </div>
</asp:Content>

<asp:Content ID="ScriptBottom" ContentPlaceHolderID="InnerScriptPlaceHolder" runat="Server">
    <script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/modules/bootstrapvalidator.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
    <script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/modules/account/bankdetails.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>

    <script>
        $(document).ready(function () {
            _w88_bankdetails.init();

            $('#form1').validator().on('submit', function (e) {
                if (!e.isDefaultPrevented()) {
                    e.preventDefault();

                    var data = {
                        Bank: { Text: $('select[id$="drpBank"] option:selected').text(), Value: $('select[id$="drpBank"]').val() },
                        BankName: $('input[id$="txtBankName"]').val(),
                        BankBranch: $('input[id$="txtBankBranch"]').val(),
                        BankAddress: $('input[id$="txtBankAddress"]').val(),
                        AccountName: $('input[id$="txtAccountName"]').val(),
                        AccountNumber: $('input[id$="txtAccountNumber"]').val(),
                        IsPreferred: $('input[id$="isPreferred"]').is(':checked')
                    }

                    _w88_bankdetails.createBankDetails(data);
                }
            });
        });
    </script>
</asp:Content>
