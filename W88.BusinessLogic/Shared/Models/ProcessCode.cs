using System;

namespace W88.BusinessLogic.Shared.Models
{
    public class ProcessCode 
    {
        public Guid Id;

        public dynamic Data = string.Empty;

        public int ProcessSerialId = 0;

        public bool IsAbort = false;

        public int Code = 0;

        public dynamic Message = string.Empty;

        public string Remark = string.Empty;

        public bool IsSuccess = false;
    }
}
