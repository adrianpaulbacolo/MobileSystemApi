<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Site.master" AutoEventWireup="true" CodeFile="Rebates.aspx.cs" Inherits="Rebates" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="/_Static/JS/Cookie.js"></script>
    <script type="text/javascript" src="/_Static/JS/modules/rebates.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="ui-content rebates" role="main">
        <form class="form" runat ="server">
            <ul class="list fixed-tablet-size">
                <li class="item item-select">
                    <label id="labelPeriod"></label>
                    <div class="ui-select">
                        <div class="">
                            <span>
                                <select id="weeks"></select>
                            </span>
                        </div>
                    </div>
                    <p class="text-center"><small><span id="monday"></span>(<span id="startdate"></span>) - <span id="sunday"></span>(<span id="endDate"></span>)</small> </p>
                </li>
            </ul>

            <div id="group"></div>

            <div class="rebates-disclaimer">

                <div><small id="rebateDisclaimer"></small></div>
                <p><small id="rebateDisclaimerMin"></small></p>

                <div class="curr_week">
                    <small id="rebateDisclaimerNoteCurrent"></small>
                    <br/>
                    <input type="button" value="" id="weeklyBtn" class="button-blue" data-theme="b">
                </div>

                <div class="prev_week">
                    <small id="rebateDisclaimerNote1"></small>
                    <small id="rebateDisclaimerNote2"></small>
                </div>
                
            </div>
            
            <asp:HiddenField ID="hfWeekPromo" runat="server" />

        </form>
    </div>


    <div id="rebatesModal" data-role="popup" data-overlay-theme="b" data-theme="b" data-history="false">
        <a href="#" id="closeModal" class="close close-enhanced">&times;</a>

        <div class="padding">
            <div id="modalContent">

                <iframe id="promoiframe" src=""
                    style="border: 0; overflow: hidden; width: 530px; height: 250px;"
                    frameborder='0'
                    border='0'
                    allowtransparency="true"></iframe>

            </div>
        </div>
    </div>

    <script>

        $("#rebatesClaim").bind("click", function () {
            $('#rebatesModal').popup('open');
        });
        $("#closeModal").bind("click", function () {
            $('#rebatesModal').popup('close');
        });

        $(document).ready(function () {

            $("#weeklyBtn").click(function () {
                $("#rebatesModal").find("iframe").css("height", 0).attr("src", $("#hfWeekPromo").val());
                $("#rebatesModal").modal("show");
                return false;
            });

            window.w88Mobile.Rebates.Initialize();

            $("#weeks").change(function () {

                if ($("#weeks").val() == $("weeks option:first").val()) {
                    $(".curr_week").show();
                    $(".prev_week").hide();
                }
                else {
                    $(".curr_week").hide();
                    $(".prev_week").show();
                }

                window.w88Mobile.Rebates.Statement();

                $("#weeklyBtn").val(sessionStorage.getItem("weeklyClaim"));
            });

        });

    </script>

</asp:Content>