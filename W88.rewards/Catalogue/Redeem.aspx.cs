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
using RewardsServices;

public partial class Catalogue_Redeem : BasePage
{
    public string localResx = "~/default.{0}.aspx";
    protected string strAlertCode = string.Empty;
    protected string strAlertMessage = string.Empty;
    protected string productType = string.Empty;
    protected string vipOnly = "Hey there! This rewards redemption is only for VIP-Gold and above HOUSE OF HIGHROLLERS, YOU DESERVED IT!";

    private int audit_serial_id = 0;
    private string audit_id = string.Empty;
    private string audit_task = string.Empty;
    private string audit_comp = string.Empty;
    private string audit_detail = string.Empty;
    private string audit_remark = string.Empty;
    private string audit_message = string.Empty;
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

                            //vip cannot select quantity
                            if (dr["categoryId"].ToString() == commonVariables.VIPCategoryId.ToString())
                            {
                                tbQuantity.Enabled = false;
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
        ProductTypeEnum productTypeEnum = (ProductTypeEnum)int.Parse(System.Web.HttpContext.Current.Session["productType"].ToString());
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

        if (!Valid())
            return;

        try
        {
            using (RewardsServices.RewardsServicesClient sClient = new RewardsServices.RewardsServicesClient())
            {
                RedemptionResponse response = null;

                switch (productTypeEnum)
                {
                    case ProductTypeEnum.Freebet:
                        RedemptionFreebetRequest requestFreebet = BuildRedemptionFreebetRequest();
                        response = sClient.RedemptionFreebet(requestFreebet);
                        break;
                    case ProductTypeEnum.Normal:
                        RedemptionNormalRequest requestNormal = BuildRedemptionNormalRequest();
                        response = sClient.RedemptionNormal(requestNormal);
                        break;
                    case ProductTypeEnum.Online:
                        RedemptionOnlineRequest requestOnline = BuildRedemptionOnlineRequest();
                        response = sClient.RedemptionOnline(requestOnline);
                        break;
                }

                #region Service response handler
                if (response == null)
                {
                    strAlertMessage = (string)System.Web.HttpContext.GetLocalResourceObject(localResx, "lblPointCheckError");

                }
                else
                {
                    switch (response.Result)
                    {
                        case RedemptionResultEnum.ConcurrencyDetected:
                            break;
                        case RedemptionResultEnum.LimitReached:
                            strAlertCode = "FAIL";
                            strAlertMessage = (string)System.Web.HttpContext.GetLocalResourceObject(localResx, "lbl_redemption_limit_reached");
                            break;
                        case RedemptionResultEnum.VIPSuccessLimitReached:
                            strAlertCode = "FAIL";
                            strAlertMessage = (string)System.Web.HttpContext.GetLocalResourceObject(localResx, "lbl_redemption_success_limit_reached");
                            break;
                        case RedemptionResultEnum.VIPProcessingLimitReached:
                            strAlertCode = "FAIL";
                            strAlertMessage = (string)System.Web.HttpContext.GetLocalResourceObject(localResx, "lbl_points_insufficient");
                            break;
                        case RedemptionResultEnum.PointIsufficient:
                            strAlertCode = "FAIL";
                            strAlertMessage = (string)System.Web.HttpContext.GetLocalResourceObject(localResx, "lbl_points_insufficient");
                            break;
                        case RedemptionResultEnum.Success:
                            strAlertCode = "SUCCESS";
                            if (productTypeEnum == ProductTypeEnum.Freebet) //Freebet success
                            {
                                foreach (var redemptionitemId in response.RedemptionIds)
                                {
                                    sendMail(commonVariables.OperatorId, strMemberCode, redemptionitemId.ToString());
                                }

                                strAlertMessage = (string)System.Web.HttpContext.GetLocalResourceObject(localResx, "lbl_redeem_success_processed");
                            }
                            else
                                strAlertMessage = (string)System.Web.HttpContext.GetLocalResourceObject(localResx, "lbl_redeem_success_submit");

                          
                            break;
                        default:
                        case RedemptionResultEnum.UnknownError:
                        case RedemptionResultEnum.PointCheckError:
                            strAlertCode = "FAIL";
                            strAlertMessage = (string)System.Web.HttpContext.GetLocalResourceObject(localResx, "lblPointCheckError");
                            break;
                    }

                    #region Audit
                    audit_id = Session.SessionID;
                    audit_task = "RewardsServices";
                    audit_comp = "btnRedeem_Click";

                    var redeemId = response.RedemptionIds != null ? String.Join("|", response.RedemptionIds.ToArray()) : "";

                    audit_remark = "Product Id:" + (string)lblproductid.Value + "; Points Required:" + System.Web.HttpContext.Current.Session["pointsRequired"].ToString() + "; Quantity:" + tbQuantity.Text.Trim(); ;

                    audit_detail = ";Redeem Result:" + response.Result + ";RedeemId:" + redeemId + ";Type:" + productTypeEnum;
                    audit_serial_id += 1;
                    commonAuditTrail.appendLog("system", "Redeem.aspx", "Redeem Now", "Redeem.aspx", "checkPoint", audit_detail, "-", "", audit_remark, audit_serial_id.ToString(), audit_id, true);
                    #endregion
                }
                #endregion
            }

            lblPoint.InnerText = "Points Bal: " + getCurrentPoints();

        }
        catch (Exception ex)
        {
            strAlertCode = "FAIL";
            strAlertMessage = HttpContext.GetLocalResourceObject(localResx, "lbl_Exception").ToString();
            Guid newerrorid = new Guid();
            commonAuditTrail.appendLog("system", "Redeem.aspx", "Redeem Now", "Redeem.aspx", "", "Redeem now", "", ex.Message + " stacktrace: " + ex.StackTrace, "Redemption id: " + redemptionId + " Member id: " + userMemberId, "", newerrorid.ToString(), false);
        }

    }

    private RedemptionFreebetRequest BuildRedemptionFreebetRequest()
    {
        var request = new RedemptionFreebetRequest();

        request.OperatorId = commonVariables.OperatorId;
        request.MemberCode = string.IsNullOrEmpty((string)Session["MemberCode"]) ? "" : (string)Session["MemberCode"];
        request.ProductId = lblproductid.Value;
        request.CategoryId = int.Parse(string.IsNullOrEmpty((string)Session["categoryId"]) ? "" : (string)Session["categoryId"]);
        request.RiskId = string.IsNullOrEmpty((string)Session["RiskId"]) ? "0" : (string)Session["RiskId"];
        request.Currency = string.IsNullOrEmpty((string)Session["CurrencyCode"]) ? "0" : (string)Session["CurrencyCode"];

        request.PointRequired = int.Parse(System.Web.HttpContext.Current.Session["pointsRequired"].ToString());
        request.Quantity = int.Parse(tbQuantity.Text.Trim());
        request.CreditAmount = Convert.ToDecimal(Session["amountLimit"]);

        return request;
    }

    private RedemptionNormalRequest BuildRedemptionNormalRequest()
    {
        var request = new RedemptionNormalRequest();

        request.OperatorId = commonVariables.OperatorId;
        request.MemberCode = string.IsNullOrEmpty((string)Session["MemberCode"]) ? "" : (string)Session["MemberCode"];
        request.ProductId = lblproductid.Value;
        request.CategoryId = int.Parse(string.IsNullOrEmpty((string)Session["categoryId"]) ? "" : (string)Session["categoryId"]);
        request.RiskId = string.IsNullOrEmpty((string)Session["RiskId"]) ? "0" : (string)Session["RiskId"];
        request.Currency = string.IsNullOrEmpty((string)Session["CurrencyCode"]) ? "0" : (string)Session["CurrencyCode"];

        request.PointRequired = int.Parse(System.Web.HttpContext.Current.Session["pointsRequired"].ToString());
        request.Quantity = int.Parse(tbQuantity.Text.Trim());
        request.Name = tbRName.Text.Trim();
        request.ContactNumber = tbContact.Text.Trim();
        request.Address = tbAddress.Value.Trim();
        request.PostalCode = tbPostal.Text.Trim();
        request.City = tbCity.Text.Trim();
        request.Country = tbCountry.Text.Trim();

        return request;
    }

    private RedemptionOnlineRequest BuildRedemptionOnlineRequest()
    {
        var request = new RedemptionOnlineRequest();

        request.OperatorId = commonVariables.OperatorId;
        request.MemberCode = string.IsNullOrEmpty((string)Session["MemberCode"]) ? "" : (string)Session["MemberCode"];
        request.ProductId = lblproductid.Value;
        request.CategoryId = int.Parse(string.IsNullOrEmpty((string)Session["categoryId"]) ? "" : (string)Session["categoryId"]);
        request.RiskId = string.IsNullOrEmpty((string)Session["RiskId"]) ? "0" : (string)Session["RiskId"];
        request.Currency = string.IsNullOrEmpty((string)Session["CurrencyCode"]) ? "0" : (string)Session["CurrencyCode"];

        request.PointRequired = int.Parse(System.Web.HttpContext.Current.Session["pointsRequired"].ToString());
        request.Quantity = int.Parse(tbQuantity.Text.Trim());
        request.AimId = tbAccount.Text.Trim();
        return request;
    }

    private bool Valid()
    {
        var productType = (ProductTypeEnum)int.Parse(System.Web.HttpContext.Current.Session["productType"].ToString());
       
        switch (productType)
        {
            case ProductTypeEnum.Freebet:
                return true;
            case ProductTypeEnum.Normal:
                {
                    string name = tbRName.Text.Trim();
                    string contact = tbContact.Text.Trim();
                    string address = tbAddress.Value.Trim();
                    string postal = tbPostal.Text.Trim();
                    string city = tbCity.Text.Trim();
                    string country = tbCountry.Text.Trim();

                    if (name.Equals(""))
                    {
                        return false;
                    }
                    else if (contact.Equals(""))
                    {
                        return false;
                    }
                    else if (address.Equals(""))
                    {
                        return false;
                    }
                    else if (postal.Equals(""))
                    {
                        return false;
                    }
                    else if (city.Equals(""))
                    {
                        return false;
                    }
                    else if (country.Equals(""))
                    {
                        return false;
                    }
                    else
                        return true;

                    return false;
                }
            case ProductTypeEnum.Wishlist:
                {
                    return false;
                }
            case ProductTypeEnum.Online:
                {
                    string aimId = tbAccount.Text.Trim();

                    if (aimId.Equals(""))
                    {
                        return false;
                    }

                    return true;
                }
        }

        return false;
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





