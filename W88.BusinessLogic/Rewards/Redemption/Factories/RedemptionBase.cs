using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using W88.BusinessLogic.Base.Helpers;
using W88.BusinessLogic.Rewards.Helpers;
using W88.BusinessLogic.Rewards.Models;
using W88.BusinessLogic.Shared.Models;
using W88.Utilities.Log.Helpers;
using W88.WebRef.RewardsServices;
using RedemptionRequest = W88.BusinessLogic.Rewards.Redemption.Model.RedemptionRequest;

namespace W88.BusinessLogic.Rewards.Redemption.Factories
{
    public abstract class RedemptionBase : BaseHelper
    {
        protected RedemptionRequest Request = null;
        protected RewardsHelper RewardsHelper = new RewardsHelper();

        protected RedemptionBase(RedemptionRequest request)
        {
            Request = request;
            Request.OperatorId = OperatorId.ToString(CultureInfo.InvariantCulture);           
        }

        public async Task<ProcessCode> Redeem()
        {
            var process = new ProcessCode();
            process.Id = Guid.NewGuid();

            try
            {
                if (!Validate(process))
                {
                    return process;
                }

                process.ProcessSerialId += 1;
                process.Data = await ProcessRedemption();
                process = await EvaluateResult(process);
                LogRedemption(process);
                return process;
            }
            catch (Exception exception)
            {
                AuditTrail.AppendLog(exception);
                process.Code = (int) Constants.StatusCode.Error;
                process.Message = RewardsHelper.GetTranslation(TranslationKeys.Errors.Exception);
                return process;
            }
        }

        protected abstract Task<RedemptionResponse> ProcessRedemption();

        protected async Task<ProcessCode> EvaluateResult(ProcessCode process)
        {
            var response = (RedemptionResponse)process.Data;
            switch (response.Result)
            {
                case RedemptionResultEnum.ConcurrencyDetected:
                    process.Code = (int)Constants.StatusCode.Error;
                    process.Message = RewardsHelper.GetTranslation(TranslationKeys.Redemption.Failed);
                    break;
                case RedemptionResultEnum.LimitReached:
                    process.Code = (int)Constants.StatusCode.Error;
                    process.Message = RewardsHelper.GetTranslation(TranslationKeys.Redemption.LimitReached);
                    break;
                case RedemptionResultEnum.VIPSuccessLimitReached:
                    process.Code = (int)Constants.StatusCode.Error;
                    process.Message = RewardsHelper.GetTranslation(TranslationKeys.Redemption.BirthdayItemRedeemed);
                    break;
                case RedemptionResultEnum.VIPProcessingLimitReached:
                    process.Code = (int)Constants.StatusCode.Error;
                    process.Message = RewardsHelper.GetTranslation(TranslationKeys.Redemption.InsufficientPoints);
                    break;
                case RedemptionResultEnum.PointIsufficient:
                    process.Code = (int)Constants.StatusCode.Error;
                    process.Message = RewardsHelper.GetTranslation(TranslationKeys.Redemption.InsufficientPoints);
                    break;
                case RedemptionResultEnum.Success:
                    process.Code = (int)Constants.StatusCode.Success;
                    if (Request.ProductType == ProductTypeEnum.Freebet) //Freebet success
                    {
                        var failedCounter = 0;
                        foreach (var redemptionItemId in response.RedemptionIds)
                        {
                            var mailProcess = await RewardsHelper.SendMail(Request.MemberCode, redemptionItemId.ToString(CultureInfo.InvariantCulture));
                            if (mailProcess.Code == (int)Constants.StatusCode.Error)
                            {
                                failedCounter++;
                            }
                        }
                        if (failedCounter > 0)
                        {
                            process.Message = RewardsHelper.GetTranslation(TranslationKeys.Redemption.EmailSendFail);
                            return process;
                        }
                        process.Message = RewardsHelper.GetTranslation(TranslationKeys.Redemption.SuccessfulRedemption);
                    }
                    else
                    {
                        process.Message = RewardsHelper.GetTranslation(TranslationKeys.Redemption.SuccessfulSubmission);
                    }
                    break;
                case RedemptionResultEnum.UnknownError:
                    process.Code = (int)Constants.StatusCode.Error;
                    process.Message = RewardsHelper.GetTranslation(TranslationKeys.Redemption.Failed);
                    break;
                case RedemptionResultEnum.PointCheckError:
                    process.Code = (int)Constants.StatusCode.Error;
                    process.Message = RewardsHelper.GetTranslation(TranslationKeys.Redemption.Failed);
                    break;
                default:
                    process.Code = (int)Constants.StatusCode.Error;
                    process.Message = RewardsHelper.GetTranslation(TranslationKeys.Redemption.Failed);
                    break;
            }
            return process;
        }

        protected bool Validate(ProcessCode process)
        {
            process.ProcessSerialId += 1;
            var messages = new List<string>();

            if (Request.Quantity < 1)
            {
                messages.Add(RewardsHelper.GetTranslation(TranslationKeys.Redemption.InvalidMinimum));
                process.IsAbort = true;
            }

            switch (Request.ProductType)
            {
                case ProductTypeEnum.Freebet:
                    break;
                case ProductTypeEnum.Online:
                    if (string.IsNullOrEmpty(Request.AimId.Trim()))
                    {
                        messages.Add(RewardsHelper.GetTranslation(TranslationKeys.Redemption.EnterAccount));
                        process.IsAbort = true;
                    }
                    break;
                default:
                    if (Request.ProductType != ProductTypeEnum.Normal && Request.ProductType != ProductTypeEnum.Wishlist)
                    {
                        break;
                    }
                    if (string.IsNullOrEmpty(Request.Name.Trim()))
                    {
                        messages.Add(RewardsHelper.GetTranslation(TranslationKeys.Redemption.EnterName));
                        process.IsAbort = true;
                    }
                    if (string.IsNullOrEmpty(Request.ContactNumber.Trim()))
                    {
                        messages.Add(RewardsHelper.GetTranslation(TranslationKeys.Redemption.EnterContactNumber));
                        process.IsAbort = true;
                    }
                    if (string.IsNullOrEmpty(Request.Address.Trim()))
                    {
                        messages.Add(RewardsHelper.GetTranslation(TranslationKeys.Redemption.EnterAddress));
                        process.IsAbort = true;
                    }
                    if (string.IsNullOrEmpty(Request.PostalCode.Trim()))
                    {
                        messages.Add(RewardsHelper.GetTranslation(TranslationKeys.Redemption.EnterPostal));
                        process.IsAbort = true;
                    }
                    if (string.IsNullOrEmpty(Request.City.Trim()))
                    {
                        messages.Add(RewardsHelper.GetTranslation(TranslationKeys.Redemption.EnterCity));
                        process.IsAbort = true;
                    }
                    if (string.IsNullOrEmpty(Request.Country.Trim()))
                    {
                        messages.Add(RewardsHelper.GetTranslation(TranslationKeys.Redemption.EnterCountry));
                        process.IsAbort = true;
                    }
                    if (Request.ProductType != ProductTypeEnum.Wishlist)
                    {
                        break;
                    }
                    if (string.IsNullOrEmpty(Request.Remarks.Trim()))
                    {
                        messages.Add(RewardsHelper.GetTranslation(TranslationKeys.Redemption.EnterRemarks));
                        process.IsAbort = true;
                    }
                    break;
            }
            if (process.IsAbort)
            {
                process.Message = string.Join("; ", messages);
                process.Remark = string.Format("Valid: {0}", !process.IsAbort);
                LogError(process);
                return false;
            }
            return true;
        }

        protected void LogError(ProcessCode process)
        {
            AuditTrail.AppendLog(Request.MemberCode, Constants.PageNames.RedeemPage, Constants.TaskNames.RedeemRewards,
                Constants.PageNames.ComponentName, string.Empty, string.Empty, Convert.ToString(process.Code),
                process.Message, process.Remark, Convert.ToString(process.ProcessSerialId), Convert.ToString(process.Id), false);
        }

        protected void LogRedemption(ProcessCode process)
        {
            var response = (RedemptionResponse)process.Data;
            var redeemId = response.RedemptionIds != null ? String.Join("|", response.RedemptionIds.ToArray()) : string.Empty;
            var remark = string.Format("Product Id: {0}; Points Required: {1}; Quantity: {2};", Request.ProductId, Request.PointRequired, Request.Quantity);
            var detail = string.Format("Redeem Result: {0}; RedeemId: {1}; Type: {2};", response.Result, redeemId, Request.ProductType);
            AuditTrail.AppendLog(Request.MemberCode, Constants.PageNames.RedeemPage, Constants.TaskNames.RedeemRewards,
                Constants.PageNames.ComponentName, Convert.ToString(process.Code), detail, string.Empty,
                string.Empty, remark, Convert.ToString(process.ProcessSerialId), Convert.ToString(process.Id), false);
        }
    }
}
