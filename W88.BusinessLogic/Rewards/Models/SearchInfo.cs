
namespace W88.BusinessLogic.Rewards.Models
{
    public class SearchInfo
    {
        private string _categoryId;

        private string _index;

        private string _pageSize;

        private string _searchText;

        private string _sortBy;

        public string CategoryId
        {
            get { return _categoryId; }
            set { _categoryId = string.IsNullOrEmpty(value) ? string.Empty : value; }
        }

        public string Index
        {
            get { return _index; }
            set { _index = string.IsNullOrEmpty(value) ? string.Empty : value; }
        }

        public string PageSize
        {
            get { return _pageSize; }
            set { _pageSize = string.IsNullOrEmpty(value) ? string.Empty : value; }
        }

        public string SearchText
        {
            get { return _searchText; } 
            set { _searchText = string.IsNullOrEmpty(value) ? string.Empty : value; }
        }

        public string SortBy
        {
            get { return _sortBy; }
            set { _sortBy = string.IsNullOrEmpty(value) ? string.Empty : value; }
        }

        public int MinPoints { get; set; }

        public int MaxPoints { get; set; }
    }
}
