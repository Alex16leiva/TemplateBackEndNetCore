using System.Text;

namespace Aplicacion.DTOs
{
    public sealed class QueryInfo
    {
        public QueryInfo()
        {
            PageIndex = 0;
            PageSize = 10;
            SortFields = new List<string>();
            CustomFilters = new Dictionary<string, object>();
        }

        /// <summary>
        /// The page index define in the query.
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// The page size define for the query.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// The list of fields to be sorted in the query.
        /// </summary>
        public List<string> SortFields { get; set; }

        /// <summary>
        /// Indicates if the query will sort the result in ascending order.
        /// </summary>
        public bool Ascending { get; set; }

        /// <summary>
        /// A custom filter to apply to the query.
        /// </summary>
        public string Predicate { get; set; }

        /// <summary>
        /// The parameters that will be applied to the query.
        /// </summary>
        public object[] ParamValues { get; set; }

        /// <summary>
        /// The names of tables to be included in the query, this is used to eagerly load those tables and avoid to scan the table.
        /// </summary>
        public List<string> Includes { get; set; }

        /// <summary>
        /// The Custom Query Operation to Perform.
        /// </summary>
        public string CustomQueryOperation { get; set; }

        public Dictionary<string, object> CustomFilters { get; set; }

        /// <summary>
        /// Gets the Uniform Resource Name that identifies resources.
        /// </summary>
        /// <returns>The Uniform Resource Name for the query request.</returns>
        public string GetUrn()
        {
            var sortFields = string.Join("|", SortFields.ToArray());
            string paramValues = GetParamValues(ParamValues);

            var urn = string.Format("{0}-{1}-{2}-{3}-{4}-{5}", Predicate,
                                    paramValues, PageIndex, PageSize,
                                    Ascending, sortFields);

            return urn;
        }

        private string GetParamValues(object[] paramValues)
        {
            if (paramValues != null)
            {
                var valuesStringBuild = new StringBuilder();

                foreach (var paramValue in paramValues)
                {
                    valuesStringBuild.Append(paramValue);
                }

                return valuesStringBuild.ToString();
            }

            return string.Empty;
        }


    }
}
