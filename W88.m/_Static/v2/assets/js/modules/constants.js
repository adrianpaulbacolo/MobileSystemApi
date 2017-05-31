w88Mobile.Keys = {
    depositSettings: siteCookie.getCookie("s") + "_depositSettings"
    , withdrawalSettings: siteCookie.getCookie("s") + "_withdrawalSettings"
    , switchLineSettings: siteCookie.getCookie("s") + "_switchLineSettings"
    , userSettings: siteCookie.getCookie("s") + "_userSettings"
}

var _constants = {
    API_URL: "/_secure/ajax"
    , IMAGES_URL: "/_static/images"
    , DASHBOARD_URL: "/v2/Dashboard.aspx"
    , LOGIN_URL: "/v2/Account/Login.aspx"
    , REGISTER_URL: "/v2/Account/Rgister.aspx"
    , REGISTER_SUCCESS_URL: "/v2/Account/RgisterSuccess.aspx"
    , FUNDS_URL: "/v2/Funds.aspx"
    , DEPOSIT_URL: "/v2/Deposit/Default.aspx"
    , TRANSFER_URL: "/v2/FundTransfer/Default.aspx"
    , WITHRAW_URL: "/v2/Withdrawal/Default.aspx"
    , HISTORY_URL: "/v2/History/Default.aspx"
    , DOWNLOAD_URL: "/v2/Downloads.aspx"
    , DOWNLOAD_PALAZZO_CASINO_URL: "/v2/Downloads/palazzo-casino"
    , DOWNLOAD_PALAZZO_SLOT_URL: "/v2/Downloads/palazzo-slots"
    , DOWNLOAD_TM_IOS_URL: "/v2/Downloads/texas-mahjong-ios"
    , DOWNLOAD_SB_IOS_URL: "/v2/Downloads/super-bull-ios"
    , DOWNLOAD_CLUBW_IOS_URL: "/v2/Downloads/clubw88-ios"
    , SLOTS_URL: "/v2/Slots"
    , SLOTS_BRAVADO_URL: "/v2/Slots/bravado"
    , SLOTS_MASSIMO_URL: "/v2/Slots/massimo"
    , SLOTS_PALAZZO_URL: "/v2/Slots/palazzo"
    , SLOTS_GALLARDO_URL: "/v2/Slots/gallardo"
    , SLOTS_APOLLO_URL: "/v2/Slots/apollo"
    , SLOTS_NUOVO_URL: "/v2/Slots/nuovo"
    , SLOTS_DIVINO_URL: "/v2/Slots/divino"
    , SPORTS_URL: "/v2/Sports.aspx"
    , LOTTERY_URL: "/v2/Lottery.aspx"
    , PROMO_URL: "/Promotions"
    , A_SPORTS_URL: "http://ismart.w88id.com/Deposit_ProcessLogin.aspx?lang={LANG}&st={TOKEN}".replace("{TOKEN}", siteCookie.getCookie("s"))
    , E_SPORTS_URL: "http://mobile.w88.testing.agent1818.com/?LangID={LANG}&amp;ExternalToken={TOKEN}&amp;oddsstyleid=1".replace("{TOKEN}", siteCookie.getCookie("s"))
    // PROD, E_SPORTS_URL: "https://mappelche.w2sports.com/?LangID={LANG}&amp;ExternalToken={TOKEN}&amp;oddsstyleid=1".replace("{TOKEN}", siteCookie.getCookie("s"))
    , X_SPORTS_FUN_URL: "http://mobile.sb5.w88uat.com/auth.aspx?lang={LANG}&amp;templatename=blue"
    , X_SPORTS_REAL_URL: "http://mobile.sb5.w88uat.com/auth.aspx?lang={LANG}&amp;user={USER}&amp;token={TOKEN}&amp;currency={CURR}&amp;templatename=blue".replace("{TOKEN}", siteCookie.getCookie("s"))
    // PROD, X_SPORTS_FUN_URL: "http://mobile.xabia51.w2sports.com/auth.aspx?lang={LANG}&amp;templatename=blue"
    // PROD, X_SPORTS_REAL_URL: "http://mobile.xabia51.w2sports.com/auth.aspx?lang={LANG}&amp;user={USER}&amp;token={TOKEN}&amp;currency={CURR}&amp;templatename=blue".replace("{TOKEN}", siteCookie.getCookie("s"))
    , V_SPORTS_URL: "/v2/V-Sports.aspx"
    , MASSIMO_URL: "https://livegames.cdn.gameassists.co.uk/AIR/Poria/Installer/V20021/w88/Download.html"
    , MASSIMO_SLOTS_URL: "http://resigner.qfcontent.com/w88.apk"
    , FISHING_MASTER_IOS_URL: siteCookie.getCookie("language") == "zh-cn" && siteCookie.getCookie("language") == "zh-cn" ?
        "itms-services://?action=download-manifest&url=https://s3-ap-southeast-1.amazonaws.com/w88download/fishing/manifest.plist" : "itms-services://?action=download-manifest&url=https://s3-ap-southeast-1.amazonaws.com/w88download/fishing/manifestEN.plist"
    , FISHING_MASTER_APK_URL: siteCookie.getCookie("language") == "zh-cn" && siteCookie.getCookie("language") == "zh-cn" ?
        "https://s3-ap-southeast-1.amazonaws.com/w88download/fishing/FishingMaster.apk" : "https://s3-ap-southeast-1.amazonaws.com/w88download/fishing/FishingMasterEN.apk"
    , POKER_IOS_URL: "itms-services://?action=download-manifest&url=https://dlportal.good-game-network.com/mobile/installer/ios/W88"
    , POKER_APK_URL: "http://dlportal.good-game-network.com/mobile/installer/android/W88"
    , TEXAS_MAHJONG_IOS_URL: "https://tm.gp2play.com/mobile/ios/install.html"
    , TEXAS_MAHJONG_APK_URL: "https://tm.gp2play.com/mobile/android/install.html"
    , SUPER_BULL_IOS_URL: siteCookie.getCookie("language") == "zh-cn" ? "https://tm.gp2play.com/mobileNiuniu/ios/install.html" : "https://tm.gp2play.com/mobileNiuniu/ios/install_en.html"
    , SUPER_BULL_APK_URL: siteCookie.getCookie("language") == "zh-cn" ? "https://tm.gp2play.com/mobileNiuniu/android/install.html" : "https://tm.gp2play.com/mobileNiuniu/android/install_en.html"
    , CLUBW_IOS_URL: "itms-services://?action=download-manifest&amp;url=https://casino.gp2api.com/ios/manifest.plist"
    , CLUBW_APK_URL: "http://casino.w88uat.com/mob/download.html?op=w88"
    // PROD, CLUBW_APK_URL: "https://casino.gp2play.com/mob/download.html?op=w88"
    , PALAZZO_CASINO_APK_URL: "http://mlive.w88palazzo.com"
    , PALAZZO_SLOT_APK_URL: "http://mgames.w88palazzo.com"
    , TERMS_URL: "https://info.w88live.com/termofuse_en.shtml"
    , LIVECHAT_URL: "http://www.{DOMAIN}/common/livechat.ashx?url=".replace("{DOMAIN}", getDomainName()) + location.href
};