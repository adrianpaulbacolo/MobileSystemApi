using System;
using System.Web;
using System.Xml.Linq;
using W88.BusinessLogic.Rewards.Helpers;
using W88.BusinessLogic.Shared.Helpers;

/// <summary>
/// Summary description for CatalogueBasePage
/// </summary>
public abstract class CatalogueBasePage : BasePage
{
    protected XElement LeftMenu = null;
    protected XElement XeErrors = null;
    protected string HeaderResx = "~/rewards.header.{0}.aspx";
    protected string LocalResx = "~/default.{0}.aspx";

	protected override void OnPreInit(EventArgs e)
	{
        base.OnPreInit(e);

        var language = HttpContext.Current.Request.QueryString["lang"];
        if (!string.IsNullOrEmpty(language))
        {
            LocalResx = string.Format(LocalResx, language);
            HeaderResx = string.Format(HeaderResx, language);
            LanguageHelpers.SelectedLanguage = language;
        }
        else
        {
            LocalResx = string.Format(LocalResx, LanguageHelpers.SelectedLanguage);
            HeaderResx = string.Format(HeaderResx, LanguageHelpers.SelectedLanguage);
        }

        XeErrors = CultureHelpers.AppData.GetRootResource("Errors");
	    LeftMenu = CultureHelpers.AppData.GetRootResource("leftMenu");
	}

    protected abstract void SetLabels();
}