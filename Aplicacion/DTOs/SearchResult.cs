using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.DTOs
{
    public class SearchResult<T> where T : class
    {
        public int PageIndex { get; set; }
        public int PageCount { get; set; }
        public int? TotalItems { get; set; }
        public int? ItemCount { get; set; }
        public List<T>? Items { get; set; }

        public Dictionary<string, int> GetPagedCollectionAttributes()
        {
            return new Dictionary<string, int> { { "PageCount", PageCount }, { "PageIndex", PageIndex } };
        }

        public string? ValidationErrorMessage { get; set; }
    }
}

