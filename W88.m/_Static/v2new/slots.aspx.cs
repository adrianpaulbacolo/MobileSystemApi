using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Static_v2new_slots : BasePage
{
    public string SlotSettingsFile;
    protected void Page_Load(object sender, EventArgs e)
    {
        SlotSettingsFile = (commonFunctions.isExternalPlatform()) ? "native.js" : "default.js";
    }
}