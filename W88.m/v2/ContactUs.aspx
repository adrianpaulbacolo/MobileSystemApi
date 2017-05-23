<%@ Page Title="" Language="C#" MasterPageFile="~/v2/MasterPages/Main.master" AutoEventWireup="true" CodeFile="ContactUs.aspx.cs" Inherits="v2_ContactUs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContentHolder" runat="Server">

    <div id="ContactContainer" class="container"></div>

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
        {% _.forEach( tplData.data, function( contact ){ %}

            <div class="row">
                <div class="col-xs-2">
                    {% if( _.lowerCase(contact.Key) == "chat"){ %}
                            <span class="icon icon-chat"></span>
                    {% } else if( _.lowerCase(contact.Key) == "skype"){ %}
                        <span class="icon icon-skype"></span>
                    {% } else if( _.lowerCase(contact.Key) == "email"){ %}
                        <span class="icon icon-mail"></span>
                    {% } else if( _.lowerCase(contact.Key) == "bank"){ %}
                        <span class="icon icon-banking"></span>
                    {% } %}
                </div>
                <div class="col-xs-10">
                    <a href="{%- contact.Link %}">
                        <h4>{%- contact.Name %}</h4>
                        <p>{%- contact.Message %}</p>
                    </a>
                </div>
            </div>

        {% }); %}

    </script>

</asp:Content>

