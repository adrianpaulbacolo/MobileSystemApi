using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace x
{
public static class DaddyPay
{
    public string companyId { get; set; }
    public string bankId { get; set; }
    public string amount { get; set; }
    public string companyOrderNum { get; set; }
    public string companyUser { get; set; }
    public string key { get; set; }
    public string estimatedPaymentBank { get; set; }
    public string depositMode { get; set; }
    public string groupId { get; set; }
    public string webUrl { get; set; }
    public string memo { get; set; }
    public string note { get; set; }
    public string noteModel { get; set; }

    public DaddyPay()
    {
        this.companyId = string.Empty;
        this.bankId = string.Empty;
        this.amount = string.Empty;
        this.companyOrderNum = string.Empty;
        this.companyUser = string.Empty;
        this.key = string.Empty;
        this.estimatedPaymentBank = string.Empty;
        this.depositMode = string.Empty;
        this.groupId = string.Empty;
        this.webUrl = string.Empty;
        this.memo = string.Empty;
        this.note = string.Empty;
        this.noteModel = string.Empty;
    }
}
}
