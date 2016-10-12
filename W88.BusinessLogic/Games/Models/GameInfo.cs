namespace W88.BusinessLogic.Games.Models
{
    public class GameInfo
    {
        public string Title { get; set; }
        public string Image { get; set; }
        public string ImagePath { get; set; }
        public string FunUrl { get; set; }
        public string RealUrl { get; set; }
    }

    public class GameLinkInfo
    {
        public string Fun { get; set; }

        public string Real { get; set; }

        public string LobbyPage { get; set; }

        public string MemberSessionId { get; set; }

        public string CashierPage { get; set; }
    }

}