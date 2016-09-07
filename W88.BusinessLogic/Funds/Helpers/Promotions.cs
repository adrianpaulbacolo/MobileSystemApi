using System;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using W88.BusinessLogic.Accounts.Models;
using W88.BusinessLogic.Base.Helpers;
using W88.BusinessLogic.Funds.Models;
using W88.BusinessLogic.Shared.Helpers;
using W88.Utilities;
using W88.Utilities.Extensions;

namespace W88.BusinessLogic.Funds.Helpers
{
    public sealed class Promotions : BaseHelper
    {

        public CheckPromoInfo CheckPromo(UserSessionInfo user, FundTransferInfo.FtRequest funds)
        {
            var dataValues = new System.Collections.Specialized.NameValueCollection
            {
                {"walletId", funds.TransferTo},
                {"operatorId", base.OperatorId.ToString()},
                {"memberCode", user.MemberCode},
                {"transferAmount", funds.TransferAmount.ToString()},
                {"promoCode", funds.PromoCode},
                {"dateTime", DateTime.Now.ToW88StringFormat()}
            };

            byte[] byteResponse = new WebClient().UploadValues(Common.GetAppSetting<string>("CheckPromoUrl"), "POST", dataValues);
            string strResponse = Encoding.UTF8.GetString(byteResponse);
            XElement xmlResult = XElement.Parse(strResponse);

            var promo = new CheckPromoInfo
            {
                StatusCode = xmlResult.Element("statusCode") == null ? string.Empty : Convert.ToString(xmlResult.Element("statusCode").Value),
                BonusAmount = xmlResult.Element("bonusAmount") == null ? string.Empty : Convert.ToString(xmlResult.Element("bonusAmount").Value),
                MinTransferAmount = xmlResult.Element("minTransferAmount") == null ? string.Empty : Convert.ToString(xmlResult.Element("minTransferAmount").Value),
                RolloverAmount = xmlResult.Element("rolloverAmount") == null ? string.Empty : Convert.ToString(xmlResult.Element("rolloverAmount").Value),
            };

            switch (promo.StatusCode)
            {
                case "00":
                    promo.StatusText = GetMessage("FT_BonusAmount") + promo.BonusAmount;
                    break;
                case "103":
                    promo.StatusText = GetMessage("FT_RolloverNotMet");
                    break;
                case "109":
                    promo.StatusText = GetMessage("Promo_PromoAlreadyClaimed");
                    break;
                case "100":
                case "101":
                case "102":
                case "104":
                case "105":
                case "106":
                case "107":
                case "108":
                    promo.StatusText = GetMessage("Promo_InvalidPromo");
                    break;
            }

            return promo;
        }
    }
}
