<%@ Page Title="" MasterPageFile="~/MasterPages/Slots.master" Language="C#" AutoEventWireup="true" CodeFile="SlotPromo.aspx.cs" Inherits="Slots_SlotPromo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="ui-content">
        <div class="daily-slots">
            <div class="daily-slots-banner">
                <img src="/_Static/Images/W88-Mobile-Daily-Slot-Promo.jpg" alt="Daily Slot Title Here">
            </div>
            <div class="daily-slots-content">
                <div class="daily-slots-content-header">
                    <h4>
                        <asp:Literal ID="mainLabel" runat="server" /></h4>
                    <p>
                        <asp:Literal ID="description" runat="server" />
                    </p>
                </div>
                <button type="button" class="ui-btn btn-gray active-btn" onclick="claimPrizeToggle()"><%=commonCulture.ElementValues.getResourceString("ClaimPrize", commonVariables.PromotionsXML)%></button>
                <div class="daily-slots-claim">
                </div>
                <button type="button" class="ui-btn btn-gray" onclick="claimToggle(0)"><%=commonCulture.ElementValues.getResourceString("ShowSlots", commonVariables.PromotionsXML)%></button>
                <div class="daily-slots-promo">
                    <ul class="daily-slots-promo-nav">
                        <li data-week="-2">
                            <button onclick="fetchPromo(-2)"><%=commonCulture.ElementValues.getResourceString("Last2Week", commonVariables.PromotionsXML)%></button></li>
                        <li data-week="-1">
                            <button onclick="fetchPromo(-1)"><%=commonCulture.ElementValues.getResourceString("LastWeek", commonVariables.PromotionsXML)%></button></li>
                        <li data-week="0">
                            <button onclick="fetchPromo(0)"><%=commonCulture.ElementValues.getResourceString("CurrentWeek", commonVariables.PromotionsXML)%></button></li>
                        <li data-week="1">
                            <button onclick="fetchPromo(1)"><%=commonCulture.ElementValues.getResourceString("NextWeek", commonVariables.PromotionsXML)%></button></li>
                    </ul>
                    <div class="daily-slots-promo-games" id="slots">
                        <p id="weekly-label"></p>
                        <div class="div-product">
                            <ul id="promo-list">
                            </ul>

                        </div>
                    </div>
                </div>
                <div class="daily-slots-claim-terms">
                    <%
                        var terms = commonCulture.ElementValues.getResourceString("Terms", commonVariables.PromotionsXML);
                        var promolink = String.Format("<a data-ajax=\"false\" href=\"{0}\">{1}</a>", "/Promotions#DAILYSLOTS", commonCulture.ElementValues.getResourceString("PromoLink", commonVariables.PromotionsXML));
                        terms = terms.Replace("{promolink}", promolink);
                        terms = terms.Replace("{dailyslottext}", commonCulture.ElementValues.getResourceString("ShowSlots", commonVariables.PromotionsXML));
                    %>
                    <%=terms %>
                </div>
            </div>
        </div>
    </div>

    <style>
        div.daily-slots {
            background-color: #252525;
        }

        div.daily-slots-claim-terms ol li span {
            text-transform: uppercase;
        }

        #promo-list .spinner-generic,
        .daily-slots-claim .spinner-generic {
            margin-left: 45%;
        }

        .daily-slots-claim-terms {
            padding-top: 15px;
        }

            .daily-slots-claim-terms ol {
                padding-left: 15px;
            }

                .daily-slots-claim-terms ol li {
                    padding-bottom: 5px;
                }
    </style>

    <script type="text/javascript">
        var activePromoClaim = {};
        var currentPromoClaim = {};
        var week = "0";
        var loader = GPInt.prototype.GetLoaderScafold();
        $(document).ready(function () {
            fetchPromo(week);
        });

        function claimPrizeToggle() {
            $(".daily-slots-claim").html("");
            if (_.isEmpty(currentPromoClaim) || _.isEmpty(currentPromoClaim.game)) {
                currentPromoClaim = {};
                showClaimInfo();
            } else {
                setPromo(currentPromoClaim);
            }
            claimToggle(1);
        }

        function formatAmount(amount) {
            return parseFloat(amount).toFixed(2);
        }

        var claimToggle = function (show) {
            if (show) {
                $(".daily-slots-claim").show();
                $(".daily-slots-promo").hide();
            } else {
                $(".daily-slots-promo").show();
                $(".daily-slots-claim").hide();
            }
        }

        var claimPromo = function (promo) {
            setPromo(promo);
            claimToggle(true);
        }

        var showClaimInfo = function () {
            if (_.isEmpty(currentPromoClaim) || _.isEmpty(currentPromoClaim.info)) {
                $.ajax({
                    url: "SlotPromo.aspx/claimDetails",
                    data: {},
                    type: "GET",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    beforeSend: function () {
                        $(".daily-slots-claim").prepend(loader);
                    },
                    success: function (mydata) {
                        $(".daily-slots-claim div.spinner-generic").remove();
                        var promo = JSON.parse(mydata.d);
                        currentPromoClaim = promo;

                        if (_.isEmpty(promo.game) || _.isEmpty(promo.game.name)) {
                            w88Mobile.Growl.shout("<%=commonCulture.ElementValues.getResourceString("NoPromo", commonVariables.PromotionsXML)%>");
                            return;
                        } else setPromo(promo);
                    }
                });
            } else {
                setPromo(currentPromoClaim);
            }
        }

        var submitClaim = function (promo) {

            if (!_.isEmpty(promo.info)) {
                if (promo.info.svc_error_code != 0 && promo.info.svc_error != "") {
                    switch (promo.info.svc_error_code) {
                        case -1:
                            if (promo.info.total_stake < 1) {
                                w88Mobile.Growl.shout("<%=commonCulture.ElementValues.getResourceXPathString("StakeAndBonus/Error90", commonVariables.PromotionsXML) %>");
                        } else {
                            w88Mobile.Growl.shout(promo.info.message);
                        }
                        break;
                    default:
                        w88Mobile.Growl.shout(promo.info.message);
                        break;
                }
                return;
            }
        }

            $.ajax({
                url: "SlotPromo.aspx/claimPromo",
                data: JSON.stringify({ id: promo.id, club: promo.game.club }),
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                beforeSend: function () {
                    $(".daily-slots-claim").prepend(loader);
                    $("button[class*='claim-submit']").prop('disabled', true);
                },
                success: function (mydata) {
                    var claim = JSON.parse(mydata.d);
                    if (claim.status != 0) {
                        w88Mobile.Growl.shout(claim.hidden_message);
                    } else {
                        w88Mobile.Growl.shout(claim.hidden_message);
                        currentPromoClaim = {};
                        showClaimInfo();
                    }
                },
                complete: function () {
                    $(".daily-slots-claim div.spinner-generic").remove();
                    $("button[class*='claim-submit']").prop('disabled', false);
                }
            });
        }

    var setPromo = function (promo) {
        activePromoClaim = promo;
        $(".daily-slots-claim").html("");
        var claimGame = $("<div>", { class: "daily-slots-claim-game" })
            .append($("<img>", { src: promo.game.image_link, alt: promo.game.name })).append(" ")
            .append($("<span>").html(promo.endDate))
        $(".daily-slots-claim").append(claimGame);
        var claimDesc = $("<table>")
            .append($("<tr>")
                .append($("<td>").html("<%=commonCulture.ElementValues.getResourceString("Slot", commonVariables.PromotionsXML)%>:"))
                    .append($("<td>").html(promo.game.name)))
                .append($("<tr>")
                    .append($("<td>").html("<%=commonCulture.ElementValues.getResourceString("Instruction", commonVariables.PromotionsXML)%>:"))
                    .append($("<td>").html(promo.instructions)))
                .append($("<tr>")
                    .append($("<td>").html("<%=commonCulture.ElementValues.getResourceString("Bonus", commonVariables.PromotionsXML)%>:"))
                    .append($("<td>").html(formatAmount(promo.game.minBonus))))
                .append($("<tr>")
                        .append($("<td>").html("<%=commonCulture.ElementValues.getResourceString("SlotClub", commonVariables.PromotionsXML)%>:"))
                        .append($("<td>").html(promo.game.clubName)));
        $(".daily-slots-claim").append(claimDesc);

        if (!_.isEmpty(promo.info)) {
            claimDesc.append($("<tr>")
                    .append($("<td>").html("<%=commonCulture.ElementValues.getResourceString("WinLose", commonVariables.PromotionsXML)%>:"))
                        .append($("<td>").html(formatAmount(promo.info.total_win_lost))))
                .append($("<tr>")
                        .append($("<td>").html("<%=commonCulture.ElementValues.getResourceString("ClaimAmount", commonVariables.PromotionsXML)%>:"))
                        .append($("<td>").html(formatAmount(promo.info.bonus_amount))))
                .append($("<tr>")
                        .append($("<td>").html("<%=commonCulture.ElementValues.getResourceString("RollOver", commonVariables.PromotionsXML)%>:"))
                        .append($("<td>").html(formatAmount(promo.info.rollover_amount))));
        }

        if (promo.status == -1) {
            var claimButton = $("<button>", { class: "ui-btn btn-primary claim-submit-button", type: "button" })
                .html("<%=commonCulture.ElementValues.getResourceString("ClaimNow", commonVariables.PromotionsXML)%>");
            claimButton.on("click", function () {
                if (window.User.hasSession != 1)
                    window.open("/_Secure/Login.aspx?redirect=" + encodeURI("Slots/SlotPromo.aspx"));
                else submitClaim(activePromoClaim);
            })
            $(".daily-slots-claim").append(claimButton);

        } else if (promo.status == 1) {

            var playButton = $("<button>", { class: "ui-btn btn-primary", type: "button" })
                .html("<%=commonCulture.ElementValues.getResourceString("PlayNow", commonVariables.PromotionsXML)%>");
                playButton.on("click", function () {
                    var cUrl = window.location.hostname;
                    window.open(activePromoClaim.game.game_link);
                })
                $(".daily-slots-claim").append(playButton);
            }
    }

    function fetchPromo(week) {

        var curDay = moment().day();
        var monday = moment().add(week, "week");
        if (curDay > 1) {
            monday = moment(monday).add(1 - curDay, "day");
        }
        var sunday = moment(monday).add(6, "day");
        var weekLabel = "<%=commonCulture.ElementValues.getResourceString("WeeklyLabel", commonVariables.PromotionsXML)%>";
        weekLabel = weekLabel.replace("{from}", monday.format("DD/MM/YYYY")).replace("{to}", sunday.format("DD/MM/YYYY"));
        $("#weekly-label").html(weekLabel);

        _.forEach($(".daily-slots-promo-nav > li"), function (nav) {
            var childNav = $(nav).children(":first");
            if ($(nav).attr("data-week") != week) {
                childNav.removeClass("active");
            } else {
                if (!childNav.hasClass("active")) childNav.addClass("active");
            }
        });
        $.ajax({
            url: "SlotPromo.aspx/getWeeklyPromo",
            data: { week: week },
            type: "GET",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            beforeSend: function () {
                $("#promo-list").html(loader);
            },
            success: function (mydata) {
                $("#promo-list").children().remove();
                var response = JSON.parse(mydata.d);
                var promoList = response.promoList;

                if (!_.isEmpty(response.message)) w88Mobile.Growl.shout(response.message);

                _.forEach(promoList, function (promo) {
                    if (_.isEmpty(promo.game) || _.isEmpty(promo.game.Id)) return;
                    promo.endDate = moment(promo.start).format("MM/DD/YYYY");
                    var state = "";
                    var claimText = "";
                    switch (promo.status) {
                        case -1:
                            //inactive for other state
                            state = "daily-active";
                            claimText = "<%=commonCulture.ElementValues.getResourceString("ClaimNow", commonVariables.PromotionsXML)%>";
                                break;
                            case 0:
                                state = "daily-inactive";
                                claimText = promo.endDate;
                                break;
                            case 1:
                            case 2:
                                state = "";
                                claimText = promo.endDate;
                                break;
                        }
                        var gameItem = $("<li>", { class: "bkg-game " + state })
                        .append($("<div>", { rel: promo.game.Id })
                            .append($("<img>", { src: promo.game.image_link, class: "img-responsive-full" }))
                            .append($("<div>", { class: "daily-game-action" })
                                .append($("<span>", {}).html(claimText))));
                        if (promo.status != 0) {
                            gameItem.on("click", function () {
                                claimPromo(promo);
                            });
                        }
                        $("#promo-list").append(gameItem).append(" ");
                    });
                }
            });

        }

    </script>
</asp:Content>
