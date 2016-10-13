﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Configuration;
using System.Web.Services;
using System.Web.Script.Serialization;
using System.Collections;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Data;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Models;
using Helpers;
using Factories.Slots.Handlers;

/// <summary>
/// Summary description for SlotPromo
/// </summary>
public class SlotPromo
{
    protected XElement xeErrors = null;
    private XElement xeResources = null;
    protected customConfig.OperatorSettings opsettings;
    protected MemberSession.UserSessionInfo userInfo;

    public SlotPromo()
    {
        opsettings = new customConfig.OperatorSettings("W88");
        var user = new Members();
        userInfo = user.MemberData();
    }

    protected Dictionary<string, string> createBaseContent()
    {

        var operatorPass = opsettings.Values.Get("OperatorKey");
        var operatorId = commonVariables.OperatorId;
        var values = new Dictionary<string, string>
        {
           { "OperatorId", operatorId },
           { "Password", commonEncryption.GetMd5Hash(operatorPass) }
        };

        return values;
    }

    public fetchPromoResponse getPromo(DateTime start, DateTime end)
    {
        var riskId = userInfo.RiskId;
        if (string.IsNullOrEmpty(riskId)) riskId = "N";

        var promoResponse = new fetchPromoResponse();
        promoResponse.promoList = new List<SlotPromoItem>();

        var values = createBaseContent();
        values.Add("RiskCategoryId", riskId);
        values.Add("PromoPeriodStartDate", start.ToString("yyyy-MM-dd hh:mm:ss"));
        values.Add("PromoPeriodEndDate", end.ToString("yyyy-MM-dd hh:mm:ss"));

        var content = new FormUrlEncodedContent(values);

        var slotClient = new HttpClient();
        HttpResponseMessage wcfResponse = slotClient.PostAsync(opsettings.Values.Get("DailySlotPromoService") + "/GetEligibleSlotPromos", content).Result;
        HttpContent stream = wcfResponse.Content;
        var data = stream.ReadAsStringAsync();

        DailySlotPromoResponse response = (DailySlotPromoResponse)JsonConvert.DeserializeObject(data.Result, typeof(DailySlotPromoResponse));
        if (response.info.ErrorCode == 0)
        {
            foreach (JsonPromoSettings promo in response.detail)
            {
                promoResponse.promoList.Add(createSlot(promo));
            }

        }
        else
        {
            promoResponse.message = commonCulture.ElementValues.getResourceXPathString(
                    "EligibleSlots/ErrorDefault",
                    commonVariables.PromotionsXML);
        }
        promoResponse.promoList.OrderBy(x => x.start).ToList();
        return promoResponse;
    }

    private string getCurrency()
    {
        var currency = commonCookie.CookieCurrency;
        //@todo check for disabled currency
        if (string.IsNullOrEmpty(currency))
        {
            switch (commonVariables.SelectedLanguage)
            {
                case "zh-cn":
                    currency = "RMB";
                    break;
                case "vi-vn":
                    currency = "VND";
                    break;
                case "th-th":
                    currency = "THB";
                    break;
                case "id-id":
                    currency = "IDR";
                    break;
                case "ko-kr":
                    currency = "KRW";
                    break;
                case "ja-jp":
                    currency = "JPY";
                    break;
                default:
                    currency = "USD";
                    break;
            }

        }
        return currency;
    }

    private SlotPromoItem createSlot(JsonPromoSettings promo)
    {
        var slotItem = new SlotPromoItem();

        slotItem.id = promo.SlotPromoSettingsId;
        // check currency if available
        slotItem.currency = getCurrency();
        int stakeColumn = promo.Amounts.Columns.IndexOf("Stake");
        var amountRow = promo.Amounts.Select(string.Format("CurrencyCode = '{0}'", slotItem.currency)).First();
        var amountStake = Convert.ToInt64(amountRow[stakeColumn]);
        if (amountStake <= 0)
        {
            return slotItem;
        }
        //// fetch game
        slotItem.game = createGame(promo.GamePromoListing);
        if (slotItem.game.name == null) return slotItem;

        slotItem.start = promo.PromoPeriodStart;
        slotItem.end = promo.PromoPeriodEnd;

        int langColumn = promo.Instructions.Columns.IndexOf("LanguageCode");
        var instructionRow = promo.Instructions.Select(string.Format("LanguageCode = '{0}'", commonVariables.SelectedLanguage)).First();
        var instruction = (instructionRow.ItemArray.Length > 1) ? instructionRow[1].ToString() : string.Empty;

        slotItem.instructions = instruction;

        slotItem.instructions = Regex.Replace(slotItem.instructions, @"\[maxBonus\]", string.Format("{0} {1:n2}", slotItem.currency, Convert.ToInt64(amountRow[promo.Amounts.Columns.IndexOf("MaximumBonus")])), RegexOptions.IgnoreCase);
        slotItem.instructions = Regex.Replace(slotItem.instructions, @"\[minBonus\]", string.Format("{0} {1:n2}", slotItem.currency, Convert.ToInt64(amountRow[promo.Amounts.Columns.IndexOf("MinimumBonus")])), RegexOptions.IgnoreCase);
        slotItem.instructions = Regex.Replace(slotItem.instructions, @"\[minStake\]", string.Format("{0} {1:n2}", slotItem.currency, amountStake), RegexOptions.IgnoreCase);
        slotItem.instructions = Regex.Replace(slotItem.instructions, @"\[pbonus\]", string.Format("{1:n0}%", slotItem.currency, Convert.ToInt64(amountRow[promo.Amounts.Columns.IndexOf("Percentage")])), RegexOptions.IgnoreCase);
        slotItem.instructions = Regex.Replace(slotItem.instructions, @"\[gameTitle\]", slotItem.game.name, RegexOptions.IgnoreCase);

        // **** start x
        slotItem.date = DateTime.Parse(slotItem.start);

        if (slotItem.date.Date == DateTime.Now.Date)
        {
            slotItem.status = 1; // today playable
        }
        else if (slotItem.date.Date > DateTime.Now.Date)
        {
            slotItem.status = 0; // future
        }
        else
        {
            var yesterday = (DateTime.Now.Hour >= 2) ? DateTime.Now.AddDays(-1).Date : DateTime.Now.AddDays(-2).Date;

            if (slotItem.date.Date == yesterday)
            {
                slotItem.status = -1; // claimable
            }
            else
            {
                slotItem.status = 0; // yester years
            }

        }

        // set game bonus
        slotItem.game.minBonus = amountRow[promo.Amounts.Columns.IndexOf("MinimumBonus")].ToString();


        return slotItem;
    }


    public SlotPromoGame createGame(JsonSlotGamePromoListing promoGame)
    {
        var game = new SlotPromoGame();
        var xmlGame = findGame(promoGame);
        if (xmlGame == null) return game;
        var gameClub = promoGame.ProductCode;
        var gameCode = promoGame.HtmlGameId;
        var lang = string.Empty;
        switch (gameClub)
        {
            case "slot":
                game.game_link = xmlGame.RealUrl;
                game.image_link = "/_Static/Images/Games/"
                    + xmlGame.Image + ".jpg";
                break;
            case "ags":
                if (!string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId))
                {
                    game.game_link = CommonClubApollo.GetRealUrl
                        .Replace("{GAME}", Convert.ToString(xmlGame.Name))
                        .Replace("{LANG}", lang)
                        .Replace("{TOKEN}", commonVariables.CurrentMemberSessionId);
                }
                break;
            case "ctxm":
            case "betsoft":
                break;
            case "png":
            case "isoftbet":
                break;
            case "vanguard":
                break;
            case "playtech":
                break;
            default:
                break;
        }
        game.Id = promoGame.HtmlGameCode.ToString();
        game.club = gameClub;
        game.clubName = commonProduct.GetWallet(game.club);
        game.name = xmlGame.Title;

        return game;
    }

    public GameInfo findGame(JsonSlotGamePromoListing promoGame)
    {
        var gameClub = promoGame.ProductCode;
        var gameCode = promoGame.HtmlGameId;
        var gameTitle = promoGame.GameTitle;
        var matchedGame = new GameInfo();
        switch (gameClub)
        {
            case "slot":

                var handler = new GPIHandler(commonVariables.CurrentMemberSessionId);

                var gpiCategory = handler.Process();

                foreach (var category in gpiCategory)
                {
                    matchedGame = category.New.Find(x => x.Id == gameCode);
                    if (matchedGame != null) break;
                    matchedGame = category.Current.Find(x => x.Id == gameCode);
                    if (matchedGame != null) break;
                }
                break;
            case "ags":
                commonCulture.appData.GetRootResourceNonLanguage("/Slots/ClubApollo.aspx", out xeResources);
                IEnumerable<XElement> apollo = xeResources.Element("Category").Element("Slots").Elements();
                break;
            case "ctxm":
            case "betsoft":
                // return for now
                commonCulture.appData.getRootResource("/Slots/ClubDivino.aspx", out xeResources);
                break;
            case "png":
            case "isoftbet":
                commonCulture.appData.getRootResource("/Slots/ClubGallardo.aspx", out xeResources);
                break;
            case "vanguard":
                commonCulture.appData.getRootResource("/Slots/ClubMassimo.aspx", out xeResources);
                break;
            case "playtech":
                commonCulture.appData.getRootResource("/Slots/ClubPalazzo.aspx", out xeResources);
                break;
            default:
                break;
        }

        return matchedGame;
    }

    public claim createClaim(long promoId, string gameClub)
    {
        var promoClaim = new claim();


        var riskId = userInfo.RiskId ?? "N";
        var memberCode = (string)userInfo.MemberCode;

        var values = createBaseContent();
        values.Add("MemberCode", memberCode);
        values.Add("SlotPromoSettingsId", promoId.ToString());
        values.Add("ProductCodeToCredit", gameClub);
        values.Add("CreatedBy", memberCode);

        var content = new FormUrlEncodedContent(values);

        var slotClient = new HttpClient();
        HttpResponseMessage wcfResponse = slotClient.PostAsync(opsettings.Values.Get("DailySlotPromoService") + "/AddSlotPromoClaim", content).Result;
        HttpContent stream = wcfResponse.Content;
        var data = stream.ReadAsStringAsync();

        PromoClaimedResult response = (PromoClaimedResult)JsonConvert.DeserializeObject(data.Result, typeof(PromoClaimedResult));

        Models.Info claimInfo = response.info;
        promoClaim.status = claimInfo.ErrorCode;

        switch (response.info.ErrorCode)
        {
            case -1:
            case -2:
            case 1:
            case 10:
            case 100:
                promoClaim.hidden_message = promoClaim.message = commonCulture.ElementValues.getResourceXPathString(
                    "PromoClaim/ErrorDefault",
                    commonVariables.PromotionsXML);
                break;
            case 0:
                promoClaim.hidden_message = promoClaim.message = commonCulture.ElementValues.getResourceXPathString(
                    "PromoClaim/Success",
                    commonVariables.PromotionsXML);
                break;
            default:
                promoClaim.hidden_message = promoClaim.message = commonCulture.ElementValues.getResourceXPathString(
                    "PromoClaim/Error" + Convert.ToString(response.info.ErrorCode),
                    commonVariables.PromotionsXML);
                if (string.IsNullOrEmpty(promoClaim.message))
                {
                    promoClaim.message = commonCulture.ElementValues.getResourceXPathString(
                        "PromoClaim/ErrorDefault",
                        commonVariables.PromotionsXML);
                    promoClaim.hidden_message = string.Format("{0}: {1}", claimInfo.ErrorCode, claimInfo.Message);
                }
                break;
        }

        return promoClaim;
    }

    public ClaimDetail getClaimInfo(long id)
    {
        var defaultErrors = new List<int>() { -1, 2, 0 };
        var info = new ClaimDetail();
        var memberCode = (string)userInfo.MemberCode;

        var values = createBaseContent();
        values.Add("MemberCode", memberCode);
        values.Add("SlotPromoSettingsId", id.ToString());

        var content = new FormUrlEncodedContent(values);

        var slotClient = new HttpClient();
        HttpResponseMessage wcfResponse = slotClient.PostAsync(opsettings.Values.Get("DailySlotPromoService") + "/GetMemberStakeAndBonus", content).Result;
        HttpContent stream = wcfResponse.Content;
        var data = stream.ReadAsStringAsync();

        PromoClaimRequest response = (PromoClaimRequest)JsonConvert.DeserializeObject(data.Result, typeof(PromoClaimRequest));

        Models.Info stakeInfo = response.info;
        info.svc_error_code = stakeInfo.ErrorCode;
        info.svc_error = stakeInfo.Message;
        var errorString = (stakeInfo.ErrorCode == -1) ? "Minus1" : Convert.ToString(stakeInfo.ErrorCode);
        if (defaultErrors.Contains(info.svc_error_code))
        {
            errorString = "ErrorDefault";
        }
        else { }
        info.message = commonCulture.ElementValues.getResourceXPathString(
            "StakeAndBonus/Error" + errorString,
            commonVariables.PromotionsXML);

        switch (stakeInfo.ErrorCode)
        {
            case -1:
            case -2:
            case 1:
            case 10:
                info.message = commonCulture.ElementValues.getResourceXPathString(
                    "StakeAndBonus/ErrorDefault",
                    commonVariables.PromotionsXML);
                break;
            default:
                info.message = commonCulture.ElementValues.getResourceXPathString(
                    "StakeAndBonus/Error" + Convert.ToString(response.info.ErrorCode),
                    commonVariables.PromotionsXML);
                if (string.IsNullOrEmpty(info.message))
                {
                    info.message = commonCulture.ElementValues.getResourceXPathString(
                        "StakeAndBonus/ErrorDefault",
                        commonVariables.PromotionsXML);
                }
                break;
        }

        info.total_stake = response.TotalStake;
        info.bonus_amount = response.ClaimAmount;
        info.rollover_amount = response.RolloverAmount;
        info.total_win_lost = response.TotalStake;

        if (response.SlotPromoSetup != null)
        {
            info.rollover = response.SlotPromoSetup.Rollover;
            info.min_amount = response.SlotPromoSetup.Stake;
        }


        return info;
    }
    public class SlotPromoGame
    {
        public string instruction { get; set; }
        public string game_link { get; set; }
        public string image_link { get; set; }
        public string name { get; set; }
        public string Id { get; set; }
        public string club { get; set; }
        public string minBonus { get; set; }
        public string clubName { get; set; }
        //public string user_currency { get; set; }
        //public string selected_currency { get; set; }
        //public string disabled_currency { get; set; }
    }

    public class SlotPromoItem
    {
        public SlotPromoGame game { get; set; }
        public ClaimDetail info { get; set; }
        public long id { get; set; }
        public int status { get; set; }
        public string currency { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string instructions { get; set; }
        public DateTime date { get; set; }
        //@todo what the hell is win_lose property
    }

    public class ClaimDetail
    {
        public decimal total_stake { get; set; }
        public decimal total_win_lost { get; set; }
        public decimal bonus_amount { get; set; }
        public decimal min_amount { get; set; }
        public decimal rollover { get; set; }
        public decimal rollover_amount { get; set; }
        public string svc_error { get; set; }
        public int svc_error_code { get; set; }
        public string message { get; set; }

    }
    public class claim
    {
        public int status { get; set; }
        public string message { get; set; }
        public string hidden_message { get; set; }
    }
    public class fetchPromoResponse
    {
        public List<SlotPromoItem> promoList { get; set; }
        public string message { get; set; }
    }
}