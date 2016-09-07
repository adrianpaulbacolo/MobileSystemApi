using System;
using audit_trail_manager;

namespace W88.Utilities.Log.Helpers
{
    public class AuditTrail
    {
        private static audit_trail_manager.log _audit;
        private static string auditLog = Common.GetAppSetting<string>("AuditLog");
        private static  string auditLogFolder = Common.GetAppSetting<string>("AuditLogFolder");

        //log audit trail into text file
        public static void AppendLog(string userName, string pageName, string taskName, string componentName,
            string processCode, string processDetail, string processErrorCode, string processErrorDetail,
            string processRemark, string processSerialId, string processId, bool IsSystemError)
        {
            bool log = false;

            if (string.Compare(auditLog, "yes", true) == 0) { log = true; }
            else { if (IsSystemError) { log = true; } }

            if (log)
            {
                _audit = new log
                {
                    log_file_path = auditLogFolder,
                    user_name = userName,
                    page_name = pageName + "_" + System.DateTime.Now.ToString("yyyy-MM-dd HH00"),
                    task_name = taskName,
                    component_name = componentName,
                    process_code = processCode,
                    process_detail = processDetail,
                    process_error_code = processErrorCode,
                    process_error_detail = processErrorDetail,
                    process_remark = processRemark,
                    process_serial_id = processSerialId,
                    process_id = processId
                };

                _audit.inserting();
            }
        }

        public static void AppendLog(Exception e)
        {
            if (!CheckIfLog(true)) return;

            _audit = new log
            {
                log_file_path = auditLogFolder,
                page_name = string.Format("{0}_{1}", e.TargetSite, DateTime.Now.ToString("yyyy-MM-dd HH00")),
                process_error_detail = e.InnerException.ToString(),
                process_remark = e.Message
            };

            _audit.inserting();
        }

        private static bool CheckIfLog(bool isSystemError)
        {
            return string.Compare(auditLog, "yes", true) == 0 || isSystemError;
        }
    }
}
