<%@ Page Title="" Language="C#" MasterPageFile="~/v2/MasterPages/Main.master" AutoEventWireup="true" CodeFile="ContactUs.aspx.cs" Inherits="v2_ContactUs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContentHolder" runat="Server">

    <div id="ContactContainer" class="container contact"></div>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="InnerScriptPlaceHolder" runat="Server">
    <script type="text/javascript" src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/modules/contactus.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>

    <script>
        $(document).ready(function () {
            $('.header-title').html($.i18n("LABEL_CONTACT_US"));

            _w88_ContactUs.init();
        });
    </script>

    <script type="text/template" id='ContactUsTemplate'>
        <ul class="media-list">
            {% _.forEach( tplData.data, function( contact ){ %}
                <li class="media">
                    <div class="media-left">
                        <span class="icon icon-{%-contact.Key%}"></span>
                    </div>
                    <div class="media-body">
                        <a href="{%- contact.Link %}">
                            <h4>{%- contact.Name %}</h4>
                            <p>{%- contact.Message %}</p>
                        </a>
                    </div>
                </li>
            {% }); %}
        </ul>
    </script>

</asp:Content>

