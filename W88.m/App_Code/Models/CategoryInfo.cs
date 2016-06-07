using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class CategoryInfo
    {
        public string Title { get; set; }
        public List<GameInfo> Games { get; set; }
    }
}