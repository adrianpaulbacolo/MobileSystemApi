using System.Collections.Generic;

namespace W88.BusinessLogic.Games.Models
{
    public class GameCategoryInfo
    {
        public string Title { get; set; }
        public List<GameInfo> New { get; set; }
        public List<GameInfo> Current { get; set; }
    }
}