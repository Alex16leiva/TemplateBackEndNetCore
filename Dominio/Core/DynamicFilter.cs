namespace Dominio.Core
{
    public class DynamicFilter
    {
        public DynamicFilter()
        {
        }

        public DynamicFilter(int pageIndex, int pageSize, List<string> sortFields = null, bool ascending = true,
            List<string> includes = null, string predicate = null, object[] paramValues = null)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Ascending = ascending;
            SortFields = sortFields;
            Filtro = predicate;
            Valores = paramValues;
            Includes = includes;
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public List<string> SortFields { get; set; }
        public bool Ascending { get; set; }
        public string Filtro { get; set; }
        public object[] Valores { get; set; }
        public List<string> Includes { get; set; }
    }
}
