using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using customConfig;
using Models;
using Newtonsoft.Json;

namespace Helpers
{
    /// <summary>
    /// Summary description for MaintenanceHelper
    /// </summary>
    public sealed class MaintenanceHelper
    {
        private OperatorSettings _opsettings;
        private MaintenanceInfo _maintenanceInfo;
        private string _operatorPass;

        public MaintenanceHelper ( MaintenanceInfo maintenanceInfo)
        {
            _opsettings = new OperatorSettings("W88");
            _maintenanceInfo = maintenanceInfo;
            _operatorPass = _opsettings.Values.Get("OperatorKey");
        }

        public MaintenanceIsActive CheckStatus()
        {
            var maintenance = CheckManualSetting();

            if (maintenance.AllSite)
            {
                return maintenance;
            }

            var values = new Dictionary<string, string>
            {
                {"OperatorId", commonVariables.OperatorId},
                {"Password", commonEncryption.GetMd5Hash(_operatorPass)}
            };

            var content = new FormUrlEncodedContent(values);

            var slotClient = new HttpClient();
            var wcfResponse = slotClient.PostAsync(_opsettings.Values.Get("MaintenanceService") + "/GetUnderMaintenanceModule", content).Result;
            var stream = wcfResponse.Content;
            var data = stream.ReadAsStringAsync();

            var response = (MaintenanceServiceResponse) JsonConvert.DeserializeObject(data.Result, typeof (MaintenanceServiceResponse));

            if (response.info.ErrorCode != 0)
            {
                maintenance.AllSite = maintenance.PerModule = false;
            }

            var hasRow = response.detail.Where(module => module.ModuleCode.ToUpper() == _maintenanceInfo.CurrentPage.ToString()).ToList();

            if (hasRow.Count > 0)
            {
                foreach (var item in hasRow)
                {
                    if (DateTime.Now > item.StartDate && DateTime.Now < item.EndDate)
                        maintenance.PerModule = true;
                    else
                        maintenance.PerModule = false;
                }
            }

            return maintenance;
        }

        private MaintenanceIsActive CheckManualSetting()
        {
            var maintenance = new MaintenanceIsActive();

            if (_maintenanceInfo.MainSite)
            {
                maintenance.AllSite = true;
            }

            switch (_maintenanceInfo.CurrentPage)
            {
                case MaintenanceModules.DPM:
                     maintenance.PerModule = _maintenanceInfo.Deposit;
                    break;
                case MaintenanceModules.FTM:
                     maintenance.PerModule = _maintenanceInfo.FundTransfer;
                     break;
                case MaintenanceModules.RS:
                     maintenance.PerModule = _maintenanceInfo.Rebates;
                     break;
                case MaintenanceModules.WPM:
                     maintenance.PerModule = _maintenanceInfo.Widrawal;
                    break;
            }

            return maintenance;
        }

    }
}