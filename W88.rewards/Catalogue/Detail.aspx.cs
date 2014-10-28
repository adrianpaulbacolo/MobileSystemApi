using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;
using System.Diagnostics;
using System.Text;

public partial class Catalogue_Detail : BasePage
{
    protected string strRedirect = string.Empty;
    protected void Page_Init(object sender, EventArgs e)
    {


    }

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.Get("id")))
        {
            lblDescription.Text = HttpContext.Current.Request.QueryString.Get("id");
        }

        string userMemberId = string.IsNullOrEmpty((string)Session["MemberId"]) ? "" : (string)Session["MemberId"];
        string countryCode = string.IsNullOrEmpty((string)Session["CountryCode"])
            ? "0"
            : (string)Session["CountryCode"];
        string currencyCode = string.IsNullOrEmpty((string)Session["CurrencyCode"])
            ? "0"
            : (string)Session["CurrencyCode"];
        string riskId = string.IsNullOrEmpty((string)Session["RiskId"]) ? "0" : (string)Session["RiskId"];
        string productID = HttpContext.Current.Request.QueryString.Get("id");
        System.Web.HttpContext.Current.Session["productId"] = productID;
        string selectedKey1 = hiddenproductitd.Value;
        string selectedKey3 = hiddenproductitd.ToString();
        string selectedKey2 = Request.Form[hiddenproductitd.Value];

        if (!string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId))
            strRedirect = string.Format("/Catalogue/Redeem.aspx?productId={0}", productID);
        else
        {
            strRedirect = string.Format("/_Secure/Login.aspx?redirect=Redeem&productid={0}", productID);
            //  strRedirect = string.Format("/_Secure/Login.aspx");
        }



        using (RewardsServices.RewardsServicesClient sClient = new RewardsServices.RewardsServicesClient())
        {
            System.Data.DataSet ds = sClient.getProductDetail(productID, commonVariables.SelectedLanguage, riskId);

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
                    {
                        System.Web.HttpContext.Current.Session["productType"] = dr["productType"].ToString();
                        System.Web.HttpContext.Current.Session["amountLimit"] = dr["amountLimit"];
                        System.Web.HttpContext.Current.Session["categoryId"] = dr["categoryId"].ToString();
                        System.Web.HttpContext.Current.Session["categoryIdReload"] = dr["categoryId"].ToString();
                        System.Web.HttpContext.Current.Session["currencyValidity"] = currencyCode;

                        dr["pointsRequired"] =
                            Convert.ToInt32(dr["pointsRequired"].ToString().Replace(" ", string.Empty));

                        if (!ds.Tables[0].Columns.Contains("pointsLeveldiscount"))
                        {
                            ds.Tables[0].Columns.Add("pointsLeveldiscount");
                            dr["pointsLeveldiscount"] = 0;
                        }

                        if (!ds.Tables[0].Columns.Contains("pointsRequired2"))
                        {
                            ds.Tables[0].Columns.Add("pointsRequired2");
                            dr["pointsRequired2"] = dr["pointsRequired"];
                        }

                        if (!ds.Tables[0].Columns.Contains("discountPercentage"))
                        {
                            ds.Tables[0].Columns.Add("discountPercentage");
                            dr["discountPercentage"] = 0;
                        }

                        if (dr["discountPoints"] != DBNull.Value)
                        {
                            System.Web.HttpContext.Current.Session["pointsRequired"] = dr["discountPoints"];
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId) &&
                                dr["productType"].ToString() != "1")
                            {
                                //grap member point level
                                string pointLevel = sClient.getMemberPointLevelFE(userMemberId);
                                int pointLevelDiscount = sClient.getMemberPointLevelDiscount(
                                    commonVariables.OperatorId, currencyCode, pointLevel);

                                double percentage = Convert.ToDouble(pointLevelDiscount) / 100;
                                int normalPoint = int.Parse(dr["pointsRequired"].ToString());

                                double points = Math.Floor(normalPoint * (1 - percentage));
                                int pointAfterLevelDiscount = Convert.ToInt32(points);

                                dr["pointsRequired"] = pointAfterLevelDiscount;
                                dr["pointsLeveldiscount"] = pointAfterLevelDiscount;
                                dr["discountPercentage"] = pointLevelDiscount;

                                System.Web.HttpContext.Current.Session["pointsRequired"] = dr["pointsRequired"];
                                System.Web.HttpContext.Current.Session["pointsLeveldiscount"] = dr["pointsRequired"];
                            }
                            else
                                System.Web.HttpContext.Current.Session["pointsRequired"] = dr["pointsRequired"];
                        }

                        dr["currencyValidity"] = currencyCode;
                        dr["imageName"] =
                            Convert.ToString(
                                System.Configuration.ConfigurationManager.AppSettings.Get("ImagesDirectoryPath") +
                                "Product/" + dr["imageName"]);


                        if (!string.IsNullOrEmpty(riskId))
                        {
                            //category
                            dr["redemptionValidityCat"] += ",";
                            if (dr["redemptionValidityCat"].ToString().ToUpper() != "ALL,")
                            {
                                if (((string)dr["redemptionValidityCat"]).IndexOf(riskId + ",") < 0)
                                    dr["redemptionValidityCat"] = "0";
                                else
                                    dr["redemptionValidityCat"] = "1";
                            }
                            else
                                dr["redemptionValidityCat"] = "1";


                            dr["redemptionValidity"] += ",";
                            if (dr["redemptionValidity"].ToString().ToUpper() != "ALL,")
                            {
                                if (((string)dr["redemptionValidity"]).IndexOf(riskId + ",") < 0)
                                    dr["redemptionValidity"] = "0";
                                else
                                    dr["redemptionValidity"] = "1";
                            }
                            else
                                dr["redemptionValidity"] = "1";

                        }
                        else
                        {
                            dr["redemptionValidity"] += "0";
                            dr["redemptionValidityCat"] += "0";
                        }

                        imgPic.ImageUrl = dr["imageName"].ToString();

                        if (!string.IsNullOrEmpty(dr["discountPoints"].ToString()) && int.Parse(dr["discountPoints"].ToString()) != 0)
                            lblPointCenter.Text = String.Format("{0:#,###,##0.##}", dr["discountPoints"].ToString()) + " Points";
                        else
                            lblPointCenter.Text = String.Format("{0:#,###,##0.##}", dr["pointsRequired"].ToString()) + " Points";


                        lblName.Text = dr["productName"].ToString();

                        lblDescription.Text = "<p>" + (dr["productDescription"].ToString()) + "</p>";

                        if (!string.IsNullOrEmpty(dr["deliveryPeriod"].ToString()))
                        {
                            lblDelivery.Text = "<p>Delivery Days: " + (dr["deliveryPeriod"].ToString()) + " Day(s).</p>";
                        }

                    }




                }
            }


        }
    }


    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        //string strRedirect = "";


        //if (!string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId))
        //    strRedirect = string.Format("/Catalogue/Redeem.aspx?productId={0}", hiddenproductitd.Value);
        //else
        //    strRedirect = string.Format("/_Secure/Login.aspx?redirect=/Redeem&productid={0}", hiddenproductitd.Value);

        //Response.Redirect(strRedirect);

    }
}