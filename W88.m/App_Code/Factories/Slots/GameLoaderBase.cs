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
        private GameProvider gameProvider { get; set; }
        private XElement xeResources = null;

        public GameLoaderBase(GameProvider gameProvider)
        {
            gameProvider = gameProvider;

            langCode = SetLanguageCode();

            commonCulture.appData.GetRootResourceNonLanguage("/Slots/" + gameProvider, out xeResources);
        }

        protected virtual string SetLanguageCode()
        {
            return commonVariables.SelectedLanguage;
        }

        protected abstract string CreateRealUrl(XElement element);

        protected abstract string CreateFunUrl(XElement element);

        public List<GameCategoryInfo> Process()
        {
            string currencyCode = commonVariables.GetSessionVariable("CurrencyCode");

            return LoadCategory(currencyCode);
        }

        private List<GameCategoryInfo> LoadCategory(string currencyCode)
        {
            var gameCategories = new List<GameCategoryInfo>();

            foreach (XElement xeCategory in xeResources.Elements())
            {
                if (IsLangNotSupported(xeCategory)) continue;

                if (IsCurrNotSupported(currencyCode, xeCategory)) continue;

                var gameCategory = new GameCategoryInfo();

                gameCategory.Title = GetHeadTranslation(xeCategory);

                List<XElement> newGames = xeCategory.Elements().Where(cat => cat.Attribute("Category") != null && cat.Attribute("Category").Value.Equals("new", StringComparison.OrdinalIgnoreCase))
                                                                .OrderBy(game => game.Element("Title").Value).ToList();

                gameCategory.New = AddGamesPerCategory(currencyCode, newGames);

                List<XElement> currentGames = xeCategory.Elements().Where(cat => cat.Attribute("Category") == null ).OrderBy(game => game.Element("Title").Value).ToList();
                gameCategory.Current = AddGamesPerCategory(currencyCode, currentGames);

                gameCategories.Add(gameCategory);
            }

            return gameCategories;
        }

        private List<GameInfo> AddGamesPerCategory(string currencyCode, List<XElement> xeGames)
        {
            var games = new List<GameInfo>();

            foreach (XElement xeGame in xeGames)
            {
                if (IsLangNotSupported(xeGame)) continue;

                if (IsCurrNotSupported(currencyCode, xeGame)) continue;

                var game = new GameInfo();

                game.Title = commonCulture.ElementValues.getResourceString("Title", xeGame);
                game.Image = commonCulture.ElementValues.getResourceString("Image", xeGame);
                game.RealUrl = CreateRealUrl(xeGame);
                game.FunUrl = CreateFunUrl(xeGame);

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

        private bool IsLangNotSupported(XElement element)
        {
            string langNotSupp = element.Attribute("NotSupportedLanguage") != null ? element.Attribute("NotSupportedLanguage").Value : "";
            string[] languages = langNotSupp.Split(',');

            return languages.Contains(commonVariables.SelectedLanguage);
        }

        private bool IsCurrNotSupported(string currencyCode, XElement element)
        {
            if (string.IsNullOrWhiteSpace(currencyCode))
                return false;

            string currNotSupp = element.Attribute("NotSupportedCurrency") != null ? element.Attribute("NotSupportedCurrency").Value : "";
            string[] currencies = currNotSupp.Split(',');

            return currencies.Contains(currencyCode);
        }
    }

    public enum GameProvider
    {
        GPI,
        PT,
        MGS,
        PNG,
        ISB,
        QTECH,
        BS,
        CTXM
    }
}