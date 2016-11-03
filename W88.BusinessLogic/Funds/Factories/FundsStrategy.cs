using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W88.BusinessLogic.Accounts.Models;
using W88.BusinessLogic.Base.Helpers;
using W88.BusinessLogic.Funds.Factories.Handlers;
using W88.BusinessLogic.Funds.Helpers;
using W88.BusinessLogic.Funds.Models;
using W88.BusinessLogic.Shared.Helpers;
using W88.BusinessLogic.Shared.Models;

namespace W88.BusinessLogic.Funds.Factories
{
    public class FundsStrategy
    {
        public static async Task<FundsBase> Initialize(UserSessionInfo userInfo, FundsInfo fundsInfo, PaymentSettingInfo settings)
        {
            switch (settings.Id)
            {
                case "120231":
                    return new BofoPayHandler(userInfo, fundsInfo, settings);

                case "120214":
                case "220815":
                    return new NetellerHandler(userInfo, fundsInfo, settings);

                case "110101":
                case "210602":
                    return new BankTransferHandler(userInfo, fundsInfo, settings, await new ListOfValuesHelper().GetMemberBankAccounts(userInfo));

                case "110308":
                case "210709":
                    return new WingMoneyHandler(userInfo, fundsInfo, settings);

                case "120218":
                    return new ECPSSHandler(userInfo, fundsInfo, settings);

                case "120227":
                    return new Help2PayHandler(userInfo, fundsInfo, settings);

                case "120204":
                    return new NextPayHandler(userInfo, fundsInfo, settings);

                case "120223":
                    return new SDPayHandler(userInfo, fundsInfo, settings);

                case "120254":
                    return new SDAPayAlipayHandler(userInfo, fundsInfo, settings);

                case "120236":
                    return new AllDebitHandler(userInfo, fundsInfo, settings);

                case "120265":
                    return new EGHLHandler(userInfo, fundsInfo, settings);

                case "120212":
                    return new NganLuongHandler(userInfo, fundsInfo, settings);

                default:
                    return null;
            }
        }
    }
}
