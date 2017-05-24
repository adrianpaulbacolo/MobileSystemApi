using System.Activities.Expressions;
using Helpers;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Factories.Slots
{
    public abstract class GameLoaderBase
    {
        protected string langCode;
        protected GameProvider GameProvider { get; set; }
        private XElement xeResources = null;
        public GameLinkInfo GameLink = new GameLinkInfo();

        public GameLoaderBase(GameProvider gameProvider)
        {
            this.GameProvider = gameProvider;

            langCode = SetLanguageCode();

            commonCulture.appData.GetRootResourceNonLanguage("/Slots/" + gameProvider, out xeResources);
        }

        protected virtual string SetLanguageCode()
        {
            return commonVariables.SelectedLanguage;
        }

        protected virtual string GetGameId(XElement xeGame)
        {
            return xeGame.Attribute("Id").Value;
        }

        protected abstract string CreateFunUrl(XElement element);

        protected abstract string CreateRealUrl(XElement element);

        public List<GameCategoryInfo> Process(bool incInjectGames = false)
        {
            string currencyCode = commonVariables.GetSessionVariable("CurrencyCode");

            return LoadCategory(currencyCode, incInjectGames);
        }

        private List<GameCategoryInfo> LoadCategory(string currencyCode, bool incInjectGames)
        {
            var gameCategories = new List<GameCategoryInfo>();

            foreach (XElement xeCategory in xeResources.Elements())
            {
                if (IsCurrNotSupported(currencyCode, xeCategory)) continue;

                var gameCategory = new GameCategoryInfo
                {
                    Title = GetHeadTranslation(xeCategory)
                };

                List<XElement> newGames;
                List<XElement> currentGames;

                if (incInjectGames)
                {
                    List<XElement> injectGames = xeCategory.Elements().Where(cat => cat.Attribute("Category") != null && cat.Attribute("Category").Value.Equals("MergeFirstP", StringComparison.OrdinalIgnoreCase)).OrderBy(game => game.Element("Title").Value).ToList();
                    gameCategory.InjectedGames = AddGamesPerCategory(currencyCode, injectGames);

                    newGames = xeCategory.Elements().Where(cat => cat.Attribute("Category") != null && cat.Attribute("Category").Value.Equals("new", StringComparison.OrdinalIgnoreCase)).OrderBy(game => game.Element("Title").Value).ToList();
                gameCategory.New = AddGamesPerCategory(currencyCode, newGames);

                    currentGames = xeCategory.Elements().Where(cat => cat.Attribute("Category") == null).OrderBy(game => game.Element("Title").Value).ToList();
                gameCategory.Current = AddGamesPerCategory(currencyCode, currentGames);
                }
                else
                {
                    newGames = xeCategory.Elements().Where(cat => cat.Attribute("Category") != null && cat.Attribute("Category").Value.Equals("new", StringComparison.OrdinalIgnoreCase) && !cat.Attribute("Category").Value.Equals("MergeFirstP", StringComparison.OrdinalIgnoreCase)).OrderBy(game => game.Element("Title").Value).ToList();
                    gameCategory.New = AddGamesPerCategory(currencyCode, newGames);

                    currentGames = xeCategory.Elements().Where(cat => cat.Attribute("Category") == null || cat.Attribute("Category").Value.Equals("MergeFirstP", StringComparison.OrdinalIgnoreCase)).OrderBy(game => game.Element("Title").Value).ToList();
                    gameCategory.Current = AddGamesPerCategory(currencyCode, currentGames);
                }

                gameCategories.Add(gameCategory);
            }

            return gameCategories;
        }

        private List<GameInfo> AddGamesPerCategory(string currencyCode, List<XElement> xeGames)
        {
            var games = new List<GameInfo>();
            var lang = commonVariables.SelectedLanguage;
            var shortLang = commonVariables.SelectedLanguageShort;

            foreach (XElement xeGame in xeGames)
            {
                if (IsCurrNotSupported(currencyCode, xeGame)) continue;

                var game = new GameInfo();
                var translatedTitle = commonCulture.ElementValues.getResourceXPathString("i18n/" + lang, xeGame);

                game.Title = (!string.IsNullOrEmpty(translatedTitle)) ? translatedTitle : commonCulture.ElementValues.getResourceString("Title", xeGame);
                game.Image = commonCulture.ElementValues.getResourceString("Image", xeGame);
                try
                {
                    var imageElem = xeGame.Element("Image");
                    var supportedImageLang = imageElem.Attribute("Languages").Value.Split(',');
                    bool hasLangSupp = supportedImageLang.Contains(shortLang, StringComparer.OrdinalIgnoreCase);
                    if (hasLangSupp)
                    {
                        game.Image = game.Image.Replace("{LANG}", "-" + shortLang);
                    }
                    else
                    {
                        game.Image = game.Image.Replace("{LANG}", "-EN");
                    }
                }catch(Exception e){
                        game.Image = game.Image.Replace("{LANG}", "-EN");
                }
                game.RealUrl = CreateRealUrl(xeGame);
                game.FunUrl = CreateFunUrl(xeGame);
                game.Id = GetGameId(xeGame);

                if (xeGame.Attribute("Provider") != null && xeGame.Attribute("Provider").Value.Length > 0)
                {
                    string provider = xeGame.Attribute("Provider").Value;
                    game.Provider = (GameProvider)Enum.Parse(typeof(GameProvider), provider);
                }
                else
                {
                    game.Provider = GameProvider;
                }

                games.Add(game);
            }

            return games;
        }

        private string GetHeadTranslation(XElement element)
        {
            string headerText;
            var lang = commonVariables.SelectedLanguage;

            if (string.IsNullOrEmpty(lang))
            {
                headerText = element.Attribute("en-us").Value;
            }
            else
            {
                if (element.Attribute(lang) != null && element.Attribute(lang).Value.Length > 0)
                {
                    headerText = element.Attribute(lang).Value;
                }
                else
                {
                    headerText = element.Attribute("en-us").Value;
                }
            }

            return headerText;
        }

        private bool IsCurrNotSupported(string currencyCode, XElement element)
        {
            if (string.IsNullOrWhiteSpace(currencyCode))
                return false;

            string currNotSupp = element.Attribute("NotSupportedCurrency") != null ? element.Attribute("NotSupportedCurrency").Value : "";
            string[] currencies = currNotSupp.Split(',');

            return currencies.Contains(currencyCode);
        }

        public List<GameInfo> InsertInjectedGames(List<GameCategoryInfo> sourceCategoryInfo, List<GameInfo> destinationList)
        {
            List<GameInfo> injGames = null;
            if (sourceCategoryInfo.Count > 0)
            {
                if (sourceCategoryInfo[0].InjectedGames != null)
                    injGames = sourceCategoryInfo[0].InjectedGames;
            }

            if (injGames != null)
            {
                foreach (var item in injGames)
                {
                    destinationList.Add(item);
                }

                destinationList = new List<GameInfo>(destinationList.OrderBy(x => x.Title));
            }

            return destinationList;
        }

        protected bool IsElementExists(string attribute, XElement element, out string url)
        {
            url = string.Empty;
            if (element.Element(attribute) == null) return false;

            url = element.Element(attribute).Value;
            return true;
        }

    }

     public enum GameLinkSetting
     {
         Fun, Real
     }

    public enum GameProvider
    {
        GPI,
        PT,
        MGS,
        PNG,
        ISB,
        QT,
        BS,
        CTXM,
        UC8,
        PP,
        TTG,
        GNS,
        PLS,
        MRS
    }

    public enum GameDevice
    {
        IOS,
        ANDROID,
        WP
    }
}