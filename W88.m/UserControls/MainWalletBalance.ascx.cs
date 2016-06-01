using System;
using System.Globalization;
using System.Linq;

namespace UserControls
{
    public partial class UserControlsMainWalletBalance : System.Web.UI.UserControl
    { 
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack) return;

            var wallet = commonPaymentMethodFunc.GetWallets().Where(x => x.Key.Equals(0)).ToList();
            var wId = wallet.Where(x => x.Key.Equals(0)).Select(type => type.Key).ToList()[0];
            commonPaymentMethodFunc.GetWalletBalance(wId);
        }
    }
}