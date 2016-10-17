using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using Helpers.GameProviders;
using W88.BusinessLogic.Games.Handlers;
using W88.BusinessLogic.Games.Helpers;
using W88.BusinessLogic.Games.Models;
using W88.BusinessLogic.Shared.Helpers;
using W88.BusinessLogic.Shared.Models;
using W88.Utilities;
using W88.Utilities.Constant;

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

                IEnumerable<XElement> currentGames = xeCategory.Elements();
                var injectValue = CultureHelpers.ElementValues.GetResourceXPathAttribute("Inject", xeCategory);
                var injectProvider = string.IsNullOrWhiteSpace(injectValue) ? null : injectValue.Split(new char[] { ',' });

                if (itemCount <= 0)
                    gameCategory.Games = AddGamesPerCategory(currencyCode, currentGames.ToList(), injectProvider, xeCategory.Name.LocalName).OrderBy(game => game.Title).ToList();
                else
                    gameCategory.Games = AddGamesPerCategory(currencyCode, currentGames.Take(totalCount).ToList(), injectProvider, xeCategory.Name.LocalName).OrderBy(game => game.Title).ToList();
                
                gameCategories.Add(gameCategory);
            }

            return gameCategories;
        }

        private List<GameInfo> AddGamesPerCategory(string currencyCode, List<XElement> xeGames, string[] injectProvider, string elementCategory)
        {
            var games = new List<GameInfo>();
          
            if (injectProvider != null)
            {
                foreach (var prov in injectProvider)
                {
                    var feat = new Featured(prov, elementCategory);
                    foreach (var f in feat.Games)
                    {
                        var pubValue = CultureHelpers.ElementValues.GetResourceXPathAttribute("Publish", f);
                        var pub = string.IsNullOrWhiteSpace(pubValue) ? null : pubValue.Split(new char[] { ',' });
                        if (pub != null && pub.Contains(gameProvider.ToString()))
                        {
                            var gpi = new Gpi(GameLink, LanguageCode);
                            AddGameToList(ref games, f, gpi.CheckRSlot(GameLinkSetting.Real, f), gpi.CheckRSlot(GameLinkSetting.Fun, f));
                        }
                    }
                }
            }

            foreach (XElement xeGame in xeGames)
            {
                if (IsCurrNotSupported(currencyCode, xeGame)) continue;

                AddGameToList(ref games, xeGame, CreateFunUrl(xeGame), CreateRealUrl(xeGame));
            }

            return games;
        }

        private void AddGameToList(ref List<GameInfo> games, XElement element, string funUrl, string realUrl)
        {
            var game = new GameInfo();
            game.Title = CultureHelpers.ElementValues.GetResourceString("Title", element);
            game.Image = CultureHelpers.ElementValues.GetResourceString("Image", element);
            game.ImagePath = this.gamePath + game.Image + this.fileType;
            game.RealUrl = funUrl;
            game.FunUrl = realUrl;

            var catValue = CultureHelpers.ElementValues.GetResourceXPathAttribute("Category", element);
            game.Category = string.IsNullOrWhiteSpace(catValue) ? new string[0] : catValue.Split(new char[] { ',' }); 
            
            var pubValue = CultureHelpers.ElementValues.GetResourceXPathAttribute("Publish", element);
            game.OtherProvider = string.IsNullOrWhiteSpace(pubValue) ? new string[0] : pubValue.Split(new char[] { ',' });

            if (game.OtherProvider.Length == 0 || game.OtherProvider.Contains(gameProvider.ToString()))
            {
                games.Add(game);
            }
        }

        private string GetHeadTranslation(XElement element)
        {
            string headerText;
            var defaultLang = new OperatorSettings(Settings.OperatorName).Values.Get(Constants.VarNames.DefaultLanguage);
            
            if (string.IsNullOrEmpty(LanguageCode))
            {
                headerText = CultureHelpers.ElementValues.GetResourceXPathAttribute(defaultLang, element);
            }
            else
            {
                if (string.IsNullOrEmpty(CultureHelpers.ElementValues.GetResourceXPathAttribute(LanguageCode, element)))
                {
                    headerText = CultureHelpers.ElementValues.GetResourceXPathAttribute(LanguageCode, element);
                }
                else
                {
                    headerText = CultureHelpers.ElementValues.GetResourceXPathAttribute(defaultLang, element);
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

        protected bool IsElementExists(string elementName, XElement element, out string url)
        {
            url = CultureHelpers.ElementValues.GetResourceString(elementName, element);
            return !string.IsNullOrWhiteSpace(url);
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