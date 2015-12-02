using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class commonAuditTrail
{
    private static string auditLog = System.Configuration.ConfigurationManager.AppSettings.Get("AuditLog");
    private static string auditLogFolder = System.Configuration.ConfigurationManager.AppSettings.Get("AuditLogFolder");

    //log audit trail into text file
    public static void appendLog(string userName, string pageName, string taskName, string componentName,
        string processCode, string processDetail, string processErrorCode, string processErrorDetail,
        string processRemark, string processSerialId, string processId, bool IsSystemError)
    {
        bool log = false;

        if (string.Compare(auditLog, "yes", true) == 0) { log = true; }
        else { if (IsSystemError) { log = true; } }

        if (log)
        {
            audit_trail_manager.log audit = new audit_trail_manager.log();

            audit.log_file_path = auditLogFolder;
            audit.user_name = userName;
            audit.page_name = pageName + "_" + System.DateTime.Now.ToString("yyyy-MM-dd HH00");
            audit.task_name = taskName;
            audit.component_name = componentName;
            audit.process_code = processCode;
            audit.process_detail = processDetail;
            audit.process_error_code = string.IsNullOrEmpty(processErrorCode) ? processCode : processErrorCode;
            audit.process_error_detail = string.IsNullOrEmpty(processErrorDetail) ? processDetail : processErrorDetail;
            audit.process_remark = processRemark;
            audit.process_serial_id = processSerialId;
            audit.process_id = processId;

            audit.inserting();

            audit = null;
        }
    }
}