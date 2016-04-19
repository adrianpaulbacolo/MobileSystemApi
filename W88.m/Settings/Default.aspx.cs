using System;

namespace Settings
{
    public partial class SettingsDefault : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack) return;

            SetTitle("Settings");

        }
    }
}