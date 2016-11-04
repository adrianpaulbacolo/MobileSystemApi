using System.ComponentModel.DataAnnotations;

namespace W88.BusinessLogic.Rewards.Models
{
    public class SearchInfo
    {
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string CategoryId { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Index { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string PageSize { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string SearchText { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string SortBy { get; set; }
        
        public int MinPoints { get; set; }

        public int MaxPoints { get; set; }
    }
}
