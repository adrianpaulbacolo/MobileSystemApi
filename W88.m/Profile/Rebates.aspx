<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Site.master" AutoEventWireup="true" CodeFile="Rebates.aspx.cs" Inherits="Rebates" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="/_Static/JS/Cookie.js"></script>
    <script type="text/javascript" src="/_Static/JS/modules/rebates.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="ui-content rebates" role="main">
        <form class="form" runat="server">
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
                    <br />
                    <button type="button" value="" id="weeklyBtn" class="button-blue" data-theme="b"></button>
                </div>

                <div class="prev_week">
                    <small id="rebateDisclaimerNote1"></small>
                    <small id="rebateDisclaimerNote2"></small>
                </div>

            </div>
            
        </form>
    </div>


    <div id="rebatesModal" data-role="popup" data-overlay-theme="b" data-theme="b" data-history="false">
        <a href="#" id="closeModal" class="close close-enhanced">&times;</a>

        <div class="padding">
            <div id="modalContent">
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
               
                window.w88Mobile.Rebates.GetWeeklySettings('<%=Username%>');
            });

            window.w88Mobile.Rebates.Initialize();

            $("#weeks").change(function () {

                window.w88Mobile.Rebates.Statement();

                if ($("#weeks").val() == $("#weeks option:first").val()) {
                    $(".curr_week").show();
                    $(".prev_week").hide();
                }
                else {
                    $(".curr_week").hide();
                    $(".prev_week").show();
                }

            });

        });

    </script>

</asp:Content>
