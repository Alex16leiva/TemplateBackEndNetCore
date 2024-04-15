using System.Reflection;

namespace Dominio.Core.Extensions
{
    public static class ReflectionManager
    {
        public static T GetPropValue<T>(this object obj, string name)
        {
            object retval = GetPropValue(obj, name);
            if (retval == null) { return default(T); }

            return (T)retval;
        }

        public static void SetPropValue<T>(this object obj, string name, object value)
        {
            foreach (String part in name.Split('.'))
            {
                if (obj == null) { return; }

                Type type = obj.GetType();
                PropertyInfo info = type.GetProperty(part);
                if (info == null) { return; }

                info.SetValue(obj, (T)value, null);
            }
        }

        private static object GetPropValue(this object obj, string name)
        {
            foreach (string part in name.Split('.'))
            {
                if (obj == null) { return null; }

                Type type = obj.GetType();
                PropertyInfo info = type.GetProperty(part);
                if (info == null) { return null; }

                obj = info.GetValue(obj, null);
            }
            return obj;
        }
    }
}
