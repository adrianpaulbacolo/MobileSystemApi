﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using W88.BusinessLogic.Account.Helpers;
using W88.BusinessLogic.Account.Models;
using W88.BusinessLogic.Accounts.Helpers;
using W88.BusinessLogic.Accounts.Models;
using W88.BusinessLogic.Base.Helpers;
using W88.BusinessLogic.Shared.Helpers;
using W88.BusinessLogic.Shared.Models;
using W88.WebRef.svcFundTransfer;
using W88.Utilities.Extensions;
using W88.BusinessLogic.Funds.Models;
using W88.WebRef.svcPayMember;
using W88.WebRef.svcPayDeposit;
using W88.Utilities.Constant;
using W88.Utilities.Log.Helpers;

namespace W88.BusinessLogic.Funds.Helpers
{
    public sealed class Payments : BaseHelper
    {
        public async Task<FundTransferInfo.FtResponse> FundTransfer(FundTransferInfo.FtRequest funds, UserSessionInfo user)
        {
            var ftResponse = new FundTransferInfo.FtResponse();

            bool error;
            var validateRequest = ValidateTransfer(funds, out error);
            if (error) return validateRequest;

            if (!string.IsNullOrWhiteSpace(funds.PromoCode)) //Checking promo if valid
            {
                var promo = new Promotions().CheckPromo(user, funds);
                if (promo.StatusCode != "00")
                {
                    ftResponse.FtCode = promo.StatusCode;
                    ftResponse.Message = new[] { promo.StatusText };
                    return ftResponse;
                }
            }

            using (var svc = new FundTransferClient())
            {
                var transferRequest = new initiateTransferRequest
                {
                    currencyCode = user.CurrencyCode,
                    memberCode = user.MemberCode,
                    operatorId = OperatorId.ToString(),
                    promoCode = funds.PromoCode,
                    sessionToken = user.Token,
                    siteCode = SiteUrl,
                    transferAmount = Math.Abs(Convert.ToDecimal(funds.TransferAmount)),
                    transferFrom = funds.TransferFrom,
                    transferTo = funds.TransferTo,
                    transferBy = transferOrigin.Mobile,
                    onBehalf = string.Empty
                };

                initiateTransferResponse response = await svc.initiateTransferAsync(transferRequest);

                ftResponse.FtCode = CultureHelpers.ElementValues.GetResourceString("statusCode", response.initiateTransferResult);

                string[] msg;
                switch (ftResponse.FtCode)
                {
                    case "-60":
                        ftResponse.Message = new[] { GetMessage("FT_TransferFailed") };
                        break;
                    case "00":
                        ftResponse.TransferId =
                            Convert.ToInt32(CultureHelpers.ElementValues.GetResourceString("transferId",
                                response.initiateTransferResult));
                        ftResponse.TransferStatus = CultureHelpers.ElementValues.GetResourceString("transferStatus",
                            response.initiateTransferResult);

                        string strPokerAddOn = string.Empty;

                        if (string.Compare(funds.TransferFrom, "6", true) == 0)
                        {
                            strPokerAddOn = user.CurrencyCode + " " + CultureHelpers.ElementValues.GetResourceString("transferAmount", response.initiateTransferResult) + GetMessage("FT_USDDeposited");
                        }
                        else if (string.Compare(funds.TransferTo, "6", true) == 0)
                        {
                            strPokerAddOn = "USD " + DisplaySettings.RoundDown(CultureHelpers.ElementValues.GetResourceString("transferAmount", response.initiateTransferResult), 2) + GetMessage("FT_USDDeposited");
                        }

                        msg = new string[4];
                        msg[0] = GetMessage("FT_TransferSuccess");

                        msg[1] = GetMessage("FT_BalanceWalletFrom").Replace("{walletFrom}", string.Format("{0} => {1}",
                            Convert.ToDecimal(
                                CultureHelpers.ElementValues.GetResourceString("transferFromBalanceBefore",
                                    response.initiateTransferResult)).ToW88StringFormat(),
                            Convert.ToDecimal(
                                CultureHelpers.ElementValues.GetResourceString("transferFromBalanceAfter",
                                    response.initiateTransferResult)).ToW88StringFormat()));

                        msg[2] = GetMessage("FT_BalanceWalletTo")
                            .Replace("{walletTo}",
                                string.Format("{0} => {1}",
                                    Convert.ToDecimal(
                                        CultureHelpers.ElementValues.GetResourceString("transferToBalanceBefore",
                                            response.initiateTransferResult)).ToW88StringFormat(),
                                    Convert.ToDecimal(
                                        CultureHelpers.ElementValues.GetResourceString("transferToBalanceAfter",
                                            response.initiateTransferResult)).ToW88StringFormat()));

                        msg[3] = strPokerAddOn;

                        ftResponse.Message = msg;

                        break;

                    case "12":
                        ftResponse.Message = new[] { GetMessage("FT_InvalidFundTransfer") };
                        break;
                    case "13":
                        ftResponse.Message = new[] { GetMessage("FT_TransferAmountDisallowed") };
                        break;
                    case "51": // "Transfer Declined - Reference ID already in used";
                        ftResponse.Message = new[] { GetMessage("FT_TransferFailed") };
                        break;
                    case "53":
                        msg = new string[2];
                        msg[0] = GetMessage("FT_TransferDeclined");
                        msg[1] = GetMessage("FT_InsufficientFunds");
                        ftResponse.Message = msg;
                        break;
                    case "54":
                        ftResponse.Message = new[] { GetMessage("FT_UnderMaintenance") };
                        break;
                    case "55": // "Transfer Declined - Funds refunded"
                        ftResponse.Message = new[] { GetMessage("ServerError") };
                        break;
                    case "62":
                        ftResponse.Message = new[] { GetMessage("FT_FundOutLimit") };
                        break;
                    case "63":
                        ftResponse.Message = new[] { GetMessage("FT_FundInLimit") };
                        break;
                    case "64":
                        ftResponse.Message = new[] { GetMessage("FT_FundOutLimitReq") };
                        break;
                    case "65":
                        ftResponse.Message = new[] { GetMessage("FT_FundInLimitReq") };
                        break;
                    case "70":
                        msg = new string[2];
                        msg[0] = GetMessage("FT_TransferFailed");
                        msg[1] = GetMessage("ServerError");
                        ftResponse.Message = msg;
                        break;

                    case "100":
                    case "101":
                    case "107":
                    case "108":
                        ftResponse.Message = new[] { GetMessage("Promotion.InvalidPromo") };
                        break;
                    case "102":
                        ftResponse.Message = new[] { GetMessage("FT_MinTransferNotMet") };
                        break;
                    case "105":
                    case "106":
                    case "109":

                        string strTransferAmountAllowed = CultureHelpers.ElementValues.GetResourceString("transferAmountAllowed", response.initiateTransferResult);
                        string strTotalStakeAmount = CultureHelpers.ElementValues.GetResourceString("totalStakeAmount", response.initiateTransferResult);
                        string strRolloverAmount = CultureHelpers.ElementValues.GetResourceString("rolloverAmount", response.initiateTransferResult);
                        decimal decRolloverAmountNeeded = Convert.ToDecimal(strRolloverAmount) - Convert.ToDecimal(strTotalStakeAmount);

                        msg = new string[3];
                        msg[0] = string.Format("{0} ({1})", GetMessage("FT_RolloverNotMet"), ftResponse.FtCode);
                        msg[1] = GetMessage("FT_RolloverAmountNeeded") + DisplaySettings.RoundDown(decRolloverAmountNeeded, 2);
                        msg[3] = GetMessage("FT_TransferAmountAllowed") + DisplaySettings.RoundDown(strTransferAmountAllowed, 2);

                        ftResponse.Message = msg;
                        break;
                    case "103":
                    case "104":
                        ftResponse.Message = new[] { GetMessage("Promotion.PromoAlreadyClaimed") };
                        break;
                    case "ERR01":
                        ftResponse.Message = new[] { GetMessage("FT_ERR01") };
                        break;
                    default:
                        ftResponse.Message = new[] { GetMessage("FT_TransferFailed") };
                        break;

                }

                return ftResponse;
            }
        }

        private FundTransferInfo.FtResponse ValidateTransfer(FundTransferInfo.FtRequest funds, out bool hasError)
        {
            var response = new FundTransferInfo.FtResponse();
            hasError = false;

            if (string.IsNullOrWhiteSpace(funds.TransferFrom))
            {
                response.Message = new[] { GetMessage("FT_SelectTransferFrom") };
                hasError = true;
            }
            else if (string.IsNullOrWhiteSpace(funds.TransferTo))
            {
                response.Message = new[] { GetMessage("FT_SelectTransferTo") };
                hasError = true;
            }
            else if (string.Compare(funds.TransferFrom, funds.TransferTo, true) == 0)
            {
                response.Message = new[] { GetMessage("FT_InvalidFundTransfer") };
                hasError = true;
            }
            else if (funds.TransferAmount <= 0)
            {
                response.Message = new[] { GetMessage("FT_InputTransferAmount") };
                hasError = true;
            }
            else if (Utilities.Security.Validation.IsInjection(Convert.ToString(funds.TransferAmount)))
            {
                response.Message = new[] { GetMessage("FT_TransferAmountDisallowed") };
                hasError = true;
            }
            else if (!Utilities.Security.Validation.IsNumeric(Convert.ToString(funds.TransferAmount)))
            {
                response.Message = new[] { GetMessage("FT_TransferAmountDisallowed") };
                hasError = true;
            }
            else if (Utilities.Security.Validation.IsInjection(funds.PromoCode))
            {
                response.Message = new[] { GetMessage("Promotion.InvalidPromo") };
                hasError = true;
            }

            return response;
        }

        public async Task<ProcessCode> SelectHistoryReport(HistoryInfoRequest historyRequest, UserSessionInfo user)
        {
            var process = new ProcessCode();

            switch (historyRequest.ReportType)
            {
                case ReportType.DepositWidraw:
                    process.Data = await new HistoryReports(historyRequest, user).GetDepositWidrawal<HistoryInfoDepositWidrawResponse>();
                    break;

                case ReportType.FundTransfer:
                    process.Data = await new HistoryReports(historyRequest, user).GetFundTransfer<HistoryInfoFundTransferResponse>();
                    break;

                case ReportType.PromoClaim:
                    process.Data = await new HistoryReports(historyRequest, user).GetPromotionClaim<HistoryInfoPromoClaimResponse>();
                    break;

                case ReportType.RefferalBonus:
                    process.Data = await new HistoryReports(historyRequest, user).GetReferralBonus<HistoryInfoReferralBonusResponse>();
                    break;
            }

            if (process.Data == null)
                process.Message = GetMessage("NoRecords", user.LanguageCode);

            return process;
        }

        public async Task<List<PaymentSettingInfo>> GetPaymentSettings(UserSessionInfo userInfo, string transType, int methodId = 0)
        {
            var paymentSettings = base.GetListOfValues<PaymentSettingInfo>("shared/PaymentSettings", "PaymentGateway", false).FirstOrDefault(x => x.Id == methodId.ToString());
            transType = paymentSettings == null ? transType : paymentSettings.Type;

            Constants.PaymentTransactionType paymentType;
            Enum.TryParse(transType, true, out paymentType);

            var settingInfos = new List<PaymentSettingInfo>();

            using (var svcInstance = new MemberClient())
            {
                var request = new getMethodLimits_MobileRequest
                {
                    operatorId = this.OperatorId.ToString(),
                    memberCode = userInfo.MemberCode,
                    paymentMethodId = methodId.ToString(),
                    isDummy = false,
                    transType = Convert.ToString((int)paymentType)
                };

                getMethodLimits_MobileResponse response = await svcInstance.getMethodLimits_MobileAsync(request);

                if (response.processCode == "00")
                {
                    settingInfos = SetPaymentSettings(userInfo, response.getMethodLimits_MobileResult, paymentType);
                }
            }

            return settingInfos;
        }

        private List<PaymentSettingInfo> SetPaymentSettings(UserSessionInfo userInfo, DataTable pgSettings, Constants.PaymentTransactionType transType)
        {
            var paymentSettings = base.GetListOfValues<PaymentSettingInfo>("shared/PaymentSettings", "PaymentGateway", false);

            var settingInfos = new List<PaymentSettingInfo>();

            foreach (DataRow row in pgSettings.Rows)
            {
                string methodId = row["methodId"].ToString();

                var settingInfo = paymentSettings.FirstOrDefault(x => x.Id == methodId);

                if (settingInfo == null)
                    continue;

                settingInfo.MerchantId = row["merchantId"].ToString();
                settingInfo.PaymentMode = row["paymentMode"].ToString();
                settingInfo.LimitDaily = Convert.ToDecimal(row["totalAllowed"].ToString()) <= 0 ? Constants.VarNames.Unlimited : Convert.ToDecimal(row["totalAllowed"].ToString()).ToW88StringFormat();
                settingInfo.TotalAllowed = Convert.ToDecimal(row["limitDaily"].ToString()) == 0 ? Constants.VarNames.Unlimited : Convert.ToDecimal(row["limitDaily"].ToString()).ToW88StringFormat();

                if (Constants.PaymentTransactionType.Deposit == transType)
                {
                    settingInfo.MinAmount = Convert.ToDecimal(row["minDeposit"].ToString());
                    settingInfo.MaxAmount = Convert.ToDecimal(row["maxDeposit"].ToString());
                }
                else
                {
                    settingInfo.MinAmount = Convert.ToDecimal(row["minWithdrawal"].ToString());
                    settingInfo.MaxAmount = Convert.ToDecimal(row["maxWithdrawal"].ToString());
                }

                settingInfos.Add(settingInfo);
            }

            return settingInfos;
        }

        public async Task<ProcessCode> GetSDAPayCurrentDeposit(long transactionID)
        {
            var process = new ProcessCode();

            using (DepositClient client = new DepositClient())
            {
                DataTable dt = await client.getSDAPayDepositTransactionAsync(transactionID);

                if (dt == null || dt.Rows[0]["state"].ToString() != "0")
                {
                    process.Message = Convert.ToString(Constants.StatusCode.Error);
                    process.Code = (int)Constants.StatusCode.Error;
                }
                else
                {
                    DataRow dr = dt.Rows[0];

                    string eBankAccount = dr["eBankAccount"].ToString();
                    if (eBankAccount.Length == 16)
                        eBankAccount = eBankAccount.Substring(0, 4) + " " + eBankAccount.Substring(4, 4) + " " + eBankAccount.Substring(8, 4) + " " + eBankAccount.Substring(12, 4);

                    var bank = new ListOfValuesHelper().GetBanksList(120254, "").FirstOrDefault(b => b.Value.Equals(dr["eBank"].ToString(), StringComparison.OrdinalIgnoreCase));

                    OperatorSettings operatorSettings = new OperatorSettings(Settings.OperatorName);
                    string bankUrl = operatorSettings.Values.Get("SDAPayAlipayBankUrl");

                    process.Data = new
                    {
                        TransactionId = dr["invId"].ToString(),
                        Amount = Convert.ToDecimal(dr["ePrice"]).ToW88StringFormat(),
                        Bank = bank.Text,
                        AccountName = dr["eName"].ToString(),
                        AccountNumber = eBankAccount,
                        BankUrl = bankUrl
                    };

                    process.Message = Convert.ToString(Constants.StatusCode.Success);
                    process.Code = (int)Constants.StatusCode.Success;
                }
            }

            return process;
        }

        public async Task<ProcessCode> GetDepositStatus(long transactionID)
        {
            var process = new ProcessCode();

            using (DepositClient client = new DepositClient())
            {
                DataTable dt = await client.getDepositTransactionAsync(transactionID);

                if (dt != null)
                {
                    process.Data = new
                    {
                        TransactionId = transactionID,
                        Status = dt.Rows[0]["status"].ToString()
                    };

                    process.Message = Convert.ToString(Constants.StatusCode.Success);
                    process.Code = (int)Constants.StatusCode.Success;

                }
                else
                {
                    process.Message = Convert.ToString(Constants.StatusCode.Error);
                    process.Code = (int)Constants.StatusCode.Error;
                }
            }

            return process;
        }

        public async Task<ProcessCode> GetLastCreditTransaction(string memberCode)
        {
            var process = new ProcessCode();

            var _cardType = new ListOfValuesHelper().GetCardType();

            using (var client = new MemberClient())
            {
                var response = await client.getMemberLastCreditCardDepositDetailAsync(Convert.ToInt64(OperatorId), memberCode);

                if (response != null && response.Rows.Count > 0)
                {
                    DataRow dr = response.Rows[0];
                    process.Data = new
                    {
                        CardType = _cardType.FirstOrDefault(f => f.Text == dr["cardType"].ToString()),
                        CardExpiryMonth = dr["expMonth"].ToString(),
                        CardExpiryYear = dr["expYear"].ToString(),
                        CardName = dr["cardName"] == DBNull.Value ? string.Empty : dr["cardName"].ToString(),
                        CardNumber = dr["cardNumber"] == DBNull.Value ? string.Empty : dr["cardNumber"].ToString()
                    };
                }

                return process;
            }
        }

        public async Task<ProcessCode> GetLastWithdrawalTransaction(string memberCode)
        {
            var process = new ProcessCode();

            using (var client = new MemberClient())
            {
                var response = await client.getLastWithdrawalWireTransferAsync(new getLastWithdrawalWireTransferRequest()
                {
                    operatorId = OperatorId,
                    memberCode = memberCode
                });

                if (response.statusCode == "00")
                {
                    DataRow dr = response.getLastWithdrawalWireTransferResult.Rows[0];

                    string memberMobile = string.IsNullOrWhiteSpace(dr["memberMobile"].ToString()) ? "" : dr["memberMobile"].ToString();

                    string[] mobileNo = memberMobile.Split('-');
                    string countryCode = string.Empty, phone = string.Empty;
                    if (mobileNo.Length == 2)
                    {
                        countryCode = mobileNo[0];
                        phone = mobileNo[1];
                    }

                    process.Data = new
                    {
                        BankId = dr["bankCode"].ToString(),
                        BankName = dr["bankNameNative"].ToString(),
                        BankBranch = dr["bankBranch"].ToString(),
                        BankAddress = dr["bankAddress"].ToString(),
                        AccountName = dr["bankAccountName"].ToString(),
                        AccountNumber = dr["bankAccountNumber"].ToString(),
                        CountryCode = countryCode,
                        Phone = phone
                    };
                }

                return process;
            }
        }

        public async Task<ProcessCode> HasPendingWithdrawal(string memberCode)
        {
            using (var client = new MemberClient())
            {
                var response = await client.getPendingWithdrawalAsync(new getPendingWithdrawalRequest
                {
                    memberCode = memberCode,
                    operatorId = Convert.ToInt64(OperatorId)
                });

                var process = new ProcessCode();
                process.Code = Convert.ToInt32(response.statusCode);

                if (process.Code == 0)
                {
                    var result = response.getPendingWithdrawalResult.ToList();
                    if (result.Count > 0)
                    {
                        process.Code = (int)Constants.StatusCode.Success;
                        process.Data = new PendingWithdrawalInfo
                            {
                                Name = result[0].payMethodDescription,
                                TransactionId = result[0].invId,
                                MethodId = result[0].payMethodId,
                                Amount = result[0].requestAmount,
                                RequestDateTime = result[0].requestDate,
                                Status = result[0].status
                            };
                    }
                }

                return process;
            }
        }

        public async Task<ProcessCode> CancelPendingWithdrawal(string memberCode, PendingWithdrawalInfo pending)
        {
            using (var client = new MemberClient())
            {
                var response = await client.cancelWithdrawalAsync(new cancelWithdrawalRequest
                {
                    invId = pending.TransactionId,
                    memberCode = memberCode,
                    methodId = pending.MethodId,
                    operatorId = Convert.ToInt64(OperatorId)
                });

                var process = new ProcessCode();
                process.Code = Convert.ToInt32(response.statusCode);

                if (process.Code == 0)
                {
                    if (response.cancelWithdrawalResult)
                    {
                        process.Code = (int)Constants.StatusCode.Success;
                        process.Message = base.GetMessage("Pay_Success");
                    }
                    else
                    {
                        process.Code = (int)Constants.StatusCode.Error;
                        process.Message = base.GetMessage("Pay_Fail");
                    }
                }
                else
                {
                    process.Message = base.GetMessage("Pay_Fail");
                }

                process.Id = Guid.NewGuid();
                process.ProcessSerialId += 1;

                AuditTrail.AppendLog(memberCode, Constants.PageNames.FundsPage,
                    Constants.TaskNames.CancelPendingWithdrawal, Constants.PageNames.ComponentName,
                    Convert.ToString(process.Code), process.Message, string.Empty, string.Empty, process.Remark,
                    Convert.ToString(process.ProcessSerialId), Convert.ToString(process.Id), false);

                return process;
            }
        }

        public ProcessCode GetExchangeRate(decimal amount, string currencyFrom, string currencyTo)
        {
            var process = new ProcessCode();

            if (string.IsNullOrWhiteSpace(currencyFrom) && string.IsNullOrWhiteSpace(currencyTo))
                return process;

            using (var client = new MemberClient())
            {
                var response = client.getCurrencyForeignRate(currencyFrom, currencyTo);

                decimal exRate = decimal.Parse(response);
                decimal convertedAmount = amount * exRate;

                process.Code = (int)Constants.StatusCode.Success;
                process.Data = new
                    {
                        Amount = decimal.Parse(convertedAmount.ToW88StringFormat()),
                        ExchangeRate = exRate
                    };

                return process;
            }
        }
    }
}
