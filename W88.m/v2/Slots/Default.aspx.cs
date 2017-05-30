using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class v2_Slots_Default : BasePage
{
    public string SlotSettingsFile;
    protected void Page_Load(object sender, EventArgs e)
    {
        SlotSettingsFile = (commonFunctions.isExternalPlatform()) ? "native" : "default";
    }
}