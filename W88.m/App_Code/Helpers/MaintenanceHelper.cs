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

            if (response.ResponseInfo.ErrorCode != 0)
            {
                maintenance.AllSite = maintenance.PerModule = false;
            }

            var hasRow = response.ModuleInfo.Any(module => module.ModuleCode.ToUpper() == _maintenanceInfo.CurrentPage.ToString());

            if (hasRow)
            {
                maintenance.PerModule = false;
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
                case MaintenanceModules.FTM:
                case MaintenanceModules.RS:
                case MaintenanceModules.WPM:
                    maintenance.PerModule = true;
                    break;
            }

            return maintenance;
        }

    }
}