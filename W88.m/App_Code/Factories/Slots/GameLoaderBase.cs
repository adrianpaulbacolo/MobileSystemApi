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

        public List<CategoryInfo> Process()
        {
            string currencyCode = commonVariables.GetSessionVariable("CurrencyCode");
           
            return LoadCategory(currencyCode);
        }

        private List<CategoryInfo> LoadCategory(string currencyCode)
        {
            var categories = new List<CategoryInfo>();

            foreach (XElement xeCategory in xeResources.Elements())
            {
                if (IsLangNotSupported(xeCategory)) continue;

                if (IsCurrNotSupported(currencyCode, xeCategory)) continue;

                var category = new CategoryInfo();

                category.Title = GetHeadTranslation(xeCategory);

                List<XElement> topgames = xeCategory.Elements().Where(m => m.Attribute("Top") != null).OrderBy(f => f.Attribute("Top").Value).ToList();

                IEnumerable<XElement> sortedGame = xeCategory.Elements().Where(m => m.Attribute("Top") == null).OrderBy(game => game.Element("Title").Value);

                topgames.AddRange(sortedGame);

                var games = new List<GameInfo>();

                foreach (XElement xeGame in topgames)
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

                category.Games = games;

                categories.Add(category);
            }
            
            return categories;
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