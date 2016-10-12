using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using W88.BusinessLogic.Games.Handlers;
using W88.BusinessLogic.Games.Models;
using W88.BusinessLogic.Shared.Helpers;
using W88.Utilities;

namespace W88.BusinessLogic.Games.Factories
{
    public abstract class GameLoaderBase
    {
        protected string LanguageCode;
        protected GameProvider gameProvider { get; set; }
        private XElement xeResources = null;
        private string gamePath;
        private string fileType;
        public GameLinkInfo GameLink = new GameLinkInfo();

        protected GameLoaderBase(GameProvider gameProvider, string languageCode)
        {
            this.gameProvider = gameProvider;
            this.gamePath = GameSettings.GamePath;
            // Note: temporary only, if all games goes to v2, then file path will be in xml(probably)
            this.fileType = this.gameProvider == GameProvider.PT ? ".png" : ".jpg";

            LanguageCode = languageCode;
            xeResources = CultureHelpers.AppData.GetRootResourceNonLanguage("/Slots/" + gameProvider);
        }

        protected virtual string SetLanguageCode()
        {
            return LanguageCode;
        }

        protected abstract string CreateFunUrl(XElement element);

        protected abstract string CreateRealUrl(XElement element);

        public List<GameCategoryInfo> Process(string currencyCode, int itemCount = 0, List<string> category = null)
        {
            return LoadCategory(currencyCode, itemCount, category);
        }

        private List<GameCategoryInfo> LoadCategory(string currencyCode, int itemCount, List<string> category)
        {
            var gameCategories = new List<GameCategoryInfo>();

            int totalCount = itemCount;

            foreach (XElement xeCategory in xeResources.Elements())
            {
                if (itemCount != 0 && totalCount == 0)
                    break;

                if (category != null && category.Count() != 0)
                    if (!category.Contains(xeCategory.Name.LocalName, StringComparer.OrdinalIgnoreCase))
                        continue;

                if (IsCurrNotSupported(currencyCode, xeCategory)) continue;

                var gameCategory = new GameCategoryInfo
                {
                    Provider = this.gameProvider.ToString(),
                    Title = GetHeadTranslation(xeCategory)
                };

                IEnumerable<XElement> newGames = xeCategory.Elements().Where(cat => cat.Attribute("Category") != null && cat.Attribute("Category").Value.Equals("new", StringComparison.OrdinalIgnoreCase))
                                                                .OrderBy(game => game.Element("Title").Value);

                int newCount = 0;
                if (itemCount <= 0)
                    gameCategory.New = AddGamesPerCategory(currencyCode, newGames.ToList());
                else
                {
                    gameCategory.New = AddGamesPerCategory(currencyCode, newGames.Take(totalCount).ToList());

                    newCount = newGames.Take(totalCount).Count();
                }

                IEnumerable<XElement> currentGames = xeCategory.Elements().Where(cat => cat.Attribute("Category") == null)
                                                                    .OrderBy(game => game.Element("Title").Value).ToList();

                int currentCount = 0;
                if (itemCount <= 0)
                    gameCategory.Current = AddGamesPerCategory(currencyCode, currentGames.ToList());
                else
                {
                    totalCount = totalCount - newCount;

                    gameCategory.Current = AddGamesPerCategory(currencyCode, currentGames.Take(totalCount).ToList());

                    currentCount = currentGames.Take(totalCount).Count();

                    totalCount = totalCount - currentCount;
                }

                gameCategories.Add(gameCategory);
            }

            return gameCategories;
        }

        private List<GameInfo> AddGamesPerCategory(string currencyCode, List<XElement> xeGames)
        {
            var games = new List<GameInfo>();

            foreach (XElement xeGame in xeGames)
            {
                if (IsCurrNotSupported(currencyCode, xeGame)) continue;

                var game = new GameInfo();

                game.Title = CultureHelpers.ElementValues.GetResourceString("Title", xeGame);
                game.Image = CultureHelpers.ElementValues.GetResourceString("Image", xeGame);
                game.ImagePath = this.gamePath + game.Image + this.fileType;
                game.RealUrl = CreateRealUrl(xeGame);
                game.FunUrl = CreateFunUrl(xeGame);

                games.Add(game);
            }

            return games;
        }

        private string GetHeadTranslation(XElement element)
        {
            string headerText;
            var lang = LanguageHelpers.SelectedLanguage;

            if (string.IsNullOrEmpty(lang))
            {
                headerText = CultureHelpers.ElementValues.GetResourceXPathAttribute("en-us", element);
            }
            else
            {
                if (string.IsNullOrEmpty(CultureHelpers.ElementValues.GetResourceXPathAttribute(lang, element)))
                {
                    headerText = CultureHelpers.ElementValues.GetResourceXPathAttribute(lang, element);
                }
                else
                {
                    headerText = CultureHelpers.ElementValues.GetResourceXPathAttribute("en-us", element);
                }
            }

            return headerText;
        }

        private bool IsCurrNotSupported(string currencyCode, XElement element)
        {
            if (string.IsNullOrWhiteSpace(currencyCode))
                return false;

            string currNotSupp = CultureHelpers.ElementValues.GetResourceXPathAttribute("NotSupportedCurrency", element);
            string[] currencies = currNotSupp.Split(',');

            return currencies.Contains(currencyCode);
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
        UC8
    }

    public enum GameDevice
    {
        IOS,
        ANDROID,
        WP
    }
}