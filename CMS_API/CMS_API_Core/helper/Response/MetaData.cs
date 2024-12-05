namespace CMS_API_Core.helper.Response
{
    public class MetaData<T>
    {
        public T Filters { get; set; }
        public string SortBy { get; set; }
        public string SortOrder { get; set; }
        public int page { get; set; }
        public int PerPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public MetaData()
        {

        }

        public MetaData(int _page, int _PerPage, int _TotalItems, T _Filters, string _SortBy = "name", string _SortOrder = "ase")
        {
            SortBy = _SortBy;
            SortOrder = _SortOrder;
            page = _page;
            PerPage = _PerPage;
            TotalPages = (int)Math.Ceiling((double)_TotalItems / _PerPage);
            TotalItems = _TotalItems;
            Filters = _Filters;

        }

    }
}
