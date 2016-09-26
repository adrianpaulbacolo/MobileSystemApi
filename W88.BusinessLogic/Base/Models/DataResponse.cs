using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W88.BusinessLogic.Base.Models
{
    public class DataResponse
    {
        public dynamic ResponseData { get; set; }

        public int ResponseCode { get; set; }

        public dynamic ResponseMessage { get; set; }

    }
}
