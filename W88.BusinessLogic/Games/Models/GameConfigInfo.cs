using System.Collections.Generic;
namespace W88.BusinessLogic.Games.Models
{
    public class GameConfigInfo
    {
        public GameConfigInfo()
        {
            Categories = new List<string>();
        }

        public List<string> Categories { get; set; }
        /// <summary>
        /// No of items to display, if 0 - take all items
        /// </summary>
        public int TotalItemCount { get; set; }

        /// <summary>
        /// Club Landing Page
        /// </summary>
        public string Lobby { get; set; }

        /// <summary>
        /// Cashier Landing Page
        /// </summary>
        public string Cashier { get; set; }

        /// <summary>
        /// Live Chat Landing Page
        /// </summary>
        public string LiveChat { get; set; }

        /// <summary>
        /// Logout Landing Page
        /// </summary>
        public string Logout { get; set; }

        /// <summary>
        /// ANDROID, IOS or WP
        /// </summary>
        public string Device { get; set; }
    }
}