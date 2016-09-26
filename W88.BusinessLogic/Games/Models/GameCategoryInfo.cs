using System.Collections.Generic;

namespace W88.BusinessLogic.Games.Models
{
    public class GameCategoryInfo
    {
        public GameCategoryInfo()
        {
            New = new List<GameInfo>();
            Current = new List<GameInfo>();
        }

        public string Provider { get; set; }
        public string Title { get; set; }
        public List<GameInfo> New { get; set; }
        public List<GameInfo> Current { get; set; }
    }
}