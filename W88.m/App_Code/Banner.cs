using Factories.Slots;
using Helpers;
using Helpers.GameProviders;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;

/// <summary>
/// Summary description for Banner
/// </summary>
public class Banner
{
    private XElement promoResource;
    public string slider = string.Empty;
    private string BannerTemplate = string.Empty;
    private int _deviceId;
    public Banner(int deviceId)
    {
        _deviceId = deviceId;
        
        string languageCode = commonVariables.SelectedLanguage;

        if (commonVariables.CDNCountryCode.Equals("MY", StringComparison.OrdinalIgnoreCase))
        {
            switch (languageCode)
            {
                case "en-us":
                    languageCode = "en-my";
                    break;
                case "zh-cn":
                    languageCode = "zh-my";
                    break;
            }
        }

        commonCulture.appData.GetRootResourceNonLanguage(string.Format("/Shared/Banners/Banners.{0}", languageCode), out promoResource);
        BannerTemplate = "<a href=\"{link}\" class=\"slick-slide\">" +
            "<img src=\"{img}\" alt=\"\">" +
        "</a>";
        SetBanners();
    }

    public string GetBanners()
    {
        return slider;
    }

    public void setTemplate(string CustomTemplate)
    {
        BannerTemplate = CustomTemplate;
        slider = string.Empty;
        SetBanners();
    }

    private void SetBanners()
    {

        try
        {
            IEnumerable<System.Xml.Linq.XElement> promoNode = promoResource.Element("PromoBanner").Elements();
            foreach (System.Xml.Linq.XElement promo in promoNode)
            {
                var imageRoot = "/_Static/Images/promo-banner/";
                var imageSrc = promo.Element("imageSrc").Value;
                var url = promo.Element("url").Value;
                var mainText = promo.Element("title").Value;
                var descText = promo.Element("description").Value;
                var linkClass = promo.Element("class").Value;
                var content = "";
                var description = "";

                try
                {
                    if (promo.Attribute("PromoStart") != null)
                    {
                        var promoStart = promo.Attribute("PromoStart").Value;
                        DateTime start = DateTime.Parse(promoStart, null);
                        if (start > DateTime.Now)
                            continue;

                    }

                    if (promo.Attribute("PromoEnd") != null)
                    {
                        var promoEnd = promo.Attribute("PromoEnd").Value;
                        DateTime end = DateTime.Parse(promoEnd, null);
                        if (end <= DateTime.Now)
                            continue;
                    }

                }
                catch (Exception e) { }

                if (promo.Attribute("Id") != null)
                {
                    var provider = string.Empty;
                    if (promo.Attribute("provider") != null)
                        provider = promo.Attribute("provider").Value;

                    url = BuildUrl(promo, provider);
                }

                if (promo.HasAttributes && promo.Attribute("deviceId") != null)
                {
                    if (_deviceId  == 3)
                    {
                        if (Convert.ToInt16(promo.Attribute("deviceId").Value) != 2)
                            continue;
                    }
                    else
                    {
                        if (Convert.ToInt16(promo.Attribute("deviceId").Value) != _deviceId)
                            continue;
                    }
                }

                var hasCurrency = (promo.HasAttributes && promo.Attribute("currency") != null);
                var isPublic = (promo.HasAttributes && promo.Attribute("public") != null);

                if (hasCurrency && !string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId))
                {
                    var currencies = promo.Attribute("currency").Value;
                    if (!string.IsNullOrEmpty(currencies))
                    {
                        if (string.IsNullOrEmpty(commonCookie.CookieCurrency)) continue;
                        var currenciesArr = currencies.ToString().Split(',');
                        var test = Array.Find(currenciesArr, element => element.StartsWith(commonCookie.CookieCurrency, StringComparison.Ordinal));
                        if (string.IsNullOrEmpty(test)) continue;
                    }
                }
                if (isPublic && string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId))
                {
                    var publicAttr = promo.Attribute("public").Value;
                    if (!string.IsNullOrEmpty(publicAttr))
                    {
                        if (publicAttr != "1")
                        {
                            continue;
                        }
                    }
                }

                StringBuilder sliderTemplate = new StringBuilder(BannerTemplate);
                sliderTemplate.Replace("{link}", CheckDomain(url));
                sliderTemplate.Replace("{img}", imageRoot + imageSrc);
                sliderTemplate.Replace("{description}", description);
                sliderTemplate.Replace("{content}", content);
                sliderTemplate.Replace("{description}", descText);

                slider += sliderTemplate.ToString();
            }
        }
        catch (Exception)
        {
        }


    }

    private string BuildUrl(XElement element, string provider)
    {

        if (provider.ToLower() == GameProvider.GPI.ToString().ToLower())
        {
            var funUrl = GameSettings.GetGameUrl(GameProvider.GPI, GameLinkSetting.Fun);
            var realUrl = GameSettings.GetGameUrl(GameProvider.GPI, GameLinkSetting.Real);

            var gpi = new Gpi(new GameLinkInfo
            {
                Fun = funUrl,
                Real = realUrl,
                MemberSessionId = commonVariables.CurrentMemberSessionId
            });

            return string.IsNullOrWhiteSpace(commonVariables.CurrentMemberSessionId)
                ? gpi.BuildUrl(funUrl, element, GameLinkSetting.Fun)
                : gpi.BuildUrl(realUrl, element, GameLinkSetting.Real);
        }

        return string.Empty;
    }

    private string CheckDomain(string url)
    {
        if (!url.Contains("{DOMAIN}")) return url;

        url = url.Replace("{DOMAIN}", commonIp.DomainName).Replace("{TOKEN}", !string.IsNullOrWhiteSpace(commonVariables.CurrentMemberSessionId) ? commonVariables.EncryptedCurrentMemberSessionId : "").Replace("{LANG}", commonVariables.SelectedLanguage);
        return url;
    }
}