using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using W88.BusinessLogic.Shared.Helpers;

namespace W88.BusinessLogic.Games.Helpers
{

    public class Featured 
    {
        public List<XElement> Games;
        private readonly string _provider;
        private readonly string _category;

        public Featured(string provider, string category)
        {
            _provider = provider;
            _category = category;
            LoadGames();
        }

        private void LoadGames()
        {
            XElement xeResources = CultureHelpers.AppData.GetRootResourceNonLanguage("/Slots/" + _provider);
            Games =
                xeResources.Elements(_category)
                    .Elements()
                    .Where(
                        cat =>
                            cat.Attribute("Category") != null &&
                            (cat.Attribute("Category").Value.Equals("Feature", StringComparison.OrdinalIgnoreCase) ||
                             cat.Attribute("Category").Value.Equals("Inject", StringComparison.OrdinalIgnoreCase)))
                    .ToList();
        }


    }
}
