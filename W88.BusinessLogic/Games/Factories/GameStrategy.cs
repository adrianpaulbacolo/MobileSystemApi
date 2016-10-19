using System;
using Factories.Slots.Handlers;
using W88.BusinessLogic.Accounts.Helpers;
using W88.BusinessLogic.Accounts.Models;
using W88.BusinessLogic.Games.Factories.Handlers;

namespace W88.BusinessLogic.Games.Factories
{
    public class GameStrategy
    {
        public static GameLoaderBase Initialize(string gameProvider, UserSessionInfo user, string lobby = "", string cashier = "", string liveChat = "", string logout = "", string device = "")
        {
            switch (gameProvider.ToUpper())
            {
                case "CTXM":
                    return new CTXMHandler(user);

                case "GPI":
                    return new GPIHandler(user);

                case "PNG":
                    return new PNGHandler(user, lobby);

                case "QT":
                    return new QTHandler(user, lobby);

                case "ISB":
                    return new ISBHandler(user, lobby, user.CurrencyCode);

                case "MGS":
                    return new MGSHandler(user, lobby, cashier);

                case "UC8":
                    return new UC8Handler(user, lobby, cashier);

                case "BS":
                    GameDevice gameDevice = GameDevice.ANDROID;

                    Enum.TryParse(device, true, out gameDevice);

                    return new BSHandler(user, lobby, cashier, gameDevice);

                case "PT":
                    return new PTHandler(user, lobby, liveChat, logout);

                case "PP":
                    return new PPHandler(user, lobby, cashier);

                default:
                    return null;
            }
        }
    }
}