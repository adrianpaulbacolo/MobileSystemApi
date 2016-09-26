using System;
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
                    return new CTXMHandler(user.Token);

                case "GPI":
                    return new GPIHandler(user.Token);

                case "PNG":
                    return new PNGHandler(user.Token, lobby);

                case "QT":
                    return new QTHandler(user.Token, lobby);

                case "ISB":
                    return new ISBHandler(user.Token, lobby, user.CurrencyCode);

                case "MGS":
                    return new MGSHandler(user.Token, lobby, cashier);

                case "BS":
                    GameDevice gameDevice = GameDevice.ANDROID;

                    Enum.TryParse(device, true, out gameDevice);

                    return new BSHandler(user.Token, lobby, cashier, gameDevice);

                case "PT":
                    return new PTHandler(user.MemberCode, lobby, liveChat, logout);

                default:
                    return null;
            }
        }
    }
}