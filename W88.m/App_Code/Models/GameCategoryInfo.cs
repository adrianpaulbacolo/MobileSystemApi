using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class GameCategoryInfo
    {
        public string Title { get; set; }
     
        public List<GameInfo> New { get; set; }
        public List<GameInfo> Current { get; set; }

        public string Provider { get; set; }
    }
}