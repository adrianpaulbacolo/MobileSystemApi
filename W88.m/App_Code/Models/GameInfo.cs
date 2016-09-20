using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Factories.Slots;

namespace Models
{
    public class GameInfo
    {
        public string Title { get; set; }
        public string Image { get; set; }
        public string FunUrl { get; set; }
        public string RealUrl { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }

        public GameProvider Provider { get; set; }
    }
}