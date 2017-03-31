using System.Collections.Generic;

namespace Models
{
    /// <summary>
    /// Summary description for MaintenanceInfo
    /// </summary>
    public class MaintenanceInfo
    {
        public bool MainSite { get; set; }

        public bool Deposit { get; set; }

        public bool Widrawal { get; set; }

        public bool FundTransfer { get; set; }

        public bool Rebates { get; set; }

        public MaintenanceModules CurrentPage { get; set; }
    }

    public enum MaintenanceModules
    {
        FTM,
        RS,
        DPM,
        WPM
    }

    public class MaintenanceServiceResponse
    {
        public Info ResponseInfo { get; set; }

        public List<MaintenanceModuleInfo> ModuleInfo { get; set; }
    }

    public class MaintenanceModuleInfo
    {
        public string ModuleCode { get; set; }

        public string ModuleName  { get; set; }
        
        public string StartDate { get; set; }
     
        public string EndDate { get; set; }
    }

    public class MaintenanceIsActive
    {
        public bool AllSite { get; set; }

        public bool PerModule { get; set; }

    }
}