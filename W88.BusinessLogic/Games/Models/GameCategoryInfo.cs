using System.Collections.Generic;

namespace W88.BusinessLogic.Games.Models
{
    public class GameCategoryInfo
    {

        public string Provider { get; set; }

        public string Title { get; set; }

        public List<GameInfo> Games { get; set; }
    }
}