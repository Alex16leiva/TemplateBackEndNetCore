using System.Collections;

namespace Dominio.Core
{
    public sealed class PagedCollection
    {
        /// <summary>
        /// Create a new entities paginated set.
        /// </summary>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="entities">The entities in the page.</param>
        /// <param name="totalItems">Total items avaibale.</param>
        /// <param name="count">The total amount of entities in the page.</param>
        public PagedCollection(int pageIndex, int pageSize, IEnumerable entities, int totalItems, int count)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Items = entities;
            TotalItems = totalItems;
            ItemCount = count;
            PageCount = pageSize > 0 ? (int)Math.Ceiling(totalItems / (decimal)pageSize) : 0;
        }

        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
        public IEnumerable Items { get; private set; }
        public int TotalItems { get; private set; }
        public int PageCount { get; private set; }
        public int ItemCount { get; private set; }
    }
}
