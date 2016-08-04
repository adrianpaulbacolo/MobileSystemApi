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

    public List<SlotPromoItem> getPromo(DateTime start, DateTime end)
    {
        var riskId = commonVariables.GetSessionVariable("RiskId");
        if (string.IsNullOrEmpty(riskId)) riskId = "N";

        var promoList = new List<SlotPromoItem>();

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
                promoList.Add(createSlot(promo));
            }

        }
        return promoList.OrderBy(x => x.start).ToList();
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
                if (!string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId))
                {
                    var slotType = string.Empty;
                    switch (commonVariables.SelectedLanguage)
                    {
                        case "zh-cn":
                            lang = "zh";
                            break;
                        default:
                            lang = "en";
                            break;
                    }

                    if (commonCulture.ElementValues.getResourceString("SlotType", xmlGame) != "")
                    {
                        slotType = Convert.ToString(commonCulture.ElementValues.getResourceString("SlotType", xmlGame));
                    }
                    if (slotType == "RSLOT")
                    {
                        game.game_link = commonClubBravado.getRealUrl_mrslot
                            .Replace("{GAME}", Convert.ToString(xmlGame.Name))
                            .Replace("{LANG}", lang)
                            .Replace("{TOKEN}", commonVariables.CurrentMemberSessionId);
                    }
                    else
                    {
                        game.game_link = commonClubBravado.getRealUrl
                            .Replace("{GAME}", Convert.ToString(xmlGame.Name))
                            .Replace("{LANG}", lang)
                            .Replace("{TOKEN}", commonVariables.CurrentMemberSessionId);
                    }

                }
                else
                {
                    game.game_link = "/_Secure/Login.aspx?redirect=" + Uri.EscapeDataString("/ClubBravado");

                }

                game.image_link = "/_Static/Images/ClubBravado/"
                    + commonCulture.ElementValues.getResourceString("ImageName", xmlGame) + ".jpg";
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
                if (!string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId))
                {
                    Uri myUri = new Uri(System.Web.HttpContext.Current.Request.Url.ToString());
                    game.game_link = commonClubDivino.getRealUrl.Replace("{GAMEID}", gameCode)
                        .Replace("{LANG}", lang)
                        .Replace("{TOKEN}", commonVariables.CurrentMemberSessionId)
                        .Replace("{HOMEURL}", myUri.Host)
                        .Replace("{CASHIERURL}", myUri.Host);
                }
                var gameName = (gameClub == "betsoft")
                    ? commonCulture.ElementValues.getResourceString("ImageName", xmlGame)
                    : xmlGame.Name;
                game.image_link = "/_Static/Images/ClubDivino/"
                    + gameName + ".jpg";
                break;
            case "png":
            case "isoftbet":
                if (!string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId))
                {
                    game.game_link = commonClubBravado.getThirdPartyRealUrl.Replace("{GAME}", Convert.ToString(gameCode))
                        .Replace("{LANG}", lang)
                        .Replace("{TOKEN}", commonVariables.CurrentMemberSessionId);
                }
                break;
            case "vanguard":
                if (!string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId))
                {
                    game.game_link = commonCulture.ElementValues.getResourceString("PlayForFunURL", xmlGame)
                        .Replace("{FunUrl}", commonClubMassimo.getFunUrl)
                        .Replace("{token}", commonVariables.CurrentMemberSessionId);
                }
                break;
            case "playtech":
                //commonCulture.appData.getRootResource("/Slots/ClubPalazzo.aspx", out xeResources);
                //matchedGame = xeResources.Element("Category").Element(gameCode);
                break;
            default:
                break;
        }
        game.name = promoGame.HtmlGameCode.ToString();
        game.club = gameClub;

        return game;
    }

    public XElement findGame(JsonSlotGamePromoListing promoGame)
    {
        var gameClub = promoGame.ProductCode;
        var gameCode = promoGame.HtmlGameId;
        var gameTitle = promoGame.GameTitle;
        XElement matchedGame = null;
        switch (gameClub)
        {
            case "slot":
                commonCulture.appData.getRootResource("/Slots/ClubBravado.aspx", out xeResources);
                IEnumerable<XElement> m = xeResources.Element("Category").Element("Slot").Elements();
                matchedGame = m.Where(x => String.Equals(x.Name.ToString(), gameCode, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                break;
            case "ags":
                commonCulture.appData.GetRootResourceNonLanguage("/Slots/ClubApollo.aspx", out xeResources);
                IEnumerable<XElement> apollo = xeResources.Element("Category").Element("Slots").Elements();
                matchedGame = apollo.Where(x => gameCode.ToLower().IndexOf(x.Name.ToString()) != -1).FirstOrDefault();
                break;
            case "ctxm":
            case "betsoft":
                // return for now
                commonCulture.appData.getRootResource("/Slots/ClubDivino.aspx", out xeResources);
                foreach (XElement gamesList in xeResources.Element("Category").Elements())
                {
                    XElement slotList = null;
                    if (gameClub == "betsoft")
                    {
                        slotList = gamesList.Element("Betsoft");
                        if (slotList == null) continue;
                        matchedGame = slotList.Elements().Where(
                            x => x.Attributes().Where(a => a.Value == gameCode).FirstOrDefault() != null).FirstOrDefault();
                    }
                    else
                    {
                        slotList = gamesList.Element("CTXM");
                        if (slotList == null) continue;
                        matchedGame = slotList.Elements().Where(x => x.Attributes().Where(a => a.Value.Contains(gameCode)) != null).FirstOrDefault();
                    }
                    if (matchedGame != null) break;
                }
                break;
            case "png":
            case "isoftbet":
                commonCulture.appData.getRootResource("/Slots/ClubGallardo.aspx", out xeResources);
                matchedGame = xeResources.Element("Category").Element(gameCode);
                break;
            case "vanguard":
                commonCulture.appData.getRootResource("/Slots/ClubMassimo.aspx", out xeResources);
                matchedGame = xeResources.Element("Category").Element(gameCode);
                break;
            case "playtech":
                commonCulture.appData.getRootResource("/Slots/ClubPalazzo.aspx", out xeResources);
                matchedGame = xeResources.Element("Category").Element(gameCode);
                break;
            default:
                break;
        }

        return matchedGame;
    }

    public claim createClaim(long promoId, string gameClub)
    {
        var promoClaim = new claim();


        var riskId = commonVariables.GetSessionVariable("RiskId") ?? "N";
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

        switch (response.ClaimStatus)
        {

            case SlotPromoClaimStatus.AutoApproved:
                promoClaim.message = "Auto Approved";
                promoClaim.hidden_message = promoClaim.message;
                break;
            case SlotPromoClaimStatus.ForApproval:
                promoClaim.message = "for Approval";
                promoClaim.hidden_message = claimInfo.Message;
                break;
            case SlotPromoClaimStatus.Pending:
                promoClaim.message = "Pending";
                promoClaim.hidden_message = claimInfo.Message;
                break;
            default:
                switch (response.info.ErrorCode)
                {
                    case 20:
                        promoClaim.message = promoClaim.hidden_message = claimInfo.Message;
                        break;
                    case 21:
                        promoClaim.message = promoClaim.hidden_message = claimInfo.Message;
                        break;

                    case 90:
                        promoClaim.message = promoClaim.hidden_message = claimInfo.Message;
                        break;
                    default:
                        promoClaim.message = promoClaim.hidden_message = string.Format("{0}: {1}", claimInfo.ErrorCode, claimInfo.Message);
                        break;
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
        if (string.IsNullOrEmpty(info.message)) info.message = info.svc_error;

        if (stakeInfo.ErrorCode == 1 || stakeInfo.ErrorCode == 0)
        {
            info.total_stake = response.TotalStake;
            info.bonus_amount = response.ClaimAmount;
            info.rollover_amount = response.RolloverAmount;
            info.total_win_lost = response.TotalStake;

            if (response.SlotPromoSetup != null)
            {
                info.rollover = response.SlotPromoSetup.Rollover;
                info.min_amount = response.SlotPromoSetup.Stake;
            }
        }


        return info;
    }
    public class SlotPromoGame
    {
        public string instruction { get; set; }
        public string game_link { get; set; }
        public string image_link { get; set; }
        public string name { get; set; }
        public string club { get; set; }
        public string minBonus { get; set; }
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
}