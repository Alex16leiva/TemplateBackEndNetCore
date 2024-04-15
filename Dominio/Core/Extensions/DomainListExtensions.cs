namespace Dominio.Core.Extensions
{
    public static class DomainListExtensions
    {
        public static List<IEqualityKey> NotIn(this IEnumerable<IEqualityKey> newList, IEnumerable<IEqualityKey> comparerList)
        {
            var comparerKeys = comparerList.Items().Select(c => c.GetEqualityKey()).ToList();

            return newList.Items().Where(c => !comparerKeys.Contains(c.GetEqualityKey())).ToList();
        }
    }

    public interface IEqualityKey
    {
        string GetEqualityKey();
    }
}
