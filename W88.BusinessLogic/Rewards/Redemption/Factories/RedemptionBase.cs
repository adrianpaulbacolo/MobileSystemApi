using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using W88.BusinessLogic.Base.Helpers;
using W88.BusinessLogic.Shared.Models;
using W88.Utilities.Log.Helpers;
using W88.WebRef.RewardsServices;
using RedemptionRequest = W88.BusinessLogic.Rewards.Redemption.Model.RedemptionRequest;

namespace W88.BusinessLogic.Rewards.Redemption.Factories
{
    public abstract class RedemptionBase : BaseHelper
    {
        protected RedemptionRequest Request = null;

        protected RedemptionBase(RedemptionRequest request)
        {
            Request = request;
            request.OperatorId = OperatorId.ToString(CultureInfo.InvariantCulture);           
        }

        protected bool Validate(ProcessCode process)
        {
            process.ProcessSerialId += 1;

            switch (Request.ProductType)
            {
                case ProductTypeEnum.Freebet:
                    return true;
                case ProductTypeEnum.Normal:
                    {
                        if (string.IsNullOrEmpty(Request.Name.Trim()))
                        {
                            process.Code = (int)Constants.StatusCode.Error;
                            process.Message = "Name is empty";
                            LogError(process);
                            return false;
                        }
                        if (string.IsNullOrEmpty(Request.ContactNumber.Trim()))
                        {
                            process.Code = (int)Constants.StatusCode.Error;
                            process.Message = "Contact number is empty";
                            LogError(process);
                            return false;
                        }
                        if (string.IsNullOrEmpty(Request.Address.Trim()))
                        {
                            process.Code = (int)Constants.StatusCode.Error;
                            process.Message = "Address is empty";
                            LogError(process);
                            return false;
                        }
                        if (string.IsNullOrEmpty(Request.PostalCode.Trim()))
                        {
                            process.Code = (int)Constants.StatusCode.Error;
                            process.Message = "Postal code is empty";
                            LogError(process);
                            return false;
                        }
                        if (string.IsNullOrEmpty(Request.City.Trim()))
                        {
                            process.Code = (int)Constants.StatusCode.Error;
                            process.Message = "City is empty";
                            LogError(process);
                            return false;
                        }
                        if (string.IsNullOrEmpty(Request.Country.Trim()))
                        {
                            process.Code = (int)Constants.StatusCode.Error;
                            process.Message = "Country is empty";
                            LogError(process);
                            return false;
                        }
                        return true;
                    }
                case ProductTypeEnum.Wishlist:
                    process.Code = (int)Constants.StatusCode.Error;
                    process.Message = "Product type is wishlist";
                    LogError(process);
                    return false;
                case ProductTypeEnum.Online:
                    if (string.IsNullOrEmpty(Request.AimId.Trim()))
                    {
                        process.Code = (int)Constants.StatusCode.Error;
                        process.Message = "Account id is missing";
                        LogError(process);
                        return false;
                    }
                    return true;
            }
            return false;
        }

        protected void LogError(ProcessCode process)
        {
            AuditTrail.AppendLog(Request.MemberCode, Constants.PageNames.RedeemPage, Constants.TaskNames.RedeemRewards,
                Constants.PageNames.ComponentName, string.Empty, string.Empty, Convert.ToString(process.Code),
                process.Message, string.Empty, Convert.ToString(process.ProcessSerialId), Convert.ToString(process.Id), false);
        }

        protected void LogRedemption(ProcessCode process)
        {
            var response = (RedemptionResponse) process.Data;
            var redeemId = response.RedemptionIds != null ? String.Join("|", response.RedemptionIds.ToArray()) : string.Empty;
            var remark = string.Format("Product Id: {0}; Points Required: {1}; Quantity: {2};", Request.ProductId, Request.PointRequired, Request.Quantity);
            var detail = string.Format("Redeem Result: {0}; RedeemId: {1}; Type: {2};",response.Result, redeemId,  Request.ProductType);
            AuditTrail.AppendLog(Request.MemberCode, Constants.PageNames.RedeemPage, Constants.TaskNames.RedeemRewards,
                Constants.PageNames.ComponentName, Convert.ToString(process.Code), detail, string.Empty,
                string.Empty, remark, Convert.ToString(process.ProcessSerialId), Convert.ToString(process.Id), false);
        }

        public abstract Task<ProcessCode> Redeem();

        protected abstract dynamic CreateRequest();
    }
}
