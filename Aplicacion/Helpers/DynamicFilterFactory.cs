﻿using Aplicacion.DTOs;
using Dominio.Core;

namespace Aplicacion.Helpers
{
    public static class DynamicFilterFactory
    {
        public static DynamicFilter CreateDynamicFilter(QueryInfo queryInfo)
        {
            var newQueryInfo = new QueryInfo();

            if (queryInfo != null)
            {
                newQueryInfo.Includes = queryInfo.Includes;
                newQueryInfo.SortFields = queryInfo.SortFields;
                newQueryInfo.Ascending = queryInfo.Ascending;
                newQueryInfo.Predicate = queryInfo.Predicate;
                newQueryInfo.ParamValues = CreateParam(queryInfo.ParamValues);

                if (queryInfo.PageIndex >= 0) newQueryInfo.PageIndex = queryInfo.PageIndex;
                if (queryInfo.PageSize > 0) newQueryInfo.PageSize = queryInfo.PageSize;
            }

            return new DynamicFilter(newQueryInfo.PageIndex, newQueryInfo.PageSize, newQueryInfo.SortFields,
                newQueryInfo.Ascending, newQueryInfo.Includes, newQueryInfo.Predicate, newQueryInfo.ParamValues);
        }
        private static object[] CreateParam(object[]? paramValues)
        {
            object[]? param = new object[paramValues.Length];
            for (int i = 0; i < paramValues.Length; i++)
            {
                param[i] = paramValues[i].ToString();
            }

            return param;
        }
    }
}
