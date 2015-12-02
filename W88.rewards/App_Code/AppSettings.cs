using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Configuration;

public static class AppSettings
{
	//------------------------------------------------------------
	public static long operator_id = long.Parse(ConfigurationManager.AppSettings["operator_id"].ToString());
	//------------------------------------------------------------
	public static string log_path = ConfigurationManager.AppSettings["log_path"];
	//------------------------------------------------------------
	public static bool enable_HTTPS = Boolean.Parse(ConfigurationManager.AppSettings["enable_HTTPS"]);
	public static bool enable_log_error = Boolean.Parse(ConfigurationManager.AppSettings["enable_log_error"]);
	public static bool enable_location_log = Boolean.Parse(ConfigurationManager.AppSettings["enable_location_log"]);
	public static bool enable_log = Boolean.Parse(ConfigurationManager.AppSettings["enable_log"]);
	//------------------------------------------------------------
	public static bool enable_accessXForwardedFor = Boolean.Parse(ConfigurationManager.AppSettings["enable_accessXForwardedFor"]);
	public static string list_block_country = ConfigurationManager.AppSettings["list_block_country"];
	//------------------------------------------------------------
	public static string list_demo_domain = ConfigurationManager.AppSettings["list_demo_domain"];
	//------------------------------------------------------------
	public static string list_domain = ConfigurationManager.AppSettings["list_domain"];
	public static string list_https_domain = ConfigurationManager.AppSettings["list_https_domain"];
	public static string list_force_https_domain = ConfigurationManager.AppSettings["list_force_https_domain"];
	
	public static string sub_domain = ConfigurationManager.AppSettings["sub_domain"];
	public static string sub_domain_account = ConfigurationManager.AppSettings["sub_domain_account"];
	public static string sub_domain_affiliates = ConfigurationManager.AppSettings["sub_domain_affiliates"];
	public static string sub_domain_fund = ConfigurationManager.AppSettings["sub_domain_fund"];
	public static string sub_domain_info = ConfigurationManager.AppSettings["sub_domain_info"];
	public static string sub_domain_payment = ConfigurationManager.AppSettings["sub_domain_payment"];
	public static string sub_domain_rewards = ConfigurationManager.AppSettings["sub_domain_rewards"];
	public static string sub_domain_www = ConfigurationManager.AppSettings["sub_domain_www"];

	public static string sub_domain_lottery = ConfigurationManager.AppSettings["sub_domain_lottery"];
	public static string sub_domain_binary = ConfigurationManager.AppSettings["sub_domain_binary"];
	//------------------------------------------------------------
	public static string list_language_code = ConfigurationManager.AppSettings["list_language_code"];
	public static string list_language_translation = ConfigurationManager.AppSettings["list_language_translation"];
	public static string list_language_abbreviation = ConfigurationManager.AppSettings["list_language_abbreviation"];
	//------------------------------------------------------------
	public static bool enable_zombie_check = Boolean.Parse(ConfigurationManager.AppSettings["enable_zombie_check"]);
	public static string str_zombie_agent = ConfigurationManager.AppSettings["str_zombie_agent"];
	public static int max_count_ddos_hits = int.Parse(ConfigurationManager.AppSettings["max_count_ddos_hits"]);
	public static int interval_ddos_hits = int.Parse(ConfigurationManager.AppSettings["interval_ddos_hits"]);
	public static int blocked_interval_ddos_hits = int.Parse(ConfigurationManager.AppSettings["blocked_interval_ddos_hits"]);
    //------------------------------------------------------------
    public static string helpdesk_email = ConfigurationManager.AppSettings["helpdesk_email"].ToString();
    //------------------------------------------------------------
	public static string https_exception_path = ConfigurationManager.AppSettings["https_exception_path"];
    //------------------------------------------------------------
    public static string palazzo_kick_user = ConfigurationManager.AppSettings["palazzo_kick_user"];

}