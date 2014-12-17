using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;
using System.Diagnostics;
using System.Text;

public partial class Catalogue_Redeem : BasePage
{
    public string localResx = "~/default.{0}.aspx";
    protected string strAlertCode = string.Empty;
    protected string strAlertMessage = string.Empty;
    protected string productType = string.Empty;
    protected string vipOnly = "Hey there! This rewards redemption is only for VIP-Gold and above HOUSE OF HIGHROLLERS, YOU DESERVED IT!";
  

    protected void Page_Init(object sender, EventArgs e)
    {
       
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            localResx = string.Format("~/default.{0}.aspx", commonVariables.SelectedLanguage);
            lbcat.Text = HttpContext.GetLocalResourceObject(localResx, "lbl_category").ToString() + ":";
            lbproduct.Text = HttpContext.GetLocalResourceObject(localResx, "lbl_product").ToString() + ":";
            lbcurr.Text = HttpContext.GetLocalResourceObject(localResx, "lbl_currency").ToString() + ":";
            lbpoint.Text = HttpContext.GetLocalResourceObject(localResx, "lbl_points").ToString() + ":";
            lbperiod.Text =HttpContext.GetLocalResourceObject(localResx, "lbl_delivery_period").ToString()+ ":";
            lbqty.Text =HttpContext.GetLocalResourceObject(localResx, "lbl_quantity").ToString() + ":";
            lbaccount.Text = HttpContext.GetLocalResourceObject(localResx, "lbl_account").ToString() + ":";
            tbRName.Attributes.Add("PLACEHOLDER", HttpContext.GetLocalResourceObject(localResx, "lbl_enter_name").ToString());
            tbAddress.Attributes.Add("PLACEHOLDER", HttpContext.GetLocalResourceObject(localResx, "lbl_enter_address").ToString());
            tbPostal.Attributes.Add("PLACEHOLDER", HttpContext.GetLocalResourceObject(localResx, "lbl_enter_postal").ToString());
            tbCity.Attributes.Add("PLACEHOLDER", HttpContext.GetLocalResourceObject(localResx, "lbl_enter_city").ToString());
            tbCountry.Attributes.Add("PLACEHOLDER", HttpContext.GetLocalResourceObject(localResx, "lbl_enter_country").ToString());
            tbContact.Attributes.Add("PLACEHOLDER", HttpContext.GetLocalResourceObject(localResx, "lbl_enter_contact").ToString());  

            btnSubmit.Text = HttpContext.GetLocalResourceObject(localResx, "lbl_redeem_now").ToString();

            if (!string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId))
            {
                lblPoint.InnerText = HttpContext.GetLocalResourceObject(localResx, "lbl_points").ToString() +": " + getCurrentPoints();
             
                divLevel.Visible = true;
            }
            else
            { Response.Redirect("~/Index");
            }
            

            string userMemberId = string.IsNullOrEmpty((string)Session["MemberId"]) ? "" : (string)Session["MemberId"];
            string strMemberCode = string.IsNullOrEmpty((string)Session["MemberCode"]) ? "" : (string)Session["MemberCode"];
            string countryCode = string.IsNullOrEmpty((string)Session["CountryCode"]) ? "0" : (string)Session["CountryCode"];
            string currencyCode = string.IsNullOrEmpty((string)Session["CurrencyCode"]) ? "0" : (string)Session["CurrencyCode"];
            string riskId = string.IsNullOrEmpty((string)Session["RiskId"]) ? "0" : (string)Session["RiskId"];
            string productID = HttpContext.Current.Request.QueryString.Get("productId");

            lblproductid.Value = productID;

            int quantity = 0;

            using (RewardsServices.RewardsServicesClient sClient = new RewardsServices.RewardsServicesClient())
            {

                #region product

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
                            System.Web.HttpContext.Current.Session["currencyValidity"] = dr["currencyValidity"].ToString();
               

                            productType = dr["productType"].ToString();

                            if (productType == "1" && (dr["currencyValidity"].ToString() != currencyCode))
                            {
                                Response.Redirect("/Catalogue?categoryId=53&sortBy=2");
                                return;
                            }

                            if (dr["countryValidity"].ToString().Trim() != "All")
                            {
                                if (!dr["countryValidity"].ToString().Contains(countryCode))
                                {
                                    Response.Redirect("/Catalogue?categoryId=" + dr["categoryId"].ToString() + "&sortBy=2");
                                    return;
                                }
                            }
                            

                            dr["pointsRequired"] = Convert.ToInt32(dr["pointsRequired"].ToString().Replace(" ", string.Empty));

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
                                System.Web.HttpContext.Current.Session["pointsRequired"] = dr["discountPoints"];
                            else
                            {
                                if (!string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId) &&
                                    dr["productType"].ToString() != "1")
                                {
                                    //grap member point level
                                    string pointLevel = sClient.getMemberPointLevelFE(userMemberId);
                                    int pointLevelDiscount = sClient.getMemberPointLevelDiscount(commonVariables.OperatorId, currencyCode, pointLevel);

                                    double percentage = Convert.ToDouble(pointLevelDiscount) / 100;
                                    int normalPoint = int.Parse(dr["pointsRequired"].ToString());

                                    double points = Math.Floor(normalPoint * (1 - percentage));
                                    int pointAfterLevelDiscount = Convert.ToInt32(points);

                                    dr["pointsRequired"] = pointAfterLevelDiscount;
                                    dr["pointsLeveldiscount"] = pointAfterLevelDiscount;
                                    dr["discountPercentage"] = pointLevelDiscount;

                                    System.Web.HttpContext.Current.Session["pointsRequired"] = dr["pointsLeveldiscount"];
                                    System.Web.HttpContext.Current.Session["pointsLeveldiscount"] =
                                        dr["pointsLeveldiscount"];
                                }
                                else
                                    System.Web.HttpContext.Current.Session["pointsRequired"] = dr["pointsRequired"];
                            }


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

                            if (!(dr["redemptionValidity"].ToString() == "1" && dr["redemptionValidityCat"].ToString() == "1"))
                            {
                                strAlertCode = "VIP";
                                  vipOnly =HttpContext.GetLocalResourceObject(localResx, "lbl_redeem_vip").ToString();
                                return;
                            }


                            imgPic.ImageUrl = dr["imageName"].ToString();
                            if (dr["discountPoints"] != DBNull.Value)
                            {
                                lblBeforeDiscount.Text =
                                    String.Format("{0:#,###,##0.##}", dr["pointsRequired"].ToString()) + " " + HttpContext.GetLocalResourceObject(localResx, "lbl_points").ToString() ;
                                lblPointCenter.Text = String.Format("{0:#,###,##0.##}", dr["discountPoints"].ToString()) +
                                                      " Points";
                            }
                            else
                            {
                                lblBeforeDiscount.Text = "";
                                lblPointCenter.Text = String.Format("{0:#,###,##0.##}", dr["pointsRequired"].ToString()) +
                                                   " " + HttpContext.GetLocalResourceObject(localResx, "lbl_points").ToString();
                            }



                            lblName.Text = dr["productName"].ToString();
                            lblCategory.Text = dr["categoryName"].ToString();

                            if (!string.IsNullOrEmpty(dr["deliveryPeriod"].ToString()))
                            {
                                lblDelivery.Text = (dr["deliveryPeriod"].ToString());
                                DeliveryDiv.Visible = true;
                            }

                            if (!string.IsNullOrEmpty(dr["currencyValidity"].ToString()))
                                lblCurrency.Text = (dr["currencyValidity"].ToString());

                            /*
                              freebet show  currency, hide recipient panel , hide delivery, hide account
                              normal product show recipient, show delivery if any,  hide currency, hide account
                              online show account, hide delivery, hide recipient, hide currency
                              */
                            switch (productType)
                            {
                                case "1"://freebet
                                    CurrencyDiv.Visible = true;
                                    RecipientDiv.Visible = false;
                                    DeliveryDiv.Visible = false;
                                    AccountDiv.Visible = false;
                                    break;
                                case "2"://normal
                                    RecipientDiv.Visible = true;
                                    CurrencyDiv.Visible = false;
                                    AccountDiv.Visible = false;
                                    break;
                                case "3"://wishlist same as normal
                                    RecipientDiv.Visible = true;
                                    CurrencyDiv.Visible = false;
                                    AccountDiv.Visible = false;
                                    break;
                                case "4"://online
                                    AccountDiv.Visible = true;
                                    CurrencyDiv.Visible = false;
                                    RecipientDiv.Visible = false;
                                    DeliveryDiv.Visible = false;
                                    break;
                            }

                        }//end for loop
                    }
                }
                #endregion product

                #region memberInfo

                System.Data.DataSet dsMember = sClient.getMemberRedemptionDetail(commonVariables.OperatorId, strMemberCode);
                if (dsMember.Tables.Count > 0)
                {
                    if (dsMember.Tables[0].Rows.Count > 0)
                    {
                        foreach (System.Data.DataRow drMember in dsMember.Tables[0].Rows)
                        {
                            if (productType == "2" || productType == "3")//normal & wishlist
                            {
                                tbRName.Text = drMember["firstName"].ToString() + " " + drMember["lastName"].ToString();
                                tbAddress.Value = drMember["address"].ToString(); //+ ", " + drMember["postal"].ToString() + ", " + drMember["city"].ToString() + ", " + drMember["countryCode"].ToString();
                                tbPostal.Text = drMember["postal"].ToString();
                                tbCity.Text = drMember["city"].ToString();
                                tbCountry.Text = drMember["countryCode"].ToString();
                                tbContact.Text = drMember["mobile"].ToString();
                            }

                        }
                    }

                    if (dsMember.Tables[1].Rows.Count > 0)
                    {
                        foreach (System.Data.DataRow drMember in dsMember.Tables[1].Rows)
                        {
                            System.Web.HttpContext.Current.Session["pointsBefore"] = drMember["pointsBefore"];
                        }
                    }
                }


                //}
                #endregion

            }

        }

    }

    public static int getCurrentPoints()
    {
        int total = 0;
        int claim = 0;
        int current = 0;
        int cart = 0;

        try
        {

            if (!string.IsNullOrEmpty((string)HttpContext.Current.Session["MemberId"]))
            {
                using (RewardsServices.RewardsServicesClient sClient = new RewardsServices.RewardsServicesClient())
                {
                    string strMemberCode = string.IsNullOrEmpty(System.Web.HttpContext.Current.Session["MemberCode"] as string) ? string.Empty : Convert.ToString(System.Web.HttpContext.Current.Session["MemberCode"]);

                    System.Data.DataSet ds = sClient.getRedemptionDetail(commonVariables.OperatorId, strMemberCode);

                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            total = int.Parse(ds.Tables[0].Rows[0][0].ToString());

                            if (ds.Tables[1].Rows.Count > 0)
                            {
                                claim = int.Parse(ds.Tables[1].Rows[0][0].ToString());
                            }

                            if (ds.Tables[2].Rows.Count > 0)
                            {
                                cart = int.Parse(ds.Tables[2].Rows[0][0].ToString());
                            }
                            claim = claim + cart;
                        }

                    }
                    current = total - claim;

                    HttpContext.Current.Session["pointsBalance"] = current;

                    return current;

                }
            }
            else
            {
                return 0;
            }
        }
        catch (Exception ex)
        {
            //throw;
            return 0;
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        localResx = string.Format("~/default.{0}.aspx", commonVariables.SelectedLanguage);

        strAlertCode = "";//FAIL SUCCESS
        strAlertMessage = "";
        int resultLog = 0;
        string userMemberId = string.IsNullOrEmpty((string)Session["MemberId"]) ? "" : (string)Session["MemberId"];
        string strMemberCode = string.IsNullOrEmpty((string)Session["MemberCode"]) ? "" : (string)Session["MemberCode"];
        string countryCode = string.IsNullOrEmpty((string)Session["CountryCode"]) ? "0" : (string)Session["CountryCode"];
        string currencyCode = string.IsNullOrEmpty((string)Session["CurrencyCode"]) ? "0" : (string)Session["CurrencyCode"];
        string riskId = string.IsNullOrEmpty((string)Session["RiskId"]) ? "0" : (string)Session["RiskId"];
        productType = (string)System.Web.HttpContext.Current.Session["productType"];
        string productID = lblproductid.Value;
        string categoryId = string.IsNullOrEmpty((string)Session["categoryId"]) ? "" : (string)Session["categoryId"];
        int pointsRequired = int.Parse(System.Web.HttpContext.Current.Session["pointsRequired"].ToString());
        string createdBy = string.IsNullOrEmpty((string)Session["MemberCode"]) ? "" : (string)Session["MemberCode"];
        string quantitytext = tbQuantity.Text.Trim();
        int quantity;

        string redemptionId = "";
        string result = "";

        bool res = int.TryParse(quantitytext, out quantity);
        if (res == false)
        {
            strAlertCode = "FAIL";
           // strAlertMessage = "Please enter an number for quantity";
            strAlertMessage = HttpContext.GetLocalResourceObject(localResx, "lbl_invalid_number").ToString();
            
            return;
        }
        else
        {
            quantity = int.Parse(quantitytext);
            if (quantity < 1)
            {
                strAlertCode = "FAIL";
              //  strAlertMessage = "Please enter an number bigger than zero";
                strAlertMessage = HttpContext.GetLocalResourceObject(localResx, "lbl_invalid_at_least").ToString();
                
                return;
            }
        }

        try
        {
            #region redeemnow
            using (RewardsServices.RewardsServicesClient sClient = new RewardsServices.RewardsServicesClient())
            {
                //checking before redeem
                string pointLevel = sClient.getMemberPointLevelFE(userMemberId);
                System.Data.DataSet dsPoint = sClient.getProductPoint(commonVariables.OperatorId, productID, riskId, currencyCode, pointLevel);

                int pointsProduct = 0;
                int pointsDiscount = 0;
                int pointsLevelDiscount = 0;
                int pointsLevelDiscountPoint = 0;
                int actualPoint = 0;

                if (dsPoint.Tables.Count > 0)
                {
                    if (dsPoint.Tables[0].Rows.Count > 0)
                    {
                        pointsProduct = int.Parse(dsPoint.Tables[0].Rows[0][0].ToString());
                        actualPoint = pointsProduct;

                        if (dsPoint.Tables[1].Rows.Count > 0)
                        {
                            pointsDiscount = int.Parse(dsPoint.Tables[1].Rows[0][0].ToString());
                            if (pointsDiscount != 0)
                                actualPoint = pointsDiscount;
                        }

                        if (dsPoint.Tables[2].Rows.Count > 0)
                        {
                            pointsLevelDiscount = int.Parse(dsPoint.Tables[2].Rows[0][0].ToString());
                            if (pointsDiscount == 0 && pointsLevelDiscount != 0 && productType != "1")
                            {
                                //reverse to new points maintain in bo Discount
                                double percentage = Convert.ToDouble(pointsLevelDiscount) / 100;
                                int normalPoint = pointsProduct;

                                double points = Math.Floor(normalPoint * (1 - percentage));
                                int pointAfterLevelDiscount = Convert.ToInt32(points);

                                pointsLevelDiscountPoint = pointAfterLevelDiscount;
                                if (pointsLevelDiscountPoint != 0)
                                    actualPoint = pointsLevelDiscountPoint;
                            }
                        }
                    }
                }

                if (pointsRequired != actualPoint)
                {
                    strAlertCode = "FAIL";
                   // strAlertMessage = "Request Submission Fail"; //
                    strAlertMessage = HttpContext.GetLocalResourceObject(localResx, "lblPointCheckError").ToString();
                    return;
                }
                else
                {
                    System.Data.DataSet ds = sClient.getRedemptionDetail(commonVariables.OperatorId, strMemberCode);
                    int total = 0;
                    int utilize = 0;
                    int current = 0;
                    int cart = 0;

                    int totalPoint = 0;

                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            total = int.Parse(ds.Tables[0].Rows[0][0].ToString());

                            if (ds.Tables[1].Rows.Count > 0)
                                utilize = int.Parse(ds.Tables[1].Rows[0][0].ToString());

                            if (ds.Tables[2].Rows.Count > 0)
                                cart = int.Parse(ds.Tables[2].Rows[0][0].ToString());
                        }
                    }
                    current = total - utilize - cart;

                    totalPoint = pointsRequired * quantity;

                    if (current < totalPoint)
                    {
                        strAlertCode = "FAIL";
                       // strAlertMessage = "Your Points Are Insufficient. Please Earn More Reward Points For This Redemption!"; 
                        strAlertMessage = HttpContext.GetLocalResourceObject(localResx, "lbl_points_insufficient").ToString();
                        //lbl_points_insufficient
                        return;
                    }
                    else
                    {

                        int totalCount = sClient.checkRedemptionLimitDaily(commonVariables.OperatorId, strMemberCode, productID, quantity);

                        // marty
                        if (sClient.CheckRedemptionLimitWithRedemptionQuantity(commonVariables.OperatorId, strMemberCode, productID, quantity))
                        {
                            strAlertCode = "FAIL";
                          //  strAlertMessage = "Redemption limit reached!"; 
                            strAlertMessage = HttpContext.GetLocalResourceObject(localResx, "lbl_redemption_limit_reached").ToString();
                            return;
                        }
                        else if (totalCount > 10)
                        {
                            strAlertCode = "FAIL";
                          //  strAlertMessage = "Redemption limit reached!"; 
                            strAlertMessage = HttpContext.GetLocalResourceObject(localResx, "lbl_redemption_limit_reached").ToString();
                            return;
                        }

                        if (productType == "1") //Freebet
                        {
                            double creditAmt = Convert.ToDouble(Session["amountLimit"]);
                            int redemptionStatus = 1;
                            decimal creditAmtFreebet = Convert.ToDecimal(Session["amountLimit"]);
                            string remarks = "Free Bets (Rewards) " + currencyCode + " " + Math.Round(creditAmt, 0);
                            //Freebets Auto Credit

                            if ((string)System.Configuration.ConfigurationManager.AppSettings.Get("AutoFreebets") == "1")
                                redemptionStatus = 2;

                            if ((string)System.Configuration.ConfigurationManager.AppSettings.Get("AutoFreebets") == "1")
                            {
                                int FreebetAuto = 0;

                                for (int i = 1; i <= quantity; i++)
                                {
                                    using (PaymentServices.MemberClient sClientFreebetCredit = new PaymentServices.MemberClient())
                                    {
                                        FreebetAuto = sClientFreebetCredit.AddMemberRewards(long.Parse(commonVariables.OperatorId), strMemberCode, currencyCode, creditAmtFreebet, remarks);

                                        if (FreebetAuto == 0) //error during credit
                                            redemptionStatus = 1;
                                    }
                                }
                            }

                            Session["redemptionStatus"] = redemptionStatus;
                            for (int i = 1; i <= quantity; i++)
                            {
                                redemptionId = sClient.addRedemptionFreebetNew(commonVariables.OperatorId, strMemberCode, productID, categoryId, productType, riskId, pointsRequired, createdBy, currencyCode, creditAmt, redemptionStatus, remarks, "0", 2);

                                #region get member Info
                                using (RewardsServices.RewardsServicesClient sClientMember = new RewardsServices.RewardsServicesClient())
                                {
                                    System.Data.DataSet dsMember = sClientMember.getMemberRedemptionDetail(commonVariables.OperatorId, strMemberCode);

                                    if (dsMember.Tables.Count > 0)
                                    {
                                        if (dsMember.Tables[1].Rows.Count > 0)
                                        {
                                            foreach (System.Data.DataRow drMember in dsMember.Tables[1].Rows)
                                                System.Web.HttpContext.Current.Session["pointsBeforeCart"] = drMember["pointsBefore"];
                                        }
                                    }
                                }
                                #endregion

                                int pointsBefore = (int)System.Web.HttpContext.Current.Session["pointsBeforeCart"];
                                int pointsAfter = pointsBefore - pointsRequired;
                                string actionId = "2";
                                resultLog = sClient.addLogPointsBalance(commonVariables.OperatorId, strMemberCode, pointsBefore, pointsRequired * -1, pointsAfter, actionId, redemptionId, createdBy);
                            }
                        }
                        else if (productType == "2") //Normal 
                        {
                            string name = tbRName.Text.Trim();
                            string contact = tbContact.Text.Trim();
                            string address = tbAddress.Value.Trim();
                            string postal = tbPostal.Text.Trim();
                            string city = tbCity.Text.Trim();
                            string country = tbCountry.Text.Trim();

                            for (int i = 1; i <= quantity; i++)
                            {
                                if (productType == "2")
                                    redemptionId = sClient.addRedemptionNormalNew(commonVariables.OperatorId, strMemberCode, productID, categoryId, productType, riskId, pointsRequired, createdBy, name, address, postal, city, country, contact, "0",2);

                                #region get member Info
                                using (RewardsServices.RewardsServicesClient sClientMember = new RewardsServices.RewardsServicesClient())
                                {
                                    System.Data.DataSet dsMember = sClientMember.getMemberRedemptionDetail(commonVariables.OperatorId, strMemberCode);

                                    if (dsMember.Tables.Count > 0)
                                    {
                                        if (dsMember.Tables[1].Rows.Count > 0)
                                        {
                                            foreach (System.Data.DataRow drMember in dsMember.Tables[1].Rows)
                                                System.Web.HttpContext.Current.Session["pointsBeforeCart"] = drMember["pointsBefore"];
                                        }
                                    }
                                }
                                #endregion

                                int pointsBefore = (int)System.Web.HttpContext.Current.Session["pointsBeforeCart"];
                                int pointsAfter = pointsBefore - pointsRequired;
                                string actionId = "2";
                                resultLog = sClient.addLogPointsBalance(commonVariables.OperatorId, strMemberCode, pointsBefore, pointsRequired * -1, pointsAfter, actionId, redemptionId, createdBy);
                            }
                        }

                        else if (productType == "4") //Online
                        {
                            string aimId = tbAccount.Text.Trim();
                            for (int i = 1; i <= quantity; i++)
                            {
                                redemptionId = sClient.addRedemptionOnlineNew(commonVariables.OperatorId, strMemberCode, productID, categoryId, productType, riskId, pointsRequired, createdBy, aimId, "0", 2);

                                #region get member Info
                                using (RewardsServices.RewardsServicesClient sClientMember = new RewardsServices.RewardsServicesClient())
                                {
                                    System.Data.DataSet dsMember = sClientMember.getMemberRedemptionDetail(commonVariables.OperatorId, strMemberCode);

                                    if (dsMember.Tables.Count > 0)
                                    {
                                        if (dsMember.Tables[1].Rows.Count > 0)
                                        {
                                            foreach (System.Data.DataRow drMember in dsMember.Tables[1].Rows)
                                            {
                                                System.Web.HttpContext.Current.Session["pointsBeforeCart"] = drMember["pointsBefore"];
                                            }
                                        }
                                    }
                                }
                                #endregion

                                int pointsBefore = (int)System.Web.HttpContext.Current.Session["pointsBeforeCart"];
                                int pointsAfter = pointsBefore - pointsRequired;
                                string actionId = "2";

                                resultLog = sClient.addLogPointsBalance(commonVariables.OperatorId, strMemberCode, pointsBefore, pointsRequired * -1, pointsAfter, actionId, redemptionId, createdBy);
                            }
                        }

                        if (resultLog == 1)
                        {
                            if (productType == "1" && (int)Session["redemptionStatus"] == 2) //Freebet success
                            {
                                sendMail(commonVariables.OperatorId, strMemberCode, redemptionId);
                                strAlertCode = "SUCCESS";
                                strAlertMessage = "Redemption is processed successfully!"; //lbl_redeem_success_processed
                                strAlertMessage = HttpContext.GetLocalResourceObject(localResx, "lbl_redeem_success_processed").ToString();
                            }
                            else
                            {
                                strAlertCode = "SUCCESS";
                                strAlertMessage = "Redemption is submitted successfully!"; //lbl_redeem_success_submit
                                strAlertMessage = HttpContext.GetLocalResourceObject(localResx, "lbl_redeem_success_submit").ToString();
                            }

                            lblPoint.InnerText = "Points Bal: " + getCurrentPoints();
                        }

                    }
                }
            }
            #endregion redeemnow
        }
        catch (Exception ex)
        {
            strAlertCode = "FAIL";
            strAlertMessage = HttpContext.GetLocalResourceObject(localResx, "lbl_Exception").ToString();
            Guid newerrorid = new Guid();
            commonAuditTrail.appendLog("system", "Redeem.aspx", "Redeem Now", "Redeem.aspx", "", "Redeem now", "", ex.Message + " stacktrace: " + ex.StackTrace, "Redemption id: " + redemptionId + " Member id: " + userMemberId, "", newerrorid.ToString(), false);
        }

    }

    private void sendMail(string operatorId, string memberCode, string redemptionId)
    {

        string email_to = "";
        string email_from = System.Configuration.ConfigurationManager.AppSettings.Get("email_from");
        string senderName = System.Configuration.ConfigurationManager.AppSettings.Get("senderName");
        string email_bcc = System.Configuration.ConfigurationManager.AppSettings.Get("email_bcc");
        string languageCode = "";
        string smtpAlternative = System.Configuration.ConfigurationManager.AppSettings.Get("smtpAlternative");
        bool alternative = false;

        string localResxMail = "~/redemption_mail.{0}.aspx";

        using (RewardsServices.RewardsServicesClient sClientMember = new RewardsServices.RewardsServicesClient())
        {
            System.Data.DataSet ds = sClientMember.getMemberInfo(operatorId, memberCode);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                email_to = dr["email"].ToString();
                languageCode = dr["languageCode"].ToString();
            }
        }

        localResxMail = string.Format(localResxMail, languageCode);

        try
        {
            System.Net.Mail.SmtpClient sClient = new System.Net.Mail.SmtpClient();
            System.Net.NetworkCredential nCredentials = new System.Net.NetworkCredential();

            using (System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage(email_from, email_to))
            {

                string[] results;
                string[] splitChar = { "|" };
                results = smtpAlternative.Split(splitChar, StringSplitOptions.None);

                foreach (string r in results)
                {
                    string mail = r;

                    if (email_to.Contains(mail))
                    {
                        alternative = true;
                        break;
                    }
                }

                //if (email_to.Contains("@qq.com") || email_to.Contains("@mail.com") || email_to.Contains("@yahoo"))
                if (alternative)
                {
                    sClient.Port = 25;
                    sClient.Host = "retail.smtp.com";

                    nCredentials.UserName = "dev@w88.com";
                    nCredentials.Password = "2NDbr0isFAT!";

                    sClient.UseDefaultCredentials = false;
                    sClient.Credentials = nCredentials;

                }
                else
                {
                    message.Bcc.Add(new System.Net.Mail.MailAddress(email_bcc));
                }
                message.From = new System.Net.Mail.MailAddress(email_from, senderName);
                message.BodyEncoding = Encoding.UTF8;
                message.IsBodyHtml = true;
                message.Subject = (string)System.Web.HttpContext.GetLocalResourceObject(localResxMail, "lbl_subject");
                message.Body = string.Format((string)System.Web.HttpContext.GetLocalResourceObject(localResxMail, "lbl_body"), memberCode.Trim(), redemptionId);
                sClient.Send(message);
            }
        }
        catch (Exception)
        {
            throw;
        }

    }

}





