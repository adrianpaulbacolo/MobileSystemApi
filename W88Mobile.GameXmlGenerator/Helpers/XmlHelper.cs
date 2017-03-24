using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GameXmlGenerator.Helpers
{
    public sealed class XmlHelper
    {
        public string[] Headers;
        public XElement Providers;

        public XmlHelper(string[] headers, XElement providers)
        {
            Providers = providers;
            Headers = headers;
        }

        public XElement BuildGame(XElement game, string[] columns, string club)
        {
            // Game Id
            var idIndex = GetItemIndex("Game ID");
            if (idIndex != -1 && !string.IsNullOrEmpty(columns[idIndex]))
            {
                game.Add(new XAttribute("Id", FormatText(columns[idIndex])));
            }
            else
            {
                // return if no game id
                return game;
            }

            // Category
            var categories = new List<string>();
            var categoryIndex = GetItemIndex("Category");
            if (categoryIndex != -1 && !string.IsNullOrEmpty(columns[categoryIndex]))
            {
                categories.Add(FormatText(columns[categoryIndex]));
            }
            var progressiveIndex = GetItemIndex("Progressive");
            if (progressiveIndex != -1 && !string.IsNullOrEmpty(columns[progressiveIndex]))
            {
                categories.Add("Progressive");
            }
            game.Add(new XAttribute("Category", string.Join(",", categories.ToArray())));


            // Game Image
            var imageIndex = GetItemIndex("Image Name");
            if (imageIndex != -1 && !string.IsNullOrEmpty(columns[imageIndex]))
            {
                var imageElem = new XElement("Image", new XCData(FormatText(columns[imageIndex])));
                var imageLangIndex = GetItemIndex("Image Language");

                if (imageLangIndex != -1 && !string.IsNullOrEmpty(columns[imageLangIndex]))
                {
                    imageElem.Add(new XAttribute("Languages", FormatText(columns[imageLangIndex].Replace("|", ","))));
                }
                game.Add(imageElem);
            }
            if (club.ToUpper() == "BS")
            {
                var androidIndex = GetItemIndex("AndroidId");
                if (androidIndex != -1 && !string.IsNullOrEmpty(columns[androidIndex]))
                {
                    game.Add(new XAttribute("AndroidId", FormatText(columns[androidIndex])));
                }

                var wpIndex = GetItemIndex("WPId");
                if (wpIndex != -1 && !string.IsNullOrEmpty(columns[wpIndex]))
                {
                    game.Add(new XAttribute("WPId", FormatText(columns[wpIndex])));
                }

                var iosIndex = GetItemIndex("IOSId");
                if (iosIndex != -1 && !string.IsNullOrEmpty(columns[iosIndex]))
                {
                    game.Add(new XAttribute("IOSId", FormatText(columns[iosIndex])));
                }
            }
            // Type
            var typeIndex = GetItemIndex("Type");
            if (typeIndex != -1 && !string.IsNullOrEmpty(columns[typeIndex]))
            {
                game.Add(new XAttribute("Type", FormatText(columns[typeIndex])));

            }
            // LanguageCode providers etc

            var langIndex = GetItemIndex("LanguageCode");
            var langCode = (langIndex != -1 && !string.IsNullOrEmpty(columns[langIndex])) ? columns[langIndex] : "";
            var extIndex = GetItemIndex("External Provider");
            var externalProvider = (extIndex != -1 && !string.IsNullOrEmpty(columns[extIndex])) ? columns[extIndex] : "";
            var dcIndex = GetItemIndex("Disabled Currency");
            var disabledCurrency = (dcIndex != -1 && !string.IsNullOrEmpty(columns[dcIndex])) ? columns[dcIndex] : "";

            if (club.ToUpper() == "QT")
            {
                if (!string.IsNullOrEmpty(externalProvider))
                {
                    game.Add(new XAttribute("Provider", externalProvider));
                    AddProvider(externalProvider, langCode, disabledCurrency);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(langCode)) game.Add(new XAttribute("LanguageCode", langCode.Replace("|", ",")));
            }

            // Release Date
            var releaseIndex = GetItemIndex("Release Date");
            if (releaseIndex != -1 && !string.IsNullOrEmpty(columns[releaseIndex]))
            {
                game.Add(new XAttribute("ReleaseDate", FormatText(columns[releaseIndex])));

            }
            // Lines
            var lineIndex = GetItemIndex("Play Lines");
            if (lineIndex != -1 && !string.IsNullOrEmpty(columns[lineIndex]))
            {
                game.Add(new XAttribute("Lines", FormatText(columns[lineIndex])));

            }
            // Section
            var topIndex = GetItemIndex("Top");
            if (topIndex != -1 && !string.IsNullOrEmpty(columns[topIndex]))
            {
                game.Add(new XElement("Top", new XCData(FormatText(columns[topIndex]))));
            }
            var newIndex = GetItemIndex("New");
            if (newIndex != -1 && !string.IsNullOrEmpty(columns[newIndex]))
            {
                game.Add(new XElement("New", new XCData(FormatText(columns[newIndex]))));
            }
            var homeIndex = GetItemIndex("Slot Page");
            if (homeIndex != -1 && !string.IsNullOrEmpty(columns[homeIndex]))
            {
                game.Add(new XElement("Home", new XCData(FormatText(columns[homeIndex]))));
            }

            var i18n = new XElement("i18n");

            // Fun/Real Urls
            var funUrIndex = GetItemIndex("Fun");
            if (funUrIndex != -1 && !string.IsNullOrEmpty(columns[funUrIndex]))
            {
                game.Add(new XElement("Fun", new XCData(FormatText(columns[funUrIndex]))));
            }
            var realUrlIndex = GetItemIndex("Real");
            if (realUrlIndex != -1 && !string.IsNullOrEmpty(columns[realUrlIndex]))
            {
                game.Add(new XElement("Real", new XCData(FormatText(columns[realUrlIndex]))));
            }

            // Title
            var titleIndex = GetItemIndex("Game Name");
            if (titleIndex != -1 && !string.IsNullOrEmpty(columns[titleIndex]))
            {
                game.Add(new XElement("Title", new XCData(FormatText(columns[titleIndex]))));
                i18n.Add(new XElement("en-us", new XCData(FormatText(columns[titleIndex]))));
            }

            //translations
            var cnIndex = GetItemIndex("CN Name");
            if (cnIndex != -1 && !string.IsNullOrEmpty(columns[cnIndex]))
            {
                i18n.Add(new XElement("zh-cn", new XCData(FormatText(columns[cnIndex]))));
            }
            var indoIndex = GetItemIndex("ID Name");
            if (indoIndex != -1 && !string.IsNullOrEmpty(columns[indoIndex]))
            {
                i18n.Add(new XElement("id-id", new XCData(FormatText(columns[indoIndex]))));
            }
            var jpIndex = GetItemIndex("JP Name");
            if (jpIndex != -1 && !string.IsNullOrEmpty(columns[jpIndex]))
            {
                i18n.Add(new XElement("ja-jp", new XCData(FormatText(columns[jpIndex]))));
            }
            var krIndex = GetItemIndex("KR Name");
            if (krIndex != -1 && !string.IsNullOrEmpty(columns[krIndex]))
            {
                i18n.Add(new XElement("ko-kr", new XCData(FormatText(columns[krIndex]))));
            }
            var thIndex = GetItemIndex("TH Name");
            if (thIndex != -1 && !string.IsNullOrEmpty(columns[thIndex]))
            {
                i18n.Add(new XElement("th-th", new XCData(FormatText(columns[thIndex]))));
            }

            game.Add(i18n);

            // Betting Lines
            if (club.ToUpper() == "GPI")
            {
                var minBet = new XElement("MinBet");
                var minBetHeaders = getMinBetHeaders();
                foreach (var mbh in minBetHeaders)
                {
                    var minbetIndex = GetItemIndex(mbh.Value);
                    if (minbetIndex != -1 && !string.IsNullOrEmpty(columns[minbetIndex]))
                    {
                        var mbhElement = new XElement("Bet", new XCData(FormatText(columns[minbetIndex])));
                        mbhElement.Add(new XAttribute("id", FormatText(mbh.Key)));
                        minBet.Add(mbhElement);
                    }
                }
                game.Add(minBet);
            }

            // Publish
            var publishIndex = GetItemIndex("Publish");
            var publishValues = (publishIndex != -1 && !string.IsNullOrEmpty(columns[publishIndex])) ? columns[publishIndex] : "";
            game = AddPublish(publishValues, game);

            return game;
        }

        public string GetClub(string filename)
        {
            try
            {
                return filename.Split('.')[0];
            }
            catch (Exception e)
            {
                return "";
            }
        }

        private int GetItemIndex(string key)
        {
            //var attrib = new XAttribute(key, FetchAttribute(header));
            var headerLower = Headers.Select(s => s.Trim().ToLowerInvariant()).ToArray();
            return Array.IndexOf(headerLower, key.ToLowerInvariant());
        }

        private string FormatText(string content)
        {
            content = content.Replace("[comma]", ",");
            return content;
        }

        private XElement AddPublish(string provider, XElement game)
        {

            if (string.IsNullOrEmpty(provider)) return game;

            // Publish
            var publishIndex = GetItemIndex("Publish");
            if (publishIndex != -1 && !string.IsNullOrEmpty(provider))
            {
                var publishList = provider.Split(';');
                var publishElem = new XElement("Publish");
                foreach (var publishItem in publishList)
                {
                    var item = publishItem.Split('|');
                    var publisher = item[0];
                    var section = item[1];
                    var sortOrder = item[2];

                    if (publishElem.Element(publisher) != null)
                    {
                        publishElem.Element(publisher).Add(new XElement(section, new XCData(sortOrder)));
                    }
                    else
                    {
                        var publisherElem = new XElement(publisher);
                        publisherElem.Add(new XElement(section, new XCData(sortOrder)));
                        publishElem.Add(publisherElem);
                    }
                }
                game.Add(publishElem);
            }

            return game;

        }

        private void AddProvider(string provider, string lang, string disabledCurrency)
        {
            var dcElem = new XElement("NotSupportedCurrency", new XCData(disabledCurrency.Replace("|", ",")));
            var langElem = new XElement("LanguageCode", new XCData(lang.Replace("|", ",")));
            if (Providers.Element(provider) == null)
            {
                var providerElem = new XElement(provider);
                providerElem.Add(dcElem);
                providerElem.Add(langElem);
                Providers.Add(providerElem);
            }
        }

        private Dictionary<string, string> getMinBetHeaders()
        {
            var headers = new Dictionary<string, string>();
            headers.Add("USD", "Min Bet (USD)");
            headers.Add("RMB", "Min Bet (RMB)");
            headers.Add("VND", "Min Bet (VND)");
            headers.Add("THB", "Min Bet (THB)");
            headers.Add("MYR", "Min Bet (MYR)");
            headers.Add("IDR", "Min Bet (IDR)");
            headers.Add("KRW", "Min Bet (KRW)");
            headers.Add("JPY", "Min Bet (JPY)");
            headers.Add("AUD", "Min Bet (AUD)");

            return headers;
        }
    }
}
